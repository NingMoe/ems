using System;
using System.Data;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class ActivityPluginsPage : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("ReadActivityPlugins", PowerCheckType.Single);
                base.BindControl(XueFuShop.Common.ActivityPlugins.ReadActivityPluginsList(), this.RecordList);
            }
        }
    }
}
