<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="PostAdd.aspx.cs" Inherits="XueFuShop.Admin.PostAdd" Title="无标题页" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<link rel="stylesheet" href="Style/jquery.bigautocomplete.css" type="text/css" />
<script type="text/javascript" src="Js/jquery.1.12.4.min.js"></script>
<script type="text/javascript" src="Js/jquery.bigautocomplete.js"></script><div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>岗位<%=GetAddUpdate()%></div>
<div class="add">	<ul>		<li class="left">公司名称：</li>		<li class="right">		    <input type="text" name="CompanyName" id="CompanyName" class="input companyname" value="<%=companyName %>" />		    <input type="hidden" name="CompanyId" id="CompanyId" value="<%=companyID %>" />		</li>	</ul>
	<ul>
		<li class="left">上级部门：</li>
		<li class="right"><asp:DropDownList ID="CateId" runat="server">
        </asp:DropDownList></li>
	</ul>
	<ul>
		<li class="left">岗位名称：</li>
		<li class="right"><XueFu:TextBox CssClass="input" Width="150px" ID="PostName" runat="server" CanBeNull="必填" /></li>
	</ul>
	<ul>
		<li class="left">排序：</li>
		<li class="right"><XueFu:TextBox CssClass="input" Width="150px" ID="OrderIndex" runat="server" HintInfo="ID号为在同一分类中显示的顺序，ID越小越靠上。" CanBeNull="必填" RequiredFieldType="数据校验" /></li>
	</ul>	
	<ul>
		<li class="left">是否岗位：</li>
		<li class="right"><asp:CheckBox ID="IsPost" Text="" runat="server" /></li>
	</ul>
	<ul><XueFu:Hint ID="Hint" runat="server"/></ul>
</div>
<div class="action">
    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
</div>
<script type="text/javascript">
$(function(){
    $(window).keydown(function (e) {
		if (e.which == 13) {
			return false;
		}
	})
	
	$("#CompanyName").bigAutocomplete({
	    width:"400px",
		url:"Ajax.aspx?Action=SearchCompanyName",
		callback:function(data){
		    $("#CompanyId").val(data.result.Id);
		    $.post("Ajax.aspx?Action=GetPostListJson",{CompanyID:data.result.Id},function(data){
	            data=data.data;
	            $('#'+globalIDPrefix+'CateId').html("<option value='0'>设置为新部门</option>")
	            for(i=0;i<data.length;i++)
	            {
	                $('#'+globalIDPrefix+'CateId').append("<option value='"+data[i].ID+"'>"+data[i].Name+"</option>");
	            }
	        },"json");
		}
	});
})
</script>
</asp:Content>
