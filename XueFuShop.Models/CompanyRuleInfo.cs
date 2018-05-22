using System;

namespace XueFuShop.Models
{
    public class CompanyRuleInfo
    {

        private int _Id;
        private int _CompanyId=int.MinValue;
        private int _CourseNum;
        private int _Frequency;
        private DateTime _StartDate;
        private DateTime _EndDate;
        private string _PostId=string.Empty;
        private DateTime _CreateDate;


        /// <summary>
        /// ���ݿ�ID
        /// </summary>
        public int Id
        {
            get { return this._Id; }
            set { this._Id = value; }
        }

        /// <summary>
        /// ��˾ID
        /// </summary>
        public int CompanyId
        {
            get { return this._CompanyId; }
            set { this._CompanyId = value; }
        }

        /// <summary>
        /// Ҫ��ɵĿγ�����
        /// </summary>
        public int CourseNum
        {
            get { return this._CourseNum; }
            set { this._CourseNum = value; }
        }

        // <summary>
        /// Ƶ�� ���ܼ���
        /// </summary>
        public int Frequency
        {
            get { return this._Frequency; }
            set { this._Frequency = value; }
        }

        /// <summary>
        /// �����ڵ�ѧϰ��λ
        /// </summary>
        public string PostId
        {
            get { return this._PostId; }
            set { this._PostId = value; }
        }


        /// <summary>
        /// ����ʼʱ��
        /// </summary>
        public DateTime StartDate
        {
            get { return this._StartDate; }
            set { this._StartDate = value; }
        }

        /// <summary>
        /// ��������ʱ��
        /// </summary>
        public DateTime EndDate
        {
            get { return this._EndDate; }
            set { this._EndDate = value; }
        }

        /// <summary>
        /// ��Ϣ�ύʱ��
        /// </summary>
        public DateTime CreateDate
        {
            get { return this._CreateDate; }
            set { this._CreateDate = value; }
        }
    }
}
