using System;
using System.Data;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{   
    public interface IUser
    {        
        int AddUser(UserInfo user);
        void ChangePassword(int id, string newPassword);
        void ChangePassword(int id, string oldPassword, string newPassword);
        void ChangeUserSafeCode(int userID, string safeCode, DateTime findDate);
        void ChangeUserStatus(string strID, int status);
        bool CheckEmail(string email);
        UserInfo CheckUserLogin(string loginName, string loginPass);
        int CheckUserName(string userName);
        void DeleteUser(string strID);
        void DeleteUserByCompanyID(int CompanyID);
        UserInfo ReadUser(int id);
        UserInfo ReadUserByOpenID(string openID);
        UserInfo ReadUserByUserName(string userName);
        UserInfo ReadUserByMobile(string mobile);
        List<string> ReadUserEmailByMoneyUsed(Dictionary<decimal, decimal> moneyUsed);
        UserInfo ReadUserMore(int id);
        string ReadUserGroupIDByCompanyID(string companyID);
        List<UserInfo> SearchReportUserList(UserSearchInfo user);
        List<UserInfo> SearchUserList(UserSearchInfo user);
        List<UserInfo> SearchUserList(int currentPage, int pageSize, UserSearchInfo user, ref int count);
        DataTable StatisticsUserActive(int currentPage, int pageSize, UserSearchInfo userSearch, ref int count, string orderField);
        DataTable StatisticsUserConsume(int currentPage, int pageSize, UserSearchInfo userSearch, ref int count, string orderField, DateTime startDate, DateTime endDate);
        DataTable StatisticsUserCount(UserSearchInfo userSearch, DateType dateType);
        DataTable StatisticsUserStatus(UserSearchInfo userSearch);
        void UpdateUser(UserInfo user);
        void UpdateUserLogin(int id, DateTime lastLoginDate, string lastLoginIP);
        DataTable UserIndexStatistics(int userID);
        DataTable CompanyIndexStatistics(int companyID);
    }
}
