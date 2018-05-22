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
                    this.GroupID.Items.Insert(0, new ListItem("��ѡ�������", "0"));

                    List<PostInfo> postList = PostBLL.ReadPostListByPostId(company.Post);
                    PostList.DataSource = postList;
                    PostList.DataTextField = "PostName";
                    PostList.DataValueField = "PostId";
                    PostList.DataBind();
                    PostList.Items.Insert(0, new ListItem("��ѡ���λ", "0"));

                    StudyPostId.DataSource = postList;
                    StudyPostId.DataTextField = "PostName";
                    StudyPostId.DataValueField = "PostId";
                    StudyPostId.DataBind();
                    StudyPostId.Items.Insert(0, new ListItem("��ѡ���λ", "0"));

                    Department.DataSource = PostBLL.ReadPostListByPostId(PostBLL.ReadDepartmentIdStrByPostId(company.Post));
                    Department.DataTextField = "PostName";
                    Department.DataValueField = "PostId";
                    Department.DataBind();
                    Department.Items.Insert(0, new ListItem("��ѡ����", "0"));

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
                Regex regex = new Regex("^([a-zA-Z0-9_һ-��])+$");
                if (!regex.IsMatch(text))
                {
                    ScriptHelper.Alert("�û���ֻ�ܰ�����ĸ�����֡��»��ߡ�����", RequestHelper.RawUrl);
                }
                user.UserPassword = StringHelper.Password(this.UserPassword.Text, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
            }
            user.ID = queryString;
            companyID = RequestHelper.GetForm<int>("CompanyID");
            if (companyID < 0) ScriptHelper.Alert("��ѡ��˾");
            user.CompanyID = companyID;
            user.RealName = RealName.Text;
            user.UserName = text;
            user.GroupID = groupID;
            if (user.GroupID <= 0) ScriptHelper.Alert("��ѡ�������");
            user.Email = this.Email.Text;
            user.Sex = Convert.ToInt32(this.Sex.Text);
            user.Tel = this.Tel.Text;
            user.Mobile = this.Mobile.Text;
            //��֤�ֻ������Ƿ����
            //if (UserBLL.IsExistMobile(user.Mobile, user.ID))
            //    ScriptHelper.Alert("�ֻ������Ѵ���");

            user.RegisterDate = RequestHelper.DateNow;
            user.LastLoginDate = RequestHelper.DateNow;
            user.FindDate = RequestHelper.DateNow;

            //�ж��Ƿ���Ҫ�ָ����Լ�¼
            bool isRecoveryTestPaper = false;
            int userState = Convert.ToInt32(this.Status.Text);
            if (user.Status == (int)UserState.Del && userState != (int)UserState.Del)
                isRecoveryTestPaper = true;
            user.Status = userState;

            string department = this.Department.SelectedValue;
            user.Department = RequestHelper.GetForm<int>(ShopConfig.ReadConfigInfo().NamePrefix + "Department");
            if (user.Department <= 0) ScriptHelper.Alert("��ѡ����");
            user.WorkingPostID = RequestHelper.GetForm<int>(ShopConfig.ReadConfigInfo().NamePrefix + "PostList");
            if (user.WorkingPostID <= 0) ScriptHelper.Alert("��ѡ������λ");
            user.PostName = this.PostName.Text;
            user.StudyPostID = RequestHelper.GetForm<int>(ShopConfig.ReadConfigInfo().NamePrefix + "StudyPostId");
            if (user.StudyPostID <= 0) ScriptHelper.Alert("��ѡ��ѧϰ��λ");
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
                    ScriptHelper.Alert("�����û��������ݲ�����ӣ�");
                }

                int id = UserBLL.AddUser(user);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("User"), id);
            }
            else
            {
                base.CheckAdminPower("UpdateUser", PowerCheckType.Single);

                if (!string.IsNullOrEmpty(PostStartDate.Text))
                    user.PostStartDate = Convert.ToDateTime(PostStartDate.Text);
                //�任ѧϰ��λ�������λ��ͨ�������ע��ʱ�䣬�Ա��λ�ƻ�����ʱ�����¼��������򲻸���ԭ�и�λ�ƻ�ͳ��ʱ��
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
                //�����˾ID���ģ���Ӧ�޸ĳɼ��б�
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
