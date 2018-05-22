using System;
using System.Data;
using XueFuShop.BLL;

using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class GiftPack : AdminBasePage
    {
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("DeleteGiftPack", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                GiftPackBLL.DeleteGiftPack(intsForm);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("GiftPack"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("ReadGiftPack", PowerCheckType.Single);
                base.BindControl(GiftPackBLL.ReadGiftPackList(base.CurrentPage, base.PageSize, ref this.Count), this.RecordList, this.MyPager);
            }
        }
    }
}
