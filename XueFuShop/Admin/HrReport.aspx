<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="HrReport.aspx.cs" Inherits="XueFuShop.Admin.HrReport" Title="无标题页" %>
<%@ Import Namespace="XueFu.EntLib" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<link rel="stylesheet" href="Style/jquery.bigautocomplete.css" type="text/css" />
<script type="text/javascript" src="http://cdn.bootcss.com/jquery/1.12.1/jquery.min.js"></script>
<script type="text/javascript" src="Js/jquery.bigautocomplete.js"></script><script type="text/javascript" src="My97DatePicker/WdatePicker.js"></script><div class="position"><img src="Style/Images/PositionIcon.png"  alt=""/>HR专用报表</div>
<style>
.style1 {
    font-size: 26px;
    background: #95f39b;
}
.style2 {
    font-size: 26px;
    background: #DA70D6;
}
.style3 {
    font-size: 26px;
    background: #f0f395;
}
</style><table class="searchTable checkbox-li">    <tr>
        <td class="left">选择公司</td>
        <td>            <input type="text" name="CompanyName" id="CompanyName" value="" size="55"  Class="input companyname"  runat="server" />            <input type="hidden" name="CompanyID" id="CompanyID" value="<%=companyID.ToString() %>" />
        </td>
    </tr>    <tr>
        <td class="left">截至时间</td>
        <td>
            <asp:TextBox CssClass="input Wdate" ID="SearchEndDate" runat="server" Width="85px" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd',readOnly:true})"></asp:TextBox>
        </td>
    </tr>    <tr<%if(string.IsNullOrEmpty(action)){ %> class="hidden"<%} %>>
        <td class="left">工作岗位 <br><a href="javascript:SelectAll('#PostNameList');">[反选]</a></td>
        <td>            <ul id="PostNameList" class="multiple" data-action="PostIdCondition">	
            </ul>        </td>    </tr>    <tr>
        <td style="text-align:center;">员工状态</td>
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
        <td class="left">岗位通过状态</td>
        <td>
            <ul ID="PassStateList" class="multiple" data-action="PassState">	
                <li id="style1" data-value="1"><span>绿灯</span></li>
                <li id="style2" data-value="2"><span>紫灯</span></li>
                <li id="style3" data-value="3"><span>黄灯</span></li>
            </ul>        </td>    </tr>    <tr>
        <td class="left" colspan="2">            <asp:Button CssClass="button" ID="SearchButton" Text=" 搜 索 " runat="server" OnClick="SearchButton_Click" />            <input type="hidden" name="PostIdCondition" id="PostIdCondition" value="<%=postIdCondition %>" />            <input type="hidden" name="GroupID" id="GroupID" value="<%=groupID %>" />            <input type="hidden" name="State" id="State" value="<%=state %>" />            <input type="hidden" name="PassState" id="PassState" value="<%=passState %>" />        </td>    </tr></table>
<%if(action=="search")   {%><div id="ReportList" runat="server" style="overflow-x:auto;"></div>
<%} %>
<script type="text/javascript">
$(document).ready(function() {
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
            loadChecked("PostIdCondition");
		    $("#StudyPostNameList,#PostNameList").parent().parent().slideDown("slow");
		})
    }

    $("#ctl00_ContentPlaceHolder_CompanyName").bigAutocomplete({
	    width:"304px",
		url:"Ajax.aspx?Action=SearchCompanyName",
		callback:function(data){
		    var result=data.result;
    		$("#CompanyID").val(result.Id);
    		loaddata(result.Id);
		}
	});
	
	$(".checkbox-li").on("click", ".multiple li", function(){
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
	
	if(parseInt($("#CompanyID").val()) > 0){
	    loaddata($("#CompanyID").val());
	}
	loadChecked("State");
	loadChecked("PassState");

	$(".checkbox-li").on("click", "#PassStateList li", function() {
		$("tr[data-style='data']").css("display","none");
		$("#PassStateList li.CheckBorder").each(function(){$("td."+$(this).attr("id")).parent().css("display","");});
	});
});

var SelectAll = function(checkBoxID){
    $(checkBoxID).find("li").click();
}
</script>
</asp:Content>
