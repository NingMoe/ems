<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reg.aspx.cs" Inherits="MobileEMS.Reg" %>
<!doctype html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no,minimal-ui">
    <meta name="MobileOptimized" content="320">
    <meta http-equiv="cleartype" content="on">
    <title>上海孟特 注册上海孟特</title>
    <link rel="stylesheet" href="css/css.css" type="text/css">
    <script type="text/javascript" src="js/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="js/pass_mobile.js"></script>
    <script type="text/javascript" src="js/other_pass_2012.js"></script>
    </head>
<body>
    <div id="wrapper">
    <header>   
        
        <a href="javascript:history.back();" class="icon_back"></a>
       
        <a href="Login.aspx" class="btn_login">登录</a>
        注册
    </header>


<fieldset class="p10 regiter_box mt10">
    <div class="input_box">
        <input type="text" class="c_input phone valid" placeholder="手机号" id="mobile" pattern="^((1[3|5|8|4][0-9]\d{8})|(170\d{8}))$" required>
        <i class=""></i>
        <p class="red mt5 hide">您的手机已注册，请换个号码试试</p>
    </div>

    
        <div class="input_box" id="Code_box">
            <input name="txtcaptcha" type="text" class="c_input code" id="txtcaptcha" placeholder="图片验证码" maxlength="8" style="width:55%;" required>            
            <img id="captcha_img" onclick="this.src='Code.aspx?v=<%=DateTime.Now %>'" src="Code.aspx?v=<%=DateTime.Now %>" style="width: 30%; height: 45px; margin-top: 3px; cursor: pointer" alt="" title="" class="fr">
            <p class="red" id="error_captcha">&nbsp;</p>
        </div>
    

    <div class="input_box">
        <input type="text" class="c_input code fl" width="100" placeholder="动态码" id="validcode" pattern="^\d{6}$" required>
        <a href="javascript:;" class="btn btnWhite btnGetCode fl" id="getcode">获取动态码</a>
        <div class="clearbox"></div>
        <p class="red mt5 hide">您的动态码输入错误，请重新获取新的动态码</p>
    </div>

    <div class="input_box">
        <input type="password" class="c_input psw" minlength="6" maxlength="20" placeholder="字母数字字符(._@#)中至少两种" id="password" required>
        <input type="text" id="password2" style="display: none;" minlength="6" maxlength="20" class="c_input psw" placeholder="6-16位，字母、字符(._@#)、数字中至少包含两种">
        <a href="javascript:;" id="btn_showpass"></a>
        <p class="red mt5 hide">您的密码不足6位，请重新输入</p>
    </div>
    <div class="input_box">
        <select name="BrandId" id="BrandId" class="c_input">
          <option value="">请选择品牌</option>
          <option value="668">长安福特</option>
          <option value="670">长安铃木</option>
          <option value="671">东风本田</option>
          <option value="706">长安汽车</option>
          <option value="685">一汽丰田</option>
          <option value="676">东南汽车</option>
          <option value="705">昌河铃木</option>
          <option value="674">东风雪铁龙</option>
          <option value="677">广汽本田</option>
          <option value="669">北京现代</option>
          <option value="686">一汽红旗</option>
          <option value="684">一汽奔腾</option>
          <option value="704">宝马中国</option>
          <option value="673">日产汽车</option>
          <option value="675">东风悦达起亚</option>
          <option value="682">雪佛兰</option>
          <option value="703">江淮汽车</option>
          <option value="702">东风轻卡</option>
          <option value="694">东风风神</option>
          <option value="687">一汽马自达</option>
          <option value="701">东风日产启辰</option>
          <option value="672">东风标致</option>
          <option value="681">沃尔沃</option>
          <option value="683">英菲尼迪</option>
          <option value="678">广汽丰田</option>
          <option value="688">郑州日产</option>
          <option value="679">观致</option>
          <option value="689">DS</option>
          <option value="690">通用别克</option>
          <option value="700">海马</option>
          <option value="699">奔驰</option>
          <option value="692">福田</option>
          <option value="691">江铃</option>
          <option value="695">奥迪</option>
          <option value="693">东风风度</option>
          <option value="698">长安轿车</option>
          <option value="697">上海大众</option>
          <option value="696">奇瑞</option>
        </select>
        <input type="text" id="Text1" style="display: none;" minlength="6" maxlength="20" class="c_input psw" placeholder="6-16位，字母、字符(._@#)、数字中至少包含两种">
        <a href="javascript:;" id="A1"></a>
        <p class="red mt5 hide">您的密码不足6位，请重新输入</p>
    </div>
    <p class="mb10">
        <input type="checkbox" id="agreement" class="checkbox" checked=""><label for="agreement" class="mr5"></label><label for="agreement" class="">我已经阅读并同意<a href="#" class="green">《上海孟特用户协议》</a></label></p>
    <p class="red mt5 hide agree_error">同意《上海孟特用户协议》才能注册</p>
    <a href="#" class="btn btnGreen" id="btn_register">立即注册</a>
