<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="MenuAdd.aspx.cs" Inherits="XueFuShop.Admin.MenuAdd" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="position"><img src="Style/Images/PositionIcon.png"  alt=""/>菜单<%=GetAddUpdate()%></div>
<div class="add">
	<ul>
		<li class="left">所属分类：</li>
		<li class="right"><asp:DropDownList ID="FatherID" runat="server" Width="380px"/></li>
	</ul>
	<ul>
		<li class="left">菜单名称：</li>
		<li class="right"><asp:TextBox  ID="MenuName" CssClass="input" Width="380px" runat="server"></asp:TextBox> <asp:RequiredFieldValidator
                ID="RequiredFieldValidator1" runat="server" ErrorMessage="不能为空" ControlToValidate="MenuName" Display="Dynamic"></asp:RequiredFieldValidator></li>
	</ul>
	<ul>
		<li class="left">菜单图标：</li>
		<li class="right"><asp:RadioButtonList ID="MenuImage" runat="server" RepeatDirection="Horizontal" RepeatColumns="10"></asp:RadioButtonList></li>
	</ul>
	<ul>
		<li class="left">链接地址：</li>
		<li class="right"><asp:TextBox ID="URL"  CssClass="input" Width="380px" HintInfo="如果是外部地址，请在地址前带上Http://" runat="server"></asp:TextBox> <asp:RequiredFieldValidator
                ID="RequiredFieldValidator2" runat="server" ErrorMessage="不能为空" ControlToValidate="URL" Display="Dynamic"></asp:RequiredFieldValidator></li>
	</ul>
	<ul>		<li class="left">排序ID：</li>		<li class="right"><asp:TextBox ID="OrderID" CssClass="input" runat="server" Width="380px" HintInfo="数字越小越排前"></asp:TextBox>  <asp:RequiredFieldValidator
            ID="RequiredFieldValidator3" runat="server" ErrorMessage="不能为空" ControlToValidate="OrderID" Display="Dynamic"></asp:RequiredFieldValidator></li>	</ul>
	<ul><asp:TextBox ID="Hint" runat="server" Visible="False"></asp:TextBox></ul>
</div>
<div class="action">
    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
</div>
</asp:Content>
