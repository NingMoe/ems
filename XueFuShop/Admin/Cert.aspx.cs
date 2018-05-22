using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class Cert : AdminBasePage
    {
        Dictionary<int, string> companyNameDic = new Dictionary<int, string>();
        Dictionary<int, UserInfo> realNameDic = new Dictionary<int, UserInfo>();
        Dictionary<int, int> postpassNumDic = new Dictionary<int, int>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = RequestHelper.GetQueryString<int>("ID");
                string Action = RequestHelper.GetQueryString<string>("Action");
                if (Action == "Search")
                {
                    string searchName = RequestHelper.GetQueryString<string>("username");
                    string searchCompanyName = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("companyname").Trim());
                    string searchPostName = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("postname").Trim());
                    DateTime startDate = RequestHelper.GetQueryString<DateTime>("StartDate");
                    DateTime endDate = RequestHelper.GetQueryString<DateTime>("EndDate");
                    int pageCount = RequestHelper.GetQueryString<int>("PageCount");
                    if (pageCount > 0)
                    {
                        base.PageSize = pageCount;
                        this.PageCount.Text = pageCount.ToString();
                    }
                    CompanyName.Text = searchCompanyName;
                    SearchName.Text = searchName;
                    PostName.Text = searchPostName;
                    if (startDate != DateTime.MinValue)
                        SearchStartDate.Text = startDate.ToString("d");
                    if (endDate != DateTime.MinValue)
                        SearchEndDate.Text = endDate.ToString("d");

                    base.CheckAdminPower("ReadTestPaper", PowerCheckType.Single);
                    PostPassInfo postpassSearch = new PostPassInfo();
                    postpassSearch.SearchStartDate = startDate;
                    postpassSearch.CreateDate = ShopCommon.SearchEndDate(endDate);
                    postpassSearch.PostName = searchPostName;
                    if (!string.IsNullOrEmpty(searchCompanyName))
                    {
                        postpassSearch.InCompanyID = CompanyBLL.ReadCompanyIdStr(searchCompanyName, 1);
                    }

                    if (!string.IsNullOrEmpty(searchName))
                    {
                        UserSearchInfo user = new UserSearchInfo();
                        user.RealName = searchName;
                        postpassSearch.InUserID = UserBLL.ReadUserIdStr(UserBLL.SearchUserList(user));
                        if (string.IsNullOrEmpty(postpassSearch.InUserID)) postpassSearch.InUserID = "0";
                    }

                    base.BindControl(PostPassBLL.ReadPostPassList(postpassSearch, base.CurrentPage, base.PageSize, ref this.Count), this.RecordList, this.MyPager);
                }
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ResponseHelper.Redirect("Cert.aspx?Action=Search&companyname=" + Server.UrlEncode(CompanyName.Text) + "&username=" + Server.UrlEncode(SearchName.Text) + "&postname=" + Server.UrlEncode(PostName.Text) + "&StartDate=" + SearchStartDate.Text + "&EndDate=" + SearchEndDate.Text + "&PageCount=" + PageCount.Text);
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

        protected int GetPostPassNum(int userID)
        {
            if (!postpassNumDic.ContainsKey(userID))
            {
                postpassNumDic.Add(userID, PostPassBLL.ReadPostPassNum(userID));
            }
            return postpassNumDic[userID];
        }
    }
}
