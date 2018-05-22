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
        /// 数据库ID
        /// </summary>
        public int Id
        {
            get { return this._Id; }
            set { this._Id = value; }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId
        {
            get { return this._UserId; }
            set { this._UserId = value; }
        }

        /// <summary>
        /// 改变数量
        /// </summary>
        public int ChangeNum
        {
            get { return this._ChangeNum; }
            set { this._ChangeNum = value; }
        }

        /// <summary>
        /// 改变原因
        /// </summary>
        public string Reason
        {
            get { return this._Reason; }
            set { this._Reason = value; }
        }

        /// <summary>
        /// 变化信息提交时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return this._CreateDate; }
            set { this._CreateDate = value; }
        }
    }
}
