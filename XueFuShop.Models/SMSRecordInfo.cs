using System;

namespace XueFuShop.Models
{
    public class SMSRecordInfo
    {
        private string mobile;
        private string verCode;
        private DateTime dataCreateDate;

        /// <summary>
        /// �ֻ�����
        /// </summary>
        public string Mobile
        {
            set { mobile = value; }
            get { return mobile; }
        }

        /// <summary>
        /// ��֤��
        /// </summary>
        public string VerCode
        {
            set { verCode = value; }
            get { return verCode; }
        }

        /// <summary>
        /// ��֤�뷢��ʱ��
        /// </summary>
        public DateTime DataCreateDate
        {
            set { dataCreateDate = value; }
            get { return dataCreateDate; }
        }          
            
    }
}
