using System;
using System.Data;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Pages;

namespace MobileEMS
{
    public partial class MCourseCenter : MobileUserBasePage
    {
        public int CateId = RequestHelper.GetQueryString<int>("CateId");
        public string Action = RequestHelper.GetQueryString<string>("Action");
        public int PostId = int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId"));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Action == "PostCourse") CenterTitle.Text = "我的课程";
            else CenterTitle.Text = "岗位计划";

            //if (CateId <= 0) CateId = PostId;

        }
    }
}
