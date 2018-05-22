<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="ActivityPlugins.aspx.cs" Inherits="XueFuShop.Admin.ActivityPluginsPage" Title="无标题页" %>
<%@ Import Namespace="XueFuShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>活动插件</div>
<table class="listTable" cellpadding="0" cellpadding="0">
    <tr class="listTableHead">
        <td style="width:20%">图片</td>
        <td  style="width:10%;">名称</td>
        <td style="width:40%">描述</td>
        <td style="width:10%">适用版本</td>
        <td style="width:10%">版权</td>
        <td style="width:5%">启用</td>
        <td style="width:5%">管理</td>
    </tr>
    <asp:Repeater ID="RecordList" runat="server">
	<ItemTemplate>	
    <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
        <td style="width:20%"><img src="<%# Eval("Photo") %>" /></td>
		<td style="width:10%;"><%# Eval("Name") %></td>
		<td style="width:40%; text-align:left;text-indent:8px;"><%# Eval("Description")%></td>
		<td style="width:10%;"><%# Eval("ApplyVersion") %></td>
		<td style="width:10%;"><%# Eval("CopyRight") %></td>
		<td style="width:5%"><span id="IsEnabled<%#Eval("Key") %>" style="cursor:pointer" onclick="updateStatus('<%#Eval("Key") %>','IsEnabled')"><%# ShopCommon.GetBoolString(Eval("IsEnabled"))%></span></td> 
		<td style="width:5%;">
		    <a href="<%# Eval("AdminUrl")%>"><img src="Style/Images/list.gif" alt="管理" title="管理" /></a>  
		</td>
    </tr>
    </ItemTemplate>
    </asp:Repeater>
</table>
</asp:Content>
