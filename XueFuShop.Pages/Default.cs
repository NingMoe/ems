using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFu.EntLib;

namespace XueFuShop.Pages
{
    public class Default : CommonBasePage
    {

        protected List<ArticleInfo> newsList = new List<ArticleInfo>();
        protected List<string> certList = new List<string>();
        protected List<ProductInfo> hotProductList = new List<ProductInfo>();
        //protected List<MemberPriceInfo> memberPriceList = new List<MemberPriceInfo>();
        protected List<ProductInfo> newProductList = new List<ProductInfo>();
        //protected List<LinkInfo> pictureLinkList = new List<LinkInfo>();
        protected List<ProductInfo> specialProductList = new List<ProductInfo>();
        protected List<ProductInfo> freeProductList = new List<ProductInfo>();
        //protected List<LinkInfo> textLinkList = new List<LinkInfo>();
        protected List<AttributeRecordInfo> attributeRecordList = new List<AttributeRecordInfo>();
        protected Dictionary<string, List<ProductInfo>> productListGroupByProductRootClass = new Dictionary<string, List<ProductInfo>>();

        protected override void PageLoad()
        {
            if (!StringHelper.CompareSingleString(base.ParentCompanyID, "667") && base.UserID > 0)
                Response.Redirect("/Bussiness.aspx");//User/CourseCenter.aspx
            base.PageLoad();
            //ArticleSearchInfo article = new ArticleSearchInfo();
            //article.ClassID = "|" + 1.ToString() + "|";
            int count = -2147483648;
            //this.newsList = ArticleBLL.SearchArticleList(1, 7, article, ref count);
            ProductSearchInfo product = new ProductSearchInfo();
            product.IsNew = 1;
            //product.IsTop = 1;
            product.IsSale = 1;
            count = -2147483648;
            this.newProductList = ProductBLL.SearchProductList(1, 15, product, ref count);
            //product.IsTop = 1;
            //product.IsSale = 1;
            //product.MarketPrice = 0;
            //count = -2147483648;
            //this.freeProductList = ProductBLL.SearchProductList(1, 6, product, ref count);
            //product = new ProductSearchInfo();
            //product.IsHot = 1;
            //product.IsTop = 1;
            //product.IsSale = 1;
            //count = -2147483648;
            //this.hotProductList = ProductBLL.SearchProductList(1, 10, product, ref count);
            //product = new ProductSearchInfo();
            //product.IsSpecial = 1;
            //product.IsTop = 1;
            //product.IsSale = 1;
            //count = -2147483648;
            //this.specialProductList = ProductBLL.SearchProductList(product); //ProductBLL.SearchProductList(1, 10, product, ref count);
            //List<ProductInfo> list = new List<ProductInfo>();
            //list.AddRange(this.newProductList);
            //list.AddRange(this.hotProductList);
            //list.AddRange(this.specialProductList);
            //string strProductID = string.Empty;
            //foreach (ProductInfo info3 in list)
            //{
            //    if (strProductID == string.Empty)
            //    {
            //        strProductID = info3.ID.ToString();
            //    }
            //    else
            //    {
            //        strProductID = strProductID + "," + info3.ID.ToString();
            //    }
            //}
            //if (strProductID != string.Empty)
            //{
            //    this.memberPriceList = MemberPriceBLL.ReadMemberPriceByProductGrade(strProductID, base.GradeID);
            //}
            //this.textLinkList = LinkBLL.ReadLinkCacheListByClass(1);
            //this.pictureLinkList = LinkBLL.ReadLinkCacheListByClass(2);

            attributeRecordList = AttributeRecordBLL.ReadList("1,2,3", ProductBLL.ReadProductIdStr(this.newProductList));

            //每日快报
            ArticleSearchInfo articleSearch = new ArticleSearchInfo();
            articleSearch.ClassID = "|29|";
            this.newsList = ArticleBLL.SearchArticleList(1, 12, articleSearch, ref base.Count);

            //证书
            certList = PostPassBLL.GetTheLatestPostCert(10);
        }

        protected List<ProductInfo> CateProductList(string ClassId, int ShowNum)
        {
            ProductSearchInfo product = new ProductSearchInfo();
            product.ClassID = "|" + ClassId + "|";
            product.IsTop = 1;
            product.IsSale = 1;
            int count = -2147483648;
            return ProductBLL.SearchProductList(1, ShowNum, product, ref count);
        }
    }
}
