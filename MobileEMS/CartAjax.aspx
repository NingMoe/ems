<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CartAjax.aspx.cs" Inherits="MobileEMS.CartAjax" %>
<%@ Import Namespace="XueFuShop.Common" %>
<div class="tabTitleBox">
    <table border="0" cellspacing="0" cellpadding="0" class="tabTitle">
    <thead>
        <tr>
            <td style="width:120px" height="40" align="center" valign="middle">课程名称</td>
            <!--<td style="width:80px">赠送积分</td>-->
            <td style="width:80px" align="center">单价</td>
            <td style="width:80px" align="center">数量</td>
            <td style="width:80px" align="center">小计</td>
            <td style="width:80px" align="center">操作</td>
        </tr>
    </thead>
    <tbody>
<asp:Repeater ID="RecordList" runat="server">	<ItemTemplate>	
        <tr valign="middle" id="Cart<%# Eval("ID")%>">
            <td><%# Eval("ProductName")%></td>
            <td>￥<%# Eval("ProductPrice")%></td>
            <td><input type="hidden" id="BuyCount<%# Eval("ID")%>" value="<%# Eval("BuyCount")%>" /><input  type="text" class="input" value="<%# Eval("BuyCount")%>"  onblur="changeOrderProductBuyCount('<%# Eval("ID")%>',this,<%# Eval("ProductPrice")%>,<%# Eval("BuyCount")%>,0,0)"/></td>
            <td id="CartProductPrice<%# Eval("ProductID")%>">￥<%# Convert.ToDecimal(Eval("ProductPrice")) * int.Parse(Eval("BuyCount").ToString())%></td>
            <td><a href='javascript:void(0)' onclick="deleteOrderProduct('<%# Eval("ID")%>',<%# Eval("ProductPrice")%>,<%# Eval("BuyCount")%>,0)">删除</a></td>
        </tr>    </ItemTemplate></asp:Repeater>
    </tbody>
    </table>
    <div class="shopCartPrice">
        <span class="total">商品总金额： </span>
        <span class="totalPrice"><span id="CartProductTotalPrice">￥<%=Sessions.ProductTotalPrice%></span></span>
    </div>
</div>
<div class="height10"></div>  
<div class="cartOperate"><a href="javascript:void(0);"> <img src="Images/cart.png"  />继续购物</a>  <a href="javascript:clearCart()"><img src="Images/delete.gif" />清空购物车</a> <a href="/CheckOut.aspx" class="btnGreen cartbutton">立即结算</a></div>