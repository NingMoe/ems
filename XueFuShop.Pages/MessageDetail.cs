using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class MessageDetail : UserBasePage
    {
        
        protected SendMessageInfo sendMessage = new SendMessageInfo();

        
        protected override void PageLoad()
        {
            base.PageLoad();
            int queryString = RequestHelper.GetQueryString<int>("ID");
            this.sendMessage = SendMessageBLL.ReadSendMessage(queryString, base.UserID);
        }
    }

 

}
