using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IPostKPISetting
    {
        int AddPostKPISetting(PostKPISettingInfo info);
        void UpdatePostKPISetting(PostKPISettingInfo info);
        void DeletePostKPISetting(string strID);
        void DeletePostKPISettingByCompanyID(string companyID);
        PostKPISettingInfo ReadPostKPISetting(int id);
        List<PostKPISettingInfo> SearchPostKPISettingList(PostKPISettingInfo info);
        List<PostKPISettingInfo> SearchPostKPISettingList(PostKPISettingInfo info, int currentPage, int pageSize, ref int count);
    }
}
