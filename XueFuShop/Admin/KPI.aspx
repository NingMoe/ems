<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="KPI.aspx.cs" Inherits="XueFuShop.Admin.KPI" Title="无标题页" %>
<%@ Import Namespace="XueFuShop.BLL" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<link rel="stylesheet" href="Style/jquery.bigautocomplete.css" type="text/css" />
<script type="text/javascript" src="Js/jquery.1.12.4.min.js"></script>
<script type="text/javascript" src="Js/jquery.bigautocomplete.js"></script>
<div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>KPI列表</div>	<ul class="search">    <li>        公司名称：<input type="text" name="CompanyName" id="CompanyName" class="input companyname" value=""  runat="server" />        指标分类：<asp:DropDownList ID="KPICate" runat="server"></asp:DropDownList>
		指标名称：<asp:TextBox CssClass="input" ID="Name" runat="server" Width="150px"></asp:TextBox>		<asp:Button CssClass="button" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" />		<input type="hidden" name="CompanyID" id="CompanyID" value="<%=companyId.ToString() %>" />   </li></ul><table class="listTable" cellpadding="0">    <tr class="listTableHead">	    <td style="width:5%">ID</td>	    <td style="width:50%; text-align:left;text-indent:8px;">指标</td>	    <td style="width:12%">公司</td> 	    <td style="width:10%">类型</td>  	    <td style="width:10%">分类</td>       	    <td style="width:5%">分数</td>
		<td>操作</td></tr><asp:Repeater ID="RecordList" runat="server">	<ItemTemplate>	             <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">            <td><input type="checkbox" name="SelectID" value="<%# Eval("ID")%>" /></td>            <td style="text-align:left;text-indent:8px;"><%# Eval("Name") %></td>            <td><%# getCompanyName(int.Parse(Eval("CompanyID").ToString()))%></td>            <td><%# EnumHelper.ReadEnumChineseName<KPIType>((int)(KPIType)Eval("Type")) %></td>            <td><%# KPIBLL.ReadKPI(int.Parse(Eval("ParentId").ToString())).Name %></td>            <td><%# Eval("Scorse").ToString() %></td>            <td>
				<span class="handle">				    <a href="KPIAdd.aspx?ID=<%# Eval("ID")%>&ReturnUrl=<%=base.Server.UrlEncode(RequestHelper.RawUrl) %>"><img src="Style/Images/edit.gif" alt="修改" title="修改" /></a>				    <a href="javascript:if(confirm('确定要删除吗？')){window.location.href='?Action=Delete&ID=<%# Eval("ID")%>'}"><img src="Style/Images/delete.gif" alt="删除" title="删除" /></a>				</span>            </td>        </tr>        </ItemTemplate></asp:Repeater></table><div class="listPage"><XueFu:CommonPager ID="MyPager" runat="server" /></div><div class="action">        <input type="button"  value=" 添 加 " class="button"  onclick="pop('KPIAdd.aspx',800,600,'KPI添加','KPIAdd')"/>&nbsp;<asp:Button CssClass="button" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>&nbsp;<input type="checkbox" name="All" onclick="selectAll(this)" />全选/取消</div>
<script type="text/javascript">
$(function(){
	$("#"+globalIDPrefix+"CompanyName").bigAutocomplete({
	    width:"304px",
		url:"Ajax.aspx?Action=SearchCompanyName",
		callback:function(data){
    		$("#CompanyID").val(data.result.Id);
		}
	});	
})
</script>
</asp:Content>
