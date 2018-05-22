<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="CourseCateAdd.aspx.cs" Inherits="XueFuShop.Admin.CourseCateAdd" Title="无标题页" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server"><div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>题库分类<%=GetAddUpdate()%></div>
<div class="add">	<ul>		<li class="left">上级分类：</li>		<li class="right"><asp:DropDownList ID="FatherID" runat="server" /></li>	</ul>	<ul>		<li class="left">分类名称：</li>		<li class="right"><XueFu:TextBox ID="ClassName" CssClass="input" runat="server" Width="200px" CanBeNull="必填" ></XueFu:TextBox></li>	</ul>	<ul>		<li class="left">排序ID：</li>		<li class="right"><XueFu:TextBox ID="OrderID" CssClass="input" runat="server" Width="200px" CanBeNull="必填" RequiredFieldType="数据校验" HintInfo="同一分类中数字越小越排前">0</XueFu:TextBox></li>
    </ul>
    <ul><XueFu:Hint ID="Hint" runat="server"/></ul></div><div class="action">    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" /></div>
</asp:Content>
