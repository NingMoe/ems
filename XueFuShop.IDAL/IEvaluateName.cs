using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IEvaluateName
    {
        int AddEvaluateName(EvaluateNameInfo evaluateName);
        void UpdateEvaluateName(EvaluateNameInfo evaluateName);
        void DeleteEvaluateName(string strID);
        void DeleteEvaluateNameByCompanyID(string companyID);
        EvaluateNameInfo ReadEvaluateName(int id);
        List<EvaluateNameInfo> SearchEvaluateNameList(EvaluateNameInfo evaluateName);
        List<EvaluateNameInfo> SearchEvaluateNameList(EvaluateNameInfo evaluateName, int currentPage, int pageSize, ref int count);
    }
}
