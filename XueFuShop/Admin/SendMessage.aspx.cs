using System;
using XueFuShop.Pages;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Admin
{
    public partial class SendMessage : AdminBasePage
    {
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("DeleteSendMessage", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                SendMessageBLL.DeleteSendMessage(intsForm, 0);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("SendMessage"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("ReadSendMessage", PowerCheckType.Single);
                SendMessageSearchInfo sendMessage = new SendMessageSearchInfo();
                sendMessage.IsAdmin = 1;
                base.BindControl(SendMessageBLL.SearchSendMessageList(base.CurrentPage, base.PageSize, sendMessage, ref this.Count), this.RecordList, this.MyPager);
            }
        }
    }
}
