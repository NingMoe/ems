using System;

namespace XueFuShop.Models.Course
{
    /// <summary>
    /// MXCourseProcess 的摘要说明
    /// </summary>
    public class MXCourseProcess
    {
        #region MXCourseProcess

        /// <summary>
        /// 岗位标识
        /// </summary>
        private int _CourseProcessId;
        public int CourseProcessId
        {
            set { _CourseProcessId = value; }
            get { return _CourseProcessId; }
        }

        /// <summary>
        /// 课程号
        /// </summary>
        private int _CourseId;
        public int CourseId
        {
            set { _CourseId = value; }
            get { return _CourseId; }
        }

        /// <summary>
        /// 用户号
        /// </summary>
        private int _UserId;
        public int UserId
        {
            set { _UserId = value; }
            get { return _UserId; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        private int _State;
        public int State
        {
            set { _State = value; }
            get { return _State; }
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

        #endregion MXCourseProcess
    }
}
