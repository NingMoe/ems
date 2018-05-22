using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.IDAL;
using XueFuShop.Common;

namespace XueFuShop.BLL
{
    public sealed class UserGroupBLL
    {
        private static readonly string cacheKey = CacheKey.ReadCacheKey("UserGroup");
        private static readonly IUserGroup dal = FactoryHelper.Instance<IUserGroup>(Global.DataProvider, "UserGroupDAL");

        public static int AddUserGroup(UserGroupInfo userGroup)
        {
            userGroup.ID = dal.AddUserGroup(userGroup);
            CacheHelper.Remove(cacheKey);
            return userGroup.ID;
        }

        public static void ChangeUserGroupCount(int id, ChangeAction action)
        {
            dal.ChangeUserGroupCount(id, action);
            CacheHelper.Remove(cacheKey);
        }

        public static void ChangeUserGroupCountByGeneral(string strID, ChangeAction action)
        {
            dal.ChangeUserGroupCountByGeneral(strID, action);
            CacheHelper.Remove(cacheKey);
        }

        public static void DeleteUserGroup(string strID)
        {
            //UserBLL.DeleteUserByGroupID(strID);
            dal.DeleteUserGroup(strID);
            CacheHelper.Remove(cacheKey);
        }

        public static UserGroupInfo ReadUserGroupCache(int id)
        {
            UserGroupInfo info = new UserGroupInfo();
            List<UserGroupInfo> list = ReadUserGroupCacheList();
            foreach (UserGroupInfo info2 in list)
            {
                if (info2.ID == id)
                {
                    return info2;
                }
            }
            return info;
        }

        public static List<UserGroupInfo> ReadUserGroupCacheList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadUserGroupAllList());
            }
            return (List<UserGroupInfo>)CacheHelper.Read(cacheKey);
        }

        public static void UpdateUserGroup(UserGroupInfo userGroup)
        {
            dal.UpdateUserGroup(userGroup);
            CacheHelper.Remove(cacheKey);
        }
    }
}
