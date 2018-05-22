using System;

namespace XueFuShop.Models
{
    /// <summary>
    /// ȫ��λ�ƻ�ִ�� ��˾ԭ�� �Ƴٱ��
    /// </summary>
    public class CompanyRecordInfo
    {
        private int _Id;
        private int _CompanyId;
        private int _ChangeNum;
        private string _PostId;
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
        /// ��˾ID
        /// </summary>
        public int CompanyId
        {
            get { return this._CompanyId; }
            set { this._CompanyId = value; }
        }

        /// <summary>
        /// ��˾�仯������
        /// </summary>
        public int ChangeNum
        {
            get { return this._ChangeNum; }
            set { this._ChangeNum = value; }
        }



        /// <summary>
        /// �仯�ĸ�λ
        /// </summary>
        public string PostId
        {
            get { return this._PostId; }
            set { this._PostId = value; }
        }


        /// <summary>
        /// �仯��ԭ��
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
