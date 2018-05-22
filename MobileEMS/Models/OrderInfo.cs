using System;

namespace MobileEMS.Models
{
    public class OrderInfo
    {
        private int id;
        private DateTime addDate = DateTime.Now;
        private string orderNote = string.Empty;
        private string orderNumber = string.Empty;
        private int orderStatus;
        private DateTime payDate = DateTime.Now;
        private string payName = string.Empty;
        private string payKey = string.Empty;
        private decimal productMoney;
        private int userID;
        private int cateId;

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public DateTime AddDate
        {
            get { return this.addDate; }
            set { this.addDate = value; }
        }

        public string OrderNote
        {
            get { return this.orderNote; }
            set { this.orderNote = value; }
        }

        public string OrderNumber
        {
            get { return this.orderNumber; }
            set { this.orderNumber = value; }
        }

        public int OrderStatus
        {
            get { return this.orderStatus; }
            set { this.orderStatus = value; }
        }

        public DateTime PayDate
        {
            get { return this.payDate; }
            set { this.payDate = value; }
        }

        public string PayName
        {
            get { return this.payName; }
            set { this.payName = value; }
        }

        public string PayKey
        {
            get { return this.payKey; }
            set { this.payKey = value; }
        }

        public decimal ProductMoney
        {
            get { return this.productMoney; }
            set { this.productMoney = value; }
        }

        public int UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }

        public int CateId
        {
            get { return this.cateId; }
            set { this.cateId = value; }
        }
    }
}
