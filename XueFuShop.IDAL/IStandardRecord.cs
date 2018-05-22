using System;
using XueFuShop.Models;
using System.Collections.Generic;

namespace XueFuShop.IDAL
{
    public interface IStandardRecord
    {
        void AddStandardRecord(StandardRecordInfo standardRecord);
        void DeleteStandardRecord(string strID);
        void DeleteStandardRecordByProductID(string strProductID);
        List<StandardRecordInfo> ReadStandardRecordByProduct(int productID, int standardType);
    }
}
