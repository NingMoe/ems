using System;

namespace XueFuShop.Models.Course
{
    /// <summary>
    /// MXAchievement 的摘要说明
    /// </summary>
    public class Achievement
    {
        #region MXAchievementModel

        /// <summary>
        /// 
        /// </summary>
        private int _AchievementId;
        public int AchievementId
        {
            set { _AchievementId = value; }
            get { return _AchievementId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _UserId;
        public int UserId
        {
            set { _UserId = value; }
            get { return _UserId; }
        }
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
        //private string _Achievement;
        //public string Achievement
        //{
        //    set { _Achievement = value; }
        //    get { return _Achievement; }
        //}
        #endregion MXAchievementModel
    }
}
