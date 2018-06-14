using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.Pages;
using System.Text;

namespace XueFuShop.Admin
{
    public partial class TestCourseAdd : AdminBasePage
    {
        protected string color = string.Empty;
        protected int productID = 0;
        protected List<ProductPhotoInfo> productPhotoList = new List<ProductPhotoInfo>();
        protected string promotDetail = string.Empty;
        protected string brandIDStr = string.Empty;
        protected int sendCount = 0;
        protected List<UserGradeInfo> userGradeList = new List<UserGradeInfo>();
        protected int companyID = RequestHelper.GetForm<int>("CompanyID");
        protected string companyName = "上海孟特管理咨询有限公司";

        protected void AddProductPhoto(int productID)
        {
            string form = RequestHelper.GetForm<string>("ProductPhoto");
            if (form != string.Empty)
            {
                foreach (string str2 in form.Split(new char[] { ',' }))
                {
                    ProductPhotoInfo productPhoto = new ProductPhotoInfo();
                    productPhoto.ProductID = productID;
                    productPhoto.Name = str2.Split(new char[] { '|' })[0];
                    productPhoto.Photo = str2.Split(new char[] { '|' })[1];
                    ProductPhotoBLL.AddProductPhoto(productPhoto);
                }
            }
        }

        protected void BindClassBrandAttributeClassStandardType()
        {
            //List<ProductBrandInfo> list = ProductBrandBLL.ReadProductBrandCacheList();
            //this.RelationBrandID.DataSource = list;
            //this.RelationBrandID.DataTextField = "Name";
            //this.RelationBrandID.DataValueField = "ID";
            //this.RelationBrandID.DataBind();
            //this.RelationBrandID.Items.Insert(0, new ListItem("所有品牌", string.Empty));

            List<ProductClassInfo> list2 = ProductClassBLL.ReadProductClassNamedList();
            foreach (ProductClassInfo info in list2)
            {
                this.RelationClassID.Items.Add(new ListItem(info.ClassName, "|" + info.ID + "|"));
            }
            this.RelationClassID.Items.Insert(0, new ListItem("所有分类", string.Empty));

            CourseCateInfo CourseCateModel = new CourseCateInfo();
            CourseCateModel.Condition = CompanyBLL.SystemCompanyId.ToString();
            List<CourseCateInfo> courseCateList = CourseCateBLL.ReadCourseCateNamedList(CourseCateModel);
            foreach (CourseCateInfo info in courseCateList)
            {
                this.AccessoryClassID.Items.Add(new ListItem(info.CateName, info.CateId.ToString()));
            }
            this.AccessoryClassID.Items.Insert(0, new ListItem("所有分类", string.Empty));

            this.AttributeClassID.DataSource = AttributeClassBLL.ReadAttributeClassCacheList();
            this.AttributeClassID.DataTextField = "Name";
            this.AttributeClassID.DataValueField = "ID";
            this.AttributeClassID.DataBind();
            this.AttributeClassID.Items.Insert(0, new ListItem("请选择", "0"));
            foreach (ArticleClassInfo info2 in ArticleClassBLL.ReadArticleClassChildList(3))
            {
                this.ArticleClassID.Items.Add(new ListItem(info2.ClassName, "|" + info2.ID + "|"));
            }
            this.ArticleClassID.Items.Insert(0, new ListItem(ArticleClassBLL.ReadArticleClassCache(3).ClassName, "|" + 3 + "|"));
            this.StandardType.DataSource = EnumHelper.ReadEnumList<ProductStandardType>();
            this.StandardType.DataTextField = "ChineseName";
            this.StandardType.DataValueField = "Value";
            this.StandardType.DataBind();
        }

