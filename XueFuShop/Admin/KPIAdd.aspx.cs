using System;
using XueFuShop.Pages;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;
using System.Web.UI.WebControls;

namespace XueFuShop.Admin
{
    public partial class KPIAdd : AdminBasePage
    {
        protected int CompanyID = RequestHelper.GetQueryString<int>("CompanyId");
        protected int ParentID = RequestHelper.GetQueryString<int>("ParentID");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (CompanyID >= 0)
                    CompanyName.Value = CompanyBLL.ReadCompany(CompanyID).CompanyName;

                KPISearchInfo kpi = new KPISearchInfo();
                kpi.ParentId = "0";

                this.FatherID.DataSource = KPIBLL.SearchKPIList(kpi);
                this.FatherID.DataTextField = "Name";
                this.FatherID.DataValueField = "ID";
                this.FatherID.DataBind();
                this.FatherID.Items.Insert(0, new ListItem("请选择分类", ""));
                FatherID.SelectedValue = ParentID.ToString();

                Type.DataSource = EnumHelper.ReadEnumList<KPIType>();
                Type.DataTextField = "ChineseName";
                Type.DataValueField = "Value";
                Type.DataBind();

                int queryString = RequestHelper.GetQueryString<int>("ID");

                if (queryString != int.MinValue)
                {
                    base.CheckAdminPower("ReadKPI", PowerCheckType.Single);
                    KPIInfo info = KPIBLL.ReadKPI(queryString);
                    CompanyName.Value = CompanyBLL.ReadCompany(info.CompanyID).CompanyName;
                    CompanyID = info.CompanyID;
                    Type.Text = ((int)info.Type).ToString();
                    FatherID.Text = info.ParentId.ToString();
                    Sort.Text = info.Sort.ToString();
                    ClassName.Text = info.Name;
                    Introduction.Text = info.EvaluateInfo;
                    Score.Text = info.Scorse.ToString();
                    Method.Text = info.Method;
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            KPIInfo kpiCate = new KPIInfo();
            CompanyID = RequestHelper.GetForm<int>("CompanyId");
            if (CompanyID < 0) ScriptHelper.Alert("请重新选择公司");
            kpiCate.CompanyID = CompanyID;
            kpiCate.ID = RequestHelper.GetQueryString<int>("ID");
            if (string.IsNullOrEmpty(FatherID.Text)) ScriptHelper.Alert("请选择分类");
            kpiCate.ParentId = Convert.ToInt32(FatherID.SelectedValue);
            kpiCate.Sort = Convert.ToInt32(Sort.Text);
            kpiCate.Name = ClassName.Text;
            kpiCate.EvaluateInfo = Introduction.Text;// string.Empty;
            kpiCate.Method = Method.Text;
            kpiCate.Type = (KPIType)int.Parse(Type.Text);
            if (!string.IsNullOrEmpty(Score.Text)) kpiCate.Scorse = float.Parse(Score.Text);
            else kpiCate.Scorse = 0;

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (kpiCate.ID == int.MinValue)
            {
                base.CheckAdminPower("AddKPI", PowerCheckType.Single);
                int id = KPIBLL.AddKPI(kpiCate);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("KPI"), id);
                ScriptHelper.Alert(alertMessage, Request.Url.AbsolutePath + "?CompanyId=" + kpiCate.CompanyID.ToString() + "&ParentID=" + kpiCate.ParentId.ToString());
            }
            else
            {
                base.CheckAdminPower("UpdateKPI", PowerCheckType.Single);
                KPIBLL.UpdateKPI(kpiCate);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("KPI"), kpiCate.ID);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
                ScriptHelper.Alert(alertMessage, base.Server.UrlDecode(RequestHelper.GetQueryString<string>("ReturnUrl")));
            }
        }
    }
}
