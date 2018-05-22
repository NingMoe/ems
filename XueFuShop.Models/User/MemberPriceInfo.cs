using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace XueFuShop.Models
{
    [Serializable]
    public sealed class MemberPriceInfo
    {
        
        private int gradeID;
        private decimal price;
        private int productID;

        
        public int GradeID
        {
            get
            {
                return this.gradeID;
            }
            set
            {
                this.gradeID = value;
            }
        }

        public decimal Price
        {
            get
            {
                return this.price;
            }
            set
            {
                this.price = value;
            }
        }

        public int ProductID
        {
            get
            {
                return this.productID;
            }
            set
            {
                this.productID = value;
            }
        }
    }
}
