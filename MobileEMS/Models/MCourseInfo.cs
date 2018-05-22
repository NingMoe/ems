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
    public sealed class MCourseInfo
    {
        #region MCourseInfo

        /// <summary>
        /// 
        /// </summary>
        private string _ClassID;
        public string ClassID
        {
            set { _ClassID = value; }
            get { return _ClassID; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Title;
        public string Title
        {
            set { _Title = value; }
            get { return _Title; }
        }
        /// <summary>
        /// 
        /// </summary>
        private string _OriginalPrice;
        public string OriginalPrice
        {
            set { _OriginalPrice = value; }
            get { return _OriginalPrice; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _ActicePrice;
        public string ActicePrice
        {
            set { _ActicePrice = value; }
            get { return _ActicePrice; }
        }


        /// <summary>
        /// 
        /// </summary>
        private string _ValidDateShow;
        public string ValidDateShow
        {
            set { _ValidDateShow = value; }
            get { return _ValidDateShow; }
        }
        /// <summary>
        /// 
        /// </summary>
        private string _Period;
        public string Period
        {
            set { _Period = value; }
            get { return _Period; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _SaleEventType;
        public int SaleEventType
        {
            set { _SaleEventType = value; }
            get { return _SaleEventType; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _IsGiveXB;
        public int IsGiveXB
        {
            set { _IsGiveXB = value; }
            get { return _IsGiveXB; }
        }
        /// <summary>
        /// 
        /// </summary>
        private string _IconUrl;
        public string IconUrl
        {
            set { _IconUrl = value; }
            get { return _IconUrl; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _PageCount;
        public int PageCount
        {
            set { _PageCount = value; }
            get { return _PageCount; }
        }

        /// <summary>
        /// 考试次数
        /// </summary>
        private int _TestCount;
        public int TestCount
        {
            set { _TestCount = value; }
            get { return _TestCount; }
        }

        /// <summary>
        /// 是否通过
        /// </summary>
        private bool _IsPass;
        public bool IsPass
        {
            set { _IsPass = value; }
            get { return _IsPass; }
        }

        /// <summary>
        /// 是否有视频
        /// </summary>
        private bool _IsVideo = false;
        public bool IsVideo
        {
            set { _IsVideo = value; }
            get { return _IsVideo; }
        }

        /// <summary>
        /// 是否绕车剧本
        /// </summary>
        private bool _IsRC = false;
        public bool IsRC
        {
            set { _IsRC = value; }
            get { return _IsRC; }
        }

        /// <summary>
        /// 绕车剧本地址
        /// </summary>
        private string _RCUrl;
        public string RCUrl
        {
            set { _RCUrl = value; }
            get { return _RCUrl; }
        }

        /// <summary>
        /// 是否可以考试
        /// </summary>
        private bool _IsTest = false;
        public bool IsTest
        {
            set { _IsTest = value; }
            get { return _IsTest; }
        }

        /// <summary>
        /// 是否为岗位课程
        /// </summary>
        private bool _IsPostCourse = false;
        public bool IsPostCourse
        {
            set { _IsPostCourse = value; }
            get { return _IsPostCourse; }
        }

        /// <summary>
        /// 是否在购买有效期内
        /// </summary>
        private bool _IsPrepaidCourse = false;
        public bool IsPrepaidCourse
        {
            set { _IsPrepaidCourse = value; }
            get { return _IsPrepaidCourse; }
        }

        #endregion MCourseInfo
    }
}
