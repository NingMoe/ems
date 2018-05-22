using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Security;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.BLL
{   
    public sealed class UserBLL
    {
        
        private static readonly IUser dal = FactoryHelper.Instance<IUser>(Global.DataProvider, "UserDAL");
        public static readonly int TableID = UploadTable.ReadTableID("User");

        ///// <summary>
        ///// 当前用户ID
        ///// </summary>
        //public static int CurrentUserId
        //{
        //    get { return Cookies.User.GetUserID(true); }
        //}

        ///// <summary>
        ///// 当前用户公司ID
        ///// </summary>
        //public static int CurrentUserCompanyId
        //{
        //    get { return Cookies.User.GetCompanyID(true); }
        //}

        ///// <summary>
        ///// 当前用户权限组ID
        ///// </summary>
        //public static int CurrentUserGroupId
        //{
        //    get { return Cookies.User.GetGroupID(true); }
        //}

        ///// <summary>
        ///// 当前用户工作岗位ID
        ///// </summary>
        //public static int CurrentUserPostId
        //{
        //    get { return int.Parse(CookiesHelper.ReadCookieValue("UserPostId")); }
        //}

        ///// <summary>
        ///// 当前用户学习岗位ID
        ///// </summary>
        //public static int CurrentUserstudyPostId
        //{
        //    get { return int.Parse(CookiesHelper.ReadCookieValue("UserStydyPostId")); }
        //}
        
        public static int AddUser(UserInfo user)
        {
            user.ID = dal.AddUser(user);
            UploadBLL.UpdateUpload(TableID, 0, user.ID, Cookies.Admin.GetRandomNumber(false));
            return user.ID;
        }

        public static void ChangePassword(int id, string newPassword)
        {
            dal.ChangePassword(id, newPassword);
        }

        public static void ChangePassword(int id, string oldPassword, string newPassword)
        {
            dal.ChangePassword(id, oldPassword, newPassword);
        }

        public static void ChangeUserSafeCode(int userID, string safeCode, DateTime findDate)
        {
            dal.ChangeUserSafeCode(userID, safeCode, findDate);
        }

        public static void ChangeUserStatus(string strID, int status)
        {
            dal.ChangeUserStatus(strID, status);
        }

        public static bool CheckEmail(string email)
        {
            return dal.CheckEmail(email);
        }

        public static UserInfo CheckUserLogin(string loginName, string loginPass)
        {
            return dal.CheckUserLogin(loginName, loginPass);
        }

        public static int CheckUserName(string userName)
        {
            return dal.CheckUserName(userName);
        }


        /// <summary>
        /// 用户数是否超限
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static bool IsUserNumOverflow(int companyID)
        {
            bool isOverflow = false;
            UserSearchInfo userSearch = new UserSearchInfo();
            userSearch.InCompanyID = companyID.ToString();
            userSearch.StatusNoEqual = (int)UserState.Del;
            if (SearchUserList(userSearch).Count >= CompanyBLL.ReadCompany(companyID).UserNum)
            {
                isOverflow = true;
            }
            return isOverflow;
        }

        /// <summary>
        /// 是否存在手机号码(只验证非删除的考试人员)
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static bool IsExistMobile(string mobile, int userID)
        {
            bool isExist = false;
            if (!string.IsNullOrEmpty(mobile))
            {
                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.Mobile = mobile;
                userSearch.StatusNoEqual = (int)UserState.Del;
                userSearch.GroupId = 36;
                if (userID > 0)
                    userSearch.OutUserID = userID.ToString();
                if (UserBLL.SearchUserList(userSearch).Count > 0)
                    isExist = true;
            }
            return isExist;
        }

        public static void DeleteUser(string strID)
        {
            //UploadBLL.DeleteUploadByRecordID(TableID, strID);
            //dal.DeleteUser(strID);
            ChangeUserStatus(strID, (int)UserState.Del);
        }

        public static void DeleteUserByCompanyID(int CompanyID)
        {
            dal.DeleteUserByCompanyID(CompanyID);
        }

        public static UserInfo ReadUser(int id)
        {
            return dal.ReadUser(id);
        }

        public static UserInfo ReadUserByOpenID(string openID)
        {
            return dal.ReadUserByOpenID(openID);
        }

        public static UserInfo ReadUserByMobile(string mobile)
        {
            return dal.ReadUserByMobile(mobile);
        }

        public static UserInfo ReadUserByUserName(string userName)
        {
            return dal.ReadUserByUserName(userName);
        }

        public static UserInfo ReadUserByUserList(List<UserInfo> userList, int userID)
        {
            UserInfo info = new UserInfo();
            foreach (UserInfo info2 in userList)
            {
                if (info2.ID == userID)
                {
                    info = info2;
                }
            }
            return info;
        }

        public static List<string> ReadUserEmailByMoneyUsed(Dictionary<decimal, decimal> moneyUsed)
        {
            return dal.ReadUserEmailByMoneyUsed(moneyUsed);
        }

        public static UserInfo ReadUserMore(int id)
        {
            return dal.ReadUserMore(id);
        }

        public static string ReadUserPhoto(string size)
        {
            string str = CookiesHelper.ReadCookieValue("UserPhoto");
            if (str == string.Empty)
            {
                return ("/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/Style/Images/NoImage.gif");
            }
            return str.Replace("Original", size);
        }


        /// <summary>
        /// 从用户列表中提取会员管理组ID串
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static string ReadUserGroupIDByCompanyID(string companyID)
        {
            return dal.ReadUserGroupIDByCompanyID(companyID);
        }

        /// <summary>
        /// 返回用户ID串
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        public static string ReadUserIdStr(List<UserInfo> userList)
        {
            string userIdStr = string.Empty;
            foreach (UserInfo info in userList)
            {
                userIdStr += "," + info.ID;
            }
            if (userIdStr.StartsWith(",")) return userIdStr.Substring(1);
            return userIdStr;
        }

        public static List<UserInfo> SearchReportUserList(UserSearchInfo user)
        {
            return dal.SearchReportUserList(user);
        }

        public static List<UserInfo> SearchUserList(UserSearchInfo user)
        {
            return dal.SearchUserList(user);
        }

        public static List<UserInfo> SearchUserList(int currentPage, int pageSize, UserSearchInfo user, ref int count)
        {
            return dal.SearchUserList(currentPage, pageSize, user, ref count);
        }

        public static DataTable StatisticsUserActive(int currentPage, int pageSize, UserSearchInfo userSearch, ref int count, string orderField)
        {
            return dal.StatisticsUserActive(currentPage, pageSize, userSearch, ref count, orderField);
        }

        public static DataTable StatisticsUserConsume(int currentPage, int pageSize, UserSearchInfo userSearch, ref int count, string orderField, DateTime startDate, DateTime endDate)
        {
            return dal.StatisticsUserConsume(currentPage, pageSize, userSearch, ref count, orderField, startDate, endDate);
        }

        public static DataTable StatisticsUserCount(UserSearchInfo userSearch, DateType dateType)
        {
            return dal.StatisticsUserCount(userSearch, dateType);
        }

        public static DataTable StatisticsUserStatus(UserSearchInfo userSearch)
        {
            return dal.StatisticsUserStatus(userSearch);
        }

        public static void UpdateUser(UserInfo user)
        {
            dal.UpdateUser(user);
            UploadBLL.UpdateUpload(TableID, 0, user.ID, Cookies.Admin.GetRandomNumber(false));
        }

        public static void UpdateUserLogin(int id, DateTime lastLoginDate, string lastLoginIP)
        {
            dal.UpdateUserLogin(id, lastLoginDate, lastLoginIP);
        }

        public static DataTable UserIndexStatistics(int userID)
        {
            return dal.UserIndexStatistics(userID);
        }

        public static DataTable CompanyIndexStatistics(int companyID)
        {
            return dal.CompanyIndexStatistics(companyID);
        }

        public static void UserLoginInit(UserInfo user)
        {
            int iD = UserGradeBLL.ReadUserGradeByMoney(user.MoneyUsed).ID;
            string str = FormsAuthentication.HashPasswordForStoringInConfigFile(user.ID.ToString() + HttpContext.Current.Server.UrlEncode(user.UserName) + user.MoneyUsed.ToString() + iD.ToString() + user.Mobile + user.GroupID.ToString() + user.CompanyID.ToString() + HttpContext.Current.Server.UrlEncode(user.RealName) + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent, "MD5");
            string str2 = string.Concat(new object[] { str, "|", user.ID.ToString(), "|", HttpContext.Current.Server.UrlEncode(user.UserName), "|", user.MoneyUsed, "|", iD, "|", user.Mobile, "|", user.GroupID, "|", user.CompanyID, "|", HttpContext.Current.Server.UrlEncode(user.RealName) });
            CookiesHelper.AddCookie(ShopConfig.ReadConfigInfo().UserCookies, str2);
            CookiesHelper.AddCookie("UserPhoto", user.Photo);
            CookiesHelper.AddCookie("UserEmail", user.Email);
            CookiesHelper.AddCookie("UserWorkingPostId", user.WorkingPostID.ToString());
            CookiesHelper.AddCookie("UserStudyPostId", user.StudyPostID.ToString());
            CompanyInfo CompanyModel = CompanyBLL.ReadCompany(user.CompanyID);
            CookiesHelper.AddCookie("UserCompanyType", CompanyModel.GroupId.ToString());
            CookiesHelper.AddCookie("UserCompanyPostSetting", CompanyModel.Post);
            CookiesHelper.AddCookie("UserCompanyBrandID", CompanyModel.BrandId);
            CookiesHelper.AddCookie("UserCompanySonCompanyID", CompanyBLL.ReadCompanyIdList(user.CompanyID.ToString()));
            CookiesHelper.AddCookie("UserCompanyParentCompanyID", CompanyBLL.ReadParentCompanyIDWithSelf(user.CompanyID));
            CartBLL.CookiesImportDataBase(user.ID, user.UserName);
            CartBLL.StaticsCart(user.ID, iD);
        }
    }
 


 

}
