using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.Common;
using System.Xml;
using System.Collections;
using Newtonsoft.Json;
using XueFuShop.Models.DTO;

namespace XueFuShop.Pages
{
    public class Ajax : AjaxBasePage
    {
        protected override void PageLoad()
        {
            base.PageLoad();
            switch (RequestHelper.GetQueryString<string>("Action"))
            {
                case "CheckUserName":
                    this.CheckUserName();
                    break;

                case "CheckEmail":
                    this.CheckEmail();
                    break;

                case "CheckMobile":
                    this.CheckMobile();
                    break;

                case "CheckCode":
                    this.CheckCode();
                    break;

                case "Collect":
                    this.Collect();
                    break;

                case "AddFriend":
                    this.AddFriend();
                    break;

                case "AddGiftPack":
                    this.AddGiftPack();
                    break;

                case "DeleteGiftPack":
                    this.DeleteGiftPack();
                    break;

                case "AddToCart":
                    this.AddToCart();
                    break;

                case "AddGiftPackToCart":
                    this.AddGiftPackToCart();
                    break;

                case "SelectShipping":
                    this.SelectShipping();
                    break;

                case "CheckUserCoupon":
                    this.CheckUserCoupon();
                    break;

                case "ChangeLanguagePack":
                    this.ChangeLanguagePack();
                    break;

                case "GetQuestion":
                    GetQuestionXml();
                    break;

                case "AnswerSet":
                    AnswerSet();
                    break;

                case "HandInTestPaper":
                    HandInTestPaper();
                    break;

                case "GetWorkPostList":
                    this.GetWorkPostList();
                    break;

                case "GetWorkingPostList":
                    this.GetWorkingPostList();
                    break;

                case "GetStudyPostJson":
                    this.GetStudyPostJson();
                    break;

                case "GetKPIListByCompanyId":
                    this.GetKPIListByCompanyId();
                    break;

                case "GetEvaluateInfo":
                    this.GetEvaluateInfo();
                    break;

                case "staffEvaluate":
                    this.staffEvaluate();
                    break;

                case "GetEvaluateNameList":
                    this.GetEvaluateNameList();
                    break;

                case "GetPostProductList":
                    this.GetPostProductList();
                    break;

                case "GetProductListByClassID":
                    this.GetProductListByClassID();
                    break;

                case "GetUserGroupJson":
                    this.GetUserGroupJson();
                    break;

                case "GetDownloadMenu":
                    this.GetDownloadMenu();
                    break;

                case "GetDownloadProductList":
                    this.GetDownloadProductList();
                    break;

                case "GetDIYProductList":
                    this.GetDIYProductList();
                    break;

                case "ReadArticle":
                    this.ReadArticle();
                    break;

                case "ArticleList":
                    this.ReadArticleList();
                    break;

                case "ChangeVideo":
                    this.ChangeVideo();
                    break;

                //case "IsTestAgain":
                //    this.IsTestAgain();
                //    break;
            }
        }
        private void ChangeVideo()
        {
            string vid = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("vid"));
            StringBuilder resultData = new StringBuilder("{");
            Dictionary<string, string> videoDic = PolyvBLL.GetVideoDic(vid);
            if (videoDic != null)
            {
                resultData.Append(string.Format("\"vid\":\"{0}\",\"ts\":\"{1}\",\"sign\":\"{2}\"", videoDic["vid"], videoDic["ts"], videoDic["sign"]));
            }
            resultData.Append("}");
            ResponseHelper.Write(resultData.ToString());
        }

        private void ReadArticleList()
        {
            int cateID = RequestHelper.GetQueryString<int>("CateID");
            int page = RequestHelper.GetQueryString<int>("Page");
            string searchKey = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("SearchKey"));

            int pageSize = 15;
            int count = 0;
            double pageCount = 1;

            if (StringHelper.CompareSingleString("33,37,38", cateID.ToString()))
            {
                pageSize = 10;
            }

            StringBuilder resultData = new StringBuilder("{");
            ArticleSearchInfo articleSearch = new ArticleSearchInfo();
            if (!string.IsNullOrEmpty(searchKey))
                articleSearch.Condition = "([Keywords] like '%" + searchKey + "%' Or [Title] like '%" + searchKey + "%')";
            if (cateID > 0)
                articleSearch.ClassID = "|" + cateID.ToString() + "|";
            List<ArticleInfo> articleList = ArticleBLL.SearchArticleList(page, pageSize, articleSearch, ref count);
            if (!StringHelper.CompareSingleString("33,37,38", cateID.ToString()))
            {
                pageCount = Math.Ceiling((double)count / pageSize);
            }
            resultData.Append("\"pages\":" + pageCount + ", \"data\":[");

            ArrayList listArray = new ArrayList();
            foreach (ArticleInfo article in articleList)
            {
                listArray.Add("{\"id\":\"" + article.ID + "\",\"title\": \"" + article.Title + "\",\"content\":\"" + article.Content.Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace("\"", "\\\"") + "\" }");
            }

