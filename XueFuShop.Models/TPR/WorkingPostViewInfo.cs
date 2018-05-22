using System;

namespace XueFuShop.Models
{
    public sealed class WorkingPostViewInfo
    {

        private int postId;
        private string postName;
        private string kpiContent;
        private int companyId;

        /// <summary>
        /// ���ID
        /// </summary>
        public int PostId
        {
            get { return this.postId; }
            set { this.postId = value; }
        }

        /// <summary>
        /// ��λ����
        /// </summary>
        public string PostName
        {
            get { return this.postName; }
            set { this.postName = value; }
        }

        /// <summary>
        /// ��λ����
        /// </summary>
        public string KPIContent
        {
            get { return this.kpiContent; }
            set { this.kpiContent = value; }
        }

        /// <summary>
        /// ��˾ID
        /// </summary>
        public int CompanyId
        {
            get { return this.companyId; }
            set { this.companyId = value; }
        }
    }
}
