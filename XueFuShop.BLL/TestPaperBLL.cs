using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.IDAL;
using XueFuShop.Models;
using System.IO;
using System.Xml;
using XueFuShop.Log;

namespace XueFuShop.BLL
{
    public sealed class TestPaperBLL
    {
        private static readonly ITestPaper dal = FactoryHelper.Instance<ITestPaper>(Global.DataProvider, "TestPaperDAL");
        

        public static TestPaperInfo ReadPaper(int TestPaperId)
        {
            TestPaperInfo Model = new TestPaperInfo();
            Model.TestPaperId = TestPaperId;
            return dal.ReadPaper(Model);
        }

        public static TestPaperInfo ReadPaper(int UserId, int CateId)
        {
            return dal.ReadPaper(UserId, CateId);
        }

        public static int AddPaper(TestPaperInfo Model)
        {
            return dal.AddPaper(Model);
        }

        public static void UpdatePaper(TestPaperInfo Model)
        {
            dal.UpdatePaper(Model);
        }

        /// <summary>
        /// 返回试卷路径
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static string ReadTestPaperPath(int userID, int productID)
        {
            return ServerHelper.MapPath("~/xml/" + userID.ToString() + "_" + productID.ToString() + ".xml");
        }

        /// <summary>
        /// 根据用户ID更新试卷的公司ID
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="CompanyId"></param>
        public static void UpdatePaperCompanyId(int UserId, int CompanyId)
        {
            dal.UpdatePaperCompanyId(UserId, CompanyId);
        }

        public static void DeletePaper(int Id)
        {
            dal.DeletePaper(Id);
        }

        /// <summary>
        /// 根据用户ID，删除考试记录
        /// </summary>
        /// <param name="UserId"></param>
        public static void DeletePaperByUserId(int userID)
        {
            DeletePaperByUserID(userID.ToString());
        }

        /// <summary>
        /// 根据用户ID串，删除考试记录
        /// </summary>
        /// <param name="UserId"></param>
        public static void DeletePaperByUserID(string userID)
        {
            dal.DeletePaperByUserID(userID);
        }

        /// <summary>
        /// 根据用户ID串，恢复考试记录
        /// </summary>
        /// <param name="UserId"></param>
        public static void RecoveryPaperByUserID(string userID)
        {
            dal.RecoveryPaperByUserID(userID);
        }

        public static List<TestPaperInfo> ReadReportList(string CompanyId, DateTime StartDate, DateTime EndDate)
        {
            return ReadReportList(CompanyId, string.Empty, StartDate, EndDate);
        }

        public static List<TestPaperInfo> ReadReportList(string CompanyId, string CateId, DateTime StartDate, DateTime EndDate)
        {
            TestPaperInfo TestPaperModel = new TestPaperInfo();
            TestPaperModel.TestMinDate = StartDate;
            TestPaperModel.TestMaxDate = EndDate;
            TestPaperModel.CompanyIdCondition = CompanyId.ToString();
            TestPaperModel.CateIdCondition = CateId;
            return NewReadList(TestPaperModel);
        }

        /// <summary>
        /// 从TempList列表中找出第一个符合条件的记录
        /// </summary>
        /// <param name="TempList"></param>
        /// <param name="UserId"></param>
        /// <param name="CateId"></param>
        /// <returns></returns>
        public static TestPaperInfo ReadReportInfo(List<TestPaperInfo> TempList, int UserId, int CateId)
        {
            List<TestPaperInfo> TempTestPaperList = TempList;
            foreach (TestPaperInfo Item in TempTestPaperList)
            {
                if (Item.UserId == UserId && Item.CateId == CateId)
                {
                    return Item;
                }
            }
            return null;
        }

        //public static string ReadListStr(int UserId, int Type)
        //{
        //    return ReadListStr(UserId, DateTime.Now.AddDays(1), Type);
        //}

        /// <summary>
        /// 返回指定用户的成绩列表字符串
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Type">0为不及格的成绩，1为及格的成绩，其它为所有成绩</param>
        /// <returns></returns>
        //public static string ReadListStr(int UserId, DateTime EndDate, int Type)
        //{
        //    return ReadListStr(UserBLL.CurrentUserCompanyId, UserId, EndDate, Type);
        //}

