using System;

namespace XueFuShop.Models
{
    /// <summary>
    /// 实体类MXComment。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public class Comment
    {
        #region CommentModel

        /// <summary>
        /// 
        /// </summary>
        private int _CommentId;
        public int CommentId
        {
            set { _CommentId = value; }
            get { return _CommentId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _CommentTitle;
        public string CommentTitle
        {
            set { _CommentTitle = value; }
            get { return _CommentTitle; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _CommentContent;
        public string CommentContent
        {
            set { _CommentContent = value; }
            get { return _CommentContent; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _ProductId;
        public int ProductId
        {
            set { _ProductId = value; }
            get { return _ProductId; }
        }

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
        private DateTime _CreateDate;
        public DateTime CreateDate
        {
            set { _CreateDate = value; }
            get { return _CreateDate; }
        }

        #endregion CommentModel
    }
}
