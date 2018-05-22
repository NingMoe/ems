<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="TemplateFileAdd.aspx.cs" Inherits="XueFuShop.Admin.TemplateFileAdd" Title="无标题页" %>
<%@ Register Assembly="XueFu.EntLib" Namespace="XueFu.EntLib" TagPrefix="XueFu" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>编辑文件</div>
<ul  class="search">
	<li> &nbsp; 文件名：<%=fileName %></li>
</ul>
<div class="add">
	<div style="padding-left:2px"><XueFu:TextBox ID="Content" runat="server" Width="100%" Height="400px" TextMode="MultiLine" /></div>
</div>
<div class="action">
    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />&nbsp;<input type="button"  value="文件列表 " class="button"  onclick="window.location.href='TemplateFile.aspx?Path=<%=path %>'"/>
</div>
</asp:Content>
