using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IPostPass
    {
        PostPassInfo ReadPostPassInfo();
        PostPassInfo ReadPostPassInfo(int Id);
        PostPassInfo ReadPostPassInfo(int userID, int postID);
        int AddPostPassInfo(PostPassInfo Model);
        void UpdatePostPassInfo(PostPassInfo Model);
        void UpdateCreateDate(PostPassInfo Model);
        void UpdateIsRZ(PostPassInfo Model);
        int ReadPostPassNum(int userID);
        List<ReportPostPassInfo> PostPassRepostList(PostPassInfo Model, string CompanyId);
        List<PostPassInfo> ReadPostPassList(PostPassInfo Model);
        List<PostPassInfo> ReadPostPassList(PostPassInfo postpassSearch, int currentPage, int pageSize, ref int count);
        List<PostPassInfo> ReadPostPassList(PostPassInfo Model, int num);
    }
}
