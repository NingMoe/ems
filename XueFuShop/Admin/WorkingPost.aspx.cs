using System;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class WorkingPost : AdminBasePage
    {
        protected string Action = RequestHelper.GetQueryString<string>("Action");
        protected int CompanyID = RequestHelper.GetQueryString<int>("CompanyID");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Action == "Delete")
            {
                int id = RequestHelper.GetQueryString<int>("ID");
                if (id > 0)
                {
                    base.CheckAdminPower("DeleteWorkingPost", PowerCheckType.Single);
                    CompanyID = WorkingPostBLL.ReadWorkingPost(id).CompanyId;
                    WorkingPostBLL.DeleteWorkingPost(id.ToString());
                    AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("WorkingPost"), id);
                    ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), Request.UrlReferrer.ToString());
                }
            }

            if (CompanyID > 0) CompanyName.Value = CompanyBLL.ReadCompany(CompanyID).CompanyName;
            else
            {
                CompanyID = 0;
                CompanyName.Value = "上海孟特管理咨询有限公司";
            }
            string postName = RequestHelper.GetQueryString<string>("Name");
            this.Name.Text = postName;
            if (!IsPostBack)
            {
                if (Action == "Search")
                {
                    base.CheckAdminPower("ReadWorkingPost", PowerCheckType.Single);
                    WorkingPostSearchInfo workingPostSearch = new WorkingPostSearchInfo();
                    workingPostSearch.CompanyId = CompanyID.ToString();
                    workingPostSearch.Name = postName;
                    base.BindControl(WorkingPostBLL.ReadWorkingPostByCompanyIDWithGroup(CompanyID), this.RecordList);
                }
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            CompanyID = RequestHelper.GetForm<int>("CompanyID");
            if (string.IsNullOrEmpty(CompanyName.Value) || (string.IsNullOrEmpty(CompanyName.Value) && CompanyID > 0))
                ScriptHelper.Alert("请选择完整的公司名称！");
            ResponseHelper.Redirect("WorkingPost.aspx?Action=Search&CompanyID=" + CompanyID.ToString() + "&Name=" + Name.Text);
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("DeleteWorkingPost", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                WorkingPostBLL.DeleteWorkingPost(intsForm);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("KPI"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), Request.UrlReferrer.ToString());
            }
        }

    }
}
