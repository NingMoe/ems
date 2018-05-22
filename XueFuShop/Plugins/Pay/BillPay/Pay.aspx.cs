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
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace XueFuShop.Pay.BillPay
{
    public partial class Pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayConfig payConfig = new PayConfig();
            merchant_id.Value = payConfig.MerchantID;		///�̻����
            string merchant_key = payConfig.MerchantKey;		///�̻���Կ
            string action = RequestHelper.GetQueryString<string>("Action");
            int userID = Cookies.User.GetUserID(true);
            switch (action)
            {
                case "Apply":
                    int applyID = RequestHelper.GetQueryString<int>("ApplyID");
                    UserRechargeInfo userRecharge = UserRechargeBLL.ReadUserRecharge(applyID, userID);
                    orderid.Value = userRecharge.Number;	///�������
                    amount.Value = userRecharge.Money.ToString();		///�������
                    commodity_info.Value = "���ϳ�ֵ��" + userRecharge.Number;		///��Ʒ��Ϣ,�����������ͨ��System.Web.HttpUtility.UrlEncode()����
                    break;
                case "PayOrder":
                    int orderID = RequestHelper.GetQueryString<int>("OrderID");
                    OrderInfo order = OrderBLL.ReadOrder(orderID, userID);
                    orderid.Value = order.OrderNumber; 	///�������
                    amount.Value = (order.ProductMoney - order.FavorableMoney + order.ShippingMoney + order.OtherMoney - order.Balance - order.CouponMoney).ToString(); ///�������
                    commodity_info.Value = "����֧����" + order.OrderNumber;		///��Ʒ��Ϣ,�����������ͨ��System.Web.HttpUtility.UrlEncode()����
                    break;
                default:
                    break;
            }
            currency.Value = "1";		///��������,1Ϊ�����
            isSupportDES.Value = "2";		///�Ƿ�ȫУ��,2Ϊ��У��,�Ƽ�
            merchant_url.Value = "http://" + Request.ServerVariables["Http_Host"] + "/Plugins/Pay/BillPay/Return.aspx";		///֧��������ص�ַ
            pname.Value = "";		///֧��������,�����������ͨ��System.Web.HttpUtility.UrlEncode()����            
            merchant_param.Value = "";		///�̻�˽�в���
            pemail.Value = "";		///����email����Ǯ����ҳ��
            pid.Value = "";		///����/��������̻����

            ///���ɼ��ܴ�,ע��˳��
            String ScrtStr = "merchant_id=" + merchant_id.Value + "&orderid=" + orderid.Value + "&amount=" + amount.Value + "&merchant_url=" + merchant_url.Value + "&merchant_key=" + merchant_key;
            mac.Value = FormsAuthentication.HashPasswordForStoringInConfigFile(ScrtStr, "MD5");
        }
    }
}
