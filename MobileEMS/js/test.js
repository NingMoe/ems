$(function() {
    var curFeedbackPage = 0,
        curQuestionPage = 1,
        curTab = 0,
		cateId = $('#hidCateID').val(),
		$loadMore = $('#loadMore'),
        isQuestionLoad = false,
        hdnClassID = parseInt($("#hdnClassID").val()),
        hdnTemplateID = parseInt($("#hdnTemplateID").val()),
        questionarea = $(".questionarea"),
		questionoption=$(".questionarea li"),
		questionType=0,
        canLoadMore = true,
        isFeedLoad = false;
    
    questionarea.delegate("li", "click", function() {
		if(questionType==2){
			$(this).toggleClass("optionselect");
		}
		else{
			$(this).siblings().removeClass("optionselect");
			$(this).addClass("optionselect");
		}
		if(IsAnswer()){
			AnswerSet(cateId,questionarea.attr("data-questionid"),questionarea.attr("data-style"),GetAnswer())
		}
		});
		
	var IsAnswer=function(){
		var answerLength=questionarea.find(".optionselect").length;
		if(questionarea.find("li").length>0){
			if(answerLength<1 || (questionType==2 && answerLength<2)){
				return false;
			}
			else{
				return true;
			}
		}else{
			return true;
		}
	};
		
	var GetAnswer = function(){
		var answer='';
		var questionAnswer=questionarea.find(".optionselect");
		questionAnswer.each(function(index, element) {
			if(answer==''){ answer=$(this).attr("data-value")}
			else{
				answer=answer+$(this).attr("data-value");
			}
		});
		return answer;
	};
	
   var GetQuestionId = function(){
	   if(!IsAnswer()){
		    GetTips('请选择答案！');
			return false;
	   }
	   var currentQuestion= $('#courseNavList span.currentquestion'),	   
	   IsAutoFindId=true;
	   
	   if(currentQuestion.length>0){
	       currentQuestion.removeClass("currentquestion").addClass("finishquestion");
		   currentQuestion=currentQuestion.next().not(".finishquestion");
		   if(currentQuestion.length>0) IsAutoFindId=false;
	   }
	   
	   var otherQuestion=$('#courseNavList span.sub_cate:not(.finishquestion)');
	   if(IsAutoFindId){
		   currentQuestion=otherQuestion.first();
	   }
	   
	   if(currentQuestion.length>0){
		   getQuestions(currentQuestion,cateId, currentQuestion.attr("data-id"), currentQuestion.attr("data-styleid"));	
		   //currentQuestion.addClass("currentquestion");
		   if(otherQuestion.length<=1){
			   //$(".prequestion").unbind("click");
			   //$(".nextquestion").unbind("click");
			   $(".nextquestion").css({"background-color":"#CCC","cursor":"none"});
			   //GetTips('已经是最后一道了！');
		   }
	   }else{
		   GetTips('已经是最后一道了！');
		   $(".nextquestion").unbind("click");
	   }
   }
   
   var GetTips = function(content){
	   var toast = '<div class="msgMode"><div class="msgMode_con"><p style="text-align:center;color:#fff;">'+content+'</div></div>';
	   $("body").append(toast);
	   setTimeout(function() {
		   $(".msgMode").hide();
		   }, 1500);
   }
   
   var starttime = new Date();	//页面载入后初始化开始时间
   var testTimeLength = parseInt($("#EndTimer").val());	//页面载入时记录考试时长
   var pauseTimeLength = 0;	//暂停总时长
   function getpassssecond(starttime){
	   var nowtime = new Date();
	   var passsecond = parseInt((nowtime.getTime() - starttime.getTime()) / 1000);
	   return passsecond;
   }
   
   function updatetime(){
	   var leftsecond = testTimeLength - getpassssecond(starttime);
	   leftsecond = leftsecond + pauseTimeLength;	//剩余时间应加上暂停的时长，并有5s的误差
	   if((leftsecond + 5) < $("#EndTimer").val()){
		   if(leftsecond < 0) leftsecond = 5;
		   $("#EndTimer").val(leftsecond);
	   }
	}

	// 暂停按钮点击事件
    $("#PauseTest").click(function(event) {
        pause();
    });

    var pauseNum = 2;	//暂停次数限制
    function pause() {
        if(pauseNum > 0) {
        	var pauseTimeStart = new Date();
            pauseNum = pauseNum - 1;
            clearInterval(sh);
            clearInterval(pauseTime);
            layer.open({
            	content: '考试暂停，现在要继续吗？（还有' + (pauseNum) + '次暂停机会）',
                btn: ['继续考试'],
  				shadeClose: false,
	            yes:function(index){
	            	pauseTimeLength = pauseTimeLength + getpassssecond(pauseTimeStart);	//统计暂停时间
	                sh = setInterval(_fresh, 1000);
	                updatetime();
	                pauseTime = setInterval(updatetime,10000);
	                layer.close(index);
	                if(pauseNum == 0) $("#PauseTest").hide(500);
	            }
        	});
        }
    }
	
	function _fresh(){	
		var leftsecond=$("#EndTimer").val();
		$("#EndTimer").val(leftsecond-1);
		__d=parseInt(leftsecond/3600/24);
		__h=parseInt((leftsecond/3600)%24);
		__m=parseInt((leftsecond/60));
		__s=parseInt(leftsecond%60);
		if(__s<10){
			$("#TimerShow").html(__m+":0"+__s);
		}else{
			$("#TimerShow").html(__m+":"+__s);
		}
		if(leftsecond<=0){			
			$("#TimerShow").html("已结束");
			SubmitTestPaper();
			clearInterval(sh);
		}
	}
	_fresh()
	var sh;  //倒计时
	var pauseTime;	//暂停
   
	$(".nextquestion").on("click",function(e) {GetQuestionId();});
	$('#courseNavList').delegate('.sub_cate', "click", function (e) {
		var $this = $(this),
		$currentQuestion= $('#courseNavList span.currentquestion');
		
	    if(!IsAnswer()){
		    GetTips('请选择答案！');
			$('#courseNav').html($currentQuestion.parent().prev("cateItem").text());
			return false;
	    }
		if($this.hasClass("finishquestion")) return false;
		$currentQuestion.removeClass("currentquestion").addClass("finishquestion");
		//$this.addClass("currentquestion");
		getQuestions($this,cateId, $this.attr("data-id"), $this.attr("data-styleid"));	
	});
	
	$(".questionarea").delegate("#teststart", "click", function() {
		//_fresh();
		GetQuestionId();
		clearInterval(sh);
		sh = setInterval(_fresh,1000); //倒计时
		pauseTime = setInterval(updatetime,10000);	//每十秒校验一次时间
		starttime = new Date(); //初始化开始时间
		//$(".prequestion").css({"background-color":"#CCC","cursor":"none"});
		$(".btn_openclass").show();
		$(".top_nav").show();
	});

	var AnswerSet=function(cateId, questionid, styleId, answer){
		var currentIndex = 0;
		//本地试题列表更新答案
		$.each(QuestionList,function(index, el) {
			if(el.QuestionId == questionid){
				el.Answer = answer;
				el["IsSuccess"] = false;
				currentIndex = index;
                return true;
			}
		});
		//网络更新答案
		$.wsajax(
		    "CourseAjax.aspx",
		    "AnswerSet", {
		        CateID: cateId,
		        QuestionID: questionid,
		        StyleID: styleId,
				Answer: answer
		    },
		    function(data) {
		    	data = JSON.parse(JSON.parse(data));
		    	if(data.Success){
					QuestionList[currentIndex]["IsSuccess"] = true;
		    	}
		    }
		);
	}
   
	$("#btnOpen").click(function(){
			var $noSelectQuestion = $('#courseNavList span.sub_cate:not(.finishquestion):not(.currentquestion)');
			var tipsMsg = "";
	    if ($noSelectQuestion.length>0) {
	        tipsMsg = "还有" + $noSelectQuestion.length + "题未查看，也可以点选题号进行浏览。";
	    }
			if(confirm(tipsMsg + "确定要交卷吗？")){SubmitTestPaper();}
	});

	var SubmitTestPaper=function(){
		var answerArray = new Array(),
		answerJson='';
		//重新整理答案
		$.each(QuestionList,function(index, el) {
			if(el.Answer.length>0 && !el["IsSuccess"]){
				answerArray.push('{"StyleID":'+el.Style+',"ID":'+el.QuestionId+',"Answer":"'+el.Answer+'"}');				
			}
		});
		if(answerArray.length>0){
			answerJson = "[" + answerArray.join() + "]";
		}

	    $.wsajax(
	        "CourseAjax.aspx",
	        "RecordTest", {
	            CateID: cateId,
	            Answer:answerJson
	        },
	        function(data) {
	            if (data) {
					var josndata=JSON.parse(JSON.parse(data));
					if(josndata.Success){
						location.href = josndata.Url;
					}else{
						//questionarea.html(html);
						alert('交卷失败！');
					}
	            }
	        }
	    );
	}

	function getQuestions(obj, cateId, id, styleId) {
		$.each(QuestionList,function(index, josndata) {
			if(josndata.CateId==id&&josndata.Style==styleId.toString()){

				obj.addClass("currentquestion");
                var html = getTemplate(josndata, "questions-tmpl");
                questionarea.html(html);
				questionarea.attr("data-style",josndata.Style);
				questionarea.attr("data-questionid",josndata.QuestionId);
				questionType=parseInt(josndata.Style);
				$("#courseNav").text($("#StyleTitle"+styleId).text());					
	            var questionNum=questionarea.attr("data-questionnum");
	            $(".classtitle").html("进度："+($('#courseNavList span.finishquestion').length+1)+"/"+questionNum);
				if(($(".top_nav").height()+$("section").height()+105)>$(window).height()){
					$('body').height($(".content").height()+115);
					document.body.scrollTop=60;
				}else{
					document.body.scrollTop=0;
				}
                canLoadMore = true;
                return true;
			}
		});
    }
   
    // function getQuestions(obj, cateId, id, styleId) {
    // 	if (!isQuestionLoad) {
    // 		isQuestionLoad = true;
	   //      $.wsajax(
	   //          "CourseAjax.aspx",
	   //          "GetQuestions", {
	   //              CateID: cateId,
	   //              ID: id,
	   //              StyleID: styleId
	   //          },
	   //          function(data) {
	   //              if (data) {
	   //              	obj.addClass("currentquestion");
				// 		var josndata=JSON.parse(data);
	   //                  var html = getTemplate(josndata, "questions-tmpl");
	   //                  questionarea.html(html);
				// 		questionarea.attr("data-style",josndata.Style);
				// 		questionarea.attr("data-questionid",josndata.QuestionId);
				// 		questionType=parseInt(josndata.Style);
				// 		$("#courseNav").text($("#StyleTitle"+styleId).text());					
			 //            var questionNum=questionarea.attr("data-questionnum");
			 //            $(".classtitle").html("进度："+($('#courseNavList span.finishquestion').length+1)+"/"+questionNum);
				// 		//var winHeight = $(window).height(),
				// 		//bodyHeight = $('body').height(),
				// 		//contentHeight=$(".content").height();
				// 		if(($(".top_nav").height()+$("section").height()+105)>$(window).height()){
				// 			$('body').height($(".content").height()+115);
				// 			document.body.scrollTop=60;
				// 		}else{
				// 			document.body.scrollTop=0;
				// 		}
	   //                  canLoadMore = true;
	   //              }
	   //          },
	   //          function(){
	   //          	isQuestionLoad = false;	            	
	   //          }
	   //      );
    //     }
    //     else{
    //     	GetTips("正在拼命的数据请求，请稍候...");
    //     }
    // }
    // 
    
});
function dealga(classId, goUrl, locat, from) {

    if (typeof(ga_exchange_click) != "undefined") {
        ga_exchange_click(locat, classId);
    }

    //已登录，跳转到确认购买页
    if (goUrl == 1) {
        setTimeout(function() {
            window.location.href = "/" + classId + "/exchange" + from;
        }, 200);
    }
}

window.onorientationchange = function() {
    var ua = navigator.userAgent,
        isAndroid = /android/i.test(ua);

    if (isAndroid) {
        setTimeout(function() {
            HJMTabSwitch.setNavMark($(".filter .cur").index() - 1);
        }, 300);
    } else {
        HJMTabSwitch.setNavMark($(".filter .cur").index() - 1);
    }
}