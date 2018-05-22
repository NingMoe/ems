using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class Order : UserBasePage
    {
        
        protected CommonPagerClass commonPagerClass = new CommonPagerClass();
        protected List<OrderInfo> orderList = new List<OrderInfo>();

        
        protected override void PageLoad()
        {
            base.PageLoad();
            int queryString = RequestHelper.GetQueryString<int>("Page");
            if (queryString < 1)
            {
                queryString = 1;
            }
            int pageSize = 20;
            int count = 0;
            OrderSearchInfo order = new OrderSearchInfo();
            order.UserID = base.UserID;
            this.orderList = OrderBLL.SearchOrderList(queryString, pageSize, order, ref count);
            this.commonPagerClass.CurrentPage = queryString;
            this.commonPagerClass.PageSize = pageSize;
            this.commonPagerClass.Count = count;
        }
    }

 

}
