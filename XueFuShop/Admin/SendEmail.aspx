<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="SendEmail.aspx.cs" Inherits="XueFuShop.Admin.SendEmail" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="position"><img src="Style/Images/PositionIcon.png"  alt=""/>发邮件</div>
<div class="add">
	<ul>
		<li class="left">发送给：</li>
		<li class="right"><input type="radio" name="ToUserType" value="1" checked="checked" onclick="selectToUserType(1)" />按邮箱地址<input type="radio" name="ToUserType" value="2"  onclick="selectToUserType(2)"/>按登录名</li>
	</ul>
	<ul>
		<li class="left"><span id="ToUserType">邮箱地址</span>：</li>
		<li class="right"><input id="ToUser" type="text" style="width:200px" onblur="readUserEmail()" /><input name="ToUserEmail" id="ToUserEmail" type="hidden"/> </li>
	</ul>
	<ul>
		<li class="left">标题：</li>
		<li class="right"><asp:TextBox Width="350px" ID="Title" runat="server"></asp:TextBox>   <asp:RequiredFieldValidator
                ID="RequiredFieldValidator2" runat="server" ErrorMessage="不能为空" ControlToValidate="Title" Display="Dynamic"></asp:RequiredFieldValidator></li>
	</ul>
	<ul>
		<li class="left">内容：</li>
		<li class="right"><asp:TextBox Width="350px" TextMode="MultiLine" Height="180px" ID="Content" runat="server"></asp:TextBox> <asp:RequiredFieldValidator
                ID="RequiredFieldValidator1" runat="server" ErrorMessage="不能为空" ControlToValidate="Content" Display="Dynamic"></asp:RequiredFieldValidator></li>
	</ul>	
</div>
<div class="action">
    <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click"  OnClientClick="return chekSendEmail()"/>
</div>
<script language="javascript" type="text/javascript">
function selectToUserType(type){
    if(type==1){
        o("ToUserType").innerHTML="邮箱地址";
    }
    else{
        o("ToUserType").innerHTML="登录名";
    }    
    readUserEmail();
}
//读取用户Email地址
function readUserEmail(){
    var toUserType=getRadioValue(os("name","ToUserType")); 
    var toUser=o("ToUser").value;    
    if(toUserType=="2"){  
        if(toUser!=""){      
            var url="Ajax.aspx?Action=ReadUserEmail&UserName="+encodeURI(toUser);
            Ajax.requestURL(url,dealReadUserEmail);
        }
        else{
             o("ToUserEmail").value="";
        }
    }
    else{
        o("ToUserEmail").value=toUser;
    }
}
function dealReadUserEmail(data){
    if(Validate.isEmail(data)){
        o("ToUserEmail").value=data;
    }
    else{
        o("ToUserEmail").value="";
    } 
}
//检查表单
function chekSendEmail(){
    var toUserEmail=o("ToUserEmail").value;  
    if(!Validate.isEmail(toUserEmail)){
        alertMessage("Email地址错误");
        return false;
    }
    return Page_ClientValidate();   
}
</script>
</asp:Content>
