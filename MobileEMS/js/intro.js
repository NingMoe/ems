$(function() {
    var curFeedbackPage = 0,
        curQuestionPage = 1,
        curTab = 0,
        isQuestionLoad = false,
        hdnClassID = parseInt($("#hdnClassID").val()),
        hdnTemplateID = parseInt($("#hdnTemplateID").val()),
        hdFeedbackCount = parseInt($("#hdnFeedbackCount").val()),
		questionarea=$(".questionarea li"),
		questionType=parseInt($("#QuestionType").val()),
        canLoadMore = true,
        isFeedLoad = false;

    initDataBoxH();
    initTab();

    questionarea.click(function(){
		if(questionType==2){
			$(this).toggleClass("optionselect");
		}
		else{
			$(this).siblings().removeClass("optionselect");
			$(this).addClass("optionselect");
		}
		alert(GetAnswer());
		});
		
	var IsAnswer=function(){
		var answerLength=questionarea.filter(".optionselect").length;
		if(answerLength<1 || (questionType==2 && answerLength<2)){
			return false;
		}
		else{
			return true;
		}
	};
		
	var GetAnswer = function(){
		var answer='';
		var questionAnswer=questionarea.filter(".optionselect");
		questionAnswer.each(function(index, element) {
			if(answer==''){ answer=$(this).attr("data-value")}
			else{
				answer=answer+$(this).attr("data-value");
			}
			});
		return answer;
	};
	
	
    if ($("#div_demoLesson").length) {
        $(".classImg").click(function(e) {
            $(".classImg").hide();
            $("#div_demoLesson").show();
            var video = $("#div_demoLesson video")[0];
            video.play();

            if (typeof(ga_demo_play) != "undefined") {
                ga_demo_play();
            }

        });
    }

    $(".filter a").on(click, function() {
        tabSwitch($(this));
    });

    function initTab() {
        var url = location.href,
            i = url.indexOf("#"),
            src = url.substring(i + 1, url.length),
            obj = null;

        (i < 0 || src.indexOf("a6") !== -1) && (src = "detail");

        obj = $(".filter a[data-panel='" + src + "']");

        tabSwitch(obj);
    }

    function initDataBoxH() {
        var $jDataBox = $(".j-data-box"),
            deviceH = $(window).height()-180;

        $jDataBox.css("min-height", deviceH);
    }

    function tabSwitch(obj) {
        var $this = obj,
            tabName = $this.attr("data-panel"),
            $filter = $this.parent(".filter"),
            $tabPanel = $("#j-" + tabName + "-data"),
            $loadingBox = $(".loading_box"),
            $btnOpenClass = $(".btn_openclass"),
            $feedBackList = $("#feedBack-list li"),
            $filterBox = $(".filter_box");

        HJMTabSwitch.TabClick(tabName);
        if (tabName == "questions" && !isQuestionLoad && (isQuestionLoad = true)) {
            !$loadingBox.length && $tabPanel.prepend('<div class="loading_box"><i class="icon_loading"></i></div>');
            getQuestions(hdnClassID, curQuestionPage, 10);
        } else if (tabName == "classList" && !$("#j-classList-data dd").length) {
            !$loadingBox.length && $tabPanel.prepend('<div class="loading_box"><i class="icon_loading"></i></div>');
            getLessonList(hdnClassID);
        } else if (tabName == "detail" && !$feedBackList.length) {
            //!$loadingBox.length && $tabPanel.prepend('<div class="loading_box"><i class="icon_loading"></i></div>');
           // getfeedBack(hdnTemplateID, 1, 1);
        }


        if ($filter.hasClass("fixed")) {
            document.body.scrollTop = $filterBox.offset().top;
        }
    }

    $("#btnMoreFeedBack").on(click, function() {
        $("#feedBack-list").html("");
        getfeedBack(hdnTemplateID, curFeedbackPage, 10);
    });

    $(".link-cang").on(click, function() {
        var $this = $(this),
            isLogin = $("#hdnIsLogin").val(),
            url = location.href;

        if (isLogin === "False") {
            location.href = "/Login.aspx?url=" + url;
            return;
        }

        if ($this.hasClass("canged")) {
            cancelFavorite($this, hdnClassID);
        } else {
            addFavorite($this, hdnClassID);
        }
    });

    var startY,
        touchend = ("ontouchend" in window) ? "touchend" : "touchcancel";

    $("#j-questions-data").on({
        "touchstart": function(e) {
            var touches = e.touches;

            if (canLoadMore) {
                if (touches.length == 1) {
                    var touch = touches[0];
                    startY = touch.pageY;
                }
            }
        },
        "touchmove": function(e) {
            var touches = e.touches || e.originalEvent.targetTouches,
                touch = touches[0];
            move = touch.pageY - startY;
            if (canLoadMore) {
                if (document.body.scrollTop >= $(".btnQuiz").offset().top - $(window).height()) {
                    if (move <= 10) {
                        canLoadMore = false;
                        getQuestions(hdnClassID, curQuestionPage, 10);
                    }
                }
            }
        }
    });

    function getQuestions(cateId, id, styleId) {
        $.wsajax(
            "/course/intro.aspx",
            "GetQuestions", {
                CateID: CateId,
                ID: id,
                StyleID: styleId
            },
            function(data) {
                var html,
                    $btnQuiz = $("#j-questions-data .btnQuiz");

                $(".loading_box").remove();
                if (data) {
                    html = getTemplate(JSON.parse(data), "questions-tmpl");
                    $btnQuiz.before(html);
                    curQuestionPage++;
                    canLoadMore = true;
                } else {
                    curQuestionPage == 1 && $btnQuiz.before('<p class="txt_c" style="margin-top:40px;"><img src="/images/blank_tip.gif" width=50% /></p>');
                    canLoadMore = false;
                }
            }
        );
    }

    $(document).scroll(function() {
        var scrollTop = document.body.scrollTop,
            $filter = $(".filter"),
            $classListDt = $("#j-classList-data dt"),
            filerTop = $(".filter_box").offset().top;

        if (scrollTop >= filerTop) {
            $filter.addClass("fixed");
        } else {
            $filter.removeClass("fixed");
        }

        $classListDt.each(function(i) {
            var $this = $(this);

            if (scrollTop > $this.offset().top) {
                $classListDt.removeClass("fixed");
                $this.addClass("fixed");
            } else {
                $this.removeClass("fixed");
            }
        });
    });

    function getfeedBack(templateId, page, pageSize) {
        if (isFeedLoad) {
            return;
        }
        isFeedLoad = true;

        $.wsajax(
            "/course/intro.aspx",
            "BestFeedback", {
                templateId: templateId,
                page: page,
                pageSize: pageSize
            },
            function(data) {
                var html,
                    $feedBackBox = $("#feedBack-list");
                var parseResult;

                $(".loading_box").remove();
                if (data) {
                    parseResult = JSON.parse(data);
                    html = getTemplate(parseResult, "feedback-tmpl");
                    $feedBackBox.append(html);
                    curFeedbackPage++;

                    //if (parseResult.length < pageSize) {
                    //    $('#btnMoreFeedBack').hide();
                    //}
                    var pageCount = Math.ceil(hdFeedbackCount / pageSize);
                    if (curFeedbackPage > pageCount) {
                        $('#btnMoreFeedBack').hide();
                    }

                } else {
                    $(".message-item").hide();
                }
                isFeedLoad = false;
            }
        );
    }

    function getLessonList(classID) {
        $.wsajax(
            "/course/intro.aspx",
            "GetLessonUnit", {
                classID: classID
            },
            function(data) {
                var html,
                    $lessonBox = $("#j-classList-data");

                if (data) {
                    html = getTemplate(JSON.parse(data), "lesson-tmpl");
                    $lessonBox.html(html);
                } else {
                    $lessonBox.html("<p style='text-align:center;margin-top:30px;'>该课程暂未发布~</p>");
                }
            }
        );
    }

    $(document).scroll(function() {
        var scrollTop = document.body.scrollTop,
            $filter = $(".filter"),
            $classListDt = $("#j-classList-data dt"),
            $classList = $("#j-classList-data"),
            $btnOpenclass = $(".btn_openclass"),
            $classPriceBox = $(".btn_openclass .class-price-box"),
            filerTop = $(".filter_box").offset().top,
            infoItemTop = $(".info-item").offset().top - parseInt($filter.height());
        filerTop = $(".filter_box").offset().top;

        if (scrollTop >= filerTop) {
            $filter.addClass("fixed");
        } else {
            $filter.removeClass("fixed");
        }


        if ($classList.css("display") === "block") {
            $classListDt.each(function(i) {
                var $this = $(this);

                if (scrollTop > $this.offset().top) {
                    $classListDt.removeClass("fixed");
                    $this.addClass("fixed");
                } else {
                    $this.removeClass("fixed");
                }
            });
        }
    });

    $(".course-detail-item").on("click", function() {
        location.href = "/" + hdnClassID + "/detail";
    });

    $(".btnUnSign").click(function() {
        $(".msgMode").hide();
    });

    var VideoPlay = function() {};
    VideoPlay.prototype = {
        init: function() {
            this.initEvent();
        },
        initEvent: function() {
            $(".class-img").on("click", function() {
                $(this).find(".play").hide();
                // $(".video")[0].style.width = $(window).width() + "px";
                // $(".video")[0].style.height = $(window).height() + "px";
                $(".video").show();
            });

            // $(".video").on("webkitfullscreenchange", function () {
            //     if (document.webkitIsFullScreen === false) {
            //         $(".class-img .play").show();
            //         $(".video").hide();
            //     }
            // });
        }
    };
    var myVideoPlay = new VideoPlay();
    myVideoPlay.init();

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

$(".message-item").delegate("dt", "click", function() {
    var $this = $(this),
        $parent = $this.parent(),
        $dd = $parent.find("dd");

    if ($parent.hasClass("slideDown")) {
        document.body.scrollTop = $this.offset().top - parseInt($(".filter").height());
    }
    $dd.toggle();
    $parent.toggleClass("slideDown");
});

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