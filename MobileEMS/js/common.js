(function($) {
    $.extend($, {
        wsajax: function(url, method, para, callback1, callback2) {

            $.ajax({
                url: url + "?Action=" + method,
                type: "post",
                //contentType: "application/json",para && JSON.stringify(para) || null
                data: para,
                //dataType: "json",
                success: function(data) {
                    callback1(data);//JSON.parse(data).d
                },
                error: function(xhr, type) {
					var $loadMore = $('#loadMore'),
					$loadingBox = $(".loading_box");
					
					if($loadingBox.length>0){
						$(".loading_box").remove();
					}
					
					if($loadMore.length>0){
						$loadMore.html('').hide();
					}
					
                    var toast = '<div class="msgMode"><div class="msgMode_con"><p style="text-align:center;color:#fff;">操作失败<br>请检查网络连接</div></div>';
                    $("body").append(toast);
                    setTimeout(function() {
                        $(".msgMode").hide();
                    }, 1000);
                },
                complete:function() {
                    if (typeof(callback2) == "function") {
                        callback2();
                    }
                }
            });
        }
    });
})(Zepto);

var canTouch = "ontouchstart" in window,
    click = canTouch ? "tap" : "click",
    cssPrefix = $.browser.cssPrefix,
    vendorPrefix = $.browser.vendorPrefix,
    transform = cssPrefix + "transform";

var getTemplate = function(obj, tmplId) {
    var tmpl = doT.template($("#" + tmplId).html());
    var templateFn = tmpl;
    var html = templateFn(obj);
    return html;
};

var HJMCommon = {
    setLocalStorage: function(key, data, expired) {
        if (!'localStorage' in window) {
            return false;
        }
        var record = {
            value: data,
            timestamp: expired ? new Date().getTime() + expired * 24 * 60 * 60 * 1000 : 0
        }
        localStorage.setItem(key, JSON.stringify(record));
        return data;
    },
    getLocalStorage: function(key) {
        if (!'localStorage' in window) {
            return false;
        }

        var record = JSON.parse(localStorage.getItem(key));
        if (!record) {
            return false;
        }
        return (record.timestamp == 0 || new Date().getTime() < record.timestamp) && (record.value);
    },
    initFooter: function() {
        if ($(".wrapper").length == 0) {
            return false;
        }
        var screenH = window.innerHeight - $("header").height();
        var height = $("footer").length ? $(".wrapper").height() + $("footer").height() : $(".wrapper").height();
        if ($(".class_list").length > 0) {
            height = $(".wrapper").height() + 162;
        }

        if (height < screenH) {
            $("footer").addClass("footer_b");
        } else {
            $("footer").removeClass("footer_b");
        }
    }
};

var HJMTabSwitch = {
    TabClick: function(tabName, callback) {
        var tabIndex = 0;
        $(".j-data").hide();
        tabIndex = $("a[data-panel=\"" + tabName + "\"]").index();
        $("a[data-panel=\"" + tabName + "\"]").parent().find("a").removeClass("cur");
        $("a[data-panel=\"" + tabName + "\"]").addClass("cur");

        var tabName = "j-" + tabName + "-data";
        $("#" + tabName).show();

        if (tabIndex != -1) {
            this.setNavMark(tabIndex - 1);
        }

        callback && callback(tabName);
    },
    setNavMark: function(index) {
        var $navMark = $(".navMark"),
            $navBtn = $navMark.parent("nav").find("a"),
            $curNavBtn = $navMark.parent("nav").find(".cur"),
            navWidth = $navBtn.width(),
            markPosition = 0,
            time = 0;
        if (index == null) {
            index = $curNavBtn.index() - 1;
        }
        markPosition = Math.round(navWidth * index), time = (navWidth - 300) * 0.3 + 250 >> 0;
        $navMark.css("width", navWidth + "px");
        this.animateTransform($navMark.get(0), "translate(" + markPosition + "px, 0)", time);
        $navMark.css(transform, "translate(" + markPosition + "px, 0)");
    },
    animateTransform: function(elem, transforms, duration, ease, complete) {
        if (duration == 0) {
            elem.style[vendorPrefix + "Transform"] = transforms;
            return;
        }
        var css = elem.style;
        css[vendorPrefix + "TransitionProperty"] = "transform";
        css[vendorPrefix + "TransitionDuration"] = duration / 1000 + "s";
        css[vendorPrefix + "TransitionTimingFunction"] = ease || "linear";
        setTimeout(function() {
            css[vendorPrefix + "Transform"] = transforms;
        }, 100);
        if (complete) {
            var type = $.browser.vendorPrefix == "Moz" ? "transitionend" : vendorPrefix + "TransitionEnd";
            elem.addEventListener(type, function(e) {
                elem.removeEventListener(e.type, arguments.callee);
                complete.call(elem);
            });
        }
    }
};


