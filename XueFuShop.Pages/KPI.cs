using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class KPI : UserManageBasePage
    {
        protected int CompanyID = RequestHelper.GetQueryString<int>("CompanyID");
        protected int ClassID = RequestHelper.GetQueryString<int>("ClassID");
        protected string Name = RequestHelper.GetQueryString<string>("Name");
        protected List<KPIInfo> KPIClassList = new List<KPIInfo>();
        protected List<KPIInfo> KPIList = new List<KPIInfo>();
        protected CompanyInfo Company = new CompanyInfo();

        protected override void PageLoad()
        {
            //string allCompanyID = CompanyBLL.SystemCompanyId + "," + base.ParentCompanyID + "," + base.SonCompanyID;
            base.PageLoad();
            base.Title = "KPI指标列表";

            string action = RequestHelper.GetQueryString<string>("Action");

            if (action == "Delete")
            {
                string selectID = RequestHelper.GetQueryString<string>("SelectID");
                if (!string.IsNullOrEmpty(selectID))
                {
                    base.CheckUserPower("DeleteKPI", PowerCheckType.Single);
                    KPIBLL.DeleteKPI(selectID);
                    AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("KPI"), selectID);
                    ResponseHelper.Redirect(Request.UrlReferrer.ToString());
                }
            }

            base.CheckUserPower("ReadKPI", PowerCheckType.Single);
            Company = CompanyBLL.ReadCompany(CompanyID);

            KPISearchInfo kpiSearch = new KPISearchInfo();
            kpiSearch.ParentId = "0";
            KPIClassList = KPIBLL.SearchKPIList(kpiSearch);
            if (ClassID > 0) kpiSearch.ParentId = ClassID.ToString();
            else kpiSearch.ParentId = string.Empty;
            kpiSearch.Name = Name;
            if (CompanyID < 0)
            {
                kpiSearch.CompanyID = CompanyBLL.SystemCompanyId.ToString();
                if (!string.IsNullOrEmpty(base.ParentCompanyID))
                    kpiSearch.CompanyID += "," + base.ParentCompanyID;
                if (!string.IsNullOrEmpty(base.SonCompanyID))
                    kpiSearch.CompanyID += "," + base.SonCompanyID;
            }
            else
            {
                kpiSearch.CompanyID = CompanyID.ToString();
            }
            KPIList = KPIBLL.SearchKPIList(kpiSearch, base.CurrentPage, base.PageSize, ref this.Count);
            base.BindPageControl(ref base.CommonPager);
        }

        public string GetSearchTitle()
        {
            if (RequestHelper.GetQueryString<string>("Action") == "search")
            {
                StringBuilder HtmlOut = new StringBuilder();
                if (CompanyID >= 0)
                {
                    if (CompanyID == CompanyBLL.SystemCompanyId)
                        HtmlOut.Append("系统指标");
                    else
                        HtmlOut.Append(CompanyBLL.ReadCompany(CompanyID).CompanyName);
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    HtmlOut.Append(" [指标：" + Name + "] ");
                }
                if (ClassID > 0)
                {
                    HtmlOut.Append(" [分类：" + KPIBLL.ReadKPI(ClassID).Name + "] ");
                }
                if (!string.IsNullOrEmpty(HtmlOut.ToString()))
                {
                    HtmlOut.Insert(0, "<tr><td colspan=\"7\">");
                    HtmlOut.Append("</td></tr>");
                }
                return HtmlOut.ToString();
            }
            return string.Empty;
        }

        protected string getCompanyName(int companyID)
        {
            if (companyID == CompanyBLL.SystemCompanyId)
                return "系统指标";
            else if (companyID != CompanyID)
                return CompanyBLL.ReadCompany(companyID).CompanySimpleName;
            else
                return Company.CompanySimpleName;
        }
    }
}
