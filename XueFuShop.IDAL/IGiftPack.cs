using System;
using XueFuShop.Models;
using System.Collections.Generic;

namespace XueFuShop.IDAL
{
    public interface IGiftPack
    {
        
        int AddGiftPack(GiftPackInfo giftPack);
        void DeleteGiftPack(string strID);
        GiftPackInfo ReadGiftPack(int id);
        List<GiftPackInfo> ReadGiftPackList(int currentPage, int pageSize, ref int count);
        void UpdateGiftPack(GiftPackInfo giftPack);
    }
}
