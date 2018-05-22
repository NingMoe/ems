using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFuShop.Common;
using System.Collections;

namespace XueFuShop.Pages
{
    public class Product : CommonBasePage
    {
        //protected string relationSearch = string.Empty;
        //protected string searchCondition = string.Empty;
        //protected int searchType = 1;
        protected string showCondition = string.Empty;
        protected string showTitle = string.Empty;
        //protected string showDescription = string.Empty;
        protected List<PostInfo> postList = new List<PostInfo>();
        protected List<ProductInfo> productSearchList = new List<ProductInfo>();
        protected int classID = RequestHelper.GetQueryString<int>("ClassID");
        protected int postID = RequestHelper.GetQueryString<int>("PostID");
        protected string brandID = RequestHelper.GetQueryString<string>("BrandID");
        protected CommonPagerClass commonPagerClass = new CommonPagerClass();

        protected override void PageLoad()
        {
            base.PageLoad();
            postList = PostBLL.ReadPostList();
            this.SearchCondition();
        }


        protected void SearchCondition()
        {
            string keyword = RequestHelper.GetQueryString<string>("Keyword");
            string tags = RequestHelper.GetQueryString<string>("Tags");

            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.IsSale = 1;
            productSearch.IsSpecial = 1;

            if (!string.IsNullOrEmpty(brandID))
            {
                List<ProductBrandInfo> productBrandList = ProductBrandBLL.ReadProductBrandCacheList(brandID);
                foreach (ProductBrandInfo info in productBrandList)
                {
                    showCondition += ShowCondition("BrandID", info.Name);
                }
                productSearch.InBrandID = brandID;
            }

            if (postID > 0)
            {
                PostInfo studyPost = PostBLL.ReadPost(postID);
                showCondition += ShowCondition("PostID", studyPost.PostName);
                productSearch.InProductID = studyPost.PostPlan;
            }

            if (classID > 0)
            {
                ProductClassInfo productClass = ProductClassBLL.ReadProductClassCache(classID);
                showCondition += ShowCondition("ClassID", productClass.ClassName);
                productSearch.ClassID = "|" + classID.ToString() + "|";
            }

            if (keyword != string.Empty)
            {
                showCondition += "“" + keyword + "”";
                productSearch.Key = keyword;
            }

            productSearchList = ProductBLL.SearchProductList(base.CurrentPage, 15, productSearch, ref Count);
            this.commonPagerClass.CurrentPage = base.CurrentPage;
            this.commonPagerClass.PageSize = 15;
            this.commonPagerClass.Count = Count;

            if (string.IsNullOrEmpty(showCondition))
                showCondition = "<li class=\"select-no\">您暂无选择筛选条件<span class=\"count-result\">共<span style=\"margin:0 4px;\">" + base.Count.ToString() + "</span>个结果</span></li>";

            //this.searchCondition = "ClassID=" + classID.ToString() + "&ProductName=" + keyword + "&BrandID=" + brandID.ToString() + "&Tags=" + tags;
            //if (classID > 0)
            //{
            //    ProductClassInfo info = ProductClassBLL.ReadProductClassCache(classID);
            //    this.showCondition = "分类：<span>" + info.ClassName + "</span>";
            //    this.showTitle = info.ClassName;
            //    //showDescription = info.Description;
            //    this.searchType = 2;
            //    int iD = info.ID;
            //    if (ProductClassBLL.ReadProductClassChildList(info.ID).Count == 0)
            //    {
            //        iD = info.FatherID;
            //    }
            //    foreach (ProductClassInfo info2 in ProductClassBLL.ReadProductClassChildList(iD))
            //    {
            //        this.relationSearch = string.Concat(new object[] { relationSearch, "<a href=\"/Product-C", info2.ID, ".aspx\">", info2.ClassName, "</a>" });
            //    }
            //}
            //if (brandID > 0)
            //{
            //    ProductBrandInfo Info = ProductBrandBLL.ReadProductBrandCache(brandID);
            //    this.showCondition = "品牌：<span>" + Info.Name + "</span>";
            //    this.showTitle = Info.Name;
            //    //showDescription = Info.Description;
            //    this.searchType = 2;
            //    foreach (ProductBrandInfo info3 in base.topProductBrandList)
            //    {
            //        this.relationSearch = string.Concat(new object[] { relationSearch, "<a href=\"/Product-B", info3.ID, ".aspx\">", info3.Name, "</a>" });
            //    }
            //}
            //if (tags != string.Empty)
            //{
            //    this.showCondition = "标签：<span>" + tags + "</span>";
            //    this.showTitle = tags;
            //    this.searchType = 2;
            //    foreach (TagsInfo info4 in base.tagsList)
            //    {
            //        this.relationSearch = this.relationSearch + "<a href=\"/Product/Tags/" + base.Server.UrlEncode(info4.Word) + ".aspx\"  style=\"font-size:" + info4.Size.ToString() + "px; color:" + info4.Color + "\">" + info4.Word + "</a>";
            //    }
            //}
            //if (RequestHelper.GetQueryString<int>("IsNew") == 1)
            //{
            //    this.showCondition = "新品上市";
            //    this.showTitle = "新品上市";
            //    this.searchCondition = "IsNew=1";
            //}
            //if (RequestHelper.GetQueryString<int>("IsHot") == 1)
            //{
            //    this.showCondition = "热销商品";
            //    this.showTitle = "热销商品";
            //    this.searchCondition = "IsHot=1";
            //}
            //if (RequestHelper.GetQueryString<int>("IsSpecial") == 1)
            //{
            //    this.showCondition = "特价商品";
            //    this.showTitle = "特价商品";
            //    this.searchCondition = "IsSpecial=1";
            //}
            //if (RequestHelper.GetQueryString<int>("IsTop") == 1)
            //{
            //    this.showCondition = "推荐商品";
            //    this.showTitle = "推荐商品";
            //    this.searchCondition = "IsTop=1";
            //}
            //if (keyword != string.Empty)
            //{
            //    this.showCondition = this.showCondition + "关键字：<span>" + keyword + "</span>";
            //    this.showTitle = this.showTitle + keyword;
            //    this.searchType = 2;
            //    foreach (string str3 in ShopConfig.ReadConfigInfo().HotKeyword.Split(new char[] { ',' }))
            //    {
            //        this.relationSearch = this.relationSearch + "<a href=\"/Product/Keyword/" + base.Server.UrlEncode(str3) + ".aspx\">" + str3 + "</a>";
            //    }
            //}
            //if (this.searchType == 1)
            //{
            //    if (this.showCondition != string.Empty)
            //    {
            //        this.showCondition = "首页 > " + this.showCondition;
            //    }
            //    else
            //    {
            //        this.showCondition = "首页 > 全部商品";
            //        this.showTitle = "全部商品";
            //    }
            //}
            //else
            //{
            //    this.showCondition = "您搜索的" + this.showCondition;
            //}
            base.Title = this.showTitle + " - 商品展示";
        }

        protected string ShowCondition(string paramName, string paramText)
        {
            StringBuilder conditionHtml = new StringBuilder();
            conditionHtml.AppendLine("<li data-id=\"" + paramName + "\" class=\"selected\"><a href=\"?" + FilterSearchConditionUrl(paramName) + "\">" + paramText + "<i class=\"hui-icon hui-icon-close\"></i></a></li>");
            return conditionHtml.ToString();
        }

        protected string FilterSearchConditionUrl(string paramName)
        {
            string urlQuery = Request.Url.Query;
            if (urlQuery.StartsWith("?")) urlQuery = urlQuery.Substring(1);
            ArrayList paramArray = new ArrayList(urlQuery.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries));
            for (int i = 0; i < paramArray.Count; i++)
            {
                if (paramArray[i].ToString().Split('=')[0] == paramName)
                {
                    paramArray.Remove(paramArray[i]);
                }
            }
            //if (paramArray.Count > 0) paramArray.Insert(0, "");
            return string.Join("&", (string[])paramArray.ToArray(typeof(string)));
        }
    }

 

}
