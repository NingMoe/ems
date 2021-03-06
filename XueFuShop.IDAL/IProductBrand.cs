using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IProductBrand
    {
        int AddProductBrand(ProductBrandInfo productBrand);
        void ChangeProductBrandCount(int id, ChangeAction action);
        void ChangeProductBrandCountByGeneral(string strID, ChangeAction action);
        void ChangeProductBrandOrder(ChangeAction action, int id);
        void DeleteProductBrand(string strID);
        List<ProductBrandInfo> ReadProductBrandAllList();
        void UpdateProductBrand(ProductBrandInfo productBrand);
    }
}
