using System;
using XueFuShop.IDAL;
using System.Collections.Generic;
using XueFuShop.Common;
using XueFu.EntLib;
using XueFuShop.Models;

namespace XueFuShop.BLL
{
    public sealed class AdminGroupBLL
    {
        private static readonly string cacheKey = CacheKey.ReadCacheKey("AdminGroup");
        private static readonly IAdminGroup dal = FactoryHelper.Instance<IAdminGroup>(Global.DataProvider, "AdminGroupDAL");

        public static int AddAdminGroup(AdminGroupInfo adminGroup)
        {
            adminGroup.ID = dal.AddAdminGroup(adminGroup);
            CacheHelper.Remove(cacheKey);
            return adminGroup.ID;
        }

        public static void ChangeAdminGroupCount(int id, ChangeAction action)
        {
            dal.ChangeAdminGroupCount(id, action);
            CacheHelper.Remove(cacheKey);
        }

        public static void ChangeAdminGroupCountByGeneral(string strID, ChangeAction action)
        {
            dal.ChangeAdminGroupCountByGeneral(strID, action);
            CacheHelper.Remove(cacheKey);
        }

        public static void DeleteAdminGroup(string strID)
        {
            AdminBLL.DeleteAdminByGroupID(strID);
            dal.DeleteAdminGroup(strID);
            CacheHelper.Remove(cacheKey);
        }

        public static AdminGroupInfo ReadAdminGroupCache(int id)
        {
            AdminGroupInfo info = new AdminGroupInfo();
            List<AdminGroupInfo> list = ReadAdminGroupCacheList();
            foreach (AdminGroupInfo info2 in list)
            {
                if (info2.ID == id)
                {
                    return info2;
                }
            }
            return info;
        }

        public static List<AdminGroupInfo> ReadAdminGroupCacheList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadAdminGroupAllList());
            }
            return (List<AdminGroupInfo>)CacheHelper.Read(cacheKey);
        }

        /// <summary>
        /// 读取后台权限分组列表
        /// </summary>
        /// <returns></returns>
        public static List<AdminGroupInfo> ReadAdminGroupList()
        {
            List<AdminGroupInfo> resultData = new List<AdminGroupInfo>();
            List<AdminGroupInfo> adminGroupCacheList = ReadAdminGroupCacheList();
            foreach (AdminGroupInfo info in adminGroupCacheList)
            {
                if (info.CompanyID == CompanyBLL.SystemCompanyId && info.State == 0)
                {
                    resultData.Add(info);
                }
            }
            return resultData;
        }

        /// <summary>
        /// 读取客户公司权限分组列表
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static List<AdminGroupInfo> ReadAdminGroupList(int companyID)
        {
            List<AdminGroupInfo> resultData = new List<AdminGroupInfo>();
            List<AdminGroupInfo> adminGroupCacheList = ReadAdminGroupCacheList();
            string companyIDString = CompanyBLL.ReadParentCompanyIDWithSelf(companyID);
            companyIDString = StringHelper.SubString(companyIDString, "0");
            foreach (AdminGroupInfo info in adminGroupCacheList)
            {
                if (StringHelper.CompareSingleString(companyIDString, info.CompanyID.ToString()) || (info.CompanyID == CompanyBLL.SystemCompanyId && info.State == 1))
                {
                    resultData.Add(info);
                }
            }
            return resultData;
        }

        /// <summary>
        /// 根据权限组ID返回权限组列表
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public static List<AdminGroupInfo> ReadAdminGroupList(string groupID)
        {
            List<AdminGroupInfo> resultData = new List<AdminGroupInfo>();
            List<AdminGroupInfo> adminGroupCacheList = ReadAdminGroupCacheList();
            foreach (AdminGroupInfo info in adminGroupCacheList)
            {
                if (StringHelper.CompareSingleString(groupID, info.ID.ToString()))
                {
                    resultData.Add(info);
                }
            }
            return resultData;
        }

        /// <summary>
        /// 根据公司ID和权限组ID返回权限组列表
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public static List<AdminGroupInfo> ReadAdminGroupList(int companyID, string groupID)
        {
            List<AdminGroupInfo> resultData = new List<AdminGroupInfo>();
            List<AdminGroupInfo> adminGroupList = ReadAdminGroupList(companyID);
            foreach (AdminGroupInfo info in adminGroupList)
            {
                if (StringHelper.CompareSingleString(groupID, info.ID.ToString()))
                {
                    resultData.Add(info);
                }
            }
            return resultData;
        }

        public static void UpdateAdminGroup(AdminGroupInfo adminGroup)
        {
            dal.UpdateAdminGroup(adminGroup);
            CacheHelper.Remove(cacheKey);
        }
    }

}
