using System;
using XueFuShop.Common;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;
using System.Web.Security;

namespace XueFuShop.Admin
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string loginName = StringHelper.SearchSafe(this.AdminName.Text);
            string content = StringHelper.SearchSafe(this.Password.Text);
            string text = this.SafeCode.Text;
            bool flag = this.Remember.Checked;
            if (!Cookies.Common.checkcode.ToLower().Equals(text.ToLower()))
            {
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("SafeCodeError"), RequestHelper.RawUrl);
            }
            content = StringHelper.Password(content, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            AdminInfo info = AdminBLL.CheckAdminLogin(loginName, content);
            if (info.ID > 0)
            {
                string str4 = Guid.NewGuid().ToString();
                string str5 = FormsAuthentication.HashPasswordForStoringInConfigFile(info.ID.ToString() + info.Name + info.GroupID.ToString() + str4 + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent, "MD5");
                string str6 = str5 + "|" + info.ID.ToString() + "|" + info.Name + "|" + info.GroupID.ToString() + "|" + str4;
                if (flag)
                {
                    CookiesHelper.AddCookie(ShopConfig.ReadConfigInfo().AdminCookies, str6, 1, TimeType.Year);
                }
                else
                {
                    CookiesHelper.AddCookie(ShopConfig.ReadConfigInfo().AdminCookies, str6);
                }
                AdminBLL.UpdateAdminLogin(info.ID, RequestHelper.DateNow, ClientHelper.IP);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("LoginSystem"));
                ResponseHelper.Redirect("Default.aspx");
            }
            else
            {
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("LoginFaild"), RequestHelper.RawUrl);
            }
        }
    }
}
