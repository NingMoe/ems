using System;

namespace XueFuShop.Models
{
    /// <summary>
    /// 实体类MXNews。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public class News
    {
        #region MXUserModel

        /// <summary>
        /// 
        /// </summary>
        private int _MXNewsId;
        public int NewsId
        {
            set { _MXNewsId = value; }
            get { return _MXNewsId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _NewsTitle;
        public string NewsTitle
        {
            set { _NewsTitle = value; }
            get { return _NewsTitle; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _CateId;
        public int CateId
        {
            set { _CateId = value; }
            get { return _CateId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _KeyWords;
        public string KeyWords
        {
            set { _KeyWords = value; }
            get { return _KeyWords; }
        }


        /// <summary>
        /// 
        /// </summary>
        private string _Author;
        public string Author
        {
            set { _Author = value; }
            get { return _Author; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _OverView;
        public string OverView
        {
            set { _OverView = value; }
            get { return _OverView; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _NewsContent;
        public string NewsContent
        {
            set { _NewsContent = value; }
            get { return _NewsContent; }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _IsLink;
        public bool IsLink
        {
            set { _IsLink = value; }
            get { return _IsLink; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _WebUrl;
        public string WebUrl
        {
            set { _WebUrl = value; }
            get { return _WebUrl; }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _IsPic;
        public bool IsPic
        {
            set { _IsPic = value; }
            get { return _IsPic; }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _IsTop;
        public bool IsTop
        {
            set { _IsTop = value; }
            get { return _IsTop; }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _IsBest;
        public bool IsBest
        {
            set { _IsBest = value; }
            get { return _IsBest; }
        }

        /// <summary>
        /// 
        /// </summary>
        private DateTime _CreateTime;
        public DateTime CreateTime
        {
            set { _CreateTime = value; }
            get { return _CreateTime; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _SmallPic;
        public string SmallPic
        {
            set { _SmallPic = value; }
            get { return _SmallPic; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _BigPic;
        public string BigPic
        {
            set { _BigPic = value; }
            get { return _BigPic; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _ClickNum;
        public int ClickNum
        {
            set { _ClickNum = value; }
            get { return _ClickNum; }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _IsChecked;
        public bool IsChecked
        {
            set { _IsChecked = value; }
            get { return _IsChecked; }
        }

        #endregion MXUserModel
    }
}
