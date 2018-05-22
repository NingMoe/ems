using System;

namespace XueFuShop.Models.Course
{
    /// <summary>
    /// MXQuestion 的摘要说明
    /// </summary>
    public class MXQuestion
    {
        #region MSQuestionModel

        /// <summary>
        /// 
        /// </summary>
        private int _QuestionId;
        public int QuestionId
        {
            set { _QuestionId = value; }
            get { return _QuestionId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _CateId;
        public string CateId
        {
            set { _CateId = value; }
            get { return _CateId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Style;
        public string Style
        {
            set { _Style = value; }
            get { return _Style; }
        }

        /// <summary>
        /// 
        /// </summary>
        private float _Score;
        public float Score
        {
            set { _Score = value; }
            get { return _Score; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Question;
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
        private string _Answer;
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
        private int _Checked;
        public int Checked
        {
            set { _Checked = value; }
            get { return _Checked; }
        }

        #endregion MSQuestionModel
    }
}
