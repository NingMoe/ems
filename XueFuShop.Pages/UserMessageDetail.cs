using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class UserMessageDetail : UserBasePage
    {
        
        protected UserMessageInfo userMessage = new UserMessageInfo();

        
        protected override void PageLoad()
        {
            base.PageLoad();
            int queryString = RequestHelper.GetQueryString<int>("ID");
            this.userMessage = UserMessageBLL.ReadUserMessage(queryString, base.UserID);
        }
    }

 

}
