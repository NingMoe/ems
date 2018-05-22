<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="UserAdd.aspx.cs" Inherits="XueFuShop.Admin.UserAdd"%>
<%@ Import Namespace="XueFuShop.BLL" %><asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server"><%--<script language="javascript" src="/Admin/Js/calendar.js" type="text/javascript"></script><script language="javascript" type="text/javascript" src="/Admin/js/UnlimitClass.js"></script>--%>
<link rel="stylesheet" href="Style/jquery.bigautocomplete.css" type="text/css" />
<script type="text/javascript" src="Js/jquery.1.12.4.min.js"></script>
<script type="text/javascript" src="Js/jquery.bigautocomplete.js"></script><script type="text/javascript" src="My97DatePicker/WdatePicker.js"></script><div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>用户<%=GetAddUpdate()%></div><div class="add">    <ul>		<li class="left">公司名称：</li>		<li class="right">		    <input type="text" name="CompanyName" id="CompanyName" value="" size="55" Class="input" style="width:330px;"   runat="server" />		    <span style="color:red">*</span>		    <input type="hidden" name="CompanyID" id="CompanyID" value="<%=companyID.ToString() %>" />		</li>	</ul>	<ul>		<li class="left">用户名：</li>		<li class="right"><XueFu:TextBox ID="UserName" CssClass="input" runat="server" Width="150px" CanBeNull="必填" onblur="checkUserName(this.value)"/></li>
		<li class="left">管理组：</li>
		<li class="right"><asp:DropDownList Width="156px" ID="GroupID" runat="server"></asp:DropDownList></li>	</ul>	<ul>		<li class="left">电子邮箱：</li>		<li class="right"><XueFu:TextBox ID="Email" CssClass="input" runat="server" Width="330px" CanBeNull="必填" RequiredFieldType="电子邮箱" /></li>	</ul>	<asp:PlaceHolder ID="Add" runat="server">	<ul>		<li class="left">密码：</li>		<li class="right"><XueFu:TextBox ID="UserPassword" CssClass="input" runat="server" Width="150px" CanBeNull="必填" RequiredFieldType="自定义验证表达式" ValidationExpression="^[\W\w]{6,16}$" CustomErr="密码长度大于6位少于16位" TextMode="Password"/></li>		<li class="left">重复密码：</li>		<li class="right"><XueFu:TextBox CssClass="input" Width="150px" ID="UserPassword2" runat="server" CanBeNull="必填" RequiredFieldType="自定义验证表达式" ValidationExpression="^[\W\w]{6,16}$" CustomErr="密码长度大于6位少于16位" TextMode="Password"/>            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="两次密码不一致" ControlToCompare="UserPassword" ControlToValidate="UserPassword2" Display="Dynamic"></asp:CompareValidator>        </li>	</ul>	</asp:PlaceHolder>	<%--<ul>		<li class="left">介绍：</li>		<li class="right"><XueFu:TextBox ID="Introduce" CssClass="input" runat="server" Width="400px"  TextMode="MultiLine" Height="100px"/></li>	</ul>	<ul>		<li class="left">照片：</li>		<li class="right"><XueFu:TextBox ID="Photo" CssClass="input" runat="server" Width="400px" /></li>	</ul>	<ul>		<li class="left">上传照片：</li>		<li class="right"><iframe src="UploadAdd.aspx?Control=Photo&TableID=<%=UserBLL.TableID%>&FilePath=UserPhoto" width="400" height="30px" frameborder="0" allowTransparency="true" scrolling="no"></iframe></li>	</ul> --%>	<ul>		<li class="left">移动电话：</li>		<li class="right"><XueFu:TextBox ID="Mobile" CssClass="input" runat="server" Width="150px" CanBeNull="必填" RequiredFieldType="移动手机" /></li>		<li class="left">固定电话：</li>		<li class="right"><XueFu:TextBox ID="Tel" CssClass="input" runat="server" Width="150px" /></li>	</ul>	<ul>
	    <li class="left">姓名：</li>
		<li class="right"><XueFu:TextBox ID="RealName" CssClass="input" runat="server" Width="150px" CanBeNull="必填" /></li>		<li class="left">性别：</li>		<li class="right"><asp:RadioButtonList ID="Sex" runat="server" RepeatDirection="Horizontal"><asp:ListItem Value="1">男</asp:ListItem><asp:ListItem Value="2">女</asp:ListItem><asp:ListItem Value="3" Selected="True">保密</asp:ListItem></asp:RadioButtonList></li>
	</ul>	<ul>		<li class="left">工作职位：</li>		<li class="right">
		    <asp:DropDownList ID="Department" runat="server"></asp:DropDownList>
		    <asp:DropDownList ID="PostList" runat="server"></asp:DropDownList>
		    原始岗位名称：<asp:TextBox ID="PostName" CssClass="input" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
        </li>
        <asp:HiddenField ID="OldStudyPostId" runat="server" />
    </ul>	<ul>		<li class="left">学习岗位：</li>		<li class="right" id="DropList1" runat="server">
		    <asp:DropDownList ID="StudyPostId" runat="server"></asp:DropDownList> 正在进行哪个岗位的课程学习
        </li>
    </ul>    <ul id="PostStart" runat="server">		<li class="left">岗位计划开始日期：</li>		<li class="right" runat="server">
        <asp:HiddenField ID="RegDate" runat="server" />
		    <XueFu:TextBox ID="PostStartDate" CssClass="input Wdate" runat="server" Width="150px" RequiredFieldType="日期" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd',readOnly:true,minDate:'#F{$dp.$D(globalIDPrefix+\'RegDate\')}'})" />
        </li>
    </ul>	<%--<ul>		<li class="left">QQ：</li>		<li class="right"><XueFu:TextBox ID="QQ" CssClass="input" runat="server" Width="150px" /></li>		<li class="left">MSN：</li>		<li class="right"><XueFu:TextBox ID="MSN" CssClass="input" runat="server" Width="150px" /></li>	</ul>	<ul>		<li class="left">所在地区：</li>		<li class="right"><XueFu:SingleUnlimitControl ID="UserRegion" runat="server" /></li>	</ul>	<ul>		<li class="left">详细地址：</li>		<li class="right"><XueFu:TextBox ID="Address" CssClass="input" runat="server" Width="400px" /></li>	</ul>--%>		<ul>		<%--<li class="left">生日：</li>		<li class="right"><XueFu:TextBox ID="Birthday" CssClass="input" runat="server" Width="140px" onfocus="cdr.show(this);"/></li>--%>		<li class="left">状态：</li>		<li class="right">            <asp:RadioButtonList ID="Status" runat="server" RepeatDirection="Horizontal">                <asp:ListItem Value="0">删除</asp:ListItem>                <asp:ListItem Value="1">未验证</asp:ListItem>                <asp:ListItem Value="2" Selected="True">正常</asp:ListItem>                <asp:ListItem Value="3">冻结</asp:ListItem>                <asp:ListItem Value="4">不考核</asp:ListItem>                <asp:ListItem Value="5">其他</asp:ListItem>
            </asp:RadioButtonList>
		</li>	</ul></div><div class="action">    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" OnClientClick="return checkForm();" /></div><script type="text/javascript">
