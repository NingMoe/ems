using System;
using XueFuShop.Models;
using System.Collections.Generic;

namespace XueFuShop.IDAL
{
    public interface IProductPhoto
    {
        int AddProductPhoto(ProductPhotoInfo productPhoto);
        void DeleteProductPhoto(string strID);
        void DeleteProductPhotoByProductID(string strProductID);
        ProductPhotoInfo ReadProductPhoto(int id);
        List<ProductPhotoInfo> ReadProductPhotoByProduct(int productID);
    }
}
