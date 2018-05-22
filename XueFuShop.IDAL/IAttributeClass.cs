using System;
using XueFuShop.Models;
using System.Collections.Generic;

namespace XueFuShop.IDAL
{
    public interface IAttributeClass
    {
        int AddAttributeClass(AttributeClassInfo attributeClass);
        void ChangeAttributeClassCount(int id, ChangeAction action);
        void ChangeAttributeClassCountByGeneral(string strID, ChangeAction action);
        void DeleteAttributeClass(string strID);
        List<AttributeClassInfo> ReadAttributeClassAllList();
        void UpdateAttributeClass(AttributeClassInfo attributeClass);
    }
}
