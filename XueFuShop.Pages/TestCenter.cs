using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;
using System.IO;
using System.Xml;
using XueFuShop.BLL;
using XueFuShop.Log;

namespace XueFuShop.Pages
{
    public class TestCenter : UserBasePage
    {

        protected int productID = RequestHelper.GetQueryString<int>("ProductID");
        protected StringBuilder questionStyleHtml = new StringBuilder();
        protected StringBuilder questionIndexHtml = new StringBuilder();
        protected StringBuilder paperInfoHtml = new StringBuilder();
        protected string paperName = string.Empty;
        protected string testTime = string.Empty;

        protected override void PageLoad()
        {
            int companyID = base.UserCompanyID;
            int userID = base.UserID;
            string filePath = TestPaperBLL.ReadTestPaperPath(userID, productID);
            base.PageLoad();
            base.ClearCache();
            base.Title = "��������";
            if (productID == int.MinValue)
            {
                ScriptHelper.Alert("���ڡ��γ��б���ѡ��γ̺󣬽��п��ԣ�");
            }
            TestSettingInfo testSetting = TestSettingBLL.ReadTestSetting(companyID, productID);
            ProductInfo product = ProductBLL.ReadProduct(productID);

            //if (!CompanyBLL.IsTest)
            //{
            //    Response.Write("<script>alert('�ù�˾�������ԣ�����ϵ����Ա��');var DG = frameElement.lhgDG;DG.cancel();</script>");
            //    Response.End();
            //}
            RenZhengCateInfo renZhengProduct = RenZhengCateBLL.ReadTestCateByID(product.ID);
            if (renZhengProduct != null)
            {
                PostApprover postApprover = new PostApprover(PostPassBLL.ReadPostPassList(new PostPassInfo() { UserId = userID, IsRZ = 1 }));
                if (!postApprover.IsTest(renZhengProduct.PostId))
                {
                    Response.Write("<script>alert('������� " + PostBLL.ReadPost(postApprover.NextPostID).PostName + " �ĸ�λ�ۺ���֤��');window.close();</script>");
                    Response.End();
                }
            }
            if (testSetting.TestStartTime.HasValue && testSetting.TestEndTime.HasValue)
            {
                //if (product.ClassID != 4387)
                //{
                //    if (Convert.ToDateTime(testSetting.TestStartTime) > DateTime.Now || Convert.ToDateTime(testSetting.TestEndTime) < DateTime.Now)
                //    {
                //        Response.Write("<script>alert('���� ���ſγ� �趨�Ŀ���ʱ���ڣ�');var DG = frameElement.lhgDG;DG.cancel();</script>");
                //        Response.End();
                //    }
                //}
                //else
                //{
                if (testSetting.TestStartTime > DateTime.Now || testSetting.TestEndTime < DateTime.Now) //&& Convert.ToDateTime(testSetting.TestStartTime).Minute <= DateTime.Now.Minute && Convert.ToDateTime(testSetting.TestEndTime).Minute >= DateTime.Now.Minute
                {
                    Response.Write("<script>alert('���� ���ſγ� �趨�Ŀ���ʱ���ڣ�');window.close();</script>");
                    Response.End();
                }
                //}
            }

            TestPaperInfo PaperModel = new TestPaperInfo();
            PaperModel.CateIdCondition = productID.ToString();
            PaperModel.UserIdCondition = userID.ToString();
            List<TestPaperInfo> PaperList = TestPaperBLL.NewReadList(PaperModel);
            if (PaperList.Count > 0)
            {
                foreach (TestPaperInfo Item in PaperList)
                {
                    if (Item.IsPass == 1)// && Item.CateId != 5368�������۹���Ӧ����˵�� ��Ҫ��ο���
                    {
                        Response.Write("<script>alert('����ͨ������ѡ�������γ̣�');</script>");
                        Response.End();
                    }
                    if ((DateTime.Now - Item.TestDate).TotalHours < testSetting.TestInterval)
                    {
                        if (testSetting.TestStartTime.HasValue || testSetting.TestEndTime.HasValue)
                            Response.Write("<script>alert('���Ѿ��μӹ����ԣ��ݲ����ؿ���');window.close();</script>");
                        else
                            Response.Write("<script>alert('����" + testSetting.TestInterval + "Сʱ������ؿ�����ѡ�������γ̣�');window.close();</script>");
                        Response.End();
                    }
                }
            }

            if ((File.Exists(filePath) && (DateTime.Now - File.GetLastWriteTime(filePath)).TotalHours < testSetting.TestInterval))//TempPaperInfo != null && (DateTime.Now - TempPaperInfo.TestDate).TotalHours < 72
            {
                bool HaveTest = false;
                XmlHelper XmlDoc1 = new XmlHelper(filePath);
                for (int StyleId = 1; StyleId <= 3; StyleId++)
                {
                    string NodeName = "TestPaper";
                    if (StyleId == 1)
                    {
                        NodeName = NodeName + "/Single";
                    }
                    else if (StyleId == 2)
                    {
                        NodeName = NodeName + "/Multi";
                    }
                    else if (StyleId == 3)
                    {
                        NodeName = NodeName + "/PanDuan";
                    }
                    //�ж����Ϳ����Ƿ��п���
                    XmlNode Node = XmlDoc1.ReadNode(NodeName);
                    if (Node != null && Node.HasChildNodes)
                    {
                        XmlNodeList NodeList = XmlDoc1.ReadChildNodes(NodeName);
                        //�����ڵ�
                        foreach (XmlNode node in NodeList)
                        {
                            if (!string.IsNullOrEmpty(node.ChildNodes[6].InnerText))
                            {
                                HaveTest = true;
                            }
                        }
                    }
                }
                if (HaveTest)
                {
                    AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("TestPaperRecord"), ShopLanguage.ReadLanguage("TestPaper"), productID);
                    TestPaperInfo testpaper = TestPaperBLL.CalcTestResult(companyID, userID, productID);
                    if (testpaper.IsPass == 1)
                    {
                        PostPassBLL.CheckPostPass(UserID, productID);

                        //�������۹���Ӧ����˵�� �γ̴���
                        //if (productID == 5368)
                        //    TestSettingBLL.SpecialTestHandle(userID, int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId")));
                    }
                    ResponseHelper.Write("<script>alert('���ϴ�δ��ɽ�����ʱϵͳ�Զ�Ϊ���������÷֣�" + testpaper.Scorse + "�֡�');window.close();</script>");
                    Response.End();
                }
            }

