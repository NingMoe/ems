using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFu.EntLib;
using YXTSMS;
using System.Security.Cryptography;
using System.Text;

namespace XueFuShop.API.Polyv
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PolyvAPI : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";

            string vid = context.Request.QueryString["vid"];
            string code = context.Request.QueryString["code"];
            string t = context.Request.QueryString["t"];
            string callback = context.Request.QueryString["callback"];
            int status = 2;
            string result = string.Empty;

            try
            {
                //判断来源，如果是铃木中国的就放行
                string referer = context.Request.ServerVariables["Http_Referer"];
                if (referer.ToLower().Contains("suzuki") || referer.ToLower().Contains("player.polyv.net"))
                {
                    status = 1;
                    string sign = BuildRequestMysign("vid=" + vid + "&secretkey=6NrHe1WPPO&username=suzuki&code=" + code + "&status=" + status + "&t=" + t);
                    result = "{\"status\":" + status + ",\"username\":\"suzuki\",\"sign\":\"" + sign + "\"}";
                }
                else
                {
                    string[] codeArray = code.Split('_');
                    UserInfo user = UserBLL.ReadUser(int.Parse(codeArray[0]));
                    if ((user.Status == (int)UserState.Normal || user.Status == (int)UserState.Free || user.Status == (int)UserState.Other) && CompanyBLL.ReadCompany(user.CompanyID).State == 0)
                    {
                        int TimeOutSeconds = SMSConfig.CodeTimeOut * 60;

                        SMSRecordInfo SMSRecordModel = SMSRecordBLL.ReadSMSRecord(user.Mobile, codeArray[1]);
                        if (SMSRecordModel != null)
                        {
                            if ((DateTime.Now - SMSRecordModel.DataCreateDate).TotalSeconds <= TimeOutSeconds)
                            {
                                status = 1;
                            }
                        }
                    }
                    string sign = BuildRequestMysign("vid=" + vid + "&secretkey=6NrHe1WPPO&username=" + user.UserName + "&code=" + code + "&status=" + status + "&t=" + t);
                    result = "{\"status\":" + status + ",\"username\":\"" + user.UserName + "\",\"sign\":\"" + sign + "\"}";
                }

                if (!string.IsNullOrEmpty(callback))
                    result = string.Format("{0}({1})", callback, result);
            }
            catch
            {
            }
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 生成请求时的签名
        /// </summary>
        /// <param name="sPara">请求的参数数组</param>
        /// <returns>签名结果</returns>
        private string BuildRequestMysign(string source)
        {
            StringBuilder mysign = new StringBuilder(32);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.Default.GetBytes(source));
            for (int i = 0; i < t.Length; i++)
            {
                mysign.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return mysign.ToString();
        }
    }
}
