using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IShipping
    {        
        int AddShipping(ShippingInfo shipping);
        void ChangeShippingOrder(ChangeAction action, int id);
        void DeleteShipping(string strID);
        List<ShippingInfo> ReadShippingAllList();
        void UpdateShipping(ShippingInfo shipping);
    }
}
