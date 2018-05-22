using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.IDAL;
using XueFuShop.Models;

namespace XueFuShop.BLL
{
    public sealed class QuestionBLL
    {
        private static readonly IQuestion dal = FactoryHelper.Instance<IQuestion>(Global.DataProvider, "QuestionDAL");

        public static QuestionInfo ReadQuestion(int Id)
        {
            return dal.ReadQuestion(Id);
        }

        public static int AddQuestion(QuestionInfo Model)
        {
            return dal.AddQuestion(Model);
        }

        public static void UpdateQuestion(QuestionInfo Model)
        {
            dal.UpdateQuestion(Model);
        }

        public static void DeleteQuestion(int Id)
        {
            dal.DeleteQuestion(Id.ToString());
        }

        public static void DeleteQuestion(string IdStr)
        {
            dal.DeleteQuestion(IdStr);
        }

        /// <summary>
        /// 通过课程ID删除考题
        /// </summary>
        /// <param name="CateId"></param>
        public static void DeleteQuestionByCateId(int CateId)
        {
            dal.DeleteQuestionByCateId(CateId);
        }

        public static void UpdateQuestionChecked(string Id, string Value)
        {
            if (Id != null && Id != ",")
            {
                dal.UpdateQuestionChecked(Id.Replace(" ",""), Value);
            }
        }

        /// <summary>
        /// 读取考题数量
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public static int ReadQuestionNum(string courseID)
        {
            return dal.ReadQuestionNum(courseID);
        }
        
        /// <summary>
        /// 根据productId读取符合条件随机抽取指定数量的考题创建xml文档
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="CourseId">题库ID</param>
        /// <param name="companyId">用户的公司ID</param>
        public static void ReadQuestionXmlList(int productId, string CourseId, int companyId, int userId)
        {
            TestSettingInfo TestSettingModel = TestSettingBLL.ReadTestSetting(companyId, productId);
            QuestionInfo Model = new QuestionInfo();
            Model.QuestionNum = " Top " + TestSettingModel.TestQuestionsCount.ToString() + " * ";
            Model.IdCondition = CourseId;
            Model.Condition = CompanyBLL.SystemCompanyId.ToString();//默认情况下都读取系统考题
            Model.Field = "CompanyId";
            List<QuestionInfo> TempQuestionList = ReadList(Model);

            //写入xml文件
            XmlHelper xmldoc = new XmlHelper();
            xmldoc.InsertNode("TestPaper");
            xmldoc.InsertElement("TestPaper", "CateId", productId.ToString());
            xmldoc.InsertNode("TestPaper", "Single", "");
            xmldoc.InsertNode("TestPaper", "Multi", "");
            xmldoc.InsertNode("TestPaper", "PanDuan", "");
            int QuestionIndex = 0;
            int SingleNum = 0;
            int MultiNum = 0;
            int PanDuanNum = 0;
            foreach (QuestionInfo Item in TempQuestionList)
            {
                string NodeName = "TestPaper";
                QuestionIndex = QuestionIndex + 1;
                if (Item.Style == "1")
                {
                    SingleNum = SingleNum + 1;
                    NodeName = NodeName + "/Single/QuestionInfo" + SingleNum.ToString();
                    xmldoc.InsertElement("TestPaper/Single", "QuestionInfo" + SingleNum.ToString(), "id", Item.QuestionId.ToString(), "");
                }
                else if (Item.Style == "2")
                {
                    MultiNum = MultiNum + 1;
                    NodeName = NodeName + "/Multi/QuestionInfo" + MultiNum.ToString();
                    xmldoc.InsertElement("TestPaper/Multi", "QuestionInfo" + MultiNum.ToString(), "id", Item.QuestionId.ToString(), "");
                }
                else if (Item.Style == "3")
                {
                    PanDuanNum = PanDuanNum + 1;
                    NodeName = NodeName + "/PanDuan/QuestionInfo" + PanDuanNum.ToString();
                    xmldoc.InsertElement("TestPaper/PanDuan", "QuestionInfo" + PanDuanNum.ToString(), "id", Item.QuestionId.ToString(), "");
                }
                xmldoc.InsertNode(NodeName, "Question", Item.Question);//.Replace("~","")

                //单项选择题四个选项随机排序
                string answer = Item.Answer.ToUpper();
                if (Item.Style == "1")
                {
                    string[,] optionArrray = new string[4, 2] { { "A", Item.A }, { "B", Item.B }, { "C", Item.C }, { "D", Item.D } };

                    optionArrray = RandomHelper.GetRandomOptionArray<string>(optionArrray, ref answer);
                    for (int i = 0; i <= optionArrray.GetUpperBound(0); i++)
                    {
                        xmldoc.InsertNode(NodeName, optionArrray[i, 0], optionArrray[i, 1]);
                    }
                }
                else
                {
                    xmldoc.InsertNode(NodeName, "A", Item.A);
                    xmldoc.InsertNode(NodeName, "B", Item.B);
                    xmldoc.InsertNode(NodeName, "C", Item.C);
                    xmldoc.InsertNode(NodeName, "D", Item.D);
                }
                xmldoc.InsertNode(NodeName, "Answer", answer);
                xmldoc.InsertNode(NodeName, "CustomerAnswer", "");
                xmldoc.InsertNode(NodeName, "Style", Item.Style);
                xmldoc.InsertNode(NodeName, "CompanyId", Item.CompanyId.ToString());
                xmldoc.InsertNode(NodeName, "QuestionId", Item.QuestionId.ToString());
            }
            xmldoc.InsertElement("TestPaper", "QuestionNum", QuestionIndex.ToString());
            xmldoc.InsertElement("TestPaper", "SingleNum", SingleNum.ToString());
            xmldoc.InsertElement("TestPaper", "MultiNum", MultiNum.ToString());
            xmldoc.InsertElement("TestPaper", "PanDunNum", PanDuanNum.ToString());

            //保存创建好的XML文档  
            if (Directory.Exists(ServerHelper.MapPath("~/xml")) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(ServerHelper.MapPath("~/xml"));
            }
            xmldoc.Save(ServerHelper.MapPath("~/xml/") + userId.ToString() + "_" + productId.ToString() + ".xml");
        }


