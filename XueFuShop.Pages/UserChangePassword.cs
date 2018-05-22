using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class UserChangePassword : UserCommonBasePage
    {
        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "修改密码";
        }

        protected override void PostBack()
        {
            string password1 = RequestHelper.GetForm<string>("UserPassword1");
            string password2 = RequestHelper.GetForm<string>("UserPassword2");
            if (string.IsNullOrEmpty(password1) || string.IsNullOrEmpty("password2"))
            {
                ScriptHelper.Alert("密码不能为空");
            }
            if (password1 != password2) ScriptHelper.Alert("两次密码不一致");
            string str = StringHelper.Password(RequestHelper.GetForm<string>("OldPassword"), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            string newPassword = StringHelper.Password(RequestHelper.GetForm<string>("UserPassword1"), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            UserInfo info = UserBLL.ReadUser(base.UserID);
            if (str == info.UserPassword)
            {
                UserBLL.ChangePassword(base.UserID, newPassword);
                ScriptHelper.Alert("密码修改成功", RequestHelper.RawUrl);
            }
            else
            {
                ScriptHelper.Alert("旧密码错误", RequestHelper.RawUrl);
            }
        }
    }
}
