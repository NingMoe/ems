using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class UserApplyAjax : UserAjaxBasePage
    {
        
        protected string action = string.Empty;
        protected AjaxPagerClass ajaxPagerClass = new AjaxPagerClass();
        protected decimal moneyLeft = 0M;
        protected List<UserApplyInfo> userApplyList = new List<UserApplyInfo>();

        
        protected void AddUserApply()
        {
            string content = string.Empty;
            decimal queryString = RequestHelper.GetQueryString<decimal>("Money");
            string str2 = StringHelper.AddSafe(RequestHelper.GetQueryString<string>("UserNote"));
            if ((queryString <= 0M) || (str2 == string.Empty))
            {
                content = "����д���ͱ�ע";
            }
            else if (UserBLL.ReadUserMore(base.UserID).MoneyLeft < queryString)
            {
                content = "���ֽ�����ʣ����";
            }
            else
            {
                UserApplyInfo userApply = new UserApplyInfo();
                Random random = new Random();
                userApply.Number = RequestHelper.DateNow.ToString("yyMMddhh") + random.Next(0x3e8, 0x270f);
                userApply.Money = queryString;
                userApply.UserNote = str2;
                userApply.Status = 1;
                userApply.ApplyDate = RequestHelper.DateNow;
                userApply.ApplyIP = ClientHelper.IP;
                userApply.AdminNote = string.Empty;
                userApply.UpdateDate = RequestHelper.DateNow;
                userApply.UpdateAdminID = 0;
                userApply.UpdateAdminName = string.Empty;
                userApply.UserID = base.UserID;
                userApply.UserName = base.UserName;
                UserApplyBLL.AddUserApply(userApply);
            }
            ResponseHelper.Write(content);
            ResponseHelper.End();
        }

        protected override void PageLoad()
        {
            base.PageLoad();
            this.action = RequestHelper.GetQueryString<string>("Action");
            int queryString = RequestHelper.GetQueryString<int>("Page");
            if (queryString < 1)
            {
                queryString = 1;
            }
            int pageSize = 20;
            int count = 0;
            string action = this.action;
            if (action != null)
            {
                if (!(action == "Read"))
                {
                    if (action == "Add")
                    {
                        this.moneyLeft = UserBLL.ReadUserMore(base.UserID).MoneyLeft;
                    }
                    else if (action == "AddUserApply")
                    {
                        this.AddUserApply();
                    }
                }
                else
                {
                    UserApplySearchInfo userApply = new UserApplySearchInfo();
                    userApply.UserID = base.UserID;
                    this.userApplyList = UserApplyBLL.SearchUserApplyList(queryString, pageSize, userApply, ref count);
                    this.ajaxPagerClass.CurrentPage = queryString;
                    this.ajaxPagerClass.PageSize = pageSize;
                    this.ajaxPagerClass.Count = count;
                }
            }
        }
    }

 

}
