using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface ICompanyRule
    {
        int AddCompanyRule(CompanyRuleInfo Model);
        int UpdateCompanyRule(CompanyRuleInfo Model);
        int DeleteCompanyRule(string Id);
        List<CompanyRuleInfo> CompanyRuleList(CompanyRuleInfo Model);
        List<CompanyRuleInfo> CompanyRuleList(int currentPage, int pageSize, CompanyRuleInfo RuleSearch, ref int count);
    }
}
