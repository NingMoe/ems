<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="AdminGroupAdd.aspx.cs" Inherits="XueFuShop.Admin.AdminGroupAdd" %>
<%@ Register Assembly="XueFu.EntLib" Namespace="XueFu.EntLib" TagPrefix="XueFu"%>
<%@ Import Namespace="XueFuShop.Models" %>
<%@ Import Namespace="XueFuShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>管理组<%=GetAddUpdate()%></div>
<div  class="listBlock">
    <ul>
        <li class="listOn" id="TitleDefault" onclick="switchBlock('Default')">基本设置</li>
        <li id="TitlePower" onclick="switchBlock('Power')">权限设置</li>
    </ul>	
</div>
<div class="line"></div>
<div class="add" id="ContentDefault">
	<ul>
		<li class="left">管理员名：</li>
		<li class="right"><XueFu:TextBox CssClass="input" Width="300px" ID="Name" runat="server" CanBeNull="必填" /></li>
	</ul>	
	<ul>
		<li class="left">备注：</li>
		<li class="right"><XueFu:TextBox CssClass="input" Width="300px" ID="Note" runat="server"  TextMode="MultiLine" Height="80px"/></li>
	</ul>	
	<ul>
		<li class="left">使用范围：</li>
		<li class="right">
            <asp:RadioButtonList ID="State" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="0">后台</asp:ListItem>
                <asp:ListItem Value="1">客户使用</asp:ListItem>
            </asp:RadioButtonList></li>
	</ul>	
</div>
<div class="add" id="ContentPower"  style="display:none">
    <%foreach (PowerInfo channelPower in channelPowerList)
      { %>
    <div class="powerBox">
	    <div class="powerHead"><%=channelPower.Text%></div>
	    <%foreach (PowerInfo blockPower in ReadPowerBlock(channelPower.XML))
       { %>
	    <ul>
		    <li class="left"><%=blockPower.Text%></li>
		    <li class="right">
		        <%foreach (PowerInfo itemPower in ReadPowerItem(blockPower.XML))
            { %>
		            <%if (power.IndexOf("|" + channelPower.Key + itemPower.Value + "|") > -1)
                    {%>
			            <input name="Rights" type="checkbox" value="<%= channelPower.Key + itemPower.Value%>" checked="checked"/>
			            <%}
                    else
                    { %>
                        <input name="Rights" type="checkbox" value="<%= channelPower.Key + itemPower.Value%>"/>
			        <%} %>
			        <%=itemPower.Text %>
			    <%} %>
		    </li>
	    </ul>
	    <%} %>
    </div>
    <%} %>
</div>
<div class="action">
    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />&nbsp;<input name="All" type="checkbox" onclick="selectAll(this)" />全选/取消
</div>
<script language="javascript" type="text/javascript">
//全选全不选
function selectAll(obj){
    var objs=os("name","Rights");
    if (objs != null && objs.length > 0) {
        for (var i = 0; i < objs.length; i++) {            
            objs[i].checked=obj.checked;            
        }
    }
}
</script>
</asp:Content>
