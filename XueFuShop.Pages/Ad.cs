using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class Ad : CommonBasePage
    {
        
        protected override void PageLoad()
        {
            base.PageLoad();
            string queryString = RequestHelper.GetQueryString<string>("URL");
            AdRecordInfo adRecord = new AdRecordInfo();
            adRecord.AdID = RequestHelper.GetQueryString<int>("AdID");
            adRecord.IP = ClientHelper.IP;
            adRecord.Date = RequestHelper.DateNow;
            adRecord.Page = base.Request.ServerVariables["HTTP_REFERER"];
            adRecord.UserID = base.UserID;
            adRecord.UserName = base.UserName;
            AdRecordBLL.AddAdRecord(adRecord);
            ResponseHelper.Redirect(queryString);
        }
    }
}
