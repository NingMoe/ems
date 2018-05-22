using System;

namespace XueFuShop.Models
{
    /// <summary>
    /// TestPaper 的摘要说明
    /// </summary>
    public sealed class TestPaperInfo
    {
        #region TestPaperModel

        /// <summary>
        /// 
        /// </summary>
        private int _TestPaperId;
        public int TestPaperId
        {
            set { _TestPaperId = value; }
            get { return _TestPaperId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _PaperName=string.Empty;
        public string PaperName
        {
            set { _PaperName = value; }
            get { return _PaperName; }
        }

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
        /// 课程名称
        /// </summary>
        private string _CourseName;
        public string CourseName
        {
            set { _CourseName = value; }
            get { return _CourseName; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _QuestionStyle;
        public string QuestionStyle
        {
            set { _QuestionStyle = value; }
            get { return _QuestionStyle; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _QuestionId;
        public string QuestionId
        {
            set { _QuestionId = value; }
            get { return _QuestionId; }
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
        private int _Locked=int.MinValue;
        public int Locked
        {
            set { _Locked = value; }
            get { return _Locked; }
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
        private string _CompanyIdCondition=string.Empty;
        public string CompanyIdCondition
        {
            set { _CompanyIdCondition = value; }
            get { return _CompanyIdCondition; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _GroupCondition = string.Empty;
        public string GroupCondition
        {
            set { _GroupCondition = value; }
            get { return _GroupCondition; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _UserId=int.MinValue;
        public int UserId
        {
            set { _UserId = value; }
            get { return _UserId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private decimal _Scorse;
        public decimal Scorse
        {
            set { _Scorse = value; }
            get { return _Scorse; }
        }

        /// <summary>
        /// 
        /// </summary>
        private decimal _MaxScorse = decimal.MinValue;
        public decimal MaxScorse
        {
            set { _MaxScorse = value; }
            get { return _MaxScorse; }
        }

        /// <summary>
        /// 
        /// </summary>
        private DateTime _TestDate;
        public DateTime TestDate
        {
            set { _TestDate = value; }
            get { return _TestDate; }
        }

        /// <summary>
        /// 
        /// </summary>
        //private string _Field=string.Empty;
        //public string Field
        //{
        //    set { _Field = value; }
        //    get { return _Field; }
        //}

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
        private string _UserIdCondition = string.Empty;
        public string UserIdCondition
        {
            set { _UserIdCondition = value; }
            get { return _UserIdCondition; }
        }


        private string _CateIdCondition = string.Empty;
        public string CateIdCondition
        {
            set { _CateIdCondition = value; }
            get { return _CateIdCondition; }
        }


        private string _OrderByCondition = string.Empty;
        public string OrderByCondition
        {
            set { _OrderByCondition = value; }
            get { return _OrderByCondition; }
        }

        /// <summary>
        /// 
        /// </summary>
        private decimal _Point;
        public decimal Point
        {
            set { _Point = value; }
            get { return _Point; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _TestNum;
        public int TestNum
        {
            set { _TestNum = value; }
            get { return _TestNum; }
        }

        /// <summary>
        /// 
        /// </summary>
        private DateTime _TestMaxDate=DateTime.MinValue;
        public DateTime TestMaxDate
        {
            set { _TestMaxDate = value; }
            get { return _TestMaxDate; }
        }

        /// <summary>
        /// 
        /// </summary>
        private DateTime _TestMinDate=DateTime.MinValue;
        public DateTime TestMinDate
        {
            set { _TestMinDate = value; }
            get { return _TestMinDate; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _Del = 0;
        public int Del
        {
            set { _Del = value; }
            get { return _Del; }
        }

        private int _IsPass = int.MinValue;
        public int IsPass
        {
            set { _IsPass = value; }
            get { return _IsPass; }
        }

        #endregion TestPaperModel
    }

    public sealed class TestPaperReportInfo
    {
        private int userID;
        private int courseID;
        private decimal score;
        private DateTime testDate;
        private string courseName;
        private int isPass;

        public int UserID
        {
            set { this.userID = value; }
            get { return this.userID; }
        }

        public int CourseID
        {
            set { this.courseID = value; }
            get { return this.courseID; }
        }

        public string CourseName
        {
            set { this.courseName = value; }
            get { return this.courseName; }
        }

        public decimal Score
        {
            set { this.score = value; }
            get { return this.score; }
        }

        public DateTime TestDate
        {
            set { testDate = value; }
            get { return this.testDate; }
        }

        public int IsPass
        {
            set { this.isPass = value; }
            get { return this.isPass; }
        }
    }
}
