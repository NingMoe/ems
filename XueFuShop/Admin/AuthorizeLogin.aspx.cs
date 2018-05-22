using System;
using XueFuShop.Pages;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace XueFuShop.Admin
{
    public partial class AuthorizeLogin : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckAdminPower("AuthorizeLogin", PowerCheckType.Single);
            int userID = RequestHelper.GetQueryString<int>("UserID");
            if (userID > 0)
            {
                UserInfo user = UserBLL.ReadUser(userID);
                UserBLL.UserLoginInit(user);
                if (user.GroupID == 36)
                    ResponseHelper.Redirect("/User/CourseCenter.aspx");
                else
                    ResponseHelper.Redirect("/User/UserList.aspx");
            }
        }
    }
}
