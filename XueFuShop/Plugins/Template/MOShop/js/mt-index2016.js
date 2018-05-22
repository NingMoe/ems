/**
 * mt index 2016
 */

function goPageTop(){$("html,body").animate({scrollTop:0},600)}
$(document).ready(function(){
    $(window).scroll(function(){
        var a=location.href,
        b=$(this).scrollTop();
        b>800?$(".gotop-nav").show():$(".gotop-nav").hide()
    }),
    $(".right-nav ul li").hover(function(){var a=$(this).attr("id");null!=a&&"tel"==a?$(this).find(".sidebox").stop().animate({width:"174px"},200).css({background:"#d30830"}):$(this).find(".sidebox").stop().animate({width:"124px"},200).css({background:"#d30830"})},function(){$(this).find(".sidebox").hasClass("qq")?$(this).find(".sidebox").stop().animate({width:"54px"},200).css({background:"#d30830"}):$(this).find(".sidebox").stop().animate({width:"54px"},200).css({background:"#000"})});
});

$(document).ready(function(){
    curPage = $("#currentPage").text();
    $("#page").val(curPage);

    ClientWidth = document.body.clientWidth || document.documentElement.clientWidth;
    if(ClientWidth >= 1200){
        $(window).scroll(function () {
            var a = $(this).scrollTop(),
                b = $(".top-bar");
            a > 0 ? b.addClass("top-bar-fixed") : b.removeClass("top-bar-fixed");
        });
    }
});

$.fn.extend({
    Scroll: function(opt, callback) {
        //参数初始化
        if (!opt) var opt = {};
        var _this = this.eq(0).find("ul:first");
        var lineH = _this.find("li:first").height(), //获取行高
            line = opt.line ? parseInt(opt.line, 10) : parseInt(this.height() / lineH, 10), //每次滚动的行数，默认为一屏，即父容器高度
            speed = opt.speed ? parseInt(opt.speed, 10) : 500, //卷动速度，数值越大，速度越慢（毫秒）
            timer = opt.timer ? parseInt(opt.timer, 10) : 3000; //滚动的时间间隔（毫秒）

        if (line == 0) line = 1;
        var upHeight = 0 - line * lineH;
        //滚动函数
        scrollUp = function() {
            _this.animate({
                marginTop: upHeight
            }, speed, function() {
                for (i = 1; i <= line; i++) {
                    _this.find("li:first").appendTo(_this);
                }
                _this.css({
                    marginTop: 0
                });
            });
        }
        //鼠标事件绑定
        _this.hover(function() {
            clearInterval(timerID);
        }, function() {
            timerID = setInterval("scrollUp()", timer);
        }).mouseout();
    }
});

