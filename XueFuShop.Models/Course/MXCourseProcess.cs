using System;

namespace XueFuShop.Models.Course
{
    /// <summary>
    /// MXCourseProcess ��ժҪ˵��
    /// </summary>
    public class MXCourseProcess
    {
        #region MXCourseProcess

        /// <summary>
        /// ��λ��ʶ
        /// </summary>
        private int _CourseProcessId;
        public int CourseProcessId
        {
            set { _CourseProcessId = value; }
            get { return _CourseProcessId; }
        }

        /// <summary>
        /// �γ̺�
        /// </summary>
        private int _CourseId;
        public int CourseId
        {
            set { _CourseId = value; }
            get { return _CourseId; }
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
        /// ״̬
        /// </summary>
        private int _State;
        public int State
        {
            set { _State = value; }
            get { return _State; }
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

        #endregion MXCourseProcess
    }
}
