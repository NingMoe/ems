﻿<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="WorkingPost.aspx.cs" Inherits="XueFuShop.Admin.WorkingPost" Title="无标题页" %>
<%@ Import Namespace="XueFuShop.BLL" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<link rel="stylesheet" href="Style/jquery.bigautocomplete.css" type="text/css" />
<script type="text/javascript" src="Js/jquery.1.12.4.min.js"></script>
<script type="text/javascript" src="Js/jquery.bigautocomplete.js"></script>
<div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>工作岗位列表</div>	
<div id="copylayer" style="display:none; margin:20px;">
    公司名称：<input type="text" name="TargetCompanyName" id="TargetCompanyName" class="input companyname" value="" />
</div>
		岗位名称：<asp:TextBox CssClass="input" ID="Name" runat="server" Width="150px"></asp:TextBox>
<%if (Action == "Search") 
{%>
<table class="listTable" cellpadding="0">
			<td style="text-align:left;"><%# !string.IsNullOrEmpty(Eval("Path").ToString())?"<span data-level=\""+Eval("Path").ToString().Split('|').Length+"\" class=\"icontree\">├</span>&nbsp;&nbsp;":""  %><%# Eval("PostName") %></td>
				<span class="handle">
<%} %>
<script type="text/javascript">
$(function(){
    $(".icontree").each(function (index, item) {
	    var level = $(this).data("level");
	    $(this).before("&nbsp;&nbsp;&nbsp;&nbsp;"); String.fromCharCode(9)
	});
    $("#" + globalIDPrefix + "CompanyName").bigAutocomplete({
	    width:"304px",
		url:"Ajax.aspx?Action=SearchCompanyName",
		callback:function(data){
    		$("#CompanyID").val(data.result.Id);
		}
    });
    $("#TargetCompanyName").bigAutocomplete({
        width: "304px",
        url: "Ajax.aspx?Action=SearchCompanyName",
        callback: function (data) {
            $("#CompanyID").val(data.result.Id);
        }
    });
    $("#copybutton").click(function () {
        var selectIDArray = new Array();
        $.each($('input:checkbox:checked'), function () {
            selectIDArray.push($(this).val());
        });
        var selectID = selectIDArray.toString();
        if (selectID == "") {
            layer.msg("请先选择岗位");
            return false;
        }
	    layer.open({
	        type: 1,
	        title: '选择复制到的目标',
	        closeBtn: 0,
	        shadeClose: true,
	        skin: 'yourclass',
	        content: $("#copylayer"),
	        btn: ['确认', '取消'],
	        zIndex:2,
	        yes: function (index, layero) {
	            var loadIndex = layer.load(1, { time: 2 * 1000, shade: 0.5 });
	            $.post("Ajax.aspx?Action=CopyPostList", { PostIDString: selectID, TargetCompanyID: $("#CompanyID").val() }, function (data) {
	                data = JSON.parse(data);
	                if (data.Success) {
	                    layer.msg("复制成功！");
	                }
	                else {
	                    layer.msg("复制失败！" + data.Msg);
	                }
	            })
	            layer.close(index);
	        }
	    });
	})
})
</script>
</asp:Content>