using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;
using XueFuShop.Models;
using MobileEMS.BLL;
using MobileEMS.Models;
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.BLL;
using XueFuShop.Common;
using Newtonsoft.Json.Linq;

namespace MobileEMS
{
    public partial class CourseAjax : MobileUserBasePage
    {
        public string Action = RequestHelper.GetQueryString<string>("Action");
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (Action)
            {
                case "GetFcate":
                    GetResult<PostCateInfo>(GetPostClassList());
                    break;
                case "GetCoursesByFcate":
                    GetResult<List<MCourseInfo>>(GetCourseList());
                    break;
                case "GetVideo":
                    GetResult<string>(CookiesHelper.ReadCookieValue("CourseVideoList"));
                    break;
                case "GetQuestions":
                    GetResult<QuestionJsonInfo>(GetQuestions());
                    break;
                case "GetQuestionsList":
                    GetResult<Dictionary<string, object>>(GetQuestionsList());
                    break;
                case "AnswerSet":
                    GetResult<string>(AnswerSet());
                    break;
                case "RecordTest":
                    GetResult<string>(RecordTest());
                    break;
                case "GetAutoComplete":
                    GetResult<ArrayList>(GetAutoComplete());
                    break;
                case "GetCoursesByKeyWord":
                    GetResult<List<MCourseInfo>>(GetCoursesByKeyWord());
                    break;
                case "GetRecordList":
                    GetResult<List<Dictionary<string, object>>>(GetRecordList());
                    break;
                case "GetCertList":
                    GetResult<List<Dictionary<string, string>>>(GetCertList());
                    break;
                case "GetPerPaidCourseList":
                    GetResult<List<Dictionary<string, object>>>(GetPerPaidCourseList());
                    break;
            }
        }

        private void GetResult<T>(T Content)
        {
            //ResultInfo ResultModel = new ResultInfo();
            //ResultModel.d = JsonConvert.SerializeObject(Content);
            //Response.Write(JsonConvert.SerializeObject(ResultModel));
            Response.Write(JsonConvert.SerializeObject(Content));
        }

        private List<Dictionary<string, object>> GetPerPaidCourseList()
        {
            Dictionary<int, DateTime> prepaidTestCateList = BLLMTestCate.ReadPrepaidTestCateList(base.UserID);
            List<Dictionary<string, object>> ReturnResult = new List<Dictionary<string, object>>();

            if (prepaidTestCateList.Count > 0)
            {
                foreach (KeyValuePair<int, DateTime> Info in prepaidTestCateList)
                {
                    Dictionary<string, object> TempDic = new Dictionary<string, object>();
                    ProductInfo product = ProductBLL.ReadProduct(Info.Key);
                    if (product != null)
                    {
                        TempDic.Add("Title", product.Name);
                        if (!string.IsNullOrEmpty(product.ProductNumber)) TempDic.Add("IsVideo", true);
                        if (!string.IsNullOrEmpty(product.Accessory)) TempDic.Add("IsTest", true);
                    }
                    else
                    {
                        TempDic.Add("Title", "��ɾ���γ�");
                    }
                    TempDic.Add("ClassID", Info.Key);
                    TempDic.Add("PayDate", Info.Value);
                    TempDic.Add("RemainderDays", 30 - (DateTime.Now - Info.Value).Days);
                    TestPaperInfo testPaperModel = TestPaperBLL.ReadPaper(base.UserID, Info.Key);
                    Dictionary<string, object> testPaperDic = new Dictionary<string, object>();
                    if (testPaperModel != null)
                    {
                        if (testPaperModel.IsPass == 1)
                        {
                            TempDic.Add("IsPass", true);
                        }
                        else
                        {
                            TempDic.Add("IsPass", false);
                            testPaperDic.Add("TestTime", Math.Round(36 - (DateTime.Now - testPaperModel.TestDate).TotalHours, 1));
                        }
                        testPaperDic.Add("Scorse", testPaperModel.Scorse);
                        TempDic.Add("TestCount", 1);
                    }
                    TempDic.Add("TestPaperInfo", testPaperDic);
                    ReturnResult.Add(TempDic);
                }
            }
            return ReturnResult;
        }


