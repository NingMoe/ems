using System;

namespace XueFuShop.Models
{
    /// <summary>
    /// Course 的摘要说明
    /// </summary>
    public class CourseInfo
    {

        #region CourseModel

        /// <summary>
        /// 
        /// </summary>
        private int _CourseId;
        public int CourseId
        {
            set { _CourseId = value; }
            get { return _CourseId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _CourseName=string.Empty;
        public string CourseName
        {
            set { _CourseName = value; }
            get { return _CourseName; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _CourseCode;
        public string CourseCode
        {
            set { _CourseCode = value; }
            get { return _CourseCode; }
        }


        /// <summary>
        /// 
        /// </summary>
        private decimal _CourseScore;
        public decimal CourseScore
        {
            set { _CourseScore = value; }
            get { return _CourseScore; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _CateId=int.MinValue;
        public int CateId
        {
            set { _CateId = value; }
            get { return _CateId; }
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
        private DateTime _CreateDate;
        public DateTime CreateDate
        {
            set { _CreateDate = value; }
            get { return _CreateDate; }
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
        private string _Condition=string.Empty;
        public string Condition
        {
            set { _Condition = value; }
            get { return _Condition; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _CateIdCondition = string.Empty;
        public string CateIdCondition
        {
            set { _CateIdCondition = value; }
            get { return _CateIdCondition; }
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
        private string _BrandId;
        public string BrandId
        {
            set { _BrandId = value; }
            get { return _BrandId; }
        }

        #endregion CourseModel
    }
}
