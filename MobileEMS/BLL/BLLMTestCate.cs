using System;
using System.Data;
using System.Collections.Generic;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace MobileEMS.BLL
{
    public sealed class BLLMTestCate
    {
        
        public static Dictionary<int, DateTime> ReadPrepaidTestCateList(int UserId)
        {
            Dictionary<int, DateTime> prepaidTestCateList = new Dictionary<int, DateTime>();

            OrderSearchInfo orderSearch = new OrderSearchInfo();
            orderSearch.StartPayDate = DateTime.Now.AddDays(-30);
            orderSearch.UserID = UserId;
            orderSearch.OrderStatus = 2;
            List<OrderInfo> orderList = OrderBLL.SearchOrderList(orderSearch);
            foreach (OrderInfo info in orderList)
            {
               List<OrderDetailInfo> orderDetailList = OrderDetailBLL.ReadOrderDetailByOrder(info.ID);
               foreach (OrderDetailInfo item in orderDetailList)
               {
                   if (!prepaidTestCateList.ContainsKey(item.ProductID))
                   {
                       prepaidTestCateList.Add(item.ProductID, info.PayDate);
                   }
               }
            }

            return prepaidTestCateList;
        }



        /// <summary>
        /// ¶ÁÈ¡ÒÑ¹ºÂòµÄ¿Î³ÌID×Ö·û´®
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static string ReadPrepaidTestCate(int UserId)
        {
            string prepaidTestCate = string.Empty;

            Dictionary<int, DateTime> prepaidTestCateList = ReadPrepaidTestCateList(UserId);

            foreach (KeyValuePair<int, DateTime> pair in prepaidTestCateList)
            {
                if (!string.IsNullOrEmpty(prepaidTestCate))
                {
                    prepaidTestCate += prepaidTestCate + "," + pair.Key.ToString();
                }
                else
                {
                    prepaidTestCate = pair.Key.ToString();
                }
            }

            return prepaidTestCate;
        }
    }
}
