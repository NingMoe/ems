using System;
using System.Collections.Generic;
using XueFuShop.Models;
using XueFuShop.IDAL;
using XueFuShop.Common;
using XueFu.EntLib;

namespace XueFuShop.BLL
{
    public sealed class UserMessageBLL
    {
        
        private static readonly IUserMessage dal = FactoryHelper.Instance<IUserMessage>(Global.DataProvider, "UserMessageDAL");

        
        public static int AddUserMessage(UserMessageInfo userMessage)
        {
            userMessage.ID = dal.AddUserMessage(userMessage);
            return userMessage.ID;
        }

        public static void DeleteUserMessage(string strID, int userID)
        {
            if (userID != 0)
            {
                strID = dal.ReadUserMessageIDList(strID, userID);
            }
            dal.DeleteUserMessage(strID, userID);
        }

        public static void DeleteUserMessageByUserID(string strUserID)
        {
            dal.DeleteUserMessageByUserID(strUserID);
        }

        public static string ReadMessageType(int messageType)
        {
            string str = string.Empty;
            switch (messageType)
            {
                case 1:
                    return "����";

                case 2:
                    return "Ͷ��";

                case 3:
                    return "ѯ��";

                case 4:
                    return "�ۺ�";

                case 5:
                    return "��";
            }
            return str;
        }

        public static UserMessageInfo ReadUserMessage(int id, int userID)
        {
            return dal.ReadUserMessage(id, userID);
        }
        /// <summary>
        /// ��ȡ��������Ķ������
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<UserMessageInfo> ReadSonUserMessage(int parentID, int userID)
        {
            UserMessageSeachInfo userMessage = new UserMessageSeachInfo();
            userMessage.ParentID = parentID;
            userMessage.UserID = userID;
            return SearchUserMessageList(userMessage);
        }

        public static List<UserMessageInfo> SearchUserMessageList(UserMessageSeachInfo userMessage)
        {
            return dal.SearchUserMessageList(userMessage);
        }

        public static List<UserMessageInfo> SearchUserMessageList(int currentPage, int pageSize, UserMessageSeachInfo userMessage, ref int count)
        {
            return dal.SearchUserMessageList(currentPage, pageSize, userMessage, ref count);
        }

        public static void UpdateUserMessage(UserMessageInfo userMessage)
        {
            dal.UpdateUserMessage(userMessage);
        }
    }
}
