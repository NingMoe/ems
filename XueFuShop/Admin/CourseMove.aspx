<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="CourseMove.aspx.cs" Inherits="XueFuShop.Admin.CourseMove" Title="无标题页" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server"><div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>题库分类移动</div>
<div class="add">	<ul>
		<li class="left">课程类别：</li>
		<li class="right"><asp:DropDownList ID="CateId" Width="300px" runat="server"></asp:DropDownList></li>
	</ul>
    <ul><XueFu:Hint ID="Hint" runat="server"/></ul></div><div class="action">    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" /></div>
</asp:Content>
