using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class TrainingAdd : UserManageBasePage
    {
        protected PostInfo post = new PostInfo();
        protected List<PostInfo> parentPostList = new List<PostInfo>();
        protected int id = RequestHelper.GetQueryString<int>("ID");
        protected string action = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("Action"));
        protected string result = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("Result"));
        protected string pageTitle = string.Empty;

        protected override void PageLoad()
        {
            base.PageLoad();

            if (id > 0)
            {
                post = PostBLL.ReadPost(id);
                base.Title = pageTitle = post.IsPost == 1 ? "修改培训信息" : "修改培训分类";
                action = post.IsPost == 1 ? "Training" : "TrainingClass";
            }

            switch (action)
            {
                case "TrainingClass":
                    base.Title = pageTitle = "添加培训分类";
                    break;

                case "Training":
                    base.Title = pageTitle = "添加培训项目";
                    //添加或修改培训信息时，需调取上级培训分类信息
                    List<PostInfo> postCateRoot = PostBLL.ReadPostCateRootList(base.UserCompanyID.ToString());
                    if (postCateRoot.Count > 0)
                    {
                        PostInfo firstPost = PostBLL.ReadPostCateRootList(base.UserCompanyID.ToString())[0];
                        parentPostList = PostBLL.FilterPostListByCompanyID(PostBLL.ReadPostListByParentID(firstPost.PostId), base.UserCompanyID.ToString());
                    }
                    break;
            }
        }

        protected override void PostBack()
        {
            post = PostBLL.ReadPost(id);

            post.PostName = StringHelper.SearchSafe(RequestHelper.GetForm<string>("PostName"));
            post.OrderIndex = 0;
            post.CompanyID = base.UserCompanyID;
            post.IsPost = 0;

            if (action == "Training")
            {
                post.ParentId = RequestHelper.GetForm<int>("ParentID");
                post.IsPost = 1;
            }

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (id > 0)
            {
                PostBLL.UpdatePost(post);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Post"), id);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
                string returnURL = ServerHelper.UrlDecode(RequestHelper.GetQueryString<string>("ReturnURL"));
                if (string.IsNullOrEmpty(returnURL))
                    ScriptHelper.Alert(alertMessage, "/User/TrainingAdd.aspx?Action=" + action);
                else
                    ScriptHelper.Alert(alertMessage, returnURL);
            }
            else
            {
                //添加培训分类时，设置ParentID的值
                if (action == "TrainingClass")
                {
                    List<PostInfo> companyTrainingList = PostBLL.ReadPostCateRootList(base.UserCompanyID.ToString());
                    if (companyTrainingList.Count > 0)
                    {
                        post.ParentId = companyTrainingList[0].PostId;
                    }
                    else
                    {
                        CompanyInfo company = CompanyBLL.ReadCompany(base.UserCompanyID);
                        PostInfo firstPost = new PostInfo();
                        firstPost.CompanyID = company.CompanyId;
                        firstPost.PostName = company.CompanySimpleName;
                        firstPost.ParentId = 0;
                        firstPost.IsPost = 0;

                        id = PostBLL.AddPost(firstPost);
                        post.ParentId = id;
                        AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Post"), id);
                    }
                }
                id = PostBLL.AddPost(post);
                //更新公司里的岗位设定
                if (post.IsPost == 1)
                {
                    CompanyInfo company = CompanyBLL.ReadCompany(base.UserCompanyID);
                    company.Post = string.IsNullOrEmpty(company.Post) ? id.ToString() : string.Concat(company.Post, ",", id.ToString());
                    CompanyBLL.UpdateCompany(company);
                }
                Session["TrainingID"] = id.ToString();
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Post"), id);
                if (post.IsPost == 1)
                    ResponseHelper.Redirect("/User/TrainingAdd.aspx?Action=" + action + "&Result=Success");
                else
                    ScriptHelper.Alert(alertMessage, "/User/TrainingAdd.aspx?Action=" + action);
            }
        }
    }
}
