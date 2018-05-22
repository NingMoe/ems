<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="AdminLog.aspx.cs" Inherits="XueFuShop.Admin.AdminLog" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="position"><img src="Style/Images/PositionIcon.png"  alt=""/>管理员日志</div>		   	 
<table class="listTable" cellpadding="0">
    <tr class="listTableHead">	
    <td style="width:10%">选择[ID]</td>
    <td style="width:40%; text-align:left;text-indent:8px;">操作记录</td>
    <td style="width:15%">帐号名称</td>
    <td style="width:20%">时间</td>
    <td style="width:15%">IP</td>
</tr>
<asp:Repeater ID="RecordList" runat="server">
    <ItemTemplate>
    <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">	
            <td style="width:5%"><input type="checkbox" name="SelectID" value="<%# Eval("ID") %>" /><%# Eval("ID") %></td>
            <td style="width:40%; text-align:left;text-indent:8px;"><%# Eval("Action") %></td>
            <td style="width:15%"><%# Eval("AdminName")%></td>
            <td style="width:20%"><%# Eval("AddDate")%></td>
            <td style="width:15%"><%# Eval("IP") %></td>	
        </tr>
    </ItemTemplate>
</asp:Repeater>
</table>
<div class="listPage">
    <XueFu:CommonPager ID="MyPager" runat="server" />
</div>
<div class="action">
    <asp:Button CssClass="button" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server" OnClick="DeleteButton_Click" />&nbsp;<input type="checkbox" name="All" onclick="selectAll(this)" />全选/取消
</div>
</asp:Content>
