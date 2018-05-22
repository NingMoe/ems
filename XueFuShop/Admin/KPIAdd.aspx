<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="KPIAdd.aspx.cs" Inherits="XueFuShop.Admin.KPIAdd" Title="无标题页" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<link rel="stylesheet" href="Style/jquery.bigautocomplete.css" type="text/css" />
<script type="text/javascript" src="Js/jquery.1.12.4.min.js"></script>
<script type="text/javascript" src="Js/jquery.bigautocomplete.js"></script>
<div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>KPI<%=GetAddUpdate()%></div><div class="add" >		<ul>		<li class="left">公司名称：</li>		<li class="right">		    <input type="text" name="CompanyName" id="CompanyName" class="input companyname" value="" runat="server" />		    <input type="hidden" name="CompanyId" id="CompanyId" value="<%=CompanyID %>" />		</li>	</ul>		<ul>		<li class="left">上级分类：</li>		<li class="right"><asp:DropDownList ID="FatherID" runat="server" /></li>	</ul>	<ul>		<li class="left">指标名称：</li>		<li class="right"><XueFu:TextBox ID="ClassName" CssClass="input" CanBeNull="必填" runat="server" Width="400px" /></li>	</ul>	<ul>		<li class="left">描述：</li>		<li class="right"><XueFu:TextBox ID="Introduction" CssClass="input" runat="server" Width="400px" Rows="5" TextMode="MultiLine" /></li>	</ul>	<ul>		<li class="left">评估人/部门：</li>		<li class="right"><XueFu:TextBox ID="Method" CssClass="input" runat="server" Width="100px" HintInfo="评估担当"/></li>	</ul>	<ul>		<li class="left">指标类型：</li>		<li class="right"><asp:DropDownList ID="Type" runat="server" Width="106px" /></li>	</ul>	<ul>		<li class="left">分数：</li>		<li class="right"><XueFu:TextBox ID="Score" CssClass="input" runat="server" Width="100px">0</XueFu:TextBox></li>	</ul>		<ul>		<li class="left">排序：</li>		<li class="right"><XueFu:TextBox ID="Sort" CssClass="input" runat="server" Width="100px">0</XueFu:TextBox></li>	</ul>	<ul><XueFu:Hint ID="Hint" runat="server"/></ul></div><div class="action">    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" /></div>
<script type="text/javascript">
$(function(){
	$("#"+globalIDPrefix+"CompanyName").bigAutocomplete({
	    width:"400px",
		url:"Ajax.aspx?Action=SearchCompanyName",
		callback:function(data){
		    $("#CompanyId").val(data.result.Id);
		}
	});
})
</script>
</asp:Content>
