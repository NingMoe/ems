<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="TestPaper.aspx.cs" Inherits="XueFuShop.Admin.TestPaper" %>
<%@ Import Namespace="XueFuShop.BLL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<link type="text/css" href="Style/jquery.bigautocomplete.css" rel="stylesheet" />
<script type="text/javascript" src="Js/jquery.1.12.4.min.js"></script>
<script type="text/javascript" src="Js/jquery.bigautocomplete.js"></script><script type="text/javascript" src="My97DatePicker/WdatePicker.js"></script><div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>成绩列表</div>	
<ul class="search">    <li>
        公司：<asp:TextBox ID="CompanyName" Width="200px" name="CompanyName" CssClass="input" runat="server"> </asp:TextBox>        
        用户名：<asp:TextBox ID="UserName" CssClass="input" runat="server"></asp:TextBox>
        姓名：<asp:TextBox ID="RealName" CssClass="input" runat="server"></asp:TextBox>
        课程名称：<asp:TextBox ID="CourseName" CssClass="input" runat="server"></asp:TextBox>                <%--学习岗位：<asp:DropDownList ID="StudyPostID" runat="server"></asp:DropDownList>--%>
        时间：<asp:TextBox CssClass="input Wdate" ID="SearchStartDate" TabIndex="2" runat="server" Width="80px" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd',readOnly:true,maxDate:'#F{$dp.$D(globalIDPrefix+\'SearchEndDate\')}'})"></asp:TextBox> -- <asp:TextBox CssClass="input Wdate" ID="SearchEndDate" runat="server" Width="80px" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd',readOnly:true,minDate:'#F{$dp.$D(globalIDPrefix+\'SearchStartDate\')}',startDate:'#F{$dp.$D(globalIDPrefix+\'SearchStartDate\')}'})"></asp:TextBox>
        状态：<asp:DropDownList ID="IsPass" runat="server"><asp:ListItem Value="">全部</asp:ListItem><asp:ListItem Value="1">通过</asp:ListItem><asp:ListItem Value="0">未通过</asp:ListItem></asp:DropDownList>        <asp:Button CssClass="button" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" />    </li></ul>
<table class="listTable" cellpadding="0">    <tr class="listTableHead">	    <td style="width:7%">选择ID</td>	    <td style="width:30%; text-align:left;text-indent:8px;">课程名称</td>	    <td style="width:5%">分数</td>	    <td style="width:10%">用户名</td>	    <td style="width:10%">学习岗位</td>	    <td style="width:15%">公司名称</td>	    <td style="width:15%">考试时间</td>	    <td>管理</td>                  </tr>    <asp:Repeater ID="RecordList" runat="server">	<ItemTemplate>	     	<tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">	    <td><input type="checkbox" name="SelectID" value="<%# Eval("TestPaperId") %>" /></td>	    <td style=" text-align:left;text-indent:8px;"><%# Eval("PaperName") %></td>	    <td><%# Eval("Scorse")%></td>        <td><%# GetRealName(Convert.ToInt32(Eval("UserId"))).RealName%></td>        <td><%# PostBLL.ReadPost(GetRealName(Convert.ToInt32(Eval("UserId"))).StudyPostID).PostName%></td>        <td><%# GetCompanyName(Convert.ToInt32(Eval("CompanyId")))%></td>        <td><%# Eval("TestDate")%></td>	    <td>			    	       <!-- <a href="javascript:pop('QuestionAdd.aspx?ID=<%#Eval("QuestionId")%>',600,600,'修改信息','CompanyAdd<%# Eval("QuestionId") %>')"><img src="Style/Images/edit.gif" alt="修改信息" title="修改密码" /></a> -->	        <%if (deleteTestPaperPower)
             {%>             <a href='?Action=Delete&ID=<%# Eval("TestPaperId") %>' onclick="return check()"><img src="Style/Images/delete.gif" alt="删除" title="删除" /></a>            <%} %>	    </td>            </tr>    </ItemTemplate>    </asp:Repeater></table><div class="listPage"><XueFu:CommonPager ID="MyPager" runat="server" /></div>
<div class="action">    <input type="button"  value=" 删 除 " class="button"  id="Button1" onserverclick="Button1_ServerClick" runat="server"/>&nbsp;    <input type="checkbox" name="All" onclick="selectAll(this)" />全选/取消</div>
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
