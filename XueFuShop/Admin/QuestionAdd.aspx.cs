using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace XueFuShop.Admin
{
    public partial class QuestionAdd : AdminBasePage
    {
        protected int systemCompanyId = CompanyBLL.SystemCompanyId;

        protected void Page_Load(object sender, EventArgs e)
        {
            Control singleTest = FindControlById(this, "singleTest");
            Control MultiTest = FindControlById(this, "MultiTest");
            Control PanDuanTest = FindControlById(this, "PanDuanTest");
            if (!IsPostBack)
            {
                int queryString = RequestHelper.GetQueryString<int>("ID");
                int CourseId = RequestHelper.GetQueryString<int>("CourseId");
                int cateid = RequestHelper.GetQueryString<int>("CateId");

                CourseCateInfo CourseCateModel = new CourseCateInfo();
                CourseCateModel.Condition = systemCompanyId.ToString();
                this._CateId.DataSource = CourseCateBLL.ReadCourseCateNamedList(CourseCateModel);
                this._CateId.DataTextField = "CateName";
                this._CateId.DataValueField = "CateId";
                this._CateId.DataBind();
                this._CateId.Items.Insert(0, new ListItem("请选择类别", "-1"));
                _CateId.SelectedValue = cateid.ToString();

                if (CourseId != int.MinValue)
                {
                    CourseInfo CourseModel = new CourseInfo();
                    CourseModel.CateId = cateid;
                    this._CourseId.DataSource = CourseBLL.ReadList(CourseModel);
                    this._CourseId.DataTextField = "CourseName";
                    this._CourseId.DataValueField = "CourseId";
                    this._CourseId.DataBind();
                    _CourseId.SelectedValue = CourseId.ToString();
                }

                if (queryString != int.MinValue)
                {
                    base.CheckAdminPower("ReadQuestion", PowerCheckType.Single);
                    QuestionInfo QuestionModel = QuestionBLL.ReadQuestion(queryString);
                    int CateId = CourseBLL.ReadCourse(QuestionModel.CateId).CateId;
                    _CateId.SelectedValue = CateId.ToString();

                    CourseInfo CourseModel = new CourseInfo();
                    CourseModel.CateId = CateId;
                    this._CourseId.DataSource = CourseBLL.ReadList(CourseModel);
                    this._CourseId.DataTextField = "CourseName";
                    this._CourseId.DataValueField = "CourseId";
                    this._CourseId.DataBind();
                    _CourseId.SelectedValue = QuestionModel.CateId.ToString();

                    this.Quetion.Text = QuestionModel.Question;
                    this.TestType.Text = QuestionModel.Style;
                    if (QuestionModel.Style == "1")
                    {
                        this.SingleA.Text = QuestionModel.A;
                        this.SingleB.Text = QuestionModel.B;
                        this.SingleC.Text = QuestionModel.C;
                        this.SingleD.Text = QuestionModel.D;
                        switch (QuestionModel.Answer.ToUpper())
                        {
                            case "A":
                                this.SingleAnswerA.Checked = true;
                                break;
                            case "B":
                                this.SingleAnswerB.Checked = true;
                                break;
                            case "C":
                                this.SingleAnswerC.Checked = true;
                                break;
                            case "D":
                                this.SingleAnswerD.Checked = true;
                                break;
                        }
                        this.singleTest.Style.Add("display", "");
                        this.MultiTest.Style.Add("display", "none");
                        this.PanDuanTest.Style.Add("display", "none");
                    }
                    else if (QuestionModel.Style == "2")
                    {
                        this.MultiA.Text = QuestionModel.A;
                        this.MultiB.Text = QuestionModel.B;
                        this.MultiC.Text = QuestionModel.C;
                        this.MultiD.Text = QuestionModel.D;
                        for (int i = 0; i < QuestionModel.Answer.Length; i++)
                        {
                            switch (QuestionModel.Answer.ToUpper().Substring(i, 1))
                            {
                                case "A":
                                    this.MultiAnswerA.Checked = true;
                                    break;
                                case "B":
                                    this.MultiAnswerB.Checked = true;
                                    break;
                                case "C":
                                    this.MultiAnswerC.Checked = true;
                                    break;
                                case "D":
                                    this.MultiAnswerD.Checked = true;
                                    break;
                            }
                        }
                        this.singleTest.Style.Add("display", "none");
                        this.MultiTest.Style.Add("display", "");
                        this.PanDuanTest.Style.Add("display", "none");
                    }
                    else if (QuestionModel.Style == "3")
                    {
                        this.JudgeAnswer.Text = QuestionModel.Answer;
                        if (QuestionModel.Answer == "0")
                        {
                            JudgeRightAnswer.Text = QuestionModel.A;
                            RightAnswer.Style["display"] = "";
                        }
                        this.singleTest.Style.Add("display", "none");
                        this.MultiTest.Style.Add("display", "none");
                        this.PanDuanTest.Style.Add("display", "");
                    }
                }
            }
        }

        private Control FindControlById(Control parent, String id)
        {
            foreach (Control item in parent.Controls)
            {
                if (item.ID == id) return item;
                Control subControl = FindControlById(item, id);
                if (subControl != null) return subControl;
            }
            return null;
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            int queryString = RequestHelper.GetQueryString<int>("ID");

            if (_CourseId.SelectedValue == "-1" || _CourseId.SelectedValue == "")
            {
                ScriptHelper.Alert("请选择课程");
                Response.End();
            }
            QuestionInfo QuestionModel = new QuestionInfo();
            QuestionModel.CateId = Convert.ToInt32(_CourseId.SelectedValue);
            QuestionModel.QuestionId = queryString;
            QuestionModel.Question = this.Quetion.Text;
            if (QuestionModel.Question == "")
            {
                ScriptHelper.Alert("请填写题目");
                Response.End();
            }
            QuestionModel.Style = this.TestType.Text;
            QuestionModel.CompanyId = systemCompanyId;
            if (QuestionModel.Style == "1")
            {
                QuestionModel.A = this.SingleA.Text;
                QuestionModel.B = this.SingleB.Text;
                QuestionModel.C = this.SingleC.Text;
                QuestionModel.D = this.SingleD.Text;

                if (QuestionModel.A == "" || QuestionModel.B == "" || QuestionModel.C == "" || QuestionModel.D == "")
                {
                    ScriptHelper.Alert("请填写选项");
                    Response.End();
                }
                if (QuestionModel.A == QuestionModel.B || QuestionModel.A == QuestionModel.C || QuestionModel.A == QuestionModel.D || QuestionModel.B == QuestionModel.C || QuestionModel.B == QuestionModel.D || QuestionModel.C == QuestionModel.D)
                {
                    ScriptHelper.Alert("请正确填写选项");
                    Response.End();
                }

                string Answer = string.Empty;
                if (SingleAnswerA.Checked) Answer = SingleAnswerA.Value;
                else if (SingleAnswerB.Checked) Answer = SingleAnswerB.Value;
                else if (SingleAnswerC.Checked) Answer = SingleAnswerC.Value;
                else if (SingleAnswerD.Checked) Answer = SingleAnswerD.Value;
                QuestionModel.Answer = Answer;
            }
            else if (QuestionModel.Style == "2")
            {
                QuestionModel.A = this.MultiA.Text;
                QuestionModel.B = this.MultiB.Text;
                QuestionModel.C = this.MultiC.Text;
                QuestionModel.D = this.MultiD.Text;

                if (QuestionModel.A == "" || QuestionModel.B == "" || QuestionModel.C == "" || QuestionModel.D == "")
                {
                    ScriptHelper.Alert("请填写选项");
                    Response.End();
                }
                if (QuestionModel.A == QuestionModel.B || QuestionModel.A == QuestionModel.C || QuestionModel.A == QuestionModel.D || QuestionModel.B == QuestionModel.C || QuestionModel.B == QuestionModel.D || QuestionModel.C == QuestionModel.D)
                {
                    ScriptHelper.Alert("请正确填写选项");
                    Response.End();
                }
                string Answer = string.Empty;
                if (MultiAnswerA.Checked) Answer = Answer + SingleAnswerA.Value;
                if (MultiAnswerB.Checked) Answer = Answer + SingleAnswerB.Value;
                if (MultiAnswerC.Checked) Answer = Answer + SingleAnswerC.Value;
                if (MultiAnswerD.Checked) Answer = Answer + SingleAnswerD.Value;
                QuestionModel.Answer = Answer;
                if (QuestionModel.Answer.Length <= 1)
                {
                    ScriptHelper.Alert("多项选择题的答案不能低于2个选项！");
                    Response.End();
                }
            }
            else if (QuestionModel.Style == "3")
            {
                QuestionModel.Answer = this.JudgeAnswer.Text;
                if (QuestionModel.Answer == "0")
                {
                    QuestionModel.A = JudgeRightAnswer.Text;
                }
            }
            if (QuestionModel.Answer == "")
            {
                ScriptHelper.Alert("请选择标准答案");
                Response.End();
            }
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (QuestionModel.QuestionId == int.MinValue)
            {
                base.CheckAdminPower("AddQuestion", PowerCheckType.Single);
                int id = QuestionBLL.AddQuestion(QuestionModel);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Question"), id);
                ResponseHelper.Write("<script>alert('" + alertMessage + "');</script>");

                this.TestType.Text = "1";
                this.Quetion.Text = string.Empty;
                this.SingleA.Text = string.Empty;
                this.SingleB.Text = string.Empty;
                this.SingleC.Text = string.Empty;
                this.SingleD.Text = string.Empty;
                this.MultiA.Text = string.Empty;
                this.MultiB.Text = string.Empty;
                this.MultiC.Text = string.Empty;
                this.MultiD.Text = string.Empty;
                this.JudgeRightAnswer.Text = string.Empty;
                SingleAnswerA.Checked = false;
                SingleAnswerB.Checked = false;
                SingleAnswerC.Checked = false;
                SingleAnswerD.Checked = false;
                MultiAnswerA.Checked = false;
                MultiAnswerB.Checked = false;
                MultiAnswerC.Checked = false;
                MultiAnswerD.Checked = false;
                this.JudgeAnswer.SelectedIndex = -1;

            }
            else
            {
                base.CheckAdminPower("UpdateQuestion", PowerCheckType.Single);
                QuestionBLL.UpdateQuestion(QuestionModel);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Question"), QuestionModel.QuestionId);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
                AdminBasePage.Alert(alertMessage, RequestHelper.RawUrl);
            }
        }

        protected void _CateId_SelectedIndexChanged(object sender, EventArgs e)
        {
            CourseInfo CourseModel = new CourseInfo();
            CourseModel.CateId = Convert.ToInt32(this._CateId.SelectedValue);
            this._CourseId.DataSource = CourseBLL.ReadList(CourseModel);
            this._CourseId.DataTextField = "CourseName";
            this._CourseId.DataValueField = "CourseId";
            this._CourseId.DataBind();
            this._CourseId.Items.Insert(0, new ListItem("请选择课程", "-1"));
        }

        protected void JudgeAnswer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (JudgeAnswer.SelectedValue == "0")
            {
                RightAnswer.Style["display"] = "";
            }
            else
            {
                RightAnswer.Style["display"] = "none";
            }
        }
    }
}
