using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;
using Newtonsoft.Json;
using MobileEMS.BLL;
using YXTSMS;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace MobileEMS
{
    public partial class SMSAjax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.Response.Cache.SetNoServerCaching();
            base.Response.Cache.SetNoStore();
            base.Response.Expires = 0;

            string queryString = RequestHelper.GetQueryString<string>("Action");
            switch (queryString)
            {
                case "Ver":
                    VerSMS();
                    break;

                case "CheckCode":
                    VerCheckCode();
                    break;

                case "SendCheckCode":
                    SendCheckCode();
                    break;

                case "OutWebsiteSMS":
                    OutWebsiteSMS();
                    break;
            }
        }

        private void OutWebsiteSMS()
        {
            string ToMobile = RequestHelper.GetQueryString<string>("ToMobile");
            string Content = RequestHelper.GetQueryString<string>("Content");
            string TempleId = RequestHelper.GetQueryString<string>("TempleId");
            string CallBack = RequestHelper.GetQueryString<string>("callback");

            SMSInfo SMSModel = new SMSInfo();
            SMSModel.AppId = "8a48b551506925be01506a33f41504f1";
            SMSModel.TempleId = TempleId;
            SMSModel.Mobile = ToMobile;
            //SMSModel.Content = new string[] { Content, CheckCode, SMSConfig.CodeTimeOut.ToString() };
            SMSModel.Content = HttpUtility.UrlDecode(Content).Split(',');

            Response.Write(CallBack);
            YXTSMS.YXTSMS api = new YXTSMS.YXTSMS();

            //if (api.init())
            //{
            //    Dictionary<string, object> retData = api.SendTemplateSMS(SMSModel);
            //    Response.Write(retData["statusCode"].ToString());
            //}

            if (api.SendSMS(SMSModel))
            {
                Response.Write("({\"result\":true})");
            }
            else
            {
                Response.Write("({\"result\":false})");
            }
        }


        private void VerSMS()
        {
            string Mobile = RequestHelper.GetQueryString<string>("Mobile");
            string CheckCode = RequestHelper.GetQueryString<string>("CheckCode");
            int productID = RequestHelper.GetQueryString<int>("CateId");
            string SendType = RequestHelper.GetQueryString<string>("SendType");
            string password = RequestHelper.GetQueryString<string>("Password");
            int Companyid = RequestHelper.GetQueryString<int>("BrandId");
            Dictionary<string, object> ReturnResult = new Dictionary<string, object>();

            try
            {
                int TimeOutSeconds = SMSConfig.CodeTimeOut * 60;

                if (CheckSMSCode(Mobile, CheckCode, TimeOutSeconds))
                {
                    switch (SendType)
                    {
                        case "Course":
                            //ProductInfo product = ProductBLL.ReadProduct(productID);
                            //string[] CateCodeArray = product.ProductNumber.Split('|');
                            //string VideoID = string.Empty;
                            //for (int i = 0; i < CateCodeArray.Length; i++)
                            //{
                            //    if (string.IsNullOrEmpty(VideoID)) VideoID = "\"" + CateCodeArray[i] + "\"";
                            //    else VideoID = VideoID + ",\"" + CateCodeArray[i] + "\"";
                            //}
                            //CookiesHelper.AddCookie("CourseVideoList", "{\"VideoCount\":" + CateCodeArray.Length + ",\"VideoID\":[" + VideoID + "],\"Check_Code\":\"" + Cookies.User.GetUserID(true) + "_" + CheckCode + "_" + productID.ToString() + "\",\"Mobile\":\"" + Mobile + "\"}", TimeOutSeconds, TimeType.Millisecond);
                            ReturnResult.Add("Code", 0);
                            break;
                        case "Login":
                            UserSearchInfo userSearch = new UserSearchInfo();
                            userSearch.InStatus = (int)UserState.Normal + "," + (int)UserState.Free + "," + (int)UserState.Other;
                            userSearch.GroupId = 36;
                            userSearch.Mobile = Mobile;
                            List<UserInfo> UserList = UserBLL.SearchUserList(userSearch);
                            if (UserList.Count > 0)
                            {
                                foreach (UserInfo info in UserList)
                                {
                                    CompanyInfo CompanyModel = CompanyBLL.ReadCompany(info.CompanyID);
                                    string SonCompanyId = CompanyBLL.ReadCompanyIdList(info.CompanyID.ToString());
                                    string CompanyParentId = CompanyBLL.ReadParentCompanyId(info.CompanyID);
                                    string CompanyBrandId = CompanyModel.BrandId;
                                    string str4 = Guid.NewGuid().ToString();
                                    string str5 = FormsAuthentication.HashPasswordForStoringInConfigFile(info.ID.ToString() + info.UserName + info.GroupID.ToString() + info.CompanyID.ToString() + SonCompanyId + CompanyBrandId + CompanyParentId + info.WorkingPostID.ToString() + CompanyModel.IsTest.ToString() + str4 + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent, "MD5");
                                    string str6 = str5 + "|" + info.ID.ToString() + "|" + info.UserName + "|" + info.GroupID.ToString() + "|" + info.CompanyID.ToString() + "|" + SonCompanyId + "|" + CompanyBrandId + "|" + CompanyParentId + "|" + info.WorkingPostID.ToString() + "|" + CompanyModel.IsTest.ToString() + "|" + str4;
                                    CookiesHelper.AddCookie(ShopConfig.ReadConfigInfo().AdminCookies, str6);
                                    AdminBLL.UpdateAdminLogin(info.ID, RequestHelper.DateNow, ClientHelper.IP);
                                    AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("LoginSystem"));
                                    CookiesHelper.AddCookie("SMSCheckCode", string.Empty);
                                    ReturnResult.Add("Code", 0);
                                }
                            }
                            else
                            {
                                ReturnResult.Add("Code", 1);
                                ReturnResult.Add("Message", "不存在此帐号，如果是管理员请电脑上操作！");
                            }
                            break;
                        case "Reg":
                            UserInfo user = new UserInfo();
                            user.CompanyID = Companyid;
                            user.RealName = "手机体验用户";
                            user.UserName = "Mobile" + DateTime.Now.ToString("yyMMddHHmm");
                            user.UserPassword = StringHelper.Password(password, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                            user.GroupID = 36;
                            user.WorkingPostID = 7;
                            user.StudyPostID = 7;
                            user.Status = (int)UserState.Normal;
                            user.RegisterDate = DateTime.Now;
                            user.LoginTimes = 0;
                            user.LastLoginDate = DateTime.Now;
                            user.PostName = "销售实习生";
                            user.Department = 3;
                            user.Mobile = Mobile;
                            UserBLL.AddUser(user);
                            Dictionary<string, string> LoginResult = BLLCommon.Login(user.UserName, password);
                            if (LoginResult["Success"] == "false")
                            {
                                ReturnResult.Add("Code", 1);
                                ReturnResult.Add("Message", "注册成功，登陆失败，请在登陆页面登陆！");
                                ReturnResult.Add("Url", "Login.aspx");
                            }
                            else
                            {
                                ReturnResult.Add("Code", 0);
                                ReturnResult.Add("Url", LoginResult["Url"]);
                            }
                            ReturnResult.Add("data", user);
                            break;
                    }
                }
                else
                {
                    ReturnResult.Add("Code", 1);
                    ReturnResult.Add("Message", "验证码错误或已失效！");
                }
            }
            catch
            {
                ReturnResult.Add("Code", 1);
                ReturnResult.Add("Message", "异常错误！");
            }
            ResponseHelper.Write(JsonConvert.SerializeObject(ReturnResult));
        }

        private bool CheckSMSCode(string Mobile, string CheckCode, int TimeOutSeconds)
        {
            bool IsOK = false;
            //从Cookies中读取验证码并解密
            string SrcCheckCode = StringHelper.Decode(CookiesHelper.ReadCookieValue("SMSCheckCode"), "SMS");

            //如果验证码值不为空（cookies的有效期只有几分钟）
            if (!string.IsNullOrEmpty(SrcCheckCode))
            {
                if (SrcCheckCode == CheckCode) IsOK = true;
                CookiesHelper.AddCookie("SMSIsChecked", StringHelper.Encode("true", "SMS"), SMSConfig.CodeTimeOut, TimeType.Minute);
            }
            else
            {
                SMSRecordInfo SMSRecordModel = SMSRecordBLL.ReadSMSRecord(Mobile, CheckCode);
                if (SMSRecordModel != null)
                {
                    if ((DateTime.Now - SMSRecordModel.DataCreateDate).TotalSeconds <= TimeOutSeconds)
                    {
                        if (SMSRecordModel.VerCode == CheckCode) IsOK = true;
                        CookiesHelper.AddCookie("SMSCheckCode", StringHelper.Encode(CheckCode, "SMS"), (SMSConfig.CodeTimeOut - (int)(DateTime.Now - SMSRecordModel.DataCreateDate).TotalMinutes), TimeType.Minute);
                        CookiesHelper.AddCookie("SMSIsChecked", StringHelper.Encode("true", "SMS"), (SMSConfig.CodeTimeOut - (int)(DateTime.Now - SMSRecordModel.DataCreateDate).TotalMinutes), TimeType.Minute);
                    }
                }
            }
            return IsOK;
        }

        private void VerCheckCode()
        {
            string CheckCode = RequestHelper.GetQueryString<string>("CheckCode");

            if (!Cookies.Common.checkcode.ToLower().Equals(CheckCode))
            {
                ResponseHelper.Write("2");
            }
            else
            { ResponseHelper.Write("1"); }
        }

        private void SendCheckCode()
        {
            string mobile = RequestHelper.GetQueryString<string>("Mobile");
            int productID = RequestHelper.GetQueryString<int>("CateId");
            string sendType = RequestHelper.GetQueryString<string>("SendType");

            if (!string.IsNullOrEmpty(mobile))
            {
                try
                {
                    //if (string.IsNullOrEmpty(CookiesHelper.ReadCookieValue("SMSCheckCode")))
                    {
                        switch (sendType)
                        {
                            case "Course":
                                UserInfo user = UserBLL.ReadUser(Cookies.User.GetUserID(true));
                                if (user != null && (user.Status == (int)UserState.Normal || user.Status == (int)UserState.Free || user.Status == (int)UserState.Other) && CompanyBLL.ReadCompany(user.CompanyID).State == 0)
                                {
                                    if (mobile == user.Mobile.ToString())
                                    {
                                        //SMSSend(mobile, ProductBLL.ReadProduct(productID).Name, "210196");
                                        SMSSend(mobile, null, "210196");
                                    }
                                    else
                                    {
                                        ResponseHelper.Write("2|输入的手机号码与登记的号码不一致！");
                                    }
                                }
                                else
                                {
                                    ResponseHelper.Write("2|帐号异常！");
                                }
                                break;

                            case "Login":
                                UserSearchInfo userSearch = new UserSearchInfo();
                                userSearch.Mobile = mobile;
                                userSearch.Status = 2;
                                List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
                                if (userList.Count > 0)
                                {
                                    SMSSend(mobile, "登陆", "17101");
                                }
                                else
                                {
                                    ResponseHelper.Write("2|帐号异常！");
                                }
                                break;
                            case "Reg":
                                SMSSend(mobile, "注册", "17101");
                                break;

                        }
                    }
                    //else
                    //{
                    //    ResponseHelper.Write("3|请勿频繁操作！");
                    //}
                }
                catch
                {
                    ResponseHelper.Write("1|异常错误！");
                }
            }
            else
            {
                ResponseHelper.Write("1|号码获取失败！");
            }
        }

        private void SMSSend(string Mobile, string Content, string TempleId)
        {
            YXTSMS.YXTSMS api = new YXTSMS.YXTSMS();
            string CheckCode = api.MakeCode();

            SMSInfo SMSModel = new SMSInfo();
            SMSModel.TempleId = TempleId;
            SMSModel.Mobile = Mobile;
            switch (TempleId)
            {
                case "17101":
                    SMSModel.Content = new string[] { Content, CheckCode, SMSConfig.CodeTimeOut.ToString() };
                    break;
                case "210196":
                    SMSModel.Content = new string[] { CheckCode, SMSConfig.CodeTimeOut.ToString() };
                    break;
            }
            //SMSModel.Content = new string[] { Content, CheckCode, SMSConfig.CodeTimeOut.ToString() };

            if (api.SendSMS(SMSModel))
            {
                SMSRecordInfo SMSRecordModel = new SMSRecordInfo();
                SMSRecordModel.Mobile = SMSModel.Mobile;
                SMSRecordModel.VerCode = CheckCode;
                SMSRecordModel.DataCreateDate = DateTime.Now;
                SMSRecordBLL.AddSMSRecord(SMSRecordModel);
                //验证码加密后写入Cookies中
                CookiesHelper.AddCookie("SMSCheckCode", StringHelper.Encode(CheckCode, "SMS"), SMSConfig.CodeTimeOut, TimeType.Minute);
                ResponseHelper.Write("0|发送成功！");
            }
            else
            {
                ResponseHelper.Write("1|发送失败！");
            }
        }
    }
}
