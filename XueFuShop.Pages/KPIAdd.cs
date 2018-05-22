using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Pages
{
    public class KPIAdd : UserManageBasePage
    {
        protected List<KPIInfo> KPIClassList = new List<KPIInfo>();
        protected int CompanyID = RequestHelper.GetQueryString<int>("CompanyID");
        protected int ClassID = RequestHelper.GetQueryString<int>("ClassId");
        protected List<EnumInfo> KPITypeList = new List<EnumInfo>();
        protected KPIInfo KPI = new KPIInfo();
        protected int ID = RequestHelper.GetQueryString<int>("ID");

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "Ìí¼ÓKPIÖ¸±ê";

            base.CheckUserPower("ReadKPI,AddKPI,UpdateKPI", PowerCheckType.OR);

            KPISearchInfo kpiSearch = new KPISearchInfo();
            kpiSearch.ParentId = "0";
            KPIClassList = KPIBLL.SearchKPIList(kpiSearch);

            KPITypeList = EnumHelper.ReadEnumList<KPIType>();

            if (ID > 0)
            {
                KPI = KPIBLL.ReadKPI(ID);
                CompanyID = KPI.CompanyID;
                ClassID = KPI.ParentId;
            }
        }

        protected override void PostBack()
        {
            KPIInfo kpiClass = new KPIInfo();
            kpiClass.ID = ID;
            kpiClass.CompanyID = RequestHelper.GetForm<int>("CompanyID");
            kpiClass.ParentId = RequestHelper.GetForm<int>("ClassID");
            kpiClass.Name = StringHelper.AddSafe(RequestHelper.GetForm<string>("Name"));
            kpiClass.EvaluateInfo = StringHelper.AddSafe(RequestHelper.GetForm<string>("Introduction"));
            kpiClass.Method = StringHelper.AddSafe(RequestHelper.GetForm<string>("Method"));
            kpiClass.Type = (KPIType)RequestHelper.GetForm<int>("Type");
            int score = RequestHelper.GetForm<int>("Score");
            if (score > 0) kpiClass.Scorse = (float)score;
            else kpiClass.Scorse = 0;
            kpiClass.Sort = RequestHelper.GetForm<int>("Sort");

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (kpiClass.ID == int.MinValue)
            {
                base.CheckUserPower("AddKPI", PowerCheckType.Single);
                int id = KPIBLL.AddKPI(kpiClass);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("KPI"), id);
            }
            else
            {
                base.CheckUserPower("UpdateKPI", PowerCheckType.Single);
                KPIBLL.UpdateKPI(kpiClass);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("KPI"), kpiClass.ID);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            string returnURL = ServerHelper.UrlDecode(RequestHelper.GetQueryString<string>("ReturnURL"));
            if (string.IsNullOrEmpty(returnURL))
                ScriptHelper.Alert(alertMessage, "/User/KPIAdd.aspx?CompanyID=" + kpiClass.CompanyID.ToString() + "&ClassID=" + kpiClass.ParentId.ToString());
            else
                ScriptHelper.Alert(alertMessage, returnURL);
        }
    }
}
