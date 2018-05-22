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
                this.errorMessage = "�û�������Ϊ��";
            }
            if (string.IsNullOrEmpty(mobile))
            {
                this.errorMessage = "�ֻ����벻��Ϊ��";
            }
            if (this.errorMessage == string.Empty)
            {
                //id = UserBLL.CheckUserName(userName);
                //if (id == 0)
                //{
                //    this.errorMessage = "�����ڸ��û���";
                //}
                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.Mobile = mobile;
                userSearch.UserName = UserName;
                if (UserBLL.SearchUserList(userSearch).Count <= 0)
                {
                    this.errorMessage = "�����ڸ��û���";
                }
            }

            if (this.errorMessage == string.Empty)
            {
                bool IsSend = false;
                //��Cookies�ж�ȡ��֤�벢����
                string SrcCheckCode = StringHelper.Decode(CookiesHelper.ReadCookieValue("SMSCheckCode"), "SMS");

                //�����֤��ֵ��Ϊ�գ�cookies����Ч��ֻ�м����ӣ�
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
                    this.errorMessage = "�ֻ���֤�����";
                }
            }
            //if ((this.errorMessage == string.Empty) && (email == string.Empty))
            //{
            //    this.errorMessage = "Email����Ϊ��";
            //}
            //if ((this.errorMessage == string.Empty) && !UserBLL.CheckEmail(email))
            //{
            //    this.errorMessage = "�����ڸ�Email";
            //}
            //if ((this.errorMessage == string.Empty) && (form.ToLower() != Cookies.Common.checkcode.ToLower()))
            //{
            //    this.errorMessage = "��֤�����";
            //}
            //if ((this.errorMessage == string.Empty) && (UserBLL.ReadUser(id).Email != email))
            //{
            //    this.errorMessage = "�û�����Email��ƥ��";
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
                //this.result = "�����������ύ�����¼��������������룡<a href=\"http://mail." + email.Substring(email.IndexOf("@") + 1) + "\"  target=\"_blank\">���ϵ�¼</a>";
                //ResponseHelper.Redirect("/User/FindPassword.aspx?Result=" + base.Server.UrlEncode(this.result));
                string userPassword = RequestHelper.GetForm<string>("password");

                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.Mobile = mobile;
                userSearch.UserName = userName;
                userSearch.StatusNoEqual = (int)UserState.Del;
                List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
                if (userList.Count < 5)  //����һ�£���ȫ��һ�������������������еĶ�����
                {
                    foreach (UserInfo user in userList)
                    {
                        user.UserPassword = StringHelper.Password(userPassword, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                        UserBLL.ChangePassword(user.ID, user.UserPassword);
                    }
                }
                ScriptHelper.Alert("�޸ĳɹ���", "/User/Login.aspx");
            }
            else
            {
                ResponseHelper.Redirect("/User/FindPassword.aspx?ErrorMessage=" + base.Server.UrlEncode(this.errorMessage));
            }
        }
    }

 

}
