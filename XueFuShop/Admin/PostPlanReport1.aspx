<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="PostPlanReport1.aspx.cs" Inherits="XueFuShop.Admin.PostPlanReport1" Title="无标题页" %>
<%@ Import Namespace="XueFu.EntLib" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<link rel="stylesheet" href="Style/jquery.bigautocomplete.css" type="text/css" />
<script type="text/javascript" src="js/jquery.1.12.4.min.js"></script><script type="text/javascript" src="My97DatePicker/WdatePicker.js"></script>
<script type="text/javascript" src="js/jquery.bigautocomplete.js"></script>
<script type="text/javascript" src="js/stupidtable.min.js"></script><div class="position"><img src="Style/Images/PositionIcon.png"  alt=""/>做报告所用数据</div><table id="StudyPost" class="searchTable checkbox-li" width="100%" border="0" align="center" cellpadding="0" cellspacing="0">    <tr>
        <td class="left">公司名称</td>
        <td style=" text-align: left; padding-left:5px;">
            <input type="text" class="input" name="CompanyName" id="CompanyName" value="<%=companyName %>" style="width:320px;" autocomplete="off"/>
            <input type="hidden" name="CompanyID" id="CompanyID" value="<%=companyID %>" />                    </td>    </tr>    <tr>
        <td class="left">时间段</td>
        <td style=" text-align: left; padding-left:5px;">            <asp:TextBox CssClass="input Wdate" ID="SearchStartDate" TabIndex="2" runat="server" Width="80px" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd',readOnly:true,maxDate:'#F{$dp.$D(globalIDPrefix+\'SearchEndDate\')}'})"></asp:TextBox> -- <asp:TextBox CssClass="input Wdate" ID="SearchEndDate" runat="server" Width="80px" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd',readOnly:true,minDate:'#F{$dp.$D(globalIDPrefix+\'SearchStartDate\')}',startDate:'#F{$dp.$D(globalIDPrefix+\'SearchStartDate\',{d:+1})}'})"></asp:TextBox>        </td>    </tr>    <tr>
        <td class="left">状态</td>
        <td>
            <ul class="multiple" data-action="State">
                <li data-value="<%=(int)UserState.Normal %>"><span>正常</span></li>
                <li data-value="<%=(int)UserState.Free %>"><span>不考核</span></li>
                <li data-value="<%=(int)UserState.Other %>"><span>其他</span></li>
                <li data-value="<%=(int)UserState.Frozen %>"><span>冻结</span></li>
                <li data-value="<%=(int)UserState.Del %>"><span>删除</span></li>
            </ul>        </td>    </tr>    <tr<%if(string.IsNullOrEmpty(action)){ %> class="hidden"<%} %>>
        <td class="left">员工组别</td>
        <td>            <ul ID="GroupList" class="multiple" data-action="GroupID">
            </ul>        </td>    </tr>    <tr>
        <td class="left">证书情况</td>
        <td>
            <ul class="single" data-action="RZ">
                <li data-value="<%=int.MinValue %>" class="CheckBorder"><span>所有</span><b>已选</b></li>
                <li data-value="0"><span>没有证书</span></li>
                <li data-value="1"><span>有证书</span></li>
            </ul>        </td>    </tr>    <tr>
        <td class="left">获取证书/已学习考试未通过/已通过数量<br>时间选择</td>
        <td>
            <ul class="single" data-action="RZDate">
                <li data-value="0" class="CheckBorder"><span>在时间段内</span><b>已选</b></li>
                <li data-value="1"><span>从开始到截止日期之间</span></li>
            </ul>        </td>    </tr>    <tr<%if(string.IsNullOrEmpty(action)){ %> class="hidden"<%} %>>
        <td class="left">学习岗位  <br><a href="javascript:SelectAll('#StudyPostNameList');">[反选]</a></td>
        <td>
	        <ul id="StudyPostNameList" class="multiple" data-action="StudyPostID">
            </ul>        </td>    </tr>    <tr<%if(string.IsNullOrEmpty(action)){ %> class="hidden"<%} %>>
        <td class="left">工作岗位 <br><a href="javascript:SelectAll('#PostNameList');">[反选]</a></td>
        <td>
            <ul id="PostNameList" class="multiple" data-action="WorkingPostID">	
            </ul>        </td>    </tr>    <tr>
        <td class="left" colspan="2">            <input type="hidden" name="State" id="State" value="<%=state %>" />            <input type="hidden" name="GroupID" id="GroupID" value="<%=groupID %>" />            <input type="hidden" name="RZ" id="RZ" value="<%=rz %>" />            <input type="hidden" name="RZDate" id="RZDate" value="<%=rzDate %>" />            <input type="hidden" name="StudyPostID" id="StudyPostID" value="<%=studyPostIdCondition %>" />            <input type="hidden" name="WorkingPostID" id="WorkingPostID" value="<%=postIdCondition %>" />            <asp:Button CssClass="button" ID="SearchButton" Text=" 搜 索 " runat="server" OnClick="SearchButton_Click" />        </td>    </tr></table>
