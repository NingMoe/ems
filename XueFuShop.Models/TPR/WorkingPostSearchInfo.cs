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
        /// ���ID
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// ���ID
        /// </summary>
        public string ParentId
        {
            get { return this.parentId; }
            set { this.parentId = value; }
        }

        /// <summary>
        /// ��˾ID
        /// </summary>
        public string CompanyId
        {
            get { return this.companyId; }
            set { this.companyId = value; }
        }

        /// <summary>
        /// �Ƿ�Ϊ��λ
        /// </summary>
        public int IsPost
        {
            get { return this.isPost; }
            set { this.isPost = value; }
        }

        /// <summary>
        /// ״̬
        /// </summary>
        public int State
        {
            get { return this.state; }
            set { this.state = value; }
        }

        /// <summary>
        /// ��λ����
        /// </summary>
        public string PostName
        {
            get { return this.postName; }
            set { this.postName = value; }
        }

        //��������
        public string Condition
        {
            get { return this.condition; }
            set { this.condition = value; }
        }
    }
}
