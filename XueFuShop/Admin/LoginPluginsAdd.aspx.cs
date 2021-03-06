using System;
using System.Data;
using System.Collections.Generic;
using XueFuShop.Models;
using XueFuShop.Pages;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.BLL;


namespace XueFuShop.Admin
{
    public partial class LoginPluginsAdd : AdminBasePage
    {
        protected LoginPluginsInfo loginPlugins = new LoginPluginsInfo();
        protected Dictionary<string, string> nameDic = new Dictionary<string, string>();
        protected Dictionary<string, string> selectValueDic = new Dictionary<string, string>();
        protected Dictionary<string, string> valueDic = new Dictionary<string, string>();

        
        protected void HanlerCanChangLoginPlugins(string key)
        {
            Dictionary<string, string> configDic = new Dictionary<string, string>();
            string form = RequestHelper.GetForm<string>("ConfigNameList");
            foreach (string str2 in form.Split(new char[] { '|' }))
            {
                if (str2 != string.Empty)
                {
                    configDic.Add(str2, RequestHelper.GetForm<string>(str2));
                }
            }
            configDic.Add("Description", this.Description.Text);
            configDic.Add("IsEnabled", this.IsEnabled.Text);
            XueFuShop.Common.LoginPlugins.UpdateLoginPlugins(key, configDic);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                string queryString = RequestHelper.GetQueryString<string>("Key");
                if (queryString != string.Empty)
                {
                    base.CheckAdminPower("ReadLoginPlugins", PowerCheckType.Single);
                    this.loginPlugins = XueFuShop.Common.LoginPlugins.ReadLoginPlugins(queryString);
                    this.Description.Text = this.loginPlugins.Description;
                    this.IsEnabled.Text = this.loginPlugins.IsEnabled.ToString();
                    XueFuShop.Common.LoginPlugins.ReadCanChangeLoginPlugins(queryString, ref this.nameDic, ref this.valueDic, ref this.selectValueDic);
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string queryString = RequestHelper.GetQueryString<string>("Key");
            string alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            base.CheckAdminPower("UpdateLoginPlugins", PowerCheckType.Single);
            this.HanlerCanChangLoginPlugins(queryString);
            AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("LoginPlugins"));
            AdminBasePage.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}
