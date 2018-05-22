using System;
using System.Collections.Generic;
using System.Text;

namespace XueFuShop.Models
{
    public sealed class UserSearchInfo
    {
        private string email;
        private DateTime endRegisterDate;
        private string inUserID;
        private string outUserID;
        private int sex;
        private DateTime startRegisterDate;
        private int status;
        private int statusNoEqual;
        private string inStatus;
        private string userName;
        private string realName;
        private string equalRealName;
        private int groupId;
        private string inGroupID;
        private string inWorkingPostID;
        private string inStudyPostID;
        private string inCompanyID;
        private string mobile;
        private int del;
        private string condition;

        public UserSearchInfo()
        {
            this.userName = string.Empty;
            this.email = string.Empty;
            this.sex = -2147483648;
            this.startRegisterDate = DateTime.MinValue;
            this.endRegisterDate = DateTime.MinValue;
            this.status = -2147483648;
            this.statusNoEqual = int.MinValue;
            this.inStatus = string.Empty;
            this.inUserID = string.Empty;
            this.outUserID = string.Empty;
            this.groupId = int.MinValue;
            this.inGroupID = string.Empty;
            this.inStudyPostID = string.Empty;
            this.inWorkingPostID = string.Empty;
            this.inCompanyID = string.Empty;
            this.realName = string.Empty;
            this.equalRealName = string.Empty;
            this.mobile = string.Empty;
            this.del = 0;
            this.condition = string.Empty;
        }

        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }

        public DateTime EndRegisterDate
        {
            get { return this.endRegisterDate; }
            set { this.endRegisterDate = value; }
        }

        public string InUserID
        {
            get { return this.inUserID; }
            set { this.inUserID = value; }
        }

        public string OutUserID
        {
            get { return this.outUserID; }
            set { this.outUserID = value; }
        }

        public int Sex
        {
            get { return this.sex; }
            set { this.sex = value; }
        }

        public DateTime StartRegisterDate
        {
            get { return this.startRegisterDate; }
            set { this.startRegisterDate = value; }
        }

        public int Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        public int StatusNoEqual
        {
            get { return this.statusNoEqual; }
            set { this.statusNoEqual = value; }
        }

        public string InStatus
        {
            get { return this.inStatus; }
            set { this.inStatus = value; }
        }

        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        public string RealName
        {
            get { return this.realName; }
            set { this.realName = value; }
        }

        public string EqualRealName
        {
            get { return this.equalRealName; }
            set { this.equalRealName = value; }
        }

        public string InCompanyID
        {
            get { return this.inCompanyID; }
            set { this.inCompanyID = value; }
        }

        public int GroupId
        {
            get { return this.groupId; }
            set { this.groupId = value; }
        }

        public string InGroupID
        {
            get { return this.inGroupID; }
            set { this.inGroupID = value; }
        }

        public string InWorkingPostID
        {
            get { return this.inWorkingPostID; }
            set { this.inWorkingPostID = value; }
        }

        public string InStudyPostID
        {
            get { return this.inStudyPostID; }
            set { this.inStudyPostID = value; }
        }

        public string Mobile
        {
            get { return this.mobile; }
            set { this.mobile = value; }
        }
        
        public int Del
        {
            get { return this.del; }
            set { this.del = value; }
        }

        /// <summary>
        /// ²éÑ¯Ìõ¼þ
        /// </summary>
        public string Condition
        {
            get { return this.condition; }
            set { this.condition = value; }
        }
    }
}
