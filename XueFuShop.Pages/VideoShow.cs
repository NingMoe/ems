using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;

namespace XueFuShop.Pages
{
    public class VideoShow : UserCommonBasePage
    {
        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "视频播放";
            //从Cookies中读取验证码并解密
            if (StringHelper.Decode(CookiesHelper.ReadCookieValue("SMSIsChecked"), "SMS") != "true")
                ResponseHelper.Redirect("/Verify.aspx?ID=" + RequestHelper.GetQueryString<int>("ID") + "&ReturnURL=" + RequestHelper.RawUrl);
        }
    }
}
