using System;

namespace XueFuShop.Models
{
    /// <summary>
    /// Question 的摘要说明
    /// </summary>
    public sealed class QuestionInfo
    {
        #region QuestionModel

        /// <summary>
        /// 
        /// </summary>
        private int _QuestionId=int.MinValue;
        public int QuestionId
        {
            set { _QuestionId = value; }
            get { return _QuestionId; }
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
        private string _Style=string.Empty;
        public string Style
        {
            set { _Style = value; }
            get { return _Style; }
        }

        /// <summary>
        /// 
        /// </summary>
        private decimal _Score;
        public decimal Score
        {
            set { _Score = value; }
            get { return _Score; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Question=string.Empty;
        public string Question
        {
            set { _Question = value; }
            get { return _Question; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _A;
        public string A
        {
            set { _A = value; }
            get { return _A; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _B;
        public string B
        {
            set { _B = value; }
            get { return _B; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _C;
        public string C
        {
            set { _C = value; }
            get { return _C; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _D;
        public string D
        {
            set { _D = value; }
            get { return _D; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Answer=string.Empty;
        public string Answer
        {
            set { _Answer = value; }
            get { return _Answer; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Remarks;
        public string Remarks
        {
            set { _Remarks = value; }
            get { return _Remarks; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _Checked=1;
        public int Checked
        {
            set { _Checked = value; }
            get { return _Checked; }
        }

        private string _Field = string.Empty;
        public string Field
        {
            set { _Field = value; }
            get { return _Field; }
        }

        private string _Condition = string.Empty;
        public string Condition
        {
            set { _Condition = value; }
            get { return _Condition; }
        }

        private string _IdCondition = string.Empty;
        public string IdCondition
        {
            set { _IdCondition = value; }
            get { return _IdCondition; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _CompanyId = int.MinValue;
        public int CompanyId
        {
            set { _CompanyId = value; }
            get { return _CompanyId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _QuestionNum=" * ";
        public string QuestionNum
        {
            set { _QuestionNum = value; }
            get { return _QuestionNum; }
        }


        #endregion QuestionModel
    }
}
