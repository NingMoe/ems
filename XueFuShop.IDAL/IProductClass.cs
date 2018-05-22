using System;
using XueFuShop.Models;
using System.Collections.Generic;

namespace XueFuShop.IDAL
{
    public interface IProductClass
    {
        int AddProductClass(ProductClassInfo productClass);
        void DeleteProductClass(int id);
        void DeleteTaobaoProductClass();
        void MoveDownProductClass(int id);
        void MoveUpProductClass(int id);
        List<ProductClassInfo> ReadProductClassAllList();
        List<string> ReadProductClassListByProductID(string productID, string inBrandID);
        void UpdateProductClass(ProductClassInfo productClass);
        void UpdateProductFatherID(Dictionary<long, int> fatherIDDic);
    }
}
