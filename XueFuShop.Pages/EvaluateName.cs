using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;

namespace XueFuShop.Pages
{
    public class EvaluateName : UserManageBasePage
    {
        protected List<EvaluateNameInfo> EvaluateNameList = new List<EvaluateNameInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "ÆÀ¹ÀÃû³ÆÁÐ±í";

            string Action = RequestHelper.GetQueryString<string>("Action");

            if (Action == "Delete")
            {
                base.CheckUserPower("DeleteEvaluateName", PowerCheckType.Single);
                string selectID = RequestHelper.GetQueryString<string>("SelectID");
                if (!string.IsNullOrEmpty(selectID))
                {
                    EvaluateNameBLL.DeleteEvaluateName(selectID);
                    AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("EvaluateName"), selectID);
                    ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), Request.UrlReferrer.ToString());
                }
            }

            base.CheckUserPower("ReadEvaluateName", PowerCheckType.Single);
            EvaluateNameInfo evaluateName = new EvaluateNameInfo();
            evaluateName.CompanyIdCondition = base.UserCompanyID.ToString();
            EvaluateNameList = EvaluateNameBLL.SearchEvaluateNameList(evaluateName, base.CurrentPage, base.PageSize, ref this.Count);
            base.BindPageControl(ref base.CommonPager);
        }
    }
}
