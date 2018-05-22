<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="DirectoryAdd.aspx.cs" Inherits="XueFuShop.Admin.DirectoryAdd" Title="无标题页" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="add">
    <ul>
        <li class="left">文件夹名：</li>
        <li class="right"><XueFu:TextBox ID="DirectoryName" Width="160px" CssClass="input" runat="server" CanBeNull="必填" /></li>
    </ul>
</div>
<div class="action">
    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
</div>
</asp:Content>
