<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="CourseAdd.aspx.cs" Inherits="XueFuShop.Admin.CourseAdd" Title="无标题页" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>题库<%=GetAddUpdate()%></div>
<div class="add">
    <%--<ul id="CompanyIdOption" runat="server">
		<li class="left">公司名称：</li>
		<li class="right"><asp:DropDownList ID="CompanyId" runat="server" OnSelectedIndexChanged="CompanyId_SelectedIndexChanged">
            <asp:ListItem></asp:ListItem>
        </asp:DropDownList></li>
	</ul>--%>
    <ul id="GroupBrand">
		<li class="left">课程类别：</li>
		<li class="right"><asp:DropDownList ID="CateId" Width="300px" runat="server"></asp:DropDownList></li>
	</ul>
	<ul>
		<li class="left">课程名称：</li>
		<li class="right"><XueFu:TextBox CssClass="input" Width="300px" ID="CourseName" CanBeNull="必填" runat="server"/></li>
	</ul>
	<%--<ul>
		<li class="left">课程编号：</li>
		<li class="right"><XueFu:TextBox CssClass="input" Width="100px" ID="CourseCode" CanBeNull="必填" runat="server"/></li>
	</ul>--%>
	<ul>
		<li class="left">排序：</li>
		<li class="right"><XueFu:TextBox CssClass="input" Width="300px" ID="OrderIndex" CanBeNull="必填" RequiredFieldType="数据校验" HintInfo="ID号为在同一分类中显示的顺序，ID越小越靠上。" runat="server"/></li>
	</ul>
    <ul><XueFu:Hint ID="Hint" runat="server"/></ul>
</div>
<div class="action">
    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
</div>
</asp:Content>
