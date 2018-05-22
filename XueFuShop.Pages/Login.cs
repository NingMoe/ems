using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class Login : CommonBasePage
    {

        protected string redirectUrl = string.Empty;
        protected string result = string.Empty;


        protected override void PageLoad()
        {
            base.PageLoad();
            this.result = RequestHelper.GetQueryString<string>("Message");
            this.redirectUrl = RequestHelper.GetQueryString<string>("RedirectUrl");
        }

        protected override void PostBack()
        {
            string str4;
            this.redirectUrl = RequestHelper.GetForm<string>("RedirectUrl");
            string loginName = StringHelper.SearchSafe(RequestHelper.GetForm<string>("UserName"));
            string loginPass = StringHelper.Password(RequestHelper.GetForm<string>("UserPassword"), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            if (!(RequestHelper.GetForm<string>("SafeCode").ToLower() == Cookies.Common.checkcode.ToLower()))
            {
                this.result = "验证码错误或失效";
            }
            else
            {
                UserInfo user = UserBLL.CheckUserLogin(loginName, loginPass);
                if (user.ID <= 0)
                {
                    this.result = "用户名或者密码错误";
                }
                else
                {
                    switch (user.Status)
                    {
                        case (int)UserState.NoCheck:
                            this.result = "该用户未激活";
                            goto Label_0142;

                        case (int)UserState.Other:
                        case (int)UserState.Free:
                        case (int)UserState.Normal:
                            user = UserBLL.ReadUserMore(user.ID);
                            //验证公司状态
                            CompanyInfo currentCompany = CompanyBLL.ReadCompany(user.CompanyID);
                            if (currentCompany.State == 1)
                            {
                                this.result = "贵公司已被锁定，不能登陆！";
                                goto Label_0142;
                            }
                            if (!string.IsNullOrEmpty(currentCompany.EndDate.ToString()) && DateTime.Today > Convert.ToDateTime(currentCompany.EndDate))
                            {
                                this.result = "贵公司使用期限已到！";
                                goto Label_0142;
                            }
                            UserBLL.UserLoginInit(user);
                            UserBLL.UpdateUserLogin(user.ID, RequestHelper.DateNow, ClientHelper.IP);
                            if (string.IsNullOrEmpty(this.redirectUrl))
                            {
                                if (user.GroupID == 36)
                                {
                                    //if (TestSettingBLL.IsTestAgain(user.ID, user.StudyPostID))
                                    //    ResponseHelper.Redirect("/User/CourseCenter.aspx?PC=1&View=Grid");
                                    //else
                                    ResponseHelper.Redirect("/Bussiness.aspx");
                                }
                                else
                                    ResponseHelper.Redirect("/User/UserList.aspx");
                            }
                            else
                            {
                                ResponseHelper.Redirect(this.redirectUrl);
                            }
                            goto Label_0142;

                        case (int)UserState.Frozen:
                            this.result = "该用户已冻结";
                            goto Label_0142;
                    }
                }
            }
        Label_0142:
            str4 = "/User/Login.aspx?Message=" + this.result;
            if (!string.IsNullOrEmpty(this.redirectUrl))
            {
                str4 = str4 + "&RedirectUrl=" + this.redirectUrl;
            }
            ResponseHelper.Redirect(str4);
        }
    }

 

}
