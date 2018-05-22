using System;
using System.Collections.Generic;
using System.Text;

namespace XueFuShop.Models
{
    public class RenZhengCateInfo
    {

        /// <summary>
        /// 
        /// </summary>
        private int _Id;
        public int Id
        {
            set { _Id = value; }
            get { return _Id; }
        }

        /// <summary>
        /// 认证课程
        /// </summary>
        private int _CateId;
        public int CateId
        {
            set { _CateId = value; }
            get { return _CateId; }
        }

        /// <summary>
        /// 岗位
        /// </summary>
        private int _PostId;
        public int PostId
        {
            set { _PostId = value; }
            get { return _PostId; }
        }

        /// <summary>
        /// 岗位
        /// </summary>
        private string _inPostID;
        public string InPostID
        {
            set { _inPostID = value; }
            get { return _inPostID; }
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        private int _Del;
        public int Del
        {
            set { _Del = value; }
            get { return _Del; }
        }

        /// <summary>
        /// 排序
        /// </summary>
        private int _Order;
        public int Order
        {
            set { _Order = value; }
            get { return _Order; }
        }
    }
}
