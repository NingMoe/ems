using System;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.BLL
{
    public sealed class KPITempletBLL
    {
        private static readonly string cacheKey = CacheKey.ReadCacheKey("SystemKPITemplet");
        private static readonly IKPITemplet dal = FactoryHelper.Instance<IKPITemplet>(Global.DataProvider, "KPITempletDAL");

        public static int AddKPITemplet(KPITempletInfo kpiTemplet)
        {
            kpiTemplet.ID = dal.AddKPITemplet(kpiTemplet);
            if (kpiTemplet.CompanyId == 0) CacheHelper.Remove(cacheKey);
            return kpiTemplet.ID;
        }

        public static void UpdateKPITemplet(KPITempletInfo kpiTemplet)
        {
            dal.UpdateKPITemplet(kpiTemplet);
            if (kpiTemplet.CompanyId == 0) CacheHelper.Remove(cacheKey);
        }

        public static void DeleteKPITemplet(int id)
        {
            DeleteKPITemplet(id.ToString());
        }

        public static void DeleteKPITemplet(string strID)
        {
            dal.DeleteKPITemplet(strID);
            CacheHelper.Remove(cacheKey);
        }

        public static void DeleteKPITempletByCompanyID(string companyID)
        {
            dal.DeleteKPITempletByCompanyID(companyID);
            CacheHelper.Remove(cacheKey);
        }

        /// <summary>
        /// 查找是否存在指定companyID下的KPI模板
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="kpiContent"></param>
        /// <returns>不存在返回0，存在返回ID值</returns>
        public static int ExistsKPITemplet(int companyID, string kpiContent)
        {
            return dal.ExistsKPITemplet(companyID, kpiContent);
        }

        public static KPITempletInfo ReadKPITemplet(int id)
        {
            return dal.ReadKPITemplet(id);
        }

        public static List<KPITempletInfo> SearchKPITempletList(KPITempletInfo kpiTemplet)
        {
            return dal.SearchKPITempletList(kpiTemplet);
        }

        public static List<KPITempletInfo> ReadSystemKPITempletList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                KPITempletInfo kpiTemplet = new KPITempletInfo();
                kpiTemplet.CompanyId = 0;
                CacheHelper.Write(cacheKey, SearchKPITempletList(kpiTemplet));
            }
            return (List<KPITempletInfo>)CacheHelper.Read(cacheKey);
        }
    }
}
