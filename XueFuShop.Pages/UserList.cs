using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class UserList : UserManageBasePage
    {
        protected int companyID = RequestHelper.GetQueryString<int>("CompanyId");
        protected int state = RequestHelper.GetQueryString<int>("State");
        protected List<UserInfo> userList = new List<UserInfo>();
        protected DataTable dt = new DataTable();
        protected List<PostInfo> workingPostList = new List<PostInfo>();
        protected CompanyInfo currentCompany = new CompanyInfo();

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "员工列表";
            this.dt = UserBLL.CompanyIndexStatistics(base.UserCompanyID);
            this.currentCompany = CompanyBLL.ReadCompany(base.UserCompanyID);

            string companyPostSetting = CookiesHelper.ReadCookieValue("UserCompanyPostSetting");
            workingPostList = PostBLL.ReadPostListByPostId(companyPostSetting);

            string action = RequestHelper.GetQueryString<string>("Action");
            if (!string.IsNullOrEmpty(action))
            {
                string selectID = RequestHelper.GetQueryString<string>("SelectID");
                if (!string.IsNullOrEmpty(selectID))
                {
                    string alertMessage = string.Empty;
                    if (action == "Delete")
                    {
                        base.CheckUserPower("DeleteUser", PowerCheckType.Single);
                        UserBLL.ChangeUserStatus(selectID, (int)UserState.Del);
                        TestPaperBLL.DeletePaperByUserID(selectID);
                        alertMessage = ShopLanguage.ReadLanguage("DeleteOK");
                        AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("User"), selectID);
                    }
                    else if (action == "Freeze")
                    {
                        base.CheckUserPower("FreezeUser", PowerCheckType.Single);
                        UserBLL.ChangeUserStatus(selectID, (int)UserState.Frozen);
                        TestPaperBLL.DeletePaperByUserID(selectID);
                        alertMessage = ShopLanguage.ReadLanguage("FreezeOK");
                        AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("FreezeRecord"), ShopLanguage.ReadLanguage("User"), selectID);
                    }
                    else if (action == "UnFreeze")
                    {
                        base.CheckUserPower("UnFreezeUser", PowerCheckType.Single);
                        UserBLL.ChangeUserStatus(selectID, (int)UserState.Normal);
                        TestPaperBLL.RecoveryPaperByUserID(selectID);
                        alertMessage = ShopLanguage.ReadLanguage("UnFreezeOK");
                        AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UnFreezeRecord"), ShopLanguage.ReadLanguage("User"), selectID);
                        AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("RecoveryRecord"), ShopLanguage.ReadLanguage("TestPaperRecord"), selectID);
                    }
                    else if (action == "Free")
                    {
                        base.CheckUserPower("FreezeUser", PowerCheckType.Single);
                        UserBLL.ChangeUserStatus(selectID, (int)UserState.Free);
                        alertMessage = ShopLanguage.ReadLanguage("FreeOK");
                        AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("FreeRecord"), ShopLanguage.ReadLanguage("User"), selectID);
                    }
                    ScriptHelper.Alert(alertMessage, Request.UrlReferrer.ToString());
                }
            }
            else
            {
                base.CheckUserPower("ReadUser", PowerCheckType.Single);
                UserSearchInfo userSearch = new UserSearchInfo();
                if (companyID > 0)
                    userSearch.InCompanyID = companyID.ToString();
                else
                    userSearch.InCompanyID = base.SonCompanyID;
                //userSearch.OutUserID = base.UserID.ToString();
                userSearch.UserName = RequestHelper.GetQueryString<string>("UserId");
                userSearch.RealName = RequestHelper.GetQueryString<string>("RealName");
                userSearch.Email = RequestHelper.GetQueryString<string>("Email");
                userSearch.Mobile = RequestHelper.GetQueryString<string>("Mobile");
                userSearch.InWorkingPostID = RequestHelper.GetQueryString<string>("WorkingPostID");
                if (state < 0) state = (int)UserState.Normal;
                //userSearch.Status = state;
                if (state == (int)UserState.Free)
                    userSearch.InStatus = string.Concat(state, ",", (int)UserState.Other);
                else
                    userSearch.Status = state;
                //userSearch.GroupId = 36;
                //userSearch.Sex = RequestHelper.GetQueryString<int>("Sex");
                //userSearch.StartRegisterDate = RequestHelper.GetQueryString<DateTime>("StartRegisterDate");
                //userSearch.EndRegisterDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndRegisterDate"));
                //if (UserDel > 0) user.Del = UserDel;

                userList = UserBLL.SearchUserList(base.CurrentPage, base.PageSize, userSearch, ref this.Count);
                base.BindPageControl(ref base.CommonPager);
            }
        }
    }
}
