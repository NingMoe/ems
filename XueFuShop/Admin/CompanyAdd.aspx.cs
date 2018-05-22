using System;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace XueFuShop.Admin
{
    public partial class CompanyAdd : AdminBasePage
    {
        protected int companyID = RequestHelper.GetQueryString<int>("ID");
        protected StringBuilder BrandHtml = new StringBuilder();
        protected StringBuilder PostHtml = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            string companyPost = string.Empty;
            string companyBrandId = string.Empty;
            if (companyID < 0) companyID = CompanyBLL.SystemCompanyId;
            if (!this.Page.IsPostBack)
            {
                if (companyID > 0)
                {
                    base.CheckAdminPower("ReadCompany", PowerCheckType.Single);
                    CompanyInfo CompanyModel = CompanyBLL.ReadCompany(companyID);
                    companyPost = CompanyModel.Post;
                    CompanyType.Text = CompanyModel.GroupId.ToString();

                    //如果是子集团或或者是集团下的子公司，都需调出上级单位
                    if (CompanyModel.GroupId > 1)
                    {
                        GroupListId.Value = CompanyModel.ParentId;
                        foreach (string Item in CompanyModel.ParentId.Split(','))
                        {
                            GroupNameList.InnerHtml += "<li id=\"li_" + Item + "\" style=\"float:none;\">" + CompanyBLL.ReadCompany(int.Parse(Item)).CompanyName + "<a onclick=\"javascript:DelGroup(" + Item + ");\"><img src=\"Style/Images/delete.gif\"></a></li>";
                        }

                        CompanyInfo Model = new CompanyInfo();
                        Model.GroupIdCondition = "1,2";
                        Model.CompanyId = companyID;
                        GroupId.DataSource = CompanyBLL.ReadCompanyList(Model);
                        GroupId.DataTextField = "CompanyName";
                        GroupId.DataValueField = "CompanyId";
                        GroupId.DataBind();
                        GroupId.Items.Insert(0, new ListItem("请选择隶属公司", "-1"));
                        GroupBrand.Style["display"] = "";
                    }

                    CompanyName.Text = CompanyModel.CompanyName;
                    CompanySimpleName.Text = CompanyModel.CompanySimpleName;
                    companyBrandId = CompanyModel.BrandId;
                    CompanyTel.Text = CompanyModel.CompanyTel;
                    CompanyPost.Text = CompanyModel.CompanyPost;
                    CompanyAddress.Text = CompanyModel.CompanyAddress;
                    PostStartDate.Text = CompanyModel.PostStartDate.ToString().Split(' ')[0];
                    if (CompanyModel.EndDate != null)
                        EndDate.Text = CompanyModel.EndDate.ToString().Split(' ')[0];
                    Sort.Text = CompanyModel.Sort.ToString();
                    State.Text = CompanyModel.State.ToString();
                    UserNum.Text = CompanyModel.UserNum.ToString();
                    IsTest.Checked = CompanyModel.IsTest;
                }

                //品牌设置
                BrandHtml.AppendLine("<dl class=\"carbrand\">");
                foreach (ProductBrandInfo info in ProductBrandBLL.ReadProductBrandCacheList())
                {
                    BrandHtml.AppendLine("<dd><input name=\"BrandId\" type=\"checkbox\" value=\"" + info.ID + "\"");
                    if (StringHelper.CompareString(companyBrandId, info.ID.ToString()))
                    {
                        BrandHtml.Append(" checked");
                    }
                    BrandHtml.Append(">" + info.Name + "</dd>");
                }
                BrandHtml.AppendLine("</dl>");

                PostHtml.Append(CreatePostHtml(companyPost, 0));
            }
        }

        private string CreatePostHtml(string companyPost,int parentPostID)
        {
            StringBuilder postHtml = new StringBuilder();
            //岗位设置
            postHtml.Append("<div>");
            List<PostInfo> postList = PostBLL.FilterPostListByCompanyID(PostBLL.ReadPostList(parentPostID), companyID);
            foreach (PostInfo info in postList)
            {
                //if (info.CompanyID != CompanyBLL.SystemCompanyId && parentPostID==0 && info.IsPost==0)
                //{
                //    postHtml.Append(CreatePostHtml(companyPost, info.PostId));
                //    continue;
                //}
                postHtml.AppendLine("<dl>");
                postHtml.AppendLine("<dt><input name=\"Department\" type=\"checkbox\" value=\"" + info.PostId + "\">" + info.PostName + "</dt>");
                postHtml.AppendLine("<div class=\"post\">");
                foreach (PostInfo info2 in PostBLL.FilterPostListByCompanyID(PostBLL.ReadPostList(info.PostId), companyID))
                {
                    postHtml.AppendLine("<dd><input name=\"Post\" data-type=\"" + info.PostId + "\" type=\"checkbox\" value=\"" + info2.PostId + "\"");
                    if (StringHelper.CompareString(companyPost, info2.PostId.ToString()))
                    {
                        postHtml.Append(" checked");
                    }
                    postHtml.Append(">" + info2.PostName + "</dd>");
                }
                postHtml.AppendLine("</div>");
                postHtml.AppendLine("</dl>");
            }
            postHtml.Append("</div>");
            return postHtml.ToString();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            CompanyInfo CompanyModel = new CompanyInfo();
            CompanyModel.CompanyId = RequestHelper.GetQueryString<int>("ID");
            CompanyModel.CompanyName = CompanyName.Text.Trim();
            if (CompanyModel.CompanyName.Length < 5)
            {
                ScriptHelper.Alert("请输入完整的公司名称！");
            }
            CompanyModel.Post = RequestHelper.GetIntsForm("Post");
            CompanyModel.CompanySimpleName = CompanySimpleName.Text.Trim();
            CompanyModel.CompanyTel = CompanyTel.Text.Trim();
            CompanyModel.CompanyPost = this.CompanyPost.Text.Trim();
            CompanyModel.CompanyAddress = CompanyAddress.Text.Trim();
            if (!string.IsNullOrEmpty(PostStartDate.Text)) CompanyModel.PostStartDate = PostStartDate.Text;
            if (!string.IsNullOrEmpty(EndDate.Text)) CompanyModel.EndDate = EndDate.Text;
            CompanyModel.State = Convert.ToInt32(State.SelectedValue);
            CompanyModel.BrandId = RequestHelper.GetIntsForm("BrandId");
            if (CompanyType.Text == "") ScriptHelper.Alert("请选择公司类型！");
            CompanyModel.GroupId = Convert.ToInt32(CompanyType.Text);
            CompanyModel.UserNum = Convert.ToInt32(UserNum.Text);
            CompanyModel.IsTest = IsTest.Checked;
            CompanyModel.Sort = Convert.ToInt32(Sort.Text);
            if (CompanyModel.GroupId == 2 || CompanyModel.GroupId == 3)
            {
                CompanyModel.ParentId = GroupListId.Value;
                if (string.IsNullOrEmpty(CompanyModel.ParentId)) ScriptHelper.Alert("请选择隶属公司");

                if (CompanyModel.GroupId == 2 && CompanyModel.CompanyId != int.MinValue)
                {
                    if (StringHelper.CompareString(CompanyBLL.ReadCompanyIdList(companyID.ToString()), CompanyModel.ParentId.ToString()))
                    {
                        ScriptHelper.Alert("如果要转移到旗下公司，请先将旗下公司移动出去！");
                    }
                }
            }
            else
            {
                if (CompanyModel.CompanyId != int.MinValue)
                    CompanyModel.ParentId = CompanyBLL.ReadCompany(CompanyModel.CompanyId).ParentId;
                else
                    CompanyModel.ParentId = CompanyBLL.SystemCompanyId.ToString();
            }
            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (CompanyModel.CompanyId == int.MinValue)
            {
                base.CheckAdminPower("AddCompany", PowerCheckType.Single);
                int id = CompanyBLL.AddCompany(CompanyModel);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Company"), id);
            }
            else
            {
                base.CheckAdminPower("UpdateCompany", PowerCheckType.Single);
                CompanyBLL.UpdateCompany(CompanyModel);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Company"), CompanyModel.CompanyId);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            AdminBasePage.Alert(alertMessage, RequestHelper.RawUrl);
        }
    }
}
