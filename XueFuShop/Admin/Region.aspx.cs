using System;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class Region : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckAdminPower("ReadRegion", PowerCheckType.Single);
        }
    }
}
