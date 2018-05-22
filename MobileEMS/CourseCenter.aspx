<%@ Page Language="C#" MasterPageFile="~/Course.Master" AutoEventWireup="true" CodeBehind="CourseCenter.aspx.cs" Inherits="MobileEMS.CourseCenter" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div id="ctl00_left_nav_pnlLast">		
		<a href="#" class="link-back" id="Previous"><i class="icon-back"></i></a>
	</div>
	<div id="ctl00_left_nav_pnlUserCenter">
		<a href="javascript:;" class="link-myinfo">
	 		<i id="userImgHead"><img src="./images/31959858.jpg" width="23" height="23"></i>
	 	</a>
	</div>
	<asp:Label ID="CenterTitle" runat="server" Text="Label"></asp:Label>
	<!--<a href="Cart.aspx" class="link-cart"><i class="icon-cart"></i></a>-->
	<a href="Search.aspx?Action=<%=Action %>" class="link-search"><i class="icon-search-s"></i></a>
	<!--<a href="javascript:;" class="link-menu"><i class="icon-menu"></i></a>-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
	<div class="content" style="min-height: 581px;">		
		<nav class="top_nav">
			<div class="nav">
				<div id="courseNav" class="first_nav"></div>
				<div id="courseNavWrapper" class="second_nav">
					<div class="courseNavCon">
						<ul id="courseNavList" class="second_nav_list cf">
						</ul>
						<%if(Action=="PostCourse") {%><a href="javascript:void(0);" id="allCourseBtn" title="岗位全部课程" class="btn">岗位全部未过课程</a><%} %>
					</div>
				</div>
			</div>
		</nav>

		<section>		
			<article id="courseList" class="course_list">
	        
	        </article>
			<div id="loadMore" class="load_more" style="display: none;"></div>
		</section>
	</div>
	<div id="layerMask" class="mask"></div>

	<input type="hidden" id="hidCateID" value="<%=CateId %>">
	<input type="hidden" id="hidePostId" value="<%=PostId %>">
	<input type="hidden" id="questType" value="<%=Action %>">
	<input type="hidden" id="hidIsCoupon" value="0"> 
	<input type="hidden" id="hidSelecttype" value="0">
	<input type="hidden" id="hHideNav" value="1">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
<script id="catelist-tmpl" type="text/tmpl">
    <li>
    	<div class="cateItem">
	    	<i class="{{=it.Icon}}"></i>
			<span class="name">{{=it.Title}}</span>
		</div>
		<div class="nav_list">
		{{~it.ChildCourseFCateView:val:j}}
			<span data-id="{{=val.ID}}" class="sub_cate">{{=val.Name}}</span>
		{{~}}
		</div>
    </li>
</script>
<script id="course-tmpl" type="text/tmpl">
    {{~it:v:i}}
   		<div class="ke">
			<div class="ke_info">
				{{? v.discount_class}}
					<i class="discount {{=v.discount_class}}"></i>
				{{?}}
				<div class="tit"><a href="javascript:void(0);">{{=v.Title}}</a></div>
				<!--<div class="desc">考试次数：{{=v.TestCount}}     {{? v.IsPass }}状态：已完成{{?? v.TestCount>0}}状态：学习中{{?}}</div>-->
				{{? v.IsPostCourse}}
					{{? v.IsVideo}}<a href="SMSCheck.aspx?CateId={{=v.ClassID}}" class="btnGreen btn btnlook">视频</a>{{?}}
                    {{? v.IsRC}}<a href="RCDetail.aspx?CourseName={{=v.Title}}&Url={{=v.RCUrl}}" class="btnGreen btn btnlook">绕车</a>{{?}}
					{{? v.IsTest}}
						{{? v.ValidDateShow && v.ValidDateShow > 0}}
						<a href="javascript:void(0);" class="btnGreen btn btntest">{{=v.ValidDateShow}}h</a>
						{{??}}
						<a href="TestCenter.aspx?CateId={{=v.ClassID}}" class="btnGreen btn btntest">考试</a>
						{{?}}
					{{?}}
				{{??}}
				<!--<div class="desc">价格：{{=v.OriginalPrice}}元     {{? v.IsPass }}状态：已完成{{?? v.TestCount>0}}状态：学习中{{?}}</div>
				<a href="javascript:addToCart({{=v.ClassID}},'{{=v.Title}}',{{=v.OriginalPrice}},'0');" class="btnGreen btn btntest">购买</a>-->
				{{?}}
			</div>
		</div>
    {{~}}
</script>
<script src="js/course.js?v=01.005" type="text/javascript"></script>
<script src="js/cart.js?v=01.005" type="text/javascript"></script>
</asp:Content>
