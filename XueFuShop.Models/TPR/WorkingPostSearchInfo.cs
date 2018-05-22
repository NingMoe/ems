using System;

namespace XueFuShop.Models
{
    public sealed class WorkingPostSearchInfo
    {
        private string name = string.Empty;
        private string companyId = string.Empty;
        private string parentId = string.Empty;
        private int isPost = int.MinValue;
        private int state = 0;
        private string postName = string.Empty;
        private string condition = string.Empty;

        /// <summary>
        /// 类别ID
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
        public string CompanyId
        {
            get { return this.companyId; }
            set { this.companyId = value; }
        }

        /// <summary>
        /// 是否为岗位
        /// </summary>
        public int IsPost
        {
            get { return this.isPost; }
            set { this.isPost = value; }
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
        /// 岗位名称
        /// </summary>
        public string PostName
        {
            get { return this.postName; }
            set { this.postName = value; }
        }

        //其他条件
        public string Condition
        {
            get { return this.condition; }
            set { this.condition = value; }
        }
    }
}
