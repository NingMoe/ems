using System;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.BLL
{
    public sealed class KPIBLL
    {
        private static readonly IKPI dal = FactoryHelper.Instance<IKPI>(Global.DataProvider, "KPIDAL");

        public static int AddKPI(KPIInfo kpi)
        {
            return dal.AddKPI(kpi);
        }

        public static void UpdateKPI(KPIInfo kpi)
        {
            dal.UpdateKPI(kpi);
        }

        public static void DeleteKPI(string strID)
        {
            dal.DeleteKPI(strID);
        }

        public static void DeleteKPIByCompanyID(string companyID)
        {
            dal.DeleteKPIByCompanyID(companyID);
        }

        public static KPIInfo ReadKPI(int id)
        {
            return dal.ReadKPI(id);
        }

        public static List<KPIInfo> SearchKPIList(KPISearchInfo kpiSearch)
        {
            return dal.SearchKPIList(kpiSearch);
        }

        public static List<KPIInfo> SearchKPIList(KPISearchInfo kpiSearch, int currentPage, int pageSize, ref int count)
        {
            return dal.SearchKPIList(kpiSearch, currentPage, pageSize, ref count);
        }
    }
}
