using System;
using System.IO;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Pages;
using XueFuShop.Common;

namespace XueFuShop.Admin
{
    public partial class TestPaper : AdminBasePage
    {
        protected bool deleteTestPaperPower = false;
        Dictionary<int, string> companyNameDic = new Dictionary<int, string>();
        Dictionary<int, UserInfo> realNameDic = new Dictionary<int, UserInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = RequestHelper.GetQueryString<int>("ID");
                string Action = RequestHelper.GetQueryString<string>("Action");
                if (Action == "Delete")
                {
                    if (id != int.MinValue)
                    {
                        base.CheckAdminPower("DeleteTestPaper", PowerCheckType.Single);
                        TestPaperInfo TestPaperModel = TestPaperBLL.ReadPaper(id);
                        string FilePath = ServerHelper.MapPath("~/xml/" + TestPaperModel.UserId.ToString() + "_" + TestPaperModel.CateId.ToString() + ".xml");
                        if (File.Exists(FilePath))
                        {
                            File.Delete(FilePath);
                        }
                        FilePath = ServerHelper.MapPath("~/m/xml/" + TestPaperModel.UserId.ToString() + "_" + TestPaperModel.CateId.ToString() + ".xml");
                        if (File.Exists(FilePath))
                        {
                            File.Delete(FilePath);
                        }
                        TestPaperBLL.DeletePaper(id);
                        AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("TestPaper"), id);
                        ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), Request.UrlReferrer.ToString());
                    }
                }
                if (Action == "Search")
                {
                    string searchUserName = RequestHelper.GetQueryString<string>("username");
                    string searchRealName = RequestHelper.GetQueryString<string>("realname");
                    string searchCompanyName = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("companyname").Trim());
                    string searchCourseName = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("coursename").Trim());
                    DateTime startDate = RequestHelper.GetQueryString<DateTime>("StartDate");
                    DateTime endDate = RequestHelper.GetQueryString<DateTime>("EndDate");
                    int isPass = RequestHelper.GetQueryString<int>("IsPass");
                    CompanyName.Text = searchCompanyName;
                    RealName.Text = searchRealName;
                    UserName.Text = searchUserName;
                    CourseName.Text = searchCourseName;
                    if (startDate != DateTime.MinValue)
                        SearchStartDate.Text = startDate.ToString("d");
                    if (endDate != DateTime.MinValue)
                        SearchEndDate.Text = endDate.ToString("d");
                    if (isPass >= 0)
                        IsPass.Text = isPass.ToString();

                    base.CheckAdminPower("ReadTestPaper", PowerCheckType.Single);
                    deleteTestPaperPower = base.CompareAdminPower("DeleteTestPaper", PowerCheckType.Single);
                    TestPaperInfo testPaperSearch = new TestPaperInfo();
                    testPaperSearch.TestMinDate = startDate;
                    testPaperSearch.TestMaxDate = ShopCommon.SearchEndDate(endDate);
                    testPaperSearch.IsPass = isPass;
                    testPaperSearch.PaperName = searchCourseName;
                    if (!string.IsNullOrEmpty(searchCompanyName))
                    {
                        testPaperSearch.CompanyIdCondition = CompanyBLL.ReadCompanyIdStr(searchCompanyName, 1);
                    }

                    if (!string.IsNullOrEmpty(searchRealName) || !string.IsNullOrEmpty(searchUserName))
                    {
                        UserSearchInfo user = new UserSearchInfo();
                        user.UserName = searchUserName;
                        user.RealName = searchRealName;
                        testPaperSearch.UserIdCondition = UserBLL.ReadUserIdStr(UserBLL.SearchUserList(user));
                        if (string.IsNullOrEmpty(testPaperSearch.UserIdCondition)) testPaperSearch.UserIdCondition = "0";
                    }

                    base.BindControl(TestPaperBLL.ReadList(testPaperSearch, base.CurrentPage, base.PageSize, ref this.Count), this.RecordList, this.MyPager);
                }
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ResponseHelper.Redirect("TestPaper.aspx?Action=Search&companyname=" + Server.UrlEncode(CompanyName.Text) + "&realname=" + Server.UrlEncode(RealName.Text) + "&username=" + Server.UrlEncode(UserName.Text) + "&coursename=" + Server.UrlEncode(CourseName.Text) + "&IsPass=" + IsPass.Text + "&StartDate=" + SearchStartDate.Text + "&EndDate=" + SearchEndDate.Text);
        }

        protected string GetCompanyName(int companyID)
        {
            if (!companyNameDic.ContainsKey(companyID))
            {
                companyNameDic.Add(companyID, CompanyBLL.ReadCompany(companyID).CompanySimpleName);
            }
            return companyNameDic[companyID];
        }

        protected UserInfo GetRealName(int userID)
        {
            if (!realNameDic.ContainsKey(userID))
            {
                realNameDic.Add(userID, UserBLL.ReadUser(userID));
            }
            return realNameDic[userID];
        }

        protected void Button1_ServerClick(object sender, EventArgs e)
        {
            base.CheckAdminPower("DeleteTestPaper", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (!string.IsNullOrEmpty(intsForm))
            {
                string[] Arr = intsForm.Split(',');
                for (int i = 0; i < Arr.Length; i++)
                {
                    TestPaperInfo TestPaperModel = TestPaperBLL.ReadPaper(int.Parse(Arr[i]));
                    string FilePath = ServerHelper.MapPath("~/xml/" + TestPaperModel.UserId.ToString() + "_" + TestPaperModel.CateId.ToString() + ".xml");
                    if (File.Exists(FilePath))
                    {
                        File.Delete(FilePath);
                    }
                    FilePath = ServerHelper.MapPath("~/m/xml/" + TestPaperModel.UserId.ToString() + "_" + TestPaperModel.CateId.ToString() + ".xml");
                    if (File.Exists(FilePath))
                    {
                        File.Delete(FilePath);
                    }
                    TestPaperBLL.DeletePaper(int.Parse(Arr[i]));
                }
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("TestPaper"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }
    }
}
