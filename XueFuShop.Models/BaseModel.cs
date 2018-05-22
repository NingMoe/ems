using System;
using System.Collections.Generic;
using System.Text;

namespace XueFuShop.Models
{
    public abstract class BaseModel
    {
        private int id;

        public int ID 
        {
            get { return this.id; }
            set { this.id = value; }
        }
    }
}
