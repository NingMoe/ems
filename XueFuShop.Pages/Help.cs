using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class Help : CommonBasePage
    {
        protected List<ArticleInfo> articleList = new List<ArticleInfo>();
        protected string searchKey = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("SearchKey"));
        protected int id = 0;

        protected override void PageLoad()
        {
            base.Title = "°ïÖúÖÐÐÄ";
            base.PageLoad();

            if (string.IsNullOrEmpty(searchKey))
            {
                this.id = RequestHelper.GetQueryString<int>("ID");
                if ((this.id == -2147483648) && (base.helpClassList.Count > 0))
                {
                    this.id = ArticleClassBLL.ReadArticleClassChildList(base.helpClassList[0].ID)[0].ID;
                }
                if (this.id > 0)
                {
                    ArticleSearchInfo articleSearch = new ArticleSearchInfo();
                    articleSearch.ClassID = "|" + this.id.ToString() + "|";
                    this.articleList = ArticleBLL.SearchArticleList(articleSearch);
                }
            }
            else
            {
                ArticleSearchInfo articleSearch = new ArticleSearchInfo();
                articleSearch.Condition = "([Keywords] like '%" + searchKey + "%' Or [Title] like '%" + searchKey + "%')";
                this.articleList = ArticleBLL.SearchArticleList(articleSearch);
            }
        }
    }
}
