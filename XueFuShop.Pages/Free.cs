using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Pages
{
    public class Free : CommonBasePage//UserCommonBasePage
    {
        protected List<ArticleInfo> newsList = new List<ArticleInfo>();
        protected List<string> certList = new List<string>();

        protected override void PageLoad()
        {
            base.PageLoad();            

            //每日快报
            ArticleSearchInfo articleSearch = new ArticleSearchInfo();
            articleSearch.ClassID = "|29|";
            this.newsList = ArticleBLL.SearchArticleList(1, 12, articleSearch, ref base.Count);

            //证书
            certList = PostPassBLL.GetTheLatestPostCert(10);
        }
    }
}
