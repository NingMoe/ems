using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.BLL
{
    public class EvaluateNameBLL
    {
        private static readonly IEvaluateName dal = FactoryHelper.Instance<IEvaluateName>(Global.DataProvider, "EvaluateNameDAL");

        public static int AddEvaluateName(EvaluateNameInfo evaluateName)
        {
            return dal.AddEvaluateName(evaluateName);
        }

        public static void UpdateEvaluateName(EvaluateNameInfo evaluateName) 
        {
            dal.UpdateEvaluateName(evaluateName);
        }

        public static void DeleteEvaluateName(string strID)
        {
            KPIEvaluateBLL.ChangeKPIEvaluateState(strID, DataState.Delete);
            dal.DeleteEvaluateName(strID);
        }

        public static void DeleteEvaluateNameByCompanyID(string companyID)
        {
            dal.DeleteEvaluateNameByCompanyID(companyID);
        }

        public static EvaluateNameInfo ReadEvaluateName(int id)
        {
            return dal.ReadEvaluateName(id);
        }

        public static List<EvaluateNameInfo> SearchEvaluateNameList(EvaluateNameInfo evaluateName)
        {
            return dal.SearchEvaluateNameList(evaluateName);
        }

        public static List<EvaluateNameInfo> SearchEvaluateNameList(EvaluateNameInfo evaluateName, int currentPage, int pageSize, ref int count)
        {
            return dal.SearchEvaluateNameList(evaluateName, currentPage, pageSize, ref count);
        }

        public static string GetEvaluateNameListHtml(string companyID, int evaluateNameId)
        {
            Dictionary<int, string> companyNameDic = new Dictionary<int, string>();
            StringBuilder evaluateNameList = new StringBuilder();
            EvaluateNameInfo evaluateName = new EvaluateNameInfo();
            evaluateName.CompanyIdCondition = companyID;
            evaluateNameList.Append("<option value=\"\">«Î—°‘Ò∆¿π¿√˚≥∆</option>");
            foreach (EvaluateNameInfo info in EvaluateNameBLL.SearchEvaluateNameList(evaluateName))
            {
                evaluateNameList.Append("<option value=\"" + info.ID + "\"");
                if (evaluateNameId == info.ID) evaluateNameList.Append(" selected");
                evaluateNameList.Append(">" + info.EvaluateName);
                if (info.CompanyId != Cookies.User.GetCompanyID(true))
                {
                    if (!companyNameDic.ContainsKey(info.CompanyId))
                    {
                        string companyName = CompanyBLL.ReadCompany(info.CompanyId).CompanySimpleName;
                        companyNameDic.Add(info.CompanyId, companyName);
                    }
                    evaluateNameList.Append("[" + companyNameDic[info.CompanyId] + "]");
                }
                evaluateNameList.Append("</option>");
            }
            return evaluateNameList.ToString();
        }
    }
}
