<%@ Page Language="C#" MasterPageFile="~/Course.Master" AutoEventWireup="true" CodeBehind="OrderDetail.aspx.cs" Inherits="MobileEMS.OrderDetail" Title="无标题页" %>
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
            <a href="Search.aspx?Action=" class="link-search" onclick="SendEvent(25, 200)"><i class="icon-search-s"></i></a>
            
            <a href="javascript:;" class="link-menu" onclick="SendEvent(25, 202)"><i class="icon-menu"></i></a>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
<div class="content" style="min-height: 581px;">
	    
		<section>		
			<article id="courseList" class="course_list">
                                        		<div class="buyTitle">基本信息</div>
                                                <div class="checkOutLine"></div>
                                                <div class="add checkOut">
                                                    <ul>
                                                        <li class="left"> 订单号：</li>
                                                        <li class="right"><%=order.OrderNumber%></li>
                                                    </ul>
                                                    <ul>
                                                        <li class="left"> 下单时间：</li>
                                                        <li class="right"><%=order.AddDate%></li>
                                                    </ul>  
                                                    <ul>
                                                        <li class="left"> 状态：</li>
                                                        <li class="right"><%=OrderBLL.ReadOrderStatus(order.OrderStatus)%></li>
                                                    </ul>
                                                    <ul>
                                                        <li class="left"> 支付方式：</li>
                                                        <li class="right"><%=order.PayName%></li>
                                                    </ul>
                                                    <ul>
                                                        <li class="left"> 订单金额：</li>
                                                        <li class="right">￥<%=order.ProductMoney%></li>
                                                    </ul>
                                                </div>
                                                <div class="buyTitle">商品信息</div>
                                                <div class="checkOutLine"></div>
                                                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tabTitle">
                                                <thead>
                                                    <tr class="cartHead">
                                                        <td >名称</td>
                                                        <td>数量</td>
                                                        <td>单价</td>
                                                    </tr>
                                                 </thead>
                                                 <tbody>
                                                    <asp:Repeater ID="RecordList" runat="server">
	                                                        <ItemTemplate>
                                                                <tr class="cartMain" valign="middle">
                                                                    <td style="width:40%; text-align:left; text-indent:8px;"><%# Eval("ProductName")%></td>
                                                                    <td style="width:30%"><%# Eval("BuyCount")%></td>
                                                                    <td style="width:30%">￥<%# Eval("ProductPrice")%></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                                </table>  
            </article>
			<div id="loadMore" class="load_more" style="display: none;"></div>
		</section>
	</div>
	<div id="layerMask" class="mask"></div>

<input type="hidden" id="hidCateID" value="">
<input type="hidden" id="hidePostId" value="">
<input type="hidden" id="questType" value="">
<input type="hidden" id="hidIsCoupon" value="0"> 
<input type="hidden" id="hidSelecttype" value="0">
<input type="hidden" id="hHideNav" value="1">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
