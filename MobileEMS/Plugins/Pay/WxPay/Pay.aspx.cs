using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WxPayAPI;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFuShop.Common;

namespace MobileEMS.Pay.WxPay
{
    public partial class Pay : System.Web.UI.Page
    {
        protected string wxJsApiParam = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Log.Info(this.GetType().ToString(), "page load");
            if (!IsPostBack)
            {
                //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
                JsApiPay jsApiPay = new JsApiPay(this);
                //调用【网页授权获取用户信息】接口获取用户的openid和access_token
                jsApiPay.GetOpenidAndAccessToken();
                //jsApiPay.openid = openid;

                //JSAPI支付预处理
                try
                {
                    int orderID = RequestHelper.GetQueryString<int>("OrderID");
                    OrderInfo order = OrderBLL.ReadOrder(orderID, Cookies.User.GetUserID(true));
                    jsApiPay.out_trade_no = order.OrderNumber;
                    jsApiPay.body = "订单号：" + order.OrderNumber;
                    jsApiPay.total_fee = (int)(order.ProductMoney * 100); //(order.ProductMoney - order.FavorableMoney + order.ShippingMoney + order.OtherMoney - order.Balance - order.CouponMoney).ToString();

                    WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                    wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                    Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
                    //在页面上显示订单信息
                    Response.Write("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr><td colspan=\"2\" align=\"center\">订单详情</td></tr><tr><td width=\"40%\" align=\"right\">订单号：</td><td width=\"60%\" align=\"left\">" + order.OrderNumber + "</td></tr><tr><td align=\"right\">总金额：</td><td align=\"left\">" + order.ProductMoney + "元</td></tr><tr><td  colspan=\"2\" align=\"center\"><div onclick=\"callpay()\" style=\"width:210px; height:50px; text-align:center; line-height:50px; border-radius: 15px;background-color:#00CD00; border:0px #FE6714 solid; cursor: pointer;  color:white;  font-size:16px; cursor:pointer;\" >立即支付</div></td></tr></table>");
                }
                catch (Exception ex)
                {
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "下单失败，请返回重试" + "</span>");
                    //submit.Visible = false;
                }
            }
        }
    }
}
