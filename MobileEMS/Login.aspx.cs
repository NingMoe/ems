using System;
using System.Web;
using XueFu.EntLib;
using XueFuShop.Common;

namespace MobileEMS
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserAgentHelper.IsMobile(HttpContext.Current.Request.UserAgent))
                ResponseHelper.Redirect("http://ems.mostool.com");

            if (Cookies.User.GetUserID(true) > 0)
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
}
