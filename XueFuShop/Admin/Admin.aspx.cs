using System;
using System.Collections.Generic;
using XueFuShop.Common;
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Admin
{
    public partial class Admin : AdminBasePage
    {
        
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("DeleteAdmin", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                AdminBLL.DeleteAdmin(intsForm);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Admin"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("ReadAdmin", PowerCheckType.Single);
                List<AdminInfo> dataSource = AdminBLL.ReadAdminList(base.CurrentPage, base.PageSize, ref this.Count);
                base.BindControl(dataSource, this.RecordList, this.MyPager);
            }
        }
    }
}
