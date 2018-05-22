using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFu.EntLib;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class OrderAjax : UserAjaxBasePage
    {
        
        private void OrderOperate()
        {
            string content = string.Empty;
            int queryString = RequestHelper.GetQueryString<int>("OrderID");
            int num2 = RequestHelper.GetQueryString<int>("OrderStatus");
            switch (num2)
            {
                case 1:
                case 2:
                case 5:
                    {
                        OrderInfo order = OrderBLL.ReadOrder(queryString, base.UserID);
                        if (order.ID == 0)
                        {
                            content = "�������ڵ�ǰ�û��Ķ���";
                        }
                        else
                        {
                            int orderStatus;
                            if ((num2 == 2) || (num2 == 1))
                            {
                                orderStatus = order.OrderStatus;
                                order.OrderStatus = 3;
                                ProductBLL.ChangeProductOrderCountByOrder(queryString, ChangeAction.Minus);
                                OrderBLL.UserUpdateOrderAddAction(order, "�û�ȡ������", 3, orderStatus);
                            }
                            else
                            {
                                int point = OrderBLL.ReadOrderSendPoint(order.ID);
                                if (point > 0)
                                {
                                    UserAccountRecordBLL.AddUserAccountRecord(0M, point, ShopLanguage.ReadLanguage("OrderReceived").Replace("$OrderNumber", order.OrderNumber), order.UserID, order.UserName);
                                }
                                orderStatus = order.OrderStatus;
                                order.OrderStatus = 6;
                                OrderBLL.UserUpdateOrderAddAction(order, "�û�ȷ���ջ�", 5, orderStatus);
                            }
                        }
                        break;
                    }
                default:
                    content = "����״̬����";
                    break;
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        protected override void PageLoad()
        {
            base.PageLoad();
            string queryString = RequestHelper.GetQueryString<string>("Action");
            if ((queryString != null) && (queryString == "OrderOperate"))
            {
                this.OrderOperate();
            }
        }
    }

 

}
