using System;

namespace XueFuShop.Models.Course
{
    /// <summary>
    /// MXCourse 的摘要说明
    /// </summary>
    public class MXCourse
    {

        #region MXCourseModel

        /// <summary>
        /// 
        /// </summary>
        private int _CourseId;
        public int CourseId
        {
            set { _CourseId = value; }
            get { return _CourseId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _CourseName;
        public string CourseName
        {
            set { _CourseName = value; }
            get { return _CourseName; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _CourseScore;
        public int CourseScore
        {
            set { _CourseScore = value; }
            get { return _CourseScore; }
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
        private int _OrderIndex;
        public int OrderIndex
        {
            set { _OrderIndex = value; }
            get { return _OrderIndex; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _CreateDate;
        public int CreateDate
        {
            set { _CreateDate = value; }
            get { return _CreateDate; }
        }

        #endregion MXCourseModel
    }
}
