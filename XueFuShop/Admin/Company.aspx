﻿<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="Company.aspx.cs" Inherits="XueFuShop.Admin.Company" Title="无标题页" %>
<%@ Import Namespace="XueFuShop.BLL" %>
<script language="javascript" type="text/javascript">
function gopage()
{
var companyidstr="";
$("[name='SelectID'][checked]:checkbox").each(function(){companyidstr=companyidstr+","+$(this).val();});
pop('UserMove.aspx?Action=Company&SelectID='+companyidstr.substr(1),800,600,'用户移动','UserAdd');
}
</script>
</asp:Content>