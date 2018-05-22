using System;
using System.Web.UI.WebControls;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFuShop.Pages;
using XueFuShop.Common;

namespace XueFuShop.Admin
{
    public partial class PostAdd : AdminBasePage
    {
        public int companyID = CompanyBLL.SystemCompanyId;
        public string companyName = "上海孟特管理咨询有限公司";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int queryString = RequestHelper.GetQueryString<int>("ID");

                base.CheckAdminPower("ReadPost", PowerCheckType.Single);

                if (queryString != -2147483648)
                {
                    PostInfo postModel = PostBLL.ReadPost(queryString);
                    if (postModel.CompanyID != companyID)
                    {
                        companyID = postModel.CompanyID;
                        companyName = CompanyBLL.ReadCompany(companyID).CompanyName;
                    }

                    //string parentCompanyID = CompanyBLL.ReadParentCompanyId(postModel.CompanyID);
                    //parentCompanyID = string.IsNullOrEmpty(parentCompanyID) ? CompanyBLL.SystemCompanyId.ToString() : CompanyBLL.SystemCompanyId.ToString() + "," + parentCompanyID;

                    this.CateId.DataSource = PostBLL.FilterPostListByCompanyID(PostBLL.ReadPostCateNamedList(), postModel.CompanyID.ToString());
                    this.CateId.DataTextField = "PostName";
                    this.CateId.DataValueField = "PostId";
                    this.CateId.DataBind();
                    this.CateId.Items.Insert(0, new ListItem("设置为新部门", "0"));

                    this.PostName.Text = postModel.PostName;
                    this.OrderIndex.Text = postModel.OrderIndex.ToString();
                    if (postModel.IsPost == 1)
                        this.IsPost.Checked = true;
                    if (CateId.Items.Contains(CateId.Items.FindByValue(postModel.ParentId.ToString()))) CateId.Items.FindByValue(postModel.ParentId.ToString()).Selected = true;
                }
                else
                {
                    this.CateId.DataSource = PostBLL.FilterPostListByCompanyID(PostBLL.ReadPostCateNamedList(), CompanyBLL.SystemCompanyId.ToString());
                    this.CateId.DataTextField = "PostName";
                    this.CateId.DataValueField = "PostId";
                    this.CateId.DataBind();
                    this.CateId.Items.Insert(0, new ListItem("设置为新部门", "0"));
                }
            }
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            int queryString = RequestHelper.GetQueryString<int>("ID");

            int ParentId = RequestHelper.GetForm<int>(ShopConfig.ReadConfigInfo().NamePrefix + "CateId");
            PostInfo PostModel = new PostInfo();
            PostModel.PostName = this.PostName.Text;
            PostModel.PostId = queryString;
            PostModel.ParentId = ParentId;
            PostModel.OrderIndex = int.Parse(OrderIndex.Text);
            PostModel.CompanyID = RequestHelper.GetForm<int>("CompanyId");
            if (IsPost.Checked)
                PostModel.IsPost = 1;
            else
                PostModel.IsPost = 0;

            //PostModel.PostPlan = RequestHelper.GetIntsForm("SelectId");
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (PostModel.PostId == -2147483648)
            {
                base.CheckAdminPower("AddPost", PowerCheckType.Single);
                int id = PostBLL.AddPost(PostModel);

                //自动把客户公司专属的岗位添加到对应的公司信息里
                if (PostModel.CompanyID > 0 && PostModel.IsPost == 1)
                {
                    CompanyInfo company = CompanyBLL.ReadCompany(PostModel.CompanyID);
                    company.Post = string.IsNullOrEmpty(company.Post) ? id.ToString() : company.Post + "," + id.ToString();
                    CompanyBLL.UpdateCompany(company);
                }
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Post"), id);
            }
            else
            {
                base.CheckAdminPower("UpdatePost", PowerCheckType.Single);
                PostModel.PostPlan = PostBLL.ReadPost(queryString).PostPlan;
                PostBLL.UpdatePost(PostModel);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Post"), queryString);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            AdminBasePage.Alert(alertMessage, RequestHelper.RawUrl);

        }
    }
}
