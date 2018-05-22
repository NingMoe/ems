using System;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class Company : AdminBasePage
    {
        protected int State = RequestHelper.GetQueryString<int>("State");
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = RequestHelper.GetQueryString<int>("ID");
            string Action = RequestHelper.GetQueryString<string>("Action");
            if (Action == "Delete" && id > 0)
            {
                base.CheckAdminPower("DeleteCompany", PowerCheckType.Single);
                CompanyBLL.DeleteCompany(id);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Company"), id);
            }

            if (!IsPostBack)
            {
                base.CheckAdminPower("ReadCompanyList", PowerCheckType.Single);
                CompanyInfo Model = new CompanyInfo();
                Model.State = State;
                if (Action == "search")
                {
                    Model.CompanyName = RequestHelper.GetQueryString<string>("CompanyName");
                    string ParentCompanyName = RequestHelper.GetQueryString<string>("ParentCompanyName");
                    string ParentId = string.Empty;
                    if (ParentCompanyName != string.Empty)
                    {
                        CompanyInfo ParentModel = new CompanyInfo();
                        ParentModel.CompanyName = ParentCompanyName;
                        List<CompanyInfo> ParentCompanyList = CompanyBLL.ReadCompanyList(ParentModel);
                        if (ParentCompanyList != null && ParentCompanyList.Count > 0)
                        {
                            foreach (CompanyInfo Item in ParentCompanyList)
                            {
                                ParentId = ParentId + "," + Item.CompanyId;
                            }
                            if (ParentId.StartsWith(",")) ParentId = ParentId.Substring(1);
                        }
                    }
                    if (!string.IsNullOrEmpty(ParentId)) Model.ParentIdCondition = ParentId;
                }
                base.BindControl(CompanyBLL.ReadCompanyList(Model, base.CurrentPage, base.PageSize, ref this.Count), this.RecordList, this.MyPager);
            }
        }

        protected void UnActiveButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("CompanyState", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                CompanyBLL.UpdateCompany("State", "1", intsForm);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UnActiveRecord"), ShopLanguage.ReadLanguage("Company"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("UnActiveOK"), RequestHelper.RawUrl);
            }

        }

        protected void ActiveButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("CompanyState", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                CompanyBLL.UpdateCompany("State", "0", intsForm);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("ActiveRecord"), ShopLanguage.ReadLanguage("Company"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("ActiveOK"), RequestHelper.RawUrl);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ResponseHelper.Redirect("Company.aspx?Action=search&" + "CompanyName=" + this.CompanyName.Text + "&ParentCompanyName=" + this.ParentCompanyName.Text);
        }

        protected void Button2_ServerClick(object sender, EventArgs e)
        {
            base.CheckAdminPower("DeleteCompany", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (!string.IsNullOrEmpty(intsForm))
            {
                string[] Arr = intsForm.Split(',');
                for (int i = 0; i < Arr.Length; i++)
                {
                    CompanyBLL.DeleteCompany(int.Parse(Arr[i]));
                }
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Company"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }
    }
}
