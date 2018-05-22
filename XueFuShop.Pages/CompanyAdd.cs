using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class CompanyAdd : UserManageBasePage
    {
        protected int companyID = RequestHelper.GetQueryString<int>("ID");
        protected CompanyInfo company = new CompanyInfo();
        protected List<ProductBrandInfo> productBrandList = new List<ProductBrandInfo>();
        protected List<PostInfo> postList = new List<PostInfo>();
        protected List<PostInfo> departmentList = new List<PostInfo>();
        protected string parentCompanyPath = string.Empty;

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "π´Àæ±‡º≠";
            base.CheckUserPower("ReadCompany,UpdateCompany,UpdateSubCompany", PowerCheckType.OR);

            string sonCompanyID = CookiesHelper.ReadCookieValue("UserCompanySonCompanyID");
            if (companyID > 0)
            {
                if (!StringHelper.CompareSingleString(sonCompanyID, companyID.ToString()))
                    ScriptHelper.Alert("“Ï≥£");
            }
            else
            {
                companyID = base.UserCompanyID;
            }
            company = CompanyBLL.ReadCompany(companyID);
            if (StringHelper.CompareSingleString(company.BrandId, "0"))
                productBrandList = ProductBrandBLL.ReadProductBrandCacheList();
            else
                productBrandList = ProductBrandBLL.ReadProductBrandCacheList(company.BrandId);
            departmentList = PostBLL.ReadParentPostListByPostId(company.Post);
            postList = PostBLL.ReadPostListByPostId(company.Post);
            if (company.GroupId == (int)CompanyType.SubCompany || company.GroupId == (int)CompanyType.SubGroup)
                parentCompanyPath = CompanyBLL.ReadParentCompanyName(company.ParentId);
        }


        protected override void PostBack()
        {
            if (companyID < 0)
                companyID = base.UserCompanyID;
            if (companyID == base.UserCompanyID)
                base.CheckUserPower("UpdateCompany", PowerCheckType.Single);
            else
                base.CheckUserPower("UpdateSubCompany", PowerCheckType.Single);
            company = CompanyBLL.ReadCompany(companyID);
            company.CompanyTel = StringHelper.AddSafe(RequestHelper.GetForm<string>("CompanyTel"));
            company.CompanyPost = StringHelper.AddSafe(RequestHelper.GetForm<string>("CompanyPost"));
            company.CompanyAddress = StringHelper.AddSafe(RequestHelper.GetForm<string>("CompanyAddress"));
            if (RequestHelper.GetForm<int>("IsTest") == 1)
                company.IsTest = true;
            else
                company.IsTest = false;
            company.State = RequestHelper.GetForm<int>("State");

            string alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            CompanyBLL.UpdateCompany(company);
            AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Company"), company.CompanyId);
            string returnURL = ServerHelper.UrlDecode(RequestHelper.GetQueryString<string>("ReturnURL"));
            if (string.IsNullOrEmpty(returnURL))
                ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
            else
                ScriptHelper.Alert(alertMessage, returnURL);
        }


        protected List<PostInfo> FilterPostListByParentID(int parentID)
        {
            return PostBLL.FilterPostListByParentID(postList, parentID);
        }

        
    }
}