        private List<Dictionary<string, string>> GetCertList()
        {
            int PageIndex = RequestHelper.GetForm<int>("pageIndex");
            int PageSize = RequestHelper.GetForm<int>("pageSize");
            List<Dictionary<string, string>> ReturnResult = new List<Dictionary<string, string>>();

            PostPassInfo PostPassModel = new PostPassInfo();
            PostPassModel.UserId = base.UserID;
            List<PostPassInfo> PostPassList = PostPassBLL.ReadPostPassList(PostPassModel);
            if (PostPassList.Count > 0)
            {
                foreach (PostPassInfo Info in PostPassList)
                {
                    Dictionary<string, string> TempDic = new Dictionary<string, string>();
                    TempDic.Add("PostName", Info.PostName);
                    TempDic.Add("CertPath", "/zs/" + base.UserCompanyID.ToString() + "/" + base.UserID.ToString() + "_" + Info.PostId.ToString() + ".jpg");
                    ReturnResult.Add(TempDic);
                }
            }
            return ReturnResult;
        }

        private List<Dictionary<string, object>> GetRecordList()
        {
            int PageIndex = RequestHelper.GetForm<int>("pageIndex");
            int PageSize = RequestHelper.GetForm<int>("pageSize");
            int Count = 0;
            List<Dictionary<string, object>> ReturnResult = new List<Dictionary<string, object>>();

            TestPaperInfo TestPaperModel = new TestPaperInfo();
            TestPaperModel.UserId = base.UserID;
            List<TestPaperInfo> TestPaperList = TestPaperBLL.ReadList(TestPaperModel, PageIndex, PageSize, ref Count);
            if (TestPaperList.Count > 0)
            {
                foreach (TestPaperInfo Info in TestPaperList)
                {
                    Dictionary<string, object> TempDic = new Dictionary<string, object>();
                    TempDic.Add("TestCateName", Info.PaperName);
                    TempDic.Add("Scorse", Info.Scorse);
                    TempDic.Add("PageCount", Count);
                    ReturnResult.Add(TempDic);
                }
            }
            return ReturnResult;
        }

