using System;

namespace XueFuShop.Models
{
    public class SMSRecordInfo
    {
        private string mobile;
        private string verCode;
        private DateTime dataCreateDate;

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile
        {
            set { mobile = value; }
            get { return mobile; }
        }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerCode
        {
            set { verCode = value; }
            get { return verCode; }
        }

        /// <summary>
        /// 验证码发送时间
        /// </summary>
        public DateTime DataCreateDate
        {
            set { dataCreateDate = value; }
            get { return dataCreateDate; }
        }          
            
    }
}
