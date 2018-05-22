using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IAd
    {
        int AddAd(AdInfo ad);
        void ChangeAdCount(int id, ChangeAction action);
        void ChangeAdCountByGeneral(string strID, ChangeAction action);
        void DeleteAd(string strID);
        AdInfo ReadAd(int id);
        List<AdInfo> ReadAdList(int currentPage, int pageSize, ref int count);
        List<AdInfo> ReadAdList(int classID, int currentPage, int pageSize, ref int count);
        void UpdateAd(AdInfo ad);
    }
}
