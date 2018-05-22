using System;
using System.Data;
using XueFuShop.Pages;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;

namespace XueFuShop.Admin
{
    public partial class EmailSendRecordDetail : AdminBasePage
    {
        protected EmailSendRecordInfo emailSendRecord = new EmailSendRecordInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                int queryString = RequestHelper.GetQueryString<int>("ID");
                if (queryString != -2147483648)
                {
                    base.CheckAdminPower("ReadEmailSendRecord", PowerCheckType.Single);
                    this.emailSendRecord = EmailSendRecordBLL.ReadEmailSendRecord(queryString);
                }
            }
        }
    }
}
