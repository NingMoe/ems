using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFu.EntLib;

namespace XueFuShop.Pages
{
   public class Training : UserManageBasePage
    {
       protected List<PostInfo> postList = new List<PostInfo>();

       protected override void PageLoad()
       {
           base.PageLoad();
           string action = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("Action"));
           if (action == "Delete")
               Delete(RequestHelper.GetQueryString<int>("SelectID"));

           string parentCompanyID = base.ParentCompanyID;
           //去除系统ID
           parentCompanyID = StringHelper.SubString(parentCompanyID, CompanyBLL.SystemCompanyId.ToString());
           postList = PostBLL.FilterPostListByCompanyID(PostBLL.ReadPostCateNamedList(parentCompanyID), parentCompanyID);
       }

       protected void Delete(int id)
       {
           PostInfo post = PostBLL.ReadPost(id);
           //只能删除自己公司的并且旗下没有分类的信息
           if (post.CompanyID == base.UserCompanyID && PostBLL.ReadPostListByParentID(id).Count == 0)
               PostBLL.DeletePost(id);
           else
               ScriptHelper.Alert("请先删除子分类");
       }
    }
}
