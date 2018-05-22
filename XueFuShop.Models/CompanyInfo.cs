using System;

namespace XueFuShop.Models
{
    public sealed class CompanyInfo
    {
        private int _CompanyId;
        private string _CompanyName = string.Empty;
        private string _CompanySimpleName = string.Empty;
        private string _CompanyAddress = string.Empty;
        private string _CompanyTel = string.Empty;
        private string _CompanyPost = string.Empty;
        private string _BrandId = string.Empty;
        private int _GroupId = int.MinValue;
        private int _State = int.MinValue;
        private int _Sort;
        private string _ParentId = string.Empty;
        private string _ParentIdCondition = string.Empty;
        private string _Condition = string.Empty;
        private string _Field = string.Empty;
        private string _GroupIdCondition = string.Empty;
        private object _PostStartDate = DBNull.Value;
        private int _UserNum;
        private bool _IsTest;
        private object _EndDate = DBNull.Value;
        private string _Post;


        public int CompanyId
        {
            get { return this._CompanyId; }
            set { this._CompanyId = value; }
        }

        public string CompanyName
        {
            get { return this._CompanyName; }
            set { this._CompanyName = value; }
        }

        public string CompanySimpleName
        {
            get { return this._CompanySimpleName; }
            set { this._CompanySimpleName = value; }
        }

        public string CompanyAddress
        {
            get { return this._CompanyAddress; }
            set { this._CompanyAddress = value; }
        }

        public string CompanyTel
        {
            get { return this._CompanyTel; }
            set { this._CompanyTel = value; }
        }

        public string CompanyPost
        {
            get { return this._CompanyPost; }
            set { this._CompanyPost = value; }
        }

        public string BrandId
        {
            get { return this._BrandId; }
            set { this._BrandId = value; }
        }

        public int GroupId
        {
            get { return this._GroupId; }
            set { this._GroupId = value; }
        }

        public int State
        {
            get { return this._State; }
            set { this._State = value; }
        }

        public int Sort
        {
            get { return this._Sort; }
            set { this._Sort = value; }
        }

        public string ParentId
        {
            get { return this._ParentId; }
            set { this._ParentId = value; }
        }

        public string ParentIdCondition
        {
            get { return _ParentIdCondition; }
            set { _ParentIdCondition = value; }
        }

        public string Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }

        public string Field
        {
            get { return _Field; }
            set { _Field = value; }
        }

        public string GroupIdCondition
        {
            get { return _GroupIdCondition; }
            set { _GroupIdCondition = value; }
        }

        public object PostStartDate
        {
            set { _PostStartDate = value; }
            get { return _PostStartDate; }
        }

        /// <summary>
        /// 使用截止时间
        /// </summary>
        public object EndDate
        {
            set { _EndDate = value; }
            get { return _EndDate; }
        }

        /// <summary>
        /// 公司允许员工数量
        /// </summary>
        public int UserNum
        {
            get { return _UserNum; }
            set { _UserNum = value; }
        }

        /// <summary>
        /// 是否允许考试
        /// </summary>
        public bool IsTest
        {
            get { return _IsTest; }
            set { _IsTest = value; }
        }

        /// <summary>
        /// 公司岗位设置
        /// </summary>
        public string Post
        {
            get { return this._Post; }
            set { this._Post = value; }
        }

    }
}
