using System;
using System.Text.RegularExpressions;
using XueFuShop.Common;
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.BLL;
using XueFuShop.Models;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace XueFuShop.Admin
{
    public partial class UserAdd : AdminBasePage
    {
        protected int city = 0;
        protected int country = 0;
        protected int district = 0;
        protected int province = 0;
        protected int companyID = int.MinValue;
        protected int userID = RequestHelper.GetQueryString<int>("ID");

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PostStart.Visible = false;
            if (!this.Page.IsPostBack)
            {
                //this.UserRegion.DataSource = RegionBLL.ReadRegionUnlimitClass();

                if (userID != -2147483648)
                {
                    base.CheckAdminPower("ReadUser", PowerCheckType.Single);

                    UserInfo info = UserBLL.ReadUser(userID);
                    companyID = info.CompanyID;
                    CompanyInfo company = CompanyBLL.ReadCompany(info.CompanyID);
                    CompanyName.Value = company.CompanyName;

                    this.GroupID.DataSource = AdminGroupBLL.ReadAdminGroupList(companyID);
                    this.GroupID.DataTextField = "Name";
                    this.GroupID.DataValueField = "ID";
                    this.GroupID.DataBind();
                    this.GroupID.Items.Insert(0, new ListItem("请选择管理组", "0"));

                    List<PostInfo> postList = PostBLL.ReadPostListByPostId(company.Post);
                    PostList.DataSource = postList;
                    PostList.DataTextField = "PostName";
                    PostList.DataValueField = "PostId";
                    PostList.DataBind();
                    PostList.Items.Insert(0, new ListItem("请选择岗位", "0"));

                    StudyPostId.DataSource = postList;
                    StudyPostId.DataTextField = "PostName";
                    StudyPostId.DataValueField = "PostId";
                    StudyPostId.DataBind();
                    StudyPostId.Items.Insert(0, new ListItem("请选择岗位", "0"));

                    Department.DataSource = PostBLL.ReadPostListByPostId(PostBLL.ReadDepartmentIdStrByPostId(company.Post));
                    Department.DataTextField = "PostName";
                    Department.DataValueField = "PostId";
                    Department.DataBind();
                    Department.Items.Insert(0, new ListItem("请选择部门", "0"));

                    this.RealName.Text = info.RealName;
                    this.UserName.Text = info.UserName;
                    this.GroupID.SelectedValue = info.GroupID.ToString();
                    this.UserPassword.Text = info.UserPassword;
                    this.Email.Text = info.Email;
                    this.Sex.Text = info.Sex.ToString();
                    this.Tel.Text = info.Tel;
                    this.Mobile.Text = info.Mobile;
                    this.Status.Text = info.Status.ToString();
                    this.Department.SelectedValue = info.Department.ToString();
                    this.PostList.SelectedValue = info.WorkingPostID.ToString();
                    this.PostName.Text = info.PostName;
                    this.StudyPostId.SelectedValue = info.StudyPostID.ToString();
                    this.PostStartDate.Text = info.PostStartDate.ToString("d");
                    this.PostStart.Visible = true;
                    this.RegDate.Value = info.RegisterDate.ToString("d");
                    //this.Introduce.Text = info.Introduce;
                    //this.Photo.Text = info.Photo;
                    //this.MSN.Text = info.MSN;
                    //this.QQ.Text = info.QQ;
                    //this.UserRegion.ClassID = info.RegionID;
                    //this.Address.Text = info.Address;
                    //this.Birthday.Text = info.Birthday;
                    //this.UserName.Enabled = false;
                    this.Add.Visible = false;
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string text = this.UserName.Text;
            int queryString = RequestHelper.GetQueryString<int>("ID");
            int groupID = RequestHelper.GetForm<int>(ShopConfig.ReadConfigInfo().NamePrefix + "GroupID");
            int oldCompanyID = int.MinValue;
            int oldStudyPostID = int.MinValue;
            UserInfo user = new UserInfo();
            if (queryString > 0)
            {
                user = UserBLL.ReadUser(queryString);
                oldCompanyID = user.CompanyID;
                oldStudyPostID = user.StudyPostID;
            }
            else
            {
                Regex regex = new Regex("^([a-zA-Z0-9_一-龥])+$");
                if (!regex.IsMatch(text))
                {
                    ScriptHelper.Alert("用户名只能包含字母、数字、下划线、中文", RequestHelper.RawUrl);
                }
                user.UserPassword = StringHelper.Password(this.UserPassword.Text, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            }
            user.ID = queryString;
            companyID = RequestHelper.GetForm<int>("CompanyID");
            if (companyID < 0) ScriptHelper.Alert("请选择公司");
            user.CompanyID = companyID;
            user.RealName = RealName.Text;
            user.UserName = text;
            user.GroupID = groupID;
            if (user.GroupID <= 0) ScriptHelper.Alert("请选择管理组");
            user.Email = this.Email.Text;
            user.Sex = Convert.ToInt32(this.Sex.Text);
            user.Tel = this.Tel.Text;
            user.Mobile = this.Mobile.Text;
            //验证手机号码是否存在
            //if (UserBLL.IsExistMobile(user.Mobile, user.ID))
            //    ScriptHelper.Alert("手机号码已存在");

            user.RegisterDate = RequestHelper.DateNow;
            user.LastLoginDate = RequestHelper.DateNow;
            user.FindDate = RequestHelper.DateNow;

            //判断是否需要恢复考试记录
            bool isRecoveryTestPaper = false;
            int userState = Convert.ToInt32(this.Status.Text);
            if (user.Status == (int)UserState.Del && userState != (int)UserState.Del)
                isRecoveryTestPaper = true;
            user.Status = userState;

            string department = this.Department.SelectedValue;
            user.Department = RequestHelper.GetForm<int>(ShopConfig.ReadConfigInfo().NamePrefix + "Department");
            if (user.Department <= 0) ScriptHelper.Alert("请选择部门");
            user.WorkingPostID = RequestHelper.GetForm<int>(ShopConfig.ReadConfigInfo().NamePrefix + "PostList");
            if (user.WorkingPostID <= 0) ScriptHelper.Alert("请选择工作岗位");
            user.PostName = this.PostName.Text;
            user.StudyPostID = RequestHelper.GetForm<int>(ShopConfig.ReadConfigInfo().NamePrefix + "StudyPostId");
            if (user.StudyPostID <= 0) ScriptHelper.Alert("请选择学习岗位");
            //user.Introduce = this.Introduce.Text;
            //user.Photo = this.Photo.Text;
            //user.MSN = this.MSN.Text;
            //user.QQ = this.QQ.Text;
            //user.RegionID = this.UserRegion.ClassID;
            //user.Address = this.Address.Text;
            //user.Birthday = this.Birthday.Text;
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (user.ID == -2147483648)
            {
                user.PostStartDate = RequestHelper.DateNow;
                base.CheckAdminPower("AddUser", PowerCheckType.Single);

                if (UserBLL.IsUserNumOverflow(user.CompanyID))
                {
                    ScriptHelper.Alert("超过用户数量，暂不能添加！");
                }

                int id = UserBLL.AddUser(user);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("User"), id);
            }
            else
            {
                base.CheckAdminPower("UpdateUser", PowerCheckType.Single);

                if (!string.IsNullOrEmpty(PostStartDate.Text))
                    user.PostStartDate = Convert.ToDateTime(PostStartDate.Text);
                //变换学习岗位，如果岗位已通过则更新注册时间，以便岗位计划按新时间重新计数，否则不更改原有岗位计划统计时间
                PostInfo studyPost = PostBLL.ReadPost(user.StudyPostID);
                PostInfo oldStudyPost = PostBLL.ReadPost(oldStudyPostID);
                if (oldCompanyID != user.CompanyID || (user.StudyPostID != oldStudyPostID && (studyPost.ParentId == 3 || oldStudyPost.ParentId == 3) && studyPost.ParentId != oldStudyPost.ParentId))
                {
                    if (user.PostStartDate < DateTime.Today)
                    {
                        user.PostStartDate = DateTime.Today;
                    }
                }

                UserBLL.UpdateUser(user);
                if (isRecoveryTestPaper)
                    TestPaperBLL.RecoveryPaperByUserID(user.ID.ToString());
                //如果公司ID更改，相应修改成绩列表
                //if (oldCompanyID != user.CompanyID)
                //{
                //    TestPaperBLL.UpdatePaperCompanyId(user.ID, user.CompanyID);
                //}
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("User"), user.ID);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            AdminBasePage.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}
