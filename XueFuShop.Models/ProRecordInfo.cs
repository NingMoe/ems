using System;

namespace XueFuShop.Models
{
    public class ProRecordInfo
    {
        private int _Id;
        private int _UserId;
        private int _ChangeNum;
        private string _Reason;
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
        /// �û�ID
        /// </summary>
        public int UserId
        {
            get { return this._UserId; }
            set { this._UserId = value; }
        }

        /// <summary>
        /// �ı�����
        /// </summary>
        public int ChangeNum
        {
            get { return this._ChangeNum; }
            set { this._ChangeNum = value; }
        }

        /// <summary>
        /// �ı�ԭ��
        /// </summary>
        public string Reason
        {
            get { return this._Reason; }
            set { this._Reason = value; }
        }

        /// <summary>
        /// �仯��Ϣ�ύʱ��
        /// </summary>
        public DateTime CreateDate
        {
            get { return this._CreateDate; }
            set { this._CreateDate = value; }
        }
    }
}