(function(global, $, undefined){
    'use strict';

    var slideItemLength = $('#topSlide .item').length;
    var time1212Start = new Date('2015','10','25','13','00','00').getTime();
    var time1212End = new Date('2015','11','12','23','59','59').getTime();
    var timeNow = new Date().getTime();
    var timeIn1212 = (timeNow>time1212Start)&&(timeNow<time1212End);
    var bannerPicUrl,bannerPicHeight;
    
    if((slideItemLength===1)){
        bannerPicHeight = 600;
        $('.top-slider,.slider-unit').css('height','600px');
        $('#topSlide .r-cont').css('display','none');
        bannerPicUrl = $('#topSlide .r-cont a').attr('href');
        $('#topSlide .item').css('cursor','pointer').click(function(){
            window.open(bannerPicUrl);
        });

        $('#topSlide .item').click(function(){
            ht.sendCustomEvent('topsliderClick');
        });
    }

    $.fn.hoverDelay = function (options) {
        var defaults = {
            hoverDuring: 200,
            outDuring: 200,
            hoverEvent: function () {
                $.noop();
            },
            outEvent: function () {
                $.noop();
            }
        };
        var sets = $.extend(defaults, options || {}),
            hoverTimer, outTimer;
        return $(this).each(function () {
            $(this).hover(function () {
                clearTimeout(outTimer);
                hoverTimer = setTimeout(sets.hoverEvent, sets.hoverDuring);
            }, function () {
                clearTimeout(hoverTimer);
                outTimer = setTimeout(sets.outEvent, sets.outDuring);
            });
        });
    };


    var $doc = $(document),
        $win = $(window),

        $searchText = $('#textSearch'),
        $search = $('#searchDiv'),
        $topSlide = $('#topSlide'),
        $topBlock = $('#topBlock'),
        $backTop = $('.backTop'),
        $topbar = $('#topBar'),

        shortVideoState = $('#shortVideoInput').val(),

        UA = navigator.userAgent.toLowerCase(),

        slider = null;



    var core = {
        init: function(){
            this._bindEvent();
            this._initSendEvent();

            if(shortVideoState==='show'){
                this._headShortVideo.init();
            }
            else{
                if($('.top-slider').css('display')==='none'){
                    $('.headShortVideoOpen').css('display','block');
                }
            }
        },
        _bindAfterSlider: function(){
            $win.on('scroll.afterSlider', function(){
                var h = $win.scrollTop();
                if(h >= (bannerPicHeight||600)){
                    $topbar.addClass('fixTop top-shadow');
                    // $backTop.show();
                    slider.stopAuto();
                }else{
                    $topbar.removeClass('fixTop top-shadow');
                    // $backTop.hide();
                    if(slideItemLength>1){
                        slider.startAuto();
                    }
                    else{
                        $('.bx-controls').css('display','none');
                    }
                }
            });

            $win.on('scroll', function(){
                var h = $win.scrollTop();
                if(h >= 400){
                    $backTop.show();
                }else{
                    $backTop.hide();
                }
            });
        },
        _bindEvent: function(){

            $doc.on('click','#btnSearch', function(){
                var keyword = $searchText.text();
                //window.location.href = "/Product.aspx?Keyword=" + keyword
            });

            $doc.on('keydown','#textSearch', function(event) {
                if (event.keyCode == 13){
                    var keyword = $searchText.text();
                    //window.location.href = "/Product.aspx?Keyword=" + keyword
                }
            });

            /*$doc.on('click', '.main .b-block', function(event){
                var $this = $(this),
                    url = $this.attr('data-url');

                window.open(url);
                event.stopPropagation();
            });*/

            $doc.on('click', '#login_out', function () {
                var passname = 'pass',
                    hostname = location.hostname;
                if(/local|qa|beta|dev|2/.test(hostname)){
                    passname = 'qapass';
                }
                else if(/yz/.test(hostname)){
                    passname = 'yzpass';
                }

                //window.location.href = 'https://' + passname + '.hujiang.com/uc/handler/logout.ashx?returnurl=' + window.location.href;
            });
            $doc.on('click', function(event){
                var $this = $(event.target),
                    $parent = $this.closest('.search'),
                    isSearch = $parent.length > 0 || $this.hasClass('search');
                if(isSearch){
                    $search.addClass('search-on');
                    $searchText.focus();
                }else{
                    $search.removeClass('search-on');
                }
            });

            $search.hover(function(){
                $search.addClass('search-on');
            }); 

            var $secNav = $('.sec-nav'),
                $zxli = $('.nav_zx'),
                $secBg = $('.wrapmask');

            //determine PC event and ipad event, sorry for so naive before
            //
            /*if(/ipad/.test(UA)){
                $('.nav_zx, .wrapmask').on('click', function(){
                    $(this).toggleClass('arrow_on');
                    $(this).toggle();
                    $secBg.toggle();
                });
                $('.user').on('click', function(){
                    $(this).toggleClass('user_on');
                });

            }else{*/
                $('.nav_zx').hover(function(){
                    $(this).addClass('arrow_on');
                    $(this).children(".sec-nav").show();
                    $secBg.stop().show();
                },function(){
                    $(this).removeClass('arrow_on');
                    $(this).children(".sec-nav").hide();
                    $secBg.stop().hide();
                });
                $('.user').on('mouseenter mouseleave', function(event){
                    if(event.type === 'mouseenter'){
                        $(this).addClass('user_on');
                    }else{
                        $(this).removeClass('user_on');
                    }
                });
            /*}*/

            // $doc.on('mouseenter mouseleave', '.sec-nav', function(event){
                
            //     if(event.type === 'mouseenter'){
            //         $zxli.addClass('arrow_on');
            //         $secNav.stop().show();
            //         $secBg.stop().show();
            //     }else{
            //         $zxli.removeClass('arrow_on');
            //         $secNav.stop().hide();
            //         $secBg.stop().hide();
            //     }
            // });

            $doc.on('mouseenter','.bx-pager-item',function(){
                $(this).find('a').addClass('pager_hover');
            });

            $doc.on('mouseout','.bx-pager-item',function(){
                $(this).find('a').removeClass('pager_hover');
            });

            $doc.on('click', '#closeSlide', function(){
                Cookies.set('HJ_slide_A', $('#sildeNewestUrl').val(), { expires: 90 });
                $topBlock.hide();
                $win.off('scroll.afterSlider');
                $topbar.addClass('fixTop top-shadow');
                $('.headShortVideoOpen').css('display','block');

                ht.sendCustomEvent('topsliderClose');
            });

            $('#topBar .headShortVideoOpen').click(function(){
                if($('.headShortVideoBox').length===0){
                    $('.top-slider').show();
                    ht.sendCustomEvent('topsliderOpen');
                    Cookies.remove('HJ_slide_A');
                    $topbar.removeClass('fixTop top-shadow');
                    $('.headShortVideoOpen').css('display','none');

                    if(!slider){
                        slider = $topSlide.bxSlider({
                            mode: 'fade',
                            pager: true,
                            controls: false,
                            speed: 800,
                            randomStart: true,
                            pause: 6000
                        });

                        

                        if(slideItemLength>1){
                            slider.startAuto();
                        }
                        else{
                            $('.bx-controls').css('display','none');
                        }
                    }
                    core._bindAfterSlider();
                }
            });
        },
        encodeURI: function(e) {
            return escape(e).replace(/\*/g, "%2A").replace(/\+/g, "%2B").replace(/-/g, "%2D").replace(/\./g, "%2E").replace(/\//g, "%2F").replace(/@/g, "%40").replace(/_/g, "%5F").replace(/%/g, "_");
        },
        htmlEncode: function(str) {
            var div = document.createElement('div'),
                text = document.createTextNode(str);
            div.appendChild(text);
            return div.innerHTML;
        },
        searchWord: function () {
            var text = core.htmlEncode($.trim($searchText.val()));
            if(text === ''){
                return false;
            }
            var url = 'http://www.hjenglish.com/new/search/' + text + '/';
            window.SendEvent(38, 877, '{"keyword":"' + text + '"}');
            window.open(url, '_blank');
        },

        _initSendEvent: function(){
            $('.htLinks a').on('click', function(){
                var $this = $(this),
                    $parent = $this.parent(),
                    planID = parseInt($parent.attr('data-planID')),
                    eventID = parseInt($parent.attr('data-eventID')),
                    text = $this.text(),
                    href = $this.attr('href');
                window.SendEvent(planID, eventID, '{"label":"' + text + '","url":"' + href + '"}');
            });
        },
        _headShortVideo:{
            init:function(){
                this.firstRender();
                this.bindEvent();
            },
            firstRender:function(){
                var shortVideoSrc = $('.headShortVideoBox .shortvideo').attr('src');
                if(Cookies.get('HJShortVideo')==='close'+shortVideoSrc){
                    $('.headShortVideoBox').css('display','none');
                    $('.shortvideo').get('0').pause();
                    $('#topBar .headShortVideoOpen').css('display','block');
                }
                else{
                    this.playShortVideo();
                }
            },
            bindEvent:function(){
                var _this = this;
                
                $('.headShortVideoButton1').click(function(){
                    ht.sendEvent(38,875,{type: 'video', url: $('.longvideo').attr('src')});
                    _this.playLongVideo();
                });

                this.fullScreenChange();

                $('.headShortVideoClose').click(function(){
                    _this.removeShortVideo();
                    _this.topBarFix();
                });

                $('#topBar .headShortVideoOpen').click(function(){
                    _this.playShortVideo();
                    _this.topBarFix();
                });

                $('.longvideo').on('ended', function(){
                    var longvideo = $('.longvideo').get(0);
                    _this.exitFullScreen(longvideo);
                });
            },
            playShortVideo:function(){
                var shortvideoSrc = $('.shortvideo').attr('src');
                $('.headShortVideoBox').css('display','block');

                $('.shortvideo').canPlayThrough(shortvideoSrc).done(function(){
                    $('.shortvideo').css('display','block');
                    $('.shortvideo').get('0').play();
                });
                $('.headShortVideoOpen').css('display','none');
                Cookies.remove('HJShortVideo');
            },
            removeShortVideo:function(){
                $('.headShortVideoBox').css('display','none');
                $('.shortvideo').css('display','none');
                $('.shortvideo').get('0').pause();
                $('.headShortVideoOpen').css('display','block');
                var shortVideoSrc = $('.headShortVideoBox .shortvideo').attr('src');
                Cookies.set('HJShortVideo', 'close'+shortVideoSrc, { expires: 90 });
            },
            playLongVideo:function(){
                var longVideo = $('.longvideo').get(0);
                $('.hideVideoBox').css('display','block');
                this.launchFullScreen(longVideo);
                longVideo.volume=0.6;
                longVideo.play();
            },
            launchFullScreen:function(element) {
                if(element.requestFullscreen) {
                    element.requestFullscreen();
                    $('.shortvideo').get(0).pause();
                }
                else if(element.webkitRequestFullscreen) {
                    element.webkitRequestFullscreen();
                    $('.shortvideo').get(0).pause();
                }
                else if(element.mozRequestFullScreen) {
                    element.mozRequestFullScreen();
                    $('.shortvideo').get(0).pause();
                }
            },
            exitFullScreen:function(element){
                if(element.exitFullScreen) {
                    element.exitFullScreen();
                }
                else if(element.webkitExitFullScreen) {
                    element.webkitExitFullScreen();
                }
                else if(document.mozCancelFullScreen) {
                    document.mozCancelFullScreen();
                }
            },
            fullScreenChange:function(){
                //true 全屏
                document.addEventListener('fullscreenchange',function(){
                    if(document.fullScreen===false||document.webkitIsFullScreen===false){
                        $('.hideVideoBox').css('display','none');
                        $('.longvideo').get(0).pause();
                        $('.shortvideo').get(0).play();
                    }
                });
                document.addEventListener('webkitfullscreenchange',function(){
                    if(document.webkitIsFullScreen===false){
                        $('.hideVideoBox').css('display','none');
                        $('.longvideo').get(0).pause();
                        $('.shortvideo').get(0).play();
                    }
                });
                document.addEventListener('mozfullscreenchange',function(){
                    if(document.mozFullScreen===false){
                        $('.hideVideoBox').css('display','none');
                        $('.longvideo').get(0).pause();
                        $('.shortvideo').get(0).play();
                    }
                });
            },
            topBarFix:function(){
                var that = this;
                that.topBarFixRender();

                $win.on('scroll', function(){
                    that.topBarFixRender();
                });
            },
            topBarFixRender:function(){
                var headShortVideoBox = $('.headShortVideoBox');
                var h = $win.scrollTop(),
                        videoHeight;

                if(headShortVideoBox.css('display')=='none'){
                    videoHeight =0;
                }
                else{
                    videoHeight = headShortVideoBox.height();
                }

                if(h > videoHeight){
                    $topbar.addClass('fixTop top-shadow');
                    $backTop.show();
                }else{
                    $topbar.removeClass('fixTop top-shadow');
                    $backTop.hide();
                }
            }
        }
    };

    core.init();

})(this, window.jQuery);