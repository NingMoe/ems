using System;

namespace XueFuShop.Models
{
    public sealed class KPIEvaluateReportInfo
    {
        private int userId;
        private string userName;
        private int postId;
        private string postName;
        private int workingPostKPINum;
        private int tempNum;
        private int fixedNum;
        private int completeNum;
        private int rate;
        private int evaluateNameId;
        private string evaluateName;
        private int totalScore;
        private string companyName;


        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
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
        /// 岗位名称
        /// </summary>
        public string PostName
        {
            get { return this.postName; }
            set { this.postName = value; }
        }

        /// <summary>
        /// 岗位KPI数量
        /// </summary>
        public int WorkingPostKPINum
        {
            get { return this.workingPostKPINum; }
            set { this.workingPostKPINum = value; }
        }

        /// <summary>
        /// 当期指标数量
        /// </summary>
        public int TempNum
        {
            get { return this.tempNum; }
            set { this.tempNum = value; }
        }

        /// <summary>
        /// 永久指标数量
        /// </summary>
        public int FixedNum
        {
            get { return this.fixedNum; }
            set { this.fixedNum = value; }
        }

        /// <summary>
        /// 完成数量
        /// </summary>
        public int CompleteNum
        {
            get { return this.completeNum; }
            set { this.completeNum = value; }
        }

        /// <summary>
        /// 完成率
        /// </summary>
        public int Rate
        {
            get { return this.rate; }
            set { this.rate = value; }
        }

        /// <summary>
        /// 评估名称ID
        /// </summary>
        public int EvaluateNameId
        {
            get { return this.evaluateNameId; }
            set { this.evaluateNameId = value; }
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
        /// KPI总分
        /// </summary>
        public int TotalScore
        {
            get { return this.totalScore; }
            set { this.totalScore = value; }
        }
        
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName
        {
            get { return this.companyName; }
            set { this.companyName = value; }
        }
    }
}
