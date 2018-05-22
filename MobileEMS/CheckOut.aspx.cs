using System;
using System.Data;
using System.Collections.Generic;
using XueFuShop.Common;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace MobileEMS
{
    public partial class CheckOut : System.Web.UI.Page
    {
        protected int UserID = Cookies.User.GetUserID(true);

        protected void Page_Load(object sender, EventArgs e)
        {
            decimal productTotalPrice = Sessions.ProductTotalPrice;
            string key = RequestHelper.GetForm<string>("Pay");
            //默认选择支付宝AliPay
            if (UserAgentHelper.IsWx(Request.UserAgent)) key = "WxPay";
            else key = "AliPay";
            PayPluginsInfo info = PayPlugins.ReadPayPlugins(key);
            OrderInfo order = new OrderInfo();
            order.PayKey = key;
            order.PayName = info.Name;
            order.OrderNumber = ShopCommon.CreateOrderNumber();
            if (info.IsCod == 1)
            {
                order.OrderStatus = 2;
            }
            else
            {
                order.OrderStatus = 1;
            }
            order.OrderNote = string.Empty;
            order.ProductMoney = productTotalPrice;
            //order.PayDate = RequestHelper.DateNow;
            order.UserID = UserID;
            //order.UserName = base.UserName;
            int orderID = OrderBLL.AddOrder(order);
            this.AddOrderProduct(orderID);
            if (order.OrderStatus == (int)OrderStatus.WaitPay && info.IsOnline == (int)BoolType.True)
                ResponseHelper.Redirect("/Plugins/Pay/" + key + "/Pay.aspx?OrderID=" + orderID + "&Action=PayOrder");
        }

        protected void AddOrderProduct(int orderID)
        {
            foreach (CartInfo info in CartBLL.ReadCartList(UserID))
            {
                OrderDetailInfo orderDetail = new OrderDetailInfo();
                orderDetail.OrderID = orderID;
                orderDetail.ProductID = info.ProductID;
                orderDetail.ProductName = info.ProductName;
                //orderDetail.ProductWeight = product.Weight;
                //orderDetail.SendPoint = product.SendPoint;
                orderDetail.ProductPrice = info.ProductPrice;
                orderDetail.BuyCount = info.BuyCount;
                orderDetail.RandNumber = info.RandNumber;
                OrderDetailBLL.AddOrderDetail(orderDetail);
            }
            CartBLL.ClearCart(UserID);
            Sessions.ProductTotalPrice = 0M;
            Sessions.ProductBuyCount = 0;
            Sessions.ProductTotalWeight = 0M;
        }
    }
}
