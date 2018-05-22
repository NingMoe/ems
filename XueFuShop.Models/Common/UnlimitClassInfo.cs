using System;

namespace XueFuShop.Models.Common
{
    public class UnlimitClassInfo
    {
        // Fields
        private int classID = 0;
        private string className = string.Empty;
        private int fatherID = 0;

        // Properties
        public int ClassID
        {
            get
            {
                return this.classID;
            }
            set
            {
                this.classID = value;
            }
        }

        public string ClassName
        {
            get
            {
                return this.className;
            }
            set
            {
                this.className = value;
            }
        }

        public int FatherID
        {
            get
            {
                return this.fatherID;
            }
            set
            {
                this.fatherID = value;
            }
        }
    }
}
