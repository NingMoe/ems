using System;
using System.Collections.Generic;
using System.Text;

namespace XueFuShop.Models
{
    public sealed class UserGroupInfo
    {
        private DateTime addDate = DateTime.Now;
        private int userCount;
        private int id;
        private string iP = string.Empty;
        private string name = string.Empty;
        private string note = string.Empty;
        private string power = string.Empty;

        public DateTime AddDate
        {
            get
            {
                return this.addDate;
            }
            set
            {
                this.addDate = value;
            }
        }

        public int UserCount
        {
            get
            {
                return this.userCount;
            }
            set
            {
                this.userCount = value;
            }
        }

        public int ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public string IP
        {
            get
            {
                return this.iP;
            }
            set
            {
                this.iP = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public string Note
        {
            get
            {
                return this.note;
            }
            set
            {
                this.note = value;
            }
        }

        public string Power
        {
            get
            {
                return this.power;
            }
            set
            {
                this.power = value;
            }
        }
    }
}
