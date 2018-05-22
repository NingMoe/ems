using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XueFuShop.Common;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFu.EntLib;

namespace XueFuShop.Pay.AliPay
{
    public partial class Pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayConfig payConfig = new PayConfig();
            string out_trade_no = string.Empty; //������
            string subject = "";	//��Ʒ����
            string body = "";		//��Ʒ����  
            string price = "";  //����
            string quantity = ""; //����
            string show_url = "";//չʾ��ַ
            string logistics_fee = ""; //�ʷ�
            string action = RequestHelper.GetQueryString<string>("Action");
            int userID = Cookies.User.GetUserID(true);
            string service = payConfig.Service;  //������� trade_create_by_buyer ��׼˫�ӿڽ��� create_direct_pay_by_user ֱ�Ӹ���,create_partner_trade_by_buyer �������� 
            switch (action)
            {
                case "Apply":
                    int applyID = RequestHelper.GetQueryString<int>("ApplyID");
                    UserRechargeInfo userRecharge = UserRechargeBLL.ReadUserRecharge(applyID, userID);
                    out_trade_no = userRecharge.Number;
                    subject = "���߳�ֵ��" + userRecharge.Number;
                    body = "���߳�ֵ";
                    price = userRecharge.Money.ToString();
                    quantity = "1";
                    show_url = "http://" + Request.ServerVariables["Http_Host"];
                    logistics_fee = "0";
                    break;
                case "PayOrder":
                    int orderID = RequestHelper.GetQueryString<int>("OrderID");
                    OrderInfo order = OrderBLL.ReadOrder(orderID, userID);
                    out_trade_no = order.OrderNumber;
                    subject = "����֧����" + order.OrderNumber;
                    body = "����֧��";
                    price = (order.ProductMoney - order.FavorableMoney + order.ShippingMoney + order.OtherMoney - order.Balance - order.CouponMoney).ToString();
                    quantity = "1";
                    show_url = "http://" + Request.ServerVariables["Http_Host"];
                    logistics_fee = "0";
                    break;
                default:
                    break;
            }
            string gateway = "https://www.alipay.com/cooperate/gateway.do?";	//'֧���ӿ�
            string partner = payConfig.Partner;		//	�������ID		
            string sign_type = "MD5"; //����Э��              
            string payment_type = "1";                  //֧������	            
            string seller_email = payConfig.SellerEmail;             //�����˺�
            string key = payConfig.SecurityKey;              //partner�˻���֧������ȫУ����
            string return_url = "http://" + Request.ServerVariables["Http_Host"] + "/Plugins/Pay/AliPay/Return.aspx"; //���������ؽӿ�
            string notify_url = "http://" + Request.ServerVariables["Http_Host"] + "/Plugins/Pay/AliPay/Notify.aspx"; //������֪ͨ�ӿ�
            string _input_charset = "utf-8";
            string logistics_type = "POST";
            string logistics_payment = "BUYER_PAY";
            AliPay ap = new AliPay();
            string aliay_url = ap.CreatUrl(
                gateway,
                service,
                partner,
                sign_type,
                out_trade_no,
                subject,
                body,
                payment_type,
                price,
                show_url,
                seller_email,
                key,
                return_url,
                _input_charset,
                notify_url,
                logistics_type,
                logistics_fee,
                logistics_payment,
                quantity
                );
            Response.Redirect(aliay_url);
        }

    }
}