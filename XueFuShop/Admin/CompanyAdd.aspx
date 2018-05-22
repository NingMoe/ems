<%@ Page Language="C#" enableEventValidation="false" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="CompanyAdd.aspx.cs" Inherits="XueFuShop.Admin.CompanyAdd" Title="无标题页" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server"><style type="text/css">
.GroupList {}
.GroupList li{margin-top:10px; padding:0px 5px; width:210px; text-align:left; height:28px; line-height:28px; border-bottom:1px solid #f3f3f3;}
.GroupList a{float:right; cursor:pointer; margin-top:0px; padding:5px; width:10px;}
#AddGroupName {margin-top:10px; margin-bottom:10px;}
dl{ width:570px; text-align:left;}
dl dt { font-weight:bold; margin: 20px auto 5px 10px; }
.post { margin:10px; margin-top:0; border:1px dashed #7194DB;}
.post dd,.carbrand dd {margin:5px; width:125px; display:inline-table; *display: inline;}
.carbrand { width:550px; margin:10px; border:1px dashed #7194DB;}
</style><script language="javascript" src="js/calendar.js" type="text/javascript"></script>
<script type="text/javascript" src="js/jquery.1.12.4.min.js"></script><div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>公司<%=GetAddUpdate()%></div><div class="add">
	<ul>
		<li class="left">公司全称：</li>
		<li class="right">
		    <XueFu:TextBox CssClass="input" Width="300px" ID="CompanyName" runat="server" CanBeNull="必填" />
        </li>
	</ul>
	<ul>
		<li class="left">公司简称：</li>
		<li class="right">
		    <XueFu:TextBox CssClass="input" Width="300px" ID="CompanySimpleName" runat="server" />
		</li>
	</ul>
	<ul>
		<li class="left">公司类型：</li>
		<li class="right">
		    <asp:RadioButtonList ID="CompanyType" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="0">独立公司</asp:ListItem>
                <asp:ListItem Value="3">集团旗下子公司</asp:ListItem>
                <asp:ListItem Value="2">集团旗下总公司</asp:ListItem>
                <asp:ListItem Value="1">集团</asp:ListItem>
            </asp:RadioButtonList>
        </li>
	</ul>
	<ul id="GroupBrand" style="display:none;" runat="server">
		<li class="left">隶属集团：</li>
		<li class="right">
		    <ul id="GroupNameList" class="GroupList" runat="server" style="border:0px;"></ul>
		    <div>
                <input id="AddGroupName" type="button" value="添加隶属集团" class="button" onclick="o('AddGroupId').style.display='';"  style="width:100px;"/>
		        <div id="AddGroupId" style="display:none;">
		            <asp:DropDownList ID="GroupId" runat="server" onChange="AddGroup(this.value,this.options[selectedIndex].text);"></asp:DropDownList>
                </div>
            </div>
            <input id="GroupListId" type="hidden" value="" runat="server" />
        </li>
	</ul>
	<ul id="CarBrand" runat="server">
		<li class="left">拥有品牌：</li>
		<li class="right" style="text-align:left;">
    		<%=BrandHtml.ToString() %>
		</li>
	</ul>
	<ul>
		<li class="left">岗位设置：</li>
		<li class="right">
    		<%=PostHtml.ToString() %>
        </li>
	</ul>
	<ul>
		<li class="left">使用截止时间：</li>
		<li class="right"><XueFu:TextBox CssClass="input" ID="EndDate" runat="server" Width="65px" RequiredFieldType="日期时间"  onfocus="cdr.show(this);" /></li>
	</ul>
	<ul>
		<li class="left">公司电话：</li>
		<li class="right">
		    <XueFu:TextBox CssClass="input" Width="300px" ID="CompanyTel" runat="server" Text="0000000" AutoCompleteType="Disabled" CanBeNull="必填" />
        </li>
	</ul>
	<ul>
		<li class="left">公司邮编：</li>
		<li class="right">
		    <XueFu:TextBox CssClass="input" Width="300px" ID="CompanyPost" runat="server" CanBeNull="必填" Text="000000" RequiredFieldType="邮编" />
        </li>
	</ul>    
	<ul>
		<li class="left">公司地址：</li>
		<li class="right">
		    <XueFu:TextBox CssClass="input" Width="300px" ID="CompanyAddress" runat="server" Text="0" CanBeNull="必填" />
        </li>
	</ul>
	<ul>
		<li class="left">岗位计划开始时间：</li>
		<li class="right"><XueFu:TextBox CssClass="input" Width="300px" ID="PostStartDate" runat="server" RequiredFieldType="日期时间"  onfocus="cdr.show(this);" /></li>
	</ul>
	<ul>
		<li class="left">公司员工数量：</li>
		<li class="right"><XueFu:TextBox CssClass="input" Width="50px" ID="UserNum" runat="server" Text="0" HintInfo="员工的总数量" /></li>
	</ul>
	<ul>
		<li class="left">考试状态：</li>
		<li class="right"><asp:CheckBox ID="IsTest" runat="server" Checked="True" Text="开启" /> 开启则可以考试</li>
	</ul>	
	<ul>
		<li class="left">显示排序：</li>
		<li class="right"><XueFu:TextBox CssClass="input" Width="50px" ID="Sort" runat="server" Text="0" HintInfo="数字越小越靠前" /></li>
	</ul>
	<ul>
		<li class="left">状态：</li>
		<li class="right"><asp:RadioButtonList ID="State" runat="server" RepeatDirection="Horizontal"><asp:ListItem Value="0" Selected="True">正常</asp:ListItem><asp:ListItem Value="1">锁定</asp:ListItem></asp:RadioButtonList></li>
	</ul>
</div>
<asp:HiddenField ID="sign" runat="server" /><div class="action">    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" /></div>
<script language="javascript" type="text/javascript">
$(document).ready(function() {
	//部门选择
	$("input[name='Department']").change(function(){
	    if($(this).is(":checked")){
	console.log($(this).is(":checked"))
		    $("input[data-type='"+$(this).val()+"']").prop("checked", true);
	    }else{
		    $("input[data-type='"+$(this).val()+"']").prop("checked", false);
	    }
    })
    
    $("input[name='"+globalNamePrefix+"CompanyType']").change(function(){
        if($(this).val() == "2" || $(this).val() == "3")
        {
            $("#" + globalIDPrefix + "GroupBrand").show();
            $.post("Ajax.aspx?Action=GetGroupCompanyListById",{companyid:0},function(data){
	            data=data.data;
	            $('#'+globalIDPrefix+'GroupId').append("<option value='-1'>请选择隶属公司</option>")
	            for(i=0;i<data.length;i++)
	            {
	                $('#'+globalIDPrefix+'GroupId').append("<option value='"+data[i].companyid+"'>"+data[i].companyname+"</option>");
	            }
	        },"json");
	    }
	    else
	    {
	        $("#" + globalIDPrefix + "GroupBrand").hide();
	    }
    });
});
//选择客户类别
function selectCompanyType(){
    var objs=os("name",globalNamePrefix+"CompanyType");
    var CompanyType=getRadioValue(objs); 
    switch(CompanyType){
       // case "1":
          //  o("GroupBrand").style.display=""; 
           // break;
        case "0":
            o("GroupBrand").style.display="none";
            break;
        default:
            o("GroupBrand").style.display="";
            break;
    }    
}

function AddGroup(Id,Text)
{
    o(globalIDPrefix+"GroupNameList").innerHTML+="<li id='li_" + Id + "' style='float:none;'>" + Text + "<a onclick='javascript:DelGroup(" + Id + ");'><img src='Style/Images/delete.gif'></a></li>";
    o("AddGroupId").style.display="none";
    var GroupNameList=o(globalIDPrefix+"GroupListId").value;
    if(GroupNameList!="")
    {
        o(globalIDPrefix+"GroupListId").value=GroupNameList+","+Id;
    }
    else
    {
        o(globalIDPrefix+"GroupListId").value=Id;
    }
}

function DelGroup(Id)
{
    if(o("li_"+Id))
    {
        o("li_"+Id).style.display="none";
        var GroupNameList=","+o(globalIDPrefix+"GroupListId").value+",";
        if(GroupNameList!="")
        {
            var reg=new RegExp(","+Id+",","g");
            var NewStr=GroupNameList.replace(reg,",");
            NewStr=NewStr.substring(1,NewStr.length-1);
            if(NewStr==",") NewStr="";
            o(globalIDPrefix+"GroupListId").value=NewStr;
        }
    }
}
//selectCompanyType();
</script>
</asp:Content>
