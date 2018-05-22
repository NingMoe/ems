<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="NoteBook.aspx.cs" Inherits="XueFuShop.Admin.NoteBook" Title="无标题页" %>
<%@ Import Namespace="XueFuShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">	
<div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>记事本</div>
<div class="add">
	<ul>
		<li class="left">管理员名：</li>
		<li class="right"><%=Cookies.Admin.GetAdminName(false)%></li>
	</ul>
	<ul>
		<li class="left">记事本：</li>
		<li class="right"><XueFu:TextBox Width="300px" TextMode="MultiLine" Height="100px" ID="NoteBookContent" runat="server"/></li>
	</ul>	
</div>
<div class="action">
    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
</div>
</asp:Content>
