using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFu.EntLib;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class UserInfoShow : UserCommonBasePage
    {
        protected string companyName = string.Empty;
        protected string department = string.Empty;
        protected string workingPostName = string.Empty;
        protected string studyPostName = string.Empty;

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "查看个人资料";

            companyName = CompanyBLL.ReadCompany(User.CompanyID).CompanyName;
            department = PostBLL.ReadPost(User.Department).PostName;
            workingPostName = User.PostName;
            studyPostName = PostBLL.ReadPost(User.StudyPostID).PostName;
            if (string.IsNullOrEmpty(workingPostName))
            {
                if (User.WorkingPostID == User.StudyPostID)
                {
                    workingPostName = studyPostName;
                }
                else
                {
                    workingPostName = PostBLL.ReadPost(User.WorkingPostID).PostName;
                }
            }
        }

        protected override void PostBack()
        {
            base.User = UserBLL.ReadUser(base.UserID);
            User.Email = StringHelper.AddSafe(RequestHelper.GetForm<string>("Email"));
            //User.Mobile = StringHelper.AddSafe(RequestHelper.GetForm<string>("Mobile"));
            //if (string.IsNullOrEmpty(User.Mobile)) ScriptHelper.Alert("请输入手机号码");
            User.Tel = StringHelper.AddSafe(RequestHelper.GetForm<string>("Tel"));
            User.PostName = StringHelper.AddSafe(RequestHelper.GetForm<string>("WorkingPostName"));
            int sex = RequestHelper.GetForm<int>("Sex");
            if (sex > 0)
            {
                User.Sex = sex;
            }

            UserBLL.UpdateUser(User);
            ScriptHelper.Alert(ShopLanguage.ReadLanguage("UpdateOK"), RequestHelper.RawUrl);
        }
    }
}
