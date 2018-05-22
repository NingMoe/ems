using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Pages
{
    public class AboutUs : CommonBasePage
    {
        protected ArticleInfo article = new ArticleInfo();
        protected int id = RequestHelper.GetQueryString<int>("ID");

        protected override void PageLoad()
        {
            base.PageLoad();
            if (id <= 0) id = 6;
            this.article = ArticleBLL.ReadArticle(id);

            base.Title = this.article.Title;
            base.Keywords = (this.article.Keywords == string.Empty) ? this.article.Title : this.article.Keywords;
            base.Description = (this.article.Summary == string.Empty) ? StringHelper.Substring(StringHelper.KillHTML(this.article.Content), 200) : this.article.Summary;
        }
    }
}
