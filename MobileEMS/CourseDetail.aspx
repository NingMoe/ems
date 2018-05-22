<%@ Page Language="C#" MasterPageFile="~/Course.Master" AutoEventWireup="true" CodeBehind="CourseDetail.aspx.cs" Inherits="MobileEMS.CourseDetail" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="css/intro.css" type="text/css">
    <div id="ctl00_left_nav_pnlLast">
        <a href="#" class="link-back" id="Previous"><i class="icon-back"></i></a>
    </div>
    <div id="ctl00_left_nav_pnlUserCenter">
        <a href="javascript:;" class="link-myinfo">
        <i id="userImgHead"><img src="images/31959858.jpg" width="23" height="23"></i></a>
    </div>
    <asp:Label ID="CenterTitle" runat="server" Text="Label"></asp:Label>
    <!--<a href="Cart.aspx" class="link-cart"><i class="icon-cart"></i></a>-->
    <a href="Search.aspx?Action=" class="link-search"><i class="icon-search-s"></i></a>
    <!--<a href="javascript:;" class="link-menu"><i class="icon-menu"></i></a>-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
<div class="btn_openclass">
        <a href="http://wpa.qq.com/msgrd?v=3&uin=800052251&site=qq&menu=yes" class="link-onlinekf" target="_blank"><i class="icon-onlinekf"></i><br>在线客服</a>
        <a href="tel:02150620208" class="link-phonekf"><i class="icon-phonekf"></i><br>电话咨询</a>
       <div class="btn_open_box">
            <a href="javascript:addToCart(<%= product.ID.ToString() %>,'<%= product.Name %>',<%= product.MarketPrice %>,'0');" class="btn btnGreen btnBuy" id="btnOpen">立即购买</a>
        </div> <!---->
    </div>
    <div class="wrapper">
        <div class="class-img"> 
             <%--<div id="TestCateImg">
                <img src="0af17f21-6d62-4aab-9ad3-ca99271e4b3a.png" class="videl-bg">
             </div>--%>
            <a href="javascript:;" class="link-cang"><i class="icon-star-white" style="margin-right: 5px;"></i>收藏</a>
        </div>
        <div class="filter_box">
            <nav class="filter clearfix">
                <div class="navMark" style="width: 474.609375px; -webkit-transition: 0.302s linear; transition: 0.302s linear; -webkit-transform: translate(0px, 0);"></div>
                <a href="javascript:;" data-panel="detail" class="cur">简介</a>
                <%--<a href="javascript:;" data-panel="questions" class="">Q&amp;A</a>
                <a href="javascript:;" data-panel="classList" onclick="SendEvent(25, 273,159017)" class="">大纲</a>--%>
            </nav>
        </div>
        <div class="j-data-box">
            <div id="j-detail-data" class="j-data">
                <div class="class-price-info">
                    <h2 class="class-title"><%= product.Name%></h2>
                    
                    <div>

                        <span class="class-price">￥<%= product.MarketPrice%> </span>

                        <%--<span class="red_block">8.4折</span><span class="blue_block">送书</span>--%>

                    </div>
                    
                    <%--<p class="origin_price"><span class="line_throu">￥4658.0</span></p>--%>
                    
                </div>
                <figure class="info-item">
                   
                   <%= product.Introduction%>
                    
                </figure>
            </div>
            <!-- 课程大纲 -->
            <%--<div id="j-classList-data" class="j-data course_list hide" style="display: none;">
            </div>--%>
            <!-- Q&A -->
            <%--<div id="j-questions-data" class="j-data hide" style="display: none;">
                <a href="#" class="btnQuiz" onclick="SendEvent(25, 278,159017)">我要咨询</a>
            </div>--%>
        </div>

        <input type="hidden" id="hdnClassID" value="159017">
        <input type="hidden" id="hdnPosition" value="intro页">
        <input type="hidden" id="hdnTemplateID" value="1268">
        <input type="hidden" id="hdnIsLogin" value="True">
        <input type="hidden" id="courseImg" value="">
        <input type="hidden" id="hdnFeedbackCount" value="7">
        <input type="hidden" id="hdnSubTitle" value="">
    </div>
    <a id="returnTop" href="javascript:scroll(0,0)" style="display: none;"><i class="icon-arrow-t"></i></a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
<script src="js/course.js?v=01.005" type="text/javascript"></script>
<script src="js/intro.js" type="text/javascript"></script>
<script src="js/cart.js?v=01.005" type="text/javascript"></script>

</asp:Content>