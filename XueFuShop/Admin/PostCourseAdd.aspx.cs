using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFuShop.Pages;
using XueFu.EntLib;
using XueFuShop.Common;

namespace XueFuShop.Admin
{
    public partial class PostCourseAdd : AdminBasePage
    {
        protected int postID = RequestHelper.GetQueryString<int>("PostID");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PostInfo post = PostBLL.ReadPost(postID);
                List<ProductClassInfo> productClassList = ProductClassBLL.ReadProductClassNamedList();
                foreach (ProductClassInfo info in productClassList)
                {
                    this.RelationClassID.Items.Add(new ListItem(info.ClassName, "|" + info.ID + "|"));
                }
                this.RelationClassID.Items.Insert(0, new ListItem("所有分类", string.Empty));

                if (!string.IsNullOrEmpty(post.PostPlan))
                {
                    ProductSearchInfo productSearch = new ProductSearchInfo();
                    productSearch.InProductID = post.PostPlan;
                    this.Product.DataSource = ProductBLL.SearchProductList(productSearch);
                    this.Product.DataTextField = "Name";
                    this.Product.DataValueField = "ID";
                    this.Product.DataBind();
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string postCourseID = RequestHelper.GetForm<string>("RelationProductID");
            PostBLL.UpdatePostPlan(postID, postCourseID);
            AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("PostCourse"), postCourseID);
            ScriptHelper.Alert(ShopLanguage.ReadLanguage("UpdateOK"), Request.RawUrl);
        }
    }
}
