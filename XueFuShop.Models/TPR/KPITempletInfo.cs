using System;

namespace XueFuShop.Models
{
    public sealed class KPITempletInfo
    {
        private int id;
        private int postId = 0;
        private string kpiContent;
        private int companyId = int.MinValue;
        private int state = 0;

        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

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

        /// <summary>
        /// ״̬
        /// </summary>
        public int State
        {
            get { return this.state; }
            set { this.state = value; }
        }
    }
}
