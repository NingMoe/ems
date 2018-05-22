using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class UserMessageAjax : AjaxBasePage
    {
        protected CommonPagerClass commonPagerClass = new CommonPagerClass();
        protected List<UserMessageInfo> userMessageList = new List<UserMessageInfo>();
        
        protected void AddUserMessage()
        {
            string resultData = "{";
            int ID = RequestHelper.GetQueryString<int>("ID");
            string title = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Title"));
            string content = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Content"));
            int messageClass = RequestHelper.GetQueryString<int>("MessageClass");
            string name = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Name"));
            string mobile = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("Mobile"));
            if ((mobile == string.Empty) || (content == string.Empty))
            {
                resultData += "\"result\":\"false\",\"error\":\"请填写手机和内容\"";
            }
            else
            {
                UserMessageInfo userMessage = new UserMessageInfo();
                userMessage.MessageClass = messageClass;
                userMessage.Title = title;
                userMessage.Content = content;
                userMessage.UserIP = ClientHelper.IP;
                userMessage.PostDate = RequestHelper.DateNow;
                userMessage.IsHandler = 0;
                userMessage.AdminReplyContent = string.Empty;
                userMessage.AdminReplyDate = RequestHelper.DateNow;
                userMessage.UserID = Cookies.User.GetUserID(false);
                userMessage.UserName = name;
                if (ID > 0) userMessage.ParentID = ID;
                userMessage.Mobile = mobile;
                UserMessageBLL.AddUserMessage(userMessage);
                resultData += "\"result\":\"true\",\"error\":\"\"";
            }
            resultData += "}";
            ResponseHelper.Write(resultData);
            ResponseHelper.End();
        }

        protected override void PageLoad()
        {
            base.PageLoad();
            if (RequestHelper.GetQueryString<string>("Action") == "AddUserMessage")
            {
                this.AddUserMessage();
            }
            int queryString = RequestHelper.GetQueryString<int>("Page");
            if (queryString < 1)
            {
                queryString = 1;
            }
            int pageSize = 3;
            int count = 0;
            UserMessageSeachInfo userMessage = new UserMessageSeachInfo();
            //userMessage.UserID = base.UserID;
            userMessage.IsChecked = 1;
            this.userMessageList = UserMessageBLL.SearchUserMessageList(queryString, pageSize, userMessage, ref count);
            this.commonPagerClass.URL = "javascript:readUserMessage($Page);";
            this.commonPagerClass.CurrentPage = queryString;
            this.commonPagerClass.PageSize = pageSize;
            this.commonPagerClass.Count = count;
        }
    }

 

}
