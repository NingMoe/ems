using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;

namespace XueFuShop.Pages
{
    public class Report : UserCommonBasePage
    {
        protected int companyID = RequestHelper.GetQueryString<int>("CompanyID");

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "ÔÂ¶ÈÏê±í";

          
        }
    }
}
