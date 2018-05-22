using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IShippingRegion
    {        
        int AddShippingRegion(ShippingRegionInfo shippingRegion);
        void DeleteShippingRegion(string strID);
        void DeleteShippingRegionByShippingID(string strShippingID);
        ShippingRegionInfo ReadShippingRegion(int id);
        List<ShippingRegionInfo> ReadShippingRegionByShipping(string strShippingID);
        void UpdateShippingRegion(ShippingRegionInfo shippingRegion);
    }
}
