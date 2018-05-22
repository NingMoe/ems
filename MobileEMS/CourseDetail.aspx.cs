using System;
using System.Data;
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace MobileEMS
{
    public partial class CourseDetail : MobileUserBasePage
    {
        protected int CateId = RequestHelper.GetQueryString<int>("CateId");
        protected string CateImg = string.Empty;
        protected string TestCateName = string.Empty;
        protected ProductInfo product = new ProductInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            product = ProductBLL.ReadProduct(CateId);
            this.CenterTitle.Text = "ฟฮณฬฯ๊ว้";
        }
    }
}
