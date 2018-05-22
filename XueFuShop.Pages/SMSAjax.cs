using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;
using YXTSMS;

namespace XueFuShop.Pages
{
    public class SMSAjax : BasePage
    {
        protected override void PageLoad()
        {
            string queryString = RequestHelper.GetQueryString<string>("Action");
            switch (queryString)
            {
                case "Ver":
                    VerSMS();
                    break;

                case "VerFind":
                    VerFindSMS();
                    break;

                case "CheckCode":
                    VerCheckCode();
                    break;

                case "SendCheckCode":
                    SendCheckCode();
                    break;

                case "SendCode":
                    SendCode();
                    break;
            }
        }

        private void VerFindSMS()
        {
            string Mobile = RequestHelper.GetQueryString<string>("Mobile");
            string CheckCode = RequestHelper.GetQueryString<string>("CheckCode");
            bool IsSend = false;

            try
            {
                //��Cookies�ж�ȡ��֤�벢����
                string SrcCheckCode = StringHelper.Decode(CookiesHelper.ReadCookieValue("SMSCheckCode"), "SMS");

                //�����֤��ֵ��Ϊ�գ�cookies����Ч��ֻ�м����ӣ�
                if (!string.IsNullOrEmpty(SrcCheckCode))
                {
                    if (SrcCheckCode == CheckCode) IsSend = true;
                }
                else
                {
                    int TimeOutSeconds = 2 * 60;

                    SMSRecordInfo SMSRecordModel = SMSRecordBLL.ReadSMSRecord(Mobile, CheckCode);
                    if (SMSRecordModel != null)
                    {
                        if ((DateTime.Now - SMSRecordModel.DataCreateDate).TotalSeconds <= TimeOutSeconds)
                        {
                            if (SMSRecordModel.VerCode == CheckCode) IsSend = true;
                        }
                    }
                }

                if (IsSend)
                {
                    ResponseHelper.Write("0");
                }
                else
                {
                    ResponseHelper.Write("��֤�벻��ȷ");
                }
            }
            catch
            {
                ResponseHelper.Write("�쳣����");
            }
        }

        //private void VerSMS()
        //{
        //    string Mobile = RequestHelper.GetQueryString<string>("Mobile");
        //    string CheckCode = RequestHelper.GetQueryString<string>("CheckCode");
        //    int productID = RequestHelper.GetQueryString<int>("CateId");
        //    int Part = RequestHelper.GetQueryString<int>("Part");
        //    int UserId = RequestHelper.GetQueryString<int>("UserId");
        //    bool IsSend = false;

        //    try
        //    {
        //        ProductInfo product = ProductBLL.ReadProduct(productID);
        //        //��Cookies�ж�ȡ��֤�벢����
        //        string SrcCheckCode = StringHelper.Decode(CookiesHelper.ReadCookieValue("SMSCheckCode"), "SMS");

        //        //�����֤��ֵ��Ϊ�գ�cookies����Ч��ֻ�м����ӣ�
        //        if (!string.IsNullOrEmpty(SrcCheckCode))
        //        {
        //            if (SrcCheckCode == CheckCode) IsSend = true;
        //        }
        //        else
        //        {
        //            int TimeOutSeconds = SMSConfig.CodeTimeOut * 60;

        //            //if (TestCateModel.CateCode.Contains("|"))
        //            //{
        //            //    //�����Ƶ����֤��Ч���ӳ������Сʱ
        //            //    TimeOutSeconds = 5 * 3600;
        //            //}
        //            SMSRecordInfo SMSRecordModel = SMSRecordBLL.ReadSMSRecord(Mobile, CheckCode);
        //            if (SMSRecordModel != null)
        //            {
        //                if ((DateTime.Now - SMSRecordModel.DataCreateDate).TotalSeconds <= TimeOutSeconds)
        //                {
        //                    if (SMSRecordModel.VerCode == CheckCode) IsSend = true;
        //                }
        //            }
        //        }

        //        if (IsSend)
        //        {
        //            //�������֤��cookies��־
        //            CookiesHelper.AddCookie("SMSIsChecked", StringHelper.Encode("true", "SMS"), SMSConfig.CodeTimeOut, TimeType.Minute);
        //            if (UserId == int.MinValue) UserId = base.UserID;

