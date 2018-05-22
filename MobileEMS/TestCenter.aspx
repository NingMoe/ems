<%@ Page Language="C#" MasterPageFile="~/Course.Master" AutoEventWireup="true" CodeBehind="TestCenter.aspx.cs" Inherits="MobileEMS.TestCenter" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="css/test.css?v=2017.04.19" type="text/css">
    <div id="ctl00_left_nav_pnlLast">		
        <a href="javascript:history.go(-1);" class="link-back" id="Previous"><i class="icon-back"></i></a>
    </div>
    <asp:Label ID="TestName" runat="server" Text=""></asp:Label>
    <!--<a href="javascript:;" class="link-menu"><i class="icon-menu"></i></a>-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="btn_openclass">
        <a href="javascript:void(0);" id="PauseTest" class="btn btnGreen prequestion">暂停</a>
        <a href="javascript:void(0);" class="btn btnGreen nextquestion">下一题</a>
        <div class="btn_open_box">
            <a href="#" class="" id="btnOpen">交 卷</a>
        </div>
    </div>
<div class="content">
		
<nav class="top_nav">
		<div class="nav">
			<div id="courseNav" class="first_nav">
				<i class="icon-all_nav"></i>全部分类
			</div>
			<div id="orderBtn" class="order_nav"><span id="TimerShow"></span></div>
			<div id="courseNavWrapper" class="second_nav">
				<div class="courseNavCon">
					<ul id="courseNavList" class="second_nav_list cf">
					</ul>
				</div>
			</div>
			<div class="classtitle"></div>
			<ul id="orderList" class="order_list">
				<li class="cur" data-title="综合" data-id="10">综合排序<i class="order_zh"></i></li>
				<li data-title="人气" data-id="20">人气最高<i class="order_renqi"></i></li>
				<li data-title="开班" data-id="30">开班时间<i class="order_time"></i></li>
				<li data-title="开班" data-id="31">开班时间<i class="order_time_1"></i></li>
			</ul>
		</div>
	</nav>
	    
		<section>		
			<article class="questionarea" data-style="" data-questionid="">
            
            </article>
			<div id="loadMore" class="load_more" style="display: none;"></div>
		</section>
	</div>
	<div id="layerMask" class="mask"></div>

<input type="hidden" id="hidCateID" value="<%=productID %>">
<input type="hidden" id="hidIsCoupon" value="0"> 
<input type="hidden" id="hidSelecttype" value="2">
<input type="hidden" id="EndTimer" value="<%=endTimer %>">
<input type="hidden" id="hHideNav" value="1">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
<script id="questionlist-tmpl" type="text/tmpl">
    {{~it:v:i}}
    {{?v.QuestionsNum>0}}
    <li>
    	<div class="cateItem">
	    	<i class="icon-en"></i>
			<span class="name" id="StyleTitle{{=v.StyleId}}">{{=v.StyleName}}</span>
		</div>
		<div class="nav_list">
		{{for(var j=0;j<v.QuestionsNum;j++){}}
			<span data-styleid="{{=v.StyleId}}" data-id="{{=j+1}}" class="sub_cate">第{{=j+1}}题</span>
		{{}}}
		</div>
    </li>
    {{?}}
    {{~}}
</script>
<script id="questions-tmpl" type="text/tmpl">
    {{=it.CateId}}、{{=it.Question}}
        <ul>
        {{?it.Style=="3"}}
            <li data-value="1">{{=it.A}}</li>
            <li data-value="0">{{=it.B}}</li>
        {{??}}
            <li data-value="A">A、{{=it.A}}</li>
            <li data-value="B">B、{{=it.B}}</li>
            <li data-value="C">C、{{=it.C}}</li>
            <li data-value="D">D、{{=it.D}}</li>
        {{?}}
        </ul>
    </script>
<script src="layer_mobile/layer.js" type="text/javascript"></script>
<script src="js/course.js?v=2017.08.02" type="text/javascript"></script>
<script src="js/test.js?v=2017.08.03" type="text/javascript"></script>
</asp:Content>