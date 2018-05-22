using System;

namespace XueFuShop.Models.Course
{
    /// <summary>
    /// MXPost ��ժҪ˵��
    /// </summary>
    public class MXPost
    {
        #region MXPost

        /// <summary>
        /// ��λ��ʶ
        /// </summary>
        private int _PostId;
        public int PostId
        {
            set { _PostId = value; }
            get { return _PostId; }
        }

        /// <summary>
        /// ��λ����
        /// </summary>
        private string _PostName;
        public string PostName
        {
            set { _PostName = value; }
            get { return _PostName; }
        }

        /// <summary>
        /// ��λ�ƻ�
        /// </summary>
        private string _PostPlan;
        public string PostPlan
        {
            set { _PostPlan = value; }
            get { return _PostPlan; }
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

        #endregion MXPost
    }
}
