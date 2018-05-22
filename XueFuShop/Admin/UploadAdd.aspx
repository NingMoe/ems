<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadAdd.aspx.cs" Inherits="XueFuShop.Admin.UploadAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>附件上传</title>
    <link href="Style/style.css" type="text/css" rel="stylesheet" media="all" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:FileUpload ID="UploadFile" runat="server" CssClass="uploadFile" />&nbsp;<asp:Button ID="UploadButton" runat="server" Text=" 上 传 " CssClass="button"  OnClick="UploadImage"/>
    </div>
    </form>
</body>
</html>
