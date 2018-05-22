using System;
using XueFuShop.Pages;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;
using System.Text;
using System.Web.UI.WebControls;

namespace XueFuShop.Admin
{
    public partial class KPI : AdminBasePage
    {
        protected int companyId = RequestHelper.GetQueryString<int>("CompanyId");
        protected string kpiName = RequestHelper.GetQueryString<string>("Name");
        protected int parentId = RequestHelper.GetQueryString<int>("ParentId");
        protected string _currentCompanyName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                string Action = RequestHelper.GetQueryString<string>("Action");
                if (Action == "Delete")
                {
                    int id = RequestHelper.GetQueryString<int>("ID");
                    if (id != -2147483648)
                    {
                        base.CheckAdminPower("DeleteKPI", PowerCheckType.Single);
                        KPIBLL.DeleteKPI(id.ToString());
                        AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("KPI"), id);
                    }
                }

                if (companyId > 0)
                {
                    CompanyInfo company = CompanyBLL.ReadCompany(companyId);
                    _currentCompanyName = company.CompanySimpleName;
                    CompanyName.Value = company.CompanyName;
                }

                base.CheckAdminPower("ReadKPI", PowerCheckType.Single);
                KPISearchInfo kpi = new KPISearchInfo();
                kpi.ParentId = "0";
                KPICate.DataSource = KPIBLL.SearchKPIList(kpi);
                KPICate.DataTextField = "Name";
                KPICate.DataValueField = "ID";
                KPICate.DataBind();
                KPICate.Items.Insert(0, new ListItem("请选择分类", ""));

                if (parentId > 0)
                {
                    kpi.ParentId = parentId.ToString();
                    KPICate.SelectedValue = parentId.ToString();
                }
                else kpi.ParentId = string.Empty;
                kpi.Name = kpiName;
                if (companyId > 0)
                    kpi.CompanyID = companyId.ToString();
                base.BindControl(KPIBLL.SearchKPIList(kpi, base.CurrentPage, base.PageSize, ref this.Count), this.RecordList, this.MyPager);
                
                Name.Text = kpiName;
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            companyId = RequestHelper.GetForm<int>("CompanyID");
            if (string.IsNullOrEmpty(CompanyName.Value) && companyId > 0)
                ScriptHelper.Alert("请选择完整的公司名称！");
            ResponseHelper.Redirect("KPI.aspx?Action=search&CompanyId=" + companyId.ToString() + "&Name=" + Name.Text + "&ParentId=" + KPICate.SelectedValue);
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("DeleteKPI", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                KPIBLL.DeleteKPI(intsForm);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("KPI"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        public string GetSearchTitle()
        {
            if (RequestHelper.GetQueryString<string>("Action") == "search")
            {
                StringBuilder HtmlOut = new StringBuilder();
                if (companyId > 0)
                {
                    HtmlOut.Append(_currentCompanyName);
                }
                if (!string.IsNullOrEmpty(kpiName))
                {
                    HtmlOut.Append(" [指标：" + kpiName + "] ");
                }
                if (parentId > 0)
                {
                    HtmlOut.Append(" [分类：" + KPIBLL.ReadKPI(parentId).Name + "] ");
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

        protected string getCompanyName(int companyId)
        {
            if (companyId == 0)
                return "系统指标";
            else if (companyId == this.companyId)
                return _currentCompanyName;
            else
                return CompanyBLL.ReadCompany(companyId).CompanySimpleName;
        }
    }
}
