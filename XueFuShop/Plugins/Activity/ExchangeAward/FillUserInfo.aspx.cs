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
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Pages;

namespace XueFuShop.Web
{
    public partial class FillUserInfo : PluginsBasePage
    {
        protected ProductInfo product = new ProductInfo();
        protected int userID = 0;
        /// <summary>
        /// ҳ����ط���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = RequestHelper.GetQueryString<int>("ID");
                userID = Cookies.User.GetUserID(true);
                if (userID == 0)
                {
                    ScriptHelper.Alert("���ȵ�¼", "/User/Login.aspx?RedirectUrl=/Plugins/Activity/ExchangeAward/FillUserInfo.aspx?ID=" + id.ToString());
                }
                UserInfo user = UserBLL.ReadUser(userID);
                Consignee.Text = user.UserName;
                Tel.Text = user.Tel;
                Mobile.Text = user.Mobile;
                Address.Text = user.Address;
                product = ProductBLL.ReadProduct(id);
                RegionID.DataSource = RegionBLL.ReadRegionUnlimitClass();
                RegionID.ClassID = user.RegionID;
                Head.Title = product.Name + " - ��Ʒ�һ�";
            }
        }
        /// <summary>
        /// �ύ��ť�������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            //��ȡ�û���Ϣ
            userID = Cookies.User.GetUserID(true);
            int pointLeft = UserBLL.ReadUserMore(userID).PointLeft;
            string userName = Cookies.User.GetUserName(false);
            string userEmail = CookiesHelper.ReadCookieValue("UserEmail");
            //������
            int id = RequestHelper.GetQueryString<int>("ID");
            product = ProductBLL.ReadProduct(id);
            ExchangeAwardInfo exchangeAward = ExchangeAwardBLL.ReadConfigInfo();
            int productPoint = 0;
            if (exchangeAward.PorudctIDList != string.Empty)
            {
                string[] productArray = exchangeAward.PorudctIDList.Split(',');
                string[] pointArray = exchangeAward.PointList.Split(',');
                for (int i = 0; i < productArray.Length; i++)
                {
                    if (productArray[i] == id.ToString())
                    {
                        productPoint = Convert.ToInt32(pointArray[i]);
                    }
                }
            }
            if (productPoint > pointLeft)
            {
                ScriptHelper.Alert("����ǰ�Ļ��ֲ����Զ�ȡ�ý�Ʒ", RequestHelper.RawUrl);
            }
            //��Ӷ���
            OrderInfo order = new OrderInfo();
            order.OrderNumber = ShopCommon.CreateOrderNumber();
            order.IsActivity = (int)BoolType.True;
            order.OrderStatus = (int)OrderStatus.WaitCheck;
            order.OrderNote = "���ֶһ���Ʒ";
            order.ProductMoney = 0;
            order.Balance = 0;
            order.FavorableMoney = 0;
            order.OtherMoney = 0;
            order.CouponMoney = 0;
            order.Consignee = StringHelper.AddSafe(Consignee.Text);
            SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
            order.RegionID = singleUnlimitClass.ClassID;
            order.Address = StringHelper.AddSafe(Address.Text);
            order.ZipCode = StringHelper.AddSafe(ZipCode.Text);
            order.Tel = StringHelper.AddSafe(Tel.Text);
            order.Email = userEmail;
            order.Mobile = StringHelper.AddSafe(Mobile.Text);
            order.ShippingID = 0;
            order.ShippingDate = RequestHelper.DateNow;
            order.ShippingNumber = string.Empty;
            order.ShippingMoney = 0;
            order.PayKey = string.Empty;
            order.PayName = string.Empty;
            order.PayDate = RequestHelper.DateNow; ;
            order.IsRefund = (int)BoolType.False;
            order.FavorableActivityID = 0;
            order.GiftID = 0;
            order.InvoiceTitle = string.Empty;
            order.InvoiceContent = string.Empty;
            order.UserMessage = string.Empty;
            order.AddDate = RequestHelper.DateNow;
            order.IP = ClientHelper.IP;
            order.UserID = userID;
            order.UserName = userName;
            int orderID = OrderBLL.AddOrder(order);
            //��Ӷ�����ϸ
            OrderDetailInfo orderDetail = new OrderDetailInfo();
            orderDetail.OrderID = orderID;
            orderDetail.ProductID = product.ID;
            orderDetail.ProductName = product.Name;
            orderDetail.ProductWeight = product.Weight;
            orderDetail.SendPoint = 0;
            orderDetail.ProductPrice = 0;
            orderDetail.BuyCount = 1;
            orderDetail.FatherID = 0;
            orderDetail.RandNumber = string.Empty;
            orderDetail.GiftPackID = 0;
            OrderDetailBLL.AddOrderDetail(orderDetail);
            //���ֲ���
            UserAccountRecordBLL.AddUserAccountRecord(0, -productPoint, "�һ���Ʒ-" + product.Name, userID, userName);
            //���Ĳ�Ʒ��涩������
            ProductBLL.ChangeProductOrderCountByOrder(orderID, ChangeAction.Plus);
            //�����¼
            string fileName = StringHelper.Encode(ShopConfig.ReadConfigInfo().SecureKey, ShopConfig.ReadConfigInfo().SecureKey) + ".txt";
            fileName = Server.MapPath("Admin/" + fileName);
            File.AppendAllText(fileName, userName + "�������ţ�" + order.OrderNumber + "����Ʒ��" + product.Name + "\r\n", System.Text.Encoding.Default);
            ScriptHelper.Alert("�ɹ��һ�", "/User/OrderDetail.aspx?ID=" + orderID.ToString());
        }
    }
}