        private List<MCourseInfo> GetCoursesByKeyWord()
        {
            string Keyword = StringHelper.SearchSafe(RequestHelper.GetForm<string>("keyWord"));
            int PageIndex = RequestHelper.GetForm<int>("pageIndex");
            int PageSize = RequestHelper.GetForm<int>("pageSize");
            List<MCourseInfo> CourseList = new List<MCourseInfo>();
            int i = 1;

            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.Key = Keyword;
            productSearch.IsSale = 1;
            productSearch.InCompanyID = CompanyBLL.SystemCompanyId.ToString();
            productSearch.InBrandID = CookiesHelper.ReadCookieValue("UserCompanyBrandID");
            List<ProductInfo> productList = ProductBLL.SearchProductList(productSearch);
            if (productList.Count > 0)
            {
                //ͨ���Ŀγ�ID
                string PassCourseId = TestPaperBLL.ReadCourseIDStr(TestPaperBLL.ReadList(base.UserID, 1));
                string PostCourseId = PostBLL.ReadPostCourseID(base.UserCompanyID, int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId")));
                string perpaidCourseId = BLLMTestCate.ReadPrepaidTestCate(base.UserID);
                foreach (ProductInfo Info in productList)
                {
                    if (i >= PageSize * (PageIndex - 1) && i < PageSize * PageIndex)
                    {
                        MCourseInfo CourseModel = new MCourseInfo();
                        CourseModel.ClassID = Info.ID.ToString();
                        CourseModel.Title = Info.Name;

                        TestPaperInfo Model = new TestPaperInfo();
                        Model.UserId = base.UserID;
                        Model.CateIdCondition = Info.ID.ToString();
                        Model.GroupCondition = "CateId";
                        TestPaperInfo TestPaperModel = TestPaperBLL.ReadList(Model).Find(delegate(TestPaperInfo TempModel) { return TempModel.CateId == Info.ID; });
                        if (TestPaperModel != null) CourseModel.TestCount = TestPaperModel.TestNum;
                        else CourseModel.TestCount = 0;
                        CourseModel.IsPass = StringHelper.CompareSingleString(PassCourseId, Info.ID.ToString());
                        CourseModel.PageCount = (int)Math.Ceiling((double)productList.Count / (double)PageSize);
                        if (!string.IsNullOrEmpty(Info.ProductNumber)) CourseModel.IsVideo = true;
                        if (!string.IsNullOrEmpty(Info.Accessory)) CourseModel.IsTest = true;
                        CourseModel.IsPostCourse = StringHelper.CompareSingleString(PostCourseId, Info.ID.ToString());
                        if (!CourseModel.IsPostCourse && CourseModel.Title.ToLower().IndexOf("vs") < 0)
                            continue;
                        CourseModel.OriginalPrice = Info.MarketPrice.ToString();
                        List<AttributeRecordInfo> attributeRecordList = AttributeRecordBLL.ReadList("5", Info.ID.ToString());
                        if (attributeRecordList.Count > 0)
                        {
                            CourseModel.IsRC = true;
                            CourseModel.RCUrl = attributeRecordList[0].Value;
                        }
                        //ѡ��Ҳ��Ϊ���Բ鿴����
                        if (!CourseModel.IsPostCourse && Info.Name.ToUpper().Contains("VS")) CourseModel.IsPostCourse = true;
                        CourseModel.IsPrepaidCourse = StringHelper.CompareSingleString(perpaidCourseId, Info.ID.ToString());
                        CourseList.Add(CourseModel);
                    }
                    if (i >= PageSize * PageIndex) break;
                    i++;
                }
            }
            return CourseList;
        }

        private ArrayList GetAutoComplete()
        {
            string Keyword = StringHelper.SearchSafe(RequestHelper.GetForm<string>("word"));
            int RowCount = RequestHelper.GetForm<int>("rowCount");
            //List<TestCateInfo> TestCateList = BLLTestCate.ReadTestCateCacheList().FindAll(delegate(TestCateInfo TempModel) { return (!string.IsNullOrEmpty(TempModel.CateCode) || !string.IsNullOrEmpty(TempModel.CourseContent)) && TempModel.CompanyId==0 && CompareStr.comparebrand(TempModel.BrandId, BLLCompany.BrandId) && TempModel.CateName.Contains(Keyword); });
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.Key = Keyword;
            productSearch.IsSale = 1;
            //productSearch.InProductID=PostBLL.ReadPostCourseID(base.UserCompanyID,CookiesHelper.ReadCookieValue("UserStudyPostId"));
            productSearch.InCompanyID = CompanyBLL.SystemCompanyId.ToString();
            productSearch.InBrandID = CookiesHelper.ReadCookieValue("UserCompanyBrandID");
            List<ProductInfo> productList = ProductBLL.SearchProductList(productSearch);

            ArrayList ResultCateList = new ArrayList();
            for (int i = 0; i < productList.Count; i++)
            {
                if (!ResultCateList.Contains(productList[i].Name))
                {
                    if (i >= RowCount) break;
                    ResultCateList.Add(productList[i].Name);
                }
            }
            return ResultCateList;
        }



        private string RecordTest()
        {
            int productID = RequestHelper.GetForm<int>("CateID");
            int userID = base.UserID;
            int companyID = base.UserCompanyID;
            string answerList = RequestHelper.GetForm<string>("Answer");

            //�����ύ��
            if (!string.IsNullOrEmpty(answerList))
            {
                //Dictionary<int, string> answerDic = new Dictionary<int, string>();
                JArray answerDic = (JArray)JsonConvert.DeserializeObject(answerList);
                foreach (JToken item in answerDic)
                {
                    if (!string.IsNullOrEmpty(item.Value<string>("Answer")))
                    {
                        AnswerSet(productID, item.Value<int>("StyleID"), item.Value<int>("ID"), item.Value<string>("Answer"));
                    }
                }
            }

            TestSettingInfo testSetting = TestSettingBLL.ReadTestSetting(companyID, productID);
            decimal score = Convert.ToDecimal(TestPaperBLL.CalcTestResult(companyID, userID, productID).Scorse);

            //�ж��Ƿ�ͨ������λ
            if (score >= testSetting.LowScore)
            {
                PostPassBLL.CheckPostPass(userID, productID);

                //�������۹���Ӧ����˵�� �γ̴���
                //if (productID == 5368)
                //    TestSettingBLL.SpecialTestHandle(userID, int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId")));
            }

            string ReturnResult = "{\"Success\":\"true\",\"Url\":\"TestPaperShow.aspx?Action=TestPaperResult&ProductID=" + productID.ToString() + "&Scorse=" + score.ToString() + "\"}";
            return ReturnResult;
        }

        private string AnswerSet()
        {
            int questionID = RequestHelper.GetForm<int>("QuestionID");
            int styleID = RequestHelper.GetForm<int>("StyleID");
            string userAnswer = RequestHelper.GetForm<string>("Answer");
            int productID = RequestHelper.GetForm<int>("CateID");
            AnswerSet(productID, styleID, questionID, userAnswer);
            return "{\"Success\":true}";
        }

        private void AnswerSet(int productID, int styleID, int questionID, string userAnswer)
        {
            XmlHelper xmldoc = new XmlHelper(TestPaperBLL.ReadTestPaperPath(base.UserID, productID));
            XmlNodeList NodeList = xmldoc.ReadChildNodes(TestPaperBLL.GetTestPaperStyleNodeName(styleID));
            if (NodeList != null)
            {
                foreach (XmlNode node in NodeList)
                {
                    if (node.Attributes["id"].Value == questionID.ToString())
                    {
                        node.ChildNodes[6].InnerText = userAnswer;
                    }
                }
            }
            xmldoc.Save();
        }

        /// <summary>
        /// �����Ծ�����Ϣ��ÿ�δ�Xmlȡ�����ع���
        /// <para value=CateId></para>
        /// </summary>
        private Dictionary<string, object> GetQuestionsList()
        {
            int productID = RequestHelper.GetForm<int>("CateID");
            string filePath = TestPaperBLL.ReadTestPaperPath(base.UserID, productID);
            XmlHelper XmlDoc = new XmlHelper(filePath);
            Dictionary<string, object> testPaper = new Dictionary<string, object>();
            testPaper.Add("CateId", XmlDoc.ReadAttribute("TestPaper", "CateId"));
            testPaper.Add("QuestionNum", XmlDoc.ReadAttribute("TestPaper", "QuestionNum"));
            testPaper.Add("QuestionsStyle", "[{\"StyleName\":\"����ѡ����\",\"StyleId\":1,\"QuestionsNum\":" + XmlDoc.ReadAttribute("TestPaper", "SingleNum") + "},{\"StyleName\":\"����ѡ����\",\"StyleId\":2,\"QuestionsNum\":" + XmlDoc.ReadAttribute("TestPaper", "MultiNum") + "},{\"StyleName\":\"�ж���\",\"StyleId\":3,\"QuestionsNum\":" + XmlDoc.ReadAttribute("TestPaper", "PanDunNum") + "}]");

            List<QuestionJsonInfo> questionList = new List<QuestionJsonInfo>();

            for (int styleID = 1; styleID <= 3; styleID++)
            {
                string NodeName = TestPaperBLL.GetTestPaperStyleNodeName(styleID);
                //�ж����Ϳ����Ƿ��п���
                XmlNode Node = XmlDoc.ReadNode(NodeName);
                if (Node != null && Node.HasChildNodes)
                {
                    XmlNodeList NodeList = XmlDoc.ReadChildNodes(NodeName);

                    int id = 0;
                    //�����ڵ�
                    foreach (XmlNode node in NodeList)
                    {
                        id++;
                        questionList.Add(this.GetQuestions(node.ChildNodes, id));
                    }
                }
            }

            testPaper.Add("QuestionsList", questionList);

            return testPaper;
        }

        /// <summary>
        /// ȡ��������Ŀ
        /// <para value=CateId></para>
        /// </summary>
        private QuestionJsonInfo GetQuestions()
        {
            int id = RequestHelper.GetForm<int>("ID");
            XmlNodeList nodeList = QuestionBLL.ReadQuestionXmlList(base.UserID, RequestHelper.GetForm<int>("CateID"), RequestHelper.GetForm<string>("StyleID"), id);
            return this.GetQuestions(nodeList, id);
        }

        /// <summary>
        /// ȡ��������Ŀ
        /// <para value=CateId></para>
        /// </summary>
        private QuestionJsonInfo GetQuestions(XmlNodeList nodeList, int id)
        {
            QuestionJsonInfo QuestionModel = new QuestionJsonInfo();
            if (nodeList != null && nodeList.Count > 0)
            {
                QuestionModel.Question = nodeList[0].InnerText;
                QuestionModel.QuestionId = int.Parse(nodeList[9].InnerText);
                QuestionModel.Style = nodeList[7].InnerText;
                QuestionModel.CateId = id;
                switch (nodeList[7].InnerText)
                {
                    case "3":
                        QuestionModel.Question = nodeList[0].InnerText.Replace("~", "");
                        QuestionModel.A = "��ȷ";
                        QuestionModel.B = "����";
                        break;

                    default:
                        QuestionModel.A = nodeList[1].InnerText;
                        QuestionModel.B = nodeList[2].InnerText;
                        QuestionModel.C = nodeList[3].InnerText;
                        QuestionModel.D = nodeList[4].InnerText;
                        break;
                }
            }
            return QuestionModel;
        }

        private PostCateInfo GetPostClassList()
        {
            string QuestType = RequestHelper.GetForm<string>("questType");
            int classID = RequestHelper.GetForm<int>("ClassID");
            int studyPostID = int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId"));
            Dictionary<string, Dictionary<string, string>> postProductClassList = new Dictionary<string, Dictionary<string, string>>();

            string postCourseID = PostBLL.ReadPostCourseID(base.UserCompanyID, studyPostID);
            string passPostCourseID = string.IsNullOrEmpty(postCourseID) ? "" : TestPaperBLL.ReadCourseIDStr(TestPaperBLL.ReadList(base.UserID, postCourseID, 1));
            postCourseID = StringHelper.SubString(postCourseID, passPostCourseID);

            //������֤����
            PostPassInfo passpost = new PostPassInfo();
            passpost.UserId = base.UserID;
            passpost.IsRZ = 0;

            RenZhengCateInfo rzCate = new RenZhengCateInfo();
            rzCate.InPostID = PostPassBLL.PassPostString(passpost);
            string rzProductID = RenZhengCateBLL.ReadTestCateID(rzCate);

            postCourseID = string.IsNullOrEmpty(rzProductID) ? postCourseID : string.IsNullOrEmpty(postCourseID) ? rzProductID : postCourseID + "," + rzProductID;

            //���ش�μ�
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.IsSale = 1;
            productSearch.ClassID = "|5298|";
            List<ProductInfo> dkjProductList = ProductBLL.SearchProductList(productSearch);
            if (dkjProductList.Count > 0)
            {
                string dkjCourseID = ProductBLL.ReadProductIdStr(dkjProductList);
                postCourseID = string.IsNullOrEmpty(postCourseID) ? dkjCourseID : postCourseID + "," + dkjCourseID;
            }

            if (!string.IsNullOrEmpty(postCourseID))
                postProductClassList = ProductClassBLL.ReadProductClassListByProductID(postCourseID, 1);
            if (postProductClassList.Count > 1)
                postProductClassList = ProductClassBLL.productClassSort(postProductClassList);

            List<Dictionary<string, string>> productClassList = new List<Dictionary<string, string>>();
            foreach (string key in postProductClassList.Keys)
            {
                Dictionary<string, string> productClassDic = new Dictionary<string, string>();
                productClassDic.Add("ID", key);
                productClassDic.Add("Name", ProductClassBLL.ReadProductClassCache(int.Parse(key)).ClassName);
                productClassList.Add(productClassDic);
            }

            PostCateInfo studyPost = new PostCateInfo();
            studyPost.EncryptFcateID = studyPostID.ToString();
            studyPost.Title = PostBLL.ReadPost(studyPostID).PostName;
            studyPost.ChildCourseFCateView = productClassList;

            return studyPost;

            //������λ���� ��Ʒѡ��
            //if (StringHelper.CompareSingleString("4,5,64", studyPostID.ToString()))
            //{
            //    ProductSearchInfo productSearch = new ProductSearchInfo();
            //    productSearch.ClassID = "|6|";
            //    productSearch.InBrandID = base.CompanyBrandID;
            //    productSearch.IsSale = 1;
            //    productSearch.NotLikeName = "����";
            //    string xxCourseID = ProductBLL.ReadProductIdStr(ProductBLL.SearchProductList(productSearch));
            //    string passXXCourseID = TestPaperBLL.ReadCourseIDStr(TestPaperBLL.ReadList(base.UserID, xxCourseID, 1));
            //    //if (passType == 1)
            //    //    xxCourseID = passXXCourseID;
            //    //else if (passType == 0)
            //    xxCourseID = StringHelper.SubString(xxCourseID, passXXCourseID);
            //    if (!string.IsNullOrEmpty(xxCourseID))
            //        this.xxProductClassList = ProductClassBLL.ReadProductClassListByProductID(xxCourseID, 1);
            //}

            //if (!string.IsNullOrEmpty(QuestType))
            //{
            //    List<PostInfo> PostCateList = new List<PostInfo>();
            //    PostCateList.Add(PostBLL.ReadPost(PostId));
            //    return ConvertToPostCate(PostCateList, true);
            //}
            //else
            //{
            //    return ConvertToPostCate(PostBLL.ReadPostCateRootList(), true);
            //}
        }

        //private List<PostCateInfo> ConvertToPostCate(List<PostInfo> PostList, bool ChildList)
        //{
        //    List<PostCateInfo> PostCateList = new List<PostCateInfo>();
        //    if (PostList != null)
        //    {
        //        foreach (PostInfo Info in PostList)
        //        {
        //            PostCateInfo PostCateModel = new PostCateInfo();
        //            PostCateModel.EncryptFcateID = Info.PostId.ToString();
        //            PostCateModel.Title = Info.PostName;
        //            if (ChildList) PostCateModel.ChildCourseFCateView = ConvertToPostCate(PostBLL.ReadPostList(Info.PostId), false);
        //            PostCateList.Add(PostCateModel);
        //        }
        //    }
        //    return PostCateList;
        //}

        private List<MCourseInfo> GetCourseList()
        {
            int postID = RequestHelper.GetForm<int>("postID");
            int page = RequestHelper.GetForm<int>("pageIndex");
            int pageSize = RequestHelper.GetForm<int>("pageSize");
            int classID = RequestHelper.GetForm<int>("classID");
            if (pageSize <= 0) pageSize = base.PageSize;
            List<MCourseInfo> CourseList = new List<MCourseInfo>();
            int userID = base.UserID;
            string prepaidCourseId = BLLMTestCate.ReadPrepaidTestCate(userID);
            if (postID < 0) postID = int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId"));

            List<ProductInfo> productList = new List<ProductInfo>();
            string inProductID = PostBLL.ReadPostCourseID(base.UserCompanyID, postID);
            string passPostCourseID = string.IsNullOrEmpty(inProductID) ? "" : TestPaperBLL.ReadCourseIDStr(TestPaperBLL.ReadList(base.UserID, inProductID, 1));
            inProductID = StringHelper.SubString(inProductID, passPostCourseID);
            ProductSearchInfo productSearch = new ProductSearchInfo();
            if (!string.IsNullOrEmpty(inProductID))
            {
                productSearch.InProductID = inProductID;
                productSearch.IsSale = 1;
                if (classID > 0) productSearch.ClassID = "|" + classID + "|";
                productSearch.OrderField = "[IsTop],[ClassID],[Sort],[ID]";
                productList = ProductBLL.SearchProductList(page, pageSize, productSearch, ref base.Count);
            }

            int pageCount = (int)Math.Ceiling((double)base.Count / pageSize);

            //����ѡ�޿γ�(��λ�γ�������ɺ��ټ���ѡ��)
            if ((classID < 0 || classID == 6) && (page == pageCount) && StringHelper.CompareSingleString("4,5,64", UserBLL.ReadUser(base.UserID).StudyPostID.ToString()))
            {
                productSearch.InProductID = string.Empty;
                productSearch.ClassID = "|6|";
                productSearch.InBrandID = base.CompanyBrandID;
                productSearch.NotLikeName = "����";
                productSearch.IsSale = 1;
                productList.AddRange(ProductBLL.SearchProductList(productSearch));
            }

            //������֤����
            if ((classID < 0 && page == 1) || classID == 4387)
            {
                PostPassInfo passpost = new PostPassInfo();
                passpost.UserId = base.UserID;
                passpost.IsRZ = 0;

                RenZhengCateInfo rzCate = new RenZhengCateInfo();
                rzCate.InPostID = PostPassBLL.PassPostString(passpost);
                string rzProductID = RenZhengCateBLL.ReadTestCateID(rzCate);
                if (!string.IsNullOrEmpty(rzProductID))
                {
                    productSearch.InProductID = rzProductID;
                    productSearch.ClassID = string.Empty;
                    productSearch.InBrandID = base.CompanyBrandID;
                    productSearch.NotLikeName = string.Empty;
                    productSearch.IsSale = 1;
                    productList.InsertRange(0, ProductBLL.SearchProductList(productSearch));
                }
            }

            //���ش�μ�
            if ((classID < 0 && page == 1) || classID == 5298)
            {
                productSearch.InProductID = string.Empty;
                productSearch.InBrandID = string.Empty;
                productSearch.NotLikeName = string.Empty;
                productSearch.IsSale = 1;
                productSearch.ClassID = "|5298|";
                productList.InsertRange(0, ProductBLL.SearchProductList(productSearch));
            }

            //����ָ��ʱ�俼��
            if (classID < 0 && page == 1)
            {
                string parentCompanyID = base.ParentCompanyID;
                if (string.IsNullOrEmpty(parentCompanyID)) parentCompanyID = base.UserCompanyID.ToString();
                else parentCompanyID += "," + base.UserCompanyID.ToString();
                List<TestSettingInfo> specialTestList = TestSettingBLL.ReadSpecialTestList(parentCompanyID);
                if (specialTestList.Count > 0)
                {
                    string specialCourseID = TestSettingBLL.ReadSpecialTestCourseID(specialTestList);
                    if (!string.IsNullOrEmpty(specialCourseID))
                    {
                        productSearch.InProductID = specialCourseID;
                        productSearch.ClassID = string.Empty;
                        productSearch.InBrandID = base.CompanyBrandID;
                        productSearch.NotLikeName = string.Empty;
                        productSearch.IsSale = 1;
                        List<ProductInfo> specialProductList = ProductBLL.SearchProductList(productSearch);
                        productList.InsertRange(0, specialProductList);
                    }
                }
            }

            //���»�ȡ��ƷID��
            inProductID = ProductBLL.ReadProductIdStr(productList);
            passPostCourseID = string.IsNullOrEmpty(inProductID) ? "" : TestPaperBLL.ReadCourseIDStr(TestPaperBLL.ReadList(base.UserID, inProductID, 1));

            //��ȡδͨ�������¼�¼
            string noPassCourseID = StringHelper.SubString(inProductID, passPostCourseID);
            List<TestPaperReportInfo> noPassTestPaperList = string.IsNullOrEmpty(noPassCourseID) ? new List<TestPaperReportInfo>() : TestPaperBLL.ReadThelatestList(base.UserID, noPassCourseID);

            List<AttributeRecordInfo> attributeRecordList = AttributeRecordBLL.ReadList("5", noPassCourseID);
            foreach (ProductInfo Info in productList)
            {
                //ͨ���Ŀγ̲�����ʾ
                //if (!StringHelper.CompareSingleString(passPostCourseID, Info.ID.ToString()))
                {
                    MCourseInfo CourseModel = new MCourseInfo();
                    CourseModel.ClassID = Info.ID.ToString();
                    CourseModel.Title = Info.Name;
                    CourseModel.IsPass = false;
                    CourseModel.PageCount = pageCount;
                    TestPaperReportInfo currentPaper = noPassTestPaperList.Find(delegate(TestPaperReportInfo tempPaper) { return tempPaper.CourseID == Info.ID; });
                    if (currentPaper != null)
                    {
                        //ʣ��ʱ��
                        int remainingTime = (ShopConfig.ReadConfigInfo().TestInterval - (int)(DateTime.Now - currentPaper.TestDate).TotalHours);
                        CourseModel.ValidDateShow = remainingTime > 0 ? remainingTime.ToString() : "";
                    }
                    CourseModel.OriginalPrice = Info.MarketPrice.ToString();
                    if (!string.IsNullOrEmpty(Info.ProductNumber)) CourseModel.IsVideo = true;

                    if (!string.IsNullOrEmpty(Info.Accessory))
                    {
                        CourseModel.IsTest = true;

                        //ͨ���Ŀγ̲����ٿ�
                        if (StringHelper.CompareSingleString(passPostCourseID, Info.ID.ToString()))
                            CourseModel.IsTest = false;

                        TestSettingInfo testSetting = TestSettingBLL.ReadTestSetting(base.UserCompanyID, Info.ID);
                        if (testSetting != null && (testSetting.TestStartTime != null || testSetting.TestEndTime != null))
                        {
                            //ָ��ʱ�俼�Կ����˾Ͳ�Ҫ�ٿ���
                            if (!string.IsNullOrEmpty(CourseModel.ValidDateShow)) CourseModel.IsTest = false;
                            if (DateTime.Now < testSetting.TestStartTime || DateTime.Now > testSetting.TestEndTime)
                                CourseModel.IsTest = false;
                        }
                    }
                    CourseModel.IsPostCourse = true;
                    CourseModel.IsPrepaidCourse = StringHelper.CompareSingleString(prepaidCourseId, Info.ID.ToString());

                    //��Ʒ֪ʶ �����籾��ַ
                    {
                        CourseModel.RCUrl = AttributeRecordBLL.ReadAttributeRecord(attributeRecordList, 5, Info.ID).Value;
                        if (!string.IsNullOrEmpty(CourseModel.RCUrl))
                            CourseModel.IsRC = true;
                    }
                    CourseList.Add(CourseModel);
                }
            }
            return CourseList;
        }
    }

    public sealed class ResultInfo
    {
        private string _d;
        public string d
        {
            set { _d = value; }
            get { return _d; }
        }
    }
}
