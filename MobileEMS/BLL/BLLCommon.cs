using System;
using System.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Security;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace MobileEMS.BLL
{
    public sealed class BLLCommon
    {

        public static Dictionary<string, string> Login(string UserName, string Password)
        {
            Dictionary<string, string> LoginResult = new Dictionary<string, string>();
            LoginResult.Add("Success", "false");
            LoginResult.Add("Intro", "");

            Regex regex = new Regex("(?=^[0-9a-zA-Z._@#]{6,16}$)((?=.*[0-9])(?=.*[^0-9])|(?=.*[a-zA-Z])(?=.*[^a-zA-Z]))");
            if (!regex.IsMatch(Password))
            {
                CookiesHelper.AddCookie("ChangePassword", "true");
            }
            else
            {
                CookiesHelper.AddCookie("ChangePassword", "");
            }
            Password = StringHelper.Password(Password, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            UserInfo user = UserBLL.CheckUserLogin(UserName, Password);
            if (user.ID > 0)
            {
                switch (user.Status)
                {
                    case (int)UserState.NoCheck:
                        LoginResult["Intro"] = "用户未激活！";
                        break;
                    case (int)UserState.Other:
                    case (int)UserState.Free:
                    case (int)UserState.Normal:
                        user = UserBLL.ReadUserMore(user.ID);
                        //验证公司状态
                        CompanyInfo currentCompany = CompanyBLL.ReadCompany(user.CompanyID);
                        if (currentCompany.State == 1)
                        {
                            LoginResult["Intro"] = "贵公司已被管理员冻结，请联系管理员！";
                        }
                        else if (!string.IsNullOrEmpty(currentCompany.EndDate.ToString()) && DateTime.Today > Convert.ToDateTime(currentCompany.EndDate))
                        {
                            LoginResult["Intro"] = "贵公司使用期限已到！";
                        }
                        else
                        {
                            UserBLL.UserLoginInit(user);
                            UserBLL.UpdateUserLogin(user.ID, RequestHelper.DateNow, ClientHelper.IP);
                            CookiesHelper.AddCookie("SMSCheckCode", string.Empty);
                            LoginResult["Success"] = "true";
                            LoginResult.Add("Url", "CourseCenter.aspx?Action=PostCourse");
                        }
                        break;
                    case (int)UserState.Frozen:
                        LoginResult["Intro"] = "此用户已被管理员冻结，请联系管理员！";
                        break;
                }
            }
            else
            {
                LoginResult["Intro"] = "帐号与密码不匹配";
            }
            return LoginResult;
        }
    }
}
