using System;
using System.Collections.Generic;
using System.Web;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.IDAL;
using XueFuShop.Models;
using System.IO;

namespace XueFuShop.BLL
{
    public sealed class TestSettingBLL
    {
        private static readonly string cacheKey = CacheKey.ReadCacheKey("TestSettingInfo");
        private static readonly ITestSetting dal = FactoryHelper.Instance<ITestSetting>(Global.DataProvider, "DALTestSetting");

        #region 考试中的课程缓存

        //同一门课程同一个帐号，不能同时开启多次考试
        private static Dictionary<string, bool> testRunning = new Dictionary<string, bool>();

        //考试锁
        private static object testLock = new object();
        /// <summary>
        /// 是否可以开始考试，并记录考试信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static bool TestBegin(int userID, int productID)
        {
            string testKey = string.Concat(userID.ToString(), "-", productID.ToString());
            if (!testRunning.ContainsKey(testKey))
            {
                lock (testLock)
                {
                    if (!testRunning.ContainsKey(testKey))
                    {
                        testRunning[testKey] = true;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 考试结束，取消该课程考试限制
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="productID"></param>
        public static void TestEnd(int userID, int productID)
        {
            string testKey = string.Concat(userID.ToString(), "-", productID.ToString());
            testRunning.Remove(testKey);
        }

        #endregion

        /// <summary>
        /// 从缓存中读取考试设定列表
        /// </summary>
        /// <returns></returns>
        public static List<TestSettingInfo> ReadTestSettingCacheList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadList());
            }
            return (List<TestSettingInfo>)CacheHelper.Read(cacheKey);
        }

        /// <summary>
        /// 返回考试设置信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static TestSettingInfo ReadTestSetting(int companyId)
        {
            return ReadTestSetting(companyId.ToString(), 0);
        }

        /// <summary>
        /// 返回考试设置信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static TestSettingInfo ReadTestSetting(int companyId, int courseId)
        {
            //string parentCompanyID = CookiesHelper.ReadCookieValue("UserCompanyParentCompanyID");
            //if (string.IsNullOrEmpty(parentCompanyID)) parentCompanyID = companyId.ToString();
            //else parentCompanyID = companyId.ToString() + "," + parentCompanyID;

            return ReadTestSetting(CookiesHelper.ReadCookieValue("UserCompanyParentCompanyID"), courseId);
        }

        /// <summary>
        /// 返回考试设置信息
        /// </summary>
        /// <param name="companyId">公司级别从低到高，以","分隔</param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public static TestSettingInfo ReadTestSetting(string companyId, int courseId)
        {
            TestSettingInfo testSetting = null;
            string[] sonCompanyID = companyId.Split(',');
            for (int i = 0; i < sonCompanyID.Length; i++)
            {
                //1、第一调取companyId公司设置的courseId课程的考试设定
                if (courseId != 0)
                {
                    testSetting = ReadCompanyTestSetting(int.Parse(sonCompanyID[i]), courseId);
                    if (testSetting != null) break;
                }
                //2、第二调取companyId公司设置的考试设定
                if (testSetting == null)
                {
                    testSetting = ReadCompanyTestSetting(int.Parse(sonCompanyID[i]), 0);
                    if (testSetting != null) break;
                }
            }
            //3、第三调取系统设置的courseId课程的考试设定
            if (testSetting == null && courseId != 0)
                testSetting = ReadSystemTestSetting(courseId);
            //4、第四调取系统设置的考试设定
            if (testSetting == null)
                testSetting = ReadSystemTestSetting();
            return testSetting;
        }

        /// <summary>
        /// 返回系统考试设置信息
        /// </summary>
        /// <returns></returns>
        public static TestSettingInfo ReadSystemTestSetting()
        {
            return ReadCompanyTestSetting(CompanyBLL.SystemCompanyId, 0);
        }

        /// <summary>
        /// 返回系统考试设置信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public static TestSettingInfo ReadSystemTestSetting(int courseId)
        {
            return ReadCompanyTestSetting(CompanyBLL.SystemCompanyId, courseId);
        }

        /// <summary>
        /// 返回公司层考试设置信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public static TestSettingInfo ReadCompanyTestSetting(int companyId, int courseId)
        {
            List<TestSettingInfo> testSettingList = ReadTestSettingCacheList();
            return testSettingList.Find(delegate(TestSettingInfo tempModel) { return tempModel.CompanyId == companyId && tempModel.CourseId == courseId; });
        }

        //public static TestSettingInfo ReadTestSys(int Id)
        //{
        //    if (HttpContext.Current.Session["TestSysInfo"] == null || ((TestSettingInfo)HttpContext.Current.Session["TestSysInfo"]).CompanyId != Id)
        //    {
        //        TestSettingInfo Model = dal.ReadTestSys(Id);
        //        if (Model == null)
        //        {
        //            Model = dal.ReadTestSys(0);
        //            Model.CompanyId = Id;
        //        }
        //        HttpContext.Current.Session["TestSysInfo"] = Model;
        //    }
        //    return (TestSettingInfo)HttpContext.Current.Session["TestSysInfo"];
        //}

        /// <summary>
        /// 添加考试设定（如果已有记录则更新记录）
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static int AddTestSetting(TestSettingInfo Model)
        {
            Model.Id = dal.AddTestSetting(Model);
            CacheHelper.Remove(cacheKey);
            return Model.Id;
        }
        public static void UpdateTestSetting(TestSettingInfo Model)
        {
            dal.UpdateTestSetting(Model);
            CacheHelper.Remove(cacheKey);
        }
        public static void DeleteTestSetting(int Id)
        {
            dal.DeleteTestSetting(Id);
            CacheHelper.Remove(cacheKey);
        }

        public static void UpdateTestSetting(int companyID, int productID, TestSettingInfo testSetting)
        {
            //删除已有记录
            TestSettingInfo currentTestSetting = TestSettingBLL.ReadCompanyTestSetting(companyID, productID);
            if (currentTestSetting != null)
            {
                DeleteTestSetting(currentTestSetting.Id);
            }

            if (testSetting.PaperScore > 0 || testSetting.TestTimeLength > 0 || testSetting.TestQuestionsCount > 0 || testSetting.LowScore > 0 || testSetting.TestStartTime.HasValue || testSetting.TestEndTime.HasValue)
            {
                //取得公司通用考试设置
                TestSettingInfo systemTestSetting = ReadTestSetting(companyID);
                testSetting.CompanyId = companyID;
                testSetting.CourseId = productID;

                //内容不合法的，都重置公司默认设置
                if (testSetting.PaperScore <= 0)
                    testSetting.PaperScore = systemTestSetting.PaperScore;
                if (testSetting.TestTimeLength <= 0)
                    testSetting.TestTimeLength = systemTestSetting.TestTimeLength;
                if (testSetting.TestQuestionsCount <= 0)
                    testSetting.TestQuestionsCount = systemTestSetting.TestQuestionsCount;
                if (testSetting.LowScore <= 0)
                    testSetting.LowScore = systemTestSetting.LowScore;

                //下面两项任一项为空，都采用公司默认设置
                if (testSetting.TestStartTime == null || testSetting.TestEndTime == null)
                {
                    testSetting.TestStartTime = systemTestSetting.TestStartTime;
                    testSetting.TestEndTime = systemTestSetting.TestEndTime;
                }
                AddTestSetting(testSetting);
            }
        }

        /// <summary>
        /// 读取指定时间考试列表
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static List<TestSettingInfo> ReadSpecialTestList(string inCompanyID)
        {
            List<TestSettingInfo> specialTestSettingList = new List<TestSettingInfo>();
            List<TestSettingInfo> testSettingList = ReadTestSettingCacheList();
            foreach (TestSettingInfo info in testSettingList)
            {
                if (StringHelper.CompareSingleString(inCompanyID, info.CompanyId.ToString()) && info.TestStartTime.HasValue && info.TestEndTime.HasValue && DateTime.Now < Convert.ToDateTime(info.TestEndTime) && (DateTime.Now >= Convert.ToDateTime(info.TestStartTime) || DateTime.Now.AddHours(228) > Convert.ToDateTime(info.TestStartTime)))
                {
                    specialTestSettingList.Add(info);
                }
            }
            return specialTestSettingList;
        }

        /// <summary>
        /// 读取产品ID串
        /// </summary>
        /// <param name="specialTestList"></param>
        /// <returns></returns>
        public static string ReadSpecialTestCourseID(List<TestSettingInfo> specialTestList)
        {
            string specialCourseID = string.Empty;
            foreach (TestSettingInfo info in specialTestList)
            {
                specialCourseID += "," + info.CourseId.ToString();
            }
            if (specialCourseID.StartsWith(",")) specialCourseID = specialCourseID.Substring(1);
            return specialCourseID;
        }

        /// <summary>
        /// 特别课程处理（如：孟特销售工具应用与说明）
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="courseID"></param>
        //public static void SpecialTestHandle(int userID, int postID)
        //{
        //    string path = ServerHelper.MapPath("~/xml/postinitcourse.config");
        //    string rootNode = "UserList";
        //    string nodeName = "User" + userID.ToString();
        //    string nodePath = rootNode + "/" + nodeName;
        //    XmlHelper xmldoc = new XmlHelper(path);
        //    if (!File.Exists(path))
        //    {
        //        xmldoc.InsertNode(rootNode);
        //    }
        //    if (xmldoc.ReadNode(nodePath) == null)
        //    {
        //        xmldoc.InsertElement(rootNode, nodeName, "ID", userID.ToString(), UserBLL.ReadUser(userID).RealName);
        //        xmldoc.InsertElement(nodePath, "Post", "ID", postID.ToString(), PostBLL.ReadPost(postID).PostName);
        //    }
        //    else
        //    {
        //        if (!xmldoc.ExistsAttribute(nodePath, "ID", postID.ToString()))
        //            xmldoc.InsertElement(nodePath, "Post", "ID", postID.ToString(), PostBLL.ReadPost(postID).PostName);
        //    }

        //    xmldoc.Save(path);
        //}

        /// <summary>
        /// 是否需要重新考试
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="postID"></param>
        /// <returns></returns>
        //public static bool IsTestAgain(int userID, int postID)
        //{
        //    bool resultData = false;

        //    string path = ServerHelper.MapPath("~/xml/postinitcourse.config");
        //    string nodePath = "UserList/User" + userID.ToString();
        //    XmlHelper xmldoc = new XmlHelper(path);
        //    if (!xmldoc.ExistsAttribute(nodePath, "ID", postID.ToString()))
        //    {
        //        resultData = true;
        //    }
        //    return resultData;
        //}

        /// <summary>
        /// 获取指定月分起至时间
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        //public static DateTime[] ReadDateRange(int CompanyId, string SelectYM)
        //{
        //    string SelectYear = SelectYM.Split('-')[0];
        //    int SelectMonth = int.Parse(SelectYM.Split('-')[1]);
        //    int TempMonth = DateTime.Now.Month;
        //    if (SelectMonth > 12 && SelectMonth < 1)
        //    {
        //        SelectMonth = TempMonth;
        //    }
        //    TestSettingInfo TestSysModel = ReadTestSetting(CompanyId);
        //    int DayStart = int.Parse(TestSysModel.MonthStart);
        //    DateTime[] ReturnDate = new DateTime[2];
        //    if (int.Parse(SelectYear) == DateTime.Now.Year && SelectMonth == TempMonth && DayStart > DateTime.Now.Day)
        //    {
        //        ReturnDate[0] = DateTime.Parse(SelectYear + "-" + (SelectMonth - 1) + "-" + DayStart);
        //        ReturnDate[1] = ReturnDate[0].AddMonths(1);
        //    }
        //    else
        //    {
        //        ReturnDate[0] = DateTime.Parse(SelectYear + "-" + SelectMonth.ToString() + "-" + DayStart.ToString());
        //        ReturnDate[1] = ReturnDate[0].AddMonths(1);
        //    }
        //    return ReturnDate;
        //}
    }
}
