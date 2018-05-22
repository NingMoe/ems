<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MobileEMS.Login" %>
<!doctype html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no,minimal-ui">
    <meta name="MobileOptimized" content="320">
    <meta http-equiv="cleartype" content="on">
	<meta name="Keywords" content="">
	<meta name="Description" content="">
	<title>上海孟特 - 登录</title>
    <link rel="stylesheet" href="css/login.css" type="text/css">
	<script type="text/javascript" src="js/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="js/hjlogin.js"></script>
    <script type="text/javascript" src="js/pass_mobile.js"></script>
    <script type="text/javascript" src="js/other_pass_2012.js"></script>
    <style type="text/css">
        #login_con .btn_login { width: 100%; }
        .fr { float: right; }
    </style>
</head>
<body>
    <div id="wrapper"> 
        <div id="login_top">
            <!--<a href="javascript:history.back();" class="icon_back"></a>-->
            登录
            <!--<a class="btn_login" href="Reg.aspx">注册</a>-->
        </div>
        <div id="login_con">
            <div id="userid_line">
                <div id="userid_block">
                    <input tabindex="1" type="text" id="userid" placeholder="用户名" maxlength="50">
                </div>
                <p id="error_username">&nbsp;</p>
            </div>
            <div id="password_line">
                <div id="pass_block">
                    <input tabindex="2" type="password" id="password" placeholder="6-20位，区分大小写" onpaste="return false;" maxlength="20">
                    <input type="text" id="password2" style="display: none;" maxlength="20" onpaste="return false;">
                    <a id="btn_showpass" href="javascript:;">···</a>
                </div>
                <p id="error_password">&nbsp;</p>
            </div>
            <a class="btn_login"  id="loginLink">登录</a>
            <div class="clear"></div>

            <div class="mb20">
                <!--<a href="#" class="link_reset_psw fr">忘记密码&gt;&gt;</a>
                <a href="SMSLogin.aspx" class="link_reset_psw" style="float: left">短信登录&gt;&gt;</a>-->
            </div>
            <div class="clear"></div>
        </div>
        <div style=" text-align:center; margin-top:30px; font-size:16px;">
            <table align="center">
                <tr>
                    <td style="padding-right:10px; text-align:right;">上海加禾汽修服务<br>上海孟特管理咨询</td>
                    <td style=" border-left:1px solid #CCC; text-align: left; padding-left:10px;">联合<br>出品</td>
                </tr>
            </table>
        </div>
    </div>
    <footer>
            <%--<a href="javascript:;" class="fr link_top"><i class="icon_arrow_up"></i>回顶部</a>
            <a href="#" target="_blank">意见反馈</a>--%>
            <div class="version_box">
                <%--<div class="version_class clearbox">
                    <span>触屏版</span>
                    <a href="http://ems.mostool.com" class="link_pc">电脑版</a>
                </div>--%>
            </div>
            <p class="txt_c pb5"><span class="gray_9">CopyRight 2015-2018 © <a href="http://mostool.com/">上海孟特</a></span></p>
    </footer>
    <script type="text/javascript">
    var account = '';
    function lightBorder(obj) {
        $('#' + obj).focus();
        $('#' + obj).parent().addClass('errorborder')
    }

    function SignInFailed_Process(data) {
        if (data.data == 1 && typeof (HJCaptcha) == 'undefined') {
            window.location.reload();
            return;
        }
        if (data.code == 1) {
            lightBorder('userid');
            $('#userid').focus();
            $('#error_username').show().html("手机号、邮箱或用户名不正确");
        }
        else if (data.code == 2) {
            lightBorder('password');
            if (typeof (HJCaptcha) != 'undefined')
                $('#captcha_img').attr('src', HJCaptcha.img + '&r=' + Math.random());
            $('#password').focus().val('');
            $('#txtcaptcha').val('');
            $('#error_password').show().html("帐号与密码不匹配");
        }
        else if (data.code == 3) {
            $('#error_username').html("您登录的账号被锁定，如有疑问可点击：<a href='#' class='blue' target='_blank'>账号申诉</a>").show();
        }
        else if (data.code == 10) {
            $('#error_username').html("您登录的账号被锁定，如有疑问可点击：<a href='#' class='blue' target='_blank'>账号申诉</a>").show();
        }
        else if (data.code == 11) {
            $('#error_captcha').html("请输入正确的验证码").show();
            if (typeof (HJCaptcha) != 'undefined')
                $('#captcha_img').attr('src', HJCaptcha.img + '&r=' + Math.random());
            $('#txtcaptcha').focus().val('');
        }
        else {
            $('#error_username').html('登录超时，请刷新浏览器再试。').show();
        }
    }

    function SignInEnd_Process() {
        window.location.href = '#';
    }

    $(function () {
        $('input').focus(function () { $(this).parent().addClass('focusborder') }).blur(function () { $(this).parent().removeClass('focusborder'); });
        $(".link_top").click(function(){
            setTimeout("window.scrollTo(0,0)",300);
        });
        var back_url = '';
        $('#login_top a.btn_back').attr('href', back_url);
        setTimeout(scrollTo, 0, 0, 0);
        if (account) {
            $('#userid').val(account);
        }

        $('#txtcaptcha').blur(function () {
            if ($(this).val != '')
                $('#error_captcha').html('');
        });

        $('#userid').focus(function () {
            $('#error_username').html('');
        }).blur(function () {
            if ($(this).val() == '')
                $('#error_username').html('请输入您的用户名、邮箱或手机号');
        }).keydown(function () { $('#error_username').html(''); }).keyup(function () { $('#error_username').html(''); });


        $('#password').focus(function () {
            $('#error_password').html('');
        });

        $('#password,#password2').blur(function () {
            if ($('#password').val() == '' && $('#password2').val() == '')

                $('#error_password').html('请输入您的密码');
        });

        $('input').blur(function () {
            if ($(this).val() != '') {
                $(this).parent().removeClass('errorborder');
                $(this).nextAll('p').html('');
            }
        });

        $('#loginLink').click(function () {
            var btn = $(this);
            var username = $.trim($('#userid').val());
            var password = $('#password').val() || $('#password2').val();
            var token = $('#token').val() ? $('#token').val() : '';
            var code = $('#txtcaptcha').val() ? $('#txtcaptcha').val() : '';
            $('#error_username,#error_password').html('');

            if (username == '') {
                $('#userid').focus();
                lightBorder('userid');
                $('#error_username').show().html("请输入您的用户名");
                return false;
            }
            if (password == '') {
                lightBorder('password');
                $('#password').focus();
                $('#error_password').show().html("请输入您的密码");
                return false;
            }
            username = encodeURIComponent(username);
            password = password;
            $('#error_username,#error_password').html('');
            $(btn).html("正在登录...");
            $(btn).addClass('unclick');
            $.post("Ajax.aspx?Action=Login",{UserName:username,PassWord:password}, function(data){
                var josndata=JSON.parse(JSON.parse(data).d);
                if(josndata.Success=="true"){
                    location.href=josndata.Url;
                }else{
                    $('#error_username').show().html(josndata.Intro);
                    $(btn).html("登录");
                    $(btn).removeClass('unclick');
                }
            });
            return false;
        });
        
        $(".icon_more_login").click(function () {
            $(".more_login_box").show().animate({ "opacity": "1" }, 300);
            $(this).hide();
        });

        $("#btn_showpass").click(function () {
            if ($(this).hasClass("showpass")) {
                $(this).removeClass("showpass");
                $(this).delay(500).html("···");
                $("#password").show();
                $("#password").val($("#password2").val());
                $("#password2").hide();
            }
            else {
                $(this).addClass("showpass");
                $(this).delay(500).html("ABC");
                $("#password").hide();
                $("#password2").val($("#password").val());
                $("#password2").show();
            }
        });
    });
    </script>
</body>
</html>
