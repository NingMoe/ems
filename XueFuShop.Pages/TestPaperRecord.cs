using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFu.EntLib;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class TestPaperRecord : UserManageBasePage
    {
        protected int companyID = RequestHelper.GetQueryString<int>("CompanyID");
        protected string userName = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("UserName").Trim());
        protected string realName = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("RealName").Trim());
        protected string courseName = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("CourseName").Trim());
        protected DateTime startDate = RequestHelper.GetQueryString<DateTime>("SearchStartDate");
        protected DateTime endDate = RequestHelper.GetQueryString<DateTime>("SearchEndDate");
        protected int score = RequestHelper.GetQueryString<int>("Score");
        protected int isPass = RequestHelper.GetQueryString<int>("IsPass");
        protected List<TestPaperInfo> testPaperList = new List<TestPaperInfo>();
        Dictionary<int, string> companyNameDic = new Dictionary<int, string>();
        Dictionary<int, string> realNameDic = new Dictionary<int, string>();


        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "最小学习量达成分析表";
            base.CheckUserPower("ReadEMSReport", PowerCheckType.Single);

            TestPaperInfo testPaperSearch = new TestPaperInfo();
            testPaperSearch.TestMinDate = startDate;
            testPaperSearch.TestMaxDate = ShopCommon.SearchEndDate(endDate);
            testPaperSearch.Scorse = score;
            testPaperSearch.IsPass = isPass;
            testPaperSearch.PaperName = courseName;
            if (companyID < 0)
                testPaperSearch.CompanyIdCondition = base.ExistsSonCompany ? base.SonCompanyID : base.UserCompanyID.ToString();
            else
                testPaperSearch.CompanyIdCondition = companyID.ToString();

            if (!string.IsNullOrEmpty(realName) || !string.IsNullOrEmpty(userName))
            {
                UserSearchInfo user = new UserSearchInfo();
                user.RealName = realName;
                user.UserName = userName;
                user.InCompanyID = testPaperSearch.CompanyIdCondition;
                testPaperSearch.UserIdCondition = UserBLL.ReadUserIdStr(UserBLL.SearchUserList(user));
                if (string.IsNullOrEmpty(testPaperSearch.UserIdCondition)) testPaperSearch.UserIdCondition = "0";
            }

            //if (!string.IsNullOrEmpty(courseName))
            //{
            //    testPaperSearch.Condition = "[CateID] in (select ID from [_Product] where [Name] like '%" + courseName + "%')";
            //}
            testPaperList = TestPaperBLL.ReadList(testPaperSearch, base.CurrentPage, base.PageSize, ref this.Count);
            base.BindPageControl(ref base.CommonPager);
        }

        protected string GetCompanyName(int companyID)
        {
            if (!companyNameDic.ContainsKey(companyID))
            {
                companyNameDic.Add(companyID, CompanyBLL.ReadCompany(companyID).CompanySimpleName);
            }
            return companyNameDic[companyID];
        }

        protected string GetRealName(int userID)
        {
            if (!realNameDic.ContainsKey(userID))
            {
                realNameDic.Add(userID, UserBLL.ReadUser(userID).RealName);
            }
            return realNameDic[userID];
        }
    }
}
