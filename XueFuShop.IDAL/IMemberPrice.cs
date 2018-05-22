using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IMemberPrice
    {
        
        void AddMemberPrice(MemberPriceInfo memberPrice);
        void DeleteMemberPriceByGradeID(string strGradeID);
        void DeleteMemberPriceByProductID(string strProductID);
        List<MemberPriceInfo> ReadMemberPriceByProduct(int productID);
        List<MemberPriceInfo> ReadMemberPriceByProduct(string strProductID);
        List<MemberPriceInfo> ReadMemberPriceByProductGrade(string strProductID, int gradeID);
    }
}
