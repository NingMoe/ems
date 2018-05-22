using System;
using System.Data;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFuShop.Common;

namespace MobileEMS.Pay.AliPay
{
    public partial class Pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayConfig payConfig = new PayConfig();
            string out_trade_no = string.Empty; //订单号
            string subject = "";	//商品名称
            string body = "";		//商品描述  
            string price = "";  //单价
            string total_fee = "";//总价
            string quantity = ""; //数量
            string show_url = "";//展示地址
            string logistics_fee = ""; //邮费
            string action = RequestHelper.GetQueryString<string>("Action");
            int userID = Cookies.User.GetUserID(true);
            switch (action)
            {
                //case "Apply":
                //    int applyID = RequestHelper.GetQueryString<int>("ApplyID");
                //    UserRechargeInfo userRecharge = UserRechargeBLL.ReadUserRecharge(applyID, userID);
                //    out_trade_no = userRecharge.Number;
                //    subject = "在线冲值：" + userRecharge.Number;
                //    body = "在线冲值";
                //    price = userRecharge.Money.ToString();
                //    quantity = "1";
                //    show_url = "http://" + Request.ServerVariables["Http_Host"];
                //    logistics_fee = "0";
                //    break;
                case "PayOrder":
                    int orderID = RequestHelper.GetQueryString<int>("OrderID");
                    OrderInfo order = OrderBLL.ReadOrder(orderID, userID);
                    out_trade_no = order.OrderNumber;
                    subject = "订单号：" + order.OrderNumber;
                    body = "在线支付";
                    total_fee = order.ProductMoney.ToString(); //(order.ProductMoney - order.FavorableMoney + order.ShippingMoney + order.OtherMoney - order.Balance - order.CouponMoney).ToString();
                    quantity = "1";//如果使用total_fee，quantity值为1
                    show_url = "http://" + Request.ServerVariables["Http_Host"];
                    logistics_fee = "0";
                    break;
                default:
                    break;
            }
            string payment_type = "1";                  //支付类型	            
            string return_url = "http://" + Request.ServerVariables["Http_Host"] + "/Plugins/Pay/AliPay/Return.aspx"; //服务器返回接口
            string notify_url = "http://" + Request.ServerVariables["Http_Host"] + "/Plugins/Pay/AliPay/Notify.aspx"; //服务器通知接口
            string logistics_type = "POST";
            string logistics_payment = "BUYER_PAY";

            AliPay ap = new AliPay();
            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", payConfig.Partner);
            sParaTemp.Add("_input_charset", payConfig.InputCharset);
            sParaTemp.Add("payment_type", payment_type);
            sParaTemp.Add("notify_url", notify_url);
            sParaTemp.Add("return_url", return_url);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("body", body);
            sParaTemp.Add("show_url", show_url);
            sParaTemp.Add("quantity", quantity);
            //PC
            if (!UserAgentHelper.IsMobile(Request.UserAgent))
            {
                sParaTemp.Add("seller_email", payConfig.SellerEmail);
                sParaTemp.Add("service", "create_direct_pay_by_user");
                sParaTemp.Add("anti_phishing_key", "");
                sParaTemp.Add("exter_invoke_ip", "");
            }
            else//移动
            {
                sParaTemp.Add("seller_id", payConfig.SellerId);
                sParaTemp.Add("service", "alipay.wap.create.direct.pay.by.user");
                sParaTemp.Add("it_b_pay", "");
                sParaTemp.Add("extern_token", "");
            }

            string aliay_url = ap.CreatUrl(sParaTemp);

            Response.Redirect(aliay_url);
        }

    }
}