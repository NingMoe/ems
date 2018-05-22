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
        /// 要完成的课程数量
        /// </summary>
        public int CourseNum
        {
            get { return this._CourseNum; }
            set { this._CourseNum = value; }
        }

        // <summary>
        /// 频率 以周计算
        /// </summary>
        public int Frequency
        {
            get { return this._Frequency; }
            set { this._Frequency = value; }
        }

        /// <summary>
        /// 适用于的学习岗位
        /// </summary>
        public string PostId
        {
            get { return this._PostId; }
            set { this._PostId = value; }
        }


        /// <summary>
        /// 规则开始时间
        /// </summary>
        public DateTime StartDate
        {
            get { return this._StartDate; }
            set { this._StartDate = value; }
        }

        /// <summary>
        /// 规则线束时间
        /// </summary>
        public DateTime EndDate
        {
            get { return this._EndDate; }
            set { this._EndDate = value; }
        }

        /// <summary>
        /// 信息提交时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return this._CreateDate; }
            set { this._CreateDate = value; }
        }
    }
}
