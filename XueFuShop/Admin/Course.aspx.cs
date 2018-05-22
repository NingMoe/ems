using System;
using System.Data;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.Pages;
using System.Web.UI.WebControls;

namespace XueFuShop.Admin
{
    public partial class Course : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = RequestHelper.GetQueryString<int>("ID");
            string Action = RequestHelper.GetQueryString<string>("Action");
            if (Action == "Delete")
            {
                if (id != int.MinValue)
                {
                    base.CheckAdminPower("DeleteCourse", PowerCheckType.Single);
                    CourseBLL.DeleteCourse(id);
                    AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Course"), id);
                }
            }

            base.CheckAdminPower("ReadCourse", PowerCheckType.Single);
            if (!IsPostBack)
            {
                //在此处加载搜索框的类别信息
                CourseCateInfo CourseCateModel = new CourseCateInfo();
                CourseCateModel.Condition = CompanyBLL.SystemCompanyId.ToString();
                SearchCategory.DataSource = CourseCateBLL.ReadCourseCateNamedList(CourseCateModel);
                SearchCategory.DataTextField = "CateName";
                SearchCategory.DataValueField = "CateId";
                SearchCategory.DataBind();
                SearchCategory.Items.Insert(0, new ListItem("可以指定类别", "-1"));

                CourseInfo Model = new CourseInfo();
                Model.CourseName = RequestHelper.GetQueryString<string>("CourseName");
                Model.CateIdCondition = RequestHelper.GetQueryString<string>("CateIdCondition");
                Model.Condition = CompanyBLL.SystemCompanyId.ToString();
                Model.Field = "CompanyId";
                base.BindControl(CourseBLL.ReadList(Model, base.CurrentPage, base.PageSize, ref this.Count), this.RecordList, this.MyPager);
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("DeleteCourse", PowerCheckType.Single);
            string ID = RequestHelper.GetIntsForm("SelectID");
            string[] Arr = ID.Split(',');
            for (int i = 0; i < Arr.Length; i++)
            {
                CourseBLL.DeleteCourse(int.Parse(Arr[i]));
                QuestionBLL.DeleteQuestionByCateId(int.Parse(Arr[i]));
            }
            AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Course"), ID);
            ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if (SearchCategory.SelectedValue == "-1" && CourseName.Text == "") ScriptHelper.Alert("请选择搜索条件！");
            CourseInfo Model = new CourseInfo();
            Model.CourseName = CourseName.Text;
            if (SearchCategory.SelectedValue != "-1") Model.CateIdCondition = SearchCategory.SelectedValue.ToString() + "," + CourseCateBLL.ReadSonCourseCateCache(int.Parse(SearchCategory.SelectedValue));
            ResponseHelper.Redirect(("Course.aspx?Action=search&" + "CourseName=" + Model.CourseName + "&") + "CateIdCondition=" + Model.CateIdCondition);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("ReadQuestion", PowerCheckType.Single);
            string ID = RequestHelper.GetIntsForm("SelectID");
            
        }
    }
}
