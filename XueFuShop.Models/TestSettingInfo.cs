using System;

namespace XueFuShop.Models
{
    public sealed class TestSettingInfo
    {
        private int id;
        private int courseId;
        private int paperScore;
        private int testQuestionsCount;
        private int testTimeLength;
        private int lowScore;
        private DateTime? testStartTime;
        private DateTime? testEndTime;
        private int companyId;

        public int Id
        {
            set { id = value; }
            get { return id; }
        }

        /// <summary>
        /// 课程ID（商品ID）
        /// </summary>
        public int CourseId
        {
            set { courseId = value; }
            get { return courseId; }
        }

        /// <summary>
        /// 试卷总分
        /// </summary>
        public int PaperScore
        {
            set { paperScore = value; }
            get { return paperScore; }
        }

        /// <summary>
        /// 试题数量
        /// </summary>
        public int TestQuestionsCount
        {
            set { testQuestionsCount = value; }
            get { return testQuestionsCount; }
        }

        /// <summary>
        /// 考试时长
        /// </summary>
        public int TestTimeLength
        {
            set { testTimeLength = value; }
            get { return testTimeLength; }
        }

        /// <summary>
        /// 及格分数
        /// </summary>
        public int LowScore
        {
            set { lowScore = value; }
            get { return lowScore; }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? TestStartTime
        {
            set { testStartTime = value; }
            get { return testStartTime; }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? TestEndTime
        {
            set { testEndTime = value; }
            get { return testEndTime; }
        }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId
        {
            set { companyId = value; }
            get { return companyId; }
        }

        /// <summary>
        /// 考试间隔
        /// </summary>
        public int TestInterval { get; set; } = 36;
    }
}
