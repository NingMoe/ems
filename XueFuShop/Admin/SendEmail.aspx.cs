using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFu.EntLib;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class SendEmail : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            EmailSendRecordInfo info;
            info = new EmailSendRecordInfo();
            info.Title = this.Title.Text;
            info.Content = this.Content.Text;
            info.IsSystem = 0;
            info.EmailList = RequestHelper.GetForm<string>("ToUserEmail");
            info.IsStatisticsOpendEmail = 0;
            info.SendStatus = 1;
            info.AddDate = RequestHelper.DateNow;
            info.SendDate = RequestHelper.DateNow;
            info.ID = EmailSendRecordBLL.AddEmailSendRecord(info);
            EmailSendRecordBLL.SendEmail(info);
            AdminBasePage.Alert(ShopLanguage.ReadLanguage("SendEmailOK"), RequestHelper.RawUrl);
        }
    }
}
