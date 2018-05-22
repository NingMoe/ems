using System;
using XueFu.EntLib;

namespace XueFuShop.Models
{
    public sealed class KPIInfo
    {
        private int id;
        private string name = string.Empty;
        private int parentId = int.MinValue;
        private string evaluateInfo;
        private string method;
        private float scorse;
        private string other=string.Empty;
        private int companyId = int.MinValue;
        private int state = 0;
        private int sort;
        private DateTime? addDate;
        private int refPostId = 0;
        private KPIType type;

        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// KPI����
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
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
        /// ����
        /// </summary>
        public string EvaluateInfo
        {
            get { return this.evaluateInfo; }
            set { this.evaluateInfo = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string Method
        {
            get { return this.method; }
            set { this.method = value; }
        }

        /// <summary>
        /// ��ֵ��Ȩ�أ�
        /// </summary>
        public float Scorse
        {
            get { return this.scorse; }
            set { this.scorse = value; }
        }

        /// <summary>
        /// ��ע
        /// </summary>
        public string Other
        {
            get { return this.other; }
            set { this.other = value; }
        }

        /// <summary>
        /// ��˾ID
        /// </summary>
        public int CompanyID
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
        /// ����
        /// </summary>
        public int Sort
        {
            get { return this.sort; }
            set { this.sort = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public DateTime? AddDate
        {
            get { return this.addDate; }
            set { this.addDate = value; }
        }

        /// <summary>
        /// ��Ӧѧϰ��λID
        /// </summary>
        public int RefPostId
        {
            get { return this.refPostId; }
            set { this.refPostId = value; }
        }

        /// <summary>
        /// ָ������
        /// </summary>
        public KPIType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

    }
}
