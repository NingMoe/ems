using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.Pages;
using org.in2bits.MyXls;
using System.Collections;

namespace XueFuShop.Admin
{
    public partial class Ajax : AdminBasePage
    {
        protected void AddProductPhoto()
        {
            ProductPhotoInfo productPhoto = new ProductPhotoInfo();
            productPhoto.ProductID = RequestHelper.GetQueryString<int>("ProductID");
            productPhoto.Name = RequestHelper.GetQueryString<string>("Name");
            productPhoto.Photo = RequestHelper.GetQueryString<string>("Photo");
            int num = ProductPhotoBLL.AddProductPhoto(productPhoto);
            ResponseHelper.Write(num.ToString() + "|" + productPhoto.Name + "|" + productPhoto.Photo);
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

        private void CheckEmail()
        {
            string content = "ok";
            if (UserBLL.CheckEmail(RequestHelper.GetQueryString<string>("Email")))
            {
                content = "error";
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        private void CheckUserName()
        {
            string content = "ok";
            if (UserBLL.CheckUserName(RequestHelper.GetQueryString<string>("UserName")) > 0)
            {
                content = "error";
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        private void CreateSitemap()
        {
            using (XmlTextWriter writer = new XmlTextWriter(base.Server.MapPath("~/Sitemap.xml"), null))
            {
                XmlDocument domDoc = new XmlDocument();
                XmlDeclaration newChild = domDoc.CreateXmlDeclaration("1.0", Encoding.UTF8.BodyName, null);
                domDoc.AppendChild(newChild);
                XmlElement element = domDoc.CreateElement("urlset");
                element.SetAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
                domDoc.AppendChild(element);
                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.IsSale = 1;
                List<ProductInfo> list = ProductBLL.SearchProductList(productSearch);
                foreach (ProductInfo info2 in list)
                {
                    this.CreateSitemapUrl(domDoc, element, "http://" + ShopConfig.ReadConfigInfo().Domain + "/Product-I" + info2.ID.ToString() + ".aspx");
                }
                TagsSearchInfo tags = new TagsSearchInfo();
                List<TagsInfo> list2 = TagsBLL.SearchTagsList(tags);
                foreach (TagsInfo info4 in list2)
                {
                    this.CreateSitemapUrl(domDoc, element, "http://" + ShopConfig.ReadConfigInfo().Domain + "/Product/Tags/" + base.Server.UrlEncode(info4.Word) + ".aspx");
                }
                ProductCommentSearchInfo productComment = new ProductCommentSearchInfo();
                productComment.Status = 2;
                List<ProductCommentInfo> list3 = ProductCommentBLL.SearchProductCommentList(productComment);
                foreach (ProductCommentInfo info6 in list3)
                {
                    this.CreateSitemapUrl(domDoc, element, "http://" + ShopConfig.ReadConfigInfo().Domain + "/ProductReply-C" + info6.ID.ToString() + ".aspx");
                }
                domDoc.WriteTo(writer);
                writer.Flush();
                writer.Close();
            }
            ResponseHelper.Write("ok");
            ResponseHelper.End();
        }

        private void CreateSitemapUrl(XmlDocument domDoc, XmlElement root, string url)
        {
            XmlElement newChild = domDoc.CreateElement("url");
            root.AppendChild(newChild);
            XmlElement element2 = domDoc.CreateElement("loc");
            element2.InnerText = url;
            newChild.AppendChild(element2);
            XmlElement element3 = domDoc.CreateElement("lastmod");
            element3.InnerText = DateTime.Now.ToString("yyy-MM-dd");
            newChild.AppendChild(element3);
            XmlElement element4 = domDoc.CreateElement("changefreq");
            element4.InnerText = ShopConfig.ReadConfigInfo().Frequency;
            newChild.AppendChild(element4);
            XmlElement element5 = domDoc.CreateElement("priority");
            element5.InnerText = ShopConfig.ReadConfigInfo().Priority;
            newChild.AppendChild(element5);
        }

        protected void DeleteEmailContent()
        {
            string queryString = RequestHelper.GetQueryString<string>("Key");
            try
            {
                EmailContentHelper.DeleteEmailContent(queryString);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("EmailContent"), queryString);
            }
            catch
            {
                queryString = "error";
            }
            ResponseHelper.Write(queryString);
            ResponseHelper.End();
        }

        private void DeleteProduct()
        {
            string content = "ok";
            int queryString = RequestHelper.GetQueryString<int>("ProductID");
            if (OrderDetailBLL.ReadOrderDetailByProductID(queryString).Count > 0)
            {
                content = "error";
            }
            else
            {
                ProductBLL.DeleteProduct(queryString.ToString());
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        protected void DeleteProductPhoto()
        {
            ProductPhotoBLL.DeleteProductPhoto(RequestHelper.GetQueryString<string>("ProductPhotoID"));
            ResponseHelper.End();
        }

        private void DeleteUser()
        {
            string content = "ok";
            int queryString = RequestHelper.GetQueryString<int>("UserID");
            if (UserAccountRecordBLL.ReadUserAccountRecordList(queryString).Count > 0)
            {
                content = "error";
            }
            else
            {
                UserBLL.DeleteUser(queryString.ToString());
                TestPaperBLL.DeletePaperByUserID(queryString.ToString());
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("User"), queryString);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("TestPaperRecord"), queryString);
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        private void DownTaobaoPhoto()
        {
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.IsTaobao = 1;
            List<ProductInfo> list = ProductBLL.SearchProductList(productSearch);
            foreach (ProductInfo info2 in list)
            {
                if (!info2.Photo.ToLower().StartsWith("/upload/productcoverphoto/"))
                {
                    string str = FileHelper.CreateFileName(FileNameType.Guid, string.Empty, FileHelper.GetFileExtension(info2.Photo));
                    string filePath = "/Upload/ProductCoverPhoto/Original/" + RequestHelper.DateNow.ToString("yyyyMM") + "/";
                    DirectoryInfo info3 = new DirectoryInfo(ServerHelper.MapPath(filePath));
                    if (!info3.Exists)
                    {
                        info3.Create();
                    }
                    string str3 = filePath + str;
                    WebClient client = new WebClient();
                    try
                    {
                        client.DownloadFile(info2.Photo, ServerHelper.MapPath(str3));
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    ProductBLL.UpdateProductCoverPhoto(info2.ID, str3);
                    Dictionary<int, int> dictionary = new Dictionary<int, int>();
                    dictionary.Add(60, 60);
                    dictionary.Add(120, 120);
                    dictionary.Add(340, 340);
                    string str4 = string.Empty;
                    string str5 = string.Empty;
                    foreach (KeyValuePair<int, int> pair in dictionary)
                    {
                        str4 = str3.Replace("Original", pair.Key.ToString() + "-" + pair.Value.ToString());
                        str5 = str5 + str4 + "|";
                        ImageHelper.MakeThumbnailImage(ServerHelper.MapPath(str3), ServerHelper.MapPath(str4), pair.Key, pair.Value, ThumbnailType.InBox);
                    }
                    str5 = str5.Substring(0, str5.Length - 1);
                    UploadInfo upload = new UploadInfo();
                    upload.TableID = ProductBLL.TableID;
                    upload.ClassID = 0;
                    upload.RecordID = info2.ID;
                    upload.UploadName = str3;
                    upload.OtherFile = str5;
                    upload.Size = Convert.ToInt32(new FileInfo(ServerHelper.MapPath(str3)).Length);
                    upload.FileType = FileHelper.GetFileExtension(info2.Photo);
                    upload.RandomNumber = Cookies.Admin.GetRandomNumber(false);
                    upload.Date = RequestHelper.DateNow;
                    upload.IP = ClientHelper.IP;
                    UploadBLL.AddUpload(upload);
                }
            }
            ResponseHelper.Write("ok");
            ResponseHelper.End();
        }

        private void ImportProductClass()
        {
            string appKey = ShopConfig.ReadConfigInfo().AppKey;
            string appSecret = ShopConfig.ReadConfigInfo().AppSecret;
            string nickName = ShopConfig.ReadConfigInfo().NickName;
            if (ShopConfig.ReadConfigInfo().DeleteProductClass == 1)
            {
                ProductClassBLL.DeleteTaobaoProductClass();
            }
            string str4 = "http://gw.api.taobao.com/router/rest?";
            string[] strArray2 = StringHelper.BubbleSortASC(new string[] { "method=taobao.sellercats.list.get", "timestamp=" + RequestHelper.DateNow.ToString("yyyy-MM-dd HH:mm:ss"), "app_key=" + appKey, "v=2.0", "sign_method=md5", "nick=" + nickName, "fields=cid,name,parent_cid,sort_order" });
            string str5 = string.Empty;
            string str6 = string.Empty;
            for (int i = 0; i < strArray2.Length; i++)
            {
                str6 = str6 + strArray2[i].Replace("=", string.Empty);
                str5 = str5 + "&" + strArray2[i];
            }
            string xml = HttpHelper.WebRequestGet(str4 + "sign=" + FormsAuthentication.HashPasswordForStoringInConfigFile(appSecret + str6 + appSecret, "MD5") + str5);
            Dictionary<long, int> fatherIDDic = new Dictionary<long, int>();
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            XmlNodeList childNodes = document.SelectSingleNode("sellercats_list_get_response/seller_cats").ChildNodes;
            foreach (XmlNode node in childNodes)
            {
                ProductClassInfo productClass = new ProductClassInfo();
                productClass.ClassName = node["name"].InnerText;
                productClass.FatherID = Convert.ToInt32(node["parent_cid"].InnerText);
                productClass.OrderID = Convert.ToInt32(node["sort_order"].InnerText);
                productClass.TaobaoID = Convert.ToInt32(node["cid"].InnerText);
                ProductClassBLL.AddProductClass(productClass);
                fatherIDDic.Add(Convert.ToInt64(node["cid"].InnerText), productClass.ID);
            }
            ProductClassBLL.UpdateProductFatherID(fatherIDDic);
            ResponseHelper.Write("ok");
            ResponseHelper.End();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.ClearCache();
            string queryString = RequestHelper.GetQueryString<string>("Action");
            switch (queryString)
            {
                case "SearchProduct":
                    this.SearchProduct();
                    break;

                case "SearchGift":
                    this.SearchGift();
                    break;

                case "ReadChildRegion":
                    ResponseHelper.Write(RegionBLL.SearchRegionList(RequestHelper.GetQueryString<int>("RegionID")));
                    ResponseHelper.End();
                    break;

                case "ReadChildProductClass":
                    ResponseHelper.Write(ProductClassBLL.SearchProductClassList(RequestHelper.GetQueryString<int>("ProductClassID")));
                    ResponseHelper.End();
                    break;

                case "AddProductPhoto":
                    this.AddProductPhoto();
                    break;

                case "DeleteProductPhoto":
                    this.DeleteProductPhoto();
                    break;

                case "TestSendEmail":
                    this.TestSendEmail();
                    break;

                case "DeleteEmailContent":
                    this.DeleteEmailContent();
                    break;

                case "SendEmail":
                    this.SendEmail();
                    break;

                case "ReadUserEmail":
                    this.ReadUserEmail();
                    break;

                case "IsEnabled":
                    this.UpdateActivityPluginsIsEnabled();
                    break;

                case "CheckUserName":
                    this.CheckUserName();
                    break;

                case "CheckEmail":
                    this.CheckEmail();
                    break;

                case "CheckMobile":
                    this.CheckMobile();
                    break;

                case "IsSpecial":
                case "IsNew":
                case "IsHot":
                case "IsTop":
                case "AllowComment":
                    this.UpdateProductStatus(queryString);
                    break;

                case "TagsIsTop":
                    this.UpdateTagsIsTop(queryString);
                    break;

                case "DeleteProduct":
                    this.DeleteProduct();
                    break;

                case "DeleteUser":
                    this.DeleteUser();
                    break;

                case "ImportProductClass":
                    this.ImportProductClass();
                    break;

                case "DownTaobaoPhoto":
                    this.DownTaobaoPhoto();
                    break;

                case "CreateSitemap":
                    this.CreateSitemap();
                    break;

                case "GetGroupCompanyListById":
                    this.GetGroupCompanyListById();
                    break;

                case "SearchCompanyName":
                    this.SearchCompanyName();
                    break;

                case "GetKPIListByCompanyId":
                    this.GetKPIListByCompanyId();
                    break;

                case "GetWorkPostList":
                    this.GetWorkPostList();
                    break;

                case "GetStudyPostJson":
                    this.GetStudyPostJson();
                    break;

                case "GetPostListJson":
                    this.GetPostListJson();
                    break;

                case "GetDepartmentListByStudyPostID":
                    this.GetDepartmentList();
                    break;

                case "GetUserGroupJson":
                    this.GetUserGroupJson();
                    break;

                case "ReportExcel":
                    this.ReportListToExcel();
                    break;

                case "ReportExcel1":
                    this.ReportListToExcel1();
                    break;

                case "ReportExcel2":
                    this.ReportListToExcel2();
                    break;

                case "UpdateTestSetting":
                    this.UpdateTestSetting();
                    break;

                case "CopyPostList":
                    this.CopyPostList();
                    break;
            }
        }

        private void CopyPostList()
        {
            StringBuilder resultData = new StringBuilder();
            resultData.Append("{");
            string copyPostID = RequestHelper.GetForm<string>("PostIDString");
            int targetCompanyID = RequestHelper.GetForm<int>("TargetCompanyID");
            if (!string.IsNullOrEmpty(copyPostID) && targetCompanyID > 0)
            {
                //�ַ�������ת����������
                int[] postIDArray = Array.ConvertAll(copyPostID.Split(','), int.Parse);
                List<WorkingPostInfo> workingPostList = WorkingPostBLL.ReadWorkingPost(postIDArray);
                if (workingPostList != null)
                {
                    ArrayList copyWorkingPostIDArray = new ArrayList();
                    CopyPost(workingPostList, targetCompanyID, 0, ref copyWorkingPostIDArray);
                }
                resultData.Append("\"Success\":true");
            }
            else
            {
                resultData.AppendFormat("\"Success\":false,\"Msg\":\"{0}\"", "��λ����Ŀ��Ϊ�գ�");
            }
            resultData.Append("}");
            ResponseHelper.Write(resultData.ToString());
        }

        private void CopyPost(List<WorkingPostInfo> workingPostList, int targetCompanyID, int parentID, ref ArrayList copyWorkingPostIDArray)
        {
            foreach (WorkingPostInfo workingPost in workingPostList)
            {
                if (copyWorkingPostIDArray.Contains(workingPost.ID))
                    continue;
                workingPost.CompanyId = targetCompanyID;
                workingPost.ParentId = parentID;
                parentID = WorkingPostBLL.AddWorkingPost(workingPost);
                copyWorkingPostIDArray.Add(workingPost.ID);
                List<WorkingPostInfo> subWorkingPostList = workingPostList.FindAll(m => m.ParentId == workingPost.ID);
                if (subWorkingPostList != null)
                {
                    CopyPost(subWorkingPostList, targetCompanyID, parentID, ref copyWorkingPostIDArray);
                }
            }
        }

        private void UpdateTestSetting()
        {
            StringBuilder resultData = new StringBuilder();
            resultData.Append("{");
            try
            {
                string selectID = RequestHelper.GetForm<string>("SelectID");

                if (!string.IsNullOrEmpty(selectID))
                {
                    TestSettingInfo testSetting = new TestSettingInfo();
                    testSetting.PaperScore = RequestHelper.GetForm<int>("PaperScore");
                    testSetting.TestTimeLength = RequestHelper.GetForm<int>("TestTimeLength");
                    testSetting.TestQuestionsCount = RequestHelper.GetForm<int>("TestQuestionsCount");
                    testSetting.LowScore = RequestHelper.GetForm<int>("LowScore");
                    testSetting.TestStartTime = RequestHelper.GetForm<DateTime>("TestStartTime");
                    testSetting.TestEndTime = RequestHelper.GetForm<DateTime>("TestEndTime");

                    ProductSearchInfo productSearch = new ProductSearchInfo();
                    productSearch.InProductID = selectID;
                    List<ProductInfo> productList = ProductBLL.SearchProductList(productSearch);
                    if (productList.Count > 0)
                    {
                        foreach (ProductInfo product in productList)
                        {
                            TestSettingBLL.UpdateTestSetting(product.CompanyID, product.ID, testSetting);
                        }
                    }
                }
                resultData.Append("\"Success\":true");
            }
            catch (Exception ex)
            {
                resultData.AppendFormat("\"Success\":false,\"Msg\":\"{0}\"", ex.Message);
            }
            resultData.Append("}");
            ResponseHelper.Write(resultData.ToString());
        }

        private void ReportListToExcel2()
        {
            //����Excel��ʼ
            string FilePath = "~/xml/Demo.xls";

            XlsDocument xls = new XlsDocument();//������xls�ĵ�

            xls.FileName = Server.MapPath(FilePath);//����·�������ֱ�ӷ��͵��ͻ��˵Ļ�ֻ��Ҫ���� ��������


            int companyID = RequestHelper.GetQueryString<int>("CompanyID");
            string courseName = RequestHelper.GetQueryString<string>("CourseName");
            DateTime startDate = RequestHelper.GetQueryString<DateTime>("SearchStartDate");
            DateTime endDate = RequestHelper.GetQueryString<DateTime>("SearchEndDate");
            string postIdCondition = RequestHelper.GetQueryString<string>("PostIdCondition");
            string studyPostIdCondition = RequestHelper.GetQueryString<string>("StudyPostIdCondition");
            string groupID = RequestHelper.GetQueryString<string>("GroupID");
            string state = RequestHelper.GetQueryString<string>("State");

            Dictionary<int, CompanyInfo> companyDic = new Dictionary<int, CompanyInfo>();
            List<string> companyIDArray = new List<string>();
            foreach (var company in CompanyBLL.ReadCompanyList(companyID.ToString()))
            {
                companyDic[company.CompanyId] = company;
                if (company.CompanyId != companyID && (company.GroupId == 0 || company.GroupId == 3))
                {
                    companyIDArray.Add(company.CompanyId.ToString());
                }
            }
            string companyIDStr = string.Join(",", companyIDArray.ToArray());

            if (endDate != DateTime.MinValue) endDate = ShopCommon.SearchEndDate(endDate);

            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.Key = courseName;
            productSearch.NotInClassID = "3644|5235|5298";
            List<ProductInfo> productList = ProductBLL.SearchProductList(productSearch);

            //ϵͳ�ȵ�ȡ��˾���ڵ����гɼ�������ѭ�����е��õĶ����ݿ�������ظ���
            List<TestPaperInfo> testReportList = TestPaperBLL.ReadReportList(companyIDStr, ProductBLL.ReadProductIdStr(productList), startDate, endDate);


            Worksheet sheet = xls.Workbook.Worksheets.AddNamed("����"); //����һ������ҳΪDome
            CompanyInfo companyModel = companyDic[companyID];

            //����ָ������ҳ���п���
            MergeArea ma = new MergeArea(1, 1, 1, productList.Count + 5);
            sheet.AddMergeArea(ma);//����ָ������ҳ���п���
            //������
            Cells cells = sheet.Cells; //���ָ������ҳ�м���
            //�в�������
            cells.Add(1, 1, companyModel.CompanySimpleName + courseName + "�ɼ�����");

            cells.Add(2, 1, "���");
            cells.Add(2, 2, "�ϼ���˾��");
            cells.Add(2, 3, "��˾����");
            cells.Add(2, 4, "����");
            cells.Add(2, 5, "ְλ");
            for (int i = 0; i < productList.Count; i++)
            {
                cells.Add(2, (6 + i), productList[i].Name);
            }
            //cells.Add(2, (3 + productList.Count), "ƽ����");
            if (!string.IsNullOrEmpty(companyIDStr))
            {
                //foreach (string item in companyIDStr.Split(','))
                {
                    UserSearchInfo userSearch = new UserSearchInfo();
                    userSearch.InCompanyID = companyIDStr;//item;
                    userSearch.InStatus = state;
                    userSearch.InGroupID = groupID;
                    userSearch.InWorkingPostID = postIdCondition;
                    userSearch.InStudyPostID = studyPostIdCondition;
                    List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
                    if (userList.Count > 0)
                    {
                        int userNum = 0;
                        CompanyInfo currentCompany = new CompanyInfo();
                        CompanyInfo parentCompany = null;
                        foreach (UserInfo user in userList)
                        {
                            if (currentCompany.CompanyId != user.CompanyID)
                            {
                                currentCompany = companyDic[user.CompanyID];
                            }
                            if (string.IsNullOrEmpty(currentCompany.ParentId))
                            {
                                parentCompany = null;
                            }
                            else
                            {
                                if (parentCompany == null || currentCompany.ParentId != parentCompany.CompanyId.ToString())
                                {
                                    parentCompany = CompanyBLL.ReadCompany(int.Parse(currentCompany.ParentId.Split(',')[0]));
                                }
                            }
                            userNum = userNum + 1;
                            cells.Add((2 + userNum), 1, userNum);
                            cells.Add((2 + userNum), 2, parentCompany.CompanySimpleName);
                            cells.Add((2 + userNum), 3, currentCompany.CompanySimpleName);
                            cells.Add((2 + userNum), 4, user.RealName);
                            cells.Add((2 + userNum), 5, PostBLL.ReadPost(user.WorkingPostID).PostName);
                            decimal userAverageScore = 0;
                            for (int i = 0; i < productList.Count; i++)
                            {
                                ProductInfo product = productList[i];
                                TestPaperInfo testPaperModel = TestPaperBLL.ReadReportInfo(testReportList, user.ID, product.ID);
                                if (testPaperModel != null)
                                {
                                    cells.Add((2 + userNum), (6 + i), testPaperModel.Scorse);
                                    userAverageScore = userAverageScore + testPaperModel.Scorse;
                                }
                                else
                                {
                                    cells.Add((2 + userNum), (6 + i), "��");
                                }
                            }
                        }
                    }
                }
            }

            //���ɱ��浽������������ڲ��Ḳ�ǲ��ұ��쳣������ɾ���ڱ����µ�
            if (File.Exists(Server.MapPath(FilePath)))
            {
                File.Delete(Server.MapPath(FilePath));//ɾ��
            }
            //�����ĵ�
            xls.Save(Server.MapPath(FilePath));//���浽������
            xls.Send();//���͵��ͻ���
        }

        private void ReportListToExcel1()
        {
            //����Excel��ʼ
            string FilePath = "~/xml/Demo.xls";

            XlsDocument xls = new XlsDocument();//������xls�ĵ�

            xls.FileName = Server.MapPath(FilePath);//����·�������ֱ�ӷ��͵��ͻ��˵Ļ�ֻ��Ҫ���� ��������


            int companyID = RequestHelper.GetQueryString<int>("CompanyID");
            string courseName = RequestHelper.GetQueryString<string>("CourseName");
            DateTime startDate = RequestHelper.GetQueryString<DateTime>("SearchStartDate");
            DateTime endDate = RequestHelper.GetQueryString<DateTime>("SearchEndDate");
            string postIdCondition = RequestHelper.GetQueryString<string>("PostIdCondition");
            string studyPostIdCondition = RequestHelper.GetQueryString<string>("StudyPostIdCondition");
            string groupID = RequestHelper.GetQueryString<string>("GroupID");
            string state = RequestHelper.GetQueryString<string>("State");
            //string companyIDStr = CompanyBLL.ReadCompanyIdList(companyID.ToString());
            //if (companyIDStr.IndexOf(",") > 0)
            //    companyIDStr = StringHelper.SubString(companyIDStr, companyID.ToString());
            //string[] arrCompanyID = companyIDStr.Split(',');

            List<string> companyIDArray = new List<string>();
            foreach (var company in CompanyBLL.ReadCompanyList(companyID.ToString()))
            {
                if (company.CompanyId != companyID && (company.GroupId == 0 || company.GroupId == 3))
                {
                    companyIDArray.Add(company.CompanyId.ToString());
                }
            }
            string[] arrCompanyID = companyIDArray.ToArray();
            string companyIDStr = string.Join(",", arrCompanyID);

            if (endDate != DateTime.MinValue) endDate = ShopCommon.SearchEndDate(endDate);

            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.Key = courseName;
            productSearch.NotInClassID = "3644|5235|5298";
            List<ProductInfo> productList = ProductBLL.SearchProductList(productSearch);

            //string[] ArrStr = CateId.Split(',');
            //DateTime[] CateIdEndDate = new DateTime[productList.Count];
            //�������顾��˾ID���γ�Id����ƽ��ֵ��
            decimal[, ,] AverageInfo = new decimal[arrCompanyID.Length, productList.Count, 7];


            //��������ʽ������ʱ����
            XF cellXF = xls.NewXF();
            cellXF.VerticalAlignment = VerticalAlignments.Centered;
            cellXF.HorizontalAlignment = HorizontalAlignments.Centered;
            cellXF.Font.Height = 24 * 12;
            cellXF.Font.Bold = true;
            //cellXF.Pattern = 1;//�趨��Ԫ�����������趨Ϊ0�����Ǵ�ɫ���
            //cellXF.PatternBackgroundColor = Colors.Red;//���ı�����ɫ
            //cellXF.PatternColor = Colors.Red;//�趨�����������ɫ
            //��������ʽ����


            XF ListBg = xls.NewXF(); // Ϊxls����һ��XFʵ����XF�ǵ�Ԫ���ʽ����
            ListBg.HorizontalAlignment = HorizontalAlignments.Centered; // �趨���־���
            ListBg.VerticalAlignment = VerticalAlignments.Centered; // ��ֱ����
            //ListBg.Pattern = 1;
            //ListBg.PatternColor= Colors.Default2B;
            ListBg.UseBorder = true; // ʹ�ñ߿�
            ListBg.TopLineStyle = 1; // �ϱ߿���ʽ
            ListBg.TopLineColor = Colors.Black; // �ϱ߿���ɫ
            ListBg.LeftLineStyle = 1; // ��߿���ʽ
            ListBg.LeftLineColor = Colors.Black; // ��߿���ɫ
            ListBg.RightLineStyle = 1; // �ұ߿���ʽ
            ListBg.RightLineColor = Colors.Black; // �ұ߿���ɫ
            ListBg.Font.FontName = "����"; // ����
            ListBg.Font.Height = 11 * 20; // �ִ�С�������С���� 1/20 point Ϊ��λ�ģ�

            XF ReportBg = xls.NewXF();
            ReportBg.HorizontalAlignment = HorizontalAlignments.Centered; // �趨���־���
            ReportBg.VerticalAlignment = VerticalAlignments.Centered; // ��ֱ����
            //ReportBg.Pattern = 1;
            //ListBg.PatternColor= Colors.Default2B;
            ReportBg.UseBorder = true; // ʹ�ñ߿�
            ReportBg.TopLineStyle = 1; // �ϱ߿���ʽ
            ReportBg.TopLineColor = Colors.Black; // �ϱ߿���ɫ
            ReportBg.LeftLineStyle = 1; // ��߿���ʽ
            ReportBg.LeftLineColor = Colors.Black; // ��߿���ɫ
            ReportBg.RightLineStyle = 1; // �ұ߿���ʽ
            ReportBg.RightLineColor = Colors.Black; // �ұ߿���ɫ
            ReportBg.Font.FontName = "����"; // ����
            ReportBg.Font.Height = 14 * 20; // �ִ�С�������С���� 1/20 point Ϊ��λ�ģ�
            ReportBg.Font.Bold = true;



            //ϵͳ�ȵ�ȡ��˾���ڵ����гɼ�������ѭ�����е��õĶ����ݿ�������ظ���
            List<TestPaperInfo> testReportList = TestPaperBLL.ReadReportList(companyIDStr, ProductBLL.ReadProductIdStr(productList), startDate, endDate);

            Worksheet SheetReport = xls.Workbook.Worksheets.AddNamed("���ܱ���");
            MergeArea maReport = new MergeArea(1, 1, 1, arrCompanyID.Length + 1);
            SheetReport.AddMergeArea(maReport);
            Cells cells1 = SheetReport.Cells; //���ָ������ҳ�м���
            cells1.Add(1, 1, CompanyBLL.ReadCompany(companyID).CompanySimpleName + "EMS�������ͳ��");//, cellXF
            if (arrCompanyID.Length > 3)
            {
                cells1.Add(2, arrCompanyID.Length, "�������ڣ�");
                cells1.Add(2, arrCompanyID.Length + 1, startDate.ToString("d"));
            }
            else
            {
                cells1.Add(2, 5, "�������ڣ�");
                cells1.Add(2, 6, startDate.ToString("d"));
            }
            cells1.Add(3, 1, "��˾����");//, ReportBg
            for (int i = 0; i < productList.Count; i++)
            {
                cells1.Add((3 + 1 + 13 * i), 1, "������������");
                cells1.Add((3 + 2 + 13 * i), 1, "ʵ�ʲμӿ�������");
                cells1.Add((3 + 3 + 13 * i), 1, "������");
                cells1.Add((3 + 4 + 13 * i), 1, "����������");
                cells1.Add((3 + 5 + 13 * i), 1, "����ƽ����");
                cells1.Add((3 + 6 + 13 * i), 1, "����ƽ��������");
                cells1.Add((3 + 7 + 13 * i), 1, "����ͨ������");
                cells1.Add((3 + 8 + 13 * i), 1, "����ͨ����");
                cells1.Add((3 + 9 + 13 * i), 1, "ͨ��������");
            }


            //��ʼ�ӹ�˾ѭ��
            for (int CompanyIndex = 0; CompanyIndex < arrCompanyID.Length; CompanyIndex++)
            {
                CompanyInfo companyModel = CompanyBLL.ReadCompany(int.Parse(arrCompanyID[CompanyIndex]));
                Worksheet sheet = xls.Workbook.Worksheets.AddNamed(companyModel.CompanySimpleName); //����һ������ҳΪDome

                //����ָ������ҳ���п���
                MergeArea ma = new MergeArea(1, 1, 1, productList.Count + 3);
                sheet.AddMergeArea(ma);//����ָ������ҳ���п���
                //������
                Cells cells = sheet.Cells; //���ָ������ҳ�м���
                //�в�������
                Cell cell = cells.Add(1, 1, companyModel.CompanySimpleName + courseName + "�ɼ�����");//, cellXF

                //����XY����
                //cell.HorizontalAlignment = HorizontalAlignments.Centered;
                //cell.VerticalAlignment = VerticalAlignments.Centered;
                //��������
                //cell.Font.Bold = true;//���ô���
                //cell.Font.ColorIndex = 0;//������ɫ��
                //cell.Font.FontFamily = FontFamilies.Roman;//�������� Ĭ��Ϊ����
                //�����н���

                string areaManager = string.Empty;
                switch (companyModel.ParentId)
                {
                    case "781":
                        areaManager = "����";
                        break;

                    case "782":
                        areaManager = "������";
                        break;

                    case "783":
                        areaManager = "����";
                        break;

                    case "784":
                        areaManager = "������";
                        break;

                    case "785":
                        areaManager = "������";
                        break;
                }

                UserInfo companyManager = new UserInfo();
                UserSearchInfo managerSearch = new UserSearchInfo();
                managerSearch.InCompanyID = companyModel.CompanyId.ToString();
                managerSearch.Status = (int)UserState.Normal;
                managerSearch.GroupId = 44;
                List<UserInfo> companyManagerList = UserBLL.SearchUserList(managerSearch);
                if (companyManagerList.Count > 0)
                    companyManager = companyManagerList[0];

                cells.Add(2, 1, "������");
                cells.Add(2, 2, areaManager);
                cells.Add(2, 3, "��������");
                cells.Add(2, 4, startDate.ToString("d"));
                cells.Add(3, 1, "�Կ���ϵ��");
                cells.Add(3, 2, companyManager.RealName);
                cells.Add(3, 3, "�绰");
                cells.Add(3, 4, companyManager.Mobile);

                cells.Add(5, 1, "���");
                cells.Add(5, 2, "����");
                cells.Add(5, 3, "ְλ");


                cells1.Add(3, 2 + CompanyIndex, companyModel.CompanySimpleName);//, ReportBg

                for (int i = 0; i < productList.Count; i++)
                {
                    cells.Add(5, (4 + i), productList[i].Name);
                }

                //cells.Add(2, (3 + productList.Count), "ƽ����");


                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.InCompanyID = companyModel.CompanyId.ToString();
                userSearch.InStatus = state;
                userSearch.InGroupID = groupID;
                userSearch.InWorkingPostID = postIdCondition;
                userSearch.InStudyPostID = studyPostIdCondition;
                List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
                int userNum = 0;
                if (userList.Count > 0)
                {
                    foreach (UserInfo user in userList)
                    {
                        userNum = userNum + 1;
                        cells.Add((5 + userNum), 1, userNum);
                        cells.Add((5 + userNum), 2, user.RealName);
                        cells.Add((5 + userNum), 3, PostBLL.ReadPost(user.WorkingPostID).PostName);
                        decimal userAverageScore = 0;
                        int courseHasTest = 0;
                        for (int i = 0; i < productList.Count; i++)
                        {
                            ProductInfo product = productList[i];
                            AverageInfo[CompanyIndex, i, 5] = AverageInfo[CompanyIndex, i, 5] + 1;
                            TestPaperInfo testPaperModel = TestPaperBLL.ReadReportInfo(testReportList, user.ID, product.ID);
                            if (testPaperModel != null)
                            {
                                cells.Add((5 + userNum), (4 + i), testPaperModel.Scorse);//, ListBg
                                userAverageScore = userAverageScore + testPaperModel.Scorse;
                                courseHasTest = courseHasTest + 1;
                                //��Ŀ�ּܷ���
                                AverageInfo[CompanyIndex, i, 0] = AverageInfo[CompanyIndex, i, 0] + testPaperModel.Scorse;
                                //�гɼ�����������
                                AverageInfo[CompanyIndex, i, 1] = AverageInfo[CompanyIndex, i, 1] + 1;
                                if (testPaperModel.IsPass == 1)
                                {
                                    AverageInfo[CompanyIndex, i, 2] = AverageInfo[CompanyIndex, i, 2] + 1;
                                }
                            }
                        }
                    }
                }

                ma = new MergeArea((6 + userNum), (6 + userNum), 1, productList.Count + 3);
                sheet.AddMergeArea(ma);
                cells.Add((6 + userNum), 1, "����ο�ָ��");

                ma = new MergeArea((7 + userNum), (7 + userNum), 1, 3);
                sheet.AddMergeArea(ma);
                cells.Add((7 + userNum), 1, "������������");
                for (int i = 0; i < productList.Count; i++)
                {
                    cells.Add((7 + userNum), (4 + i), AverageInfo[CompanyIndex, i, 5]);//AdminList.Count
                    cells1.Add((3 + 1 + 13 * i), 2 + CompanyIndex, AverageInfo[CompanyIndex, i, 5]);
                }

                ma = new MergeArea((8 + userNum), (8 + userNum), 1, 3);
                sheet.AddMergeArea(ma);
                cells.Add((8 + userNum), 1, "������");
                for (int i = 0; i < productList.Count; i++)
                {
                    decimal TempNum = AverageInfo[CompanyIndex, i, 5] != 0 ? Math.Round((AverageInfo[CompanyIndex, i, 1] / AverageInfo[CompanyIndex, i, 5]), 4) : 0;
                    cells.Add((8 + userNum), (4 + i), TempNum.ToString("P"));
                    cells1.Add((3 + 3 + 13 * i), 2 + CompanyIndex, TempNum.ToString("P"));
                    AverageInfo[CompanyIndex, i, 3] = TempNum;
                }

                ma = new MergeArea((9 + userNum), (9 + userNum), 1, 3);
                sheet.AddMergeArea(ma);
                cells.Add((9 + userNum), 1, "����ͨ������");
                for (int i = 0; i < productList.Count; i++)
                {
                    cells.Add((9 + userNum), (4 + i), AverageInfo[CompanyIndex, i, 2]);
                    cells1.Add((3 + 7 + 13 * i), 2 + CompanyIndex, AverageInfo[CompanyIndex, i, 2]);
                }

                ma = new MergeArea((10 + userNum), (10 + userNum), 1, 3);
                sheet.AddMergeArea(ma);
                cells.Add((10 + userNum), 1, "ʵ�ʲμӿ�������");
                for (int i = 0; i < productList.Count; i++)
                {
                    cells.Add((10 + userNum), (4 + i), AverageInfo[CompanyIndex, i, 1]);
                    cells1.Add((3 + 2 + 13 * i), 2 + CompanyIndex, AverageInfo[CompanyIndex, i, 1]);
                }

                ma = new MergeArea((11 + userNum), (11 + userNum), 1, 3);
                sheet.AddMergeArea(ma);
                cells.Add((11 + userNum), 1, "����ƽ����");
                for (int i = 0; i < productList.Count; i++)
                {
                    decimal TempNum = AverageInfo[CompanyIndex, i, 1] > 0 ? Math.Round(AverageInfo[CompanyIndex, i, 0] / AverageInfo[CompanyIndex, i, 1], 2) : 0;
                    cells.Add((11 + userNum), (4 + i), TempNum);
                    cells1.Add((3 + 5 + 13 * i), 2 + CompanyIndex, TempNum);
                    AverageInfo[CompanyIndex, i, 4] = TempNum;
                }


                ma = new MergeArea((12 + userNum), (12 + userNum), 1, 3);
                sheet.AddMergeArea(ma);
                cells.Add((12 + userNum), 1, "�γ�ͨ����");
                for (int i = 0; i < productList.Count; i++)
                {
                    decimal TempNum = AverageInfo[CompanyIndex, i, 2] != 0 ? Math.Round(AverageInfo[CompanyIndex, i, 2] / AverageInfo[CompanyIndex, i, 5], 4) : 0;
                    cells.Add((12 + userNum), (4 + i), TempNum.ToString("P"));
                    cells1.Add((3 + 8 + 13 * i), 2 + CompanyIndex, TempNum.ToString("P"));
                    AverageInfo[CompanyIndex, i, 6] = TempNum;
                }

            }

            ArrayList sortArray1 = new ArrayList(), sortArray2 = new ArrayList(), sortArray3 = new ArrayList();
            decimal[] totalData = new decimal[4];
            for (int i = 0; i < productList.Count; i++)
            {
                //������
                for (int CompanyIndex = 0; CompanyIndex < arrCompanyID.Length; CompanyIndex++)
                {
                    sortArray1.Add(AverageInfo[CompanyIndex, i, 3]);
                    sortArray2.Add(AverageInfo[CompanyIndex, i, 4]);
                    sortArray3.Add(AverageInfo[CompanyIndex, i, 6]);
                    totalData[0] += AverageInfo[CompanyIndex, i, 5];//Ӧ������
                    totalData[1] += AverageInfo[CompanyIndex, i, 1];//�ο�����
                    totalData[2] += AverageInfo[CompanyIndex, i, 2];//ͨ������
                    totalData[3] += AverageInfo[CompanyIndex, i, 0];//�ܷ�
                }
                sortArray1.Sort();
                sortArray1.Reverse();
                sortArray2.Sort();
                sortArray2.Reverse();
                sortArray3.Sort();
                sortArray3.Reverse();
                for (int CompanyIndex = 0; CompanyIndex < arrCompanyID.Length; CompanyIndex++)
                {
                    cells1.Add((3 + 4 + 13 * i), 2 + CompanyIndex, sortArray1.IndexOf(AverageInfo[CompanyIndex, i, 3]) + 1);
                    cells1.Add((3 + 6 + 13 * i), 2 + CompanyIndex, sortArray2.IndexOf(AverageInfo[CompanyIndex, i, 4]) + 1);
                    cells1.Add((3 + 9 + 13 * i), 2 + CompanyIndex, sortArray3.IndexOf(AverageInfo[CompanyIndex, i, 6]) + 1);
                }

                cells1.Add((3 + 11 + 13 * i), 1, "��������������");
                cells1.Add((3 + 11 + 13 * i), 2, totalData[0]);
                cells1.Add((3 + 11 + 13 * i), 3, "ʵ��������");
                cells1.Add((3 + 11 + 13 * i), 4, totalData[1]);
                cells1.Add((3 + 11 + 13 * i), 5, "������");
                cells1.Add((3 + 11 + 13 * i), 6, totalData[0] > 0 ? Math.Round(totalData[1] / totalData[0], 4).ToString("P") : "0");

                cells1.Add((3 + 12 + 13 * i), 1, "����ƽ����");
                cells1.Add((3 + 12 + 13 * i), 2, totalData[1] > 0 ? Math.Round(totalData[3] / totalData[1], 2) : 0);
                cells1.Add((3 + 12 + 13 * i), 3, "����ͨ��������");
                cells1.Add((3 + 12 + 13 * i), 4, totalData[2]);
                cells1.Add((3 + 12 + 13 * i), 5, "����ͨ����");
                cells1.Add((3 + 12 + 13 * i), 6, totalData[0] > 0 ? Math.Round(totalData[2] / totalData[0], 4).ToString("P") : "0");

                Array.Clear(totalData, 0, totalData.Length);
                sortArray1.Clear();
                sortArray2.Clear();
                sortArray3.Clear();
            }


            //���ɱ��浽������������ڲ��Ḳ�ǲ��ұ��쳣������ɾ���ڱ����µ�
            if (File.Exists(Server.MapPath(FilePath)))
            {
                File.Delete(Server.MapPath(FilePath));//ɾ��
            }
            //�����ĵ�
            xls.Save(Server.MapPath(FilePath));//���浽������
            xls.Send();//���͵��ͻ���
        }

        private void ReportListToExcel()
        {
            //����Excel��ʼ
            string FilePath = "~/xml/Demo.xls";

            XlsDocument xls = new XlsDocument();//������xls�ĵ�

            xls.FileName = Server.MapPath(FilePath);//����·�������ֱ�ӷ��͵��ͻ��˵Ļ�ֻ��Ҫ���� ��������


            int companyID = RequestHelper.GetQueryString<int>("CompanyID");
            string courseName = RequestHelper.GetQueryString<string>("CourseName");
            DateTime startDate = RequestHelper.GetQueryString<DateTime>("SearchStartDate");
            DateTime endDate = RequestHelper.GetQueryString<DateTime>("SearchEndDate");
            string postIdCondition = RequestHelper.GetQueryString<string>("PostIdCondition");
            string studyPostIdCondition = RequestHelper.GetQueryString<string>("StudyPostIdCondition");
            string groupID = RequestHelper.GetQueryString<string>("GroupID");
            string state = RequestHelper.GetQueryString<string>("State");
            string companyIDStr = CompanyBLL.ReadCompanyIdList(companyID.ToString());
            string[] arrCompanyID = companyIDStr.Split(',');

            if (endDate != DateTime.MinValue) endDate = ShopCommon.SearchEndDate(endDate);

            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.Key = courseName;
            productSearch.NotInClassID = "3644|5235|5298";
            List<ProductInfo> productList = ProductBLL.SearchProductList(productSearch);

            //string[] ArrStr = CateId.Split(',');
            //DateTime[] CateIdEndDate = new DateTime[productList.Count];
            //�������顾��˾ID���γ�Id����ƽ��ֵ��
            decimal[, ,] AverageInfo = new decimal[arrCompanyID.Length, productList.Count, 6];


            //��������ʽ������ʱ����
            XF cellXF = xls.NewXF();
            cellXF.VerticalAlignment = VerticalAlignments.Centered;
            cellXF.HorizontalAlignment = HorizontalAlignments.Centered;
            cellXF.Font.Height = 24 * 12;
            cellXF.Font.Bold = true;
            //cellXF.Pattern = 1;//�趨��Ԫ�����������趨Ϊ0�����Ǵ�ɫ���
            //cellXF.PatternBackgroundColor = Colors.Red;//���ı�����ɫ
            //cellXF.PatternColor = Colors.Red;//�趨�����������ɫ
            //��������ʽ����


            XF ListBg = xls.NewXF(); // Ϊxls����һ��XFʵ����XF�ǵ�Ԫ���ʽ����
            ListBg.HorizontalAlignment = HorizontalAlignments.Centered; // �趨���־���
            ListBg.VerticalAlignment = VerticalAlignments.Centered; // ��ֱ����
            //ListBg.Pattern = 1;
            //ListBg.PatternColor= Colors.Default2B;
            ListBg.UseBorder = true; // ʹ�ñ߿�
            ListBg.TopLineStyle = 1; // �ϱ߿���ʽ
            ListBg.TopLineColor = Colors.Black; // �ϱ߿���ɫ
            ListBg.LeftLineStyle = 1; // ��߿���ʽ
            ListBg.LeftLineColor = Colors.Black; // ��߿���ɫ
            ListBg.RightLineStyle = 1; // �ұ߿���ʽ
            ListBg.RightLineColor = Colors.Black; // �ұ߿���ɫ
            ListBg.Font.FontName = "����"; // ����
            ListBg.Font.Height = 11 * 20; // �ִ�С�������С���� 1/20 point Ϊ��λ�ģ�

            XF ReportBg = xls.NewXF();
            ReportBg.HorizontalAlignment = HorizontalAlignments.Centered; // �趨���־���
            ReportBg.VerticalAlignment = VerticalAlignments.Centered; // ��ֱ����
            //ReportBg.Pattern = 1;
            //ListBg.PatternColor= Colors.Default2B;
            ReportBg.UseBorder = true; // ʹ�ñ߿�
            ReportBg.TopLineStyle = 1; // �ϱ߿���ʽ
            ReportBg.TopLineColor = Colors.Black; // �ϱ߿���ɫ
            ReportBg.LeftLineStyle = 1; // ��߿���ʽ
            ReportBg.LeftLineColor = Colors.Black; // ��߿���ɫ
            ReportBg.RightLineStyle = 1; // �ұ߿���ʽ
            ReportBg.RightLineColor = Colors.Black; // �ұ߿���ɫ
            ReportBg.Font.FontName = "����"; // ����
            ReportBg.Font.Height = 14 * 20; // �ִ�С�������С���� 1/20 point Ϊ��λ�ģ�
            ReportBg.Font.Bold = true;



            //ϵͳ�ȵ�ȡ��˾���ڵ����гɼ�������ѭ�����е��õĶ����ݿ�������ظ���
            List<TestPaperInfo> testReportList = TestPaperBLL.ReadReportList(companyIDStr, ProductBLL.ReadProductIdStr(productList), startDate, endDate);

            //Worksheet SheetReport = xls.Workbook.Worksheets.AddNamed("���ܱ���");
            MergeArea maReport = new MergeArea(1, 1, 2, arrCompanyID.Length + 1);
            //SheetReport.AddMergeArea(maReport);
            //Cells cells1 = SheetReport.Cells; //���ָ������ҳ�м���
            //cells1.Add(1, 2, CompanyBLL.ReadCompany(companyID).CompanySimpleName + "EMS�������ͳ��", cellXF);

            //cells1.Add(3, 1, "��˾����", ReportBg);
            //for (int i = 1; i < productList.Count; i++)
            //{
            //    cells1.Add((3 + 2 + 7 * (i - 1)), 1, "�γ�����", ReportBg);
            //    cells1.Add((3 + 3 + 7 * (i - 1)), 1, "Ӧ������", ReportBg);
            //    cells1.Add((3 + 4 + 7 * (i - 1)), 1, "ʵ������", ReportBg);
            //    cells1.Add((3 + 5 + 7 * (i - 1)), 1, "������", ReportBg);
            //    cells1.Add((3 + 6 + 7 * (i - 1)), 1, "ƽ����", ReportBg);
            //    cells1.Add((3 + 7 + 7 * (i - 1)), 1, "����ƽ����", ReportBg);

            //}


            //��ʼ�ӹ�˾ѭ��
            for (int CompanyIndex = 0; CompanyIndex < arrCompanyID.Length; CompanyIndex++)
            {
                CompanyInfo companyModel = CompanyBLL.ReadCompany(int.Parse(arrCompanyID[CompanyIndex]));
                Worksheet sheet = xls.Workbook.Worksheets.AddNamed(companyModel.CompanySimpleName); //����һ������ҳΪDome

                //����ָ������ҳ���п���
                MergeArea ma = new MergeArea(1, 1, 1, productList.Count + 3);
                sheet.AddMergeArea(ma);//����ָ������ҳ���п���
                //������
                Cells cells = sheet.Cells; //���ָ������ҳ�м���
                //�в�������
                Cell cell = cells.Add(1, 1, companyModel.CompanySimpleName, cellXF);

                //����XY����
                cell.HorizontalAlignment = HorizontalAlignments.Centered;
                cell.VerticalAlignment = VerticalAlignments.Centered;
                //��������
                cell.Font.Bold = true;//���ô���
                cell.Font.ColorIndex = 0;//������ɫ��
                cell.Font.FontFamily = FontFamilies.Roman;//�������� Ĭ��Ϊ����
                //�����н���
                cells.Add(2, 1, "���");
                cells.Add(2, 2, "����");
                cells.Add(2, 3, "��λ");


                //cells1.Add(3, 2 + CompanyIndex, CompanyModel.CompanySimpleName, ReportBg);

                for (int i = 0; i < productList.Count; i++)
                {
                    //TestCateInfo TempCateModel = BLLTestCate.ReadTestCateCache(int.Parse(ArrStr[i]));
                    //if (TempCateModel.TestEndTime != DBNull.Value)
                    //{
                    //    CateIdEndDate[i] = Convert.ToDateTime(TempCateModel.TestEndTime);
                    //}
                    //else
                    //{
                    //    CateIdEndDate[i] = DateTime.Today;
                    //}
                    cells.Add(2, (4 + i), productList[i].Name);
                    //cells1.Add((3 + 2 + 7 * (i - 1)), 2 + CompanyIndex, TempCateModel.CateName, ReportBg);
                    //MergeArea Reportma = new MergeArea((3 + 2 + 7 * (i - 1)), (3 + 2 + 7 * (i - 1)), 2, arrCompanyID.Length + 3);
                    //SheetReport.AddMergeArea(Reportma);
                }

                //cells.Add(2, (3 + productList.Count), "ƽ����");


                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.InCompanyID = companyModel.CompanyId.ToString();
                userSearch.InStatus = state;
                userSearch.InGroupID = groupID;
                userSearch.InWorkingPostID = postIdCondition;
                userSearch.InStudyPostID = studyPostIdCondition;
                List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
                int userNum = 0;
                if (userList.Count > 0)
                {
                    foreach (UserInfo user in userList)
                    {
                        //bool HaveCourse = false;
                        //for (int i = 1; i < productList.Count; i++)
                        //{
                        //    if (user.RegisterDate < CateIdEndDate[i])
                        //    {
                        //        HaveCourse = true;
                        //        break;
                        //    }
                        //}
                        //if (!HaveCourse) continue;
                        userNum = userNum + 1;
                        cells.Add((2 + userNum), 1, userNum);
                        cells.Add((2 + userNum), 2, user.RealName);
                        cells.Add((2 + userNum), 3, PostBLL.ReadPost(user.WorkingPostID).PostName);
                        decimal userAverageScore = 0;
                        int courseHasTest = 0;
                        for (int i = 0; i < productList.Count; i++)
                        {
                            ProductInfo product = productList[i];
                            string CourseState = "";
                            //if (user.RegisterDate < CateIdEndDate[i])
                            //{
                            AverageInfo[CompanyIndex, i, 5] = AverageInfo[CompanyIndex, i, 5] + 1;
                            //}
                            //else
                            //{
                            //    CourseState = "��������";
                            //}
                            TestPaperInfo testPaperModel = TestPaperBLL.ReadReportInfo(testReportList, user.ID, product.ID);
                            if (testPaperModel != null)
                            {
                                cells.Add((2 + userNum), (4 + i), testPaperModel.Scorse);//, ListBg
                                userAverageScore = userAverageScore + testPaperModel.Scorse;
                                courseHasTest = courseHasTest + 1;
                                //��Ŀ�ּܷ���
                                AverageInfo[CompanyIndex, i, 0] = AverageInfo[CompanyIndex, i, 0] + testPaperModel.Scorse;
                                //�гɼ�����������
                                AverageInfo[CompanyIndex, i, 1] = AverageInfo[CompanyIndex, i, 1] + 1;
                                if (testPaperModel.IsPass == 1)
                                {
                                    AverageInfo[CompanyIndex, i, 2] = AverageInfo[CompanyIndex, i, 2] + 1;
                                }
                            }
                            else
                            {
                                cells.Add((2 + userNum), (4 + i), CourseState);
                            }
                        }
                        //�˾���
                        //if (courseHasTest > 0)
                        //{
                        //    cells.Add((2 + userNum), (3 + productList.Count), Math.Round(userAverageScore / courseHasTest, 2));
                        //}
                        //else
                        //{
                        //    cells.Add((2 + userNum), (3 + productList.Count), "");
                        //}
                    }
                }

                ma = new MergeArea((2 + userNum + 1), (2 + userNum + 1), 1, 3);
                sheet.AddMergeArea(ma);
                cells.Add((2 + userNum + 1), 1, "Ӧ������");
                for (int i = 0; i < productList.Count; i++)
                {
                    cells.Add((2 + userNum + 1), (4 + i), AverageInfo[CompanyIndex, i, 5]);
                }

                ma = new MergeArea((2 + userNum + 2), (2 + userNum + 2), 1, 3);
                sheet.AddMergeArea(ma);
                cells.Add((2 + userNum + 2), 1, "ʵ�ʲμӿ�������");
                for (int i = 0; i < productList.Count; i++)
                {
                    cells.Add((2 + userNum + 2), (4 + i), AverageInfo[CompanyIndex, i, 1]);
                }

                ma = new MergeArea((2 + userNum + 3), (2 + userNum + 3), 1, 3);
                sheet.AddMergeArea(ma);
                cells.Add((2 + userNum + 3), 1, "������");
                for (int i = 0; i < productList.Count; i++)
                {
                    cells.Add((2 + userNum + 3), (4 + i), (AverageInfo[CompanyIndex, i, 5] != 0 ? Math.Round((AverageInfo[CompanyIndex, i, 1] / AverageInfo[CompanyIndex, i, 5]), 4).ToString("P") : "0"));
                }

                ma = new MergeArea((2 + userNum + 4), (2 + userNum + 4), 1, 3);
                sheet.AddMergeArea(ma);
                cells.Add((2 + userNum + 4), 1, "����ƽ����");
                for (int i = 0; i < productList.Count; i++)
                {
                    cells.Add((2 + userNum + 4), (4 + i), AverageInfo[CompanyIndex, i, 1] != 0 ? Math.Round(AverageInfo[CompanyIndex, i, 0] / AverageInfo[CompanyIndex, i, 1], 2) : 0);
                }

                ma = new MergeArea((2 + userNum + 5), (2 + userNum + 5), 1, 3);
                sheet.AddMergeArea(ma);
                cells.Add((2 + userNum + 5), 1, "����ͨ������");
                for (int i = 0; i < productList.Count; i++)
                {
                    cells.Add((2 + userNum + 5), (4 + i), AverageInfo[CompanyIndex, i, 2]);
                }

                ma = new MergeArea((2 + userNum + 6), (2 + userNum + 6), 1, 3);
                sheet.AddMergeArea(ma);
                cells.Add((2 + userNum + 6), 1, "�γ�ͨ����");
                for (int i = 0; i < productList.Count; i++)
                {
                    cells.Add((2 + userNum + 6), (4 + i), AverageInfo[CompanyIndex, i, 2] != 0 ? Math.Round(AverageInfo[CompanyIndex, i, 2] / AverageInfo[CompanyIndex, i, 5], 4).ToString("P") : "0");
                }
            }

            //���ɱ��浽������������ڲ��Ḳ�ǲ��ұ��쳣������ɾ���ڱ����µ�
            if (File.Exists(Server.MapPath(FilePath)))
            {
                File.Delete(Server.MapPath(FilePath));//ɾ��
            }
            //�����ĵ�
            xls.Save(Server.MapPath(FilePath));//���浽������
            xls.Send();//���͵��ͻ���
        }

        private void GetUserGroupJson()
        {
            int companyID = RequestHelper.GetForm<int>("CompanyID");
            string type = RequestHelper.GetForm<string>("Type");
            string resultData = "{\"data\":[";

            if (companyID > 0)
            {
                List<AdminGroupInfo> userGroupList = new List<AdminGroupInfo>();
                if (string.IsNullOrEmpty(type))
                    if (base.IsGroupCompany(CompanyBLL.ReadCompany(companyID).GroupId))
                    {
                        userGroupList = AdminGroupBLL.ReadAdminGroupList(companyID, UserBLL.ReadUserGroupIDByCompanyID(CompanyBLL.ReadCompanyIdList(companyID.ToString())));
                    }
                    else
                    {
                        userGroupList = AdminGroupBLL.ReadAdminGroupList(companyID, UserBLL.ReadUserGroupIDByCompanyID(companyID.ToString()));
                    }
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

        private void GetPostListJson()
        {
            int companyID = RequestHelper.GetForm<int>("CompanyID");
            string resultData = "{\"data\":[";

            if (companyID > 0)
            {
                List<PostInfo> postList = PostBLL.FilterPostListByCompanyID(PostBLL.ReadPostCateNamedList(), companyID.ToString());
                foreach (PostInfo post in postList)
                {
                    resultData += "{\"ID\":\"" + post.PostId + "\",\"Name\":\"" + post.PostName + "\"},";
                }
            }

            if (resultData.EndsWith(",")) resultData = resultData.Substring(0, resultData.Length - 1);
            resultData += "]}";
            ResponseHelper.Write(resultData);
        }

        private void GetDepartmentList()
        {
            string postID = RequestHelper.GetForm<string>("PostID");
            StringBuilder resultHtml = new StringBuilder();

            resultHtml.AppendLine("<option value=\"0\">��ѡ����</option>");
            foreach (PostInfo info in PostBLL.ReadPostListByPostId(PostBLL.ReadDepartmentIdStrByPostId(postID)))
            {
                resultHtml.AppendLine("<option value=\"" + info.PostId + "\">" + info.PostName + "</option>");
            }
            resultHtml.AppendLine("|");
            resultHtml.AppendLine("<option value=\"0\">��ѡ���λ</option>");
            foreach (PostInfo info in PostBLL.ReadPostListByPostId(postID))
            {
                resultHtml.AppendLine("<option value=\"" + info.PostId + "\">" + info.PostName + "</option>");
            }
            ResponseHelper.Write(resultHtml.ToString());
        }

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        private void GetWorkPostList()
        {
            int companyID = RequestHelper.GetForm<int>("CompanyID");
            StringBuilder resultHtml = new StringBuilder();

            foreach (WorkingPostInfo info in WorkingPostBLL.CreateWorkingPostTreeList(companyID))
            {
                resultHtml.AppendLine("<option value=\"" + info.ID + "\">" + info.PostName + "</option>");
            }
            resultHtml.Insert(0, "<option value=\"0\">��Ϊһ������</option>");
            ResponseHelper.Write(resultHtml.ToString());
        }

        private void GetKPIListByCompanyId()
        {
            int companyID = RequestHelper.GetForm<int>("CompanyID");
            if (companyID > 0)
            {
                string allCompanyID = CompanyBLL.SystemCompanyId.ToString();
                string parentCompanyID = CompanyBLL.ReadParentCompanyId(companyID);
                if (!string.IsNullOrEmpty(parentCompanyID))
                    allCompanyID += "," + parentCompanyID;

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
                trHtml.AppendLine("<tr data-type=\"" + info.ParentId + "\" class=\"listTableMain\" onmousemove=\"changeColor(this,'#FFFDD7')\" onmouseout=\"changeColor(this,'#FFF')\">");
                if (i == 1)
                    trHtml.AppendLine("	<td rowspan=\"" + kpiList.Count + "\" class=\"indicator_name\">" + KPIBLL.ReadKPI(info.ParentId).Name + "</td>");
                trHtml.AppendLine("	<td class=\"choose\">" + EnumHelper.ReadEnumChineseName<KPIType>((int)info.Type) + "</td>");
                trHtml.AppendLine("	<td class=\"evaluation_content choose\" data-id=\"" + info.ID.ToString() + "\">" + i.ToString() + "." + info.Name + "</td>");
                trHtml.AppendLine("	<td class=\"choose\">" + ((info.CompanyID == 0) ? "ϵͳָ��" : CompanyBLL.ReadCompany(info.CompanyID).CompanySimpleName) + "</td>");
                trHtml.AppendLine("	<td class=\"schedule\"></td>");
                trHtml.AppendLine("</tr>");
                i++;
            }
            return trHtml.ToString();
        }

        private void SearchCompanyName()
        {
            string keyWord = HttpUtility.UrlDecode(RequestHelper.GetForm<string>("keyword"));
            string resultData = "{\"data\":[";
            CompanyInfo company = new CompanyInfo();
            company.CompanyName = keyWord;
            company.State = 0;
            foreach (CompanyInfo info in CompanyBLL.ReadCompanyList(company))
            {
                if (resultData != "{\"data\":[")
                {
                    resultData = resultData + ",";
                }
                resultData = resultData + "{\"title\":\"" + info.CompanyName + "\",\"result\":{\"Id\":\"" + info.CompanyId + "\",\"address\":\"" + info.CompanyAddress + "\",\"tel\":\"" + info.CompanyTel + "\",\"post\":\"" + info.Post + "\"}}";
            }
            resultData = resultData + "]}";
            ResponseHelper.Write(resultData);
        }

        private void GetGroupCompanyListById()
        {
            int companyId = RequestHelper.GetForm<int>("companyid");
            string resultData = "{\"data\":[";
            CompanyInfo company = new CompanyInfo();
            company.GroupIdCondition = "1,2";
            company.Condition = CompanyBLL.ReadCompanyIdList(companyId.ToString());
            company.Field = "CompanyId";
            foreach (CompanyInfo info in CompanyBLL.ReadCompanyList(company))
            {
                if (resultData != "{\"data\":[")
                {
                    resultData = resultData + ",";
                }
                resultData = resultData + "{\"companyname\":\"" + info.CompanyName + "\",\"companyid\":\"" + info.CompanyId + "\"}";
            }
            resultData = resultData + "]}";
            Response.Write(resultData);
        }

        protected void ReadUserEmail()
        {
            ResponseHelper.Write(UserBLL.ReadUserByUserName(RequestHelper.GetQueryString<string>("UserName")).Email);
            ResponseHelper.End();
        }

        protected void SearchGift()
        {
            string content = string.Empty;
            GiftSearchInfo gift = new GiftSearchInfo();
            gift.Name = RequestHelper.GetQueryString<string>("Name");
            gift.InGiftID = RequestHelper.GetQueryString<string>("GiftID");
            List<GiftInfo> list = GiftBLL.SearchGiftList(gift);
            foreach (GiftInfo info2 in list)
            {
                if (content == string.Empty)
                {
                    content = info2.ID + "|" + info2.Name.Replace("|", "").Replace("#", "");
                }
                else
                {
                    object obj2 = content;
                    content = string.Concat(new object[] { obj2, "#", info2.ID, "|", info2.Name.Replace("|", "").Replace("#", "") });
                }
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        protected void SearchProduct()
        {
            string content = string.Empty;
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.Name = RequestHelper.GetQueryString<string>("ProductName");
            productSearch.ClassID = RequestHelper.GetQueryString<string>("ClassID");
            List<ProductInfo> list = ProductBLL.SearchProductList(productSearch);
            foreach (ProductInfo info2 in list)
            {
                if (content == string.Empty)
                {
                    content = string.Concat(new object[] { info2.ID, "|", info2.Name, "|", info2.Photo });
                }
                else
                {
                    object obj2 = content;
                    content = string.Concat(new object[] { obj2, "#", info2.ID, "|", info2.Name, "|", info2.Photo });
                }
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        protected void SendEmail()
        {
            ResponseHelper.Write(EmailSendRecordBLL.SendEmail(EmailSendRecordBLL.ReadEmailSendRecord(RequestHelper.GetQueryString<int>("ID"))).SendDate.ToString());
            ResponseHelper.End();
        }

        protected void TestSendEmail()
        {
            string content = "ok";
            EmailContentInfo info = EmailContentHelper.ReadSystemEmailContent("TestEmail");
            MailInfo mail = new MailInfo();
            mail.ToEmail = RequestHelper.GetQueryString<string>("ToEmail");
            mail.Title = info.EmailTitle;
            mail.Content = info.EmailContent;
            mail.UserName = RequestHelper.GetQueryString<string>("EmailUserName");
            mail.Password = RequestHelper.GetQueryString<string>("EmailPassword");
            mail.Server = RequestHelper.GetQueryString<string>("EmailServer");
            mail.ServerPort = RequestHelper.GetQueryString<int>("EmailServerPort");
            try
            {
                MailClass.SendEmail(mail);
            }
            catch
            {
                content = "error";
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        private void UpdateActivityPluginsIsEnabled()
        {
            string queryString = RequestHelper.GetQueryString<string>("Action");
            string key = RequestHelper.GetQueryString<string>("ID");
            Dictionary<string, string> configDic = new Dictionary<string, string>();
            configDic.Add("IsEnabled", RequestHelper.GetQueryString<string>("Status"));
            ActivityPlugins.UpdateActivityPlugins(key, configDic);
            ResponseHelper.Write(queryString + "|" + key);
            ResponseHelper.End();
        }

        private void UpdateProductStatus(string action)
        {
            int queryString = RequestHelper.GetQueryString<int>("ID");
            int status = RequestHelper.GetQueryString<int>("Status");
            string str = action;
            if (str != null)
            {
                if (!(str == "IsSpecial"))
                {
                    if (str == "IsNew")
                    {
                        ProductBLL.ChangProductIsNew(queryString, status);
                    }
                    else if (str == "IsHot")
                    {
                        ProductBLL.ChangProductIsHot(queryString, status);
                    }
                    else if (str == "IsTop")
                    {
                        ProductBLL.ChangProductIsTop(queryString, status);
                    }
                    else if (str == "AllowComment")
                    {
                        ProductBLL.ChangProductAllowComment(queryString, status);
                    }
                }
                else
                {
                    ProductBLL.ChangProductIsSpecial(queryString, status);
                }
            }
            ResponseHelper.Write(action + "|" + queryString.ToString());
            ResponseHelper.End();
        }

        private void UpdateTagsIsTop(string action)
        {
            int queryString = RequestHelper.GetQueryString<int>("ID");
            int status = RequestHelper.GetQueryString<int>("Status");
            TagsBLL.UpdateTagsIsTop(queryString, status);
            ResponseHelper.Write(action + "|" + queryString.ToString());
            ResponseHelper.End();
        }
    }
}
