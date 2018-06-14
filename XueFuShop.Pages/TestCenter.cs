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
            base.Title = "考试中心";
            if (productID == int.MinValue)
            {
                ScriptHelper.Alert("请在“课程列表”中选择课程后，进行考试！");
            }
            TestSettingInfo testSetting = TestSettingBLL.ReadTestSetting(companyID, productID);
            ProductInfo product = ProductBLL.ReadProduct(productID);

            //if (!CompanyBLL.IsTest)
            //{
            //    Response.Write("<script>alert('该公司不允许考试，请联系管理员！');var DG = frameElement.lhgDG;DG.cancel();</script>");
            //    Response.End();
            //}
            RenZhengCateInfo renZhengProduct = RenZhengCateBLL.ReadTestCateByID(product.ID);
            if (renZhengProduct != null)
            {
                PostApprover postApprover = new PostApprover(PostPassBLL.ReadPostPassList(new PostPassInfo() { UserId = userID, IsRZ = 1 }));
                if (!postApprover.IsTest(renZhengProduct.PostId))
                {
                    Response.Write("<script>alert('请先完成 " + PostBLL.ReadPost(postApprover.NextPostID).PostName + " 的岗位综合认证！');window.close();</script>");
                    Response.End();
                }
            }
            if (testSetting.TestStartTime.HasValue && testSetting.TestEndTime.HasValue)
            {
                //if (product.ClassID != 4387)
                //{
                //    if (Convert.ToDateTime(testSetting.TestStartTime) > DateTime.Now || Convert.ToDateTime(testSetting.TestEndTime) < DateTime.Now)
                //    {
                //        Response.Write("<script>alert('不在 该门课程 设定的考试时间内！');var DG = frameElement.lhgDG;DG.cancel();</script>");
                //        Response.End();
                //    }
                //}
                //else
                //{
                if (testSetting.TestStartTime > DateTime.Now || testSetting.TestEndTime < DateTime.Now) //&& Convert.ToDateTime(testSetting.TestStartTime).Minute <= DateTime.Now.Minute && Convert.ToDateTime(testSetting.TestEndTime).Minute >= DateTime.Now.Minute
                {
                    Response.Write("<script>alert('不在 该门课程 设定的考试时间内！');window.close();</script>");
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
                    if (Item.IsPass == 1)// && Item.CateId != 5368孟特销售工具应用与说明 需要多次考试
                    {
                        Response.Write("<script>alert('您已通过，请选择其它课程！');</script>");
                        Response.End();
                    }
                    if ((DateTime.Now - Item.TestDate).TotalHours < testSetting.TestInterval)
                    {
                        if (testSetting.TestStartTime.HasValue || testSetting.TestEndTime.HasValue)
                            Response.Write("<script>alert('您已经参加过考试，暂不能重考！');window.close();</script>");
                        else
                            Response.Write("<script>alert('考完" + testSetting.TestInterval + "小时后才能重考，请选择其它课程！');window.close();</script>");
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
                    //判断题型库里是否有考题
                    XmlNode Node = XmlDoc1.ReadNode(NodeName);
                    if (Node != null && Node.HasChildNodes)
                    {
                        XmlNodeList NodeList = XmlDoc1.ReadChildNodes(NodeName);
                        //遍历节点
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

                        //孟特销售工具应用与说明 课程处理
                        //if (productID == 5368)
                        //    TestSettingBLL.SpecialTestHandle(userID, int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId")));
                    }
                    ResponseHelper.Write("<script>alert('您上次未完成交卷，此时系统自动为您补交！得分：" + testpaper.Scorse + "分。');window.close();</script>");
                    Response.End();
                }
            }

            //开启防止多次开启同一课程考试
            //TestSettingBLL.TestBegin(userID, productID);

            paperName = product.Name;
            testTime = (testSetting.TestTimeLength * 60).ToString();

            QuestionBLL.ReadQuestionXmlList(productID, product.Accessory, companyID, userID);
            XmlHelper XmlDoc = new XmlHelper(filePath);


            paperInfoHtml.AppendLine("<p>考试时长：" + testSetting.TestTimeLength.ToString() + " 分钟</p>");
            paperInfoHtml.AppendLine("<p>试卷总分：" + testSetting.PaperScore.ToString() + " 分</p>");
            paperInfoHtml.AppendLine("<p>试题数量：共 " + XmlDoc.ReadAttribute("TestPaper", "QuestionNum") + " 道。其中 ");
            string TempNum = XmlDoc.ReadAttribute("TestPaper", "SingleNum");
            if (int.Parse(TempNum) > 0) paperInfoHtml.Append("单项选择题：" + TempNum + " 道；");
            TempNum = XmlDoc.ReadAttribute("TestPaper", "MultiNum");
            if (int.Parse(TempNum) > 0) paperInfoHtml.Append("多项选择题：" + TempNum + " 道；");
            TempNum = XmlDoc.ReadAttribute("TestPaper", "PanDunNum");
            if (int.Parse(TempNum) > 0) paperInfoHtml.Append("判断题：" + TempNum + " 道；");
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