</fieldset>

<section class="register_suc p10 mb40 clearbox hide">
    <p class="mb20 big green txt_c mt40"><i class="icon_valid vmiddle mr10"></i>恭喜您注册成功</p>
    <p class="mb10">
        欢迎您加入上海孟特学习平台，和千万行业人才一起学习，一起进步！您默认的用户名为：<span class="green"></span>
    </p>
    <p class="mb20">
        您可以使用用户名密码登陆，也可以使用手机登陆。
    </p>
    <a href="javascript:;" class="btn btnWhite fl" id="btnSkip">直接访问页面</a>
</section>


<div class="msgMode hide">
    <div class="msgMode_con">
        <div class="warnMsg clearbox">
            <p class="mb20">您的用户名今后将不可再次修改，确定要使用这个用户名么？</p>
            <a href="javascript:;" onclick="msgModeClose()" class="btn btnWhite halfbtn fl">我再看看</a>
            <a href="#" class="btn btnGreen halfbtn fr">确定</a>
        </div>
    </div>
</div>
<script type="text/javascript" src="js/hjlogin.js"></script>



<script type="text/javascript">
    var mobile_is_used = 0;
    var isApp = 0;
    var token  = '5a458f3d872f5aedeb3a7f6d8316ef1d';
    $.ajax({'async':false});
    function msgModeClose(){
        $(".msgMode").fadeOut(200);
    }
    var nullcheck = /^[\s\n\r\t]*$/;
    var mobile_regex = /^0?((1[3|5|8|4][0-9]\d{8})|(17\d{9}))$/;
    var password_regex=/(?=^[0-9a-zA-Z._@#]{6,16}$)((?=.*[0-9])(?=.*[^0-9])|(?=.*[a-zA-Z])(?=.*[^a-zA-Z]))/;
    function check_mobile() {
        
        mobile_is_used = 0;
        var mobile = $('#mobile').val();
        if (!mobile_regex.test(mobile)) {
            addFocus($('#mobile'), '请输入正确的手机号');
            return false;
        }
        $.get('handler/UserHandler.ashx?Action=CheckMobile&mobile=' + mobile, function (data) {
            data=JSON.parse(data);
            if (data.code == 0) {
                if (data.data) {
                    clearFocus($('#mobile'));
                    mobile_is_used = 0;
                    $('#mobile').next().addClass('icon_valid_b');
                }
                else {
                    addFocus($('#mobile'), '您的手机已注册，请换个号码试试');
                    $('#mobile').next().addClass('icon_error');
                    mobile_is_used = 1;
                    return false;
                }
            }
            else {
                addFocus($('#mobile'), data.message);
                $('#mobile').next().addClass('icon_valid_b');
                return false;
            }
        });
    }


    var is_send = false;
   
    var time = 120 , timeConst = 120;
    var interval;
    



    function send_validation_code() {
        if(is_send || mobile_is_used) return;
        //验证图片验证码
        clearFocus($("#txtcaptcha"));
        is_send = true;
        var mobile = $('#mobile').val();
        $('#getcode').text('动态码获取中');

        var Code = $("#txtcaptcha").val();
        var ImgToken = $("#token").val();

        $.get("SMSAjax.aspx?Action=SendCheckCode&SendType=Reg&Mobile="+mobile, function (data) {
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
//            else if (data.code == 1101)
//            {
//                addFocus($('#mobile'), '您的手机已注册，请换个号码试试');
//                $('#getcode').text('获取动态码');
//                is_send = false;
//            }
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

            //刷新图片验证码
            if(!is_send){
                $("#captcha_img").click();
                $("#txtcaptcha").val("");
            }

        });
    }

    function check_password() {
        var _password = $.trim($('#password').val());
        if (_password == '' || _password.lenght<6)
            return [false, '您的密码不足6位，请重新输入'];
        if ( _password.lenght > 16)
            return [false, '密码长度不能大于16位，请重新输入'];
        if(!password_regex.test(_password))
            return [false, '6-16位，字母、字符(._@#)、数字中至少包含两种'];
        return [true, ''];
    }

    $(function () {
        if(isApp)
        {
            $('#btnSkip').html('直接访问应用').off('click').click(function(){
                window.location.href='/m/redirect.aspx';
                return false;
            });
            
        }
        $('#password').keydown(function () {
            
            $('#password2').val($(this).val());
        }).keyup(function () {
            
            $('#password2').val($(this).val());
        });
        $('#password2').keydown(function () {
            $('#password').val($(this).val());
        }).keyup(function () {
            $('#password').val($(this).val());
        });

        $("#btn_showpass").click(function () {

            if ($(this).hasClass("showpass")) {
                $(this).removeClass("showpass");
                $("#password").show();
                $("#password").val($("#password2").val());
                $("#password2").hide();
            }
            else {
                $(this).addClass("showpass");
                $("#password").hide();
                $("#password2").val($("#password").val());
                $("#password2").show();
            }
        });


        $('#mobile').blur(function () {
            check_mobile();
        }).focus(function () {
            clearFocus($('#mobile'));
            $('#mobile').next().removeClass('icon_error').removeClass('icon_valid_b');
        });


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

        $('#password').focus(function () {
            clearFocus($('#password'));
        }).blur(function () {
            var _check = check_password();
            if (!_check[0]) {
                addFocus($('#password'), _check[1]);
            }
        });

        $("#txtcaptcha").focus(function () {
            clearFocus(this);
        }).blur(function () {
            if (nullcheck.test($(this).val())) {
                addFocus(this, '请输入右侧的图片验证码');
            }
        })
        
        $('#BrandId').focus(function () {
            clearFocus($('#BrandId'));
        }).blur(function () {
            if (nullcheck.test($(this).val())) {
                addFocus($('#BrandId'), '请选择所属品牌');
            }
        });

        $('#btn_register').click(function () {

            var data = {};
            data["mobile"] = $('#mobile').val();
            data['validcode'] = $('#validcode').val();
            data['password'] = $('#password').val();
            data['BrandId'] = $('#BrandId').val();
            param = $.param(data, true);
            var IsPass = true;
            if ($("#Code_box").length > 0) {
                clearFocus($("#txtcaptcha"));
                if (nullcheck.test($("#txtcaptcha").val())) {
                    addFocus($("#txtcaptcha"), '请输入图片验证码');
                    IsPass = false;
                }
            }
            if (!/^\d{6}$/.test(data['validcode'])) {
                addFocus($('#validcode'), '请输入手机验证码');
                IsPass = false;
            }
           
            if (data['password'].length < 6 || data['password'].length > 16 || nullcheck.test(data['password'])|| !password_regex.test(data['password'])) {
                addFocus($('#password'), '请输入密码（6-16位，字母、字符(._@#)、数字中至少包含两种)');
                IsPass = false;
            }
            if(nullcheck.test(data['BrandId']))
            {
                addFocus($('#BrandId'), '请选择所属品牌');
                IsPass = false;
            }
            if(!IsPass){
                return false;
            }
            if (!$('#agreement').is(':checked')) {
                $('p.agree_error').show();
                return false;
            }
            var isredirect = 0;
                $.ajax({
			    url:"SMSAjax.aspx",
			    type:"get",
			    data:"Action=Ver&SendType=Reg&Mobile="+$('#mobile').val()+"&CheckCode="+data['validcode']+"&BrandId="+data['BrandId']+"&Password="+data['password'],
			    dataType: "text",
			    success:function (data) {
    			data=JSON.parse(data);
                    if (data.Code == 0) {                    
                        $('fieldset').hide();
                        $('section').show();
                        $('section span.green').html(data.data.Name);
                        $('header a.icon_back').hide();
                        $('header a.btn_login').text('退出').attr('href','Logout.aspx');
                    }
                    else {
                        addFocus($('#validcode'), data.Message);
                        $("#txtcaptcha").val("");
                    }
                }
                });
            return false;
        });
        $('#getcode').click(function () {
            var mobile = $('#mobile').val();
            if (!mobile_regex.test(mobile)) {
                addFocus($('#mobile'), '请输入正确的手机号');
                return false;
            }
            if (nullcheck.test($("#txtcaptcha").val())) {
            addFocus($("#txtcaptcha"), "请输入图片验证码");
            return false;
            }
            $.ajax({
			        url:"SMSAjax.aspx",
			        type:"get",
			        data:"Action=CheckCode&CheckCode="+$("#txtcaptcha").val(),
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
            return false;
        });

        $("#btnSkip").click(function(){
            
            if(!isApp)
                //$(".msgMode").fadeIn(200);
            {
                window.location.href='CourseCenter.aspx?Action=PostCourse';
            }
        });
        
        $("select").bind('click', function() {
			var o = $(this);			
			if (o.val() == '') {
				o.css({"color":"#CCC"});
			} else {
				o.css({"color":"#000"});
			}
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
        <p class="txt_c pb5"><span class="gray_9">CopyRight 2015 © <a href="http://mostool.com/">上海孟特</a></span></p>
</footer>
</body></html>
