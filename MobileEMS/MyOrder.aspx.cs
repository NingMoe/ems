using System;
using XueFuShop.Pages;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace MobileEMS
{
    public partial class MyOrder : MobileUserBasePage
    {
        public string Action = RequestHelper.GetQueryString<string>("Action");
        int UserId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            UserId = base.UserID;
            CenterTitle.Text = "ÎÒµÄ¶©µ¥";
            int queryString = RequestHelper.GetQueryString<int>("Page");
            if (queryString < 1)
            {
                queryString = 1;
            }
            OrderSearchInfo order = new OrderSearchInfo();
            order.UserID = UserId;
            RecordList.DataSource = OrderBLL.SearchOrderList(queryString, base.PageSize, order, ref this.Count);
            RecordList.DataBind();
            
        }
    }
}
