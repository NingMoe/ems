<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAjax.aspx.cs" Inherits="XueFuShop.Admin.UserAjax" %>
<%@ Register Namespace="XueFu.EntLib" Assembly="XueFu.EntLib" TagPrefix="XueFu"%>
<%@ Import Namespace="XueFuShop.Models.User" %>
<div style="border:1px solid #EEEEEE;">
<%foreach(UserInfo user in userList){ %>
<span><input type="checkbox" name="user" value="<%=user.ID%>|<%=user.UserName %>"  onclick="SendMessageAdd.selectUser(this)"/><%=user.UserName %></span>
<%} %>
<div class="clear"></div>
<div class="listPage"><XueFu:AjaxPager ID="MyPager" runat="server" />
</div>
</div>
