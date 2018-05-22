using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace MobileEMS.Pay.AliPay
{
    public partial class Return : System.Web.UI.Page
    {   
        protected string message = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sPara = GetRequestGet();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                AlipayNotify aliNotify = new AlipayNotify();
                bool verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);

                if (verifyResult)//验证成功
                {
                    if (Request.QueryString["trade_status"] == "TRADE_FINISHED" || Request.QueryString["trade_status"] == "TRADE_SUCCESS")
                    {
                        //更新数据库的订单语句
                        message = "成功付款";
                        string orderNumber = RequestHelper.GetQueryString<string>("out_trade_no");
                        OrderInfo order = OrderBLL.ReadOrderByNumber(orderNumber, 0);
                        if (order.ID > 0)
                        {
                            if (order.OrderStatus == (int)OrderStatus.WaitPay)
                            {
                                order.OrderStatus = (int)OrderStatus.WaitCheck;
                                OrderBLL.UpdateOrder(order);
                                //增加操作记录
                                OrderActionInfo orderAction = new OrderActionInfo();
                                orderAction.OrderID = order.ID;
                                orderAction.OrderOperate = (int)OrderOperate.Pay;
                                orderAction.StartOrderStatus = (int)OrderStatus.WaitPay;
                                orderAction.EndOrderStatus = (int)OrderStatus.WaitCheck;
                                orderAction.Note = "客户支付宝在线支付";
                                orderAction.IP = ClientHelper.IP;
                                orderAction.Date = RequestHelper.DateNow;
                                orderAction.AdminID = 0;
                                orderAction.AdminName = string.Empty;
                                OrderActionBLL.AddOrderAction(orderAction);

                                message = "您已经成功支付订单：" + orderNumber;
                            }
                        }
                        //else
                        //{
                        //    UserRechargeInfo userRecharge = UserRechargeBLL.ReadUserRechargeByNumber(orderNumber, 0);
                        //    if (userRecharge.ID > 0 && userRecharge.IsFinish == (int)BoolType.False)
                        //    {
                        //        userRecharge.IsFinish = (int)BoolType.True;
                        //        UserRechargeBLL.UpdateUserRecharge(userRecharge);
                        //        //账户记录
                        //        string note = "支付宝在线冲值：" + userRecharge.Number;
                        //        UserAccountRecordBLL.AddUserAccountRecord(userRecharge.Money, 0, note, userRecharge.UserID, userRecharge.UserName);

                        //        message = "您的冲值已经成功完成";
                        //    }
                        //}
                        ResponseHelper.Redirect("PrePaidCourse.aspx");
                    }
                    else
                    {
                        Response.Write("trade_status=" + Request.QueryString["trade_status"]);
                    }
                }
                else//验证失败
                {
                    Response.Write("验证失败");
                }
            }
            else
            {
                Response.Write("无返回参数");
            }
        }


        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }
    }
}
