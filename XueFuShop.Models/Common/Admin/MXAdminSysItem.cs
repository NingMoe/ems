using System;

namespace XueFuShop.Models
{
    /// <summary>
    /// 实体类MXItem。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public class MXAdminSysItem
    {
        #region MXAdminSysItemModel

        /// <summary>
        /// 系统菜单 标识
        /// </summary>
        private int _ItemId;
        public int ItemId
        {
            set { _ItemId = value; }
            get { return _ItemId; }
        }

        /// <summary>
        /// 系统菜单 名称
        /// </summary>
        private string _ItemName;
        public string ItemName
        {
            set { _ItemName = value; }
            get { return _ItemName; }
        }

        /// <summary>
        /// 系统菜单 父标识
        /// </summary>
        private int _ParentItemId;
        public int ParentItemId
        {
            set { _ParentItemId = value; }
            get { return _ParentItemId; }
        }


        /// <summary>
        /// 排序号码
        /// </summary>
        private int _OrderIndex;
        public int OrderIndex
        {
            set { _OrderIndex = value; }
            get { return _OrderIndex; }
        }

        /// <summary>
        /// 图标地址
        /// </summary>
        private string _ItemImage;
        public string ItemImage
        {
            set { _ItemImage = value; }
            get { return _ItemImage; }
        }

        /// <summary>
        /// 指向URL
        /// </summary>
        private string _ItemURL;
        public string ItemURL
        {
            set { _ItemURL = value; }
            get { return _ItemURL; }
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        private string _CreateDate;
        public string CreateDate
        {
            set { _CreateDate = value; }
            get { return _CreateDate; }
        }

        #endregion MXAdminSysItemModel
    }
}
