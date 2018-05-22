<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="EmailSendRecordAdd.aspx.cs" Inherits="XueFuShop.Admin.EmailSendRecordAdd" Title="无标题页" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>Email营销 </div>
<div class="add">
	<ul>
		<li class="left">邮件内容：</li>
		<li class="right"><asp:DropDownList id="Key"  runat="server"/></li>
	</ul>
	<ul>
		<li class="left">是否统计用户打开：</li>
		<li class="right"><asp:RadioButtonList ID="IsStatisticsOpendEmail" RepeatDirection="Horizontal" runat="server"><asp:ListItem Value="0" Selected="True">否</asp:ListItem><asp:ListItem Value="1">是</asp:ListItem></asp:RadioButtonList></li>
	</ul>
	<ul>
		<li class="left">用户等级：</li>
		<li class="right"><asp:CheckBoxList ID="UserGrade" runat="server" RepeatDirection="Horizontal" /></li>
	</ul>
	<ul>
		<li class="left">备注：</li>
		<li class="right"><asp:TextBox ID="Note" TextMode="MultiLine" runat="server"  Width="400px"  Height="100px"/></li>
	</ul>
</div>
<div class="action">
    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click"  OnClientClick="return CheckEmialContentSelect()"/>
</div>
</asp:Content>
