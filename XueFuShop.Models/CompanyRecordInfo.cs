using System;

namespace XueFuShop.Models
{
    /// <summary>
    /// 全岗位计划执行 公司原因 推迟变更
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
        /// 数据库ID
        /// </summary>
        public int Id
        {
            get { return this._Id; }
            set { this._Id = value; }
        }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId
        {
            get { return this._CompanyId; }
            set { this._CompanyId = value; }
        }

        /// <summary>
        /// 公司变化的数量
        /// </summary>
        public int ChangeNum
        {
            get { return this._ChangeNum; }
            set { this._ChangeNum = value; }
        }



        /// <summary>
        /// 变化的岗位
        /// </summary>
        public string PostId
        {
            get { return this._PostId; }
            set { this._PostId = value; }
        }


        /// <summary>
        /// 变化的原因
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
