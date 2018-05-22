using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using System.Text.RegularExpressions;
using XueFuShop.Models;
using System.Web;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class Register : CommonBasePage
    {
        
        protected string errorMessage = string.Empty;
        protected string result = string.Empty;

        
        protected override void PageLoad()
        {
            base.PageLoad();
            this.result = RequestHelper.GetQueryString<string>("Result");
            this.errorMessage = RequestHelper.GetQueryString<string>("ErrorMessage");
        }

        protected override void PostBack()
        {
            string userName = StringHelper.SearchSafe(StringHelper.AddSafe(RequestHelper.GetForm<string>("UserName")));
            string str2 = StringHelper.SearchSafe(StringHelper.AddSafe(RequestHelper.GetForm<string>("Email")));
            string form = RequestHelper.GetForm<string>("UserPassword1");
            string str4 = RequestHelper.GetForm<string>("UserPassword2");
            string str5 = RequestHelper.GetForm<string>("SafeCode");
            if (userName == string.Empty)
            {
                this.errorMessage = "�û�������Ϊ��";
            }
            if (this.errorMessage == string.Empty)
            {
                string forbiddenName = ShopConfig.ReadConfigInfo().ForbiddenName;
                if (forbiddenName != string.Empty)
                {
                    foreach (string str7 in forbiddenName.Split(new char[] { '|' }))
                    {
                        if (userName.IndexOf(str7.Trim()) != -1)
                        {
                            this.errorMessage = "�û������зǷ��ַ�";
                            break;
                        }
                    }
                }
            }
            if ((this.errorMessage == string.Empty) && (UserBLL.CheckUserName(userName) > 0))
            {
                this.errorMessage = "�û����Ѿ���ռ��";
            }
            if (this.errorMessage == string.Empty)
            {
                Regex regex = new Regex("^([a-zA-Z0-9_һ-��])+$");
                if (!regex.IsMatch(userName))
                {
                    this.errorMessage = "�û���ֻ�ܰ�����ĸ�����֡��»��ߡ�����";
                }
            }
            if ((this.errorMessage == string.Empty) && ((form == string.Empty) || (str4 == string.Empty)))
            {
                this.errorMessage = "���벻��Ϊ��";
            }
            if ((this.errorMessage == string.Empty) && (form != str4))
            {
                this.errorMessage = "�������벻һ��";
            }
            if ((this.errorMessage == string.Empty) && (str5.ToLower() != Cookies.Common.checkcode.ToLower()))
            {
                this.errorMessage = "��֤�����";
            }
            if (this.errorMessage == string.Empty)
            {
                UserInfo user = new UserInfo();
                user.UserName = userName;
                user.UserPassword = StringHelper.Password(form, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                user.Email = str2;
                user.RegisterIP = ClientHelper.IP;
                user.RegisterDate = RequestHelper.DateNow;
                user.PostStartDate = RequestHelper.DateNow;
                user.LastLoginIP = ClientHelper.IP;
                user.LastLoginDate = RequestHelper.DateNow;
                user.FindDate = RequestHelper.DateNow;
                if (ShopConfig.ReadConfigInfo().RegisterCheck == 1)
                {
                    user.Status = 2;
                }
                else
                {
                    user.Status = 1;
                }
                int id = UserBLL.AddUser(user);
                if (ShopConfig.ReadConfigInfo().RegisterCheck == 1)
                {
                    HttpCookie cookie = new HttpCookie(ShopConfig.ReadConfigInfo().UserCookies);
                    cookie["User"] = StringHelper.Encode(userName, ShopConfig.ReadConfigInfo().SecureKey);
                    cookie["Password"] = StringHelper.Encode(form, ShopConfig.ReadConfigInfo().SecureKey);
                    cookie["Key"] = StringHelper.Encode(ClientHelper.Agent, ShopConfig.ReadConfigInfo().SecureKey);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                    user = UserBLL.ReadUserMore(id);
                    UserBLL.UserLoginInit(user);
                    UserBLL.UpdateUserLogin(user.ID, RequestHelper.DateNow, ClientHelper.IP);
                    ResponseHelper.Redirect("/User/Index.aspx");
                }
                else if (ShopConfig.ReadConfigInfo().RegisterCheck == 2)
                {
                    string newValue = "http://" + base.Request.ServerVariables["HTTP_HOST"] + "/User/ActiveUser.aspx?CheckCode=" + StringHelper.Encode(string.Concat(new object[] { id, "|", str2, "|", userName }), ShopConfig.ReadConfigInfo().SecureKey);
                    EmailContentInfo info2 = EmailContentHelper.ReadSystemEmailContent("Register");
                    EmailSendRecordInfo emailSendRecord = new EmailSendRecordInfo();
                    emailSendRecord.Title = info2.EmailTitle;
                    emailSendRecord.Content = info2.EmailContent.Replace("$UserName$", user.UserName).Replace("$Url$", newValue);
                    emailSendRecord.IsSystem = 1;
                    emailSendRecord.EmailList = str2;
                    emailSendRecord.IsStatisticsOpendEmail = 0;
                    emailSendRecord.SendStatus = 1;
                    emailSendRecord.AddDate = RequestHelper.DateNow;
                    emailSendRecord.SendDate = RequestHelper.DateNow;
                    emailSendRecord.ID = EmailSendRecordBLL.AddEmailSendRecord(emailSendRecord);
                    EmailSendRecordBLL.SendEmail(emailSendRecord);
                    this.result = "��ϲ����ע��ɹ������¼���伤�<a href=\"http://mail." + str2.Substring(str2.IndexOf("@") + 1) + "\"  target=\"_blank\">���ϼ���</a>";
                }
                else
                {
                    this.result = "��ϲ����ע��ɹ�����ȴ����ǵ���ˣ�";
                }
                ResponseHelper.Redirect("/User/Register.aspx?Result=" + base.Server.UrlEncode(this.result));
            }
            else
            {
                ResponseHelper.Redirect("/User/Register.aspx?ErrorMessage=" + base.Server.UrlEncode(this.errorMessage));
            }
        }
    }

 

}
