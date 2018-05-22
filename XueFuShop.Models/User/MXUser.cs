using System;
namespace XueFuShop.Models
{
    /// <summary>
    /// 实体类MXUser。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public class MXUser
    {
        #region MXUserModel

        /// <summary>
        /// 
        /// </summary>
        private int _UserId;
        public int UserId
        {
            set { _UserId = value; }
            get { return _UserId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserName;
        public string UserName
        {
            set { _UserName = value; }
            get { return _UserName; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserPwd;
        public string UserPwd
        {
            set { _UserPwd = value; }
            get { return _UserPwd; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserRealName;
        public string UserRealName
        {
            set { _UserRealName = value; }
            get { return _UserRealName; }
        }


        /// <summary>
        /// 
        /// </summary>
        private int _UserSex;
        public int UserSex
        {
            set { _UserSex = value; }
            get { return _UserSex; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserBirthday;
        public string UserBirthday
        {
            set { _UserBirthday = value; }
            get { return _UserBirthday; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserEmail;
        public string UserEmail
        {
            set { _UserEmail = value; }
            get { return _UserEmail; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserTel;
        public string UserTel
        {
            set { _UserTel = value; }
            get { return _UserTel; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserMobile;
        public string UserMobile
        {
            set { _UserMobile = value; }
            get { return _UserMobile; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserQQ;
        public string UserQQ
        {
            set { _UserQQ = value; }
            get { return _UserQQ; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserMsn;
        public string UserMsn
        {
            set { _UserMsn = value; }
            get { return _UserMsn; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserCompany;
        public string UserCompany
        {
            set { _UserCompany = value; }
            get { return _UserCompany; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserPosition;
        public string UserPosition
        {
            set { _UserPosition = value; }
            get { return _UserPosition; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserPost;
        public string UserPost
        {
            set { _UserPost = value; }
            get { return _UserPost; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserAddress;
        public string UserAddress
        {
            set { _UserAddress = value; }
            get { return _UserAddress; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserCompanyAddress;
        public string UserCompanyAddress
        {
            set { _UserCompanyAddress = value; }
            get { return _UserCompanyAddress; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _IsChecked;
        public int IsChecked
        {
            set { _IsChecked = value; }
            get { return _IsChecked; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _IsLocked;
        public int IsLocked
        {
            set { _IsLocked = value; }
            get { return _IsLocked; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _RegDate;
        public string RegDate
        {
            set { _RegDate = value; }
            get { return _RegDate; }
        }

        /// <summary>
        /// 
        /// </summary>
        private DateTime _LastLoginDate;
        public DateTime LastLoginDate
        {
            set { _LastLoginDate = value; }
            get { return _LastLoginDate; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _State;
        public int State
        {
            set { _State = value; }
            get { return _State; }
        }

        #endregion MXUserModel
    }

    public sealed class UserSearchInfo
    {
        
        private string email;
        private DateTime endRegisterDate;
        private string inUserID;
        private int sex;
        private DateTime startRegisterDate;
        private int status;
        private string userName;

        
        public UserSearchInfo()
        {
            this.userName = string.Empty;
            this.email = string.Empty;
            this.sex = -2147483648;
            this.startRegisterDate = DateTime.MinValue;
            this.endRegisterDate = DateTime.MinValue;
            this.status = -2147483648;
            this.inUserID = string.Empty;
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

        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }
    }
}
