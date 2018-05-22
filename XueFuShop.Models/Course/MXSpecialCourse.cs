using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace XueFuShop.Models.Course
{
    /// <summary>
    /// MXQuestion 的摘要说明
    /// </summary>
    public class MXSpecialCourse
    {
        #region MXSpecialCourseModel

        /// <summary>
        /// 
        /// </summary>
        private int _SpecialCourseId;
        public int SpecialCourseId
        {
            set { _SpecialCourseId = value; }
            get { return _SpecialCourseId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _CompanyId;
        public string CompanyId
        {
            set { _CompanyId = value; }
            get { return _CompanyId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _CourseContent;
        public string CourseContent
        {
            set { _CourseContent = value; }
            get { return _CourseContent; }
        }

        /// <summary>
        /// 
        /// </summary>
        private float _StartDate;
        public float StartDate
        {
            set { _StartDate = value; }
            get { return _StartDate; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _EndDate;
        public string EndDate
        {
            set { _EndDate = value; }
            get { return _EndDate; }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _Locked;
        public bool Locked
        {
            set { _Locked = value; }
            get { return _Locked; }
        }

        #endregion MXSpecialCourseModel
    }
}