            resultData.Append(string.Join(",", (string[])listArray.ToArray(typeof(string))) + "]}");
            ResponseHelper.Write(resultData.ToString());
        }

        private void ReadArticle()
        {
            int id = RequestHelper.GetQueryString<int>("ID");
            ArticleInfo article = ArticleBLL.ReadArticle(id);
            ResponseHelper.Write("{\"id\":\"" + article.ID + "\",\"title\": \"" + article.Title + "\",\"content\":\"" + article.Content.Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace("\"", "\\\"") + "\" }");
        }

        private void GetDIYProductList()
        {
            int classID = RequestHelper.GetForm<int>("ClassID");
            string productName = RequestHelper.GetForm<string>("ProductName");
            string resultData = "[";

            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.InBrandID = CompanyBrandID;
            productSearch.IsSale = 1;
            productSearch.InCompanyID = ParentCompanyID;
            if (classID > 0)
                productSearch.ClassID = "|" + classID + "|";
            productSearch.Name = productName;
            productSearch.OrderField = "Order by [IsTop] desc,[ClassID],[Sort],[ID] Desc";
            List<ProductInfo> productList = ProductBLL.SearchProductList(productSearch);
            if (productList.Count > 0)
            {
                foreach (ProductInfo product in productList)
                {
                    resultData += "{\"id\":\"" + product.ID + "\",\"name\": \"" + product.Name + "\"";
                    resultData += " },";
                }
            }
            if (resultData.EndsWith(",")) resultData = resultData.Substring(0, resultData.Length - 1);
            resultData += "]";
            ResponseHelper.Write(resultData);
        }

        private void GetDownloadProductList()
        {
            int classID = RequestHelper.GetForm<int>("ID");
            string searchKey = RequestHelper.GetForm<string>("SearchKey");
            string resultData = "[";

            //获取不用下载的类目ID
            string productClassID = string.Empty;
            foreach (ProductClassInfo productClass in ProductClassBLL.ReadProductClassCacheList())
            {
                if (productClass.IsDownload == 0)
                {
                    productClassID += "|" + productClass.ID;
                }
            }
            if (productClassID.StartsWith("|")) productClassID = productClassID.Substring(1);

            ProductSearchInfo productSearch = new ProductSearchInfo();
            if (classID >= 0)
                productSearch.ClassID = "|" + classID + "|";
            productSearch.Key = searchKey;
            productSearch.InBrandID = base.CompanyBrandID;
            productSearch.IsSale = 1;
            productSearch.NotInClassID = productClassID;
            productSearch.OrderField = "Order by [IsTop] desc,[ClassID],[Sort],[ID] Desc";
            List<ProductInfo> productList = ProductBLL.SearchProductList(productSearch);
            if (productList.Count > 0)
            {
                string postProductID = PostBLL.ReadPostCourseID(base.UserCompanyID, int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId")));
                List<AttributeRecordInfo> attributeRecordList = AttributeRecordBLL.ReadList("1,2", ProductBLL.ReadProductIdStr(productList));
                foreach (ProductInfo product in productList)
                {
                    if (base.UserGroupID != 36 || StringHelper.CompareString(product.ClassID, "3644|5235", '|') || StringHelper.CompareSingleString(postProductID, product.ID.ToString()))
                    {
                        resultData += "{\"id\":\"" + product.ID + "\",\"name\": \"" + product.Name + "\",";
                        resultData += "\"image\":\"" + product.Photo + "\",";
                        resultData += "\"url\":\"" + AttributeRecordBLL.ReadAttributeRecord(attributeRecordList, 1, product.ID).Value + "\",";
                        resultData += "\"surl\":\"" + AttributeRecordBLL.ReadAttributeRecord(attributeRecordList, 2, product.ID).Value + "\"";
                        resultData += " },";
                    }
                }
            }
            if (resultData.EndsWith(",")) resultData = resultData.Substring(0, resultData.Length - 1);
            resultData += "]";
            ResponseHelper.Write(resultData);
        }

        private void GetDownloadMenu()
        {
            int classID = RequestHelper.GetForm<int>("ID");
            string spreadPath = RequestHelper.GetForm<string>("SpreadPath");
            ResponseHelper.Write(GetDownloadMenu(classID, spreadPath));
        }

        private string GetDownloadMenu(int classID, string spreadPath)
        {
            string resultData = "[";
            List<ProductClassInfo> productClassList = ProductClassBLL.ReadProductClassChildList(classID);
            foreach (ProductClassInfo info in productClassList)
            {
                if (info.IsDownload == 1 && GetDownloadProductNum(info.ID) > 0)
                {
                    resultData += "{\"id\":\"" + info.ID + "\",\"name\": \"" + info.ClassName + "\"";
                    if (StringHelper.CompareSingleString(spreadPath, info.ID.ToString()))
                        resultData += ",\"spread\": true";
                    string childrenJson = GetDownloadMenu(info.ID, spreadPath);
                    if (childrenJson.Length > 2)
                        resultData += ",\"children\":" + childrenJson;
                    resultData += " },";
                }
            }
            if (resultData.EndsWith(",")) resultData = resultData.Substring(0, resultData.Length - 1);
            resultData += "]";
            return resultData;
        }

        private int GetDownloadProductNum(int classID)
        {
            int productNum = 0;
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.ClassID = "|" + classID + "|";
            productSearch.InBrandID = base.CompanyBrandID;
            productSearch.IsSale = 1;
            if (base.UserGroupID != 36)
            {
                productNum = ProductBLL.SearchProductNum(productSearch);
            }
            else
            {
                List<ProductInfo> productList = ProductBLL.SearchProductList(productSearch);
                if (productList.Count > 0)
                {
                    string postProductID = PostBLL.ReadPostCourseID(base.UserCompanyID, int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId")));
                    productList = productList.FindAll(delegate(ProductInfo tempProduct) { return (StringHelper.CompareString(tempProduct.ClassID, "3644|5235", '|') || StringHelper.CompareSingleString(postProductID, tempProduct.ID.ToString())); });
                    productNum = productList.Count;
                }
            }
            return productNum;
        }

        //private void IsTestAgain()
        //{
        //    string resultData = "{";

        //    if (TestSettingBLL.IsTestAgain(base.UserID, int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId"))))
        //    {
        //        resultData += "\"result\":\"true\"";
        //    }
        //    else
        //    {
        //        resultData += "\"result\":\"false\"";
        //    }
        //    resultData += "}";

        //    ResponseHelper.Write(resultData);
        //}

        private void GetUserGroupJson()
        {
            int companyID = RequestHelper.GetForm<int>("CompanyID");
            string type = RequestHelper.GetForm<string>("Type");
            string resultData = "{\"data\":[";

            if (companyID > 0)
            {
                List<AdminGroupInfo> userGroupList = new List<AdminGroupInfo>();
                if (string.IsNullOrEmpty(type))
                    if (companyID == base.UserCompanyID)
                        userGroupList = AdminGroupBLL.ReadAdminGroupList(companyID, UserBLL.ReadUserGroupIDByCompanyID(base.SonCompanyID));
                    else
                        userGroupList = AdminGroupBLL.ReadAdminGroupList(companyID, UserBLL.ReadUserGroupIDByCompanyID(companyID.ToString()));
                else
                    userGroupList = AdminGroupBLL.ReadAdminGroupList(companyID);
                foreach (AdminGroupInfo userGroup in userGroupList)
                {
                    resultData += "{\"ID\":\"" + userGroup.ID + "\",\"Name\":\"" + userGroup.Name + "\"},";
                }
                if (resultData.EndsWith(",")) resultData = resultData.Substring(0, resultData.Length - 1);
            }
            resultData += "]}";
            ResponseHelper.Write(resultData);
        }

        private void GetStudyPostJson()
        {
            int companyID = RequestHelper.GetForm<int>("CompanyID");
            string resultData = "{\"data\":[";

            if (companyID > 0)
            {
                List<PostInfo> postList = PostBLL.ReadPostListByPostId(CompanyBLL.ReadCompany(companyID).Post);
                foreach (PostInfo post in postList)
                {
                    resultData += "{\"ID\":\"" + post.PostId + "\",\"Name\":\"" + post.PostName + "\"},";
                }
            }

            if (resultData.EndsWith(",")) resultData = resultData.Substring(0, resultData.Length - 1);
            resultData += "]}";
            ResponseHelper.Write(resultData);
        }

        private void CheckCode()
        {
            string checkCode = RequestHelper.GetForm<string>("CheckCode");
            string resultData = "{";

            if (!Cookies.Common.checkcode.ToLower().Equals(checkCode))
            {
                resultData += "\"result\":\"false\"";
            }
            else
            {
                resultData += "\"result\":\"true\"";
            }
            resultData += "}";

            ResponseHelper.Write(resultData);
        }

        /// <summary>
        /// 获取产品列表
        /// </summary>
        private void GetProductListByClassID()
        {
            int isTop = RequestHelper.GetForm<int>("IsTop");
            int isSpecial = RequestHelper.GetForm<int>("IsSpecial");
            int classID = RequestHelper.GetForm<int>("ClassID");
            int page = RequestHelper.GetForm<int>("Page");
            int pageSize = RequestHelper.GetForm<int>("PageSize");
            int count = 0;
            page = page < 0 ? 1 : page;
            pageSize = pageSize < 0 ? 10 : pageSize;

            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.ClassID = string.Format("|{0}|", classID);
            productSearch.IsSale = 1;
            productSearch.IsTop = isTop;
            productSearch.IsSpecial = isSpecial;
            productSearch.OrderField = "[ClassID],[Sort],[ID]";
            productSearch.OrderType = OrderType.Asc;
            List<ProductInfo> productList = ProductBLL.SearchProductList(page, pageSize, productSearch, ref count);
            List<CourseDTO> courseList = new List<CourseDTO>();

            if (productList.Count > 0)
            {
                string inProductID = ProductBLL.ReadProductIdStr(productList);
                List<AttributeRecordInfo> attributeRecordList = AttributeRecordBLL.ReadList("1,2,5", inProductID);
                foreach (ProductInfo product in productList)
                {
                    CourseDTO course = new CourseDTO();
                    course.IsTest = !string.IsNullOrEmpty(product.Accessory);
                    course.ID = product.ID;
                    course.ClassID = product.ClassID;
                    course.Name = product.Name;
                    course.Image = product.Photo;
                    course.Video = product.ProductNumber;
                    course.Url = AttributeRecordBLL.ReadAttributeRecord(attributeRecordList, 1, product.ID).Value;
                    course.SUrl = AttributeRecordBLL.ReadAttributeRecord(attributeRecordList, 2, product.ID).Value;
                    course.RCUrl = AttributeRecordBLL.ReadAttributeRecord(attributeRecordList, 5, product.ID).Value;
                    course.Teacher = "孟特讲师";
                    course.Summary = product.Summary;
                    courseList.Add(course);
                }
            }

            Dictionary<string, object> resultData = new Dictionary<string, object>();
            resultData.Add("cid", classID);
            resultData.Add("TotalCount", count);
            resultData.Add("list", courseList);
            ResponseHelper.Write(JsonConvert.SerializeObject(resultData));
        }

        private void GetPostProductList()
        {
            List<ProductInfo> productList = new List<ProductInfo>();
            string view = StringHelper.SearchSafe(RequestHelper.GetForm<string>("View"));
            int reslutType = RequestHelper.GetForm<int>("ResultType");
            int classType = RequestHelper.GetForm<int>("ClassType");//选修
            int studyPostID = int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId"));
            int classID = RequestHelper.GetForm<int>("ClassID");
            string resultData = "{\"cid\":\"" + classID + "\",\"list\":";
            string inProductID = string.Empty;

            string parentCompanyID = base.ParentCompanyID;
            //if (string.IsNullOrEmpty(parentCompanyID)) parentCompanyID = base.UserCompanyID.ToString();
            //else parentCompanyID = base.UserCompanyID.ToString() + "," + parentCompanyID;

            //公共课程
            if (reslutType == 4)
            {
                ProductSearchInfo productSearch = new ProductSearchInfo();
                //productSearch.ClassID = "|5298|";
                productSearch.ClassID = "|" + classID + "|";
                productSearch.IsSale = 1;
                productList = ProductBLL.SearchProductList(productSearch);
                inProductID = ProductBLL.ReadProductIdStr(productList);
            }
            ////认证课程
            else if (reslutType == 3)
            {
                PostPassInfo passpost = new PostPassInfo();
                passpost.UserId = base.UserID;
                passpost.IsRZ = int.MinValue;

                RenZhengCateInfo rzCate = new RenZhengCateInfo();
                rzCate.InPostID = PostPassBLL.PassPostString(passpost);
                inProductID = RenZhengCateBLL.ReadTestCateID(rzCate);
                if (!string.IsNullOrEmpty(inProductID))
                {
                    ProductSearchInfo productSearch = new ProductSearchInfo();
                    productSearch.InProductID = inProductID;
                    productList = ProductBLL.SearchProductList(productSearch);
                }
            }
            else
            {
                //列表模式读取岗位内所有课程
                if (view == "List")
                {
                    inProductID = PostBLL.ReadPostCourseID(base.UserCompanyID, studyPostID);
                    ProductSearchInfo productSearch = new ProductSearchInfo();
                    if (!string.IsNullOrEmpty(inProductID))
                    {
                        productSearch.InProductID = inProductID;
                        productSearch.IsSale = 1;
                        productSearch.OrderField = "Order by [IsTop] desc,[ClassID],[Sort],[ID]";
                        productList = ProductBLL.SearchProductList(productSearch);
                    }

                    //加载选修课程
                    if (StringHelper.CompareSingleString("4,5,64", UserBLL.ReadUser(base.UserID).StudyPostID.ToString()))
                    {
                        productSearch.InProductID = string.Empty;
                        productSearch.ClassID = "|6|";
                        productSearch.InBrandID = base.CompanyBrandID;
                        productSearch.NotLikeName = "必修";
                        productList.AddRange(ProductBLL.SearchProductList(productSearch));
                        //重新获取产品ID串
                        inProductID = ProductBLL.ReadProductIdStr(productList);
                    }

                    //加载指定时间考试
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
                            List<ProductInfo> specialProductList = ProductBLL.SearchProductList(productSearch);
                            if (specialProductList.Count > 0)
                            {
                                specialCourseID = ProductBLL.ReadProductIdStr(specialProductList);
                                if (string.IsNullOrEmpty(inProductID)) inProductID = specialCourseID;
                                else inProductID = specialCourseID + "," + inProductID;
                                productList.InsertRange(0, specialProductList);
                            }
                        }
                    }
                }
                //视图模式按分类读取岗位内课程
                else
                {
                    if (classType == 1) //加载选修
                    {
                        ProductSearchInfo productSearch = new ProductSearchInfo();
                        productSearch.ClassID = "|" + classID + "|";
                        productSearch.InBrandID = base.CompanyBrandID;
                        productSearch.IsSale = 1;
                        productSearch.NotLikeName = "必修";
                        productList = ProductBLL.SearchProductList(productSearch);
                        inProductID = ProductBLL.ReadProductIdStr(productList);
                    }
                    else if (classType == 2) //加载指定时间考试
                    {
                        List<TestSettingInfo> specialTestList = TestSettingBLL.ReadSpecialTestList(parentCompanyID);
                        if (specialTestList.Count > 0)
                        {
                            inProductID = TestSettingBLL.ReadSpecialTestCourseID(specialTestList);
                            if (!string.IsNullOrEmpty(inProductID))
                            {
                                ProductSearchInfo productSearch = new ProductSearchInfo();
                                productSearch.InProductID = inProductID;
                                productSearch.InBrandID = base.CompanyBrandID;
                                productSearch.IsSale = 1;
                                productList = ProductBLL.SearchProductList(productSearch);
                                inProductID = ProductBLL.ReadProductIdStr(productList);
                            }
                        }
                    }
                    else
                    {
                        inProductID = PostBLL.ReadPostCourseID(base.UserCompanyID, studyPostID, classID, ref productList);
                    }
                }
            }

            if (productList.Count > 0)
            {
                List<AttributeRecordInfo> attributeRecordList = AttributeRecordBLL.ReadList("1,2,5", inProductID);
                List<TestPaperReportInfo> testRecordList = TestPaperBLL.ReadThelatestList(base.UserID, inProductID);
                resultData += "[";
                foreach (ProductInfo product in productList)
                {
                    string singleData = string.Empty;
                    singleData += "{";

                    bool isTest = true;
                    bool isPass = false;
                    bool isSpecial = false;

                    //判断是否指定时间
                    TestSettingInfo testSetting = TestSettingBLL.ReadTestSetting(StringHelper.SubString(parentCompanyID, "0"), product.ID);//base.UserCompanyID
                    if (testSetting != null && (testSetting.TestStartTime.HasValue || testSetting.TestEndTime.HasValue))
                    {
                        isSpecial = true;
                        singleData += "\"startdate\":\"" + testSetting.TestStartTime + "\",";
                        singleData += "\"enddate\":\"" + testSetting.TestEndTime + "\",";
                        if (DateTime.Now < testSetting.TestStartTime || DateTime.Now > testSetting.TestEndTime)
                            isTest = false;
                    }

                    //判断是否有考题
                    if (isTest && !string.IsNullOrEmpty(product.Accessory))
                    {
                        foreach (TestPaperReportInfo info in testRecordList)
                        {
                            if (info.CourseID == product.ID)
                            {
                                if (info.IsPass == 1)
                                {
                                    singleData += "\"istest\":\"false\",";
                                    singleData += "\"pass\":\"true\",";
                                    isTest = false;
                                    isPass = true;
                                    break;
                                }
                                else if (!isSpecial && info.IsPass == 0 && ((DateTime.Now - info.TestDate).TotalHours < ShopConfig.ReadConfigInfo().TestInterval))
                                {
                                    singleData += "\"istest\":\"false\",";
                                    singleData += "\"pass\":\"false\",";
                                    singleData += "\"hour\":\"" + (ShopConfig.ReadConfigInfo().TestInterval - (int)(DateTime.Now - info.TestDate).TotalHours) + "\",";
                                    isTest = false;
                                    break;
                                }
                                else if (isSpecial && info.IsPass == 0 && ((DateTime.Now - info.TestDate).TotalHours < 24))//指定时间考试没有再次考试的提醒
                                {
                                    isTest = false;
                                    singleData += "\"istest\":\"false\",";
                                }
                            }
                        }
                    }
                    else
                    {
                        singleData += "\"istest\":\"false\",";
                        isTest = false;
                    }
                    if (isTest)
                    {
                        singleData += "\"istest\":\"true\",";
                    }
                    if (reslutType == 0 && isPass == true)
                        continue;
                    if (reslutType == 1 && isPass == false)
                        continue;

                    singleData += "\"id\":\"" + product.ID + "\",";
                    if (product.ClassID.IndexOf("|6|") != -1 && product.Name.IndexOf("必修") == -1)
                        singleData += "\"name\":\"" + product.Name + "【选修】\",";
                    else
                        singleData += "\"name\":\"" + product.Name + "\",";
                    singleData += "\"image\":\"" + product.Photo + "\",";
                    singleData += "\"video\":\"" + product.ProductNumber + "\",";
                    singleData += "\"url\":\"" + AttributeRecordBLL.ReadAttributeRecord(attributeRecordList, 1, product.ID).Value + "\",";
                    singleData += "\"surl\":\"" + AttributeRecordBLL.ReadAttributeRecord(attributeRecordList, 2, product.ID).Value + "\",";
                    singleData += "\"rcurl\":\"" + AttributeRecordBLL.ReadAttributeRecord(attributeRecordList, 5, product.ID).Value + "\"";
                    if (view == "List")
                    {
                        singleData += ",\"teacher\":\"孟特讲师\",";
                        singleData += "\"summary\":\"" + product.Summary + "\"";
                    }

                    singleData += "},";
                    resultData += singleData;
                }
                if (resultData.EndsWith(",")) resultData = resultData.Substring(0, resultData.Length - 1);
                resultData += "]";
            }
            else
            {
                resultData += "\"\"";
            }
            resultData += "}";
            ResponseHelper.Write(resultData);
        }

        /// <summary>
        /// 读取评估名称列表
        /// </summary>
        private void GetEvaluateNameList()
        {
            string companyId = RequestHelper.GetForm<string>("companyid");
            if (!string.IsNullOrEmpty(companyId))
            {
                int type = RequestHelper.GetForm<int>("Type");

                //请求的类型为1时，返回自己以及父公司的数据
                if (type == 1 && int.Parse(companyId) > 0)
                {
                    //string parentCompanyID = StringHelper.SubString(CompanyBLL.ReadParentCompanyIDWithSelf(int.Parse(companyId)), "0");
                    //if (!string.IsNullOrEmpty(parentCompanyID))
                    //{
                    //    companyId = parentCompanyID + "," + companyId;
                    //}

                    companyId = StringHelper.SubString(CompanyBLL.ReadParentCompanyIDWithSelf(int.Parse(companyId)), "0");
                }
                if (companyId == "0") companyId = base.UserCompanyID.ToString();

                ResponseHelper.Write(EvaluateNameBLL.GetEvaluateNameListHtml(companyId, 0));
            }
        }

        private void staffEvaluate()
        {
            int userId = RequestHelper.GetQueryString<int>("UserId");
            int kpiID = RequestHelper.GetQueryString<int>("DataId");
            int score = RequestHelper.GetQueryString<int>("DataValue");
            int evaluateNameId = RequestHelper.GetQueryString<int>("EvaluateNameId");
            string evaluateDate = RequestHelper.GetQueryString<string>("EvaluateDate");
            int evaluateUserId = RequestHelper.GetQueryString<int>("EvaluateUserId");

            KPIEvaluateInfo kpiEvaluate = new KPIEvaluateInfo();
            kpiEvaluate.UserId = userId;
            kpiEvaluate.KPIId = kpiID;
            kpiEvaluate.Scorse = score;
            kpiEvaluate.EvaluateDate = evaluateDate;
            kpiEvaluate.EvaluateNameId = evaluateNameId;
            kpiEvaluate.EvaluateUserId = evaluateUserId;
            if (KPIEvaluateBLL.AddKPIEvaluate(kpiEvaluate) > 0)
                ResponseHelper.Write("ok");
        }

        private void GetEvaluateInfo()
        {
            string userName = RequestHelper.GetQueryString<string>("UserName");
            string companyId = RequestHelper.GetQueryString<string>("CompanyId");
            int evaluateNameId = RequestHelper.GetQueryString<int>("EvaluateNameId");
            int evaluateUserId = RequestHelper.GetQueryString<int>("EvaluateUserId");

            string resultData = "{";
            UserSearchInfo userSearch = new UserSearchInfo();
            userSearch.EqualRealName = userName;
            userSearch.InCompanyID = companyId;
            userSearch.StatusNoEqual = (int)UserState.Del;
            List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
            if (userList.Count > 0)
            {
                UserInfo user = userList[0];
                resultData += "\"UserId\":" + user.ID;
                if (string.IsNullOrEmpty(user.PostName))
                    resultData += ",\"PostName\":\"" + PostBLL.ReadPost(user.WorkingPostID).PostName + "\"";
                else
                    resultData += ",\"PostName\":\"" + user.PostName + "\"";
                resultData += ",\"Department\":\"" + PostBLL.ReadPost(user.Department).PostName + "\"";
                EvaluateNameInfo evaluateName = EvaluateNameBLL.ReadEvaluateName(evaluateNameId);
                resultData += ",\"EvaluateDate\":\"" + evaluateName.Date + "\"";
                resultData += ",\"EvaluateStartDate\":\"" + evaluateName.StartDate + "\"";
                resultData += ",\"EvaluateEndDate\":\"" + evaluateName.EndDate + "\"";


                KPIEvaluateSearchInfo kpievaluateSearch = new KPIEvaluateSearchInfo();
                kpievaluateSearch.EvaluateNameId = evaluateNameId;
                kpievaluateSearch.UserId = user.ID.ToString();
                kpievaluateSearch.PostId = "0";
                kpievaluateSearch.EvaluateUserId = evaluateUserId;
                resultData += ",\"ScoreStr\":\"" + KPIEvaluateBLL.ReadStaffEvaluateData(kpievaluateSearch) + "\"";
            }
            else
                resultData += "\"UserId\": 0";
            resultData += "}";
            ResponseHelper.Write(resultData);
        }

        private void GetKPIListByCompanyId()
        {
            int companyID = RequestHelper.GetForm<int>("CompanyID");
            if (companyID > 0)
            {
                string allCompanyID = CompanyBLL.SystemCompanyId.ToString();
                if (companyID == base.UserCompanyID)
                {
                    if (!string.IsNullOrEmpty(base.ParentCompanyID))
                        allCompanyID += "," + base.ParentCompanyID;
                }
                else
                {
                    string parentCompanyID = CompanyBLL.ReadParentCompanyId(companyID);
                    if (!string.IsNullOrEmpty(parentCompanyID))
                        allCompanyID += "," + parentCompanyID;
                }

                KPISearchInfo kpiSearch = new KPISearchInfo();
                kpiSearch.CompanyID = allCompanyID;
                kpiSearch.ParentId = "1,2,3";
                List<KPIInfo> kpiList = KPIBLL.SearchKPIList(kpiSearch);

                List<KPIInfo> tempList1 = new List<KPIInfo>();
                List<KPIInfo> tempList2 = new List<KPIInfo>();
                List<KPIInfo> tempList3 = new List<KPIInfo>();
                foreach (KPIInfo info in kpiList)
                {
                    switch (info.ParentId)
                    {
                        case 1:
                            tempList1.Add(info);
                            break;
                        case 2:
                            tempList2.Add(info);
                            break;
                        case 3:
                            tempList3.Add(info);
                            break;
                    }
                }
                ResponseHelper.Write(GetTrHtml(tempList1));
                ResponseHelper.Write(GetTrHtml(tempList2));
                ResponseHelper.Write(GetTrHtml(tempList3));
            }
        }

        private string GetTrHtml(List<KPIInfo> kpiList)
        {
            StringBuilder trHtml = new StringBuilder();
            int i = 1;
            foreach (KPIInfo info in kpiList)
            {
                trHtml.AppendLine("<tr data-type=\"" + info.ParentId + "\">");
                if (i == 1)
                    trHtml.AppendLine("	<td rowspan=\"" + kpiList.Count + "\" class=\"indicator_name\">" + KPIBLL.ReadKPI(info.ParentId).Name + "</td>");
                trHtml.AppendLine("	<td class=\"choose\">" + EnumHelper.ReadEnumChineseName<KPIType>((int)info.Type) + "</td>");
                trHtml.AppendLine("	<td class=\"evaluation_content choose\" data-id=\"" + info.ID.ToString() + "\">" + i.ToString() + "." + info.Name + "</td>");
                //trHtml.AppendLine("	<td class=\"choose\">" + ((info.CompanyID == 0) ? "系统指标" : CompanyBLL.ReadCompany(info.CompanyID).CompanySimpleName) + "</td>");
                trHtml.AppendLine("	<td class=\"schedule\"></td>");
                trHtml.AppendLine("</tr>");
                i++;
            }
            return trHtml.ToString();
        }

        /// <summary>
        /// 读取工作岗位列表
        /// </summary>
        private void GetWorkingPostList()
        {
            string companyId = RequestHelper.GetForm<string>("companyid");
            if (!string.IsNullOrEmpty(companyId))
            {
                StringBuilder resultHtml = new StringBuilder();

                WorkingPostSearchInfo workingPostSearch = new WorkingPostSearchInfo();
                workingPostSearch.CompanyId = companyId;
                workingPostSearch.IsPost = 1;
                foreach (WorkingPostViewInfo info in WorkingPostBLL.SearchWorkingPostViewList(workingPostSearch))
                {
                    resultHtml.AppendLine("<option value=\"" + info.PostId + "\">" + info.PostName + "</option>");
                }
                resultHtml.Insert(0, "<option value=\"0\">请选择岗位</option>");
                ResponseHelper.Write(resultHtml.ToString());
            }
        }

        /// <summary>
        /// 读取部门列表
        /// </summary>
        private void GetWorkPostList()
        {
            string companyID = RequestHelper.GetForm<string>("CompanyID");
            StringBuilder resultHtml = new StringBuilder();

            WorkingPostSearchInfo workingPostSearch = new WorkingPostSearchInfo();
            workingPostSearch.CompanyId = companyID;
            workingPostSearch.ParentId = "0";
            workingPostSearch.IsPost = 0;
            foreach (WorkingPostInfo info in WorkingPostBLL.SearchWorkingPostList(workingPostSearch))
            {
                resultHtml.AppendLine("<option value=\"" + info.ID + "\">" + info.PostName + "</option>");
            }
            resultHtml.Insert(0, "<option value=\"0\">作为最大类</option>");
            ResponseHelper.Write(resultHtml.ToString());
        }


        /// <summary>
        /// 交卷
        /// </summary>
        private void HandInTestPaper()
        {
            int productID = RequestHelper.GetForm<int>("ProductID");
            int userID = base.UserID;
            int companyID = base.UserCompanyID;

            TestSettingInfo testSetting = TestSettingBLL.ReadTestSetting(companyID, productID);
            StringBuilder TextOut = new StringBuilder();
            decimal score = Convert.ToDecimal(TestPaperBLL.CalcTestResult(companyID, userID, productID).Scorse);

            TextOut.AppendLine("<div class=\"title\">");
            if (score >= 60 && score < testSetting.LowScore)
                TextOut.Append("好可惜哦，就差一点点了！");
            else if (score >= testSetting.LowScore)
                TextOut.Append("恭喜你通过了~，你太厉害了！");
            else
                TextOut.Append("成绩不太理想喔!学习后来征服它~~");
            TextOut.Append("</div>");
            TextOut.AppendLine("<div class=\"exam\">本次考试总分：" + testSetting.PaperScore + "分，你的得分是：<span class=\"active\">" + score + "</span>分</div>");

            //判断是否通过各岗位
            if (score >= testSetting.LowScore)
            {
                PostPassBLL.CheckPostPass(userID, productID);

                //孟特销售工具应用与说明 课程处理
                //if (productID == 5368)
                //    TestSettingBLL.SpecialTestHandle(userID, int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId")));
            }

            ResponseHelper.Write(TextOut.ToString());
        }

        /// <summary>
        /// 在线考试答案设定
        /// </summary>
        private void AnswerSet()
        {
            int questionID = RequestHelper.GetForm<int>("QuestionID");
            int styleID = RequestHelper.GetForm<int>("StyleID");
            string userAnswer = RequestHelper.GetForm<string>("Answer");
            int productID = RequestHelper.GetForm<int>("ProductID");
            XmlHelper xmldoc = new XmlHelper(TestPaperBLL.ReadTestPaperPath(base.UserID, productID));
            string NodeName = TestPaperBLL.GetTestPaperStyleNodeName(styleID);
            XmlNodeList NodeList = xmldoc.ReadChildNodes(NodeName);
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
        /// 在线考试右边题目列表，每次从Xml取出后重构。
        /// <para value=CateId></para>
        /// </summary>
        private void GetQuestionXml()
        {
            int ID = RequestHelper.GetQueryString<int>("ID");
            StringBuilder TempStr = new StringBuilder();
            XmlNodeList NodeList = QuestionBLL.ReadQuestionXmlList(base.UserID, RequestHelper.GetQueryString<int>("ProductID"), RequestHelper.GetQueryString<string>("StyleID"), ID);
            if (NodeList != null && NodeList.Count > 0)
            {
                TempStr.AppendLine("<div class=\"timu\" data-id=\"" + NodeList[9].InnerText + "\" data-style=\"" + NodeList[7].InnerText + "\">" + (ID).ToString() + ". " + NodeList[0].InnerText.Replace("~", "") + "</div>");
                TempStr.AppendLine("<ul class=\"clearfix option\">");
                switch (NodeList[7].InnerText)
                {
                    case "1":
                        TempStr.AppendLine("<li><input name=\"Answer\" id=\"AnswerA\" type=\"radio\" value=\"A\"" + IsChecked("A", NodeList[6].InnerText.ToUpper()) + " />A、" + NodeList[1].InnerText + "</li>");
                        TempStr.AppendLine("<li><input name=\"Answer\" id=\"AnswerB\" type=\"radio\" value=\"B\"" + IsChecked("B", NodeList[6].InnerText.ToUpper()) + " />B、" + NodeList[2].InnerText + "</li>");
                        TempStr.AppendLine("<li><input name=\"Answer\" id=\"AnswerC\" type=\"radio\" value=\"C\"" + IsChecked("C", NodeList[6].InnerText.ToUpper()) + " />C、" + NodeList[3].InnerText + "</li>");
                        TempStr.AppendLine("<li><input name=\"Answer\" id=\"AnswerD\" type=\"radio\" value=\"D\"" + IsChecked("D", NodeList[6].InnerText.ToUpper()) + " />D、" + NodeList[4].InnerText + "</li>");
                        break;
                    case "2":
                        TempStr.AppendLine("<li><input name=\"Answer\" id=\"AnswerA\" type=\"checkbox\" value=\"A\"" + IsChecked("A", NodeList[6].InnerText.ToUpper()) + " />A、" + NodeList[1].InnerText + "</li>");
                        TempStr.AppendLine("<li><input name=\"Answer\" id=\"AnswerB\" type=\"checkbox\" value=\"B\"" + IsChecked("B", NodeList[6].InnerText.ToUpper()) + " />B、" + NodeList[2].InnerText + "</li>");
                        TempStr.AppendLine("<li><input name=\"Answer\" id=\"AnswerC\" type=\"checkbox\" value=\"C\"" + IsChecked("C", NodeList[6].InnerText.ToUpper()) + " />C、" + NodeList[3].InnerText + "</li>");
                        TempStr.AppendLine("<li><input name=\"Answer\" id=\"AnswerD\" type=\"checkbox\" value=\"D\"" + IsChecked("D", NodeList[6].InnerText.ToUpper()) + " />D、" + NodeList[4].InnerText + "</li>");
                        break;
                    case "3":
                        TempStr.AppendLine("<li><input name=\"Answer\" id=\"AnswerA\" type=\"radio\" value=\"1\"" + IsChecked("1", NodeList[6].InnerText.ToUpper()) + " />正确</li>");
                        TempStr.AppendLine("<li><input name=\"Answer\" id=\"AnswerB\" type=\"radio\" value=\"0\"" + IsChecked("0", NodeList[6].InnerText.ToUpper()) + " />错误</li>");
                        break;
                }
                TempStr.AppendLine("</ul>");
            }
            ResponseHelper.Write(TempStr.ToString());
            ResponseHelper.End();
        }

        private string IsChecked(string Option, string UserAnswer)
        {
            if (UserAnswer != string.Empty)
            {
                if (UserAnswer.IndexOf(Option.ToLower()) >= 0)
                {
                    return " checked ";
                }
            }
            return string.Empty;
        }

        protected void AddFriend()
        {
            string content = string.Empty;
            int queryString = RequestHelper.GetQueryString<int>("UserID");
            if (queryString > 0)
            {
                if (base.UserID == 0)
                {
                    content = "还未登录";
                }
                else if (base.UserID == queryString)
                {
                    content = "不能添加自己为好友";
                }
                else if (UserFriendBLL.ReadUserFriendByFriendID(queryString, base.UserID).ID > 0)
                {
                    content = "该用户已经是你好友";
                }
                else
                {
                    UserFriendInfo userFriend = new UserFriendInfo();
                    userFriend.FriendID = queryString;
                    userFriend.FriendName = UserBLL.ReadUser(queryString).UserName;
                    userFriend.UserID = base.UserID;
                    userFriend.UserName = base.UserName;
                    UserFriendBLL.AddUserFriend(userFriend);
                    content = "成功加入";
                }
            }
            else
            {
                content = "请选择用户";
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        private void AddGiftPack()
        {
            string content = string.Empty;
            int queryString = RequestHelper.GetQueryString<int>("GiftPackID");
            int num2 = RequestHelper.GetQueryString<int>("GroupID");
            int num3 = RequestHelper.GetQueryString<int>("Count");
            int num4 = RequestHelper.GetQueryString<int>("ProductID");
            string str2 = RequestHelper.GetQueryString<string>("ProductPhoto");
            string str3 = RequestHelper.GetQueryString<string>("ProductName");
            string str4 = num2.ToString() + "," + num3.ToString() + "," + num4.ToString() + "," + str2.Replace(",", string.Empty).Replace("|", string.Empty) + "," + str3.Replace(",", string.Empty).Replace("|", string.Empty);
            string str5 = CookiesHelper.ReadCookieValue("GiftPack" + queryString.ToString());
            if (str5 == string.Empty)
            {
                str4 = StringHelper.Encode(str4, ShopConfig.ReadConfigInfo().SecureKey);
                CookiesHelper.AddCookie("GiftPack" + queryString.ToString(), str4);
            }
            else
            {
                str5 = StringHelper.Decode(str5, ShopConfig.ReadConfigInfo().SecureKey);
                int num5 = 0;
                bool flag = false;
                foreach (string str6 in str5.Split(new char[] { '|' }))
                {
                    if (str6 != string.Empty)
                    {
                        if (str6.Split(new char[] { ',' })[0] == num2.ToString())
                        {
                            num5++;
                        }
                        if ((str6.Split(new char[] { ',' })[0] == num2.ToString()) && (str6.Split(new char[] { ',' })[2] == num4.ToString()))
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (!flag)
                {
                    if (num5 < num3)
                    {
                        str4 = StringHelper.Encode(str5 + "|" + str4, ShopConfig.ReadConfigInfo().SecureKey);
                        CookiesHelper.AddCookie("GiftPack" + queryString.ToString(), str4);
                    }
                    else
                    {
                        content = "已经达到该礼品组的限购数量";
                    }
                }
                else
                {
                    content = "请勿加入重复商品";
                }
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        protected void AddGiftPackToCart()
        {
            string content = "ok";
            int queryString = RequestHelper.GetQueryString<int>("GiftPackID");
            GiftPackInfo info = GiftPackBLL.ReadGiftPack(queryString);
            int num2 = 0;
            foreach (string str2 in info.GiftGroup.Split(new char[] { '#' }))
            {
                num2 += Convert.ToInt32(str2.Split(new char[] { '|' })[1]);
            }
            string str3 = StringHelper.Decode(CookiesHelper.ReadCookieValue("GiftPack" + queryString.ToString()), ShopConfig.ReadConfigInfo().SecureKey);
            int length = str3.Split(new char[] { '|' }).Length;
            if (num2 == length)
            {
                string str4 = Guid.NewGuid().ToString();
                foreach (string str2 in str3.Split(new char[] { '|' }))
                {
                    string[] strArray = str2.Split(new char[] { ',' });
                    CartInfo cart = new CartInfo();
                    cart.ProductID = Convert.ToInt32(strArray[2]);
                    cart.ProductName = strArray[4];
                    cart.BuyCount = 1;
                    cart.FatherID = 0;
                    cart.RandNumber = str4;
                    cart.GiftPackID = queryString;
                    cart.UserID = base.UserID;
                    cart.UserName = base.UserName;
                    CartBLL.AddCart(cart, base.UserID);
                    Sessions.ProductBuyCount++;
                }
                Sessions.ProductTotalPrice += info.Price;
                CookiesHelper.DeleteCookie("GiftPack" + queryString.ToString());
                string str5 = content;
                content = str5 + "|" + Sessions.ProductBuyCount.ToString() + "|" + Sessions.ProductTotalPrice.ToString();
            }
            else
            {
                content = "没有满足礼品包的要求";
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        protected void AddToCart()
        {
            string content = "ok";
            int queryString = RequestHelper.GetQueryString<int>("ProductID");
            string productName = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("ProductName"));
            int num2 = RequestHelper.GetQueryString<int>("BuyCount");
            decimal num3 = RequestHelper.GetQueryString<decimal>("CurrentMemberPrice");
            if (!CartBLL.IsProductInCart(queryString, productName, base.UserID))
            {
                CartInfo cart = new CartInfo();
                cart.ProductID = queryString;
                cart.ProductName = productName;
                cart.BuyCount = num2;
                cart.FatherID = 0;
                cart.RandNumber = string.Empty;
                cart.GiftPackID = 0;
                cart.UserID = base.UserID;
                cart.UserName = base.UserName;
                int num4 = CartBLL.AddCart(cart, base.UserID);
                Sessions.ProductBuyCount += num2;
                Sessions.ProductTotalPrice += num2 * num3;
                ProductInfo info2 = ProductBLL.ReadProduct(queryString);
                if (info2.Accessory != string.Empty)
                {
                    ProductSearchInfo productSearch = new ProductSearchInfo();
                    productSearch.InProductID = info2.Accessory;
                    List<ProductInfo> list = ProductBLL.SearchProductList(productSearch);
                    foreach (ProductInfo info4 in list)
                    {
                        cart = new CartInfo();
                        cart.ProductID = info4.ID;
                        cart.ProductName = info4.Name;
                        cart.BuyCount = num2;
                        cart.FatherID = num4;
                        cart.RandNumber = string.Empty;
                        cart.GiftPackID = 0;
                        cart.UserID = base.UserID;
                        cart.UserName = base.UserName;
                        CartBLL.AddCart(cart, base.UserID);
                    }
                }
            }
            else
            {
                content = "该产品已经在购物车";
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        protected void CheckMobile()
        {
            string mobile = StringHelper.SearchSafe(RequestHelper.GetForm<string>("Mobile"));
            int userID = RequestHelper.GetForm<int>("UserID");
            if (UserBLL.IsExistMobile(mobile, userID))
                ResponseHelper.Write("{\"result\":false}");
            else
                ResponseHelper.Write("{\"result\":true}");
        }


        protected void CheckEmail()
        {
            int num = 1;
            string email = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("Email"));
            if ((email != string.Empty) && UserBLL.CheckEmail(email))
            {
                num = 2;
            }
            ResponseHelper.Write(num.ToString());
            ResponseHelper.End();
        }

        protected void CheckUserCoupon()
        {
            string content = string.Empty;
            string number = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("Number"));
            string password = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("Password"));
            if ((number != string.Empty) && (password != string.Empty))
            {
                UserCouponInfo userCoupon = UserCouponBLL.ReadUserCouponByNumber(number, password);
                if (userCoupon.ID > 0)
                {
                    if (userCoupon.UserID == 0)
                    {
                        if (userCoupon.IsUse == 0)
                        {
                            CouponInfo info2 = CouponBLL.ReadCoupon(userCoupon.CouponID);
                            if ((RequestHelper.DateNow >= info2.UseStartDate) && (RequestHelper.DateNow <= info2.UseEndDate))
                            {
                                if (Sessions.ProductTotalPrice >= info2.UseMinAmount)
                                {
                                    content = userCoupon.ID.ToString() + "|" + info2.Money.ToString();
                                    if (base.UserID > 0)
                                    {
                                        userCoupon.UserID = base.UserID;
                                        userCoupon.UserName = base.UserName;
                                        UserCouponBLL.UpdateUserCoupon(userCoupon);
                                    }
                                }
                                else
                                {
                                    content = "购物车的金额小于该优惠券要求的最低消费的金额";
                                }
                            }
                            else
                            {
                                content = "该优惠券没在使用期限内";
                            }
                        }
                        else
                        {
                            content = "该优惠券已经使用了";
                        }
                    }
                    else
                    {
                        content = "该优惠券已经绑定了用户";
                    }
                }
                else
                {
                    content = "不存在该优惠券";
                }
            }
            else
            {
                content = "编号或者密码不能为空";
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        protected void CheckUserName()
        {
            int num = 1;
            string userName = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("UserName"));
            if (userName != string.Empty)
            {
                string forbiddenName = ShopConfig.ReadConfigInfo().ForbiddenName;
                if (forbiddenName != string.Empty)
                {
                    foreach (string str3 in forbiddenName.Split(new char[] { '|' }))
                    {
                        if (userName.IndexOf(str3.Trim()) != -1)
                        {
                            num = 3;
                            break;
                        }
                    }
                }
                if ((num != 3) && (UserBLL.CheckUserName(userName) > 0))
                {
                    num = 2;
                }
            }
            ResponseHelper.Write(num.ToString());
            ResponseHelper.End();
        }

        public void Collect()
        {
            string content = string.Empty;
            int queryString = RequestHelper.GetQueryString<int>("ProductID");
            if (queryString > 0)
            {
                if (base.UserID == 0)
                {
                    content = "还未登录";
                }
                else if (ProductCollectBLL.ReadProductCollectByProductID(queryString, base.UserID).ID > 0)
                {
                    content = "您已经收藏了该产品";
                }
                else
                {
                    ProductCollectInfo productCollect = new ProductCollectInfo();
                    productCollect.ProductID = queryString;
                    productCollect.Date = RequestHelper.DateNow;
                    productCollect.UserID = base.UserID;
                    productCollect.UserName = base.UserName;
                    ProductCollectBLL.AddProductCollect(productCollect);
                    content = "成功收藏";
                }
            }
            else
            {
                content = "请选择产品";
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        protected void DeleteGiftPack()
        {
            string content = string.Empty;
            int queryString = RequestHelper.GetQueryString<int>("GiftPackID");
            int num2 = RequestHelper.GetQueryString<int>("GroupID");
            int num3 = RequestHelper.GetQueryString<int>("ProductID");
            string str2 = StringHelper.Decode(CookiesHelper.ReadCookieValue("GiftPack" + queryString.ToString()), ShopConfig.ReadConfigInfo().SecureKey);
            string str3 = string.Empty;
            foreach (string str4 in str2.Split(new char[] { '|' }))
            {
                if ((str4 != string.Empty) && ((str4.Split(new char[] { ',' })[0] != num2.ToString()) || (str4.Split(new char[] { ',' })[2] != num3.ToString())))
                {
                    if (str3 == string.Empty)
                    {
                        str3 = str4;
                    }
                    else
                    {
                        str3 = str3 + "|" + str4;
                    }
                }
            }
            str3 = StringHelper.Encode(str3, ShopConfig.ReadConfigInfo().SecureKey);
            CookiesHelper.AddCookie("GiftPack" + queryString.ToString(), str3);
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        protected void ChangeLanguagePack()
        {
            CookiesHelper.AddCookie("LanguagePack", RequestHelper.GetQueryString<string>("Language"));
            //CacheHelper.Remove("Config");
            //CacheHelper.Remove("EnglishConfig");
            //ShopConfig.RefreshConfigCache(RequestHelper.GetQueryString<string>("Language"));
            string Url = RequestHelper.GetQueryString<string>("Url");
            if (Url.IndexOf("/English") < 0)
                ResponseHelper.Write("<script>window.location.href='/English" + RequestHelper.GetQueryString<string>("Url") + "';</script>");
            else
                ResponseHelper.Write("<script>window.location.href='" + RequestHelper.GetQueryString<string>("Url").Replace("/English", string.Empty) + "';</script>");
        }

        protected void SelectShipping()
        {
            int queryString = RequestHelper.GetQueryString<int>("ShippingID");
            string regionID = RequestHelper.GetQueryString<string>("RegionID");
            decimal fixedMoeny = 0M;
            ShippingInfo info = ShippingBLL.ReadShippingCache(queryString);
            ShippingRegionInfo info2 = ShippingRegionBLL.SearchShippingRegion(queryString, regionID);
            switch (info.ShippingType)
            {
                case 1:
                    fixedMoeny = info2.FixedMoeny;
                    break;

                case 2:
                    {
                        decimal productTotalWeight = Sessions.ProductTotalWeight;
                        if (productTotalWeight > info.FirstWeight)
                        {
                            fixedMoeny = info2.FirstMoney + (Math.Ceiling((decimal)((productTotalWeight - info.FirstWeight) / info.AgainWeight)) * info2.AgainMoney);
                            break;
                        }
                        fixedMoeny = info2.FirstMoney;
                        break;
                    }
                case 3:
                    {
                        int productBuyCount = Sessions.ProductBuyCount;
                        fixedMoeny = info2.OneMoeny + ((productBuyCount - 1) * info2.AnotherMoeny);
                        break;
                    }
            }
            decimal num5 = 0M;
            FavorableActivityInfo info3 = FavorableActivityBLL.ReadFavorableActivity(DateTime.Now, DateTime.Now, 0);
            if ((info3.ID > 0) && ((("," + info3.UserGrade + ",").IndexOf("," + this.GradeID.ToString() + ",") > -1) && (Sessions.ProductTotalPrice >= info3.OrderProductMoney)))
            {
                switch (info3.ReduceWay)
                {
                    case 1:
                        num5 += info3.ReduceMoney;
                        break;

                    case 2:
                        num5 += (Sessions.ProductTotalPrice * (10M - info3.ReduceDiscount)) / 10M;
                        break;
                }
                if ((info3.ShippingWay == 1) && ShippingRegionBLL.IsRegionIn(regionID, info3.RegionID))
                {
                    num5 += fixedMoeny;
                }
            }
            ResponseHelper.Write(fixedMoeny.ToString() + "|" + num5.ToString());
            ResponseHelper.End();
        }
    }



}
