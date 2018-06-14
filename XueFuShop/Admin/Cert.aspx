<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="Cert.aspx.cs" Inherits="XueFuShop.Admin.Cert" Title="无标题页" %>
<%@ Import Namespace="XueFuShop.BLL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<link type="text/css" href="Style/jquery.bigautocomplete.css" rel="stylesheet" />
<script type="text/javascript" src="Js/jquery.1.12.4.min.js"></script>
<script type="text/javascript" src="Js/jquery.bigautocomplete.js"></script><script type="text/javascript" src="My97DatePicker/WdatePicker.js"></script><div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>成绩列表</div>	
<ul class="search">    <li>
        公司：<asp:TextBox ID="CompanyName" Width="200px" name="CompanyName" CssClass="input" runat="server"> </asp:TextBox>        
        姓名：<asp:TextBox ID="SearchName" CssClass="input" runat="server"></asp:TextBox>
        岗位名称：<asp:TextBox ID="PostName" CssClass="input" runat="server"></asp:TextBox>                <%--学习岗位：<asp:DropDownList ID="StudyPostID" runat="server"></asp:DropDownList>--%>
        时间：<asp:TextBox CssClass="input Wdate" ID="SearchStartDate" TabIndex="2" runat="server" Width="80px" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd',readOnly:true,maxDate:'#F{$dp.$D(globalIDPrefix+\'SearchEndDate\')}'})"></asp:TextBox> -- <asp:TextBox CssClass="input Wdate" ID="SearchEndDate" runat="server" Width="80px" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd',readOnly:true,minDate:'#F{$dp.$D(globalIDPrefix+\'SearchStartDate\')}',startDate:'#F{$dp.$D(globalIDPrefix+\'SearchStartDate\')}'})"></asp:TextBox>        每页条数：<asp:TextBox ID="PageCount" CssClass="input" runat="server"></asp:TextBox>        <asp:Button CssClass="button" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" />    </li></ul>
<table class="listTable" cellpadding="0">    <tr class="listTableHead">	    <td style="width:30%;">公司简称</td>	    <td style="width:10%">姓名</td>	    <td style="width:30%">认证岗位</td>	    <td style="width:15%">日期</td>	    <td style="width:15%">岗位认证总数</td>    </tr>    <asp:Repeater ID="RecordList" runat="server">	<ItemTemplate>	     	<tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">	    <td><%# GetCompanyName(GetRealName(Convert.ToInt32(Eval("UserId"))).CompanyID)%></td>	    <td><%# GetRealName(Convert.ToInt32(Eval("UserId"))).RealName%></td>        <td><%# Eval("PostName") %></td>        <td><%# Convert.ToDateTime(Eval("CreateDate")).ToString("d")%></td>        <td><%# GetPostPassNum(Convert.ToInt32(Eval("UserId")))%></td>     </tr>    </ItemTemplate>    </asp:Repeater></table><div class="listPage"><XueFu:CommonPager ID="MyPager" runat="server" /></div>
<script type="text/javascript">
    $(function(){
	    document.onkeydown = function(event) {  
        var target, code, tag;  
        if (!event) {  
            event = window.event; //针对ie浏览器  
            target = event.srcElement;  
            code = event.keyCode;  
            if (code == 13) {  
                tag = target.tagName;  
                if (tag == "TEXTAREA") { return true; }  
                else { return false; }  
            }  
        }  
        else {  
            target = event.target; //针对遵循w3c标准的浏览器，如Firefox  
            code = event.keyCode;  
            if (code == 13) {  
                tag = target.tagName;  
                if (tag == "INPUT") { return false; }  
                else { return true; }  
            }  
        }  
    };
	
	$("#ctl00_ContentPlaceHolder_CompanyName").bigAutocomplete({
	    width:"304px",
		url:"Ajax.aspx?Action=SearchCompanyName",
		callback:function(data){
		}
	});
})
</script>
</asp:Content>