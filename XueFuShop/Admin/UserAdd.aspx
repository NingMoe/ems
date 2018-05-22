﻿<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="UserAdd.aspx.cs" Inherits="XueFuShop.Admin.UserAdd"%>
<%@ Import Namespace="XueFuShop.BLL" %>
<link rel="stylesheet" href="Style/jquery.bigautocomplete.css" type="text/css" />
<script type="text/javascript" src="Js/jquery.1.12.4.min.js"></script>
<script type="text/javascript" src="Js/jquery.bigautocomplete.js"></script>
		<li class="left">管理组：</li>
		<li class="right"><asp:DropDownList Width="156px" ID="GroupID" runat="server"></asp:DropDownList></li>
	    <li class="left">姓名：</li>
		<li class="right"><XueFu:TextBox ID="RealName" CssClass="input" runat="server" Width="150px" CanBeNull="必填" /></li>
	</ul>
		    <asp:DropDownList ID="Department" runat="server"></asp:DropDownList>
		    <asp:DropDownList ID="PostList" runat="server"></asp:DropDownList>
		    原始岗位名称：<asp:TextBox ID="PostName" CssClass="input" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
        </li>
        <asp:HiddenField ID="OldStudyPostId" runat="server" />
    </ul>
		    <asp:DropDownList ID="StudyPostId" runat="server"></asp:DropDownList> 正在进行哪个岗位的课程学习
        </li>
    </ul>
        <asp:HiddenField ID="RegDate" runat="server" />
		    <XueFu:TextBox ID="PostStartDate" CssClass="input Wdate" runat="server" Width="150px" RequiredFieldType="日期" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd',readOnly:true,minDate:'#F{$dp.$D(globalIDPrefix+\'RegDate\')}'})" />
        </li>
    </ul>
            </asp:RadioButtonList>
		</li>
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
</script>
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
			});