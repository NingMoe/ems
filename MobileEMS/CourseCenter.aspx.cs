using System;
using XueFu.EntLib;
using XueFuShop.Pages;

namespace MobileEMS
{
    public partial class CourseCenter : MobileUserBasePage
    {
        public int CateId = RequestHelper.GetQueryString<int>("CateId");
        public string Action = RequestHelper.GetQueryString<string>("Action");
        public int PostId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            PostId = int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId"));
            if (string.IsNullOrEmpty(Action)) Action = "PostCourse";
            if (Action == "PostCourse") CenterTitle.Text = "我的课程";
            else CenterTitle.Text = "岗位计划";
        }
    }
}
