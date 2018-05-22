using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XueFu.EntLib;

namespace MobileEMS
{
    public partial class RCDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CenterTitle.Text = RequestHelper.GetQueryString<string>("CourseName");
        }
    }
}