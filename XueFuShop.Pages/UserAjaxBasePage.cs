using System;
using System.Collections.Generic;
using System.Text;

namespace XueFuShop.Pages
{
    public abstract class UserAjaxBasePage : AjaxBasePage
    {
        
        protected UserAjaxBasePage()
        {
        }

        protected override void PageLoad()
        {
            base.PageLoad();
            base.CheckUserLogin();
        }
    }

 

}
