using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.IDAL;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;

namespace XueFuShop.BLL
{
    public sealed class UserApplyBLL
    {
        private static readonly IUserApply dal = FactoryHelper.Instance<IUserApply>(Global.DataProvider, "UserApplyDAL");

        public static int AddUserApply(UserApplyInfo userApply)
        {
            userApply.ID = dal.AddUserApply(userApply);
            return userApply.ID;
        }

        public static void DeleteUserApply(string strID, int userID)
        {
            if (userID != 0)
            {
                strID = dal.ReadUserApplyIDList(strID, userID);
            }
            dal.DeleteUserApply(strID, userID);
        }

        public static string ReadApplyStatus(int applyStatus)
        {
            string str = string.Empty;
            switch (applyStatus)
            {
                case 1:
                    return "������";

                case 2:
                    return "�����";

                case 3:
                    return "ȡ��";
            }
            return str;
        }

        public static UserApplyInfo ReadUserApply(int id, int userID)
        {
            return dal.ReadUserApply(id, userID);
        }

        public static List<UserApplyInfo> SearchUserApplyList(UserApplySearchInfo userApply)
        {
            return dal.SearchUserApplyList(userApply);
        }

        public static List<UserApplyInfo> SearchUserApplyList(int currentPage, int pageSize, UserApplySearchInfo userApply, ref int count)
        {
            return dal.SearchUserApplyList(currentPage, pageSize, userApply, ref count);
        }

        public static void UpdateUserApply(UserApplyInfo userApply)
        {
            dal.UpdateUserApply(userApply);
        }
    }
}
