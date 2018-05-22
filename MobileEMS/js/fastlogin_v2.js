///*
// * fastlogin_v2.js
// */

var login = {
    //兼容v1版本的回调方法    
    doLogin: function (optOrCallback) {
        var opt = null;
        if (typeof (optOrCallback) === 'object') {//配置
            opt = optOrCallback;
        }
        else if (typeof (optOrCallback) === 'function') {//回调
            opt = { callback: optOrCallback };
        }
        window.hjquicklogin.doLogin(opt);
        return false;
    },
    doRegister: function (optOrCallback) {
        var opt = null;
        if (typeof (optOrCallback) === 'object') {//配置
            opt = optOrCallback;
        }
        else if (typeof (optOrCallback) === 'function') {//回调
            opt = { callback: optOrCallback };
        }
        window.hjquicklogin.doRegister(opt);
        return false;
    },
    checkInfo: function (userID, passHost) {
        register_tooltip.userID = userID;
        register_tooltip.passHost = passHost;
        register_tooltip.check();
    }
};

window.hjquicklogin = {
    init: function (opt) {
        var me = this;
        me.control = me.control || hui.Control.create('HJ_QuickLogin', {
            formName: 'hj_quicklogin',
            top: 0.33,
            className: 'hj001_login_box'
        });
        // config
        me.control.getPassName = function () {
            var subName = 'http://pass';
            return subName;
        };
        me.control.source = opt && opt.source ? opt.source : 'niming';
        if (opt && opt.callback) {
            me.control.onLoginSuccess = opt.callback;
        }
        else {
            me.control.onLoginSuccess = function (result) {
                setTimeout(function () {
                    window.location.reload();
                }, 800);
            };
        }
        me.control.onRegisteSuccessMobile = function (result) {
            setTimeout(function () {
                window.location.reload();
            }, 800);
        };

        me.control.monitorList['hj_reg'] = function () {/* 注册沪江页展示            */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 4) } };
        me.control.monitorList['hj_reg_login'] = function () {/* 登录沪江页展示            */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 2) } };
        me.control.monitorList['hj_reg_mobile_success'] = function () {/* 手机注册成功页            */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 10) } };
        me.control.monitorList['hj_reg_email_success'] = function () {/* 邮箱注册成功页            */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 11) } };


        me.control.monitorList['hj_reg_login_submit'] = function () {/* 登录按钮事件              */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 5) } };
        me.control.monitorList['hj_reg_mobile_submit'] = function () {/* 手机号注册按钮事件        */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 6) } };
        me.control.monitorList['hj_reg_email_submit'] = function () {/* 邮箱注册按钮事件          */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 7) } };
        me.control.monitorList['hj_reg_email_success_active'] = function () {/* 邮箱激活按钮事件          */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 8) } };
        me.control.monitorList['hj_reg_email_continue'] = function () {/* 继续浏览按钮事件          */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 9) } };
        me.control.monitorList['hj_reg_close'] = function () {/* 关闭按钮事件              */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 12) } };
        me.control.monitorList['hj_reg_mobile_sendsms'] = function () {/* 获取手机验证码按钮事件    */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 13) } };

        me.control.monitorList['hj_reg_mobile_email'] = function () {/* 在手机注册页的邮箱注册点击*/ if (typeof SendEvent !== 'undefined') { SendEvent(2, 16) } };
        me.control.monitorList['hj_reg_email_mobile'] = function () {/* 在邮箱注册页的手机注册点击*/ if (typeof SendEvent !== 'undefined') { SendEvent(2, 15) } };
        me.control.monitorList['hj_reg_login_qq'] = function () {/* QQ_连接登录               */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 17) } };
        me.control.monitorList['hj_reg_login_weibo'] = function () {/* 新浪微博_连接登录         */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 18) } };
        me.control.monitorList['hj_reg_login_renren'] = function () {/* 人人_连接登录             */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 19) } };
        me.control.monitorList['hj_reg_login_baidu'] = function () {/* 百度_连接登录             */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 20) } };
        me.control.monitorList['hj_reg_login_douban'] = function () {/* 豆瓣_连接登录             */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 21) } };
        me.control.monitorList['hj_reg_login_zhifu'] = function () {/* 支付宝_连接登录           */ if (typeof SendEvent !== 'undefined') { SendEvent(2, 22) } };
    },
    doLogin: function (opt) {
        var me = this;
        !me.control && me.init(opt);

        me.control.showDialog('hj_reg_login');
    },
    doRegister: function (opt) {
        var me = this;
        !me.control && me.init(opt);
        me.control.showDialog('hj_reg_mobile');
    }
};

var register_tooltip = {
    userID: 0,
    passHost:'pass.hjclass.com',//默认采用hjclass
    check: function () {
        var userID = parseInt(register_tooltip.userID);        
        if (userID > 0) {
            $.ajax({
                type: 'GET',
                url: 'http://pass.hjclass.com/quick/account/?act=check_tooltip&userid=' + userID,
                data: '{}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'jsonp',
                jsonp: 'callback',
                success: function (result) {
                    if (result.data.showtips) {
                        //显示标签                        
                        $('.pass_register_tooltip').show();
                        $('.pass_register_tooltip').attr('href', 'http://' + register_tooltip.passHost + '/q/profile/?template=class&returnurl=' + encodeURIComponent(document.location.href));
                        $('.pass_register_tooltip').click(function () {
                            if (typeof SendEvent !== 'undefined') { SendEvent(2, 23) }
                        });
                    }
                }
            });
        }
    }
};