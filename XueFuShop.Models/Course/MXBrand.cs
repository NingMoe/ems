using System;

namespace XueFuShop.Models.Course
{
    /// <summary>
    /// MXBrand 的摘要说明
    /// </summary>
    public class MXBrand
    {

        #region MXBrandModel

        /// <summary>
        /// 
        /// </summary>
        private int _BrandId;
        public int BrandId
        {
            set { _BrandId = value; }
            get { return _BrandId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _BrandName;
        public string BrandName
        {
            set { _BrandName = value; }
            get { return _BrandName; }
        }
        #endregion MXBrandModel
    }
}
