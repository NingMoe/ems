using System;

namespace XueFuShop.Models
{
    /// <summary>
    /// CourseCate 的摘要说明
    /// </summary>
    [Serializable]
    public sealed class CourseCateInfo
    {

        #region CourseCateModel

        /// <summary>
        /// 
        /// </summary>
        private int _CateId;
        public int CateId
        {
            set { _CateId = value; }
            get { return _CateId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _CateName;
        public string CateName
        {
            set { _CateName = value; }
            get { return _CateName; }
        }
        /// <summary>
        /// 
        /// </summary>
        private int _ParentCateId;
        public int ParentCateId
        {
            set { _ParentCateId = value; }
            get { return _ParentCateId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _CompanyId;
        public int CompanyId
        {
            set { _CompanyId = value; }
            get { return _CompanyId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _OrderIndex;
        public int OrderIndex
        {
            set { _OrderIndex = value; }
            get { return _OrderIndex; }
        }


        /// <summary>
        /// 
        /// </summary>
        private string _BrandId="0";
        public string BrandId
        {
            set { _BrandId = value; }
            get { return _BrandId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Field;
        public string Field
        {
            set { _Field = value; }
            get { return _Field; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Condition;
        public string Condition
        {
            set { _Condition = value; }
            get { return _Condition; }
        }

        #endregion CourseCateModel
    }
}
