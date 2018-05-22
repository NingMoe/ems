using System;
using System.Collections.Generic;
using XueFuShop.Models;
using XueFuShop.IDAL;
using XueFuShop.Common;
using XueFu.EntLib;

namespace XueFuShop.BLL
{
    public sealed class ProductBrandBLL
    {
        private static readonly string cacheKey = CacheKey.ReadCacheKey("ProductBrand");
        private static readonly IProductBrand dal = FactoryHelper.Instance<IProductBrand>(Global.DataProvider, "ProductBrandDAL");
        public static readonly int TableID = UploadTable.ReadTableID("ProductBrand");

        public static int AddProductBrand(ProductBrandInfo productBrand)
        {
            productBrand.ID = dal.AddProductBrand(productBrand);
            UploadBLL.UpdateUpload(TableID, 0, productBrand.ID, Cookies.Admin.GetRandomNumber(false));
            CacheHelper.Remove(cacheKey);
            return productBrand.ID;
        }

        public static void ChangeProductBrandCount(int id, ChangeAction action)
        {
            dal.ChangeProductBrandCount(id, action);
            CacheHelper.Remove(cacheKey);
        }

        public static void ChangeProductBrandCountByGeneral(string strID, ChangeAction action)
        {
            dal.ChangeProductBrandCountByGeneral(strID, action);
            CacheHelper.Remove(cacheKey);
        }

        public static void ChangeProductBrandOrder(ChangeAction action, int id)
        {
            dal.ChangeProductBrandOrder(action, id);
            CacheHelper.Remove(cacheKey);
        }

        public static void DeleteProductBrand(string strID)
        {
            UploadBLL.DeleteUploadByRecordID(TableID, strID);
            dal.DeleteProductBrand(strID);
            CacheHelper.Remove(cacheKey);
        }

        public static ProductBrandInfo ReadProductBrandCache(int id)
        {
            ProductBrandInfo info = new ProductBrandInfo();
            List<ProductBrandInfo> list = ReadProductBrandCacheList();
            foreach (ProductBrandInfo info2 in list)
            {
                if (info2.ID == id)
                {
                    return info2;
                }
            }
            return info;
        }

        public static List<ProductBrandInfo> ReadProductBrandCacheList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadProductBrandAllList());
            }
            return (List<ProductBrandInfo>)CacheHelper.Read(cacheKey);
        }

        public static List<ProductBrandInfo> ReadProductBrandIsTopCacheList()
        {
            List<ProductBrandInfo> list = new List<ProductBrandInfo>();
            foreach (ProductBrandInfo info in ReadProductBrandCacheList())
            {
                if (info.IsTop == 1)
                {
                    list.Add(info);
                }
            }
            return list;
        }

        /// <summary>
        /// 读取品牌列表
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public static List<ProductBrandInfo> ReadProductBrandCacheList(string strID)
        {
            List<ProductBrandInfo> list = new List<ProductBrandInfo>();
            foreach (ProductBrandInfo info in ReadProductBrandCacheList())
            {
                if (StringHelper.CompareSingleString(strID, info.ID.ToString()))
                {
                    list.Add(info);
                }
            }
            return list;
        }

        public static void UpdateProductBrand(ProductBrandInfo productBrand)
        {
            dal.UpdateProductBrand(productBrand);
            UploadBLL.UpdateUpload(TableID, 0, productBrand.ID, Cookies.Admin.GetRandomNumber(false));
            CacheHelper.Remove(cacheKey);
        }
    }

}
