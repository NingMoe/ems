using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using XueFuShop.Common;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.Pages;

namespace XueFuShop.Web
{
    public partial class ExchangeAward : PluginsBasePage
    {
        protected List<ProductInfo> productList = new List<ProductInfo>();
        protected ExchangeAwardInfo exchangeAward = new ExchangeAwardInfo();
        protected Dictionary<int, int> awardDic = new Dictionary<int, int>();
        protected int pointLeft = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            exchangeAward = ExchangeAwardBLL.ReadConfigInfo();
            if (exchangeAward.PorudctIDList != string.Empty)
            {
                string[] productArray = exchangeAward.PorudctIDList.Split(',');
                string[] pointArray = exchangeAward.PointList.Split(',');
                for (int i = 0; i < productArray.Length; i++)
                {
                    awardDic.Add(Convert.ToInt32(productArray[i]), Convert.ToInt32(pointArray[i]));
                }
                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.InProductID = exchangeAward.PorudctIDList;
                productList = ProductBLL.SearchProductList(productSearch);
            }
            pointLeft = UserBLL.ReadUserMore(Cookies.User.GetUserID(true)).PointLeft;
            Head.Title = "兑换奖品";
        }
    }
}
