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
            base.Title = "�޸�����";
        }

        protected override void PostBack()
        {
            string password1 = RequestHelper.GetForm<string>("UserPassword1");
            string password2 = RequestHelper.GetForm<string>("UserPassword2");
            if (string.IsNullOrEmpty(password1) || string.IsNullOrEmpty("password2"))
            {
                ScriptHelper.Alert("���벻��Ϊ��");
            }
            if (password1 != password2) ScriptHelper.Alert("�������벻һ��");
            string str = StringHelper.Password(RequestHelper.GetForm<string>("OldPassword"), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            string newPassword = StringHelper.Password(RequestHelper.GetForm<string>("UserPassword1"), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            UserInfo info = UserBLL.ReadUser(base.UserID);
            if (str == info.UserPassword)
            {
                UserBLL.ChangePassword(base.UserID, newPassword);
                ScriptHelper.Alert("�����޸ĳɹ�", RequestHelper.RawUrl);
            }
            else
            {
                ScriptHelper.Alert("���������", RequestHelper.RawUrl);
            }
        }
    }
}
