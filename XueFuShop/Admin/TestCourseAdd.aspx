﻿<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="TestCourseAdd.aspx.cs" Inherits="XueFuShop.Admin.TestCourseAdd" Title="无标题页" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<link rel="stylesheet" href="Style/jquery.bigautocomplete.css" type="text/css" />
<script type="text/javascript" src="Js/jquery.1.12.4.min.js"></script>
<script type="text/javascript" src="Js/jquery.bigautocomplete.js"></script>
		    <XueFu:TextBox ID="PaperScore" CssClass="input" runat="server" Width="100px" RequiredFieldType="数据校验" HintInfo="" />
		    <asp:Literal ID="PaperScorseTips" runat="server"></asp:Literal></li>
		    <XueFu:TextBox ID="TestTimeLength" CssClass="input" runat="server" Width="100px" RequiredFieldType="数据校验" HintInfo="" />
		    <asp:Literal ID="TestTimeLengthTips" runat="server"></asp:Literal>
		    <XueFu:TextBox ID="QuestionsNum" CssClass="input" runat="server" Width="100px" RequiredFieldType="数据校验" HintInfo="" />
		    <asp:Literal ID="QuestionsNumTips" runat="server"></asp:Literal>
		    <XueFu:TextBox ID="LowScore" CssClass="input" runat="server" Width="100px" RequiredFieldType="数据校验" HintInfo="" />
		    <asp:Literal ID="LowScoreTips" runat="server"></asp:Literal>
<script type="text/javascript">
$("input[name='AllBrandID']").change(function(){
    if($(this).is(":checked")){
	    $("input[data-type='"+$(this).val()+"']").prop("checked",true);
    }else{
	    $("input[data-type='"+$(this).val()+"']").prop("checked",false);
    }
})
$("input[name='BrandID']").change(function(){
    if($(this).is(":checked")){
        if($("input[name='BrandID']:not(:checked)").length <= 0){
	        $("input[name='AllBrandID']").prop("checked",true);
	    }
    }else{
	    $("input[name='AllBrandID']").prop("checked",false);
    }
})
$(function(){
	$("#CompanyName").bigAutocomplete({
	    width:"400px",
		url:"Ajax.aspx?Action=SearchCompanyName",
		callback:function(data){
		    $("#CompanyId").val(data.result.Id);
		}
	});
})
</script>
</asp:Content>