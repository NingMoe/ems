using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.BLL;
using XueFuShop.Models;
using System.Web;

namespace XueFuShop.Pages
{
    public abstract class UserCommonBasePage : BasePage
    {
        protected int Count = 0;
        protected int PageSize = 20;
        protected CommonPagerClass CommonPager = new CommonPagerClass();
        protected UserInfo User = new UserInfo();
        protected string SonCompanyID = CookiesHelper.ReadCookieValue("UserCompanySonCompanyID");
        protected string ParentCompanyID = CookiesHelper.ReadCookieValue("UserCompanyParentCompanyID");

        protected override void PageLoad()
        {
            if (UserAgentHelper.IsMobile(HttpContext.Current.Request.UserAgent))
                ResponseHelper.Redirect("http://m.mostool.com");
            base.CheckUserLogin();
            User = UserBLL.ReadUser(base.UserID);
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
    }
}
