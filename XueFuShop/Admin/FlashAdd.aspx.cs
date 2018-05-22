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
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.Models;
using XueFuShop.BLL;

using XueFuShop.Common;

namespace XueFuShop.Admin
{
    public partial class FlashAdd : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                int queryString = RequestHelper.GetQueryString<int>("ID");
                if (queryString != -2147483648)
                {
                    base.CheckAdminPower("ReadFlash", PowerCheckType.Single);
                    FlashInfo info = FlashBLL.ReadFlash(queryString);
                    this.Title.Text = info.Title;
                    this.Introduce.Text = info.Introduce;
                    this.Width.Text = info.Width.ToString();
                    this.Height.Text = info.Height.ToString();
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            FlashInfo flash = new FlashInfo();
            flash.ID = RequestHelper.GetQueryString<int>("ID");
            flash.Title = this.Title.Text;
            flash.Introduce = this.Introduce.Text;
            flash.Width = Convert.ToInt32(this.Width.Text);
            flash.Height = Convert.ToInt32(this.Height.Text);
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (flash.ID == -2147483648)
            {
                base.CheckAdminPower("AddFlash", PowerCheckType.Single);
                int id = FlashBLL.AddFlash(flash);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Flash"), id);
            }
            else
            {
                base.CheckAdminPower("UpdateFlash", PowerCheckType.Single);
                FlashBLL.UpdateFlash(flash);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Flash"), flash.ID);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            AdminBasePage.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}
