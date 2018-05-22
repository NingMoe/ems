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

namespace MobileEMS
{
    public partial class PrePaidCourse : System.Web.UI.Page
    {
        public int CateId = RequestHelper.GetQueryString<int>("CateId");
        public string Action = RequestHelper.GetQueryString<string>("Action");

        protected void Page_Load(object sender, EventArgs e)
        {
            CenterTitle.Text = "已购有效课程";
        }
    }
}
