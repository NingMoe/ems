<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="AdminAdd.aspx.cs" Inherits="XueFuShop.Admin.AdminAdd" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="position"><img src="Style/Images/PositionIcon.png"  alt=""/>管理员<%=GetAddUpdate()%></div>
<div class="add">
    <ul>
		<li class="left">管理组：</li>
		<li class="right"><asp:DropDownList Width="300px" ID="GroupID" runat="server"></asp:DropDownList></li>
	</ul>
    
	<ul>
		<li class="left">管理员名：</li>
		<li class="right"><asp:TextBox CssClass="input" Width="300px" ID="Name" runat="server"></asp:TextBox>   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="不能为空" ControlToValidate="Name" Display="Dynamic"></asp:RequiredFieldValidator></li>
        </ul>
	<ul>
		<li class="left">电子邮箱：</li>
		<li class="right"><asp:TextBox CssClass="input" Width="300px" ID="Email" runat="server"></asp:TextBox>  <asp:RegularExpressionValidator
                ID="RegularExpressionValidator3" runat="server" ErrorMessage="请填写正确的E-mail地址" ControlToValidate="Email" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
        </li>
	</ul>	
<asp:PlaceHolder ID="Add" runat="server">
	<ul>
		<li class="left">密码：</li>
		<li class="right"><asp:TextBox CssClass="input" Width="300px" ID="Password" runat="server" TextMode="Password"></asp:TextBox>   
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="不能为空" ControlToValidate="Password" Display="Dynamic"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^[\W\w]{6,16}$" ErrorMessage="密码长度大于6位少于16位" ControlToValidate="Password" Display="Dynamic"></asp:RegularExpressionValidator></li>
	</ul>       
	<ul>
		<li class="left">重复密码：</li>
		<li class="right"><asp:TextBox CssClass="input" Width="300px" ID="Password2" runat="server" TextMode="Password"></asp:TextBox>  
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="不能为空" ControlToValidate="Password2" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="两次密码不一致" ControlToCompare="Password" ControlToValidate="Password2" Display="Dynamic"></asp:CompareValidator>
        </li>
	</ul>
</asp:PlaceHolder>
</div>
<div class="action">
    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
</div>
</asp:Content>
