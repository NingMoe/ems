$(document).ready(function(){
    var productID = $("#ProductID").val(),
    ajaxFlag = false,
    $obj = $("#QuestionMenu .bd li:eq(0)");

 	function GetQuestion(id){
        ajaxFlag = true;
        var $currentStyle= $obj.parent(),
        styleID = $currentStyle.data("style"),
        styleName = $("#QuestionMenu .hd li[data-id='"+styleID+"']").text();
        $.ajax({
            url: "Ajax.aspx?Action=GetQuestion&ProductID=" + productID + "&StyleID=" + styleID + "&ID=" + id
        })
        .done(function(data) {
            if(data != "") {
                data = "<div class='heading'><span>" + styleName + "</span></div>" + data;
                $(".testcontent").html(data);
                $obj.addClass('on').unbind("click");
            }
        })
        .fail(function() {
            layer.msg("网络中断，请稍候再试...");
        })
        .always(function() {
            ajaxFlag = false;
        });
        
		// $.get("Ajax.aspx?Action=GetQuestion&ProductID=" + productID + "&StyleID=" + styleID + "&ID=" + id, function(data){
  //           ajaxFlag = false;
  //           if(data != "") {
  //               data = "<div class='heading'><span>" + styleName + "</span></div>" + data;
  //               $(".testcontent").html(data);
  //               $obj.addClass('on').unbind("click");
  //           }
  //           // else{
  //           //     $obj.removeClass('on');
  //           // }          
		// });
	}

    $("#QuestionMenu .bd li").click(function(event) {
        if ($(".testcontent").text() == "" || GetAnswer() != "") {
            if (!ajaxFlag) {
                //$obj = $(this).addClass('on');
                $obj = $(this);
                GetQuestion($(this).text());
                var $hdobj = $("#QuestionMenu .hd li[data-id='"+$(this).parent().data("style")+"']");
                if(!$hdobj.hasClass('on')) {
                    $hdobj.click();
                }
            }
            else{
                layer.msg("正在拼命的数据请求，请稍候...");
            }
        }
        else {
            layer.msg("请选择答案");
        }
    });

    // 算法一(注意死循环)
    // var FilterNextObj = function(parentObj){
    //     var $parentObj = $(parentObj); console.log("1111");
    //     if($parentObj.next().length > 0) {
    //         $parentObj = $parentObj.next();
    //     }
    //     else {
    //         $parentObj = $parentObj.siblings().first();
    //     }
    //     var $nextObj= $parentObj.children("li:not(.on)");
    //     if($nextObj.length <= 0) {
    //         return FilterNextObj($parentObj);
    //     }
    //     else {
    //         return $nextObj;
    //     }
    // }

    $(".nextquestion").click(function() {
        var $nextObj = $obj.nextAll("li:not(.on)");
        if (!$obj.hasClass('on')) {
            $nextObj = $obj;
        }
        if($nextObj.length <= 0){
            //$nextObj = FilterNextObj($obj.parent());
            var $parentObj = $obj.parent();
            $nextObj = $parentObj.next();
            if($nextObj.length > 0) {
                $nextObj = $nextObj.children("li:not(.on)");
            }
            else {
                $nextObj = $parentObj.parent().find('li:not(.on)');
            }
            if($nextObj.length <= 0) {
                $(".complete").click();
                return false;
            }
        }
        $nextObj.eq(0).click();
    });

    $(".complete").click(function(event) {
        var $noSelectQuestion = $("#QuestionMenu .bd li:not(.on)");
        var tipsMsg = "";
        if ($noSelectQuestion.length>0) {
            tipsMsg = "还有" + $noSelectQuestion.length + "题未查看，也可以点选左侧题号进行浏览。";
        }
        layer.confirm(tipsMsg + '确定要交卷吗？', {
            btn: ['交卷','取消'], //按钮
            skin: 'layui-layer-molv', //样式类名
        }, function(){
            HandInTestPaper();
        });
    });

    //交卷
    var HandInTestPaper = function(){
        $.ajax({
            url: "Ajax.aspx?Action=HandInTestPaper",
            type: "POST",
            data: {ProductID : productID},
            async: false,
            success: function(data) {
                if(data != "") {
                    clearInterval(sh);
                    $(".operation a").unbind('click');
                    $(window).unbind('beforeunload');
                    //iframe层
                    layer.open({
                        type: 1,
                        title: '考试结果', //按钮
                        skin: 'layui-layer-molv',
                        area: ['550px', '330px'],
                        content: data, //iframe的url
                        btn: ['关 闭'],
                        closeBtn: 0, 
                        yes: function(index, layero){
                            layer.closeAll();
                            window.close();
                        }
                    }); 
                }
                else {
                    //提示层
                    layer.msg('交卷不成功！');
                }
            }
        });
    }

    //获取题目答案
    var GetAnswer = function() {
        var checkValue = "";
        var $answerOption = $(".option input");
        if($answerOption.length > 0) {
            $answerOption.each(function(index, el) {
                if ($(this).prop("checked") == true){
                    checkValue += $(this).val();
                }
            });
        }
        return checkValue;
    }

    $(".testcontent").on('click', '.option input', function(event) {
        var checkValue = GetAnswer();
        if (checkValue != "") {
            var $timu = $(".testcontent .timu");
            $.post("Ajax.aspx?Action=AnswerSet", 
                {
                    ProductID : productID,
                    StyleID : $timu.data("style"),
                    Answer : checkValue,
                    QuestionID : $timu.data("id")
                }, 
                function(data) {
            });
        }
    });

    // $(window).bind('beforeunload',function(event) {
    //     layer.msg('交卷中...');
    //     HandInTestPaper();
    // });

    //默认载入
    _fresh();
    var sh = setInterval(_fresh, 1000); //考试时间
	$obj.click();

    // function _fresh()
    // {
    //     var leftsecond = document.getElementById("TestTime").value;
    //     document.getElementById("TestTime").value = leftsecond - 1;
    //     __d = parseInt(leftsecond / 3600 / 24);
    //     __h = parseInt((leftsecond / 3600) % 24);
    //     __m = parseInt((leftsecond / 60));
    //     __s = parseInt(leftsecond % 60);
    //     if(__s < 10) {
    //         document.getElementById("times").innerHTML = __m + ":0" + __s;
    //     }
    //     else {
    //         document.getElementById("times").innerHTML = __m + ":" + __s;
    //     }
    //     if(leftsecond <= 0){
    //         document.getElementById("times").innerHTML = "已结束";
    //         HandInTestPaper();
    //         clearInterval(sh);
    //     }
    // }

    function _fresh()
    {
        var leftsecond = document.getElementById("TestTime").value;
        leftsecond = leftsecond - 1;
        document.getElementById("TestTime").value = leftsecond;
        document.getElementById("times").innerHTML = _TimeShow(leftsecond);
        if(leftsecond <= 0){
            document.getElementById("times").innerHTML = "已结束";
            HandInTestPaper();
            clearInterval(sh);
        }
    }

    function _TimeShow(leftsecond)
    {
        var result = "";
        __d = parseInt(leftsecond / 3600 / 24);
        __h = parseInt((leftsecond / 3600) % 24);
        __m = parseInt((leftsecond / 60));
        __s = parseInt(leftsecond % 60);
        if(__s < 10) {
            result = __m + ":0" + __s;
        }
        else {
            result = __m + ":" + __s;
        }
        return result;
    }

    var pauseMaxTime = 300;
    function _pausefresh()
    {
        pauseMaxTime = pauseMaxTime - 1;
        document.getElementById("pausetimeshow").innerHTML = _TimeShow(pauseMaxTime);
        if(pauseMaxTime <= 0){
            clearInterval(pauseTime);
            layer.closeAll(); 
            sh = setInterval(_fresh, 1000);
            if(pauseNum == 0) $("#PauseTest").hide(500);
        }
    }

    $("#PauseTest").click(function(event) {
        pause();
        pauseMaxTime = 300;
        pauseTime = setInterval(_pausefresh, 1000);
    });

    var pauseNum = 2;
    function pause() {
        if(pauseNum > 0) {
            pauseNum = pauseNum - 1;
            clearInterval(sh);
            layer.alert('考试将暂停 [<span id="pausetimeshow">5:00</span>]，现在要继续吗？（还有' + (pauseNum) + '次暂停机会）', {
                title: '暂停提示',
                btn: ['继续考试'],
                skin: 'layui-layer-molv', //样式类名
                closeBtn: 0
            }, function(index){
                layer.close(index);
                clearInterval(pauseTime);
                sh = setInterval(_fresh, 1000);
                if(pauseNum == 0) $("#PauseTest").hide(500);
            });
        }
    }
});
//屏蔽按键
document.onkeydown = function(e){
    e = window.event || e;
    var keycode = e.keyCode || e.which;
    if(e.ctrlKey || e.altKey || e.shiftKey|| keycode >= 112 && keycode <= 123){
    if(window.event){// ie
        try{ e.keyCode = 0; }catch(e) {}
        e.returnValue = false;
    } else {// ff
        e.preventDefault();
        }
    }
}
//屏蔽右键
document.oncontextmenu = function(e){
    return false;
}

document.body.onselectstart = function(e){ 
    return false;
}