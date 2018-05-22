using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.Pages;

namespace MobileEMS
{
    public partial class CartAjax : MobileUserBasePage
    {

        protected List<CartInfo> cartList = new List<CartInfo>();
        protected List<ProductInfo> productList = new List<ProductInfo>();
        protected int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            userID = base.UserID;
            this.ClearCache();
            string queryString = RequestHelper.GetQueryString<string>("Action");
            switch (queryString)
            {
                case "Read":
                    this.ReadCart();
                    break;
                case "ClearCart":
                    this.ClearCart();
                    break;
                case "ChangeBuyCount":
                    this.ChangeBuyCount();
                    break;
                case "Delete":
                    this.Delete();
                    break;
                case "AddToCart":
                    this.AddToCart();
                    break;
                case "OrderOperate":
                    this.OrderOperate();
                    break;
            }
        }

        protected void ClearCache()
        {
            base.Response.Cache.SetNoServerCaching();
            base.Response.Cache.SetNoStore();
            base.Response.Expires = 0;
        }

        private void ReadCart()
        {
            int ProductBuyCount = 0;
            decimal ProductTotalPrice = 0M;
            this.cartList = CartBLL.ReadCartList(userID);
            string strProductID = string.Empty;
            foreach (CartInfo info in this.cartList)
            {
                if (strProductID == string.Empty)
                {
                    strProductID = info.ProductID.ToString();
                }
                else
                {
                    strProductID = strProductID + "," + info.ProductID.ToString();
                }
            }
            //List<MemberPriceInfo> memberPriceList = MemberPriceBLL.ReadMemberPriceByProductGrade(strProductID, base.GradeID);
            foreach (CartInfo info in this.cartList)
            {
                ProductInfo product = ProductBLL.ReadProduct(info.ProductID);
                info.ProductPrice = product.MarketPrice;//MemberPriceBLL.ReadCurrentMemberPrice(memberPriceList, base.GradeID, TestCateModel);
                ProductBuyCount += 1;
                ProductTotalPrice += info.ProductPrice;
            }
            RecordList.DataSource = cartList;
            RecordList.DataBind();
            Sessions.ProductBuyCount = ProductBuyCount;
            Sessions.ProductTotalPrice = ProductTotalPrice;
        }


        private void Delete()
        {
            string strID = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("StrCartID"));
            int queryString = RequestHelper.GetQueryString<int>("OldCount");
            decimal num2 = RequestHelper.GetQueryString<decimal>("Price");
            int num3 = RequestHelper.GetQueryString<int>("ProductCount");
            decimal num4 = RequestHelper.GetQueryString<decimal>("ProductWeight");
            CartBLL.DeleteCart(strID, userID);
            Sessions.ProductBuyCount -= queryString * num3;
            Sessions.ProductTotalPrice -= queryString * num2;
            Sessions.ProductTotalWeight -= queryString * num4;
            ResponseHelper.Write(strID + "|" + Sessions.ProductBuyCount.ToString() + "|" + Sessions.ProductTotalPrice.ToString());
            ResponseHelper.End();
        }

        private void ClearCart()
        {
            CartBLL.ClearCart(userID);
            Sessions.ProductBuyCount = 0;
            Sessions.ProductTotalPrice = 0M;
            Sessions.ProductTotalWeight = 0M;
            ResponseHelper.End();
        }

        private void ChangeBuyCount()
        {
            string strID = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("StrCartID"));
            int queryString = RequestHelper.GetQueryString<int>("BuyCount");
            int num2 = RequestHelper.GetQueryString<int>("OldCount");
            decimal num3 = RequestHelper.GetQueryString<decimal>("Price");
            int num4 = RequestHelper.GetQueryString<int>("ProductCount");
            decimal num5 = RequestHelper.GetQueryString<decimal>("ProductWeight");
            CartBLL.UpdateCart(strID, queryString, userID);
            Sessions.ProductBuyCount += (queryString - num2) * num4;
            Sessions.ProductTotalPrice += (queryString - num2) * num3;
            //Sessions.ProductTotalWeight += (queryString - num2) * num5;
            string[] strArray = new string[] { strID, "|", Sessions.ProductBuyCount.ToString(), "|", Sessions.ProductTotalPrice.ToString(), "|", (queryString * num3).ToString(), "|", queryString.ToString() };
            ResponseHelper.Write(string.Concat(strArray));
            ResponseHelper.End();
        }


        protected void AddToCart()
        {
            string content = "ok";
            int queryString = RequestHelper.GetQueryString<int>("ProductID");
            string productName = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("ProductName"));
            int num2 = RequestHelper.GetQueryString<int>("BuyCount");
            decimal num3 = RequestHelper.GetQueryString<decimal>("CurrentMemberPrice");
            UserInfo userModel = UserBLL.ReadUser(base.UserID) ;
            if (!CartBLL.IsProductInCart(queryString, productName, userID))
            {
                CartInfo cart = new CartInfo();
                cart.ProductID = queryString;
                cart.ProductName = productName;
                cart.BuyCount = num2;
                cart.FatherID = 0;
                cart.RandNumber = string.Empty;
                cart.GiftPackID = 0;
                cart.UserID = userID;
                cart.ProductPrice = num3;
                cart.UserName = userModel.RealName;
                int num4 = CartBLL.AddCart(cart, userID);
                Sessions.ProductBuyCount += num2;
                Sessions.ProductTotalPrice += num2 * num3;                
            }
            else
            {
                content = "该产品已经在购物车";
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        private void OrderOperate()
        {
            string content = string.Empty;
            int queryString = RequestHelper.GetQueryString<int>("OrderID");
            int num2 = RequestHelper.GetQueryString<int>("OrderStatus");
            switch (num2)
            {
                case 1:
                case 2:
                case 5:
                    {
                        OrderInfo order = OrderBLL.ReadOrder(queryString, userID);
                        if (order.ID == 0)
                        {
                            content = "不是属于当前用户的订单";
                        }
                        else
                        {
                            int orderStatus;
                            if ((num2 == 2) || (num2 == 1))
                            {
                                orderStatus = order.OrderStatus;
                                order.OrderStatus = 3;
                               // ProductBLL.ChangeProductOrderCountByOrder(queryString, ChangeAction.Minus);
                                OrderBLL.UserUpdateOrderAddAction(order, "用户取消订单", 3, orderStatus);
                            }
                            else
                            {
                                //int point = OrderBLL.ReadOrderSendPoint(order.ID);
                                //if (point > 0)
                                //{
                                //    UserAccountRecordBLL.AddUserAccountRecord(0M, point, ShopLanguage.ReadLanguage("OrderReceived").Replace("$OrderNumber", order.OrderNumber), order.UserID, order.UserName);
                                //}
                                orderStatus = order.OrderStatus;
                                order.OrderStatus = 6;
                                OrderBLL.UserUpdateOrderAddAction(order, "用户确认收货", 5, orderStatus);
                            }
                        }
                        break;
                    }
                default:
                    content = "订单状态错误";
                    break;
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }
    }
}
