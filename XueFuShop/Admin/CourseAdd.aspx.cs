using System;
using System.Data;
using System.Web.UI.WebControls;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Pages;
using XueFuShop.Models;
using XueFuShop.Common;

namespace XueFuShop.Admin
{
    public partial class CourseAdd : AdminBasePage
    {
        protected static int systemCompanyId = CompanyBLL.SystemCompanyId;
        protected int TempGroupId = CompanyBLL.ReadCompany(systemCompanyId).GroupId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                int queryString = RequestHelper.GetQueryString<int>("ID");
                CourseCateInfo CourseCateModel = new CourseCateInfo();
                CourseCateModel.Condition = systemCompanyId.ToString();
                this.CateId.DataSource = CourseCateBLL.ReadCourseCateNamedList(CourseCateModel);
                this.CateId.DataTextField = "CateName";
                this.CateId.DataValueField = "CateId";
                this.CateId.DataBind();
                this.CateId.Items.Insert(0, new ListItem("请选择类别", "-1"));

                if (queryString != int.MinValue)
                {
                    base.CheckAdminPower("UpdateCourse", PowerCheckType.Single);
                    CourseInfo CourseModel = CourseBLL.ReadCourse(queryString);
                    //if (CompanyId.Items.Contains(CompanyId.Items.FindByValue(CourseModel.CompanyId.ToString()))) CompanyId.Items.FindByValue(CourseModel.CompanyId.ToString()).Selected = true;
                    if (this.CateId.Items.Contains(this.CateId.Items.FindByValue(CourseModel.CateId.ToString()))) this.CateId.Items.FindByValue(CourseModel.CateId.ToString()).Selected = true;
                    this.CourseName.Text = CourseModel.CourseName;
                    //this.CourseCode.Text = CourseModel.CourseCode;
                    this.OrderIndex.Text = CourseModel.OrderIndex.ToString();
                }
                else
                {
                    base.CheckAdminPower("AddCourse", PowerCheckType.Single);
                }
            }
            //Control control = FindControlById(this, "CompanyId1");
            //if (control != null) ScriptHelper.Alert("找到了");

        }


        //private Control FindControlById(Control parent, String id)
        //{
        //    foreach (Control item in parent.Controls)
        //    {
        //        if (item.ID == id) return item;
        //        Control subControl = FindControlById(item, id);
        //        if (subControl != null) return subControl;
        //    }
        //    return null;
        //}


        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            int queryString = RequestHelper.GetQueryString<int>("ID");

            CourseInfo CourseModel = new CourseInfo();
            //if (_CompanyId > 0 && TempGroupId != 1 && TempGroupId != 2)
            //{
            CourseModel.CompanyId = systemCompanyId;
            //}
            //else
            //{
            //    if (CompanyId.SelectedValue == "-1") ScriptHelper.Alert("请选择公司");
            //    CourseModel.CompanyId = Convert.ToInt32(this.CompanyId.SelectedValue);
            //}
            if (this.CateId.SelectedValue == "-1") ScriptHelper.Alert("请选择类别");
            CourseModel.CourseId = queryString;
            CourseModel.CourseName = this.CourseName.Text;
            //CourseModel.CourseCode = this.CourseCode.Text;
            CourseModel.CateId = Convert.ToInt32(this.CateId.SelectedValue);
            CourseModel.OrderIndex = Convert.ToInt32(this.OrderIndex.Text);

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (CourseModel.CourseId == int.MinValue)
            {
                base.CheckAdminPower("AddCourse", PowerCheckType.Single);
                int id = CourseBLL.AddCourse(CourseModel);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Course"), id);
            }
            else
            {
                base.CheckAdminPower("UpdateCourse", PowerCheckType.Single);
                CourseBLL.UpdateCourse(CourseModel);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Course"), CourseModel.CourseId);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            AdminBasePage.Alert(alertMessage, RequestHelper.RawUrl);
        }

        //protected void CompanyId_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    this.CateId.DataSource = BLLCourseCate.ReadCourseCateNamedList();
        //    this.CateId.DataTextField = "CateName";
        //    this.CateId.DataValueField = "CateId";
        //    this.CateId.DataBind();
        //    ScriptHelper.Alert("aa");
        //    this.CateId.Items.Insert(0, new ListItem("请选择类别", "-1"));
        //}
    }
}
