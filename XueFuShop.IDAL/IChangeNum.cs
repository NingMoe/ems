using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface ICompanyChangeNum
    {
        int AddCompanyRecord(CompanyRecordInfo Model);
        int UpdateCompanyRecord(CompanyRecordInfo Model);
        int DeleteCompanyRecord(string Id);
        int CompanyChangeNum(int CompanyId);
        int CompanyChangeNum(int CompanyId, DateTime StartDate, DateTime EndDate);
        int CompanyChangeNum(int CompanyId, int PostId, DateTime StartDate, DateTime EndDate);
        List<CompanyRecordInfo> CompanyRecordList(int currentPage, int pageSize, CompanyRecordInfo RecordSearch, ref int count);
    }

    public interface IUserChangeNum
    {
        int AddUserRecord(ProRecordInfo Model);
        int UpdateUserRecord(ProRecordInfo Model);
        int DeleteUserRecord(string Id);
        int UserChangeNum(int UserId);
        int UserChangeNum(int UserId, DateTime StartDate, DateTime EndDate);
        List<ProRecordInfo> UserRecordList(int currentPage, int pageSize, ProRecordInfo RecordSearch, ref int count);
    }
}
