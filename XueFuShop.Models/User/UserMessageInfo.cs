using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace XueFuShop.Models
{
    public sealed class UserMessageInfo
    {
        private string adminReplyContent;
        private DateTime adminReplyDate;
        private string content;
        private int id;
        private int isHandler;
        private int messageClass;
        private DateTime postDate;
        private string title;
        private int userID;
        private string userIP;
        private string userName;
        private int parentID = 0;
        private string mobile = string.Empty;
        private int isChecked = int.MinValue;

        public UserMessageInfo()
        { }

        public string AdminReplyContent 
        {
            set { adminReplyContent = value; }
            get { return adminReplyContent; }
        }
        public DateTime AdminReplyDate
        {
            get { return adminReplyDate; }
            set { adminReplyDate = value; }
        }
        public string Content 
        {
            get { return content; }
            set { content = value; }
        }
        public int ID 
        {
            get { return id; }
            set { id = value; }
        }
        public int IsHandler 
        {
            get { return isHandler; }
            set { isHandler = value; }
        }
        public int MessageClass 
        {
            get { return messageClass; }
            set { messageClass = value; }
        }
        public DateTime PostDate 
        {
            get { return postDate; }
            set { postDate = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public int UserID 
        {
            get { return userID; }
            set { userID = value; }
        }
        public string UserIP 
        {
            get { return userIP; }
            set { userIP = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }
        /// <summary>
        /// …Û∫À
        /// </summary>
        public int IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }
        
        /// <summary>
        /// ÷˜Ã‚ID
        /// </summary>
        public int ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }
    }
}
