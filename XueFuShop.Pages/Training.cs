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
           //ȥ��ϵͳID
           parentCompanyID = StringHelper.SubString(parentCompanyID, CompanyBLL.SystemCompanyId.ToString());
           postList = PostBLL.FilterPostListByCompanyID(PostBLL.ReadPostCateNamedList(parentCompanyID), parentCompanyID);
       }

       protected void Delete(int id)
       {
           PostInfo post = PostBLL.ReadPost(id);
           //ֻ��ɾ���Լ���˾�Ĳ�������û�з������Ϣ
           if (post.CompanyID == base.UserCompanyID && PostBLL.ReadPostListByParentID(id).Count == 0)
               PostBLL.DeletePost(id);
           else
               ScriptHelper.Alert("����ɾ���ӷ���");
       }
    }
}
