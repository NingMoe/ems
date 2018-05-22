using System;
using System.Data;
using XueFuShop.Pages;
using XueFu.EntLib;

namespace MobileEMS
{
    public partial class Search : MobileUserBasePage
    {
        public string Action = RequestHelper.GetQueryString<string>("Action");
        protected void Page_Load(object sender, EventArgs e)
        {
            CenterTitle.Text = "¿Î³ÌËÑË÷";
        }
    }
}
