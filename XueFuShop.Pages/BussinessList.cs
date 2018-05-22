using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class BussinessList : CommonBasePage
    {
        protected List<ArticleInfo> articleList = new List<ArticleInfo>();
        protected List<ArticleClassInfo> articleClassList = new List<ArticleClassInfo>();
        protected string searchKey = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("SearchKey"));
        protected int id = 0;
        protected int classID = RequestHelper.GetQueryString<int>("ClassID");
        protected string title = string.Empty;

        protected override void PageLoad()
        {
            base.PageLoad();

            articleClassList = ArticleClassBLL.ReadArticleClassChildList(classID);
            if (string.IsNullOrEmpty(searchKey))
            {
                this.id = RequestHelper.GetQueryString<int>("ID");
                if (articleClassList.Count > 0)
                {
                    if (this.id <= 0)
                    {
                        this.id = articleClassList[0].ID;
                        title = base.Title = articleClassList[0].ClassName;
                    }
                    else
                    {
                        ArticleClassInfo article = articleClassList.Find(delegate(ArticleClassInfo articleClass) { return articleClass.ID == this.id; });
                        if (article != null)
                            title = base.Title = article.ClassName;
                    }
                }
            }
        }
    }
}
