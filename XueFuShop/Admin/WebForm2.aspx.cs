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
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;

namespace XueFuShop.Admin
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ResponseHelper.Write("2222222222");
            int count = 0;
            TeacherInfo teacher = new TeacherInfo();
            teacher.Name = "test";
            TeacherBLL teacherBLL = new TeacherBLL();
            //teacherBLL.ReadList(teacher, 1, 20, ref count);
            teacherBLL.Update(teacher);

            //BaseBLL<TeacherInfo> baseBLL = new BaseBLL<TeacherInfo>();
            //baseBLL.Add(teacher);
        }
    }
}
