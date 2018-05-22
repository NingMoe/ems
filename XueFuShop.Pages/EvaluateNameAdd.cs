using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class EvaluateNameAdd : UserManageBasePage
    {
        protected int CompanyID = RequestHelper.GetQueryString<int>("CompanyID");
        protected int ID = RequestHelper.GetQueryString<int>("ID");
        protected EvaluateNameInfo EvaluateName = new EvaluateNameInfo();
        protected string ReturnUrl = RequestHelper.GetQueryString<string>("ReturnUrl");

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "Ìí¼ÓÆÀ¹ÀÃû³Æ";
            base.CheckUserPower("AddEvaluateName,UpdateEvaluateName", PowerCheckType.OR);

            if (ID > 0)
            {
                EvaluateName = EvaluateNameBLL.ReadEvaluateName(ID);
            }
        }

        protected override void PostBack()
        {
            EvaluateName.ID = RequestHelper.GetForm<int>("EvaluationNameId");
            EvaluateName.EvaluateName = StringHelper.AddSafe(RequestHelper.GetForm<string>("EvaluationName"));
            EvaluateName.Date = StringHelper.AddSafe(RequestHelper.GetForm<string>("EvaluationTime"));
            EvaluateName.StartDate = StringHelper.AddSafe(RequestHelper.GetForm<string>("EvaluationStartTime"));
            EvaluateName.EndDate = StringHelper.AddSafe(RequestHelper.GetForm<string>("EvaluationEndTime"));
            EvaluateName.CompanyId = base.UserCompanyID;
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (EvaluateName.ID > 0)
            {
                base.CheckUserPower("UpdateEvaluateName", PowerCheckType.Single);
                EvaluateNameBLL.UpdateEvaluateName(EvaluateName);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("EvaluateName"), EvaluateName.ID);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            else
            {
                base.CheckUserPower("AddEvaluateName", PowerCheckType.Single);
                int id = EvaluateNameBLL.AddEvaluateName(EvaluateName);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("EvaluateName"), id);
            }
            if (string.IsNullOrEmpty(ReturnUrl))
                ReturnUrl = RequestHelper.RawUrl;
            ScriptHelper.Alert(alertMessage, ReturnUrl);
        }
    }
}
