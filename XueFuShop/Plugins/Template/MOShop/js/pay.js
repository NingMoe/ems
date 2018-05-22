usingNamespace("HuJiang.Order")["Pay"] = {
    //支付偏好设置
    payPreference: {
        paymethod: '',
        platform: '',
        bankcode: '',
        iscredit: 0,
        isdirect:0
    },
    //支付数据信息
    payDataInfo: {
        bankcode: '',
        platform: '',
        isdirect: 0,
        iscredit:0
    },
    returnUrl: '', //业务方的成功页
    showUrl:'', //业务方显示订单详情页
    totalFee: 0, //订单支付金额
    orderId: 0, //订单号,
    orderType:0, //订单类型
    productName:'', //商品名称
    icbcRate: [], //工行费率
    cmbRate: [],  //招行费率
    ccbRate: [],  //建行费率
    bocRate:[], //中行费率
    instalmentType: 'li_409',  //分期付款类型
    defaultInstalment:1,  //默认分期数
    /*初始化收银台*/
    initCash: function () {

        HuJiang.Order.Pay.render();

        //ie6-8效果兼容
        if (!$.support.leadingWhitespace) {
            var $iptRadio = $('.bank_ico').prev();
            $('.bank_cont').delegate('li', 'click', function () {
                $iptRadio.attr('checked', false);
                $(this).find('>input').attr('checked', true);
            });
        };

        /*达到10个以上时显示查看更多*/
        if ($("#tmpl_more_bank").length > 0) {
            $('li:gt(9)', 'ul.bank_list').addClass('hide').parent().after($("#tmpl_more_bank").tmpl());
        }

        //设置用户支付偏好
        HuJiang.Order.Pay.settingPayPreference();

        //页面事件绑定
        HuJiang.Order.Pay.bindEvent();
    },

    /*初始化支付成功页*/
    initSuccess: function() {
        $('a.btn_true').attr('href', HuJiang.Order.Pay.returnUrl);
        HuJiang.Order.Pay.showSuccessDialog();
    },

    /*页面效果渲染*/
    render: function () {
        //假如没有优惠提示信息,则显示角标
        $('.ebank_name i').each(function() {
            if ($(this).text() == null || $(this).text() == '') {
                $(this).remove();
            } else {
                $(this).addClass('icon_activity');
            }
        });
        //优惠券提示
        if (HuJiang.Order.Pay.orderType == 21) {
            $('.dingdan_info h3').text('订单已提交，请在24小时内完成支付!');
        }
        //页面公告
        HuJiang.Order.Pay.loadNoticeBoard();
    },

    /*加载收银台公告信息*/
    loadNoticeBoard: function() {
        $.ajaxSetup({
            headers: {
                'sign': 'xxxxxxxxxx'
            }
        });
        var url = '/api/v2.ashx?opt=get_cash_noticeboard&type=0' + '&r=' + Math.random() + "&appkey=a78b6cd6b5a0173ed9e4afbc7ac9b588&nonceid=" + Math.random();
        $.getJSON(url, function (o) {
            if (o.code == 0) {
                var notice = o.data;
                if (notice != '' && notice != null) {
                    $('#noticeboard-tips').text(notice);
                    $('#noticeboard-wrapper').show();
                    //$('#wrapper').css('margin-top', '5px');
                }
            }
        });
    },

    /*页面dom元素事件绑定*/
    bindEvent: function () {

        /*显示更多事件绑定*/
        $('.span_tips').on('click', function () {
            HuJiang.Order.Pay.showMore($(this));
        });

        /*立即支付*/
        $('#submit_btn').on('click', function () {
            HuJiang.Order.Pay.doSubmitEevent();
        });

        /*付款方式tab切换*/
        $('.bank_change li').on('click', function () {
            var self = $(this);
            var index = $('.bank_change li').index(this);
            HuJiang.Order.Pay.choosePayType(index);

            if (index == 0) { //支付平台
                SendEvent(33, 462);
            } else if (index == 1) { //信用卡
                SendEvent(33, 463);
            } else if (index == 2) { //借记卡
                SendEvent(33, 464);
            } else if (index == 3) { //信用卡分期
                SendEvent(33, 465);

                HuJiang.Order.Pay.calculateRate(HuJiang.Order.Pay.instalmentType);
            }
        });

        //关闭提示框按钮事件绑定
        $('.close_box').click(function () {
            easyDialog.close();
        });

        //提示框点击支付完成事件绑定
        $('a.btn_true').click(function () {
            HuJiang.Order.Pay.showTipsDialog();
        });

        //选择银行事件绑定
        //$('.ebank_name').unbind('click').click(function (e) {
        $('.ebank_name').on('click',function(e){   
            var self = $(this);
            
            //主要针对分期付款
            if (self.hasClass('checked')) {
                HuJiang.Order.Pay.instalmentType = self.attr('id');
            }
            if (typeof (HuJiang.Order.Pay.instalmentType) != 'undefined') {                
                HuJiang.Order.Pay.calculateRate(HuJiang.Order.Pay.instalmentType);
            }

            $(".ebank_name").removeClass("checked");
            self.addClass("checked");

            $('#submit_btn').removeClass('disabled');
            HuJiang.Order.Pay.selectBankEvent(self);
        });

        //银行选择移出效果
        $('.ebank_name').on('mouseout', function() {
            $(this).removeClass('current');
        });

        //银行选择移入效果
        $('.ebank_name').on('mouseover', function () {
            $(this).addClass('current');
        });

        //分期付款选择分期数事件绑定
        $(".stageList li").on("click", function () {
            $(".stageList li").removeClass("checked");
            $(this).toggleClass("checked");

            //重新定位选择银行
            $(".bank_list .ebank_name").each(function (i, item) {
                if ($(item).hasClass('checked')) {
                    HuJiang.Order.Pay.instalmentType = $(item).attr('id');
                }
            });
            HuJiang.Order.Pay.calculateRate(HuJiang.Order.Pay.instalmentType);

            $('#instalment').val($(this).attr('data-value'));

            SendEvent(33, 470, "{\"nper\":\"" + $(this).attr('data-value') + "\"}");
        });

        //分期付款选择分期数循环绑定
        $(".stageList li").each(function (i, m) {
            if ($(m).attr('data-rate') == HuJiang.Order.Pay.defaultInstalment) {
                $(m).addClass('checked');
            }
        });

        //重新选择支付方式
        $('.other_type').on('click', function() {
            $('#overlay,#dialog_confirm').hide();
        });

        //订单详情事件绑定
        $('.order-detail').on('click', function() {
            SendEvent(33, 467, "{\"orderid\":\"" + HuJiang.Order.Pay.orderId + "\"}");
        });

        $('#noticeboard-close').on('click', function() {
            $("#noticeboard-wrapper").slideUp();
            //$('#wrapper').css('margin-top', '0px');
        });
    },

    

    /*立即支付触发事件*/
    doSubmitEevent: function () {

        if ($('#submit_btn').hasClass('disabled')) {
            return false;
        }
        /*纠正用户支付偏好数据*/
        $('.bank_change li').on('click', function() {            
            var index = $('.bank_change li').index(this);
            $('#paymethod').val(index);
            if (index != 3) {
                $('#iscredit').val(0);
            } else {
                $('#iscredit').val(1);
            }
        });

        //提交支付表单数据
        $('#submit_pay').submit();
        easyDialog.open({
            container: 'dialog_confirm',
            lock: true
        });
    },

    /*选择支付类型*/
    choosePayType: function (index) {
        $('.bank_change li').each(function() {
            $(this).removeClass('current');
        });

        $('li.ebank_name').removeClass('current checked');
        $('input[type="radio"]').removeAttr('checked');

        $('.bank_change li').eq(index).addClass('current');
        $('.select_result .bank_cont').addClass('hide').eq(index).removeClass('hide');

        //如果是分期付款，则选中
        if (index == 3) {
            $('#input_409').prop('checked', 'checked');
            $('#li_409').addClass('checked');
            $('#platform').val('CMB-B2C');
            $('#instalment').val('3');
            $('#bankcode').val('CMBB2CInstalment');
            $('#submit_btn').removeClass('disabled');
        }
        else {
            $('#input_409').prop('checked', '');
            $('#li_409').removeClass('current checked');
            $('#platform').val();
            $('#instalment').val('');
            $('#bankcode').val('');
            $('#submit_btn').addClass('disabled');
        }
        $('#paymethod').val(index);
    },

    reselect: function () {
        var selectResult = hui.c('select_result')[0], index = selectResult.current || 0;
        hui.addClass(hui.g('submit_btn'), 'disabled');
        hui.removeClass(hui.c('bank_cont', hui.c('select_result')[0])[index], 'hide');
    },

    /*选择银行事件绑定*/
    selectBankEvent: function(self) {
        HuJiang.Order.Pay.parsePayData(self.attr('data-info'));

        //Paypal支付显示实时汇率
        if (HuJiang.Order.Pay.payPreference.platform.toLowerCase() == 'paypal') {
            $('p.notice').show();
            HuJiang.Order.Pay.setPaypalCurrency('USD', 'CNY');
        } else {
            $('p.notice').hide();
        }

        //填充支付表单数据
        $('#platform').val(HuJiang.Order.Pay.payPreference.platform);
        $('#bankcode').val(HuJiang.Order.Pay.payPreference.bankcode);
        $('#isdirect').val(HuJiang.Order.Pay.payPreference.isdirect);
        $('#iscredit').val(HuJiang.Order.Pay.payPreference.iscredit);
    },

    /*用户支付偏好设置*/
    settingPayPreference: function() {
        if (HuJiang.Order.Pay.payPreference == null) {
            $('#submit_btn').addClass('disabled');
        } else {
            if (HuJiang.Order.Pay.payPreference.paymethod == 3 && HuJiang.Order.Pay.totalFee < 2000) {
                $('#submit_btn').addClass('disabled');
                return;
            }

            $('ul.bank_change li').removeClass('current');
            if ($('ul.bank_change li').eq(HuJiang.Order.Pay.payPreference.paymethod).length == 0) {
                HuJiang.Order.Pay.payPreference.paymethod = 0;
                $('#submit_btn').addClass('disabled');
            }
            $('ul.bank_change li').eq(HuJiang.Order.Pay.payPreference.paymethod).addClass('current');
            $('div.bank_cont').addClass('hide');
            $('div.bank_cont').eq(HuJiang.Order.Pay.payPreference.paymethod).removeClass('hide');
            var banks = $('div.bank_cont').eq(HuJiang.Order.Pay.payPreference.paymethod).find('li.ebank_name ');

            var datainfo = [];
            if (HuJiang.Order.Pay.payPreference.platform == 'unionpay' && HuJiang.Order.Pay.payPreference.paymethod == 0)
                HuJiang.Order.Pay.payPreference.bankcode = '8607';

            $(banks).each(function (i, item) {
                datainfo = $(item).attr('data-info').split(',');

                if (datainfo[0].toLowerCase() == HuJiang.Order.Pay.payPreference.platform.toLowerCase() && datainfo[3] == HuJiang.Order.Pay.payPreference.bankcode) {
                    $(this).prependTo($(this).parent());
                    $(this).find('input').prop('checked', 'checked');
                    HuJiang.Order.Pay.instalmentType = $(this).attr('id');
                    //$(this).addClass('checked current');
                    $('#confirm_pay').removeClass('hide');
                }
            });

            if (HuJiang.Order.Pay.payPreference.iscredit) {
                $('#iscredit').val('1');
            }
            else if (HuJiang.Order.Pay.payPreference.isdirect) {
                $('#isdirect').val('1');
            }
            //默认偏好是分期付款时,默认分期数量为3期
            if (HuJiang.Order.Pay.payPreference.paymethod == 3) {
                $('#instalment').val(3);
                $('#isdirect').val('1');
                $('#iscredit').val('1');
                HuJiang.Order.Pay.calculateRate(HuJiang.Order.Pay.instalmentType);
            }
        }
        if ($('div.bank_cont input:radio:checked').length == 0) {
            $('#submit_btn').addClass('disabled');
            //init form
            $('#platform').val('');
            $('#bankcode').val('');
            $('#isdirect').val('0');
            $('#iscredit').val('0');
        }
        else {
            if (HuJiang.Order.Pay.payPreference != null) {
                $('#platform').val(HuJiang.Order.Pay.payPreference.platform);
                $('#bankcode').val(HuJiang.Order.Pay.payPreference.bankcode);
                $('#isdirect').val(HuJiang.Order.Pay.payPreference.isdirect);
                $('#iscredit').val(HuJiang.Order.Pay.payPreference.iscredit);
                $('#paymethod').val(HuJiang.Order.Pay.payPreference.paymethod);
            }
        }
    },

    /*格式化支付数据.平台,是否信用卡,是否直连,银行编码*/
    parsePayData: function(dataStr) {
        var dataArray = dataStr.split(',');
        if (dataArray.length == 4) {
            HuJiang.Order.Pay.payPreference.bankcode = dataArray[3];
            HuJiang.Order.Pay.payPreference.platform = dataArray[0];
            HuJiang.Order.Pay.payPreference.iscredit = dataArray[1];
            HuJiang.Order.Pay.payPreference.isdirect = dataArray[2];
        }
    },

    /*显示立即支付后的弹出框提示*/
    showTipsDialog: function() {
        $('p.confrim_info').html('<strong>5</strong>秒后跳转到来源页，请稍候......');
        $('div.box_btn,a.other_type ').hide();
        var time = 5;

        var p = setInterval(function () {
            time--;
            time = Math.max(0, time);
            $('p.confrim_info strong').text(time);
            if (time <= 0) {
                clearInterval(p);
                window.location.href = HuJiang.Order.Pay.returnUrl;
            }
        }, 1000);
    },

    /*支付成功后的弹出框提示*/
    showSuccessDialog: function() {
        setTimeout(function () {
            easyDialog.open({
                container: 'dialog_confirm',
                lock: false
            });
            $('p.confrim_info').html('正在同步订单信息，<strong>5</strong>秒后跳转到来源页。');
            var time = 5;
            var interval = setInterval(function () {
                time--;
                time = Math.max(0, time);
                $('p.confrim_info strong').text(time);
                if (time <= 0) {
                    clearInterval(interval);
                    dsp.push('ecom_pay_online', [{ 'p_id': 0, 'p_name': HuJiang.Order.Pay.productName, 'p_class1': HuJiang.Order.Pay.orderId, 'p_price': HuJiang.Order.Pay.totalFee }]).send();
                    window.location.href = HuJiang.Order.Pay.returnUrl;
                }
            }, 1000);
        }, 100);
    },

    /*计算分期费率*/
    calculateRate: function (instalmentType) {
        var stage = $('ul.stageList li.checked').attr('data-value');
        if (typeof (stage) == 'undefined' || stage == 0) {
            stage = 3;
        }
        var rates;
        if (instalmentType == 'li_310') {
            rates = HuJiang.Order.Pay.icbcRate;
        } else if (instalmentType == 'li_409') {
            rates = HuJiang.Order.Pay.cmbRate;
        } else if (instalmentType == 'li_414') {
            rates = HuJiang.Order.Pay.ccbRate;
        } else if (instalmentType == 'li_420') {
            rates = HuJiang.Order.Pay.bocRate;
        }else {
            rates = HuJiang.Order.Pay.cmbRate; //默认招行
        }
        if (rates.length == 0) {
            return;
        }
        var totalFee = parseFloat(HuJiang.Order.Pay.totalFee);

        var dataRate = $('ul.stageList li.checked').attr('data-rate');
        if (typeof(dataRate) == 'undefined') {
            dataRate = 1;
        }
        var rate = rates[dataRate];
        var totalInterest = Utils.mul(totalFee, rate); //总费率

        var firstMonthInterest = totalInterest - parseInt(totalInterest / stage) * (stage - 1);

        var monthInterest = parseInt(totalInterest / stage);

        var firstMonthFee = totalFee - parseInt(totalFee / stage) * (stage - 1);
       
        var monthFee = parseInt(Utils.div(totalFee, stage));

        //用户付款总额
        var userTotalFee = totalFee + totalInterest;

        $('#total_fee').text(userTotalFee.toFixed(2));
        
        //订单金额
        $('#fee').text(totalFee.toFixed(2));

        //用户应付手续费
        $('#poundage').text((totalFee * rate).toFixed(2));

        //首期本金
        $('#first_principal').text(firstMonthFee.toFixed(2));
        //首期手续费
        $('#first_poundage').text(firstMonthInterest.toFixed(2));

        //后续本金
        $('#other_principal').text(monthFee.toFixed(2));

        //后续手续费
        $('#other_poundage').text(monthInterest.toFixed(2));
    },

    /*设置Paypal汇率*/
    setPaypalCurrency: function(fromCurrency,toCurrency) {
        $.ajaxSetup({
            headers: {
                'sign': 'xxxxxxxxxx'
            }
        });
        var url = '/api/v2.ashx?opt=get_paypal_currency&fromCurrency=' + fromCurrency + '&toCurrency=' + toCurrency + '&dealfee=' + HuJiang.Order.Pay.totalFee + '&r=' + Math.random() + "&appkey=a78b6cd6b5a0173ed9e4afbc7ac9b588&nonceid=" + Math.random();
        $.getJSON(url, function (o) {
            if (o.code == 0) {
                var currency = o.data;
                var str = '当前汇率：' + currency.currency + ',应付' + currency.usd + '美元';
                $('p.notice').text(str);
            }
        });
    }
};