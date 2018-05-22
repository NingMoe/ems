// JavaScript Document
function v(obj,pattern){
	if(pattern.test(obj)){
		return true;
	}
	return false;
}
  //验证手机
function verifyMobile(opt){
	var pattern = /^1[3|4|5|7|8]\d{9}$/;
	 var isright= v(opt,pattern);
	 return isright;
}
//过滤<script><\/script>标签
function clearScript(s) {  
    return s.replace(/<script.*?>.*?<\/script>/ig, '');  
}
var wait = 120;
function time1(id,id1,cc) {
    if (wait == 0) {
    	$("#"+id).removeAttr("disabled");
    	//$("#"+id1).removeAttr("disabled");
    	$("#"+id).val(cc);
        wait = 120;
        //$("#movecode").val("");
    } else {
        $("#"+id).attr("disabled", true);
        //$("#"+id1).attr("disabled", true);
        $("#"+id).attr("cursor", "pointer");
        $("#"+id).val( wait + "秒");
        wait--;
        setTimeout(function () {
            time1(id,id1,cc);
        },
        1000);
    }
}
$(function(){
	$("input[type='text'],input[type='password']").focus(function(){
    	$(this).attr("placeholder","");
    })
    
    $("input[type='text'],input[type='password']").each(function(){
    	var placeholder_val=$(this).attr("placeholder");
    	$(this).blur(function(){
    	$(this).attr("placeholder",placeholder_val);
    	$(this).css("color","#aaa");
    })
    })
    
	$("#btn").click(function(){
		var inputmovecode = $("#inputmovecode").val();
		var userName = $("#UserName").val();
        var reg = /^([a-zA-Z0-9_\u4E00-\u9FA5])+$/;
		if(!reg.test(userName)){
			layer.msg("请正确输入用户名");
			return false;
		}
		if(!vPhone()){
	      	 return;
		}
		if(!picCk()){
			return; 
		}
		if(inputmovecode=="" || inputmovecode.length != 6){
			layer.msg("请正确输入验证码");
			return;
		}
		var password = $("#password").val(), password2 = $("#password2").val();
		if (password==""||password2=="") {
			layer.msg("请输入密码");
			return false;
		}
		if (password!=password2) {
			layer.msg("再次密码输入不一致！");
			return false;
		}
		var reg=/(?=^[0-9a-zA-Z._@#]{6,16}$)((?=.*[0-9])(?=.*[^0-9])|(?=.*[a-zA-Z])(?=.*[^a-zA-Z]))/;
		if (!reg.test(password)) {
			layer.msg("密码为6-16位字母数字组合！");
			return false;
		}
		compareMoveCode($("#movecode").val(),0,$("#phone").val(),inputmovecode);
	});
	var srcCode = $("#srccode").val(), mobile = $("#phone").val();
	if(srcCode != '' && mobile != ''){
		compareMoveCode($("#movecode").val(),0,mobile,srcCode);
	}
})

function smsOpt(phone){
	setTimeout(function () {
		voiceCodeAjax(phone);
		time1("voicecodeinput","smscodeinput","获取语音验证码");
	},
	1000);
}
function voiceCode(){
	var phone = $("#phone").val();
	if(!vPhone()){
		return;
	}
	if(picCk()){
		var code = $("#expvoicecode").val();
		toCkPic(code,"smsOpt("+phone+")");
	}
}

function voiceCodeAjax(phone){
	$("#voicecodeinput").attr("disabled", true);
	$.ajax({
		url:"/checkcode/voiceExtCode",
		type:"post",
		data:"phone="+phone,
		dataType: "text",
		success: function (data) {
          $("#movecode").val(data);
        },
        error: function (msg) {
        	 $("#movecode").val("");
        }
	});
}
function smsCodeAjax(phone){
	$.ajax({
		url:"/SMSAjax.aspx",
		type:"get",
		data:"Action=SendCode&CateId="+$("#movecode").val()+"&Mobile="+phone+"&UserId="+$("#userID").val(),
		dataType: "text",
		success: function (data) {
			data = clearScript(data);
		    var ResultData= new Array();
		    ResultData=data.split("|");
		    if(ResultData[0]=="1")
		    {
	            $("#smscodeinput").attr("disabled", false);
	        }
		    if(ResultData[0]!="0")
		    {
		        layer.msg(ResultData[1]);
		    }
		    else
		    {
		        time1("smscodeinput","voicecodeinput","获取短信验证码");
		    }
        },
        error: function (msg) {	        	 
        }
	});
}
function compareMoveCode(CateId,Part,Phone,Value){	
	$.ajax({
		url:"/SMSAjax.aspx",
		type:"get",
		data:"Action=VerFind&Mobile="+Phone+"&CheckCode="+Value,
		dataType: "text",
		success: function (data) {
			data = clearScript(data);
			if(data=="0")
			{
				$("#contactform").submit();
			}
			else
			{
				layer.msg(data);
			}
        }
	});
}
function voiceOpt(phone){
	setTimeout(function () {
	    smsCodeAjax(phone);
    },1000);
}
function smsCode(){
	var phone = $("#phone").val();
	if(!vPhone()){
		return;
	}
	if(picCk()){
		var code = $("#expvoicecode").val();
		toCkPic(code,"voiceOpt("+phone+")");
	}
}
function vPhone(){		
	// if($("#movecode").val()<0)
	// {
	// 	$("#phone_error").text("页面异常，请从正确的入口进入。");
	// 	$("#phone_error").fadeIn();
	// 	return false;
	// }
	var phone = $("#phone").val();
	if(phone=="" || !verifyMobile(phone)){
	  layer.msg("请正确输入手机号码！");
  	  return false;
	}
	return true;
}
function picCk(){
	var code = $("#expvoicecode").val();
	if(code == "" || code.length != 4){
	  layer.msg("请正确输入图形验证码");
   	  return false;
	}
	return true;
}
function toCkPic(code,obj){		
	$("#smscodeinput").attr("disabled", true);
	$.ajax({
		url:"/SMSAjax.aspx",
		type:"get",
		data:"Action=CheckCode&CheckCode="+code,
		dataType: "text",
		success: function (data) {
			data = clearScript(data);
			if(data != 2){
				eval(obj);
			}else{
				 $("#smscodeinput").attr("disabled", false);
				 layer.msg("验证码有误，请重新输入。");
			}
        },
        error: function (msg) {
        }
	});
}