$(function() {
    //去除url控制条
    // setTimeout(scrollTo, 0, 0, 0);
    $("#returnTop").on("click", function() {
        setTimeout(function() {
            window.scrollTo(0, 0);
        }, 300);
    });

    var startX = 0,
        startY = 0,
        move = 0,
        isHide = false,
        clientHeight = $(window).height();

    window.onscroll = function() {
        var srollPos = document.documentElement.scrollTop || document.body.scrollTop;
        if (srollPos >= 100 && !isHide) {
            $("#returnTop").show();
        } else {
            $("#returnTop").hide();
        }
    };

    document.addEventListener("touchstart", function(e) {
        var touches = e.targetTouches,
            scrollY = window.scrollY;

        if (touches.length == 1) {
            var touch = touches[0];

            startX = touch.pageX;
            startY = touch.pageY;
        }
    });

    document.addEventListener("touchmove", function(e) {
        var touches = e.touches;
        var touch = touches[0];

        move = touch.pageY - startY;
        if (0 > move + 10 || 0 < move - 10) {
            isHide = true;
        }
    });

    var touchEndHandler = function() {
        isHide = false;
    };
    // touchEvents = ("ontouchend" in window) ? "touchend": "touchcancel";

    document.addEventListener("touchend", touchEndHandler);
    document.addEventListener("touchcancel", touchEndHandler);

});


$(function() {
    var cssPrefix = $.browser.cssPrefix,
        vendorPrefix = $.browser.vendorPrefix,
        transform = cssPrefix + "transform",
        canTouch = "ontouchstart" in window,
        userAgent = navigator.userAgent,
        click = canTouch ? "tap" : "click",
        rawclick = canTouch ? "touchend" : "click";

    setTimeout(function() {
        HJMCommon.initFooter();
    }, 500);

    if(userAgent.indexOf("iPad") !== -1){
        $("#kfTel").remove();
    }
    var $userCenter = $(".user_center"),
        $container = $("#container"),
        $linkMyInfo = $(".link-myinfo"),
        $rightMenu = $(".right_menu"),
        $linkMenu = $(".link-menu");

    $(document).on({    
        swipeLeft: function(){
            removeAnimation($userCenter, "moveToRight");
        },
        swipeRight: function(){
            removeAnimation($container, "moveToLeft");
        }
    });

    $linkMyInfo.on(click, function() {
        var $input = $("input");
        
        addAnimation($userCenter, "moveToRight");
        $input.length && $input.blur();
    });

    $linkMenu.on(click, function() {
        var $input = $("input");

        $rightMenu.css("display", "block");
        $input.length && $input.blur();
        window.scrollTo(0, 0);
        setTimeout(function(){
            addAnimation($container, "moveToLeft");
            $rightMenu.css("height", $(window).height());
        },450);
    });

    function addAnimation(obj, animation){
        addMask();
        obj.addClass(animation);
        $(document).bind("touchmove", preventDefault);
    }

    function preventDefault(e){
        e.preventDefault();
    }

    function removeAnimation(obj, animation){
        if(!obj.hasClass(animation)){
            return;
        }
        $(document).unbind("touchmove", preventDefault);
        obj.removeClass(animation);
        $(".headerMask").hide();
    }

    function addMask(){
        if($("#container .headerMask").length){
            $(".headerMask").show();
        }else{
            $("#container").append("<div class='headerMask'></div>");
        }
        if ($(".playContent div iframe").length > 0) {
            $(".playContent div iframe").hide();
        }
    }

    $(".headerMask").live(click, function(){
        setTimeout(function(){
            $container.hasClass("moveToLeft") && removeAnimation($container, "moveToLeft");
            $userCenter.hasClass("moveToRight") && removeAnimation($userCenter, "moveToRight");
        }, 400);
        if ($(".playContent div iframe").length > 0) {
            $(".playContent div iframe").show();
        }
    });


    $(".link-back").on(click, function() {
        $(this).addClass("hover");
    });

    //分类导航展示
    $(".type-ul>li").on(click, function() {
        $(".sub-nav").hide();
        $(".sub-nav." + $(this).attr("data-nav")).show();
        $(".type-ul>li").removeClass("on")
        $(this).addClass("on");
    });

    $(".link_top").on(click, function() {
        setTimeout("window.scrollTo(0,0)", 300);
        e.preventDefault();
    });

    $(".link_search").on(click, function() {
        setTimeout(function() {
            $(".menu-list").removeClass("show");
            $(".icon_menu").removeClass("hover");
            $(".search_form").show();
            $(".search_form").addClass("show");
        }, 500);
        // $(".search-box").focus();
    });

    $(".msgMode .icon_close").on("click", function() {
        var obj = $(this);
        setTimeout(function() {
            obj.parent(".msgMode").hide();
        }, 200);
    });

    $(".menu-list li em").on(click, function() {
        window.location.href = $(this).parent("a").attr("href");
    });

    //分类子导航点击
    $(".sub-nav li a").on("click", function() {
        $("#type-nav").removeClass("swapanim");
        $(".class_box_bg").removeClass("swapanim");
        var cate = $(this).attr("cate");
        var fcate = $(this).attr("fcate");
        $(".all-type").html($(this).html() + "<i class=\"arrow\"></i>");
        window.scrollTo(0, 0);
        //if (typeof (fcate) == "undefined") {
        //    asyncData.getDataByCate(cate);
        //}
        //else {
        //    asyncData.getDataByCate(cate, true);
        //}
        asyncData.getDataByCate(cate, !!fcate);
    });

    
});

// document.addEventListener('DOMContentLoaded', function() {
//     setTimeout(function() {
//         window.scrollTo(0, 1);
//     }, 100);
// }, false);

