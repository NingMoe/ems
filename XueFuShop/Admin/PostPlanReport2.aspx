<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="PostPlanReport2.aspx.cs" Inherits="XueFuShop.Admin.PostPlanReport2" Title="无标题页" %>
<%@ Import Namespace="XueFu.EntLib" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<link rel="stylesheet" href="Style/jquery.bigautocomplete.css" type="text/css" />
<script type="text/javascript" src="http://cdn.bootcss.com/jquery/1.12.1/jquery.min.js"></script>
<script type="text/javascript" src="Js/jquery.bigautocomplete.js"></script><script type="text/javascript" src="My97DatePicker/WdatePicker.js"></script>
<script type="text/javascript" src="js/stupidtable.min.js"></script><div class="position"><img src="Style/Images/PositionIcon.png"  alt=""/>最小学习量达成分析表</div>
<table class="searchTable checkbox-li">    <tr>
        <td class="left">公司名称</td>
        <td>            <input type="text" name="CompanyName" id="CompanyName" value="<%=companyName %>" size="55"  Class="input companyname" />        </td>    </tr>    <tr>
        <td class="left">时间段</td>
        <td>            <asp:TextBox CssClass="input Wdate" ID="SearchStartDate" TabIndex="2" runat="server" Width="80px" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd',readOnly:true,maxDate:'#F{$dp.$D(globalIDPrefix+\'SearchEndDate\')}'})"></asp:TextBox> -- <asp:TextBox CssClass="input Wdate" ID="SearchEndDate" runat="server" Width="80px" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd',readOnly:true,minDate:'#F{$dp.$D(globalIDPrefix+\'SearchStartDate\')}',startDate:'#F{$dp.$D(globalIDPrefix+\'SearchStartDate\',{d:+6})}'})"></asp:TextBox>            学习量达标情况以周为单位，请以周一开始周日结束        </td>    </tr>    <tr>
        <td style="text-align:center;">状态</td>
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
            </ul>        </td>    </tr>    <tr<%if(string.IsNullOrEmpty(action)){ %> class="hidden"<%} %>>
        <td class="left">学习岗位  <br><a href="javascript:SelectAll('#StudyPostNameList');">[反选]</a></td>
        <td>
	        <ul id="StudyPostNameList" class="multiple" data-action="StudyPostIdCondition">
            </ul>        </td>    </tr>    <tr<%if(string.IsNullOrEmpty(action)){ %> class="hidden"<%} %>>
        <td class="left">工作岗位 <br><a href="javascript:SelectAll('#PostNameList');">[反选]</a></td>
        <td>
            <ul id="PostNameList" class="multiple" data-action="PostIdCondition">	
            </ul>        </td>    </tr>    <tr>
        <td class="left" colspan="2">            <asp:Button CssClass="button" ID="SearchButton" Text=" 搜 索 " runat="server" OnClick="SearchButton_Click" />            <input type="hidden" name="CompanyID" id="CompanyID" value="<%=companyID.ToString() %>" />            <input type="hidden" name="StudyPostIdCondition" id="StudyPostIdCondition" value="<%=studyPostIdCondition %>" />            <input type="hidden" name="PostIdCondition" id="PostIdCondition" value="<%=postIdCondition %>" />            <input type="hidden" name="GroupID" id="GroupID" value="<%=groupID %>" />            <input type="hidden" name="State" id="State" value="<%=state %>" />        </td>    </tr></table>

<div  style="display:none;" id="ShowArea" runat="server"><div id="ReportList" runat="server" style="width:100%; overflow-x:auto;"></div>
</div>

<script type="text/javascript">
    if (typeof JSON == 'undefined') {
        $('head').append($("<script type='text/javascript' src='js/json2.js'>"));
    }
$(function(){
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
            loadChecked("StudyPostIdCondition");
            loadChecked("PostIdCondition");
		    $("#StudyPostNameList,#PostNameList").parent().parent().slideDown("slow");
		})
    }
    

	$("#CompanyName").bigAutocomplete({
	    width:"304px",
		url:"Ajax.aspx?Action=SearchCompanyName",
		callback:function(data){
		    var result=data.result;
    		$("#CompanyID").val(result.Id);
    		loaddata(result.Id);
		}
	});
	
	$("#ctl00_ContentPlaceHolder_SearchButton").click(function(){
	    var startDate = $("#ctl00_ContentPlaceHolder_SearchStartDate").val();
	    var endDate = $("#ctl00_ContentPlaceHolder_SearchEndDate").val();
	    if(parseInt($("#CompanyID").val()) <= 0) {
	        alert("公司数据有误，请重新选择公司");
	        return false;
	    }
	    if(startDate == "" || endDate =="" ){
	        alert("时间段不正确");
	        return false;
	    }
	    return true;
	})
	
	
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
	
	$("[data-sort]").closest("table").stupidtable();
})

var SelectAll = function(checkBoxID){
    $(checkBoxID).find("li").click();
}

function preview(tableid) {
    copyToClipboard(document.all(tableid).outerHTML.replace(/<br>/g,"").replace(/<td/g,"<td style=text-align:center;"));   
    try{
        var ExApp = new ActiveXObject("Excel.Application")   
        var ExWBk = ExApp.workbooks.add()   
        var ExWSh = ExWBk.worksheets(1)   
        ExApp.DisplayAlerts = false  
        ExApp.visible = true  
    }     
    catch(e){
        alert("请使用IE浏览器，或者浏览器的兼容模式");   
        return false  
    }    
    ExWBk.worksheets(1).Paste;    
}

function copyToClipboard(txt) {  
    if (window.clipboardData) {  
        window.clipboardData.clearData();  
        window.clipboardData.setData("Text", txt);
        alert("复制成功！")  
    } else if (navigator.userAgent.indexOf("Opera") != -1) {  
        window.location = txt;  
        alert("复制成功！");  
    } else if (window.netscape) {  
        try {  
            netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");  
        } catch (e) {  
            alert("被浏览器拒绝！\n请在浏览器地址栏输入'about:config'并回车\n然后将 'signed.applets.codebase_principal_support'设置为'true'");  
        }  
        var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);  
        if (!clip)  
            return;  
        var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);  
        if (!trans)  
            return;  
        trans.addDataFlavor('text/unicode');  
        var str = new Object();  
        var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);  
        var copytext = txt;  
        str.data = copytext;  
        trans.setTransferData("text/unicode", str, copytext.length * 2);  
        var clipid = Components.interfaces.nsIClipboard;  
        if (!clip)  
            return false;  
        clip.setData(trans, null, clipid.kGlobalClipboard);  
        alert("复制成功！")  
    }
//else if(copy){  
//        copy(txt);  
//        alert("复制成功！")  
//    }  
}  
</script>
</asp:Content>

