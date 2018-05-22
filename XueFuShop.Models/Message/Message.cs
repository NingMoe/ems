using System;

namespace XueFuShop.Models
{
    /// <summary>
    /// 实体类MXMessage。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public class Message
    {
        #region MXMessageModel

        /// <summary>
        /// 
        /// </summary>
        private int _MXMessageId;
        public int MessageId
        {
            set { _MXMessageId = value; }
            get { return _MXMessageId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _MessageTitle;
        public string MessageTitle
        {
            set { _MessageTitle = value; }
            get { return _MessageTitle; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _MessageContent;
        public string MessageContent
        {
            set { _MessageContent = value; }
            get { return _MessageContent; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _ItemId;
        public int ItemId
        {
            set { _ItemId = value; }
            get { return _ItemId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _ParentId;
        public int ParentId
        {
            set { _ParentId = value; }
            get { return _ParentId; }
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
        #endregion MXMessageModel
    }
}