        public static XmlNodeList ReadQuestionXmlList(int userID, int productID, string Style, int ID)
        {
            XmlHelper xmldoc = new XmlHelper(TestPaperBLL.ReadTestPaperPath(userID, productID));
            string NodeName = "TestPaper";
            if (Style == "1")
            {
                NodeName = NodeName + "/Single/QuestionInfo" + ID.ToString();
            }
            else if (Style == "2")
            {
                NodeName = NodeName + "/Multi/QuestionInfo" + ID.ToString();
            }
            else if (Style == "3")
            {
                NodeName = NodeName + "/PanDuan/QuestionInfo" + ID.ToString();
            }
            return xmldoc.ReadChildNodes(NodeName);
        }

        /// <summary>
        /// 根据CateId读取符合条件随机抽取指定数量的考题
        /// </summary>
        /// <param name="CateId">课程ID</param>
        /// <returns></returns>
        //public static List<QuestionInfo> ReadQuestionList(string CateId)
        //{
        //    TestSettingInfo TestSysModel = BLLTestSys.ReadTestSys(Cookies.Admin.GetCompanyID(false));
        //    QuestionInfo Model = new QuestionInfo();
        //    Model.QuestionNum = " * ";
        //    Model.IdCondition = CateId;
        //    if ((!CateId.Contains(",")) && Convert.ToString(BLLTestCate.ReadTestCateCache(int.Parse(CateId)).CourseContent) == "")
        //    {
        //        Model.Condition = BLLCompany.ReadCompanyIdList(BLLCompany.CompanyId.ToString());
        //        Model.Field = "CompanyId";
        //    }                
        //    return dal.ReadList(Model);
        //}

        public static string GetPaperQuestionId(QuestionInfo Model)
        {
            string TempStr = string.Empty;
            List<QuestionInfo> TempList = ReadList(Model);
            foreach (QuestionInfo Item in TempList)
            {
                TempStr = TempStr + "," + Item.QuestionId;
            }
            if (TempStr != string.Empty)
            {
                return TempStr.Substring(1);
            }
            return TempStr;
        }

        public static string GetQuestionId(QuestionInfo Model,int SingleNum,int MultiNum,int PanDuanNum)
        {
            string TempStr = string.Empty;
            List<QuestionInfo> TempList = ReadList(Model);
            foreach (QuestionInfo Item in TempList)
            {
                TempStr = TempStr + "," + Item.QuestionId;
            }
            if (TempStr != string.Empty)
            {
                return TempStr.Substring(1);
            }
            return TempStr;
        }

        //public static int ReadQuestionNum(string CateId)
        //{
        //    QuestionInfo Model = new QuestionInfo();
        //    Model.QuestionNum = " * ";
        //    Model.IdCondition = CateId;
        //    Model.Condition = CompanyBLL.SonCompanyId;
        //    Model.Field = "CompanyId";
        //    Model.CompanyId = 0;
        //    return dal.ReadList(Model).Count;
        //}

        public static List<QuestionInfo> ReadList(int CateId)
        {
            return dal.ReadList(CateId);
        }

        public static List<QuestionInfo> ReadList(QuestionInfo Model)
        {
            return dal.ReadList(Model);
        }
        public static List<QuestionInfo> ReadList(int currentPage, int pageSize, ref int count)
        {
            return dal.ReadList(currentPage, pageSize, ref count);
        }
        public static List<QuestionInfo> ReadList(QuestionInfo Model, int currentPage, int pageSize, ref int count)
        {
            return dal.ReadList(Model, currentPage, pageSize, ref count);
        }
    }
}
