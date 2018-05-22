using System;
using System.Collections.Generic;
using XueFuShop.Models;
using XueFuShop.IDAL;
using XueFuShop.Common;
using XueFu.EntLib;

namespace XueFuShop.BLL
{
    public sealed class OrderActionBLL
    {
        private static readonly IOrderAction dal = FactoryHelper.Instance<IOrderAction>(Global.DataProvider, "OrderActionDAL");

        public static int AddOrderAction(OrderActionInfo orderAction)
        {
            orderAction.ID = dal.AddOrderAction(orderAction);
            return orderAction.ID;
        }

        public static int AdminAddOrderAction(int orderID, int startOrderStatus, int endOrderStatus, string note, int orderOperate)
    {
        OrderActionInfo info;
        info = new OrderActionInfo();
        info.OrderID = orderID;
        info.OrderOperate = orderOperate;
        info.StartOrderStatus = startOrderStatus;
        info.EndOrderStatus = endOrderStatus;
        info.Note = note;
        info.IP = ClientHelper.IP;
        info.Date = RequestHelper.DateNow;
        info.AdminID = Cookies.Admin.GetAdminID(false);
        info.AdminName = Cookies.Admin.GetAdminName(false);
        info.ID = dal.AddOrderAction(info);
        return info.ID;
    }

        public static void DeleteOrderAction(string strID)
        {
            dal.DeleteOrderAction(strID);
        }

        public static void DeleteOrderActionByOrderID(string strOrderID)
        {
            dal.DeleteOrderActionByOrderID(strOrderID);
        }

        public static OrderActionInfo ReadLatestOrderAction(int orderID, int endOrderStatus)
        {
            return dal.ReadLatestOrderAction(orderID, endOrderStatus);
        }

        public static OrderActionInfo ReadOrderAction(int id)
        {
            return dal.ReadOrderAction(id);
        }

        public static List<OrderActionInfo> ReadOrderActionByOrder(int orderID)
        {
            return dal.ReadOrderActionByOrder(orderID);
        }

        public static string ReadOrderOperate(int orderOperate)
        {
            string str = string.Empty;
            switch (orderOperate)
            {
                case 1:
                    return "付款";

                case 2:
                    return "审核";

                case 3:
                    return "取消";

                case 4:
                    return "发货";

                case 5:
                    return "收货确认";

                case 6:
                    return "换货确认";

                case 7:
                    return "退货确认";

                case 8:
                    return "撤销";

                case 9:
                    return "退款";
            }
            return str;
        }

        public static int UserAddOrderAction(int orderID, int startOrderStatus, int endOrderStatus, string note, int orderOperate)
        {
            OrderActionInfo info;
            info = new OrderActionInfo();
            info.OrderID = orderID;
            info.OrderOperate = orderOperate;
            info.StartOrderStatus = startOrderStatus;
            info.EndOrderStatus = endOrderStatus;
            info.Note = note;
            info.IP = ClientHelper.IP;
            info.Date = RequestHelper.DateNow;
            info.AdminID = 0;
            info.AdminName = string.Empty;
            info.ID = dal.AddOrderAction(info);
            return info.ID;
        }
    }
}
