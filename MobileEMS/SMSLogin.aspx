<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSLogin.aspx.cs" Inherits="MobileEMS.SMSLogin" %>
<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no,minimal-ui">
    <meta name="MobileOptimized" content="320" />
    <meta http-equiv="cleartype" content="on" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />    
    <title>上海孟特 短信登录</title>
    <link rel='stylesheet' href='css/style.css' type='text/css'/>
    <script type="text/javascript" src="js/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="js/pass_mobile.js"></script>    
    <script type="text/javascript" src="http://common.hjfile.cn/analytics/site/other_pass_2012.js?v=1"></script>
    </head>
<body>
    <div id="wrapper">
    <header>
        <a href="javascript:history.back();" class="icon_back"></a>       
        <a href="Reg.aspx" class="btn_login">注册</a>
        短信登录
    </header>


<fieldset class="p10 regiter_box mt10">
    <div class="input_box">
        <input type="text" class="c_input phone valid" maxlength="11" placeholder="手机号" id="mobile" pattern="^((1[3|5|8|4][0-9]\d{8})|(170\d{8}))$" required="required"/>
        <i class=""></i>
        <p class="red mt5 hide">您的手机已注册，请换个号码试试</p>
    </div>

    
        <div class="input_box" id="Code_box">
            <input name="txtcaptcha" type="text" class="c_input code" id="txtcaptcha" placeholder="图片验证码" maxlength="8" style="width:55%;" required="required" />
            <input type="hidden" id="token" />
            <img id="captcha_img" onclick="this.src='Code.aspx?v=<%=DateTime.Now %>'" src="Code.aspx?v=<%=DateTime.Now %>" style="width: 30%; height: 45px; margin-top: 3px; cursor: pointer" alt="" title="" class="fr" />
            <p class="red" id="error_captcha">&nbsp;</p>
        </div>
    

    <div class="input_box">
        <input type="text" maxlength="6"  class="c_input code fl" width="100" placeholder="动态码" id="validcode"  required="required" />
        <a href="javascript:;" class="btn btnWhite btnGetCode fl" id="getcode">获取动态码</a>
        <div class="clearbox"></div>
        <p class="red mt5 hide">您的动态码输入错误，请重新获取新的动态码</p>
    </div>

    

    <p class="mb10">
        <input type="checkbox" id="agreement" class="checkbox" checked/><label for="agreement" class="mr5"></label><label for="agreement" class="">我已经阅读并同意<a href="/m/Agreement.aspx" class="green">《上海孟特用户协议》</a></label></p>
    <p class="red mt5 hide agree_error">同意《上海孟特用户协议》才能注册</p>
    <a href="#1" class="btn btnGreen" id="btn_register">马上登录</a>
    <p class="mt10"><a href="Login.aspx" class="green">常规登录>></a></p>
</fieldset>


<script type="text/javascript" src="js/hjlogin.js"></script>

