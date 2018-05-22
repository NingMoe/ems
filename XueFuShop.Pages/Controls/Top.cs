using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace XueFuShop.Pages.Controls
{
    public class Top : UserControl
    {
        
        protected List<ProductClassInfo> allProductClassList = new List<ProductClassInfo>();
        protected string hotKeyword = string.Empty;

        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.allProductClassList = ProductClassBLL.ReadProductClassNamedList();
            this.hotKeyword = ShopConfig.ReadConfigInfo().HotKeyword;
        }
    }
}
