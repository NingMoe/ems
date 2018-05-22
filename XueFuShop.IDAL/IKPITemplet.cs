using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IKPITemplet
    {
        int AddKPITemplet(KPITempletInfo kpiTemplet);
        void UpdateKPITemplet(KPITempletInfo kpiTemplet);
        void DeleteKPITemplet(string strID);
        void DeleteKPITempletByCompanyID(string companyID);
        int ExistsKPITemplet(int companyID, string kpiContent);
        KPITempletInfo ReadKPITemplet(int id);
        List<KPITempletInfo> SearchKPITempletList(KPITempletInfo kpiTemplet);
    }
}
