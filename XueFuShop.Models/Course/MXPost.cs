using System;

namespace XueFuShop.Models.Course
{
    /// <summary>
    /// MXPost 的摘要说明
    /// </summary>
    public class MXPost
    {
        #region MXPost

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
        private string _PostName;
        public string PostName
        {
            set { _PostName = value; }
            get { return _PostName; }
        }

        /// <summary>
        /// 岗位计划
        /// </summary>
        private string _PostPlan;
        public string PostPlan
        {
            set { _PostPlan = value; }
            get { return _PostPlan; }
        }

        /// <summary>
        /// 锁定
        /// </summary>
        private bool _Locked;
        public bool Locked
        {
            set { _Locked = value; }
            get { return _Locked; }
        }

        #endregion MXPost
    }
}
