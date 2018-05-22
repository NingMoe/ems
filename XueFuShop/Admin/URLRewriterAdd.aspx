<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="URLRewriterAdd.aspx.cs" Inherits="XueFuShop.Admin.URLRewriterAdd" Title="无标题页" %>
<%@ Register Assembly="XueFu.EntLib" Namespace="XueFu.EntLib" TagPrefix="XueFu" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>地址重写<%=GetAddUpdate()%></div>
<div class="add">		
	<ul>
		<li class="left">实际地址：</li>
		<li class="right"><XueFu:TextBox ID="RealPath" CssClass="input" runat="server" Width="400px" CanBeNull="必填" /></li>
	</ul>
	<ul>
		<li class="left">重写地址：</li>
		<li class="right"><XueFu:TextBox ID="VitualPath" CssClass="input" runat="server" Width="400px" CanBeNull="必填" /></li>
	</ul>
	<ul>
		<li class="left">是否启用：</li>
		<li class="right"><asp:RadioButtonList ID="IsEffect" RepeatDirection="Horizontal" runat="server"><asp:ListItem Value="1" Selected="True">是</asp:ListItem><asp:ListItem Value="0">否</asp:ListItem></asp:RadioButtonList></li>
	</ul>
</div>
<div class="action">
    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
</div>
</asp:Content>
