using System;
using XueFu.EntLib;

namespace XueFuShop.Models
{
    public sealed class ProductSearchInfo
    {
        private string aliasName = string.Empty;
        private string inBrandID = string.Empty;
        private string classID = string.Empty;
        private string notInClassID = string.Empty;
        private DateTime endAddDate = DateTime.MinValue;
        private string inProductID = string.Empty;
        private int isHot = -2147483648;
        private int isNew = -2147483648;
        private int isSale = -2147483648;
        private int isSpecial = -2147483648;
        private int isTaobao = -2147483648;
        private int isTop = -2147483648;
        private decimal marketPrice = decimal.MinValue;
        private string key = string.Empty;
        private string keywords = string.Empty;
        private string name = string.Empty;
        private string notLikeName = string.Empty;
        private string notInProductID = string.Empty;
        private OrderType orderType = OrderType.Desc;
        private string productNumber = string.Empty;
        private string productOrderType = string.Empty;
        private string spelling = string.Empty;
        private int standardType = -2147483648;
        private DateTime startAddDate = DateTime.MinValue;
        private int storageAnalyse = -2147483648;
        private string tags = string.Empty;
        private string relationProduct = string.Empty;
        private string accessory = string.Empty;
        private string inCompanyID = string.Empty;
        private int isComplete = 0;
        private string orderField = string.Empty;
        private string editor = string.Empty;
        private int isSpecialTestSetting = int.MinValue;


        public string AliasName
        {
            get { return this.aliasName; }
            set { this.aliasName = value; }
        }

        public string InBrandID
        {
            get { return this.inBrandID; }
            set { this.inBrandID = value; }
        }

        public string ClassID
        {
            get { return this.classID; }
            set { this.classID = value; }
        }

        public string NotInClassID
        {
            get { return this.notInClassID; }
            set { this.notInClassID = value; }
        }

        public DateTime EndAddDate
        {
            get { return this.endAddDate; }
            set { this.endAddDate = value; }
        }

        public string InProductID
        {
            get { return this.inProductID; }
            set { this.inProductID = value; }
        }

        public int IsHot
        {
            get { return this.isHot; }
            set { this.isHot = value; }
        }

        public int IsNew
        {
            get { return this.isNew; }
            set { this.isNew = value; }
        }

        public int IsSale
        {
            get { return this.isSale; }
            set { this.isSale = value; }
        }

        public int IsSpecial
        {
            get { return this.isSpecial; }
            set { this.isSpecial = value; }
        }

        public int IsTaobao
        {
            get { return this.isTaobao; }
            set { this.isTaobao = value; }
        }

        public int IsTop
        {
            get { return this.isTop; }
            set { this.isTop = value; }
        }

        public decimal MarketPrice
        {
            get { return this.marketPrice; }
            set { this.marketPrice = value; }
        }

        public string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        public string Keywords
        {
            get { return this.keywords; }
            set { this.keywords = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string NotLikeName
        {
            get { return this.notLikeName; }
            set { this.notLikeName = value; }
        }

        public string NotInProductID
        {
            get { return this.notInProductID; }
            set { this.notInProductID = value; }
        }

        public OrderType OrderType
        {
            get { return this.orderType; }
            set { this.orderType = value; }
        }

        public string ProductNumber
        {
            get { return this.productNumber; }
            set { this.productNumber = value; }
        }

        public string ProductOrderType
        {
            get { return this.productOrderType; }
            set { this.productOrderType = value; }
        }

        public string Spelling
        {
            get { return this.spelling; }
            set { this.spelling = value; }
        }

        public int StandardType
        {
            get { return this.standardType; }
            set { this.standardType = value; }
        }

        public DateTime StartAddDate
        {
            get { return this.startAddDate; }
            set { this.startAddDate = value; }
        }

        public int StorageAnalyse
        {
            get { return this.storageAnalyse; }
            set { this.storageAnalyse = value; }
        }

        public string Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }

        public string RelationProduct
        {
            get { return this.relationProduct; }
            set { this.relationProduct = value; }
        }

        public string Accessory
        {
            get { return this.accessory; }
            set { this.accessory = value; }
        }

        public string InCompanyID
        {
            get { return this.inCompanyID; }
            set { this.inCompanyID = value; }
        }

        /// <summary>
        /// 是否完整
        /// </summary>
        public int IsComplete
        {
            get { return this.isComplete; }
            set { this.isComplete = value; }
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderField
        {
            get { return this.orderField; }
            set { this.orderField = value; }
        }

        /// <summary>
        /// 编辑者（作者）
        /// </summary>
        public string Editor
        {
            set { editor = value; }
            get { return editor; }
        }

        /// <summary>
        ///  有指定的考试设置
        /// </summary>
        public int IsSpecialTestSetting
        {
            set { this.isSpecialTestSetting = value; }
            get { return this.isSpecialTestSetting; }
        }
    }
}
