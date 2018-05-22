using System;
using System.Web.UI.WebControls;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Pages;
using XueFuShop.Models;

namespace XueFuShop.Admin
{
    public partial class CourseMove : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                base.CheckAdminPower("UpdateCourse", PowerCheckType.Single);
                CourseCateInfo CourseCateModel = new CourseCateInfo();
                CourseCateModel.Condition = CompanyBLL.SystemCompanyId.ToString();
                this.CateId.DataSource = CourseCateBLL.ReadCourseCateNamedList(CourseCateModel);
                this.CateId.DataTextField = "CateName";
                this.CateId.DataValueField = "CateId";
                this.CateId.DataBind();
                this.CateId.Items.Insert(0, new ListItem("请选择类别", "-1"));
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("UpdateCourse", PowerCheckType.Single);
            string courseID = RequestHelper.GetQueryString<string>("CourseID");
            if (this.CateId.SelectedValue == "-1")
                ScriptHelper.Alert("请选择类别");

            CourseBLL.UpdateCourse(courseID, Convert.ToInt32(this.CateId.SelectedValue));
            AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Course"), courseID);
            AdminBasePage.Alert(ShopLanguage.ReadLanguage("UpdateOK"), RequestHelper.RawUrl);
        }
    }
}
