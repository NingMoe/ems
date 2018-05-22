using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IUserRecharge
    {
        int AddUserRecharge(UserRechargeInfo userRecharge);
        void DeleteUserRecharge(string strID, int userID);
        UserRechargeInfo ReadUserRecharge(int id, int userID);
        UserRechargeInfo ReadUserRechargeByNumber(string number, int userID);
        string ReadUserRechargeIDList(string strID, int userID);
        List<UserRechargeInfo> SearchUserRechargeList(UserRechargeSearchInfo userRecharge);
        List<UserRechargeInfo> SearchUserRechargeList(int currentPage, int pageSize, UserRechargeSearchInfo userRecharge, ref int count);
        void UpdateUserRecharge(UserRechargeInfo userRecharge);
    }
}
