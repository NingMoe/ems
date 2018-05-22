using System;
using XueFuShop.Common;

namespace XueFuShop.Admin
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IconFontPath.Href = ShopConfig.ReadConfigInfo().ManageIconFontPath;
        }
    }
}
