using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IUserGroup
    {
        int AddUserGroup(UserGroupInfo userGroup);
        void ChangeUserGroupCount(int id, ChangeAction action);
        void ChangeUserGroupCountByGeneral(string strID, ChangeAction action);
        void DeleteUserGroup(string strID);
        List<UserGroupInfo> ReadUserGroupAllList();
        void UpdateUserGroup(UserGroupInfo userGroup);
    }
}
