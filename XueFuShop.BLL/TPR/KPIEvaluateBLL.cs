using System;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.BLL
{
    public sealed class KPIEvaluateBLL
    {
        private static readonly IKPIEvaluate dal = FactoryHelper.Instance<IKPIEvaluate>(Global.DataProvider, "KPIEvaluateDAL");

        public static int AddKPIEvaluate(KPIEvaluateInfo kpiEvaluate)
        {
            return dal.AddKPIEvaluate(kpiEvaluate);
        }

        public static void UpdateKPIEvaluate(KPIEvaluateInfo kpiEvaluate)
        {
            dal.UpdateKPIEvaluate(kpiEvaluate);
        }

        public static void DeleteKPIEvaluate(string strID)
        {
            dal.DeleteKPIEvaluate(strID);
        }

        public static void DeleteKPIEvaluate(int userId, int postId, int evaluateNameId)
        {
            dal.DeleteKPIEvaluate(userId, postId, evaluateNameId);
        }

        public static void DeleteKPIEvaluateByUserID(string userID)
        {
            dal.DeleteKPIEvaluateByUserID(userID);
        }

        /// <summary>
        /// 更改评估记录状态
        /// </summary>
        /// <param name="evaluateNameId"></param>
        /// <param name="state"></param>
        public static void ChangeKPIEvaluateState(string evaluateNameId, DataState state)
        {
            dal.ChangeKPIEvaluateState(evaluateNameId, state);
        }

        public static KPIEvaluateInfo ReadKPIEvaluate(int id)
        {
            return dal.ReadKPIEvaluate(id);
        }

        public static List<KPIEvaluateReportInfo> KPIEvaluateReportList(KPIEvaluateSearchInfo kpiEvaluateSearch)
        {
            return dal.KPIEvaluateReportList(kpiEvaluateSearch);
        }

        //public static List<KPIEvaluateReportInfo> KPIEvaluateReportList(int userId, int postId)
        //{
        //    return dal.KPIEvaluateReportList(userId, postId);
        //}

        public static List<KPIEvaluateInfo> SearchKPIEvaluateList(KPIEvaluateSearchInfo kpiEvaluateSearch)
        {
            return dal.SearchKPIEvaluateList(kpiEvaluateSearch);
        }

        public static List<KPIEvaluateInfo> SearchFixedKPIEvaluateList(KPIEvaluateSearchInfo kpiEvaluateSearch)
        {
            return dal.SearchFixedKPIEvaluateList(kpiEvaluateSearch);
        }

        public static List<KPIEvaluateInfo> SearchKPIEvaluateList(KPIEvaluateSearchInfo kpiEvaluateSearch, int currentPage, int pageSize, ref int count)
        {
            return dal.SearchKPIEvaluateList(kpiEvaluateSearch, currentPage, pageSize, ref count);
        }

        /// <summary>
        /// 读取个人综合评估的数据
        /// </summary>
        /// <param name="kpievaluateSearch"></param>
        /// <returns></returns>
        public static string ReadStaffEvaluateData(KPIEvaluateSearchInfo kpievaluateSearch)
        {
            string scoreStr = string.Empty;
            List<KPIEvaluateInfo> kpiEvaluateList = SearchKPIEvaluateList(kpievaluateSearch);
            if (kpiEvaluateList.Count > 0)
            {
                foreach (KPIEvaluateInfo info in kpiEvaluateList)
                {
                    if (string.IsNullOrEmpty(scoreStr))
                        scoreStr = info.KPIId + ":" + info.Scorse;
                    else
                        scoreStr += "," + info.KPIId + ":" + info.Scorse;
                }
            }
            return scoreStr;
        }
    }
}
