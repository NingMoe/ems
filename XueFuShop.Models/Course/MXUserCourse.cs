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
    /// MXUserCourse 的摘要说明
    /// </summary>
    public class MXUserCourse
    {
        #region MXUserCourse

        /// <summary>
        /// 用户课程标识
        /// </summary>
        private int _UserCourseId;
        public int UserCourseId
        {
            set { _UserCourseId = value; }
            get { return _UserCourseId; }
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
        /// 训练名称
        /// </summary>
        private string _TrainingName;
        public string TrainingName
        {
            set { _TrainingName = value; }
            get { return _TrainingName; }
        }

        /// <summary>
        /// 用户课程
        /// </summary>
        private string _UserCourse;
        public string UserCourse
        {
            set { _UserCourse = value; }
            get { return _UserCourse; }
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

        #endregion MXUserCourse
    }
}