<script type="text/javascript">
    var mobile_is_used = 0;

    $.ajax({'async':false});
    function msgModeClose(){
        $(".msgMode").fadeOut(200);
    }
    var mobile_regex = /^0?((1[3|5|8|4][0-9]\d{8})|(17\d{9}))$/;
    var nullcheck = /^[\s\n\r\t]*$/;
    var is_send = false;
    var time = 120 , timeConst = 120;
    var interval;
    function send_validation_code() {
        if (is_send || mobile_is_used) return;
        //验证图片验证码
        clearFocus($("#txtcaptcha"));         
        is_send = true;
        var mobile = $('#mobile').val();
        $('#getcode').text('动态码获取中');
     
        $.get("SMSAjax.aspx?Action=SendCheckCode&SendType=Login&Mobile="+mobile, function (data) {
            var ResultData= new Array(); 
		    ResultData=data.split("|");
            
            if (ResultData[0] == "0") {
                interval = setInterval(function () {
                    
                    time--;
                    if (time <= 0) {
                        $('#getcode').text("重新获取动态码");
                        $('#getcode').removeClass("disabled");
                        clearInterval(interval);
                        time = timeConst;
                        $('#getcode').on('click', send_validation_code);
                        is_send = false;
                        
                    }
                    else {
                        $('#getcode').text(time + '秒再获取').off('click');
                        $('#getcode').addClass("disabled");
                    }
                }, 1000);
                
                
            }
            else if (ResultData[0] == "2")
            {
                addFocus($('#mobile'), ResultData[1]);
                $('#getcode').text('获取动态码');
                is_send = false;
            }
            else if (ResultData[0] == "3"|| ResultData[0] == "1")
            {
                addFocus($('#validcode'), ResultData[1]);
                $('#getcode').text('获取动态码');
                is_send = false;
            }
            if (!is_send) {
                $("#captcha_img").click();
                $("#txtcaptcha").val("");
            }
        });
    }
    
    function sso_callback  () { window.location.href = 'CourseCenter.aspx'; }
    
    $(function () {
        $('#validcode').focus(function () {
            clearFocus($('#validcode'));
        }).blur(function () {
            if ($(this).val() == '') {
                addFocus($('#validcode'), '请输入手机动态码');
            }
            else {
                if (!/^\d{6}$/.test($(this).val())) {
                    addFocus($('#validcode'), '手机动态码格式不正确');
                }
            }
        });

        $("#txtcaptcha").focus(function () {
            clearFocus(this);
        }).blur(function () {
            if (nullcheck.test($(this).val())) {
                addFocus(this, '请输入右侧的图片验证码');
            }
        })

        $("#mobile").focus(function () {
            clearFocus(this);
        }).blur(function () {
            if (!mobile_regex.test($(this).val())) {
                addFocus($(this), '请输入正确的手机号');
            }
        });

        $('#btn_register').click(function () {
            var isPass = true;
            if (!mobile_regex.test($('#mobile').val())) {
                addFocus($('#mobile'), '请输入正确的手机号');
                isPass = false;
            }

            clearFocus($("#txtcaptcha"));
            if (nullcheck.test($("#txtcaptcha").val())) {
                addFocus($("#txtcaptcha"), '请输入图片验证码');
                isPass = false;
            }

            if (!/^\d{6}$/.test($('#validcode').val())) {
                addFocus($('#validcode'), '请输入手机验证码');
                isPass = false;
            }
            if (!isPass) {
                return false;
            }
            if (!$('#agreement').is(':checked')) {
                $('p.agree_error').show();
                return false;
            }
            
            $.ajax({
			url:"SMSAjax.aspx",
			type:"get",
			data:"Action=Ver&SendType=Login&Mobile="+$('#mobile').val()+"&CheckCode="+$('#validcode').val(),
			dataType: "text",
			success:function (data) {
			data=JSON.parse(data);
                if (data.Code == 0){
                    //cross_login(data.data.ssotoken, 'sso_callback');
                    sso_callback();
                }
                else {
                if(data.Code == 1)
                {
                    addFocus($('#mobile'), data.Message);
                }
                else
                {
                    addFocus($('#validcode'), data.Message);
                }
                    $("#captcha_img").click();
                    $("#txtcaptcha").val("");
                }
            }
            });
            return false;
        });

        $('#getcode').click(function () {
            clearFocus($('#getcode'))
            //验证手机号
            var mobile = $('#mobile').val();
            //图片验证码参数获取
            var Code = $("#txtcaptcha").val();
            if (!mobile_regex.test(mobile)) {
                addFocus($('#mobile'), '请输入正确的手机号');
                return false;
            } 
            else if (nullcheck.test(Code)) {
                addFocus($("#txtcaptcha"), "请输入图片验证码");
                return false;
            }
            else
            {
                $.ajax({
			        url:"SMSAjax.aspx",
			        type:"get",
			        data:"Action=CheckCode&CheckCode="+Code,
			        dataType: "text",
			        success: function (data) {
				        if(data!=2){
					         send_validation_code();
				        }else{
	        	             addFocus($("#txtcaptcha"), "验证码有误，请重新输入。");
                             return false;
				        }
	                },
	                error: function (msg) {}
		        });
		    }
            clearFocus($('#mobile'))
            return false;
        });
    });
</script>

</div>
<footer>
        <a href="javascript:;" class="fr link_top"><i class="icon_arrow_up"></i>回顶部</a>
        <a href="#" target="_blank">意见反馈</a>
        <div class="version_box">
            <div class="version_class clearbox">
                <span>触屏版</span>
                <a href="http://test.mostool.com" class="link_pc">电脑版</a>
            </div>
        </div>
        <p class="txt_c pb5"><span class="gray_9">CopyRight 2015 © <a href="http://mostool.com">上海孟特</a></span></p>
</footer>

</body>
</html>
