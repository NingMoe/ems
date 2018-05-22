using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class CourseCenter : UserCommonBasePage
    {
        protected PostInfo studyPost = new PostInfo();
        protected List<ProductClassInfo> productClassList = new List<ProductClassInfo>();
        protected int passType = RequestHelper.GetQueryString<int>("Type");
        protected int rz = RequestHelper.GetQueryString<int>("RZ");
        protected int pc = RequestHelper.GetQueryString<int>("PC");
        protected string view = RequestHelper.GetQueryString<string>("View");
        protected Dictionary<string, Dictionary<string, string>> xxProductClassList = new Dictionary<string, Dictionary<string, string>>();
        protected Dictionary<string, Dictionary<string, string>> postProductClassList = new Dictionary<string, Dictionary<string, string>>();
        protected Dictionary<string, string> tempProductClassList = new Dictionary<string, string>();
        protected int postCourseNum = 0;
        protected int passCourseNum = 0;
        protected string postImageName = string.Empty;
        protected bool hasSpecialTest = false;
        //protected bool isTestAgain = false;
        protected List<ProductInfo> newProductList = new List<ProductInfo>();
        protected List<AttributeRecordInfo> newProductAttributeRecordList = new List<AttributeRecordInfo>();

        protected override void PageLoad()
        {
            if (StringHelper.CompareSingleString(base.ParentCompanyID, "667"))
                Response.Redirect("/");
            base.PageLoad();
            base.CheckUserPower("PostStudy", PowerCheckType.Single);

            ProductSearchInfo product = new ProductSearchInfo();
            product.IsNew = 1;
            //product.IsTop = 1;
            product.IsSale = 1;
            int count = 0;
            this.newProductList = ProductBLL.SearchProductList(1, 10, product, ref count);
            this.newProductAttributeRecordList = AttributeRecordBLL.ReadList("3", ProductBLL.ReadProductIdStr(this.newProductList));

            //默认加载视图模式
            if (string.IsNullOrEmpty(view)) view = "Grid";

            int studyPostID = int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId"));
            this.studyPost = PostBLL.ReadPost(studyPostID);
            this.productClassList = ProductClassBLL.ReadProductClassRootList();
            string postCourseID = PostBLL.ReadPostCourseID(base.UserCompanyID, studyPostID);
            postCourseNum = string.IsNullOrEmpty(postCourseID) ? 0 : postCourseID.Split(',').Length;
            string passPostCourseID = string.IsNullOrEmpty(postCourseID) ? "" : TestPaperBLL.ReadCourseIDStr(TestPaperBLL.ReadList(base.UserID, postCourseID, 1));
            passCourseNum = string.IsNullOrEmpty(passPostCourseID) ? 0 : passPostCourseID.Split(',').Length;
            //isTestAgain = TestSettingBLL.IsTestAgain(base.UserID, int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId")));

            switch (studyPostID)
            {
                case 5:
                case 37:
                case 64:
                case 87:
                case 693:
                    postImageName = "post1.jpg";
                    break;
                case 8:
                case 86:
                case 4:
                case 85:
                case 276:
                case 286:
                    postImageName = "post2.jpg";
                    break;
                case 82:
                case 220:
                case 166:
                case 168:
                case 88:
                case 251:
                case 258:
                case 302:
                case 308:
                case 341:
                    postImageName = "post3.jpg";
                    break;
                case 83:
                case 89:
                case 158:
                case 265:
                case 312:
                case 318:
                    postImageName = "post4.jpg";
                    break;
                case 11:
                case 327:
                    postImageName = "post5.jpg";
                    break;

                default:
                    postImageName = "post1.jpg";
                    break;
            }

            //视图模式时加载分类目录
            if (view == "Grid")
            {
                //公共课程
                if (pc == 1)
                {
                    ProductSearchInfo productSearch = new ProductSearchInfo();
                    productSearch.ClassID = "|5298|";
                    productSearch.IsSale = 1;
                    postCourseID = ProductBLL.ReadProductIdStr(ProductBLL.SearchProductList(productSearch));
                }
                //获取认证课程ID
                else if (rz == 1)
                {
                    PostPassInfo passpost = new PostPassInfo();
                    passpost.UserId = base.UserID;
                    passpost.IsRZ = int.MinValue;

                    RenZhengCateInfo rzCate = new RenZhengCateInfo();
                    rzCate.InPostID = PostPassBLL.PassPostString(passpost);
                    postCourseID = RenZhengCateBLL.ReadTestCateID(rzCate);
                }
                else
                {
                    //默认加载未通过的课程
                    if (passType <= 0) passType = 0;

                    if (passType == 1 || passType == 0)
                    {
                        if (passType == 1)
                            postCourseID = passPostCourseID;//StringHelper.EqualString(postCourseID, filterCourseID);
                        else if (passType == 0)
                            postCourseID = StringHelper.SubString(postCourseID, passPostCourseID);
                    }
                }

                if (!string.IsNullOrEmpty(postCourseID))
                    this.postProductClassList = ProductClassBLL.ReadProductClassListByProductID(postCourseID, 1);
                if (postProductClassList.Count > 1)
                    this.postProductClassList = productClassSort(this.postProductClassList);

                //特定时间考试
                if (pc != 1 && rz != 1)
                {
                    string parentCompanyID = CookiesHelper.ReadCookieValue("UserCompanyParentCompanyID");
                    //if (string.IsNullOrEmpty(parentCompanyID)) parentCompanyID = base.UserCompanyID.ToString();
                    //else parentCompanyID += "," + base.UserCompanyID.ToString();
                    List<TestSettingInfo> specialTestList = TestSettingBLL.ReadSpecialTestList(parentCompanyID);
                    if (specialTestList.Count > 0)
                    {
                        string specialCourseID = TestSettingBLL.ReadSpecialTestCourseID(specialTestList);
                        if (!string.IsNullOrEmpty(specialCourseID))
                        {
                            ProductSearchInfo productSearch = new ProductSearchInfo();
                            productSearch.InProductID = specialCourseID;
                            productSearch.InBrandID = CookiesHelper.ReadCookieValue("UserCompanyBrandID");
                            productSearch.IsSale = 1;
                            specialCourseID = ProductBLL.ReadProductIdStr(ProductBLL.SearchProductList(productSearch));
                            string passSpecialCourseID = TestPaperBLL.ReadCourseIDStr(TestPaperBLL.ReadList(base.UserID, specialCourseID, 1));
                            if (passType == 1)
                                specialCourseID = passSpecialCourseID;
                            else if (passType == 0)
                                specialCourseID = StringHelper.SubString(specialCourseID, passSpecialCourseID);

                            if (!string.IsNullOrEmpty(specialCourseID))
                                hasSpecialTest = true;
                        }
                    }
                }

                //三个岗位加载 竞品选修
                if (passType >= 0 && StringHelper.CompareSingleString("4,5,64", studyPostID.ToString()))
                {
                    ProductSearchInfo productSearch = new ProductSearchInfo();
                    productSearch.ClassID = "|6|";
                    productSearch.InBrandID = CookiesHelper.ReadCookieValue("UserCompanyBrandID");
                    productSearch.IsSale = 1;
                    productSearch.NotLikeName = "必修";
                    string xxCourseID = ProductBLL.ReadProductIdStr(ProductBLL.SearchProductList(productSearch));
                    string passXXCourseID = TestPaperBLL.ReadCourseIDStr(TestPaperBLL.ReadList(base.UserID, xxCourseID, 1));
                    if (passType == 1)
                        xxCourseID = passXXCourseID;
                    else if (passType == 0)
                        xxCourseID = StringHelper.SubString(xxCourseID, passXXCourseID);
                    if (!string.IsNullOrEmpty(xxCourseID))
                        this.xxProductClassList = ProductClassBLL.ReadProductClassListByProductID(xxCourseID, 1);
                }
            }
        }

        /// <summary>
        /// 一级分类排序
        /// </summary>
        /// <param name="productClassDic"></param>
        /// <returns></returns>
        protected Dictionary<string, Dictionary<string, string>> productClassSort(Dictionary<string, Dictionary<string, string>> productClassDic)
        {
            List<ProductClassInfo> classList = new List<ProductClassInfo>();
            Dictionary<string, Dictionary<string, string>> sortProductClassDic = new Dictionary<string, Dictionary<string, string>>();
            foreach (string key in productClassDic.Keys)
            {
                classList.Add(ProductClassBLL.ReadProductClassByProductClassList(this.productClassList, int.Parse(key)));
            }

            classList.Sort(SortCompare);

            for (int i = 0; i < classList.Count; i++)
            {
                sortProductClassDic.Add(classList[i].ID.ToString(), productClassDic[classList[i].ID.ToString()]);
            }

            return sortProductClassDic;
        }

        /// <summary>
        /// 二级分类排序
        /// </summary>
        /// <param name="productClassDic"></param>
        /// <returns></returns>
        protected Dictionary<string, string> productClassSort(Dictionary<string, string> productClassDic)
        {
            List<ProductClassInfo> classList = new List<ProductClassInfo>();
            Dictionary<string, string> sortProductClassDic = new Dictionary<string, string>();
            foreach (string key in productClassDic.Keys)
            {
                classList.Add(ProductClassBLL.ReadProductClassCache(int.Parse(key)));
            }

            classList.Sort(SortCompare);

            for (int i = 0; i < classList.Count; i++)
            {
                sortProductClassDic.Add(classList[i].ID.ToString(), productClassDic[classList[i].ID.ToString()]);
            }

            return sortProductClassDic;
        }

        #region SortCompare()函数，对List<ProductClassInfo>进行排序时作为参数使用

        /// <summary>
        /// 对List<ProductClassInfo>进行排序时作为参数使用（升序）
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        private static int SortCompare(ProductClassInfo c1, ProductClassInfo c2)
        {
            if (c1.OrderID < c2.OrderID)
            {
                return -1;
            }
            else if (c1.OrderID > c2.OrderID)
            {
                return 1;
            }
            return 0;
        }
        #endregion

    }
}
