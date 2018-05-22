using System;

namespace XueFuShop.Models
{
    public sealed class AdminGroupInfo
    {
        private DateTime addDate = DateTime.Now;
        private int adminCount;
        private int id;
        private string iP = string.Empty;
        private string name = string.Empty;
        private string note = string.Empty;
        private string power = string.Empty;
        private int companyID = int.MinValue;
        private int state = 0;

        public DateTime AddDate
        {
            get { return this.addDate; }
            set { this.addDate = value; }
        }

        public int AdminCount
        {
            get { return this.adminCount; }
            set { this.adminCount = value; }
        }

        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string IP
        {
            get { return this.iP; }
            set { this.iP = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Note
        {
            get { return this.note; }
            set { this.note = value; }
        }

        public string Power
        {
            get { return this.power; }
            set { this.power = value; }
        }

        public int CompanyID
        {
            get { return this.companyID; }
            set { this.companyID = value; }
        }

        /// <summary>
        /// 是否内部（1:开放到客户；0:只后台使用）
        /// </summary>
        public int State
        {
            get { return this.state; }
            set { this.state = value; }
        }
    }
}
