using System;

namespace XueFuShop.Models
{
    /// <summary>
    /// Post ��ժҪ˵��
    /// </summary>
    [Serializable]
    public sealed class PostInfo
    {
        #region Post

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
        private string _PostName=string.Empty;
        public string PostName
        {
            set { _PostName = value; }
            get { return _PostName; }
        }

        /// <summary>
        /// ��λ�ƻ�
        /// </summary>
        private string _PostPlan=string.Empty;
        public string PostPlan
        {
            set { _PostPlan = value; }
            get { return _PostPlan; }
        }

        /// <summary>
        /// ��λ�㼶��ϵID
        /// </summary>
        private int _ParentId=int.MinValue;
        public int ParentId
        {
            set { _ParentId = value; }
            get { return _ParentId; }
        }

        /// <summary>
        /// ��λ����
        /// </summary>
        private int _OrderIndex=0;
        public int OrderIndex
        {
            set { _OrderIndex = value; }
            get { return _OrderIndex; }
        }

        /// <summary>
        /// ����
        /// </summary>
        private int _Locked=int.MinValue;
        public int Locked
        {
            set { _Locked = value; }
            get { return _Locked; }
        }

        /// <summary>
        /// ����
        /// </summary>
        //private string _Condition = string.Empty;
        //public string Condition
        //{
        //    set { _Condition = value; }
        //    get { return _Condition; }
        //}

        /// <summary>
        /// ��Ϊ��λ
        /// </summary>
        private int _IsPost = int.MinValue;
        public int IsPost
        {
            set { _IsPost = value; }
            get { return _IsPost; }
        }

        /// <summary>
        /// ������˾
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
