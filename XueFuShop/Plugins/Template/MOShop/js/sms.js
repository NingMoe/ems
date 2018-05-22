// JavaScript Document
function v(obj,pattern){
	if(pattern.test(obj)){
		return true;
	}
	return false;
}
  //验证手机
function verifyMobile(opt){
	var pattern = /^1[3|4|5|7|8|9]\d{9}$/;
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
        $("#"+id).val( wait + "秒后重新获取");
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
		if(!vPhone()){
	      	 return;
		}
		if(!picCk()){
			return; 
		}
		if(inputmovecode=="" || inputmovecode.length!=6){
			$("#move_phone_code_error").html("请正确输入验证码");
			$("#move_phone_code_error").show();
			return;
		}else{
			$("#move_phone_code_error").hide();
		}
		compareMoveCode($("#movecode").val(),0,$("#phone").val(),inputmovecode);
	});
	// var srcCode = $("#srccode").val(), mobile = $("#phone").val();
	// if(srcCode != '' && mobile != ''){
	// 	compareMoveCode($("#movecode").val(),0,mobile,srcCode);
	// }
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
		url:"SMSAjax.aspx",
		type:"get",
		data:"Action=SendCheckCode&CateId="+$("#movecode").val()+"&Mobile="+phone+"&UserId="+$("#userID").val(),
		dataType: "text",
		success: function (data) {
			data = clearScript(data);
		    var ResultData= new Array(); 
		    ResultData=data.split("|");
		    if(ResultData[0]=="1")
		    {
	            $("#smscodeinput").attr("disabled", false);
		    }
		    if (ResultData[0] == "0") {
		        var code = $('.playContent').data("code").split("_");
		        if (code[0] == "0") {
		            $('.playContent').data("code", ResultData[2] + "_" + code[1]);
		        }
		    }
		    if(ResultData[0]!="0")
		    {
		        $("#phone_error").text(ResultData[1]);
		        $("#phone_error").fadeIn();
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
// function compareMoveCode(CateId,Part,Phone,Value){	
// 	$.ajax({
// 		url:"SMSAjax.aspx",
// 		type:"get",
// 		data:"Action=Ver&CateId="+CateId+"&Part="+Part+"&Mobile="+Phone+"&CheckCode="+Value+"&UserId="+$("#userID").val(),
// 		dataType: "text",
// 		success: function (data) {
// 			data = clearScript(data);
// 			if(data.length>20)
// 			{
// 				var ResultData= new Array(); 
// 				ResultData=data.split("|");
// 				$(".display_box").hide();
// 				$("#VideoList").html(ResultData[0]);
// 				$("#loadvideo").attr("src",ResultData[1]);
// 				$("#willesPlay").show();
// 			}
// 			else
// 			{
// 				$("#move_phone_code_error").text(data);
// 				$("#move_phone_code_error").fadeIn();					
// 			}
//         }
// 	});
// }

function compareMoveCode(CateId,Part,Phone,Value){	
	$.ajax({
		url:"SMSAjax.aspx",
		type:"get",
		data:"Action=Ver&CateId="+CateId+"&Part="+Part+"&Mobile="+Phone+"&CheckCode="+Value+"&UserId="+$("#userID").val(),
		dataType: "text",
		success: function (data) {
			data = clearScript(data);
			if(data=="true")
			{
				player = polyvObject('.playContent').videoPlayer({					
					'width':'800',
					'height':'475',
					'vid' : $("#VideoList li").eq(0).data("vid"),
					'code': $('.playContent').data("code")+Value
				});
				// var ResultData= new Array(); 
				// ResultData=data.split("|");
				$(".display_box").hide();
				// $("#VideoList").html(ResultData[0]);
				// $("#loadvideo").attr("src",ResultData[1]);
				$("#willesPlay").show();
			}
			else
			{
				$("#move_phone_code_error").text(data);
				$("#move_phone_code_error").fadeIn();					
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
	  $("#phone_error").text("请输入正确的手机号码。");
  	  $("#phone_error").fadeIn();
  	  return false;
	}
	$("#phone_error").hide();
	return true;
}
function picCk(){
	var code = $("#expvoicecode").val();
// 	var vphonecheckcode = $("#vphonecheckcode").val();
	if(code == "" || code.length != 4){
	  $("#phone_code_error").text("请正确输入验证码");
   	  $("#phone_code_error").fadeIn();
   	  return false;
	}else{
		$("#phone_code_error").fadeOut();
	}
	return true;
}
function toCkPic(code,obj){		
	$("#smscodeinput").attr("disabled", true);
	$.ajax({
		url:"SMSAjax.aspx",
		type:"get",
		data:"Action=CheckCode&CheckCode="+code,
		dataType: "text",
		success: function (data) {
			data = clearScript(data);
			if(data != 2){
				eval(obj);
			}else{
				 $("#smscodeinput").attr("disabled", false);
				 $("#phone_code_error").text("验证码有误，请重新输入。");
        	     $("#phone_code_error").fadeIn();
			}
        },
        error: function (msg) {
        }
	});
}