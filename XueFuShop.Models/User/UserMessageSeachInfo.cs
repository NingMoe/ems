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
    public sealed class UserMessageSeachInfo
    {
        private DateTime endPostDate = DateTime.MinValue;
        private int isHandler = -2147483648;
        private int messageClass = -2147483648;
        private DateTime startPostDate = DateTime.MinValue;
        private string title = string.Empty;
        private int userID = -2147483648;
        private string userName = string.Empty;
        private int parentID = int.MinValue;
        private string mobile = string.Empty;
        private int isChecked = int.MinValue;

        public DateTime EndPostDate
        {
            get
            {
                return this.endPostDate;
            }
            set
            {
                this.endPostDate = value;
            }
        }

        public int IsHandler
        {
            get
            {
                return this.isHandler;
            }
            set
            {
                this.isHandler = value;
            }
        }

        public int MessageClass
        {
            get
            {
                return this.messageClass;
            }
            set
            {
                this.messageClass = value;
            }
        }

        public DateTime StartPostDate
        {
            get
            {
                return this.startPostDate;
            }
            set
            {
                this.startPostDate = value;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        public int UserID
        {
            get
            {
                return this.userID;
            }
            set
            {
                this.userID = value;
            }
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
            }
        }

        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }
        /// <summary>
        /// Ö÷ÌâID
        /// </summary>
        public int ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        public int IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }
    }
}
