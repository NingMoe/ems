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
                //����������ز��������ͳһ�µ��ӿڣ���ú�����ؽӿڵ���ڲ���
                JsApiPay jsApiPay = new JsApiPay(this);
                //���á���ҳ��Ȩ��ȡ�û���Ϣ���ӿڻ�ȡ�û���openid��access_token
                jsApiPay.GetOpenidAndAccessToken();
                //jsApiPay.openid = openid;

                //JSAPI֧��Ԥ����
                try
                {
                    int orderID = RequestHelper.GetQueryString<int>("OrderID");
                    OrderInfo order = OrderBLL.ReadOrder(orderID, Cookies.User.GetUserID(true));
                    jsApiPay.out_trade_no = order.OrderNumber;
                    jsApiPay.body = "�����ţ�" + order.OrderNumber;
                    jsApiPay.total_fee = (int)(order.ProductMoney * 100); //(order.ProductMoney - order.FavorableMoney + order.ShippingMoney + order.OtherMoney - order.Balance - order.CouponMoney).ToString();

                    WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                    wxJsApiParam = jsApiPay.GetJsApiParameters();//��ȡH5����JS API����                    
                    Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
                    //��ҳ������ʾ������Ϣ
                    Response.Write("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr><td colspan=\"2\" align=\"center\">��������</td></tr><tr><td width=\"40%\" align=\"right\">�����ţ�</td><td width=\"60%\" align=\"left\">" + order.OrderNumber + "</td></tr><tr><td align=\"right\">�ܽ�</td><td align=\"left\">" + order.ProductMoney + "Ԫ</td></tr><tr><td  colspan=\"2\" align=\"center\"><div onclick=\"callpay()\" style=\"width:210px; height:50px; text-align:center; line-height:50px; border-radius: 15px;background-color:#00CD00; border:0px #FE6714 solid; cursor: pointer;  color:white;  font-size:16px; cursor:pointer;\" >����֧��</div></td></tr></table>");
                }
                catch (Exception ex)
                {
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "�µ�ʧ�ܣ��뷵������" + "</span>");
                    //submit.Visible = false;
                }
            }
        }
    }
}
