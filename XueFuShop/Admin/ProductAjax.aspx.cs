using System;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class ProductAjax : AdminBasePage
    {
        protected string action = string.Empty;
        protected List<ArticleInfo> articleList = new List<ArticleInfo>();
        protected string controlName = string.Empty;
        protected string cssContent = string.Empty;
        protected string dobuleClickContent = string.Empty;
        protected List<EmailContentInfo> emailContentList = new List<EmailContentInfo>();
        protected List<ProductInfo> productList = new List<ProductInfo>();
        protected List<UserInfo> userList = new List<UserInfo>();
        protected List<CourseInfo> courseList = new List<CourseInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            base.ClearCache();
            this.controlName = RequestHelper.GetQueryString<string>("ControlName");
            this.action = RequestHelper.GetQueryString<string>("Action");
            string queryString = RequestHelper.GetQueryString<string>("Flag");
            string action = this.action;
            if (action != null)
            {
                if (!(action == "SearchRelationProduct"))
                {
                    if (action == "SearchProductQuestionBank")
                    {
                        this.SearchProductQuestionBank();
                        this.cssContent = "style=\"width:350px; height:300px;\"multiple=\"multiple\"";
                        this.dobuleClickContent = " ondblclick=\"addProductAccessorySingle('" + base.IDPrefix + "CandidateAccessory','" + base.IDPrefix + "Accessory')\"";
                    }
                    else if (action == "SearchProductAccessory")
                    {
                        this.SearchProductAccessory();
                        this.cssContent = "style=\"width:260px; height:300px;\"multiple=\"multiple\"";
                        this.dobuleClickContent = " ondblclick=\"addProductAccessorySingle('" + base.IDPrefix + "CandidateAccessory','" + base.IDPrefix + "Accessory')\"";
                    }
                    else if (action == "SearchRelationArticle")
                    {
                        this.SearchRelationArticle();
                        this.cssContent = "style=\"width:260px; height:300px;\"multiple=\"multiple\"";
                        this.dobuleClickContent = " ondblclick=\"addSingle('" + base.IDPrefix + "CandidateArticle','" + base.IDPrefix + "Article')\"";
                    }
                    else if (action == "SearchUser")
                    {
                        this.SearchUser();
                        this.cssContent = "style=\"width:260px; height:300px;\"multiple=\"multiple\"";
                        this.dobuleClickContent = " ondblclick=\"addSingle('" + base.IDPrefix + "CandidateUser','" + base.IDPrefix + "User')\"";
                    }
                    else if (action == "SearchProductByName")
                    {
                        this.SearchProductByName();
                        this.cssContent = "style=\"width:240px;\"";
                    }
                }
                else
                {
                    this.SearchRelationProduct();
                    this.cssContent = "style=\"width:260px; height:300px;\"multiple=\"multiple\"";
                    this.dobuleClickContent = " ondblclick=\"addSingle('" + base.IDPrefix + "CandidateProduct','" + base.IDPrefix + "Product')\"";
                }
            }
        }

        private void SearchProductQuestionBank()
        {
            CourseInfo courseSearch = new CourseInfo();
            courseSearch.CourseName = RequestHelper.GetQueryString<string>("CourseName");
            courseSearch.CateIdCondition = RequestHelper.GetQueryString<string>("ClassID");
            courseSearch.Condition = CompanyBLL.SystemCompanyId.ToString();
            courseSearch.Field = "CompanyId";
            this.courseList = CourseBLL.ReadList(courseSearch);
        }

        private void SearchProductAccessory()
        {
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.NotInProductID = RequestHelper.GetQueryString<string>("ID");
            productSearch.Name = RequestHelper.GetQueryString<string>("ProductName");
            productSearch.ClassID = RequestHelper.GetQueryString<string>("ClassID");
            productSearch.InBrandID = RequestHelper.GetQueryString<string>("BrandID");
            this.productList = ProductBLL.SearchProductList(productSearch);
        }

        private void SearchProductByName()
        {
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.Name = RequestHelper.GetQueryString<string>("ProductName");
            this.productList = ProductBLL.SearchProductList(productSearch);
        }

        private void SearchRelationArticle()
        {
            ArticleSearchInfo articleSearch = new ArticleSearchInfo();
            articleSearch.Title = RequestHelper.GetQueryString<string>("ArticleTitle");
            articleSearch.ClassID = RequestHelper.GetQueryString<string>("ClassID");
            this.articleList = ArticleBLL.SearchArticleList(articleSearch);
        }

        private void SearchRelationProduct()
        {
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.NotInProductID = RequestHelper.GetQueryString<string>("ID");
            productSearch.Name = RequestHelper.GetQueryString<string>("ProductName");
            productSearch.ClassID = RequestHelper.GetQueryString<string>("ClassID");
            productSearch.InBrandID = RequestHelper.GetQueryString<string>("BrandID");
            productSearch.IsSale = 1;
            this.productList = ProductBLL.SearchProductList(productSearch);
        }

        private void SearchUser()
        {
            UserSearchInfo user = new UserSearchInfo();
            user.UserName = RequestHelper.GetQueryString<string>("UserName");
            this.userList = UserBLL.SearchUserList(user);
        }
    }
}
