using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Common;
using XueFu.EntLib;

namespace XueFuShop.Pages
{
    public class Logout : CommonBasePage
    {
        
        protected override void PageLoad()
        {
            base.PageLoad();
            CookiesHelper.DeleteCookie(ShopConfig.ReadConfigInfo().UserCookies);
            CookiesHelper.DeleteCookie("SMSIsChecked");
            CookiesHelper.DeleteCookie("SMSCheckCode");
            CookiesHelper.DeleteCookie("UserPhoto");
            CookiesHelper.DeleteCookie("PublicCourseView");
            ResponseHelper.Redirect("/");
        }
    }

 

}
