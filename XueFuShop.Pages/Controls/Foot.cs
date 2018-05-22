using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace XueFuShop.Pages.Controls
{
    public class Foot : UserControl
    {
        
        protected List<ArticleInfo> bottomList = new List<ArticleInfo>();
        protected List<ArticleClassInfo> helpClassList = new List<ArticleClassInfo>();

        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.helpClassList = ArticleClassBLL.ReadArticleClassChildList(4);
            this.bottomList = ArticleBLL.ReadBottomList();
        }
    }
}
