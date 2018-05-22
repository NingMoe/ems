using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Pages
{
    public class StaffEvaluateAdd : UserManageBasePage
    {
        protected int evaluateNameId = RequestHelper.GetQueryString<int>("EvaluateNameId");
        protected int evaluateUserId = RequestHelper.GetQueryString<int>("EvaluateUserId");
        protected int userId = RequestHelper.GetQueryString<int>("UserId");
        protected int evaluateType = RequestHelper.GetQueryString<int>("EvaluateType");
        protected string scoreStr = string.Empty;
        protected string userName = string.Empty;
        protected int companyID = RequestHelper.GetQueryString<int>("CompanyID");

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "×ÛºÏÄÜÁ¦ÆÀ¹À";

            base.CheckUserPower("AddEvaluate", PowerCheckType.Single);

            if (companyID <= 0)
                companyID = base.UserCompanyID;

            if (evaluateNameId > 0 && userId > 0)
            {
                UserInfo user = UserBLL.ReadUser(userId);
                companyID = user.CompanyID;
                userName = user.RealName;
                KPIEvaluateSearchInfo kpievaluateSearch = new KPIEvaluateSearchInfo();
                kpievaluateSearch.EvaluateNameId = evaluateNameId;
                kpievaluateSearch.UserId = userId.ToString();
                kpievaluateSearch.PostId = "0";
                kpievaluateSearch.EvaluateUserId = evaluateUserId;
                scoreStr = KPIEvaluateBLL.ReadStaffEvaluateData(kpievaluateSearch);
            }
        }
    }
}
