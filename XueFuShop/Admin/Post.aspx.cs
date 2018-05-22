using System;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Pages;
using XueFuShop.Models;

namespace XueFuShop.Admin
{
    public partial class Post : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckAdminPower("ReadPost", PowerCheckType.Single); 
            string action = RequestHelper.GetQueryString<string>("Action");
            int id = RequestHelper.GetQueryString<int>("ID");
            if ((!string.IsNullOrEmpty(action)) && (id != -2147483648))
            {
                if (!string.IsNullOrEmpty(action))
                {
                    if (action == "Up")
                    {
                        base.CheckAdminPower("UpdatePostOrder", PowerCheckType.Single);
                        PostBLL.MoveUp(id);
                        AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("MoveRecord"), ShopLanguage.ReadLanguage("Post"), id);

                    }
                    else if (action == "Down")
                    {
                        base.CheckAdminPower("UpdatePostOrder", PowerCheckType.Single);
                        PostBLL.MoveDown(id);
                        AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("MoveRecord"), ShopLanguage.ReadLanguage("Post"), id);
                    }
                    else if (action == "Delete")
                    {
                        base.CheckAdminPower("DeletePost", PowerCheckType.Single);
                        PostBLL.DeletePost(id);
                        AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Post"), id);
                    }
                }
            }
            PostInfo Model = new PostInfo();
            Model.ParentId = 0;
            base.BindControl(PostBLL.ReadPostCateNamedList(), this.RecordList);
        }
    }
}
