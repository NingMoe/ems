using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class ReadMessage : UserBasePage
    {
        
        protected ReceiveMessageInfo receiveMessage = new ReceiveMessageInfo();

        
        protected override void PageLoad()
        {
            base.PageLoad();
            int queryString = RequestHelper.GetQueryString<int>("ID");
            this.receiveMessage = ReceiveMessageBLL.ReadReceiveMessage(queryString, base.UserID);
            if ((this.receiveMessage.ID > 0) && (this.receiveMessage.IsRead == 0))
            {
                this.receiveMessage.IsRead = 1;
                ReceiveMessageBLL.UpdateReceiveMessage(this.receiveMessage);
            }
        }

        protected override void PostBack()
        {
            int form = RequestHelper.GetForm<int>("ID");
            this.receiveMessage = ReceiveMessageBLL.ReadReceiveMessage(form, base.UserID);
            if ((this.receiveMessage.ID > 0) && (this.receiveMessage.IsAdmin == 0))
            {
                SendMessageInfo sendMessage = new SendMessageInfo();
                sendMessage.Title = "�ظ���" + this.receiveMessage.Title;
                sendMessage.Content = StringHelper.AddSafe(RequestHelper.GetForm<string>("Content"));
                sendMessage.Date = RequestHelper.DateNow;
                sendMessage.ToUserID = this.receiveMessage.FromUserID.ToString();
                sendMessage.ToUserName = this.receiveMessage.FromUserName;
                sendMessage.UserID = base.UserID;
                sendMessage.UserName = base.UserName;
                sendMessage.IsAdmin = 0;
                SendMessageBLL.AddSendMessage(sendMessage);
                ReceiveMessageInfo receiveMessage = new ReceiveMessageInfo();
                this.receiveMessage.ID = RequestHelper.GetQueryString<int>("ID");
                receiveMessage.Title = sendMessage.Title;
                receiveMessage.Content = sendMessage.Content;
                receiveMessage.Date = sendMessage.Date;
                receiveMessage.IsRead = 0;
                receiveMessage.IsAdmin = 0;
                receiveMessage.FromUserID = base.UserID;
                receiveMessage.FromUserName = base.UserName;
                receiveMessage.UserID = this.receiveMessage.FromUserID;
                receiveMessage.UserName = this.receiveMessage.FromUserName;
                ReceiveMessageBLL.AddReceiveMessage(receiveMessage);
                ScriptHelper.Alert("�ظ��ɹ�", "/User/ReadMessage.aspx?ID=" + form);
            }
            else
            {
                ScriptHelper.Alert("���ִ���", "/User/ReadMessage.aspx?ID=" + form);
            }
        }
    }

 

}
