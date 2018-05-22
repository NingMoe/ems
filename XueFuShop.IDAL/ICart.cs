using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface ICart
    {        
        int AddCart(CartInfo cart);
        void ClearCart(int userID);
        void DeleteCart(string strID, int userID);
        bool IsProductInCart(int productID, string productName, int userID);
        string ReadCartIDList(string strID, int userID);
        List<CartInfo> ReadCartListByUser(int userID);
        void UpdateCart(string strID, int count);
    }
}
