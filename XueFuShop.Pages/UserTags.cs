using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class UserTags : UserBasePage
    {
        
        protected List<TagsInfo> tagsList = new List<TagsInfo>();

        
        protected override void PageLoad()
        {
            base.PageLoad();
            TagsSearchInfo tags = new TagsSearchInfo();
            tags.UserID = base.UserID;
            this.tagsList = TagsBLL.SearchTagsList(tags);
        }
    }

 

}