            //������ֹ��ο���ͬһ�γ̿���
            //TestSettingBLL.TestBegin(userID, productID);

            paperName = product.Name;
            testTime = (testSetting.TestTimeLength * 60).ToString();

            QuestionBLL.ReadQuestionXmlList(productID, product.Accessory, companyID, userID);
            XmlHelper XmlDoc = new XmlHelper(filePath);


            paperInfoHtml.AppendLine("<p>����ʱ����" + testSetting.TestTimeLength.ToString() + " ����</p>");
            paperInfoHtml.AppendLine("<p>�Ծ��ܷ֣�" + testSetting.PaperScore.ToString() + " ��</p>");
            paperInfoHtml.AppendLine("<p>������������ " + XmlDoc.ReadAttribute("TestPaper", "QuestionNum") + " �������� ");
            string TempNum = XmlDoc.ReadAttribute("TestPaper", "SingleNum");
            if (int.Parse(TempNum) > 0) paperInfoHtml.Append("����ѡ���⣺" + TempNum + " ����");
            TempNum = XmlDoc.ReadAttribute("TestPaper", "MultiNum");
            if (int.Parse(TempNum) > 0) paperInfoHtml.Append("����ѡ���⣺" + TempNum + " ����");
            TempNum = XmlDoc.ReadAttribute("TestPaper", "PanDunNum");
            if (int.Parse(TempNum) > 0) paperInfoHtml.Append("�ж��⣺" + TempNum + " ����");
            paperInfoHtml.Append("</p>");

            int NoteCount = XmlDoc.ReadChildNodes("TestPaper/Single").Count;
            if (NoteCount > 0)
            {
                questionStyleHtml.AppendLine("<li data-id=\"1\"><a href=\"javascript:void(0);\">" + EnumHelper.ReadEnumChineseName<QuestionType>(1) + "</a></li>");
                questionIndexHtml.AppendLine("<ul data-style=\"1\" class=\"clearfix\" id=\"radio\">");
                for (int i = 1; i <= NoteCount; i++)
                {
                    questionIndexHtml.AppendLine("<li><a href=\"javascript:void(0);\">" + (i) + "</a></li>");
                }
                questionIndexHtml.AppendLine("</ul>");
            }

            NoteCount = XmlDoc.ReadChildNodes("TestPaper/Multi").Count;
            if (NoteCount > 0)
            {
                questionStyleHtml.AppendLine("<li data-id=\"2\"><a href=\"javascript:void(0);\">" + EnumHelper.ReadEnumChineseName<QuestionType>(2) + "</a></li>");
                questionIndexHtml.AppendLine("<ul data-style=\"2\" class=\"clearfix\" id=\"checkbox\">");
                for (int i = 1; i <= NoteCount; i++)
                {
                    questionIndexHtml.AppendLine("<li><a href=\"javascript:void(0);\">" + (i) + "</a></li>");
                }
                questionIndexHtml.AppendLine("</ul>");
            }

            NoteCount = XmlDoc.ReadChildNodes("TestPaper/PanDuan").Count;
            if (NoteCount > 0)
            {
                questionStyleHtml.AppendLine("<li data-id=\"3\"><a href=\"javascript:void(0);\">" + EnumHelper.ReadEnumChineseName<QuestionType>(3) + "</a></li>");
                questionIndexHtml.AppendLine("<ul data-style=\"3\" class=\"clearfix\" id=\"judge\">");
                for (int i = 1; i <= NoteCount; i++)
                {
                    questionIndexHtml.AppendLine("<li><a href=\"javascript:void(0);\">" + (i) + "</a></li>");
                }
                questionIndexHtml.AppendLine("</ul>");
            }

            UserLogBLL.AddUserLog(ShopLanguage.ReadLanguage("StartTest"), ShopLanguage.ReadLanguage("TestPaper"), paperName);
            UserLogBLL.AddUserLog(Request.Browser.Browser + "|" + Request.Browser.Version, productID);
        }
    }
}
