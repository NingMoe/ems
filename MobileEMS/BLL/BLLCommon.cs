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
                        LoginResult["Intro"] = "�û�δ���";
                        break;
                    case (int)UserState.Other:
                    case (int)UserState.Free:
                    case (int)UserState.Normal:
                        user = UserBLL.ReadUserMore(user.ID);
                        //��֤��˾״̬
                        CompanyInfo currentCompany = CompanyBLL.ReadCompany(user.CompanyID);
                        if (currentCompany.State == 1)
                        {
                            LoginResult["Intro"] = "��˾�ѱ�����Ա���ᣬ����ϵ����Ա��";
                        }
                        else if (!string.IsNullOrEmpty(currentCompany.EndDate.ToString()) && DateTime.Today > Convert.ToDateTime(currentCompany.EndDate))
                        {
                            LoginResult["Intro"] = "��˾ʹ�������ѵ���";
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
                        LoginResult["Intro"] = "���û��ѱ�����Ա���ᣬ����ϵ����Ա��";
                        break;
                }
            }
            else
            {
                LoginResult["Intro"] = "�ʺ������벻ƥ��";
            }
            return LoginResult;
        }
    }
}
