using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Security;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class MobileUserBasePage : Page
    {
        private string description = string.Empty;
        private string keywords = string.Empty;
        private string title = string.Empty;
        protected int Count = 0;
        protected int PageSize = 20;
        protected CommonPagerClass CommonPager = new CommonPagerClass();
        protected int UserID = 0;
        protected string UserName = string.Empty;
        protected decimal MoneyUsed = 0M;
        protected int GradeID = 0;
        protected string UserMobile = string.Empty;
        protected int UserGroupID = 0;
        protected int UserCompanyID = int.MinValue;
        protected string UserRealName = string.Empty;
        protected string SonCompanyID = CookiesHelper.ReadCookieValue("UserCompanySonCompanyID");
        protected string ParentCompanyID = CookiesHelper.ReadCookieValue("UserCompanyParentCompanyID");
        protected string CompanyBrandID = CookiesHelper.ReadCookieValue("UserCompanyBrandID");

        protected override void OnInit(EventArgs e)
        {
            //if (!UserAgentHelper.IsMobile(HttpContext.Current.Request.UserAgent))
            //    ResponseHelper.Redirect("http://ems.mostool.com");
            this.ReadUserCookies();
            this.CheckUserLogin();
            base.OnInit(e);
        }

        protected void CheckUserLogin()
        {
            if (this.UserID == 0)
            {
                ResponseHelper.Write("<script language='javascript'>window.location.href='/Login.aspx';</script>");
                ResponseHelper.End();
            }
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
                //this.GradeID = UserGradeBLL.ReadUserGradeByMoney(0M).ID;
            }
        }

        protected void ClearCache()
        {
            base.Response.Cache.SetNoServerCaching();
            base.Response.Cache.SetNoStore();
            base.Response.Expires = 0;
        }

        protected void BindPageControl(ref CommonPagerClass commonPager)
        {
            if (commonPager != null)
            {
                commonPager.CurrentPage = this.CurrentPage;
                commonPager.PageSize = this.PageSize;
                commonPager.Count = this.Count;
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

        protected void CheckUserPower(string powerString, PowerCheckType powerCheckType)
        {
            if (!CompareUserPower(powerString, powerCheckType))
            {
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("NoPower"));
            }
        }

        /// <summary>
        /// 判断是否当前页面
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected bool IsCurrentPage(string fileName)
        {
            bool isCurrentPage = false;
            fileName = fileName.ToLower();
            if (fileName.IndexOf("$") > 0)
            {
                if (RequestHelper.RawUrl.ToLower().EndsWith(fileName.Replace("$", "")))
                    isCurrentPage = true;
            }
            else
            {
                if (RequestHelper.RawUrl.ToLower().IndexOf(fileName) >= 0)
                    isCurrentPage = true;
            }
            return isCurrentPage;
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
