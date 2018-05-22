using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IUserFriend
    {
        int AddUserFriend(UserFriendInfo userFriend);
        void DeleteUserFriend(string strID, int userID);
        UserFriendInfo ReadUserFriend(int id, int userID);
        UserFriendInfo ReadUserFriendByFriendID(int friendID, int userID);
        string ReadUserFriendIDList(string strID, int userID);
        List<UserFriendInfo> SearchUserFriendList(UserFriendSearchInfo userFriend);
        List<UserFriendInfo> SearchUserFriendList(int currentPage, int pageSize, UserFriendSearchInfo userFriend, ref int count);
        void UpdateUserFriend(UserFriendInfo userFriend);
    }
}
