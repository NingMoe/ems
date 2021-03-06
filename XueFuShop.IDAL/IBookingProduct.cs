using System;
using XueFuShop.Models;
using System.Collections.Generic;

namespace XueFuShop.IDAL
{
    public interface IBookingProduct
    {
        int AddBookingProduct(BookingProductInfo bookingProduct);
        void DeleteBookingProduct(string strID, int userID);
        BookingProductInfo ReadBookingProduct(int id, int userID);
        string ReadBookingProductIDList(string strID, int userID);
        List<BookingProductInfo> SearchBookingProductList(BookingProductSearchInfo bookingProduct);
        List<BookingProductInfo> SearchBookingProductList(int currentPage, int pageSize, BookingProductSearchInfo bookingProduct, ref int count);
        void UpdateBookingProduct(BookingProductInfo bookingProduct);
    }
}
