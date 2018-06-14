using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.Pages;
using MobileEMS.BLL;

namespace MobileEMS
{
    public partial class TestCenter : MobileUserBasePage
    {
        protected int productID = RequestHelper.GetQueryString<int>("CateId");
        protected string filePath = string.Empty;
        protected int endTimer = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.ClearCache();

            int companyID = base.UserCompanyID;
            int userID = base.UserID;
            filePath = TestPaperBLL.ReadTestPaperPath(userID, productID);

            if (!IsPostBack)
            {
                if (productID < 0)
                {
                    ScriptHelper.Alert("ѡ��γ̺󣬽��п��ԣ�");
                    Response.End();
                }
                TestSettingInfo testSetting = TestSettingBLL.ReadTestSetting(companyID, productID);
                ProductInfo product = ProductBLL.ReadProduct(productID);

                RenZhengCateInfo renZhengProduct = RenZhengCateBLL.ReadTestCateByID(product.ID);
                if (renZhengProduct != null)
                {
                    PostApprover postApprover = new PostApprover(PostPassBLL.ReadPostPassList(new PostPassInfo() { UserId = userID, IsRZ = 1 }));
                    if (!postApprover.IsTest(renZhengProduct.PostId))
                    {
                        Response.Write("<script>alert('������� " + PostBLL.ReadPost(postApprover.NextPostID).PostName + " �ĸ�λ�ۺ���֤��');history.go(-1);</script>");
                        Response.End();
                    }
                }

                if (testSetting.TestStartTime != null && testSetting.TestEndTime != null)
                {
                    if (testSetting.TestStartTime > DateTime.Now || testSetting.TestEndTime < DateTime.Now)
                    {
                        Response.Write("<script>alert('���� ���ſγ� �趨�Ŀ���ʱ���ڣ�');history.go(-1);</script>");
                        Response.End();
                    }
                }
                TestPaperInfo PaperModel = new TestPaperInfo();
                PaperModel.CateIdCondition = productID.ToString();
                PaperModel.UserIdCondition = userID.ToString();
                List<TestPaperInfo> PaperList = TestPaperBLL.NewReadList(PaperModel);
                if (PaperList.Count > 0)
                {
                    foreach (TestPaperInfo Item in PaperList)
                    {
                        if (Item.IsPass == 1)// && Item.CateId != 5368//�������۹���Ӧ����˵�� ��Ҫ��ο���
                        {
                            Response.Write("<script>alert('����ͨ������ѡ�������γ̣�');</script>");
                            Response.End();
                        }
                        if ((DateTime.Now - Item.TestDate).TotalHours < testSetting.TestInterval)
                        {
                            if (testSetting.TestStartTime != null || testSetting.TestEndTime != null)
                                Response.Write("<script>alert('���Ѿ��μӹ����ԣ��ݲ����ؿ���');window.close();</script>");
                            else
                                Response.Write("<script>alert('����" + testSetting.TestInterval + "Сʱ������ؿ�����ѡ�������γ̣�');history.go(-1);</script>");
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
                        ResponseHelper.Write("<script>alert('���ϴ�δ��ɽ�����ʱϵͳ�Զ�Ϊ���������÷֣�" + testpaper.Scorse + "�֡�');window.location.href='CourseCenter.aspx';</script>");
                        Response.End();
                    }
                }


                TestName.Text = product.Name;
                endTimer = testSetting.TestTimeLength * 60;

                QuestionBLL.ReadQuestionXmlList(productID, product.Accessory, companyID, userID);

                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("StartTest"), ShopLanguage.ReadLanguage("TestCate"), productID);
                AdminLogBLL.AddAdminLog(Request.Browser.Browser + "|" + Request.Browser.Version, productID);
            }
        }
    }
}
