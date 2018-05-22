<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="XueFuShop.Admin.ChangePassword" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="position"><img src="Style/Images/PositionIcon.png"  alt=""/>修改密码</div>
<div class="add">
	<ul>
		<li class="left">登陆名：</li>
		<li class="right"><asp:TextBox CssClass="input" Width="300px" ID="Name" runat="server" Enabled="false"></asp:TextBox></li>
	</ul>
	<ul>
		<li class="left">旧密码：</li>
		<li class="right"><asp:TextBox CssClass="input" Width="300px" ID="Password" runat="server" TextMode="Password"></asp:TextBox>   
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="不能为空" ControlToValidate="Password" Display="Dynamic"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="^[\W\w]{6,16}$" ErrorMessage="密码长度大于6位少于16位" Display="Dynamic" ControlToValidate="NewPassword"></asp:RegularExpressionValidator></li>
	</ul>
	<ul>
		<li class="left">新密码：</li>
		<li class="right"><asp:TextBox CssClass="input" Width="300px" ID="NewPassword" runat="server" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator
                ID="RequiredFieldValidator2" runat="server" ErrorMessage="不能为空" ControlToValidate="NewPassword" Display="Dynamic"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^[\W\w]{6,16}$" ErrorMessage="密码长度大于6位少于16位" Display="Dynamic" ControlToValidate="NewPassword"></asp:RegularExpressionValidator></li>
	</ul>
    
	<ul>
		<li class="left">重复密码：</li>
		<li class="right"><asp:TextBox CssClass="input" Width="300px" ID="NewPassword2" runat="server" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator
                ID="RequiredFieldValidator3" runat="server" ErrorMessage="不能为空" ControlToValidate="NewPassword2" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="两次密码不一致" ControlToCompare="NewPassword" ControlToValidate="NewPassword2" Display="Dynamic"></asp:CompareValidator>
        </li>
	</ul>
</div>
<div class="action">
    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
</div>
</asp:Content>
