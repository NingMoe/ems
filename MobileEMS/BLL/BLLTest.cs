using System;
using System.Xml;
using System.IO;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace MobileEMS.BLL
{
    public sealed class BLLTest
    {
        public static string RecordTest(int CateId)
        {
            string FilePath=ServerHelper.MapPath("~/xml/" + Cookies.User.GetUserID(true).ToString()+ "_" + CateId.ToString() + ".xml");
            if (File.Exists(FilePath))
            {
                XmlHelper XmlDoc = new XmlHelper(FilePath);

                int QuestionNum = int.Parse(XmlDoc.ReadAttribute("TestPaper", "QuestionNum"));

                decimal Scorse = 0; //�Ծ�÷�
                int RightNum = 0;  //��ȷ����Ŀ����

                //��xml���ȡ�𰸣�����У��
                string QuestionList = string.Empty;
                string UserAnswerList = string.Empty;
                for (int StyleId = 1; StyleId <= 3; StyleId++)
                {
                    string NodeName = GetTestPaperStyleNodeName(StyleId);
                    //�ж����Ϳ����Ƿ��п���
                    XmlNode Node = XmlDoc.ReadNode(NodeName);
                    if (Node != null && Node.HasChildNodes)
                    {
                        XmlNodeList NodeList = XmlDoc.ReadChildNodes(NodeName);
                        //�����ڵ�
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
                TestSettingInfo Model = TestSettingBLL.ReadTestSetting(Cookies.User.GetCompanyID(true),CateId);
                if (QuestionNum == 0)
                {
                    Scorse = 0;
                }
                else
                {
                    Scorse = Convert.ToDecimal(Math.Round((decimal.Parse(Model.PaperScore.ToString()) * RightNum / QuestionNum), 1));//����������ָܷ�׼��
                }
                if (QuestionList != string.Empty && QuestionList.StartsWith(","))
                {
                    QuestionList = QuestionList.Substring(1);
                    UserAnswerList = UserAnswerList.Substring(1);
                }

                TestPaperInfo PaperModel = new TestPaperInfo();
                PaperModel.CateId = CateId;
                PaperModel.CompanyId = Cookies.User.GetCompanyID(true);
                PaperModel.UserId = Cookies.User.GetUserID(true);
                PaperModel.QuestionId = QuestionList;
                PaperModel.Answer = UserAnswerList;
                PaperModel.Scorse = Scorse;
                if (Scorse >= Model.LowScore) PaperModel.Point = 2;
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("ApplyTestPaper"), ShopLanguage.ReadLanguage("TestCate"), CateId);
                TestPaperBLL.AddPaper(PaperModel);
                File.SetLastWriteTime(FilePath, DateTime.Now);

                return "TestPaperShow.aspx?Action=TestPaperResult&CompanyId=" + ProductBLL.ReadProduct(CateId).CompanyID.ToString() + "&CateId=" + CateId.ToString() + "&Scorse=" + Scorse.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// ȡ������·������
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
    }
}
