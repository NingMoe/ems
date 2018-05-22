using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IAttributeRecord
    {
        void AddAttributeRecord(AttributeRecordInfo attributeRecord);
        void DeleteAttributeRecordByProductID(string strProductID);
        List<AttributeRecordInfo> ReadAttributeRecordAllList();
        List<AttributeRecordInfo> ReadAttributeRecordByProduct(int productID);
        List<AttributeRecordInfo> ReadList(string attributeID, string productID);
    }
}
