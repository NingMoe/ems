using System;
using XueFuShop.Models;
using System.Collections.Generic;

namespace XueFuShop.IDAL
{
    public interface IOrderAction
    {
        int AddOrderAction(OrderActionInfo orderDetail);
        void DeleteOrderAction(string strID);
        void DeleteOrderActionByOrderID(string strOrderID);
        OrderActionInfo ReadLatestOrderAction(int orderID, int endOrderStatus);
        OrderActionInfo ReadOrderAction(int id);
        List<OrderActionInfo> ReadOrderActionByOrder(int orderID);
    }
}
