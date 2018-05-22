using System;
using System.Collections.Generic;
using MobileEMS.Models;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Pages;

namespace MobileEMS
{
    public partial class OrderDetail : UserCommonBasePage
    {
        protected OrderInfo order = new OrderInfo();
        protected List<OrderDetailInfo> orderDetailList = new List<OrderDetailInfo>();
        protected List<ProductInfo> TestCateList = new List<ProductInfo>();
        protected int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            CenterTitle.Text = "∂©µ•œÍ«È";
            userID = base.UserID;
            int queryString = RequestHelper.GetQueryString<int>("ID");
            this.order = OrderBLL.ReadOrder(queryString, userID);
            RecordList.DataSource = OrderDetailBLL.ReadOrderDetailByOrder(queryString);
            RecordList.DataBind();
        }
    }
}
