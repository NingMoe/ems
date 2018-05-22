using System;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFuShop.Pages;
using XueFuShop.Common;
using System.Web.UI.WebControls;

namespace XueFuShop.Admin
{
    public partial class CourseCateAdd : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CourseCateInfo CourseCateModel = new CourseCateInfo();
                CourseCateModel.Condition = CompanyBLL.SystemCompanyId.ToString();
                this.FatherID.DataSource = CourseCateBLL.ReadCourseCateNamedList(CourseCateModel);
                this.FatherID.DataTextField = "CateName";
                this.FatherID.DataValueField = "CateId";
                this.FatherID.DataBind();
                this.FatherID.Items.Insert(0, new ListItem("作为最大类", "0"));

                int queryString = RequestHelper.GetQueryString<int>("ID");

                if (queryString != -2147483648)
                {
                    base.CheckAdminPower("ReadCourseCate", PowerCheckType.Single);
                    CourseCateInfo info = CourseCateBLL.ReadCourseCateCache(queryString);
                    this.FatherID.Text = info.ParentCateId.ToString();
                    this.OrderID.Text = info.OrderIndex.ToString();
                    this.ClassName.Text = info.CateName;
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            CourseCateInfo CourseClass = new CourseCateInfo();
            CourseClass.CateId = RequestHelper.GetQueryString<int>("ID");
            CourseClass.ParentCateId = Convert.ToInt32(this.FatherID.Text);
            CourseClass.OrderIndex = Convert.ToInt32(this.OrderID.Text);
            CourseClass.CateName = this.ClassName.Text;
            CourseClass.CompanyId = CompanyBLL.SystemCompanyId;
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (CourseClass.CateId == -2147483648)
            {
                base.CheckAdminPower("AddCourseCate", PowerCheckType.Single);
                int id = CourseCateBLL.AddCourseCate(CourseClass);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("CourseCate"), id);
            }
            else
            {
                base.CheckAdminPower("UpdateCourseCate", PowerCheckType.Single);
                CourseCateBLL.UpdateCourseCate(CourseClass);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("CourseCate"), CourseClass.CateId);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            AdminBasePage.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}
