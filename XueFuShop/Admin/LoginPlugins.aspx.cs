using System;
using System.Data;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class LoginPlugins : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("ReadLoginPlugins", PowerCheckType.Single);
                base.BindControl(XueFuShop.Common.LoginPlugins.ReadLoginPluginsList(), this.RecordList);
            }
        }
    }
}
