using System;
using System.Collections.Generic;
using System.Text;

namespace XueFuShop.Models
{
    public sealed class SendMessageSearchInfo
    {
        private int isAdmin = -2147483648;
        private int userID = -2147483648;

        public int IsAdmin
        {
            get
            {
                return this.isAdmin;
            }
            set
            {
                this.isAdmin = value;
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
    }
}