        protected void BindRelation(ProductInfo product)
        {
            ProductSearchInfo info2;
            if (product.RelationArticle != string.Empty)
            {
                ArticleSearchInfo articleSearch = new ArticleSearchInfo();
                articleSearch.InArticleID = product.RelationArticle;
                this.Article.DataSource = ArticleBLL.SearchArticleList(articleSearch);
                this.Article.DataTextField = "Title";
                this.Article.DataValueField = "ID";
                this.Article.DataBind();
            }
            if (product.RelationProduct != string.Empty)
            {
                info2 = new ProductSearchInfo();
                info2.InProductID = product.RelationProduct;
                this.Product.DataSource = ProductBLL.SearchProductList(info2);
                this.Product.DataTextField = "Name";
                this.Product.DataValueField = "ID";
                this.Product.DataBind();
            }
            if (product.Accessory != string.Empty)
            {
                CourseInfo courseSearch = new CourseInfo();
                courseSearch.Condition = product.Accessory;
                courseSearch.Field = "CourseId";
                this.Accessory.DataSource = CourseBLL.ReadList(courseSearch);
                this.Accessory.DataTextField = "CourseName";
                this.Accessory.DataValueField = "CourseId";
                this.Accessory.DataBind();
            }
        }

        protected void HanderAttribute(ProductInfo product)
        {
            if (product.ID > 0)
            {
                AttributeRecordBLL.DeleteAttributeRecordByProductID(product.ID.ToString());
            }
            List<AttributeInfo> list = AttributeBLL.ReadAttributeListByClassID(product.AttributeClassID);
            foreach (AttributeInfo info in list)
            {
                AttributeRecordInfo attributeRecord = new AttributeRecordInfo();
                attributeRecord.AttributeID = info.ID;
                attributeRecord.ProductID = product.ID;
                attributeRecord.Value = RequestHelper.GetForm<string>(info.ID.ToString() + "Value");
                AttributeRecordBLL.AddAttributeRecord(attributeRecord);
            }
        }

        protected void HanderMemberPrice(int productID)
        {
            if (productID > 0)
            {
                MemberPriceBLL.DeleteMemberPriceByProductID(productID.ToString());
            }
            List<UserGradeInfo> list = UserGradeBLL.ReadUserGradeCacheList();
            decimal form = -1M;
            foreach (UserGradeInfo info in list)
            {
                form = RequestHelper.GetForm<decimal>("MemberPrice" + info.ID);
                if (form != -1M)
                {
                    MemberPriceInfo memberPrice = new MemberPriceInfo();
                    memberPrice.ProductID = productID;
                    memberPrice.GradeID = info.ID;
                    memberPrice.Price = form;
                    MemberPriceBLL.AddMemberPrice(memberPrice);
                }
            }
        }

