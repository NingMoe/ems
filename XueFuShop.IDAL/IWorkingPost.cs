using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IWorkingPost
    {
        int AddWorkingPost(WorkingPostInfo workingPost);
        void UpdateWorkingPost(WorkingPostInfo workingPost);
        void DeleteWorkingPost(string strID);
        void DeleteWorkingPostByCompanyID(string companyID);
        WorkingPostViewInfo ReadWorkingPostView(int id);
        List<WorkingPostViewInfo> SearchWorkingPostViewList(WorkingPostSearchInfo workingPostSearch);
        WorkingPostInfo ReadWorkingPost(int id);
        List<WorkingPostInfo> SearchWorkingPostList(WorkingPostSearchInfo workingPostSearch);
        List<WorkingPostInfo> SearchWorkingPostList(WorkingPostSearchInfo workingPostSearch, int currentPage, int pageSize, ref int count);
    }
}
