using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XueFuShop.Common;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFu.EntLib;

namespace XueFuShop.Pay.TenPay
{
    public partial class Pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PayConfig payConfig = new PayConfig();
            int userID = Cookies.User.GetUserID(true);
            Md5Pay md5pay = new Md5Pay();
            string action = RequestHelper.GetQueryString<string>("Action");
            switch (action)
            {
                case "Apply":
                    int applyID = RequestHelper.GetQueryString<int>("ApplyID");
                    UserRechargeInfo userRecharge = UserRechargeBLL.ReadUserRecharge(applyID, userID);
                    md5pay.Sp_billno = userRecharge.Number;
                    md5pay.Desc = "���ϳ�ֵ��" + userRecharge.Number;
                    md5pay.Total_fee = Convert.ToInt64(userRecharge.Money * 100);
                    break;
                case "PayOrder":
                    int orderID = RequestHelper.GetQueryString<int>("OrderID");
                    OrderInfo order = OrderBLL.ReadOrder(orderID, userID);
                    md5pay.Sp_billno = order.OrderNumber;
                    md5pay.Desc = "����֧����" + order.OrderNumber;
                    md5pay.Total_fee = Convert.ToInt64((order.ProductMoney - order.FavorableMoney + order.ShippingMoney + order.OtherMoney - order.Balance - order.CouponMoney) * 100);
                    break;
                default:
                    break;
            }
            md5pay.Bargainor_id = payConfig.BargainorID;  //�����̻���           
            md5pay.Key = payConfig.BusinessKey; //�����̻�key   
            md5pay.Date = DateTime.Now.ToString("yyyyMMdd");//��������            
            md5pay.Attach = "tenpay"; //���ױ�ʶ           
            md5pay.Purchaser_id = "";  //����ʺ�            
            md5pay.Return_url = "http://" + Request.ServerVariables["Http_Host"] + "/Plugins/Pay/TenPay/Return.aspx"; //�̻��ص�url
            md5pay.Transaction_id = md5pay.Bargainor_id + md5pay.Date + md5pay.UnixStamp();  //ؔ��ͨ���׺�,�豣֤�˶�����ÿ��Ψһ,�в����ظ���
            //  md5pay.Spbill_create_ip = Page.Request.UserHostAddress;
            string url = "";
            if (!md5pay.GetPayUrl(out url))
            {
                ResponseHelper.Write("������ַʧ��");
            }
            else
            {
                Response.Redirect(url);
            }
        }
    }
}
