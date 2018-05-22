using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IKPI
    {
        int AddKPI(KPIInfo kpi);
        void UpdateKPI(KPIInfo kpi);
        void DeleteKPI(string strID);
        void DeleteKPIByCompanyID(string companyID);
        KPIInfo ReadKPI(int id);
        List<KPIInfo> SearchKPIList(KPISearchInfo kpiSearch);
        List<KPIInfo> SearchKPIList(KPISearchInfo kpiSearch, int currentPage, int pageSize, ref int count);
    }
}
