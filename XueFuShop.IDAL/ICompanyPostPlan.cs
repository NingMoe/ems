using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface ICompanyPostPlan
    {
        int AddCompanyPostPlan(CompanyPostPlanInfo Model);
        int UpdateCompanyPostPlan(CompanyPostPlanInfo Model);
        int DeleteCompanyPostPlan(string Id);
        DateTime ReadCompanyPostPlan(int CompanyId, int PostId);
        List<CompanyPostPlanInfo> CompanyPostPlanList(CompanyPostPlanInfo Model);
        List<CompanyPostPlanInfo> CompanyPostPlanList(int currentPage, int pageSize, CompanyPostPlanInfo RuleSearch, ref int count);
    }
}
