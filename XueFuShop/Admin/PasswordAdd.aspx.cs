using System;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class PasswordAdd : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckAdminPower("UpdateAdmin", PowerCheckType.Single);
            int queryString = RequestHelper.GetQueryString<int>("ID");
            if (queryString != -2147483648)
            {
                this.Name.Text = AdminBLL.ReadAdmin(queryString).Name;
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs E)
        {
            int queryString = RequestHelper.GetQueryString<int>("ID");
            if (queryString != -2147483648)
            {
                string newPassword = StringHelper.Password(this.NewPassword.Text, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                AdminBLL.ChangePassword(queryString, newPassword);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("ChangeAdminPassword"), queryString);
                AdminBasePage.Alert(ShopLanguage.ReadLanguage("UpdateOK"), RequestHelper.RawUrl);
            }
        }
    }
}
