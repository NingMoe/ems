using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class EvaluateReport : UserManageBasePage
    {
        protected string action = RequestHelper.GetForm<string>("action");
        protected int evaluateNameId = RequestHelper.GetForm<int>("EvaluationName");
        protected string name = RequestHelper.GetForm<string>("Name");
        protected string postId = RequestHelper.GetForm<string>("PostId");
        protected string postName = RequestHelper.GetForm<string>("PostName");
        protected int qualifiedRate = RequestHelper.GetForm<int>("QualifiedRate");
        protected int companyID = RequestHelper.GetForm<int>("CompanyID");
        protected List<KPIEvaluateReportInfo> reportList = new List<KPIEvaluateReportInfo>();
        protected string companyIDString = string.Empty;

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "综合能力评估";
            base.CheckUserPower("ReadTPRReport", PowerCheckType.Single);

            if (qualifiedRate < 0) qualifiedRate = 0;
            companyIDString = base.UserCompanyID.ToString();
            if (companyID <= 0)
                companyIDString = base.SonCompanyID;
            else if (companyID > 0)
                companyIDString = companyID.ToString();

            if (!base.ExistsSonCompany) companyID = base.UserCompanyID;

            //三者不能同时为空
            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(postId) || !string.IsNullOrEmpty(postName) || evaluateNameId > 0)
            {
                KPIEvaluateSearchInfo kpiEvaluateSearch = new KPIEvaluateSearchInfo();
                if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(companyIDString))
                {
                    kpiEvaluateSearch.Condition = "[UserID] in (select id from [_User] where Del=0";
                    if (!string.IsNullOrEmpty(name))
                        kpiEvaluateSearch.Condition += " And [RealName]='" + name + "'";
                    if (!string.IsNullOrEmpty(companyIDString))
                        kpiEvaluateSearch.Condition += " And [CompanyId] in (" + companyIDString + ")";
                    kpiEvaluateSearch.Condition += ")";
                }
                if (!string.IsNullOrEmpty(postId))
                    kpiEvaluateSearch.PostId = postId;
                else
                {
                    //string workingPostIdStr = string.Empty;
                    //WorkingPostSearchInfo workingPostSearch = new WorkingPostSearchInfo();
                    //workingPostSearch.CompanyId = companyIDString;
                    //workingPostSearch.PostName = postName;
                    //workingPostSearch.IsPost = 1;
                    //foreach (WorkingPostInfo info in WorkingPostBLL.SearchWorkingPostList(workingPostSearch))
                    //{
                    //    if (string.IsNullOrEmpty(workingPostIdStr))
                    //        workingPostIdStr = info.ID.ToString();
                    //    else
                    //        workingPostIdStr += "," + info.ID.ToString();
                    //}
                    //kpiEvaluateSearch.PostId = workingPostIdStr;
                    if (!string.IsNullOrEmpty(kpiEvaluateSearch.Condition))
                        kpiEvaluateSearch.Condition += " And ";
                    kpiEvaluateSearch.Condition += "[PostId] in (Select [Id] from [_WorkingPost] where [IsPost]=1 and [State]=0";
                    if (!string.IsNullOrEmpty(postName))
                        kpiEvaluateSearch.Condition += " And [PostName]='" + postName + "'";
                    if (!string.IsNullOrEmpty(companyIDString))
                        kpiEvaluateSearch.Condition += " And [CompanyId] in (" + companyIDString + ")";
                    kpiEvaluateSearch.Condition += ")";
                }
                kpiEvaluateSearch.EvaluateNameId = evaluateNameId;
                foreach (KPIEvaluateReportInfo info in KPIEvaluateBLL.KPIEvaluateReportList(kpiEvaluateSearch))
                {
                    if (info.Rate >= qualifiedRate)
                    {
                        info.CompanyName = CompanyBLL.ReadCompany(UserBLL.ReadUser(info.UserId).CompanyID).CompanySimpleName;
                        reportList.Add(info);
                    }
                }
            }
        }

        protected string GetDropDownListContent()
        {
            //装载岗位下拉菜单
            WorkingPostSearchInfo workingPostSearch = new WorkingPostSearchInfo();
            workingPostSearch.CompanyId = companyID.ToString();
            workingPostSearch.IsPost = 1;
            StringBuilder DropDownListHtml = new StringBuilder();
            DropDownListHtml.AppendLine("<option value=\"\">请选择岗位</option>");
            foreach (WorkingPostInfo info in WorkingPostBLL.SearchWorkingPostList(workingPostSearch))
            {
                if (info.ID.ToString() == postId)
                    DropDownListHtml.Append("<option value=\"" + info.ID + "\" selected>" + info.PostName + "</option>");
                else
                    DropDownListHtml.Append("<option value=\"" + info.ID + "\">" + info.PostName + "</option>");
            }
            return DropDownListHtml.ToString();
        }
    }
}
