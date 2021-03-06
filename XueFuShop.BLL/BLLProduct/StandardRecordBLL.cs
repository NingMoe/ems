using System;
using System.Collections.Generic;
using XueFuShop.Models;
using XueFuShop.IDAL;
using XueFuShop.Common;
using XueFu.EntLib;

namespace XueFuShop.BLL
{
    public sealed class StandardRecordBLL
    {
        private static readonly IStandardRecord dal = FactoryHelper.Instance<IStandardRecord>(Global.DataProvider, "StandardRecordDAL");

        public static void AddStandardRecord(StandardRecordInfo standardRecord)
        {
            dal.AddStandardRecord(standardRecord);
        }

        public static void DeleteStandardRecord(string strID)
        {
            dal.DeleteStandardRecord(strID);
        }

        public static void DeleteStandardRecordByProductID(string strProductID)
        {
            dal.DeleteStandardRecordByProductID(strProductID);
        }

        public static List<StandardRecordInfo> ReadStandardRecordByProduct(int productID, int standardType)
        {
            return dal.ReadStandardRecordByProduct(productID, standardType);
        }
    }
}
