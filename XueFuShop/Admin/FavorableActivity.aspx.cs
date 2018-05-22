using System;
using System.Data;
using XueFuShop.BLL;

using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.Common;

namespace XueFuShop.Admin
{
    public partial class FavorableActivity : AdminBasePage
    {
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("DeleteFavorableActivity", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                FavorableActivityBLL.DeleteFavorableActivity(intsForm);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("FavorableActivity"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("ReadFavorableActivity", PowerCheckType.Single);
                base.BindControl(FavorableActivityBLL.ReadFavorableActivityList(base.CurrentPage, base.PageSize, ref this.Count), this.RecordList, this.MyPager);
            }
        }
    }
}
