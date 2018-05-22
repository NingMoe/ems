using System;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class ShopInfo : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckAdminPower("Product", PowerCheckType.Single);
        }
    }
}
