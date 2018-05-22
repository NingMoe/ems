using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;

namespace XueFuShop.Pages
{
    public abstract class AjaxBasePage : BasePage
    {
        protected string SonCompanyID = CookiesHelper.ReadCookieValue("UserCompanySonCompanyID");
        protected string ParentCompanyID = CookiesHelper.ReadCookieValue("UserCompanyParentCompanyID");
        protected string CompanyBrandID = CookiesHelper.ReadCookieValue("UserCompanyBrandID");

        protected AjaxBasePage()
        {
        }

        protected void ClearCache()
        {
            base.Response.Cache.SetNoServerCaching();
            base.Response.Cache.SetNoStore();
            base.Response.Expires = 0;
        }

        protected override void PageLoad()
        {
            this.ClearCache();

            //防止通过修改CompanyID值来非法获取其他公司信息
            string companyID = Request["CompanyID"];
            if (!string.IsNullOrEmpty(companyID) && companyID != "0" && companyID != int.MinValue.ToString() && !string.IsNullOrEmpty(StringHelper.SubString(companyID, this.SonCompanyID)))
            {
                ScriptHelper.Alert("公司信息有误！");
            }
        }
    }

 

}
