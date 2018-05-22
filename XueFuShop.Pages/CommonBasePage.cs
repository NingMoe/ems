using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFu.EntLib;

namespace XueFuShop.Pages
{
    public abstract class CommonBasePage : BasePage
    {
        protected List<ProductClassInfo> allProductClassList = new List<ProductClassInfo>();
        protected List<ArticleInfo> bottomList = new List<ArticleInfo>();
        protected List<ArticleClassInfo> helpClassList = new List<ArticleClassInfo>();
        protected string hotKeyword = string.Empty;
        protected List<ProductBrandInfo> productBrandList = new List<ProductBrandInfo>();
        protected List<ProductClassInfo> productClassList = new List<ProductClassInfo>();
        protected List<TagsInfo> tagsList = new List<TagsInfo>();
        protected List<ProductBrandInfo> topProductBrandList = new List<ProductBrandInfo>();
        protected int Count = 0;
        protected int PageSize = 20;
        protected string ParentCompanyID = CookiesHelper.ReadCookieValue("UserCompanyParentCompanyID");

        protected CommonBasePage()
        {
        }

        protected override void PageLoad()
        {
            this.helpClassList = ArticleClassBLL.ReadArticleClassChildList(4);
            this.productClassList = ProductClassBLL.ReadProductClassRootList();
            //this.allProductClassList = ProductClassBLL.ReadProductClassNamedList();
            this.topProductBrandList = ProductBrandBLL.ReadProductBrandIsTopCacheList();
            this.productBrandList = ProductBrandBLL.ReadProductBrandCacheList();
            this.hotKeyword = ShopConfig.ReadConfigInfo().HotKeyword;
            this.bottomList = ArticleBLL.ReadBottomList();
            //this.tagsList = TagsBLL.ReadHotTagsList();
        }
    }
}
