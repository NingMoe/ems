using System;

namespace XueFuShop.Models
{
    public sealed class KPIEvaluateSearchInfo
    {
        private string kpiIdStr = string.Empty;
        private string userId = string.Empty;
        private string evaluateDate = string.Empty;
        private int evaluateNameId = int.MinValue;
        private string postId = string.Empty;
        private int evaluateUserId = int.MinValue;
        private string condition = string.Empty;

        /// <summary>
        /// KPI的ID串
        /// </summary>
        public string KPIdStr
        {
            get { return this.kpiIdStr; }
            set { this.kpiIdStr = value; }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        /// <summary>
        /// 评估日期
        /// </summary>
        public string EvaluateDate
        {
            get { return this.evaluateDate; }
            set { this.evaluateDate = value; }
        }

        /// <summary>
        /// 评估名称Id
        /// </summary>
        public int EvaluateNameId
        {
            get { return this.evaluateNameId; }
            set { this.evaluateNameId = value; }
        }

        /// <summary>
        /// 评估人ID
        /// </summary>
        public int EvaluateUserId
        {
            get { return this.evaluateUserId; }
            set { this.evaluateUserId = value; }
        }

        /// <summary>
        /// 岗位Id
        /// </summary>
        public string PostId
        {
            get { return this.postId; }
            set { this.postId = value; }
        }

        /// <summary>
        /// 条件
        /// </summary>
        public string Condition
        {
            get { return this.condition; }
            set { this.condition = value; }
        }
    }
}
