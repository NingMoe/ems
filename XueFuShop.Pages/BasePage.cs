using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using XueFu.EntLib;
using System.Diagnostics;
using XueFuShop.Common;
using System.Web.Security;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class BasePage : IHttpHandler, IRequiresSessionState
    {

        private HttpContext context;
        private HttpRequest request;
        private HttpResponse response;
        private HttpServerUtility server;
        private HttpSessionState session;
        private bool needUserCookies = true;
        private string description = string.Empty;
        private string keywords = string.Empty;
        private string title = string.Empty;
        protected int GradeID = 0;
        protected decimal MoneyUsed = 0M;
        protected double processTime = 0.0;
        protected int UserID = 0;
        protected string UserName = string.Empty;
        protected int UserGroupID = 0;
        protected int UserCompanyID = int.MinValue;
        protected string UserMobile = string.Empty;
        protected string UserRealName = string.Empty;

        protected void CheckUserLogin()
        {
            if (this.UserID == 0)
            {
                ResponseHelper.Write("<script language='javascript'>window.location.href='/User/Login.aspx';</script>");
                ResponseHelper.End();
            }
        }

        private void PageInit(HttpContext context)
        {
            this.request = context.Request;
            this.server = context.Server;
            this.response = context.Response;
            this.session = context.Session;
            this.context = context;

            if (this.needUserCookies)
            {
                this.ReadUserCookies();
            }
        }

        protected virtual void PageLoad()
        {
        }

        protected virtual void PostBack()
        {
        }

        public void ProcessRequest(HttpContext context)
        {
            this.PageInit(context);
            if (RequestHelper.GetForm<string>("Action") == "PostBack")
            {
                this.PostBack();
            }
            else
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                this.PageLoad();
                stopwatch.Stop();
                this.processTime = stopwatch.Elapsed.TotalSeconds;
                this.ShowPage();
            }
        }

        /// <summary>
        /// 比较权限类的字符串
        /// </summary>
        /// <param name="powerString">比较字符串，用','隔开</param>
        /// <param name="powerCheckType">比较类型</param>
        /// <returns></returns>
        public bool CompareUserPower(string powerString, PowerCheckType powerCheckType)
        {
            string power = AdminGroupBLL.ReadAdminGroupCache(this.UserGroupID).Power;
            bool flag = false;
            string powerKey = ShopConfig.ReadConfigInfo().PowerKey;
            switch (powerCheckType)
            {
                case PowerCheckType.Single:
                    if (power.IndexOf("|" + powerKey + powerString + "|") > -1)
                    {
                        flag = true;
                    }
                    break;

                case PowerCheckType.OR:
                    foreach (string str2 in powerString.Split(new char[] { ',' }))
                    {
                        if (power.IndexOf("|" + powerKey + str2 + "|") > -1)
                        {
                            flag = true;
                            break;
                        }
                    }
                    break;

                case PowerCheckType.AND:
                    flag = true;
                    foreach (string str2 in powerString.Split(new char[] { ',' }))
                    {
                        if (power.IndexOf("|" + powerKey + str2 + "|") == -1)
                        {
                            flag = false;
                            break;
                        }
                    }
                    break;
            }
            return flag;
        }

        private void ReadUserCookies()
        {
            string userCookies = ShopConfig.ReadConfigInfo().UserCookies;
            string userCookiesValue = CookiesHelper.ReadCookieValue(userCookies);
            if (!string.IsNullOrEmpty(userCookiesValue))
            {
                try
                {
                    string[] strArray = userCookiesValue.Split(new char[] { '|' });
                    string ciphertext = strArray[0];
                    string userID = strArray[1];
                    string userName = strArray[2];
                    string moneyUsed = strArray[3];
                    string gradeID = strArray[4];
                    string mobile = strArray[5];
                    string groupID = strArray[6];
                    string companyID = strArray[7];
                    string realName = strArray[8];
                    if (FormsAuthentication.HashPasswordForStoringInConfigFile(userID + userName + moneyUsed.ToString() + gradeID.ToString() + mobile + groupID.ToString() + companyID.ToString() + realName + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent, "MD5").ToLower() == ciphertext.ToLower())
                    {
                        this.UserID = Convert.ToInt32(userID);
                        this.UserName = HttpContext.Current.Server.UrlDecode(userName);
                        this.MoneyUsed = Convert.ToDecimal(moneyUsed);
                        this.GradeID = Convert.ToInt32(gradeID);
                        this.UserMobile = mobile;
                        this.UserGroupID = Convert.ToInt32(groupID);
                        this.UserCompanyID = Convert.ToInt32(companyID);
                        this.UserRealName = HttpContext.Current.Server.UrlDecode(realName);
                    }
                    else
                    {
                        CookiesHelper.DeleteCookie(userCookies);
                    }
                }
                catch
                {
                    CookiesHelper.DeleteCookie(userCookies);
                }
            }
            if (this.GradeID == 0)
            {
                this.GradeID = UserGradeBLL.ReadUserGradeByMoney(0M).ID;
            }
        }

        protected virtual void ShowPage()
        {
        }


        public HttpContext Context
        {
            get { return this.context; }
        }

        public HttpRequest Request
        {
            get { return this.request; }
        }

        public HttpResponse Response
        {
            get { return this.response; }
        }

        public HttpServerUtility Server
        {
            get { return this.server; }
        }

        public HttpSessionState Session
        {
            get { return this.session; }
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public bool NeedUserCookies
        {
            set { this.needUserCookies = value; }
        }

        public string Author
        {
            get { return ShopConfig.ReadConfigInfo().Author; }
        }

        public string Copyright
        {
            get { return ShopConfig.ReadConfigInfo().Copyright; }
        }

        public string Description
        {
            get
            {
                string description = this.description;
                if (description == string.Empty)
                {
                    description = ShopConfig.ReadConfigInfo().Description;
                }
                return description;
            }
            set
            {
                this.description = value;
            }
        }

        public string Keywords
        {
            get
            {
                string keywords = this.keywords;
                if (keywords == string.Empty)
                {
                    keywords = ShopConfig.ReadConfigInfo().Keywords;
                }
                return keywords;
            }
            set
            {
                this.keywords = value;
            }
        }

        public string Title
        {
            get
            {
                string title = ShopConfig.ReadConfigInfo().Title;
                if (this.title != string.Empty)
                {
                    title = this.title + " - " + ShopConfig.ReadConfigInfo().Title;
                }
                return title;
            }
            set
            {
                this.title = value;
            }
        }

        protected int CurrentPage
        {
            get
            {
                int queryString = RequestHelper.GetQueryString<int>("Page");
                if (queryString < 1)
                {
                    queryString = 1;
                }
                return queryString;
            }
        }
    }
}
