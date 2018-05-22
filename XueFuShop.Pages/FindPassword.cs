using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;

namespace XueFuShop.Pages
{
    public class FindPassword : CommonBasePage
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
            string userName = StringHelper.SearchSafe(RequestHelper.GetForm<string>("UserName"));
            //string email = StringHelper.SearchSafe(RequestHelper.GetForm<string>("Email"));
            //string form = RequestHelper.GetForm<string>("SafeCode");
            string mobile = StringHelper.SearchSafe(RequestHelper.GetForm<string>("phone"));
            string code = StringHelper.SearchSafe(RequestHelper.GetForm<string>("inputmovecode"));
            //int id = 0;
            if (userName == string.Empty)
            {
                this.errorMessage = "用户名不能为空";
            }
            if (string.IsNullOrEmpty(mobile))
            {
                this.errorMessage = "手机号码不能为空";
            }
            if (this.errorMessage == string.Empty)
            {
                //id = UserBLL.CheckUserName(userName);
                //if (id == 0)
                //{
                //    this.errorMessage = "不存在该用户名";
                //}
                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.Mobile = mobile;
                userSearch.UserName = UserName;
                if (UserBLL.SearchUserList(userSearch).Count <= 0)
                {
                    this.errorMessage = "不存在该用户名";
                }
            }

            if (this.errorMessage == string.Empty)
            {
                bool IsSend = false;
                //从Cookies中读取验证码并解密
                string SrcCheckCode = StringHelper.Decode(CookiesHelper.ReadCookieValue("SMSCheckCode"), "SMS");

                //如果验证码值不为空（cookies的有效期只有几分钟）
                if (!string.IsNullOrEmpty(SrcCheckCode))
                {
                    if (SrcCheckCode == code) IsSend = true;
                }
                else
                {
                    int TimeOutSeconds = 2 * 60;

                    SMSRecordInfo SMSRecordModel = SMSRecordBLL.ReadSMSRecord(mobile, code);
                    if (SMSRecordModel != null)
                    {
                        if ((DateTime.Now - SMSRecordModel.DataCreateDate).TotalSeconds <= TimeOutSeconds)
                        {
                            if (SMSRecordModel.VerCode == code) IsSend = true;
                        }
                    }
                }

                if (!IsSend)
                {
                    this.errorMessage = "手机验证码错误！";
                }
            }
            //if ((this.errorMessage == string.Empty) && (email == string.Empty))
            //{
            //    this.errorMessage = "Email不能为空";
            //}
            //if ((this.errorMessage == string.Empty) && !UserBLL.CheckEmail(email))
            //{
            //    this.errorMessage = "不存在该Email";
            //}
            //if ((this.errorMessage == string.Empty) && (form.ToLower() != Cookies.Common.checkcode.ToLower()))
            //{
            //    this.errorMessage = "验证码错误";
            //}
            //if ((this.errorMessage == string.Empty) && (UserBLL.ReadUser(id).Email != email))
            //{
            //    this.errorMessage = "用户名和Email不匹配";
            //}
            if (this.errorMessage == string.Empty)
            {
                //string safeCode = Guid.NewGuid().ToString();
                //UserBLL.ChangeUserSafeCode(id, safeCode, RequestHelper.DateNow);
                //string newValue = "http://" + base.Request.ServerVariables["HTTP_HOST"] + "/User/ResetPassword.aspx?CheckCode=" + StringHelper.Encode(string.Concat(new object[] { id, "|", email, "|", userName, "|", safeCode }), ShopConfig.ReadConfigInfo().SecureKey);
                //EmailContentInfo info2 = EmailContentHelper.ReadSystemEmailContent("FindPassword");
                //EmailSendRecordInfo emailSendRecord = new EmailSendRecordInfo();
                //emailSendRecord.Title = info2.EmailTitle;
                //emailSendRecord.Content = info2.EmailContent.Replace("$Url$", newValue);
                //emailSendRecord.IsSystem = 1;
                //emailSendRecord.EmailList = email;
                //emailSendRecord.IsStatisticsOpendEmail = 0;
                //emailSendRecord.SendStatus = 1;
                //emailSendRecord.AddDate = RequestHelper.DateNow;
                //emailSendRecord.SendDate = RequestHelper.DateNow;
                //emailSendRecord.ID = EmailSendRecordBLL.AddEmailSendRecord(emailSendRecord);
                //EmailSendRecordBLL.SendEmail(emailSendRecord);
                //this.result = "您的申请已提交，请登录邮箱重设你的密码！<a href=\"http://mail." + email.Substring(email.IndexOf("@") + 1) + "\"  target=\"_blank\">马上登录</a>";
                //ResponseHelper.Redirect("/User/FindPassword.aspx?Result=" + base.Server.UrlEncode(this.result));
                string userPassword = RequestHelper.GetForm<string>("password");

                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.Mobile = mobile;
                userSearch.UserName = userName;
                userSearch.StatusNoEqual = (int)UserState.Del;
                List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
                if (userList.Count < 5)  //限制一下，安全第一，以免条件出错，把所有的都改了
                {
                    foreach (UserInfo user in userList)
                    {
                        user.UserPassword = StringHelper.Password(userPassword, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                        UserBLL.ChangePassword(user.ID, user.UserPassword);
                    }
                }
                ScriptHelper.Alert("修改成功！", "/User/Login.aspx");
            }
            else
            {
                ResponseHelper.Redirect("/User/FindPassword.aspx?ErrorMessage=" + base.Server.UrlEncode(this.errorMessage));
            }
        }
    }

 

}
