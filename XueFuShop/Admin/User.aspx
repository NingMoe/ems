﻿<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="XueFuShop.Admin.User"%>
<%@ Import Namespace="XueFu.EntLib" %>
<%@ Import Namespace="XueFuShop.BLL" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<link rel="stylesheet" href="Style/jquery.bigautocomplete.css" type="text/css" />
<script type="text/javascript" src="http://cdn.bootcss.com/jquery/1.12.1/jquery.min.js"></script>
<script type="text/javascript" src="Js/jquery.bigautocomplete.js"></script>
          {%><li style="border:0;"><asp:Button CssClass="button" ID="UserOutXls" Text=" 导 出 " runat="server"  OnClick="UserOutXls_Click" /></li><%} %>
          { %><asp:Button CssClass="button" ID="ActiveButton" Text=" 激 活 " OnClientClick="return checkSelect()" runat="server"  OnClick="ActiveButton_Click"/>&nbsp;<%} %>
          { %>
<script type="text/javascript">
$(function(){
	$("#ctl00_ContentPlaceHolder_CompanyName").bigAutocomplete({
	    width:"304px",
		url:"Ajax.aspx?Action=SearchCompanyName",
		callback:function(data){
		    var result=data.result;
    		$("#CompanyID").val(result.Id);
		}
	});
})
</script>
function deleteUser(userID){