using System;

namespace XueFuShop.Models
{
    public sealed class KPISearchInfo
    {
        private string idStr = string.Empty;
        private string name = string.Empty;
        private string parentId = string.Empty;
        private string companyId = string.Empty;
        private int state = 0;

        /// <summary>
        /// ID串
        /// </summary>
        public string IdStr
        {
            get { return this.idStr; }
            set { this.idStr = value; }
        }

        /// <summary>
        /// KPI名称
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// 类别ID
        /// </summary>
        public string ParentId
        {
            get { return this.parentId; }
            set { this.parentId = value; }
        }

        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyID
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
