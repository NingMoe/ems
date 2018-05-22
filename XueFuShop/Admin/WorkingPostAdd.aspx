<%@ Page Language="C#" MasterPageFile="MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="WorkingPostAdd.aspx.cs" Inherits="XueFuShop.Admin.WorkingPostAdd" Title="无标题页" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<style>
.indicator_name { background: #fff !important;}
.evaluation_content { text-align: left !important; padding-left: 10px;}
tr.active {  background: #FFFDD7 !important; }
.preset_post li{
display: inline-block;
padding: 5px 10px;
margin: 5px;
border:#A5D5F5 1px solid;
cursor: pointer;
}
.preset_post li.active { background: #A5D5F5;}
</style>
<link rel="stylesheet" href="Style/jquery.bigautocomplete.css" type="text/css" />
<script type="text/javascript" src="Js/jquery.1.12.4.min.js"></script>
<script type="text/javascript" src="Js/jquery.bigautocomplete.js"></script>
<div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>职位<%=GetAddUpdate()%></div>
<div class="add" >		<ul>		<li class="left">公司名称：</li>		<li class="right">		    <input type="text" name="CompanyName" id="CompanyName" class="input companyname" value="" runat="server" />		    <input type="hidden" name="CompanyId" id="CompanyId" value="<%=_companyId %>" />		</li>	</ul>		<ul>		<li class="left">上级部门：</li>		<li class="right"><asp:DropDownList class="form_select" ID="ParentId" runat="server"></asp:DropDownList></li>	</ul>	<ul>		<li class="left">职位名称：</li>		<li class="right"><XueFu:TextBox ID="PostName" CssClass="input" CanBeNull="必填" runat="server" Width="200px" /></li>	</ul>		<ul>		<li class="left">属性：</li>		<li class="right">            <input type="checkbox" name="IsPostCheck" value="0"<%=(workingPost.IsPost ==0 ? " checked":"") %>>作为部门
            <input type="checkbox" name="IsPostCheck" value="1"<%=(workingPost.IsPost ==1 ? " checked":"") %>>作为岗位
		</li>	</ul>	<ul><XueFu:Hint ID="Hint" runat="server"/></ul></div><div id="preset_indicator" style="display: none;">
    <p style="color: Red; font-size:14px; line-height:30px; font-weight:bold;">说明：若选择相关岗位，则所填岗位将按勾选岗位指标进行预设</p>
	<ul class="preset_post">
		<%=GetPostHtml() %>
	</ul>
</div><table class="listTable" style="display: none;">
	<tr class="listTableHead">
		<td style="width:5%">指标类</td>
		<td style="width:10%">核定担当</td>
		<td style="width:50%">评估项目内容</td>
		<td style="width:10%">隶属公司</td>
		<td style="width:5%">设置</td>
	</tr>
	<tbody id="KPIListArea" data-typeid="0">
        <%=GetTrHtml(tempList1) %>
        <%=GetTrHtml(tempList2) %>
        <%=GetTrHtml(tempList3) %>
    </tbody>
</table><div class="action">    <input class="button" style="display: none;" type="button" name="prev" id="prev" value="上一步"></input>	<asp:Button CssClass="button" ID="next" Text="保存提交" runat="server"  OnClick="SubmitButton_Click" />    <input id="kpiidstr" name="kpiidstr" type="hidden" value="<%=kpiTemplet.KPIContent %>"></input></div>
<script type="text/javascript" src="Js/WorkingPostAdd.js"></script>
</asp:Content>