$(function(){
	$(window).keydown(function (e) {
		if (e.which == 13) {
			return false;
		}
	})
	
	$("#ctl00_ContentPlaceHolder_CompanyName").bigAutocomplete({
	    width:"304px",
		url:"Ajax.aspx?Action=SearchCompanyName",
		callback:function(data){
		    var result=data.result;
		    $("#CompanyID").val(data.result.Id);
		    $.post("Ajax.aspx?Action=GetUserGroupJson",{CompanyID:result.Id,Type:1},function(data){
		        var currentGroupID = $("#"+globalIDPrefix+"GroupID").val();
                $("#"+globalIDPrefix+"GroupID").html('<option value="0">请选择管理组</option>');
		        data=JSON.parse(data);
		        $.each(data.data, function(i, item){
		            if(item.ID == currentGroupID){
                        $("#"+globalIDPrefix+"GroupID").append("<option value=\"" + item.ID + "\" selected>" + item.Name + "</option>")
                    }
                    else{
                        $("#"+globalIDPrefix+"GroupID").append("<option value=\"" + item.ID + "\">" + item.Name + "</option>")
                    }
                });
		    })
		    $.post("Ajax.aspx?Action=GetDepartmentListByStudyPostID",{PostID:result.post},function(data){
		        var currentDepartment = $("#"+globalIDPrefix+"Department").val(),
		        currentPostID = $("#"+globalIDPrefix+"PostList").val(),
		        currentStudyPostID = $("#"+globalIDPrefix+"StudyPostId").val();
		        
		        $("#"+globalIDPrefix+"Department").html(data.split("|")[0]).find("option[value = '"+currentDepartment+"']").attr("selected","selected");
		        $("#"+globalIDPrefix+"PostList").html(data.split("|")[1]).find("option[value = '"+currentPostID+"']").attr("selected","selected");
		        $("#"+globalIDPrefix+"StudyPostId").html(data.split("|")[1]).find("option[value = '"+currentStudyPostID+"']").attr("selected","selected");
		        
		    });
		}
	});
})
</script><script language="javascript" type="text/javascript">    function checkMobile(){        var mobile=$("#"+globalIDPrefix+"Mobile").val();        var userID=<%=userID %>;        if(mobile != ""){            var isPass=false;            $.ajax({
                type: 'POST',
			  	url: 'Ajax.aspx?Action=CheckMobile',
			  	data: {Mobile: mobile,UserID: userID},
			  	async : false,  
			  	success: function(data){
			  		data = JSON.parse(data);
					if (data.result) {
						isPass = true;
					}
			  	}
			});			return isPass;        }    }        var isCheckUserName=false;    function checkUserName(userName){        isCheckUserName=false;        if(userName!=""){            var reg = /^([a-zA-Z0-9_\u4E00-\u9FA5])+$/;            if(reg.test(userName)){                Ajax.requestURL("Ajax.aspx?Action=CheckUserName&UserName="+encodeURI(userName),dealCheckUserName);            }            else{                alertMessage("用户名只能包含字母、数字、下划线、中文");                return false;            }        }    }    function dealCheckUserName(data){        if(data=="ok"){            isCheckUserName=true;        }        else{            alertMessage("该用户名已经被占用");        }    }    function checkForm(){        if(Page_ClientValidate()){            if(getQueryString("ID")=="") {                if(!isCheckUserName){                    alertMessage("该用户名已经被占用");                    return false;                }            }//            if(!checkMobile()){//                alertMessage("考试人员中手机号码已经被使用");//                return false;//            }            return true;        }        else{             return false;        }     }</script></asp:Content>