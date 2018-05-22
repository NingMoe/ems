using System;

namespace XueFuShop.Models
{
    public class PostPassInfo
    {
        private int _Id;
        private int _UserId = int.MinValue;
        private int _PostId = int.MinValue;
        private string _PostName = string.Empty;
        private DateTime _CreateDate = DateTime.MinValue;
        private DateTime _SearchStartDate = DateTime.MinValue;
        private int _IsRZ = 1;
        private string _InCompanyID = string.Empty;
        private string _InUserID = string.Empty;

        public int Id
        {
            get { return this._Id; }
            set { this._Id = value; }
        }

        /// <summary>
        /// �û�ID
        /// </summary>
        public int UserId
        {
            get { return this._UserId; }
            set { this._UserId = value; }
        }

        /// <summary>
        /// ͨ���ĸ�λID
        /// </summary>
        public int PostId
        {
            get { return this._PostId; }
            set { this._PostId = value; }
        }

        public string PostName
        {
            get { return this._PostName; }
            set { this._PostName = value; }
        }

        /// <summary>
        /// ��λͨ����ʱ��
        /// </summary>
        public DateTime CreateDate
        {
            get { return this._CreateDate; }
            set { this._CreateDate = value; }
        }

        /// <summary>
        /// ����ʱ��ʼʱ��
        /// </summary>
        public DateTime SearchStartDate
        {
            get { return this._SearchStartDate; }
            set { this._SearchStartDate = value; }
        }

        /// <summary>
        /// �Ƿ�ͨ���ۺϿ���
        /// </summary>
        public int IsRZ
        {
            get { return this._IsRZ; }
            set { this._IsRZ = value; }
        }

        public string InCompanyID
        {
            get { return this._InCompanyID; }
            set { this._InCompanyID = value; }
        }

        public string InUserID
        {
            get { return this._InUserID; }
            set { this._InUserID = value; }
        }
    }

    public class ReportPostPassInfo
    {
        private int _Id;
        private int _UserId = int.MinValue;
        private string _RealName;
        private string _PassPostName;
        private string _PassPostId;
        private int _PassPostNum;
        private int _WorkingPostId;
        private string _WorkingPostName;
        private int _StudyPostId;


        public int Id
        {
            get { return this._Id; }
            set { this._Id = value; }
        }

        /// <summary>
        /// �û�ID
        /// </summary>
        public int UserId
        {
            get { return this._UserId; }
            set { this._UserId = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string RealName
        {
            get { return this._RealName; }
            set { this._RealName = value; }
        }

        /// <summary>
        /// ������λ����
        /// </summary>
        public string WorkingPostName
        {
            get { return this._WorkingPostName; }
            set { this._WorkingPostName = value; }
        }

        /// <summary>
        /// ͨ���ĸ�λID
        /// </summary>
        public string PassPostId
        {
            get { return this._PassPostId; }
            set { this._PassPostId = value; }
        }

        /// <summary>
        /// ͨ���ĸ�λ����
        /// </summary>
        public string PassPostName
        {
            get { return this._PassPostName; }
            set { this._PassPostName = value; }
        }

        /// <summary>
        /// ͨ���ĸ�λ����
        /// </summary>
        public int PassPostNum
        {
            get { return this._PassPostNum; }
            set { this._PassPostNum = value; }
        }

        /// <summary>
        /// ������λ
        /// </summary>
        public int WorkingPostId
        {
            get { return this._WorkingPostId; }
            set { this._WorkingPostId = value; }
        }

        /// <summary>
        /// ѧϰ��λ
        /// </summary>
        public int StudyPostId
        {
            get { return this._StudyPostId; }
            set { this._StudyPostId = value; }
        }
    }
}
