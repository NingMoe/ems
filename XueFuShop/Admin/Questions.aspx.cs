using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class Questions : AdminBasePage
    {
        protected int systemCompanyId = CompanyBLL.SystemCompanyId;
        protected bool updateQuestionPower = false;
        protected bool deleteQuestionPower = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = RequestHelper.GetQueryString<int>("ID");
            string Action = RequestHelper.GetQueryString<string>("Action");
            if (Action == "Delete")
            {
                if (id != int.MinValue)
                {
                    base.CheckAdminPower("DeleteQuestion", PowerCheckType.Single);
                    QuestionBLL.DeleteQuestion(id);
                    AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Question"), id);
                }
            }
            if (!IsPostBack)
            {
                base.CheckAdminPower("ReadQuestion", PowerCheckType.Single);

                updateQuestionPower = base.CompareAdminPower("UpdateQuestion", PowerCheckType.Single);
                deleteQuestionPower = base.CompareAdminPower("DeleteQuestion", PowerCheckType.Single);
                //在此处加载搜索框的信息
                //题型下拉框
                SearchQuestionStyle.DataSource = EnumHelper.ReadEnumList<QuestionType>();
                SearchQuestionStyle.DataValueField = "Value";
                SearchQuestionStyle.DataTextField = "ChineseName";
                SearchQuestionStyle.DataBind();
                SearchQuestionStyle.Items.Insert(0, new ListItem("选择题型", string.Empty));

                CourseCateInfo CourseCateModel = new CourseCateInfo();
                CourseCateModel.Condition = systemCompanyId.ToString();
                SearchCategory.DataSource = CourseCateBLL.ReadCourseCateNamedList(CourseCateModel);
                SearchCategory.DataTextField = "CateName";
                SearchCategory.DataValueField = "CateId";
                SearchCategory.DataBind();
                SearchCategory.Items.Insert(0, new ListItem("可以指定类别", int.MinValue.ToString()));

                QuestionInfo Model = new QuestionInfo();
                Model.Style = RequestHelper.GetQueryString<string>("Style");
                Model.Question = RequestHelper.GetQueryString<string>("QuestionName");
                Model.IdCondition = RequestHelper.GetQueryString<string>("IdCondition");
                Model.Field = "CompanyId";
                Model.Condition = systemCompanyId.ToString();
                base.BindControl(QuestionBLL.ReadList(Model, base.CurrentPage, base.PageSize, ref this.Count), this.RecordList, this.MyPager);//

                SearchQuestionName.Text = Model.Question;
                SearchQuestionStyle.SelectedValue = Model.Style;
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("DeleteQuestion", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                QuestionBLL.DeleteQuestion(intsForm);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Question"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            //定义一个课程Id变量，为后续题目的相关搜索条件提供前提搜索条件
            string TempStr = string.Empty;

            if (SearchCategory.SelectedValue == int.MinValue.ToString() && SearchCourseName.Text == "" && SearchQuestionName.Text == "" && SearchQuestionStyle.SelectedValue == "") ScriptHelper.Alert("请填写搜索条件！");

            CourseInfo CourseModel = new CourseInfo();
            if (SearchCategory.SelectedValue != int.MinValue.ToString()) CourseModel.CateId = int.Parse(SearchCategory.SelectedValue);
            if (!string.IsNullOrEmpty(SearchCourseName.Text)) CourseModel.CourseName = SearchCourseName.Text;
            List<CourseInfo> TempList = CourseBLL.ReadList(CourseModel);
            if (TempList != null)
            {
                foreach (CourseInfo Item in TempList)
                {
                    TempStr = TempStr + "," + Item.CourseId;
                }
                if (TempStr.StartsWith(",")) TempStr = TempStr.Substring(1);
            }
           ResponseHelper.Redirect("Questions.aspx?Action=search&QuestionName=" + Server.UrlEncode(SearchQuestionName.Text) + "&IdCondition=" + TempStr + "&Style=" + SearchQuestionStyle.SelectedValue);
        }
    }
}
