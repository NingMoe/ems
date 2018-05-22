<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="WorkingPost.aspx.cs" Inherits="XueFuShop.Admin.WorkingPost" Title="无标题页" %>
<%@ Import Namespace="XueFuShop.BLL" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<link rel="stylesheet" href="Style/jquery.bigautocomplete.css" type="text/css" />
<script type="text/javascript" src="Js/jquery.1.12.4.min.js"></script>
<script type="text/javascript" src="Js/jquery.bigautocomplete.js"></script>
<div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>工作岗位列表</div>	
<div id="copylayer" style="display:none; margin:20px;">
    公司名称：<input type="text" name="TargetCompanyName" id="TargetCompanyName" class="input companyname" value="" />
</div><ul class="search">    <li>        公司名称：<input type="text" name="CompanyName" id="CompanyName" class="input companyname" value=""  runat="server" />
		岗位名称：<asp:TextBox CssClass="input" ID="Name" runat="server" Width="150px"></asp:TextBox>		<asp:Button CssClass="button" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" />		<input type="hidden" name="CompanyID" id="CompanyID" value="<%=CompanyID.ToString() %>" />   </li></ul>
<%if (Action == "Search") 
{%>
<table class="listTable" cellpadding="0">    <tr class="listTableHead">	    <td style="width:5%">ID</td>	    <td style="width:50%;">岗位名称</td>	    <td>操作</td></tr><asp:Repeater ID="RecordList" runat="server">	<ItemTemplate>	             <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">            <td><input type="checkbox" name="SelectID" title="<%# Eval("ID")%>" value="<%# Eval("ID")%>" /></td>
			<td style="text-align:left;"><%# !string.IsNullOrEmpty(Eval("Path").ToString())?"<span data-level=\""+Eval("Path").ToString().Split('|').Length+"\" class=\"icontree\">├</span>&nbsp;&nbsp;":""  %><%# Eval("PostName") %></td>            <td>
				<span class="handle">				    <a href="WorkingPostAdd.aspx?ID=<%# Eval("ID")%>&ReturnURL=<%# ServerHelper.UrlEncode(RequestHelper.RawUrl) %>"><img src="Style/Images/edit.gif" alt="修改" title="修改" /></a>				    <a href="javascript:if(confirm('确定要删除吗？')){window.location.href='?Action=Delete&ID=<%# Eval("ID")%>'}"><img src="Style/Images/delete.gif" alt="删除" title="删除" /></a>				</span>            </td>        </tr>        </ItemTemplate></asp:Repeater></table><div class="listPage"><XueFu:CommonPager ID="MyPager" runat="server" /></div><div class="action">    <input type="button"  value=" 添 加 " class="button"  onclick="pop('WorkingPostAdd.aspx',800,550,'岗位添加','WorkingPostAdd')"/>&nbsp;    <asp:Button CssClass="button" ID="DeleteButton" Text=" 删 除 " OnClientClick="return checkSelect()" runat="server"  OnClick="DeleteButton_Click"/>&nbsp;    <input type="button"  value=" 复 制 " class="button" id="copybutton" />&nbsp;    <input type="checkbox" name="All" onclick="selectAll(this)" />全选/取消</div>
<%} %>
<script type="text/javascript">
$(function(){
    $(".icontree").each(function (index, item) {
	    var level = $(this).data("level");
	    $(this).before("&nbsp;&nbsp;&nbsp;&nbsp;"); String.fromCharCode(9)
	});
    $("#" + globalIDPrefix + "CompanyName").bigAutocomplete({
	    width:"304px",
		url:"Ajax.aspx?Action=SearchCompanyName",
		callback:function(data){
    		$("#CompanyID").val(data.result.Id);
		}
    });
    $("#TargetCompanyName").bigAutocomplete({
        width: "304px",
        url: "Ajax.aspx?Action=SearchCompanyName",
        callback: function (data) {
            $("#CompanyID").val(data.result.Id);
        }
    });
    $("#copybutton").click(function () {
        var selectIDArray = new Array();
        $.each($('input:checkbox:checked'), function () {
            selectIDArray.push($(this).val());
        });
        var selectID = selectIDArray.toString();
        if (selectID == "") {
            layer.msg("请先选择岗位");
            return false;
        }
	    layer.open({
	        type: 1,
	        title: '选择复制到的目标',
	        closeBtn: 0,
	        shadeClose: true,
	        skin: 'yourclass',
	        content: $("#copylayer"),
	        btn: ['确认', '取消'],
	        zIndex:2,
	        yes: function (index, layero) {
	            var loadIndex = layer.load(1, { time: 2 * 1000, shade: 0.5 });
	            $.post("Ajax.aspx?Action=CopyPostList", { PostIDString: selectID, TargetCompanyID: $("#CompanyID").val() }, function (data) {
	                data = JSON.parse(data);
	                if (data.Success) {
	                    layer.msg("复制成功！");
	                }
	                else {
	                    layer.msg("复制失败！" + data.Msg);
	                }
	            })
	            layer.close(index);
	        }
	    });
	})
})
</script>
</asp:Content>