        //            if (Part < 0) Part = 0;
        //            product.ProductNumber = "a5b7f39294fe8738289db2ae88ccc896_a|a5b7f39294ccd4aa12c69403ad6287ec_a";
        //            string[] CateCodeArray = product.ProductNumber.Split('|');
        //            for (int i = 0; i < CateCodeArray.Length; i++)
        //            {
        //                if (Part == i)
        //                    ResponseHelper.Write("<li class=\"current\"><a href=\"javascript:compareMoveCode(" + productID.ToString() + "," + i.ToString() + ",'" + Mobile + "','" + CheckCode + "');\">�� " + (i + 1).ToString() + " ��</a></li>");
        //                else
        //                    ResponseHelper.Write("<li><a href=\"javascript:compareMoveCode(" + productID.ToString() + "," + i.ToString() + ",'" + Mobile + "','" + CheckCode + "');\">�� " + (i + 1).ToString() + " ��</a></li>");
        //            }

        //            ResponseHelper.Write("|http://player.polyv.net/videos/player.swf?vid=" + CateCodeArray[Part] + "&code=" + UserId.ToString() + "_" + CheckCode);
        //            //ResponseHelper.Write("|http://yuntv.letv.com/bcloud.html?uu=debb2235d3&vu=" + CateCodeArray[Part] + "&auto_play=1&gpcflag=1&width=800&height=475&payer_name=" + Mobile + "&check_code=" + UserId.ToString() + "_" + CheckCode + "_" + productID.ToString() + "&extend=0&share=0");
        //        }
        //        else
        //        {
        //            ResponseHelper.Write("��֤�벻��ȷ");
        //        }
        //    }
        //    catch
        //    {
        //        ResponseHelper.Write("�쳣����");
        //    }
        //}

