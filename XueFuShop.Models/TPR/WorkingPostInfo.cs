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
        /// ��λ����
        /// </summary>
        public string PostName
        {
            get { return this.postName; }
            set { this.postName = value; }
        }

        /// <summary>
        /// ���ID
        /// </summary>
        public int ParentId
        {
            get { return this.parentId; }
            set { this.parentId = value; }
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

        /// <summary>
        /// �Ƿ�Ϊ��λ
        /// </summary>
        public int IsPost
        {
            get { return this.isPost; }
            set { this.isPost = value; }
        }

        /// <summary>
        /// KPIģ��ı�ʶ
        /// </summary>
        public int KPITempletId
        {
            get { return this.kpiTempletId; }
            set { this.kpiTempletId = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public int Sort
        {
            get { return this.sort; }
            set { this.sort = value; }
        }

        /// <summary>
        /// �ؼ���
        /// </summary>
        public string Path
        {
            get { return this.path; }
            set { this.path = value; }
        }
    }
}
