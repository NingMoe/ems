using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Pages
{
    public class Verify : BasePage
    {
        protected string vu = RequestHelper.GetQueryString<string>("vu");
        protected int productID = RequestHelper.GetQueryString<int>("ID");
        protected string checkCode = string.Empty;
        protected string mobile = RequestHelper.GetQueryString<string>("Mobile");
        protected int userID = RequestHelper.GetQueryString<int>("UserID");
        protected Dictionary<string, string> videoDic = new Dictionary<string, string>();

        protected override void PageLoad()
        {
            //从Cookies中读取验证码并解密
            if (StringHelper.Decode(CookiesHelper.ReadCookieValue("SMSIsChecked"), "SMS") == "true")
                checkCode = StringHelper.Decode(CookiesHelper.ReadCookieValue("SMSCheckCode"), "SMS");

            if (string.IsNullOrEmpty(mobile))
                mobile = base.UserMobile;

            if (userID <= 0)
                userID = base.UserID;

            ProductInfo product = ProductBLL.ReadProduct(productID);
            if (product != null)
            {
                vu = product.ProductNumber;
                videoDic = PolyvBLL.GetVideoDic(product.ProductNumber.Split('|')[0]);
            }
        }
    }
}