        private void VerSMS()
        {
            string Mobile = RequestHelper.GetQueryString<string>("Mobile");
            string CheckCode = RequestHelper.GetQueryString<string>("CheckCode");
            int productID = RequestHelper.GetQueryString<int>("CateId");
            int Part = RequestHelper.GetQueryString<int>("Part");
            int UserId = RequestHelper.GetQueryString<int>("UserId");
            bool IsSend = false;

            try
            {
                //��Cookies�ж�ȡ��֤�벢����
                string SrcCheckCode = StringHelper.Decode(CookiesHelper.ReadCookieValue("SMSCheckCode"), "SMS");

                //�����֤��ֵ��Ϊ�գ�cookies����Ч��ֻ�м����ӣ�
                if (!string.IsNullOrEmpty(SrcCheckCode))
                {
                    if (SrcCheckCode == CheckCode) IsSend = true;
                    CookiesHelper.AddCookie("SMSIsChecked", StringHelper.Encode("true", "SMS"), SMSConfig.CodeTimeOut, TimeType.Minute);
                }
                else
                {
                    int TimeOutSeconds = SMSConfig.CodeTimeOut * 60;
                    SMSRecordInfo SMSRecordModel = SMSRecordBLL.ReadSMSRecord(Mobile, CheckCode);
                    if (SMSRecordModel != null)
                    {
                        if ((DateTime.Now - SMSRecordModel.DataCreateDate).TotalSeconds <= TimeOutSeconds)
                        {
                            if (SMSRecordModel.VerCode == CheckCode) IsSend = true;
                            CookiesHelper.AddCookie("SMSCheckCode", StringHelper.Encode(CheckCode, "SMS"), (SMSConfig.CodeTimeOut - (int)(DateTime.Now - SMSRecordModel.DataCreateDate).TotalMinutes), TimeType.Minute);
                            CookiesHelper.AddCookie("SMSIsChecked", StringHelper.Encode("true", "SMS"), (SMSConfig.CodeTimeOut - (int)(DateTime.Now - SMSRecordModel.DataCreateDate).TotalMinutes), TimeType.Minute);
                        }
                    }
                }

                if (IsSend)
                {
                    //�������֤��cookies��־
                    //CookiesHelper.AddCookie("SMSIsChecked", StringHelper.Encode("true", "SMS"), SMSConfig.CodeTimeOut, TimeType.Minute);
                    ResponseHelper.Write("true");
                }
                else
                {
                    ResponseHelper.Write("��֤�벻��ȷ");
                }
            }
            catch
            {
                ResponseHelper.Write("�쳣����");
            }
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
            int userID = RequestHelper.GetQueryString<int>("UserId");

            if (!string.IsNullOrEmpty(mobile))
            {
                //try
                {
                    //if (string.IsNullOrEmpty(CookiesHelper.ReadCookieValue("SMSCheckCode")))
                    {
                        UserInfo user = new UserInfo();
                        if (userID == int.MinValue) userID = base.UserID;
                        if (UserID > 0)
                        {
                            user = UserBLL.ReadUser(userID);
                        }
                        else
                        {
                            user = UserBLL.ReadUserByMobile(mobile);
                        }

                        if ((user.Status == (int)UserState.Normal || user.Status == (int)UserState.Free || user.Status == (int)UserState.Other) && CompanyBLL.ReadCompany(user.CompanyID).State == 0)
                        {
                            if (mobile == user.Mobile)
                            {
                                YXTSMS.YXTSMS api = new YXTSMS.YXTSMS();
                                string CheckCode = api.MakeCode();

                                SMSInfo SMSModel = new SMSInfo();
                                SMSModel.Mobile = mobile;
                                //SMSModel.Content = new string[] { ProductBLL.ReadProduct(productID).Name, CheckCode, SMSConfig.CodeTimeOut.ToString() };
                                SMSModel.Content = new string[] { CheckCode, SMSConfig.CodeTimeOut.ToString() };
                                if (api.SendSMS(SMSModel))
                                {
                                    SMSRecordInfo SMSRecordModel = new SMSRecordInfo();
                                    SMSRecordModel.Mobile = SMSModel.Mobile;
                                    SMSRecordModel.VerCode = CheckCode;
                                    SMSRecordModel.DataCreateDate = DateTime.Now;
                                    SMSRecordBLL.AddSMSRecord(SMSRecordModel);

                                    //��֤����ܺ�д��Cookies��
                                    CookiesHelper.AddCookie("SMSCheckCode", StringHelper.Encode(CheckCode, "SMS"), SMSConfig.CodeTimeOut, TimeType.Minute);
                                    ResponseHelper.Write("0|���ͳɹ���|" + user.ID);
                                }
                                else
                                {
                                    ResponseHelper.Write("1|����ʧ�ܣ�");
                                }
                            }
                            else
                            {
                                ResponseHelper.Write("2|������ֻ�������Ǽǵĺ��벻һ�£�");
                            }
                        }
                        else
                        {
                            ResponseHelper.Write("2|�ʺŲ���ʹ�ã�");
                        }
                    }
                    //else
                    //{
                    //    ResponseHelper.Write("1|����Ƶ��������");
                    //}
                }
                //catch
                //{
                //    ResponseHelper.Write("1|�쳣����");
                //}
            }
            else
            {
                ResponseHelper.Write("1|�����ȡʧ�ܣ�");
            }
        }

        private void SendCode()
        {
            string Mobile = RequestHelper.GetQueryString<string>("Mobile");

            if (!string.IsNullOrEmpty(Mobile))
            {
                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.Mobile = Mobile;
                List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
                if (userList.Count > 0)
                {
                    YXTSMS.YXTSMS api = new YXTSMS.YXTSMS();
                    string CheckCode = api.MakeCode();

                    SMSInfo SMSModel = new SMSInfo();
                    SMSModel.Mobile = Mobile;
                    SMSModel.Content = new string[] { "�һ�����", CheckCode, "2" };
                    if (api.SendSMS(SMSModel))
                    {
                        SMSRecordInfo SMSRecordModel = new SMSRecordInfo();
                        SMSRecordModel.Mobile = SMSModel.Mobile;
                        SMSRecordModel.VerCode = CheckCode;
                        SMSRecordModel.DataCreateDate = DateTime.Now;
                        SMSRecordBLL.AddSMSRecord(SMSRecordModel);

                        //��֤����ܺ�д��Cookies��
                        CookiesHelper.AddCookie("SMSCheckCode", StringHelper.Encode(CheckCode, "SMS"), 2, TimeType.Minute);
                        ResponseHelper.Write("0|���ͳɹ���");
                    }
                    else
                    {
                        ResponseHelper.Write("1|����ʧ�ܣ�");
                    }
                }
                else
                {
                    ResponseHelper.Write("2|�ֻ����벻���ڣ�");
                }
            }
            else
            {
                ResponseHelper.Write("1|�����ȡʧ�ܣ�");
            }
        }
    }
}
