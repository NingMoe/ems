using System;
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.Common;
using XueFuShop.BLL;
using XueFuShop.Models;


namespace XueFuShop.Admin
{
    public partial class EmailContentAdd : AdminBasePage
    {
        public static string GetEmailContentAddUpdate()
        {
            string str = "Ìí¼Ó";
            if (RequestHelper.GetQueryString<string>("Key") != string.Empty)
            {
                str = "ÐÞ¸Ä";
            }
            return str;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                string queryString = RequestHelper.GetQueryString<string>("Key");
                int num = RequestHelper.GetQueryString<int>("IsSystem");
                if (queryString != string.Empty)
                {
                    base.CheckAdminPower("ReadEmailContent", PowerCheckType.Single);
                    EmailContentInfo info = new EmailContentInfo();
                    if (num == 0)
                    {
                        info = EmailContentHelper.ReadCommonEmailContent(queryString);
                    }
                    else
                    {
                        info = EmailContentHelper.ReadSystemEmailContent(queryString);
                    }
                    this.EmailTitle.Text = info.EmailTitle;
                    this.EmailContent.Value = info.EmailContent;
                    if (info.Note != string.Empty)
                    {
                        this.NotePlaceHolder.Visible = true;
                        this.Note.Text = info.Note;
                    }
                    else
                    {
                        this.NotePlaceHolder.Visible = false;
                    }
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            EmailContentInfo emailContent = new EmailContentInfo();
            emailContent.Key = RequestHelper.GetQueryString<string>("Key");
            emailContent.IsSystem = RequestHelper.GetQueryString<int>("IsSystem");
            emailContent.EmailTitle = this.EmailTitle.Text;
            emailContent.EmailContent = this.EmailContent.Value;
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (emailContent.Key == string.Empty)
            {
                emailContent.Key = Guid.NewGuid().ToString();
                base.CheckAdminPower("AddEmailContent", PowerCheckType.Single);
                EmailContentHelper.AddEmailContent(emailContent);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("EmailContent"), emailContent.Key);
            }
            else
            {
                base.CheckAdminPower("UpdateEmailContent", PowerCheckType.Single);
                EmailContentHelper.UpdateEmailContent(emailContent);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("EmailContent"), emailContent.Key);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            AdminBasePage.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}
