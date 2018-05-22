using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Pages
{
    public class CompanyList : UserManageBasePage
    {
        protected string companyName = RequestHelper.GetQueryString<string>("CompanyName");
        protected string parentCompanyName = RequestHelper.GetQueryString<string>("ParentCompanyName");
        protected List<CompanyInfo> searchCompanyList = new List<CompanyInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "公司列表";

            base.CheckUserPower("ReadCompanyList", PowerCheckType.Single);

            CompanyInfo companySearch = new CompanyInfo();
            companySearch.Condition = base.SonCompanyID;
            companySearch.Field = "CompanyID";
            companySearch.State = 0;
            companySearch.CompanyName = companyName;
            if (!string.IsNullOrEmpty(parentCompanyName))
            {
                companySearch.ParentIdCondition = CompanyBLL.ReadCompanyIdStr(parentCompanyName);
            }
            searchCompanyList = CompanyBLL.ReadCompanyList(companySearch, base.CurrentPage, base.PageSize, ref base.Count);
            base.BindPageControl(ref base.CommonPager);
        }
    }
}
