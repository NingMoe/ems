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
using XueFu.EntLib;
using XueFuShop.BLL;

namespace XueFuShop.Admin
{
    public partial class Logout : Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("LogoutSystem"));
            CookiesHelper.DeleteCookie(ShopConfig.ReadConfigInfo().AdminCookies);
            ResponseHelper.Redirect("Default.aspx");
        }
    }
}
