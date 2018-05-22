using System;
using System.Collections.Generic;
using System.Text;

namespace XueFuShop.DAL
{
    public sealed class PostSearchInfo
    {
        #region Post

        /// <summary>
        /// 岗位标识
        /// </summary>
        private int _PostId;
        public int PostId
        {
            set { _PostId = value; }
            get { return _PostId; }
        }

        /// <summary>
        /// 岗位名称
        /// </summary>
        private string _PostName = string.Empty;
        public string PostName
        {
            set { _PostName = value; }
            get { return _PostName; }
        }


        /// <summary>
        /// 岗位计划
        /// </summary>
        private string _PostPlan = string.Empty;
        public string PostPlan
        {
            set { _PostPlan = value; }
            get { return _PostPlan; }
        }

        /// <summary>
        /// 岗位层级关系ID
        /// </summary>
        private int _ParentId = int.MinValue;
        public int ParentId
        {
            set { _ParentId = value; }
            get { return _ParentId; }
        }

        /// <summary>
        /// 锁定
        /// </summary>
        private int _Locked = int.MinValue;
        public int Locked
        {
            set { _Locked = value; }
            get { return _Locked; }
        }

        /// <summary>
        /// 作为岗位
        /// </summary>
        private int _IsPost = int.MinValue;
        public int IsPost
        {
            set { _IsPost = value; }
            get { return _IsPost; }
        }

        /// <summary>
        /// 隶属公司
        /// </summary>
        private int _CompanyID = int.MinValue;
        public int CompanyID
        {
            set { _CompanyID = value; }
            get { return _CompanyID; }
        }

        private string _InCompanyID = string.Empty;
        public string InCompanyID
        {
            set { _InCompanyID = value; }
            get { return _InCompanyID; }
        }

        #endregion Post
    }
}