        protected void HanderProductStandard(ProductInfo product)
        {
            string strID = string.Empty;
            if (product.StandardType == 2)
            {
                strID = ("," + RequestHelper.GetForm<string>("Product") + ",").Replace(",0,", "," + product.ID.ToString() + ",");
                strID = strID.Substring(1, strID.Length - 2);
            }
            ProductBLL.UpdateProductStandardType(strID, product.StandardType, product.ID);
            if (product.ID > 0)
            {
                StandardRecordBLL.DeleteStandardRecordByProductID(product.ID.ToString());
            }
            if (product.StandardType != 0)
            {
                int form = RequestHelper.GetForm<int>("recordCount");
                string str2 = RequestHelper.GetForm<string>("StandardIDList");
                if (str2 != string.Empty)
                {
                    string[] strArray = strID.Split(new char[] { ',' });
                    int index = 0;
                    for (int i = 0; i < form; i++)
                    {
                        if (base.Request.Form["Standard" + i] != null)
                        {
                            StandardRecordInfo standardRecord = new StandardRecordInfo();
                            standardRecord.StandardIDList = str2;
                            standardRecord.ValueList = RequestHelper.GetForm<string>("Standard" + i);
                            if (product.StandardType == 2)
                            {
                                standardRecord.GroupTag = strID;
                                standardRecord.ProductID = Convert.ToInt32(strArray[index]);
                            }
                            else
                            {
                                standardRecord.ProductID = product.ID;
                            }
                            StandardRecordBLL.AddStandardRecord(standardRecord);
                            index++;
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (companyID < 0) companyID = CompanyBLL.SystemCompanyId;
            if (!this.Page.IsPostBack)
            {
                this.BindClassBrandAttributeClassStandardType();
                this.ProductClass.DataSource = ProductClassBLL.ReadProductClassUnlimitClass();
                this.productID = RequestHelper.GetQueryString<int>("ID");
                if (this.productID != -2147483648)
                {
                    base.CheckAdminPower("ReadProduct", PowerCheckType.Single);
                    ProductInfo product = ProductBLL.ReadProduct(this.productID);
                    companyID = product.CompanyID;
                    companyName = CompanyBLL.ReadCompany(product.CompanyID).CompanyName;
                    this.Name.Text = product.Name;
                    this.Name.Attributes.Add("style", "color:" + product.Color);
                    this.color = product.Color;
                    //this.FontStyle.Text = product.FontStyle;
                    this.ProductNumber.Text = product.ProductNumber;
                    this.ProductClass.ClassIDList = product.ClassID;
                    this.Keywords.Text = product.Keywords;
                    brandIDStr = product.BrandID;
                    this.MarketPrice.Text = product.MarketPrice.ToString();
                    this.Photo.Text = product.Photo;
                    this.Summary.Text = product.Summary;
                    this.Introduction.Value = product.Introduction;
                    this.IsSpecial.Text = product.IsSpecial.ToString();
                    this.IsNew.Text = product.IsNew.ToString();
                    this.IsHot.Text = product.IsHot.ToString();
                    this.IsSale.Text = product.IsSale.ToString();
                    this.IsTop.Text = product.IsTop.ToString();
                    this.Remark.Text = product.Remark;
                    this.AllowComment.Text = product.AllowComment.ToString();
                    this.AttributeClassID.Text = product.AttributeClassID.ToString();
                    this.StandardType.Text = product.StandardType.ToString();
                    this.sendCount = product.SendCount;
                    this.Sort.Text = product.Sort.ToString();
                    this.Editor.Text = product.Editor;
                    this.BindRelation(product);
                    this.BindTestSetting(product);
                    this.productPhotoList = ProductPhotoBLL.ReadProductPhotoByProduct(this.productID);
                }
                this.BindSystemTestSetting();
                this.userGradeList = UserGradeBLL.JoinUserGrade(this.productID);
            }
        }

        protected void BindTestSetting(ProductInfo product)
        {
            TestSettingInfo systemTestSetting = TestSettingBLL.ReadCompanyTestSetting(product.CompanyID, product.ID);
            if (systemTestSetting != null)
            {
                TestStartTime.Text = systemTestSetting.TestStartTime.ToString().Replace('/', '-');
                TestEndTime.Text = systemTestSetting.TestEndTime.ToString().Replace('/', '-');
                PaperScore.Text = systemTestSetting.PaperScore.ToString();
                TestTimeLength.Text = systemTestSetting.TestTimeLength.ToString();
                QuestionsNum.Text = systemTestSetting.TestQuestionsCount.ToString();
                LowScore.Text = systemTestSetting.LowScore.ToString();
                TestInterval.Text = systemTestSetting.TestInterval.ToString();
            }
        }

        protected void BindSystemTestSetting()
        {
            TestSettingInfo systemTestSetting = TestSettingBLL.ReadSystemTestSetting();
            PaperScorseTips.Text = PaperScore.HintInfo = "留空总分则为" + systemTestSetting.PaperScore + "分";
            TestTimeLengthTips.Text = TestTimeLength.HintInfo = "留空考试时长则为" + systemTestSetting.TestTimeLength + "分钟";
            QuestionsNumTips.Text = QuestionsNum.HintInfo = "留空则为" + systemTestSetting.TestQuestionsCount + "道试题";
            LowScoreTips.Text = LowScore.HintInfo = "留空则及格线为" + systemTestSetting.LowScore + "分";
            TestIntervalTips.Text = TestInterval.HintInfo = "两次考试相隔的小时数，留空为" + systemTestSetting.TestInterval + "小时。";
        }

        protected void HanderTestSetting(ProductInfo product)
        {
            try
            {
                string paperScore = PaperScore.Text.Trim();
                string testTimeLength = TestTimeLength.Text.Trim();
                string testQuestionsCount = QuestionsNum.Text.Trim();
                string lowScore = LowScore.Text.Trim();
                string testStartTime = TestStartTime.Text.Trim();
                string testEndTime = TestEndTime.Text.Trim();
                string testInterval = TestInterval.Text.Trim();

                TestSettingInfo testSetting = new TestSettingInfo();
                if (!string.IsNullOrEmpty(paperScore))
                    testSetting.PaperScore = Convert.ToInt32(paperScore);
                if (!string.IsNullOrEmpty(testTimeLength))
                    testSetting.TestTimeLength = Convert.ToInt32(testTimeLength);
                if (!string.IsNullOrEmpty(testQuestionsCount))
                    testSetting.TestQuestionsCount = Convert.ToInt32(testQuestionsCount);
                if (!string.IsNullOrEmpty(lowScore))
                    testSetting.LowScore = Convert.ToInt32(lowScore);
                if (!string.IsNullOrEmpty(testStartTime) && !string.IsNullOrEmpty(testEndTime))
                {
                    testSetting.TestStartTime = Convert.ToDateTime(testStartTime);
                    testSetting.TestEndTime = Convert.ToDateTime(testEndTime);
                }
                if (!string.IsNullOrEmpty(testInterval))
                {
                    testSetting.TestInterval = Convert.ToInt32(testInterval);
                }
                TestSettingBLL.UpdateTestSetting(product.CompanyID, product.ID, testSetting);
            }
            catch (Exception ex)
            {
                ScriptHelper.Alert("考试时间设置出现异常！");
            }

            //TestSettingInfo systemTestSetting = TestSettingBLL.ReadCompanyTestSetting(product.CompanyID, product.ID);
            //if (!string.IsNullOrEmpty(paperScore) || !string.IsNullOrEmpty(testTimeLength) || !string.IsNullOrEmpty(testQuestionsCount) || !string.IsNullOrEmpty(testQuestionsCount) || !string.IsNullOrEmpty(lowScore) || !string.IsNullOrEmpty(testStartTime) || !string.IsNullOrEmpty(testEndTime))
            //{
            //    if (systemTestSetting == null)
            //    {
            //        systemTestSetting = TestSettingBLL.ReadTestSetting(product.CompanyID);
            //        systemTestSetting.CompanyId = product.CompanyID;
            //        systemTestSetting.CourseId = product.ID;
            //    }
            //    if (!string.IsNullOrEmpty(paperScore)) systemTestSetting.PaperScore = Convert.ToInt32(paperScore);
            //    if (!string.IsNullOrEmpty(testTimeLength)) systemTestSetting.TestTimeLength = Convert.ToInt32(testTimeLength);
            //    if (!string.IsNullOrEmpty(testQuestionsCount)) systemTestSetting.TestQuestionsCount = Convert.ToInt32(testQuestionsCount);
            //    if (!string.IsNullOrEmpty(lowScore)) systemTestSetting.LowScore = Convert.ToInt32(lowScore);
            //    //下面两项同时为空或同时不为空时可以修改
            //    if ((!string.IsNullOrEmpty(testStartTime) && !string.IsNullOrEmpty(testEndTime)) || (string.IsNullOrEmpty(testStartTime) && string.IsNullOrEmpty(testEndTime)))
            //    {
            //        systemTestSetting.TestStartTime = testStartTime;
            //        systemTestSetting.TestEndTime = testEndTime;
            //    }
            //    TestSettingBLL.AddTestSetting(systemTestSetting);
            //}
            //else
            //{
            //    if (systemTestSetting != null)
            //    {
            //        TestSettingBLL.DeleteTestSetting(systemTestSetting.Id);
            //    }
            //}
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            ProductInfo product = new ProductInfo();
            product.ID = RequestHelper.GetQueryString<int>("ID");
            product.CompanyID = companyID;
            product.Name = this.Name.Text;
            product.Spelling = ChineseCharacterHelper.GetFirstLetter(this.Name.Text);
            product.Color = RequestHelper.GetForm<string>("ProductColor");
            //product.FontStyle = this.FontStyle.Text;
            product.ProductNumber = this.ProductNumber.Text;
            product.ClassID = this.ProductClass.ClassIDList;
            product.Keywords = this.Keywords.Text;
            if (string.IsNullOrEmpty(RequestHelper.GetForm<string>("AllBrandID")))
                product.BrandID = RequestHelper.GetIntsForm("BrandID");
            else
                product.BrandID = RequestHelper.GetIntsForm("AllBrandID");
            product.MarketPrice = Convert.ToDecimal(this.MarketPrice.Text);
            product.Photo = this.Photo.Text;
            product.Summary = this.Summary.Text;
            product.Introduction = this.Introduction.Value;
            product.IsSpecial = Convert.ToInt32(this.IsSpecial.Text);
            product.IsNew = Convert.ToInt32(this.IsNew.Text);
            product.IsHot = Convert.ToInt32(this.IsHot.Text);
            product.IsSale = Convert.ToInt32(this.IsSale.Text);
            product.IsTop = Convert.ToInt32(this.IsTop.Text);
            product.Remark = this.Remark.Text;
            product.Accessory = RequestHelper.GetForm<string>("RelationAccessoryID");
            product.RelationProduct = RequestHelper.GetForm<string>("RelationProductID");
            product.RelationArticle = RequestHelper.GetForm<string>("RelationArticleID");
            product.AllowComment = Convert.ToInt32(this.AllowComment.Text);
            product.AttributeClassID = Convert.ToInt32(this.AttributeClassID.Text);
            product.StandardType = Convert.ToInt32(this.StandardType.Text);
            product.AddDate = RequestHelper.DateNow;
            product.Sort = Convert.ToInt32(Sort.Text);
            product.Editor = Editor.Text;
            int productID = 0;
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (product.ID == -2147483648)
            {
                base.CheckAdminPower("AddProduct", PowerCheckType.Single);
                productID = ProductBLL.AddProduct(product);
                this.AddProductPhoto(productID);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Product"), productID);
            }
            else
            {
                base.CheckAdminPower("UpdateProduct", PowerCheckType.Single);
                //公司发生变化,删除旧的考试设置
                ProductInfo oldProduct = ProductBLL.ReadProduct(product.ID);
                if (product.CompanyID != ProductBLL.ReadProduct(product.ID).CompanyID)
                {
                    try
                    {
                        TestSettingBLL.DeleteTestSetting(TestSettingBLL.ReadCompanyTestSetting(oldProduct.CompanyID, product.ID).Id);
                    }
                    catch (Exception ex)
                    { }
                }
                ProductBLL.UpdateProduct(product);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Product"), product.ID);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            this.HanderAttribute(product);
            //this.HanderMemberPrice(product.ID);
            this.HanderProductStandard(product);
            if (product.ID == int.MinValue) product.ID = productID;
            this.HanderTestSetting(product);
            AdminBasePage.Alert(alertMessage, RequestHelper.RawUrl);
        }

        protected string CreatBrandCheckBoxHtml(string brandIDStr)
        {
            StringBuilder brandCheckBoxHtml = new StringBuilder();
            brandCheckBoxHtml.Append("<div class=\"checkboxlistarea\">");
            brandCheckBoxHtml.AppendLine("<dl>");
            brandCheckBoxHtml.Append("<dt><input name=\"AllBrandID\" type=\"checkbox\" value=\"0\" ");
            if (brandIDStr == "0")
                brandCheckBoxHtml.Append(" checked");
            brandCheckBoxHtml.Append(">所有品牌</dt>");
            brandCheckBoxHtml.AppendLine("<div class=\"checkboxlist\">");
            foreach (ProductBrandInfo info in ProductBrandBLL.ReadProductBrandCacheList())
            {
                brandCheckBoxHtml.AppendLine("<dd><input name=\"BrandID\" data-type=\"0\" type=\"checkbox\" value=\"" + info.ID + "\"");
                if (StringHelper.CompareStringContainSpecialValue(brandIDStr, info.ID.ToString()))
                {
                    brandCheckBoxHtml.Append(" checked");
                }
                brandCheckBoxHtml.Append(">" + info.Name + "</dd>");
            }
            brandCheckBoxHtml.AppendLine("</div>");
            brandCheckBoxHtml.AppendLine("</dl>");
            brandCheckBoxHtml.Append("</div>");

            return brandCheckBoxHtml.ToString();
        }
    }
}
