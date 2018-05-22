using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;

namespace XueFuShop.Pages
{
    public class WorkingPost : UserManageBasePage
    {
        protected int CompanyID = RequestHelper.GetQueryString<int>("CompanyId");
        protected string Action = RequestHelper.GetQueryString<string>("Action");
        protected List<WorkingPostInfo> WorkingPostList = new List<WorkingPostInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "¸ÚÎ»ÁÐ±í";

            if (Action == "Delete")
            {
                string selectID = RequestHelper.GetQueryString<string>("SelectID");
                if (!string.IsNullOrEmpty(selectID))
                {
                    base.CheckUserPower("DeleteWorkingPost", PowerCheckType.Single);
                    WorkingPostBLL.DeleteWorkingPost(selectID);
                    AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("WorkingPost"), selectID);
                    ResponseHelper.Redirect(Request.UrlReferrer.ToString());
                }
            }

            if (!base.ExistsSonCompany || Action == "Search")
            {
                base.CheckUserPower("ReadWorkingPost", PowerCheckType.Single);
                if (CompanyID < 0) CompanyID = base.UserCompanyID;

                WorkingPostSearchInfo workingPostSearch = new WorkingPostSearchInfo();
                workingPostSearch.CompanyId = CompanyID.ToString();
                //workingPostSearch.IsPost = 1;
                WorkingPostList = WorkingPostBLL.SearchWorkingPostList(workingPostSearch);
            }
        }
    }
}
