using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class BookingProduct : UserBasePage
    {
        
        protected List<BookingProductInfo> bookingProductList = new List<BookingProductInfo>();
        protected CommonPagerClass commonPagerClass = new CommonPagerClass();

        
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
            BookingProductSearchInfo bookingProduct = new BookingProductSearchInfo();
            bookingProduct.UserID = base.UserID;
            this.bookingProductList = BookingProductBLL.SearchBookingProductList(queryString, pageSize, bookingProduct, ref count);
        }
    }

 

}
