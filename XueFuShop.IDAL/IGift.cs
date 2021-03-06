using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IGift
    {
        int AddGift(GiftInfo gift);
        void DeleteGift(string strID);
        GiftInfo ReadGift(int id);
        List<GiftInfo> SearchGiftList(GiftSearchInfo gift);
        List<GiftInfo> SearchGiftList(int currentPage, int pageSize, GiftSearchInfo gift, ref int count);
        void UpdateGift(GiftInfo gift);
    }
}
