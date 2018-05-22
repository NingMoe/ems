using System;
using System.Data;
using XueFuShop.Pages;

using XueFuShop.Common;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace XueFuShop.Admin
{
    public partial class Gift : AdminBasePage
    {
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("DeleteGift", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                GiftBLL.DeleteGift(intsForm);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Gift"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("ReadGift", PowerCheckType.Single);
                GiftSearchInfo gift = new GiftSearchInfo();
                gift.Name = RequestHelper.GetQueryString<string>("Name");
                base.BindControl(GiftBLL.SearchGiftList(base.CurrentPage, base.PageSize, gift, ref this.Count), this.RecordList, this.MyPager);
            }
        }
    }
}
