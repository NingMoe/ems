using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;


namespace XueFuShop.IDAL
{
    public interface ISendMessage
    {
        int AddSendMessage(SendMessageInfo sendMessage);
        void DeleteSendMessage(string strID, int userID);
        SendMessageInfo ReadSendMessage(int id, int userID);
        string ReadSendMessageIDList(string strID, int userID);
        List<SendMessageInfo> SearchSendMessageList(SendMessageSearchInfo sendMessage);
        List<SendMessageInfo> SearchSendMessageList(int currentPage, int pageSize, SendMessageSearchInfo sendMessage, ref int count);
        void UpdateSendMessage(SendMessageInfo sendMessage);
    }
}
