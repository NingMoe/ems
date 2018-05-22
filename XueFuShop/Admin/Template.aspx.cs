using System;
using System.Data;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class Template : AdminBasePage
    {
        protected List<TemplatePluginsInfo> templatePluginsList = new List<TemplatePluginsInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("Template", PowerCheckType.Single);
                string queryString = RequestHelper.GetQueryString<string>("Action");
                string str2 = RequestHelper.GetQueryString<string>("Path");
                if ((queryString == "Active") && (str2 != string.Empty))
                {
                    ShopConfigInfo config = ShopConfig.ReadConfigInfo();
                    config.TemplatePath = str2;
                    ShopConfig.UpdateConfigInfo(config);
                    ScriptHelper.Alert("∆Ù”√≥…π¶", "Template.aspx");
                }
                this.templatePluginsList = TemplatePlugins.ReadTemplatePluginsList();
            }
        }
    }
}
