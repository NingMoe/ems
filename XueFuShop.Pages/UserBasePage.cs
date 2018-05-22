using System;
using System.Collections.Generic;
using System.Text;

namespace XueFuShop.Pages
{
    public abstract class UserBasePage : CommonBasePage
    {
        
        protected UserBasePage()
        {
        }

        protected override void PageLoad()
        {
            base.PageLoad();
            base.CheckUserLogin();
        }

        protected void ClearCache()
        {
            base.Response.Cache.SetNoServerCaching();
            base.Response.Cache.SetNoStore();
            base.Response.Expires = 0;
        }
    }

 

}
