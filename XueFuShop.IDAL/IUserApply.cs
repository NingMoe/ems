using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IUserApply
    {
        int AddUserApply(UserApplyInfo userApply);
        void DeleteUserApply(string strID, int userID);
        void DeleteUserApplyByUserID(string strUserID);
        UserApplyInfo ReadUserApply(int id, int userID);
        string ReadUserApplyIDList(string strID, int userID);
        List<UserApplyInfo> SearchUserApplyList(UserApplySearchInfo userApply);
        List<UserApplyInfo> SearchUserApplyList(int currentPage, int pageSize, UserApplySearchInfo userApply, ref int count);
        void UpdateUserApply(UserApplyInfo userApply);
    }
}
