using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XueFuShop.Pages;
using XueFuShop.Common;

namespace XueFuShop.Admin
{
    public partial class Pay : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("ReadPay", PowerCheckType.Single);
                base.BindControl(PayPlugins.ReadPayPluginsList(), this.RecordList);
            }
        }
    }
}
