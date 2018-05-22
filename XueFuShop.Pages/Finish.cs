using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class Finish : CommonBasePage
    {
        
        protected OrderInfo order = new OrderInfo();
        protected PayPluginsInfo payPlugins = new PayPluginsInfo();

        
        protected override void PageLoad()
        {
            base.PageLoad();
            if ((ShopConfig.ReadConfigInfo().AllowAnonymousAddCart == 1) && (base.UserID == 0))
            {
                ResponseHelper.Redirect("/User/Login.aspx");
                ResponseHelper.End();
            }
            int queryString = RequestHelper.GetQueryString<int>("ID");
            this.order = OrderBLL.ReadOrder(queryString, base.UserID);
            this.payPlugins = PayPlugins.ReadPayPlugins(this.order.PayKey);
            base.Title = "¶©µ¥Íê³É";
        }
    }

 

}
