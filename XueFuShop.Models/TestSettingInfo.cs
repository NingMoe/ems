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
        /// �γ�ID����ƷID��
        /// </summary>
        public int CourseId
        {
            set { courseId = value; }
            get { return courseId; }
        }

        /// <summary>
        /// �Ծ��ܷ�
        /// </summary>
        public int PaperScore
        {
            set { paperScore = value; }
            get { return paperScore; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public int TestQuestionsCount
        {
            set { testQuestionsCount = value; }
            get { return testQuestionsCount; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public int TestTimeLength
        {
            set { testTimeLength = value; }
            get { return testTimeLength; }
        }

        /// <summary>
        /// �������
        /// </summary>
        public int LowScore
        {
            set { lowScore = value; }
            get { return lowScore; }
        }

        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        public DateTime? TestStartTime
        {
            set { testStartTime = value; }
            get { return testStartTime; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime? TestEndTime
        {
            set { testEndTime = value; }
            get { return testEndTime; }
        }

        /// <summary>
        /// ��˾ID
        /// </summary>
        public int CompanyId
        {
            set { companyId = value; }
            get { return companyId; }
        }

        /// <summary>
        /// ���Լ��
        /// </summary>
        public int TestInterval { get; set; } = 36;
    }
}
