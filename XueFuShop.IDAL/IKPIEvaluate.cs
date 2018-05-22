using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IKPIEvaluate
    {
        int AddKPIEvaluate(KPIEvaluateInfo kpiEvaluate);
        void UpdateKPIEvaluate(KPIEvaluateInfo kpiEvaluate);
        void DeleteKPIEvaluate(string strID);
        void DeleteKPIEvaluate(int userId, int postId, int evaluateNameId);
        void DeleteKPIEvaluateByUserID(string userID);
        void ChangeKPIEvaluateState(string evaluateNameId, DataState state);
        KPIEvaluateInfo ReadKPIEvaluate(int id);
        List<KPIEvaluateReportInfo> KPIEvaluateReportList(KPIEvaluateSearchInfo kpiEvaluateSearch);
        //List<KPIEvaluateReportInfo> KPIEvaluateReportList(int userId, int postId);
        List<KPIEvaluateInfo> SearchKPIEvaluateList(KPIEvaluateSearchInfo kpiEvaluateSearch);
        List<KPIEvaluateInfo> SearchFixedKPIEvaluateList(KPIEvaluateSearchInfo kpiEvaluateSearch);
        List<KPIEvaluateInfo> SearchKPIEvaluateList(KPIEvaluateSearchInfo kpiEvaluateSearch, int currentPage, int pageSize, ref int count);
    }
}
