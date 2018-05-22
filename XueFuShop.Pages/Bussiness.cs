using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFu.EntLib;

namespace XueFuShop.Pages
{

    public class Bussiness : UserCommonBasePage
    {
        protected List<ArticleInfo> newsList = new List<ArticleInfo>();
        protected List<string> certList = new List<string>();
        protected List<ProductInfo> newProductList = new List<ProductInfo>();
        protected Dictionary<int, ArticleInfo> articleDc = new Dictionary<int, ArticleInfo>();
        protected string companyBrandID = string.Empty;
        protected string passPostCourseID = string.Empty;
        protected List<AttributeRecordInfo> attributeRecordList = new List<AttributeRecordInfo>();

        protected override void PageLoad()
        {
            if (StringHelper.CompareSingleString(base.ParentCompanyID, "667"))
                Response.Redirect("/");
            base.PageLoad();
            base.CheckUserPower("PostStudy", PowerCheckType.Single);
            //最新一条信息
            int[] classIDArray = new int[] { 33, 37, 38 };
            foreach (int item in classIDArray)
            {
                ArticleInfo article = ArticleBLL.ReadFirstAtricle(item);
                if (article.Date >= DateTime.Now.AddDays(-7))
                {
                    articleDc.Add(item, article);
                }
            }

            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.IsNew = 1;
            //productSearch.IsTop = 1;
            productSearch.IsSale = 1;
            int count = -2147483648;
            this.newProductList = ProductBLL.SearchProductList(1, 10, productSearch, ref count);

            //每日快报
            ArticleSearchInfo articleSearch = new ArticleSearchInfo();
            articleSearch.ClassID = "|29|";
            this.newsList = ArticleBLL.SearchArticleList(1, 12, articleSearch, ref base.Count);

            //证书
            certList = PostPassBLL.GetTheLatestPostCert(10);


            attributeRecordList = AttributeRecordBLL.ReadList("1,2,3", ProductBLL.ReadProductIdStr(this.newProductList));
            int studyPostID = int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId"));
            //this.studyPost = PostBLL.ReadPost(studyPostID);
            string postCourseID = PostBLL.ReadPostCourseID(base.UserCompanyID, studyPostID);
            companyBrandID = CookiesHelper.ReadCookieValue("UserCompanyBrandID");
            passPostCourseID = string.IsNullOrEmpty(postCourseID) ? "" : TestPaperBLL.ReadCourseIDStr(TestPaperBLL.ReadList(base.UserID, postCourseID, 1));
        }
    }
}
