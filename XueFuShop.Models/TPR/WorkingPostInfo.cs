using System;

namespace XueFuShop.Models
{
    public sealed class WorkingPostInfo
    {
        private int id;
        private string postName;
        private int parentId;
        private int companyId;
        private int state;
        private int isPost = 0;
        private int kpiTempletId;
        private int sort;
        private string path;

        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
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
        /// 类别ID
        /// </summary>
        public int ParentId
        {
            get { return this.parentId; }
            set { this.parentId = value; }
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

        /// <summary>
        /// 是否为岗位
        /// </summary>
        public int IsPost
        {
            get { return this.isPost; }
            set { this.isPost = value; }
        }

        /// <summary>
        /// KPI模板的标识
        /// </summary>
        public int KPITempletId
        {
            get { return this.kpiTempletId; }
            set { this.kpiTempletId = value; }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort
        {
            get { return this.sort; }
            set { this.sort = value; }
        }

        /// <summary>
        /// 关键词
        /// </summary>
        public string Path
        {
            get { return this.path; }
            set { this.path = value; }
        }
    }
}
