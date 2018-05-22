using System;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Pages;
using YXTSMS;

namespace MobileEMS
{
    public partial class SMSCheck : MobileUserBasePage
    {
        protected int productID = RequestHelper.GetQueryString<int>("CateId");

        protected void Page_Load(object sender, EventArgs e)
        {
            bool isVerify = false;
            if (StringHelper.Decode(CookiesHelper.ReadCookieValue("SMSIsChecked"), "SMS") == "true" && !string.IsNullOrEmpty(StringHelper.Decode(CookiesHelper.ReadCookieValue("SMSCheckCode"), "SMS")))
                isVerify = true;

            //if (!isVerify && base.UserID > 0)
            //{
            //    if (!string.IsNullOrEmpty(base.UserMobile))
            //    {
            //        SMSRecordInfo smsRecord = SMSRecordBLL.ReadSMSRecord(base.UserMobile);
            //        if (smsRecord != null)
            //        {
            //            if ((DateTime.Now - smsRecord.DataCreateDate).TotalSeconds <= SMSConfig.CodeTimeOut * 60)
            //            {
            //                isVerify = true;
            //                CookiesHelper.AddCookie("SMSIsChecked", StringHelper.Encode("true", "SMS"), (SMSConfig.CodeTimeOut - (int)(DateTime.Now - smsRecord.DataCreateDate).TotalMinutes), TimeType.Minute);
            //            }
            //        }
            //    }
            //}
            if (isVerify)
                ResponseHelper.Redirect("CourseVideo.aspx?CateId=" + productID.ToString());
        }
    }
}