        /// <summary>
        /// 返回指定用户的成绩列表字符串
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Type">0为不及格的成绩，1为及格的成绩，其它为所有成绩</param>
        /// <returns></returns>
        //public static string ReadListStr(int UserId, DateTime StartDate, DateTime EndDate, int Type)
        //{
        //    return ReadListStr(UserBLL.CurrentUserCompanyId, UserId, StartDate, EndDate, Type);
        //}

        /// <summary>
        /// 返回指定用户的成绩列表字符串
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Type">0为不及格的成绩，1为及格的成绩，其它为所有成绩</param>
        /// <returns></returns>
        //public static string ReadListStr(int CompanyId, int UserId, DateTime EndDate, int Type)
        //{
        //    List<TestPaperInfo> TempList = ReadList(UserId, EndDate);
        //    string ReturnStr = string.Empty;
        //    foreach (TestPaperInfo info in TempList)
        //    {
        //        if (!StringHelper.CompareSingleString(ReturnStr, info.CateId.ToString()))
        //        {
        //            TestSettingInfo TestSettingModel = TestSettingBLL.ReadTestSetting(CompanyId, info.CateId);
        //            if (Type == 1 && info.Scorse >= TestSettingModel.LowScore)
        //            {
        //                ReturnStr = ReturnStr + "," + info.CateId.ToString();
        //            }
        //            else if (Type == 0 && info.Scorse < TestSettingModel.LowScore)
        //            {
        //                ReturnStr = ReturnStr + "," + info.CateId.ToString();
        //            }
        //            else if (Type != 0 && Type != 1)
        //            {
        //                ReturnStr = ReturnStr + "," + info.CateId.ToString();
        //            }
        //        }
        //    }
        //    if (ReturnStr.StartsWith(",")) return ReturnStr.Substring(1);
        //    return ReturnStr;
        //}

        /// <summary>
        /// 返回指定用户的成绩列表字符串
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Type">0为不及格的成绩，1为及格的成绩，其它为所有成绩</param>
        /// <returns></returns>
        //public static string ReadListStr(int CompanyId, int UserId, DateTime StartDate, DateTime EndDate, int Type)
        //{
        //    List<TestPaperInfo> TempList = ReadList(UserId, StartDate, EndDate, Type);
        //    string ReturnStr = string.Empty;
        //    foreach (TestPaperInfo info in TempList)
        //    {
        //        if (!StringHelper.CompareSingleString(ReturnStr, info.CateId.ToString()))
        //        {
        //            TestSettingInfo TestSettingModel = TestSettingBLL.ReadTestSetting(CompanyId, info.CateId);
        //            if (Type == 1 && info.Scorse >= TestSettingModel.LowScore)
        //            {
        //                ReturnStr = ReturnStr + "," + info.CateId.ToString();
        //            }
        //            else if (Type == 0 && info.Scorse < TestSettingModel.LowScore)
        //            {
        //                ReturnStr = ReturnStr + "," + info.CateId.ToString();
        //            }
        //            else if (Type != 0 && Type != 1)
        //            {
        //                ReturnStr = ReturnStr + "," + info.CateId.ToString();
        //            }
        //        }
        //    }
        //    if (ReturnStr.StartsWith(",")) return ReturnStr.Substring(1);
        //    return ReturnStr;
        //}

        /// <summary>
        /// 返回课程ID字符串
        /// </summary>
        /// <param name="testPaperList"></param>
        /// <returns></returns>
        public static string ReadCourseIDStr(List<TestPaperInfo> testPaperList)
        {
            string courseIDStr = string.Empty;
            foreach (TestPaperInfo info in testPaperList)
            {
                if (!StringHelper.CompareSingleString(courseIDStr, info.CateId.ToString()))
                {
                    courseIDStr += "," + info.CateId.ToString();

                }
            }
            if (courseIDStr.StartsWith(",")) return courseIDStr.Substring(1);
            return courseIDStr;
        }

        /// <summary>
        /// 返回指定用户的成绩列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<TestPaperInfo> ReadList(int userID)
        {
            return ReadList(userID, string.Empty, DateTime.MinValue, DateTime.MinValue, int.MinValue);
        }

        /// <summary>
        /// 返回指定用户的成绩列表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="isPass"></param>
        /// <returns></returns>
        public static List<TestPaperInfo> ReadList(int userID, int isPass)
        {
            return ReadList(userID, string.Empty, DateTime.MinValue, DateTime.MinValue, isPass);
        }

        /// <summary>
        /// 返回指定用户的成绩列表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static List<TestPaperInfo> ReadList(int userID, string productID)
        {
            return ReadList(userID, productID, DateTime.MinValue, DateTime.MinValue, int.MinValue);
        }

