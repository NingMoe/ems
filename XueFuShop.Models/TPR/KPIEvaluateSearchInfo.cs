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
        /// KPI��ID��
        /// </summary>
        public string KPIdStr
        {
            get { return this.kpiIdStr; }
            set { this.kpiIdStr = value; }
        }

        /// <summary>
        /// �û�ID
        /// </summary>
        public string UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string EvaluateDate
        {
            get { return this.evaluateDate; }
            set { this.evaluateDate = value; }
        }

        /// <summary>
        /// ��������Id
        /// </summary>
        public int EvaluateNameId
        {
            get { return this.evaluateNameId; }
            set { this.evaluateNameId = value; }
        }

        /// <summary>
        /// ������ID
        /// </summary>
        public int EvaluateUserId
        {
            get { return this.evaluateUserId; }
            set { this.evaluateUserId = value; }
        }

        /// <summary>
        /// ��λId
        /// </summary>
        public string PostId
        {
            get { return this.postId; }
            set { this.postId = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Condition
        {
            get { return this.condition; }
            set { this.condition = value; }
        }
    }
}
