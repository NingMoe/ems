using System;
using System.Data;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFu.EntLib;

namespace MobileEMS
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("LogoutSystem"));
            CookiesHelper.DeleteCookie(ShopConfig.ReadConfigInfo().UserCookies);
            CookiesHelper.DeleteCookie("SMSIsChecked");
            CookiesHelper.DeleteCookie("SMSCheckCode");
            ResponseHelper.Redirect("Login.aspx");
        }
    }
}
