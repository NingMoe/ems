using System;
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.Models;
using XueFuShop.Common;

namespace XueFuShop.Admin
{
    public partial class Sitemap : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("Sitemap", PowerCheckType.Single);
                this.Domain.Text = ShopConfig.ReadConfigInfo().Domain;
                this.Frequency.Text = ShopConfig.ReadConfigInfo().Frequency;
                this.Priority.Text = ShopConfig.ReadConfigInfo().Priority;
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("Sitemap", PowerCheckType.Single);
            ShopConfigInfo config = ShopConfig.ReadConfigInfo();
            config.Domain = this.Domain.Text;
            config.Frequency = this.Frequency.Text;
            config.Priority = this.Priority.Text;
            ShopConfig.UpdateConfigInfo(config);
            ScriptHelper.Alert(ShopLanguage.ReadLanguage("UpdateOK"), RequestHelper.RawUrl);
        }
    }
}
