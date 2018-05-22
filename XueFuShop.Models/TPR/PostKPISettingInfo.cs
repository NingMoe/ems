using System;

namespace XueFuShop.Models
{
    public sealed class PostKPISettingInfo
    {
        private int id;
        private string name;
        private string kPIContent;
        private int companyId;
        private int state;

        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }


        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// KPI指标内容
        /// </summary>
        public string KPIContent
        {
            get { return this.kPIContent; }
            set { this.kPIContent = value; }
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
