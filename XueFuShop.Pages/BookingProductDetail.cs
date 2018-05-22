using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class BookingProductDetail : UserBasePage
    {
        
        protected BookingProductInfo bookingProduct = new BookingProductInfo();

        
        protected override void PageLoad()
        {
            base.PageLoad();
            int queryString = RequestHelper.GetQueryString<int>("ID");
            this.bookingProduct = BookingProductBLL.ReadBookingProduct(queryString, base.UserID);
        }
    }

 

}
