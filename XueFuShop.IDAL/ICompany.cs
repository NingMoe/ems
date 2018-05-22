using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface ICompany
    {
        int AddCompany(CompanyInfo Model);
        CompanyInfo ReadCompany(int id);
        void UpdateCompany(CompanyInfo Model);
        void UpdateCompany(string Field, string Value, string Id);
        void DeleteCompany(int Id);
        List<CompanyInfo> ReadCompanyListByCompanyID(string companyID);
        List<CompanyInfo> ReadCompanyListByParentID(string parentId);
        List<CompanyInfo> ReadCompanyList(CompanyInfo Model);
        List<CompanyInfo> ReadCompanyList(int currentPage, int pageSize, ref int count);
        List<CompanyInfo> ReadCompanyList(CompanyInfo Model, int currentPage, int pageSize, ref int count);
    }
}
