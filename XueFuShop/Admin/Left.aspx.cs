using System;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class Left : AdminBasePage
    {
        protected List<MenuInfo> menuList = new List<MenuInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckAdminPower("ReadMenu", PowerCheckType.Single);
            int queryString = RequestHelper.GetQueryString<int>("ID");
            if (queryString == -2147483648)
            {
                queryString = 1;
            }
            this.menuList = MenuBLL.ReadMenuChildList(queryString);
        }
    }
}
