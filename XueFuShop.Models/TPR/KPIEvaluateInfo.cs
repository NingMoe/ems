using System;

namespace XueFuShop.Models
{
    public sealed class KPIEvaluateInfo
    {
        private int id;
        private int userId;
        private int postId;
        private int kPIId;
        private float scorse;
        private int rate;
        private int state = 0;
        private string evaluateDate;
        private DateTime? addDate;
        private int evaluateNameId;
        private int evaluateUserId;

        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        /// <summary>
        /// 岗位ID
        /// </summary>
        public int PostId
        {
            get { return this.postId; }
            set { this.postId = value; }
        }

        /// <summary>
        /// KPI标识
        /// </summary>
        public int KPIId
        {
            get { return this.kPIId; }
            set { this.kPIId = value; }
        }

        /// <summary>
        /// 分值（权重）
        /// </summary>
        public float Scorse
        {
            get { return this.scorse; }
            set { this.scorse = value; }
        }

        /// <summary>
        /// 进度比率
        /// </summary>
        public int Rate
        {
            get { return this.rate; }
            set { this.rate = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int State
        {
            get { return this.state; }
            set { this.state = value; }
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
        /// 日期
        /// </summary>
        public DateTime? AddDate
        {
            get { return this.addDate; }
            set { this.addDate = value; }
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
    }
}
