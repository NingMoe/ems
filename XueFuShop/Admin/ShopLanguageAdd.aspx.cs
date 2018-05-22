using System;
using System.Collections.Generic;
using XueFu.EntLib;
using System.Xml;

using XueFuShop.Common;
using XueFuShop.Pages;
using XueFuShop.BLL;

namespace XueFuShop.Admin
{
    public partial class ShopLanguageAdd : AdminBasePage
    {
        protected Dictionary<string, string> language = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("ReadLanguage", PowerCheckType.Single);
                using (XmlHelper helper = new XmlHelper(ServerHelper.MapPath("~/Config/ShopLanguage.config")))
                {
                    foreach (XmlNode node in helper.ReadNode("Language").ChildNodes)
                    {
                        this.language.Add(node.Attributes["key"].Value, node.InnerText);
                    }
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("UpdateLanguage", PowerCheckType.Single);
            using (XmlHelper helper = new XmlHelper(ServerHelper.MapPath("~/Config/ShopLanguage.config")))
            {
                foreach (XmlNode node in helper.ReadNode("Language").ChildNodes)
                {
                    node.InnerText = RequestHelper.GetForm<string>(node.Attributes["key"].Value);
                }
                helper.Save();
            }
            AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateLanguage"));
            ScriptHelper.Alert(ShopLanguage.ReadLanguage("UpdateOK"), RequestHelper.RawUrl);
        }
    }
}
