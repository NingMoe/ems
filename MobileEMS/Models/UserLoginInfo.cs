using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MobileEMS.Models
{
    public sealed class UserLoginStateInfo
    {

        #region UserLoginStateInfo

        /// <summary>
        /// 
        /// </summary>
        private bool _IsSuccess;
        public bool IsSuccess
        {
            set { _IsSuccess = value; }
            get { return _IsSuccess; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _ErrorCode;
        public string ErrorCode
        {
            set { _ErrorCode = value; }
            get { return _ErrorCode; }
        }
        /// <summary>
        /// 
        /// </summary>
        private string _ErrorMessage;
        public string ErrorMessage
        {
            set { _ErrorMessage = value; }
            get { return _ErrorMessage; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Content;
        public string Content
        {
            set { _Content = value; }
            get { return _Content; }
        }

        #endregion UserLoginStateInfo
    }

    public sealed class UserLoginInfo
    {

        #region UserLoginInfo

        /// <summary>
        /// 
        /// </summary>
        private string _Favicon;
        public string Favicon
        {
            set { _Favicon = value; }
            get { return _Favicon; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _UserID;
        public int UserID
        {
            set { _UserID = value; }
            get { return _UserID; }
        }
        /// <summary>
        /// 
        /// </summary>
        private int _PunchCardSate;
        public int PunchCardSate
        {
            set { _PunchCardSate = value; }
            get { return _PunchCardSate; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserXB;
        public string UserXB
        {
            set { _UserXB = value; }
            get { return _UserXB; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _UserMark;
        public string UserMark
        {
            set { _UserMark = value; }
            get { return _UserMark; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _StudyedLessons;
        public string StudyedLessons
        {
            set { _StudyedLessons = value; }
            get { return _StudyedLessons; }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _IsHavePayOrder;
        public bool IsHavePayOrder
        {
            set { _IsHavePayOrder = value; }
            get { return _IsHavePayOrder; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _OpenClassState;
        public int OpenClassState
        {
            set { _OpenClassState = value; }
            get { return _OpenClassState; }
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

        private string _StudyPostName;
        public string StudyPostName
        {
            set { _StudyPostName = value; }
            get { return _StudyPostName; }
        }

        #endregion UserLoginInfo
    }
}
