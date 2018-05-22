using System;
using System.Collections.Generic;
using XueFuShop.Pages;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;


namespace XueFuShop.Admin
{
    public partial class UserAjax : AdminBasePage
    {
        protected List<UserInfo> userList = new List<UserInfo>();
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ClearCache();
            UserSearchInfo user = new UserSearchInfo();
            user.UserName = RequestHelper.GetQueryString<string>("UserName");
            this.userList = UserBLL.SearchUserList(base.CurrentPage, base.PageSize, user, ref this.Count);
            this.MyPager.CurrentPage = base.CurrentPage;
            this.MyPager.PageSize = base.PageSize;
            this.MyPager.Count = base.Count;
        }
    }
}
