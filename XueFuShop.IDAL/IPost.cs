using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IPost
    {
        PostInfo ReadPost(int Id);
        int AddPost(PostInfo Model);
        void UpdatePost(PostInfo Model);
        void DeletePost(int Id);
        void UpdatePostPlan(int PostId, string PostPlan);
        List<PostInfo> ReadPostCateAllList();
        List<PostInfo> ReadPostList(PostInfo Model);
        void MoveUp(int id);
        void MoveDown(int id);
    }
}
