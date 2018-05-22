using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class WorkingPostAdd : UserManageBasePage
    {
        protected int CompanyID = RequestHelper.GetQueryString<int>("CompanyID");
        protected int WorkPostID = RequestHelper.GetQueryString<int>("ID");
        protected List<WorkingPostInfo> WorkingPostClassList = new List<WorkingPostInfo>();
        protected WorkingPostInfo WorkingPost = new WorkingPostInfo();
        protected string KPIContent = string.Empty;
        protected int IsPost = 0;
        protected List<KPIInfo> TempList1 = new List<KPIInfo>();
        protected List<KPIInfo> TempList2 = new List<KPIInfo>();
        protected List<KPIInfo> TempList3 = new List<KPIInfo>();


        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "岗位列表";
            base.CheckUserPower("ReadWorkingPost,AddWorkingPost,UpdateWorkingPost", PowerCheckType.OR);

            string allCompanyID = CompanyBLL.SystemCompanyId.ToString();
            if (!string.IsNullOrEmpty(base.ParentCompanyID))
                allCompanyID += "," + base.ParentCompanyID;
            //if (!string.IsNullOrEmpty(base.SonCompanyID))
            //    allCompanyID += "," + base.SonCompanyID;

            if (CompanyID < 0) CompanyID = base.UserCompanyID;
            if (WorkPostID > 0)
            {
                WorkingPost = WorkingPostBLL.ReadWorkingPost(WorkPostID);
                CompanyID = WorkingPost.CompanyId;
                KPIContent = KPITempletBLL.ReadKPITemplet(WorkingPost.KPITempletId).KPIContent;
                if (CompanyID != base.UserCompanyID)
                {
                    string parentCompanyID = CompanyBLL.ReadParentCompanyId(CompanyID);
                    string sonCompanyID = CompanyBLL.ReadCompanyIdList(CompanyID.ToString());
                    allCompanyID = CompanyBLL.SystemCompanyId.ToString();
                    if (!string.IsNullOrEmpty(parentCompanyID))
                        allCompanyID += "," + parentCompanyID;
                    //if (!string.IsNullOrEmpty(sonCompanyID))
                    //    allCompanyID += "," + sonCompanyID;
                }
            }

            WorkingPostSearchInfo workingPostSearch = new WorkingPostSearchInfo();
            workingPostSearch.CompanyId = CompanyID.ToString();
            workingPostSearch.ParentId = "0";
            workingPostSearch.IsPost = 0;
            WorkingPostClassList = WorkingPostBLL.SearchWorkingPostList(workingPostSearch);

            KPISearchInfo kpiSearch = new KPISearchInfo();
            kpiSearch.CompanyID = allCompanyID;
            kpiSearch.ParentId = "1,2,3";
            List<KPIInfo> kpiList = KPIBLL.SearchKPIList(kpiSearch);
            foreach (KPIInfo info in kpiList)
            {
                switch (info.ParentId)
                {
                    case 1:
                        TempList1.Add(info);
                        break;
                    case 2:
                        TempList2.Add(info);
                        break;
                    case 3:
                        TempList3.Add(info);
                        break;
                }
            }
        }

        protected override void PostBack()
        {
            IsPost = RequestHelper.GetForm<int>("IsPostCheck");
            WorkingPost.CompanyId = RequestHelper.GetForm<int>("CompanyID");
            if (WorkingPost.CompanyId < 0) WorkingPost.CompanyId = base.UserCompanyID;
            WorkingPost.ParentId = RequestHelper.GetForm<int>("ParentId");
            WorkingPost.PostName = StringHelper.AddSafe(RequestHelper.GetForm<string>("PostName"));
            WorkingPost.ID = WorkPostID;
            WorkingPost.IsPost = IsPost;

            if (WorkingPost.IsPost == 1)
            {
                KPIContent = RequestHelper.GetForm<string>("kpiidstr");
                WorkingPost.KPITempletId = KPITempletBLL.ExistsKPITemplet(0, KPIContent);
                if (WorkingPost.KPITempletId == 0)
                {
                    KPITempletInfo kpiTemplet = new KPITempletInfo();
                    kpiTemplet.KPIContent = KPIContent;
                    kpiTemplet.CompanyId = WorkingPost.CompanyId;
                    WorkingPost.KPITempletId = KPITempletBLL.AddKPITemplet(kpiTemplet);
                }
            }
            else
                WorkingPost.IsPost = 0;

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (WorkPostID == int.MinValue)
            {
                base.CheckUserPower("AddWorkingPost", PowerCheckType.Single);
                int id = WorkingPostBLL.AddWorkingPost(WorkingPost);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("WorkingPost"), id);
            }
            else
            {
                base.CheckUserPower("UpdateWorkingPost", PowerCheckType.Single);
                WorkingPostBLL.UpdateWorkingPost(WorkingPost);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("WorkingPost"), WorkPostID);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            string returnURL = ServerHelper.UrlDecode(RequestHelper.GetQueryString<string>("ReturnURL"));
            if (string.IsNullOrEmpty(returnURL))
                ScriptHelper.Alert(alertMessage, "WorkingPostAdd.aspx?CompanyID=" + WorkingPost.CompanyId.ToString());
            else
                ScriptHelper.Alert(alertMessage, returnURL);
        }

        protected string GetTrHtml(List<KPIInfo> kpiList)
        {
            StringBuilder trHtml = new StringBuilder();
            int i = 1;
            foreach (KPIInfo info in kpiList)
            {
                trHtml.AppendLine("<tr data-type=\"" + info.ParentId + "\">");
                if (i == 1)
                    trHtml.AppendLine("	<td rowspan=\"" + kpiList.Count + "\" class=\"indicator_name\">" + KPIBLL.ReadKPI(info.ParentId).Name + "</td>");
                trHtml.AppendLine("	<td class=\"choose\">" + info.Method + "</td>");
                trHtml.AppendLine("	<td class=\"evaluation_content choose\" data-id=\"" + info.ID.ToString() + "\">" + i.ToString() + "." + info.Name + "</td>");
                //trHtml.AppendLine("	<td class=\"choose\">" + ((info.CompanyID == 0) ? "系统指标" : CompanyBLL.ReadCompany(info.CompanyID).CompanySimpleName) + "</td>");
                trHtml.AppendLine("	<td class=\"schedule\"></td>");
                trHtml.AppendLine("</tr>");
                i++;
            }
            return trHtml.ToString();
        }

        protected string GetPostHtml()
        {
            StringBuilder postHtml = new StringBuilder();
            WorkingPostSearchInfo workingPostSearch = new WorkingPostSearchInfo();
            workingPostSearch.CompanyId = CompanyBLL.SystemCompanyId.ToString();
            //workingPostSearch.State = int.MinValue;
            //workingPostSearch.IsPost = 1;
            int i = 1;
            foreach (WorkingPostViewInfo info in WorkingPostBLL.SearchWorkingPostViewList(workingPostSearch))
            {
                if (i % 4 == 1)
                    postHtml.AppendLine("<tr>");
                postHtml.AppendLine("<td class=\"post_name\" data-kpi=\"" + info.KPIContent + "\">" + info.PostName + "</td>");
                if (i % 4 == 0)
                    postHtml.AppendLine("</tr>");
                i++;
            }
            if (i % 4 > 0)
                postHtml.AppendLine("</tr>");
            return postHtml.ToString();
        }
    }
}
