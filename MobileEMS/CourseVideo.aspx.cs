using System;
using System.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using XueFuShop.Pages;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using MobileEMS.BLL;

namespace MobileEMS
{
    public partial class CourseVideo : MobileUserBasePage
    {
        protected int CateId = RequestHelper.GetQueryString<int>("CateId");
        protected int VideoListId = RequestHelper.GetQueryString<int>("VideoListId");
        protected ProductInfo product = new ProductInfo();
        protected string checkCode = string.Empty;
        protected int userID = RequestHelper.GetQueryString<int>("UserID");
        protected Dictionary<string, string> videoDic = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (VideoListId < 0) VideoListId = 0;
            product = ProductBLL.ReadProduct(CateId);
            videoDic = PolyvBLL.GetVideoDic(product.ProductNumber.Split('|')[0]);
            checkCode = StringHelper.Decode(CookiesHelper.ReadCookieValue("SMSCheckCode"), "SMS");

            if (userID <= 0)
                userID = base.UserID;
        }
    }
}
