using System;
using System.Data;
using XueFuShop.Pages;
using XueFuShop.BLL;

namespace XueFuShop.Admin
{
    public partial class Right : AdminBasePage
    {
        protected DataTable dt = new DataTable();
        protected string queryString = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.queryString = "?Date=" + DateTime.Now.Year.ToString() + "|" + DateTime.Now.Month.ToString();
            this.dt = ProductBLL.NoHandlerStatistics();
        }
    }
}
