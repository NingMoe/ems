using System;
using System.Data;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.BLL
{
    public class PostKPISettingBLL
    {
        private static readonly IPostKPISetting dal = FactoryHelper.Instance<IPostKPISetting>(Global.DataProvider, "PostKPISettingDAL");

        public static int AddPostKPISetting(PostKPISettingInfo info)
        {
            return dal.AddPostKPISetting(info);
        }
        public static void UpdatePostKPISetting(PostKPISettingInfo info)
        {
            dal.UpdatePostKPISetting(info);
        }
        public static void DeletePostKPISetting(string strID)
        {
            dal.DeletePostKPISetting(strID);
        }
        public static void DeletePostKPISettingByCompanyID(string companyID)
        {
            dal.DeletePostKPISettingByCompanyID(companyID);
        }
        public static PostKPISettingInfo ReadPostKPISetting(int id)
        {
            return dal.ReadPostKPISetting(id);
        }
        public static List<PostKPISettingInfo> SearchPostKPISettingList(PostKPISettingInfo info)
        {
            return dal.SearchPostKPISettingList(info);
        }
        public static List<PostKPISettingInfo> SearchPostKPISettingList(PostKPISettingInfo info, int currentPage, int pageSize, ref int count)
        {
            return dal.SearchPostKPISettingList(info, currentPage, pageSize, ref count);
        }
    }
}
