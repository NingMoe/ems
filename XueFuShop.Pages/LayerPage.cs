using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class LayerPage : UserManageBasePage
    {
        protected string action = RequestHelper.GetQueryString<string>("Action");
        protected UserInfo userModel = new UserInfo();

        protected override void PageLoad()
        {
            base.PageLoad();

            switch (action)
            { 
                case "ChangeUserPassword":
                    ChangeUserPasswordLoad();
                    break;
            }
        }

        protected override void PostBack()
        {
            switch (action)
            {
                case "ChangeUserPassword":
                    ChangeUserPassword();
                    break;
            }
        }

        private void ChangeUserPasswordLoad()
        {
            int userID = RequestHelper.GetQueryString<int>("UserID");
            base.CheckUserIDInCompany(userID);
            base.CheckUserPower("UpdateUserPassword", PowerCheckType.Single);
            userModel = UserBLL.ReadUser(userID);
        }

        private void ChangeUserPassword()
        {
            int userID = RequestHelper.GetQueryString<int>("UserID");
            string newPassword = RequestHelper.GetForm<string>("UserPassword1");
            if (userID > 0)
            {
                newPassword = StringHelper.Password(newPassword, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                UserBLL.ChangePassword(userID, newPassword);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("ChangeUserPassword"), userID);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("UpdatePasswordOK"));
            }
        }
    }
}
