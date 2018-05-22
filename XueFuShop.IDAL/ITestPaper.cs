using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface ITestPaper
    {
        TestPaperInfo ReadPaper(TestPaperInfo Model);
        TestPaperInfo ReadPaper(int UserId, int CateId);
        TestPaperInfo ReadTheOldTestPaperInfo(int UserId);
        int AddPaper(TestPaperInfo Model);
        void UpdatePaper(TestPaperInfo Model);
        void DeletePaper(int Id);
        void DeletePaperByUserID(string userID);
        void RecoveryPaperByUserID(string userID);
        Dictionary<int, float> ReadTheBestList(int userID, string courseID);
        List<TestPaperReportInfo> ReadThelatestList(int userID, string courseID);
        List<TestPaperReportInfo> ReadTheFirstRecordList(string companyID);
        List<TestPaperInfo> ReadList(TestPaperInfo Model);
        List<TestPaperInfo> NewReadList(TestPaperInfo Model);
        List<TestPaperInfo> ReadList(int currentPage, int pageSize, ref int count);
        List<TestPaperInfo> ReadList(TestPaperInfo Model, int currentPage, int pageSize, ref int count);
        void UpdatePaperCompanyId(int UserId, int CompanyId);
    }
}
