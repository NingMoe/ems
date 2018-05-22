$(window).scroll(function(){
   cur_scroll();

});
    var cur_scroll = function(){
         var _top=$(window).scrollTop()+100;
    var _win = $(window).height();//可视窗口高度
    
    // course_detail
    var total = 100+$('#intro-header').outerHeight(true)+$(".position").outerHeight(true);
    // var total = $(".intro-nav1").offset().top-98;
    console.log("total:"+total);
    console.log("scrollTop:"+_top);


    // console.log(_top>=total)
    if(_top>=total){
        $('.wrapper').addClass("mt100");
        $('.intro-siderbar').addClass("top177");
       $(".intro-nav1").hide();
       $(".intro-nav2").show();
    }else{
        console.log("123");
        $('.wrapper').removeClass("mt100");
        $('.intro-siderbar').removeClass("top177");
        $(".intro-nav1").show();
       $(".intro-nav2").hide();
    }

    var winH = $(window).height();//可视窗口高度
         var iTop = $(window).scrollTop();//滚动条到顶部滚动的距离
         
         //鼠标滑动式改变  
         var a_Array = new Array;
         for (var i = 0; i < aDiv.length; i++) {
                a_Array[i] = $(".intro-panel:eq("+i+")").offset().top - 153;
                
            }
        for (var i = 0; i < aDiv.length; i++) {
                // console.log(a_Array[i+1])
            if (i<aDiv.length-1) {
                if (iTop<=a_Array[0]) {
                    aNav.removeClass('cur');
                    aNav.eq(0).addClass('cur');
                }
                if (iTop>=a_Array[i] && iTop<a_Array[i+1]) {
                    aNav.removeClass('cur');
                    aNav.eq(i).addClass('cur');
                }
            }else{
                if (iTop>=a_Array[i]) {
                    aNav.removeClass('cur');
                    aNav.eq(i).addClass('cur');
                }
                }
            }
            //鼠标滑动式改变  
         var ba_Array = new Array;
         for (var bi = 0; bi < baDiv.length; bi++) {
                ba_Array[bi] = $(".consult-pic:eq("+bi+")").offset().top - 153;
                
            }
        for (var bi = 0; bi < baDiv.length; bi++) {
                // console.log(a_Array[i+1])
            if (bi<baDiv.length-1) {
                if (iTop<=ba_Array[0]) {
                    baNav.removeClass('cur');
                    baNav.eq(0).addClass('cur');
                }
                if (iTop>=ba_Array[bi] && iTop<ba_Array[bi+1]) {
                    baNav.removeClass('cur');
                    baNav.eq(bi).addClass('cur');
                }
            }else{
                if (iTop>=ba_Array[bi]) {
                    baNav.removeClass('cur');
                    baNav.eq(bi).addClass('cur');
                }
                }
            }



    }
$(function(){

    var _top=$(window).scrollTop();
    var _win = $(window).height();//可视窗口高度
    var total = $('#topBar').height()+$('#intro-header').outerHeight()+$(".position").height()-100;

    $('.nav-list .cur').click(function(){
        $('.detailboxo').css("display","block");
        $('.detailboxt').css("display","none");
        $('.intro-nav-main .nav-list a.cur').css({"background":"#18bd9c","color": "#fff"});
        $('.intro-nav-main .nav-list a.ask').css({"background":"#f5f5f5","color": "#494949"});
        $(window).scrollTop(total);
        $('.siderbar-detail-nav').children().eq(0).addClass("cur").siblings().removeClass("cur");
        cur_scroll();
    }); 
    $('.nav-list .ask').click(function(){
        $('.detailboxo').css("display","none");
        $('.detailboxt').css("display","block");
        $('.intro-nav-main .nav-list a.cur').css({"background":"#f5f5f5","color": "#494949"});
        $('.intro-nav-main .nav-list a.ask').css({"background":"#18bd9c","color": "#fff"});
        $(window).scrollTop(total);
        $('.siderbar-detail-nav').children().eq(0).addClass("cur").siblings().removeClass("cur");
        cur_scroll();
    });

});




  var oNav = $('.siderbar-consult-nav');//导航壳
   var aNav = oNav.find('a');//导航
   var aDiv = $('.nav-panel .intro-panel');//楼层



    //点击回到当前楼层
    aNav.click(function(){
        var t = aDiv.eq($(this).index()).offset().top;
        $('body,html').animate({"scrollTop":t-152},500);
        // $(this).addClass('cur').siblings().removeClass('cur');
    });


   var boNav = $('.siderbar-detail-nav');//导航壳

   var baNav = boNav.find('a');//导航
   var baDiv = $('.nav-panel .consult-pic');//楼层

    //点击回到当前楼层
    baNav.click(function(){
        var bt = baDiv.eq($(this).index()).offset().top;
        $('body,html').animate({"scrollTop":bt-152},500);
        $(this).addClass('cur').siblings().removeClass('cur');
    });
