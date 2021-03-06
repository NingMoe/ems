using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IAdRecord
    {
        
        int AddAdRecord(AdRecordInfo adRecord);
        void DeleteAdRecord(string strID, int userID);
        void DeleteAdRecordByAdID(string strAdID);
        AdRecordInfo ReadAdRecord(int id, int userID);
        List<AdRecordInfo> ReadAdRecordList(int currentPage, int pageSize, ref int count, int userID);
        List<AdRecordInfo> ReadAdRecordList(int adID, int currentPage, int pageSize, ref int count, int userID);
        void UpdateAdRecord(AdRecordInfo adRecord);
    }
}