<div  style="display:none;" id="ShowArea" runat="server">                    <div id="ReportList" runat="server" style="width:100%; overflow-x:auto;"></div>
</div>
<script language="javascript" type="text/javascript">
function SelectAll(id)
{
    if($(id).hasClass("CheckBorder"))
    {
        $(id).find("b").remove();
        $(id).removeClass("CheckBorder");
    }
    else
    {
        $(id).append("<b>选中</b>");
        $(id).addClass("CheckBorder");
    }
}

$(function(){
	$(".checkbox-li").on("click", ".multiple li", function() {
	    if($(this).hasClass("CheckBorder"))
	    {
		    $(this).removeClass("CheckBorder").find("b").remove();
		}
		else
		{
		    $(this).addClass("CheckBorder").append("<b>选中</b>");
		}
		
		var id = $(this).parent().data("action");
		var valueStr="";
		$(this).parent().find(".CheckBorder").each(function(){
		    valueStr = valueStr + "," + $(this).data("value");
		});
		$("#" + id).val(valueStr.substr(1));
	});
	
	$(".checkbox-li").on("click", ".single li", function() {
	    if(!$(this).hasClass("CheckBorder"))
	    {
	        $(this).siblings().removeClass("CheckBorder").find("b").remove();
		    $(this).addClass("CheckBorder").append("<b>选中</b>");
		
		    var id = $(this).parent().data("action");
		    $("#"+id).val($(this).data("value"));
		}
	});

    $(window).keydown(function (e) {
		if (e.which == 13) {
			return false;
		}
	})
	
	var loadChecked = function(objid){
        var checkedValue = $("#"+objid).val();
        if(checkedValue != "" && checkedValue != null){
            $.each(checkedValue.split(','),function(i,item){
                $("[data-action='"+objid+"']").find("[data-value='"+item+"']").click();
            })
        }
        else{
            $("[data-action='"+objid+"'] li").click();
            $("#"+objid).val('');
        }
    }
	
	var loaddata = function(companyID){
        $.post("Ajax.aspx?Action=GetUserGroupJson",{CompanyID:companyID},function(data){
            $("#GroupList").html('');
		    data=JSON.parse(data);
		    $.each(data.data, function(i, item){
                $("#GroupList").append("<li data-value=\"" + item.ID + "\"><span>" + item.Name + "</span></li>")
            });
            loadChecked("GroupID");
            $("#GroupList").parent().parent().slideDown("slow");
		})
		$.post("Ajax.aspx?Action=GetStudyPostJson",{CompanyID:companyID},function(data){
		    $("#StudyPostNameList,#PostNameList").html('');
		    data=JSON.parse(data);
		    $.each(data.data, function(i, item){
                $("#StudyPostNameList,#PostNameList").append("<li data-value=\"" + item.ID + "\"><span>" + item.Name + "</span></li>")
            });
            loadChecked("StudyPostID");
            loadChecked("WorkingPostID");
		    $("#StudyPostNameList,#PostNameList").parent().parent().slideDown("slow");
		})
    }
	
	$("#CompanyName").bigAutocomplete({
	    width:"320px",
		url:"Ajax.aspx?Action=SearchCompanyName",
		callback:function(data){
		    $("#CompanyID").val(data.result.Id);
		    loaddata(data.result.Id);
		}
	});
	
	if(parseInt($("#CompanyID").val()) > 0){
	    loaddata($("#CompanyID").val());
	}
	loadChecked("State");
	loadChecked("RZDate");
	loadChecked("RZ");
	loadChecked("Del");
    
    $("[data-sort]").closest("table").stupidtable();
})
var SelectAll = function(checkBoxID){
    $(checkBoxID).find("li").click();
}
</script>
</asp:Content>
