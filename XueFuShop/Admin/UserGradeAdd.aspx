﻿<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="UserGradeAdd.aspx.cs" Inherits="XueFuShop.Admin.UserGradeAdd" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="position"><img src="Style/Images/PositionIcon.png"  alt=""/>用户等级<%=GetAddUpdate()%></div>
        ID="RequiredFieldValidator1" runat="server" ErrorMessage="不能为空" ControlToValidate="Name" Display="Dynamic"></asp:RequiredFieldValidator></li>
        ID="RequiredFieldValidator2" runat="server" ErrorMessage="不能为空" ControlToValidate="Discount" Display="Dynamic"></asp:RequiredFieldValidator></li>
        ID="RequiredFieldValidator3" runat="server" ErrorMessage="不能为空" ControlToValidate="MinMoney" Display="Dynamic"></asp:RequiredFieldValidator></li>
        ID="RequiredFieldValidator4" runat="server" ErrorMessage="不能为空" ControlToValidate="MaxMoney" Display="Dynamic"></asp:RequiredFieldValidator></li>
    <asp:TextBox ID="Hit" runat="server" Visible="False"></asp:TextBox>
</asp:Content>