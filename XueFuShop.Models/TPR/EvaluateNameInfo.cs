using System;

namespace XueFuShop.Models
{
    public sealed class EvaluateNameInfo
    {
        private int id;
        private string evaluateName;
        private string startDate;
        private string endDate;
        private string date;
        private int companyId;
        private string companyIdCondition;
        private int state = 0;

        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// 评估名称
        /// </summary>
        public string EvaluateName
        {
            get { return this.evaluateName; }
            set { this.evaluateName = value; }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartDate
        {
            get { return this.startDate; }
            set { this.startDate = value; }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate
        {
            get { return this.endDate; }
            set { this.endDate = value; }
        }

        /// <summary>
        /// 评估日期
        /// </summary>
        public string Date
        {
            get { return this.date; }
            set { this.date = value; }
        }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId
        {
            get { return this.companyId; }
            set { this.companyId = value; }
        }

        /// <summary>
        /// 公司Id查询条件
        /// </summary>
        public string CompanyIdCondition
        {
            get { return this.companyIdCondition; }
            set { this.companyIdCondition = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int State
        {
            get { return this.state; }
            set { this.state = value; }
        }
    }
}
