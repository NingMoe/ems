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
    /// MXUserCourse ��ժҪ˵��
    /// </summary>
    public class MXUserCourse
    {
        #region MXUserCourse

        /// <summary>
        /// �û��γ̱�ʶ
        /// </summary>
        private int _UserCourseId;
        public int UserCourseId
        {
            set { _UserCourseId = value; }
            get { return _UserCourseId; }
        }

        /// <summary>
        /// �û���
        /// </summary>
        private int _UserId;
        public int UserId
        {
            set { _UserId = value; }
            get { return _UserId; }
        }

        /// <summary>
        /// ѵ������
        /// </summary>
        private string _TrainingName;
        public string TrainingName
        {
            set { _TrainingName = value; }
            get { return _TrainingName; }
        }

        /// <summary>
        /// �û��γ�
        /// </summary>
        private string _UserCourse;
        public string UserCourse
        {
            set { _UserCourse = value; }
            get { return _UserCourse; }
        }

        /// <summary>
        /// ����
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
