using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class HelpDetail : CommonBasePage
    {
        protected ArticleInfo article = new ArticleInfo();

        protected override void PageLoad()
        {
            base.PageLoad();
            int id = RequestHelper.GetQueryString<int>("ID");
            article = ArticleBLL.ReadArticle(id);
            base.Title = article.Title;
        }
    }
}
