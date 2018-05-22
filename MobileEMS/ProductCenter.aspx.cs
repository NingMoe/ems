using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XueFu.EntLib;
using XueFuShop.Pages;

namespace MobileEMS
{
    public partial class ProductCenter : MobileUserBasePage
    {
        public int CateId = RequestHelper.GetQueryString<int>("CateId");
        public string Action = RequestHelper.GetQueryString<string>("Action");
        public int PostId = int.Parse(CookiesHelper.ReadCookieValue("UserStudyPostId"));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Action == "PostCourse") CenterTitle.Text = "我的课程";
            else CenterTitle.Text = "岗位计划";
        }
    }
}
