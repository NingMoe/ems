using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.IDAL;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;

namespace XueFuShop.BLL
{
    public sealed class UserFriendBLL
    {
        
        private static readonly IUserFriend dal = FactoryHelper.Instance<IUserFriend>(Global.DataProvider, "UserFriendDAL");

        
        public static int AddUserFriend(UserFriendInfo userFriend)
        {
            userFriend.ID = dal.AddUserFriend(userFriend);
            return userFriend.ID;
        }

        public static void DeleteUserFriend(string strID, int userID)
        {
            if (userID != 0)
            {
                strID = dal.ReadUserFriendIDList(strID, userID);
            }
            dal.DeleteUserFriend(strID, userID);
        }

        public static UserFriendInfo ReadUserFriend(int id, int userID)
        {
            return dal.ReadUserFriend(id, userID);
        }

        public static UserFriendInfo ReadUserFriendByFriendID(int friendID, int userID)
        {
            return dal.ReadUserFriendByFriendID(friendID, userID);
        }

        public static List<UserFriendInfo> SearchUserFriendList(UserFriendSearchInfo userFriend)
        {
            return dal.SearchUserFriendList(userFriend);
        }

        public static List<UserFriendInfo> SearchUserFriendList(int currentPage, int pageSize, UserFriendSearchInfo userFriend, ref int count)
        {
            return dal.SearchUserFriendList(currentPage, pageSize, userFriend, ref count);
        }

        public static void UpdateUserFriend(UserFriendInfo userFriend)
        {
            dal.UpdateUserFriend(userFriend);
        }
    }
}
