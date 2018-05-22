<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="XueFuShop.Admin.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title><%=XueFuShop.Common.Global.ProductName%>后台管理</title>
    <link href="Style/style.css" type="text/css" rel="stylesheet" media="all" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="mainBody" style="width:800px;margin: auto">
        <div class="loginbody">
            <div class="logo"></div>
            <div class="loginTable">
                <div class="title">欢迎登录<%=XueFuShop.Common.Global.ProductName%></div>             
                <div class="lable">用户名：<asp:TextBox ID="AdminName" CssClass="input" Width="150px" runat="server"></asp:TextBox></div>
                <div class="lable">密　码：<asp:TextBox ID="Password" TextMode="password" CssClass="input" Width="150px" runat="server"></asp:TextBox></div>
                <div class="lable">验证码：<asp:TextBox ID="SafeCode" CssClass="input" Width="100px" runat="server"></asp:TextBox>&nbsp;<img src="Code.aspx" onclick="this.src='Code.aspx'" style="cursor:pointer" alt="点击刷新验证码" align="absmiddle" id="IMG1" /></div>   
                <div class="lable blank"><asp:CheckBox ID="Remember" runat="server"/> <label for="Remember" >记住登录状态</label></div>
                <div class="lable blank"><asp:Button CssClass="button" ID="SubmitButton" Text=" 登  录 " runat="server"  OnClick="SubmitButton_Click"/></div>                       
                <div class="lable2"></div>         
            </div>
            <div style="clear:both"></div>
        </div>    
        <div class="footer"> 
            <span id="SkyVersion"><%=XueFuShop.Common.Global.ProductName%> <%=XueFuShop.Common.Global.Version%></span>
	        <span><a href="#" target="_blank"><%=XueFuShop.Common.Global.CopyRight%></a></span>
	    </div>
	</div>
    </form>
</body>
</html>
