using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{    
    public interface IAdminGroup
    {       
        int AddAdminGroup(AdminGroupInfo adminGroup);
        void ChangeAdminGroupCount(int id, ChangeAction action);
        void ChangeAdminGroupCountByGeneral(string strID, ChangeAction action);
        void DeleteAdminGroup(string strID);
        List<AdminGroupInfo> ReadAdminGroupAllList();
        void UpdateAdminGroup(AdminGroupInfo adminGroup);
    }
}
