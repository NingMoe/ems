using System;
using XueFuShop.Pages;
using XueFu.EntLib;
using XueFuShop.Models;
using System.Collections.Generic;
using XueFuShop.BLL;
using XueFuShop.Common;
using System.Text;
using System.Web.UI.WebControls;

namespace XueFuShop.Admin
{
    public partial class WorkingPostAdd : AdminBasePage
    {
        protected int _companyId = RequestHelper.GetQueryString<int>("CompanyId");
        protected int workPostId = RequestHelper.GetQueryString<int>("ID");
        protected int isPost = 0;
        protected string kpiContent = string.Empty;
        protected WorkingPostInfo workingPost = new WorkingPostInfo();
        protected KPITempletInfo kpiTemplet = new KPITempletInfo();
        protected CompanyInfo Company = new CompanyInfo();

        protected List<KPIInfo> tempList1 = new List<KPIInfo>();
        protected List<KPIInfo> tempList2 = new List<KPIInfo>();
        protected List<KPIInfo> tempList3 = new List<KPIInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (workPostId > 0)
                {
                    workingPost = WorkingPostBLL.ReadWorkingPost(workPostId);
                    //kpiTemplet = KPITempletBLL.ReadKPITemplet(workingPost.KPITempletId);
                    PostName.Text = workingPost.PostName;

                    _companyId = workingPost.CompanyId;

                    //KPISearchInfo kpiSearch = new KPISearchInfo();
                    //string parentCompanyId = CompanyBLL.ReadParentCompanyId(_companyId);
                    //if (string.IsNullOrEmpty(parentCompanyId))
                    //    kpiSearch.CompanyID = _companyId.ToString();
                    //else
                    //    kpiSearch.CompanyID = _companyId.ToString() + "," + parentCompanyId;
                    //kpiSearch.ParentId = "1,2,3";
                    //List<KPIInfo> kpiList = KPIBLL.SearchKPIList(kpiSearch);
                    //foreach (KPIInfo info in kpiList)
                    //{
                    //    switch (info.ParentId)
                    //    {
                    //        case 1:
                    //            tempList1.Add(info);
                    //            break;
                    //        case 2:
                    //            tempList2.Add(info);
                    //            break;
                    //        case 3:
                    //            tempList3.Add(info);
                    //            break;
                    //    }
                    //}
                }

                Company = CompanyBLL.ReadCompany(_companyId);
                CompanyName.Value = Company.CompanyName;
                WorkingPostSearchInfo workingPostSearch = new WorkingPostSearchInfo();
                workingPostSearch.CompanyId = _companyId.ToString();
                workingPostSearch.ParentId = "0";
                workingPostSearch.IsPost = 0;
                this.ParentId.DataSource = WorkingPostBLL.CreateWorkingPostTreeList(_companyId);
                this.ParentId.DataTextField = "PostName";
                this.ParentId.DataValueField = "Id";
                this.ParentId.DataBind();
                this.ParentId.Items.Insert(0, new ListItem("作为一级部门", "0"));
                if (ParentId.Items.Contains(ParentId.Items.FindByValue(workingPost.ParentId.ToString()))) ParentId.Items.FindByValue(workingPost.ParentId.ToString()).Selected = true;
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            isPost = RequestHelper.GetForm<int>("IsPostCheck");
            workingPost.CompanyId = RequestHelper.GetForm<int>("CompanyId");
            workingPost.ParentId = RequestHelper.GetForm<int>(ShopConfig.ReadConfigInfo().NamePrefix + "ParentId");//ParentId.SelectedValue
            workingPost.PostName = PostName.Text;
            workingPost.ID = workPostId;
            workingPost.IsPost = isPost;

            //if (workingPost.IsPost == 1)
            //{
            //    kpiContent = RequestHelper.GetForm<string>("kpiidstr");
            //    workingPost.KPITempletId = KPITempletBLL.ExistsKPITemplet(0, kpiContent);
            //    if (workingPost.KPITempletId == 0)
            //    {
            //        kpiTemplet.KPIContent = kpiContent;
            //        kpiTemplet.CompanyId = workingPost.CompanyId;
            //        workingPost.KPITempletId = KPITempletBLL.AddKPITemplet(kpiTemplet);
            //    }
            //}
            //else
            //    workingPost.IsPost = 0;
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (workPostId == int.MinValue)
            {
                base.CheckAdminPower("AddWorkingPost", PowerCheckType.Single);
                int id = WorkingPostBLL.AddWorkingPost(workingPost);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("WorkingPost"), id);
            }
            else
            {
                base.CheckAdminPower("UpdateWorkingPost", PowerCheckType.Single);
                WorkingPostBLL.UpdateWorkingPost(workingPost);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("WorkingPost"), workPostId);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            string returnURL = ServerHelper.UrlDecode(RequestHelper.GetQueryString<string>("ReturnURL"));
            if (string.IsNullOrEmpty(returnURL))
                ScriptHelper.Alert(alertMessage, Request.Url.AbsolutePath + "?CompanyId=" + workingPost.CompanyId.ToString());
            else
                ScriptHelper.Alert(alertMessage, returnURL);
        }

        protected string GetTrHtml(List<KPIInfo> kpiList)
        {
            StringBuilder trHtml = new StringBuilder();
            int i = 1;
            foreach (KPIInfo info in kpiList)
            {
                trHtml.AppendLine("<tr data-type=\"" + info.ParentId + "\" class=\"listTableMain\" onmousemove=\"changeColor(this,'#FFFDD7')\" onmouseout=\"changeColor(this,'#FFF')\">");
                if (i == 1)
                    trHtml.AppendLine("	<td rowspan=\"" + kpiList.Count + "\" class=\"indicator_name\">" + KPIBLL.ReadKPI(info.ParentId).Name + "</td>");
                trHtml.AppendLine("	<td class=\"choose\">" + info.Method + "</td>");
                trHtml.AppendLine("	<td class=\"evaluation_content choose\" data-id=\"" + info.ID.ToString() + "\">" + i.ToString() + "." + info.Name + "</td>");
                trHtml.Append("	<td class=\"choose\">");
                if (info.CompanyID == 0)
                    trHtml.Append("系统指标");
                else if (info.CompanyID == _companyId)
                    trHtml.Append(Company.CompanySimpleName);
                else
                    trHtml.Append(CompanyBLL.ReadCompany(info.CompanyID).CompanySimpleName);
                trHtml.Append("</td>");
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
            workingPostSearch.CompanyId = "0";
            //workingPostSearch.State = int.MinValue;
            //workingPostSearch.IsPost = 1;
            foreach (WorkingPostViewInfo info in WorkingPostBLL.SearchWorkingPostViewList(workingPostSearch))
            {
                postHtml.AppendLine("<li data-kpi=\"" + info.KPIContent + "\">" + info.PostName + "</li>");

            }
            return postHtml.ToString();
        }
    }
}
