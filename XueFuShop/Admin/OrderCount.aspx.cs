using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XueFuShop.Pages;
using XueFuShop.Common;
using XueFu.EntLib;

namespace XueFuShop.Admin
{
    public partial class OrderCount : AdminBasePage
    {
        protected string queryString = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("StatisticsOrder", PowerCheckType.Single);
                ShopCommon.BindYearMonth(this.Year, this.Month);
                int queryString = RequestHelper.GetQueryString<int>("Year");
                queryString = (queryString == -2147483648) ? DateTime.Now.Year : queryString;
                int num2 = RequestHelper.GetQueryString<int>("Month");
                this.Year.Text = queryString.ToString();
                this.Month.Text = num2.ToString();
                this.queryString = "?Date=" + queryString.ToString() + "|" + num2.ToString();
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ResponseHelper.Redirect(("OrderCount.aspx?Action=search&" + "Year=" + this.Year.Text + "&") + "Month=" + this.Month.Text);
        }
    }
}