        /// <summary>
        /// 返回指定用户的成绩列表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="productID"></param>
        /// <param name="isPass"></param>
        /// <returns></returns>
        public static List<TestPaperInfo> ReadList(int userID, string productID, int isPass)
        {
            return ReadList(userID, productID, DateTime.MinValue, DateTime.MinValue, isPass);
        }

        /// <summary>
        /// 返回指定用户的成绩列表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static List<TestPaperInfo> ReadList(int userID, DateTime endDate)
        {
            return ReadList(userID, string.Empty, DateTime.MinValue, endDate, int.MinValue);
        }

        /// <summary>
        /// 返回指定用户的成绩列表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="endDate"></param>
        /// <param name="isPass"></param>
        /// <returns></returns>
        public static List<TestPaperInfo> ReadList(int userID, DateTime endDate, int isPass)
        {
            return ReadList(userID, string.Empty, DateTime.MinValue, endDate, isPass);
        }

        /// <summary>
        /// 返回指定用户的成绩列表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isPass"></param>
        /// <returns></returns>
        public static List<TestPaperInfo> ReadList(int userID, DateTime startDate, DateTime endDate, int isPass)
        {
            return ReadList(userID, string.Empty, startDate, endDate, isPass);
        }

        /// <summary>
        /// 返回指定用户的成绩列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static List<TestPaperInfo> ReadList(int userID, string productID, DateTime startDate, DateTime endDate, int isPass)
        {
            TestPaperInfo Model = new TestPaperInfo();
            Model.UserId = userID;
            Model.TestMinDate = startDate;
            Model.TestMaxDate = endDate;
            Model.CateIdCondition = productID;
            Model.IsPass = isPass;
            Model.OrderByCondition = " CateId asc,Scorse desc";
            return dal.NewReadList(Model);
        }

        /// <summary>
        /// 读取最早的一次考试记录
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static TestPaperInfo ReadTheOldTestPaperInfo(int UserId)
        {
            return dal.ReadTheOldTestPaperInfo(UserId);
        }

        /// <summary>
        /// 读取最早的一次考试记录
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static List<TestPaperReportInfo> ReadTheFirstRecordList(string companyID)
        {
            return dal.ReadTheFirstRecordList(companyID);
        }

        /// <summary>
        /// 读取最新的成绩列表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public static List<TestPaperReportInfo> ReadThelatestList(int userID, string courseID)
        {
            return dal.ReadThelatestList(userID, courseID);
        }

        /// <summary>
        /// 读取最新的成绩
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public static TestPaperReportInfo ReadTheLatestPaper(int userID, int courseID)
        {
            TestPaperReportInfo testPaper = new TestPaperReportInfo();
            List<TestPaperReportInfo> theLastestList = ReadThelatestList(userID, courseID.ToString());
            if (theLastestList.Count > 0)
            {
                testPaper = theLastestList[0];
            }
            return testPaper;
        }

        /// <summary>
        /// 读取最好的成绩列表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public static Dictionary<int, float> ReadTheBestList(int userID, string courseID)
        {
            return dal.ReadTheBestList(userID, courseID);
        }

        public static List<TestPaperInfo> ReadList(TestPaperInfo Model)
        {
            return dal.ReadList(Model);
        }
        public static List<TestPaperInfo> NewReadList(TestPaperInfo Model)
        {
            return dal.NewReadList(Model);
        }
        public static List<TestPaperInfo> ReadList(int currentPage, int pageSize, ref int count)
        {
            return dal.ReadList(currentPage, pageSize, ref count);
        }
        public static List<TestPaperInfo> ReadList(TestPaperInfo Model, int currentPage, int pageSize, ref int count)
        {
            return dal.ReadList(Model, currentPage, pageSize, ref count);
        }

        /// <summary>
        /// 取得题型路径名称
        /// </summary>
        /// <param name="StyleId"></param>
        /// <returns></returns>
        public static string GetTestPaperStyleNodeName(int StyleId)
        {
            string NodeName = "TestPaper";
            switch (StyleId)
            {
                case 1:
                    NodeName = NodeName + "/Single";
                    break;
                case 2:
                    NodeName = NodeName + "/Multi";
                    break;
                case 3:
                    NodeName = NodeName + "/PanDuan";
                    break;
            }
            return NodeName;
        }

