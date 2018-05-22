using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IFavorableActivity
    {
        int AddFavorableActivity(FavorableActivityInfo favorableActivity);
        void DeleteFavorableActivity(string strID);
        FavorableActivityInfo ReadFavorableActivity(int id);
        FavorableActivityInfo ReadFavorableActivity(DateTime startDate, DateTime endDate, int id);
        List<FavorableActivityInfo> ReadFavorableActivityList(int currentPage, int pageSize, ref int count);
        void UpdateFavorableActivity(FavorableActivityInfo favorableActivity);
    }
}
