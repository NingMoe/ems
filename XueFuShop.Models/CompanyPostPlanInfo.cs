using System;
using System.Collections.Generic;
using System.Text;

namespace XueFuShop.Models
{
    public class CompanyPostPlanInfo
    {
        private int _Id;
        private int _CompanyId = int.MinValue;
        private DateTime _StartDate;
        private string _PostId = string.Empty;
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
        /// �����ڵ�ѧϰ��λ
        /// </summary>
        public string PostId
        {
            get { return this._PostId; }
            set { this._PostId = value; }
        }


        /// <summary>
        /// ��λ�ƻ���ʼʱ��
        /// </summary>
        public DateTime StartDate
        {
            get { return this._StartDate; }
            set { this._StartDate = value; }
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
