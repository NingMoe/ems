<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="FileAdd.aspx.cs" Inherits="XueFuShop.Admin.FileAdd" Title="无标题页" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="add">
    <ul>
        <li class="left" style="width:60px">文件：</li>
        <li class="right"><asp:FileUpload ID="UploadFile"  runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="UploadFile" Display="Dynamic" ErrorMessage="* 请选择文件" runat="server"></asp:RequiredFieldValidator></li>
    </ul>
</div>
<div class="action">
    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
</div>
</asp:Content>
