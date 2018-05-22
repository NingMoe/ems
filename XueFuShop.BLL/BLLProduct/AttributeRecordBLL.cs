using System;
using System.Collections.Generic;
using XueFuShop.Models;
using XueFuShop.IDAL;
using XueFuShop.Common;
using XueFu.EntLib;

namespace XueFuShop.BLL
{
    public sealed class AttributeRecordBLL
    {
        //产品不多，所以开启缓存机制
        private static readonly string cacheKey = CacheKey.ReadCacheKey("AttributeRecord");
        private static readonly IAttributeRecord dal = FactoryHelper.Instance<IAttributeRecord>(Global.DataProvider, "AttributeRecordDAL");

        public static void AddAttributeRecord(AttributeRecordInfo attributeRecord)
        {
            dal.AddAttributeRecord(attributeRecord);
            CacheHelper.Remove(cacheKey);
        }

        public static void DeleteAttributeRecordByProductID(string strProductID)
        {
            dal.DeleteAttributeRecordByProductID(strProductID);
            CacheHelper.Remove(cacheKey);
        }

        public static AttributeRecordInfo ReadAttributeRecord(List<AttributeRecordInfo> attributeRecordList, int attributeID, int productID)
        {
            foreach (AttributeRecordInfo info in attributeRecordList)
            {
                if (attributeID == info.AttributeID && productID == info.ProductID)
                {
                    return info;
                }
            }
            return new AttributeRecordInfo();
        }

        public static List<AttributeRecordInfo> ReadAttributeRecordByProduct(int productID)
        {
            //return dal.ReadAttributeRecordByProduct(productID);
            List<AttributeRecordInfo> list = new List<AttributeRecordInfo>();
            foreach (AttributeRecordInfo info in ReadAttributeRecordCacheList())
            {
                if (info.ProductID == productID)
                {
                    list.Add(info);
                }
            }
            return list;
        }

        public static List<AttributeRecordInfo> ReadList(string productID)
        {
            //return ReadList(string.Empty, productID);
            List<AttributeRecordInfo> list = new List<AttributeRecordInfo>();
            foreach (AttributeRecordInfo info in ReadAttributeRecordCacheList())
            {
                if (StringHelper.CompareSingleString(productID, info.ProductID.ToString()))
                {
                    list.Add(info);
                }
            }
            return list;
        }

        public static List<AttributeRecordInfo> ReadList(string attributeID, string productID)
        {
            //return dal.ReadList(attributeID, productID);
            List<AttributeRecordInfo> list = new List<AttributeRecordInfo>();
            foreach (AttributeRecordInfo info in ReadAttributeRecordCacheList())
            {
                if (StringHelper.CompareSingleString(attributeID, info.AttributeID.ToString()) && StringHelper.CompareSingleString(productID, info.ProductID.ToString()))
                {
                    list.Add(info);
                }
            }
            return list;
        }

        public static List<AttributeRecordInfo> ReadAttributeRecordCacheList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadAttributeRecordAllList());
            }
            return (List<AttributeRecordInfo>)CacheHelper.Read(cacheKey);
        }
    }
}
