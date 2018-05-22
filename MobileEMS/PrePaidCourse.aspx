<%@ Page Language="C#" MasterPageFile="~/Course.Master" AutoEventWireup="true" CodeBehind="PrePaidCourse.aspx.cs" Inherits="MobileEMS.PrePaidCourse" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="ctl00_left_nav_pnlLast">
		
<a href="#" class="link-back" id="Previous"><i class="icon-back"></i></a>

	</div>
<div id="ctl00_left_nav_pnlUserCenter">
		
 <a href="javascript:;" class="link-myinfo" onclick="SendEvent(25, 203)">
 	<i id="userImgHead"><img src="./course_files/31959858.jpg" width="23" height="23"></i></a>

	</div>
    <asp:Label ID="CenterTitle" runat="server" Text="Label"></asp:Label>
            <a href="Cart.aspx" class="link-cart"><i class="icon-cart"></i></a>
            <a href="Search.aspx?Action=<%=Action %>" class="link-search" onclick="SendEvent(25, 200)"><i class="icon-search-s"></i></a>
            
            <a href="javascript:;" class="link-menu" onclick="SendEvent(25, 202)"><i class="icon-menu"></i></a>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
<div class="content" style="min-height: 581px;">
    
		<section>		
			<article id="courseList" class="course_list">
            
            </article>
			<div id="loadMore" class="load_more" style="display: none;"></div>
		</section>
	</div>
	<div id="layerMask" class="mask"></div>

<input type="hidden" id="hidCateID" value="<%=CateId %>">
<input type="hidden" id="questType" value="<%=Action %>">
<input type="hidden" id="hidIsCoupon" value="0"> 
<input type="hidden" id="hidSelecttype" value="6">
<input type="hidden" id="hHideNav" value="1">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
<script id="course-tmpl" type="text/tmpl">
    {{~it:v:i}}
   		<div class="ke">

			<div class="ke_info">
				{{? v.discount_class}}
					<i class="discount {{=v.discount_class}}"></i>
				{{?}}
				<div class="tit"><a href="CourseDetail.aspx?CateId={{=v.ClassID}}">{{=v.Title}}</a></div>
				<div class="desc">{{? v.TestCount>0 }}考试次数：{{=v.TestCount}}{{?}}     {{? v.IsPass }}状态：已完成{{?? v.TestCount>0}}状态：学习中{{??}}状态：待学习{{?}}  剩余学习时间：{{=v.RemainderDays}}天</div>
				{{? v.IsVideo}}<a href="SMSCheck.aspx?CateId={{=v.ClassID}}" class="btnGreen btn btnlook">视频</a>{{?}}
				{{? v.IsTest}}<a href="TestCenter.aspx?CateId={{=v.ClassID}}" class="btnGreen btn btntest">考试</a>{{?}}
			</div>
		</div>
    {{~}}
</script>
<script src="js/course.js?v=01.005" type="text/javascript"></script>
</asp:Content>
