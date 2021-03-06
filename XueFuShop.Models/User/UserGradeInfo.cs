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
    public sealed class UserGradeInfo
    {
        
        private decimal discount;
        private int id;
        private decimal maxMoney;
        private MemberPriceInfo memberPrice = new MemberPriceInfo();
        private decimal minMoney;
        private string name = string.Empty;

        
        public decimal Discount
        {
            get
            {
                return this.discount;
            }
            set
            {
                this.discount = value;
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

        public decimal MaxMoney
        {
            get
            {
                return this.maxMoney;
            }
            set
            {
                this.maxMoney = value;
            }
        }

        public MemberPriceInfo MemberPrice
        {
            get
            {
                return this.memberPrice;
            }
            set
            {
                this.memberPrice = value;
            }
        }

        public decimal MinMoney
        {
            get
            {
                return this.minMoney;
            }
            set
            {
                this.minMoney = value;
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
    }
}
