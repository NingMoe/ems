<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="Course.aspx.cs" Inherits="XueFuShop.Admin.Course" Title="无标题页" %>
<%@ Import Namespace="XueFuShop.BLL" %><%@ Import Namespace="XueFuShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<script language="javascript" src="js/jquery.1.12.4.min.js" type="text/javascript"></script>
<div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>题库列表</div>
<ul class="search">    <li>
        分类：<asp:DropDownList ID="SearchCategory" runat="server"></asp:DropDownList>        课程名称：<XueFu:TextBox ID="CourseName" CssClass="input" runat="server"></XueFu:TextBox>        <asp:Button CssClass="button" ID="SearchButton" Text=" 搜 索 " runat="server"  OnClick="SearchButton_Click" />    </li></ul>
<table class="listTable" cellpadding="0">    <tr class="listTableHead">	    <td style="width:8%">选择</td>	    	    <td style="width:40%; text-align:left;text-indent:8px;">课程名称</td>	    <td style="width:15%">试题数量</td>	    <td style="width:15%">分类</td>	    <td>操作</td>                  </tr><asp:Repeater ID="RecordList" runat="server">	<ItemTemplate>	             	<tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">			    <td><input type="checkbox" name="SelectID" value="<%# Eval("CourseId") %>" /><%//# Eval("CourseId")%></td>			    			    <td style=" text-align:left;text-indent:8px;"><%# Eval("CourseName") %></td>			    <td><%# QuestionBLL.ReadList(int.Parse(Eval("CourseId").ToString())).Count.ToString()%></td>	            <td><%# CourseCateBLL.ReadCourseCateInfo(int.Parse(Eval("CateId").ToString())).CateName%></td>			    <td id="<%# Eval("CompanyId") %>">				      <%# base.CompareAdminPower("AddQuestion", PowerCheckType.Single) ? "<a href=\"javascript:pop(\'QuestionXls.aspx?CateId=" + Eval("CourseId") + "&Action=CourseFileIn\',800,500,\'导入考题\',\'QuestionIn" + Eval("CourseId") + "\')\"  title=\"导出考题\">导入</a> <a href=\"javascript:pop(\'QuestionXls.aspx?CateId=" + Eval("CourseId") + "&Action=CourseFileOut\',800,300,\'导出考题\',\'QuestionAdd" + Eval("CourseId") + "\')\" title=\"导出考题\">导出</a> <a href=\"javascript:pop(\'QuestionAdd.aspx?CourseId=" + Eval("CourseId") + "&CateId=" + Eval("CateId") + "\',800,600,\'添加考题\',\'QuestionAdd" + Eval("CourseId") + "\')\">添加考题</a>" : ""%>    			      <%# base.CompareAdminPower("UpdateCourse", PowerCheckType.Single) ? "<a href=\"javascript:pop(\'CourseAdd.aspx?ID=" + Eval("CourseId") + "\',700,300,\'修改信息\',\'CompanyAdd" + Eval("CourseId") + "\')\"><img src=\"Style/Images/edit.gif\" alt=\"修改信息\" title=\"修改信息\" /></a>" : ""%>			      <%# base.CompareAdminPower("DeleteCourse", PowerCheckType.Single) ? "<a href=\'Course.aspx?Action=Delete&ID=" + Eval("CourseID") + "\' onclick=\"return check()\"><img src=\"Style/Images/delete.gif\" alt=\"删除\" title=\"删除\" /></a>" : ""%>			    </td>		    </tr>        </ItemTemplate></asp:Repeater></table><div class="listPage"><XueFu:CommonPager ID="MyPager" runat="server" /></div>
<%if (CompareAdminPower("AddCourseCate", PowerCheckType.Single))
  {%><div class="action">    <input type="hidden" name="CheckBoxValue" id="CheckBoxValue" value="" />    <input type="button"  value=" 添 加 " class="button" onclick="pop('CourseAdd.aspx',700,300,'题库添加','CourseAdd')" id="Button1"/>
    <asp:Button  class="button" ID="Button2" runat="server" Text="删除" OnClick="Button2_Click" />    <input type="button"  class="button" ID="BatchOut" value="导出试题" />    <input type="button"  class="button" ID="BatchMove" value="批量移动" />    <input type="checkbox" name="All" onclick="selectAll(this)" />全选/取消</div>
<script language="javascript">
$(function(){
    $("[name='SelectID'],[name='All']").click(function(){
        var checkboxvalue='';
        $("input[name='SelectID']:checked").each(function(){
                checkboxvalue += ',' + $(this).attr('value');
        }); 
        $("#CheckBoxValue").val(checkboxvalue.substr(1));
    });
    
    $("#BatchOut").click(function(){
        if($("#CheckBoxValue").val() != ''){
            pop('QuestionXls.aspx?CourseID='+$("#CheckBoxValue").val()+'&Action=BatchFileOut',700,300,'批量导出','CourseAdd');
        }
    })
    
    $("#BatchMove").click(function(){
        if($("#CheckBoxValue").val() != ''){
            pop('CourseMove.aspx?CourseID='+$("#CheckBoxValue").val()+'',700,200,'课程分类移动','CourseMove');
        }
    })
})
</script>
<%} %>
</asp:Content>