        /// <summary>
        /// 计算考试成绩
        /// </summary>
        /// <returns></returns>
        public static TestPaperInfo CalcTestResult(int companyID, int userID, int productID)
        {
            TestPaperInfo PaperModel = new TestPaperInfo();
            string filePath = ReadTestPaperPath(userID, productID);
            TestPaperReportInfo testPaper = ReadTheLatestPaper(userID, productID);
            if (File.Exists(filePath) && (testPaper.TestDate == DateTime.MinValue || (DateTime.Now - testPaper.TestDate).TotalHours >= ShopConfig.ReadConfigInfo().TestInterval))
            {
                XmlHelper XmlDoc = new XmlHelper(filePath);

                int QuestionNum = int.Parse(XmlDoc.ReadAttribute("TestPaper", "QuestionNum"));

                decimal Scorse = 0; //试卷得分
                int RightNum = 0;  //正确的题目数量

                //从xml里读取答案，进行校卷
                string QuestionList = string.Empty;
                string UserAnswerList = string.Empty;
                for (int StyleId = 1; StyleId <= 3; StyleId++)
                {
                    string NodeName = GetTestPaperStyleNodeName(StyleId);
                    //判断题型库里是否有考题
                    XmlNode Node = XmlDoc.ReadNode(NodeName);
                    if (Node != null && Node.HasChildNodes)
                    {
                        XmlNodeList NodeList = XmlDoc.ReadChildNodes(NodeName);
                        //遍历节点
                        foreach (XmlNode node in NodeList)
                        {
                            QuestionList = QuestionList + "," + node.ChildNodes[9].InnerText;
                            UserAnswerList = UserAnswerList + "," + node.ChildNodes[6].InnerText;
                            if (node.ChildNodes[6].InnerText.ToLower() == node.ChildNodes[5].InnerText.ToLower())
                            {
                                RightNum = RightNum + 1;
                            }
                        }
                    }

                }

                //UserInfo user = UserBLL.ReadUser(userID);
                TestSettingInfo testSetting = TestSettingBLL.ReadTestSetting(companyID, productID);
                if (QuestionNum == 0)
                    Scorse = 0;
                else
                    Scorse = Convert.ToDecimal(Math.Round((decimal.Parse(testSetting.PaperScore.ToString()) * RightNum / QuestionNum), 1));//这样计算的总分更准备
                if (QuestionList != string.Empty && QuestionList.StartsWith(","))
                {
                    QuestionList = QuestionList.Substring(1);
                    UserAnswerList = UserAnswerList.Substring(1);
                }

                PaperModel.CateId = productID;
                PaperModel.PaperName = ProductBLL.ReadProduct(productID).Name;
                PaperModel.CompanyId = companyID;
                PaperModel.UserId = userID;
                PaperModel.QuestionId = QuestionList;
                PaperModel.Answer = UserAnswerList;
                PaperModel.Scorse = Scorse;
                if (Scorse >= testSetting.LowScore)
                {
                    PaperModel.Point = 2;
                    PaperModel.IsPass = 1;
                }
                else
                {
                    PaperModel.IsPass = 0;
                }

                UserLogBLL.AddUserLog(ShopLanguage.ReadLanguage("ApplyTest"), ShopLanguage.ReadLanguage("TestPaper"), ProductBLL.ReadProduct(productID).Name);
                TestPaperBLL.AddPaper(PaperModel);
                File.SetLastWriteTime(filePath, DateTime.Now);
            }
            if ((testPaper.TestDate > DateTime.MinValue && (DateTime.Now - testPaper.TestDate).TotalHours < ShopConfig.ReadConfigInfo().TestInterval))
            {
                PaperModel.Scorse = testPaper.Score;
            }

            //解除本课程的考试限制
            //TestSettingBLL.TestEnd(userID, productID);
            
            return PaperModel;
        }

        public sealed class MonthReportInfo
        {
            private string _UserName;
            public string UserName
            {
                get { return _UserName; }
                set { _UserName = value; }
            }

            private int _UserId;
            public int UserId
            {
                get { return _UserId; }
                set { _UserId = value; }
            }

            private int _Num;
            public int Num
            {
                get { return _Num; }
                set { _Num = value; }
            }

            private decimal _Scorse;
            public decimal Scorse
            {
                get { return _Scorse; }
                set { _Scorse = value; }
            }

            private decimal _Point;
            public decimal Point
            {
                get { return _Point; }
                set { _Point = value; }
            }
        }
    }
}
