<%@ Page Language="C#" MasterPageFile="~/Course.Master" AutoEventWireup="true" CodeBehind="MyOrder.aspx.cs" Inherits="MobileEMS.MyOrder" Title="无标题页" %>
<%@ Import Namespace="XueFuShop.BLL" %>
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
	    <table cellpadding="0"  class="tabTitle">    <tr class="listTableHead">
                    <td>订单编号</td>
                    <%--<td>下单时间</td>--%>
                    <td>订单总额</td>
                    <%--<td>支付方式</td>--%>
                    <td>订单状态</td>
                    <td>操作</td>
                </tr>
<asp:Repeater ID="RecordList" runat="server">
	<ItemTemplate>	     
            <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
                <td style="width:5%"><%# Eval("OrderNumber")%></td>
                <%--<td ><%# Eval("AddDate")%></td>--%>
                <td style="width:15%"><%# Eval("ProductMoney")%></td>		 
                <%--<td style="width:10%"><%# Eval("PayName")%></td>--%>
                <td style="width:20%" id="OrderStatus<%#  Eval("Id")%>"><%# EnumHelper.ReadEnumChineseName<OrderStatus>(int.Parse(Eval("OrderStatus").ToString()))%></td>	
                <td style="width:25%" id="OrderOperate<%#  Eval("Id")%>"><a href="OrderDetail.aspx?ID=<%# Eval("Id")%>">查看</a> <%# OrderBLL.ReadOrderUserOperate(int.Parse(Eval("Id").ToString()),int.Parse(Eval("OrderStatus").ToString()),Eval("PayKey").ToString()) %></td>                  
            </tr>
        </ItemTemplate>
</asp:Repeater>
</table>
		<section>		
			<article id="courseList" class="course_list">
            
            </article>
			<div id="loadMore" class="load_more" style="display: none;"></div>
		</section>
	</div>
	<div id="layerMask" class="mask"></div>

<input type="hidden" id="hidIsCoupon" value="0"> 
<input type="hidden" id="hidSelecttype" value="0">
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
				<div class="desc">考试次数：{{=v.TestCount}}     {{? v.IsPass }}状态：已完成{{?? v.TestCount>0}}状态：学习中{{?}}</div>
				<%if(Action=="PostCourse") {%>
				{{? v.IsVideo}}<a href="SMSCheck.aspx?CateId={{=v.ClassID}}" class="btnGreen btn btnlook">视频</a>{{?}}
				{{? v.IsTest}}<a href="TestCenter.aspx?CateId={{=v.ClassID}}" class="btnGreen btn btntest">考试</a>{{?}}
				<%} %>
			</div>
		</div>
    {{~}}
</script>
<script src="js/Order.js?v=01.005" type="text/javascript"></script>
</asp:Content>
