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
        /// 类别ID
        /// </summary>
        public int PostId
        {
            get { return this.postId; }
            set { this.postId = value; }
        }

        /// <summary>
        /// 岗位名称
        /// </summary>
        public string KPIContent
        {
            get { return this.kpiContent; }
            set { this.kpiContent = value; }
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
        /// 状态
        /// </summary>
        public int State
        {
            get { return this.state; }
            set { this.state = value; }
        }
    }
}
