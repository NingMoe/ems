using System;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class PostCourse : AdminBasePage
    {
        protected int postID = RequestHelper.GetQueryString<int>("PostID");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (RequestHelper.GetQueryString<string>("Action") == "Delete")
                {
                    string selectID = RequestHelper.GetQueryString<string>("ID");
                    if (!string.IsNullOrEmpty(selectID))
                    {
                        DeletePostCourseID(selectID);
                    }
                }
                ProductSearchInfo product = new ProductSearchInfo();
                string postCourseID = PostBLL.ReadPost(postID).PostPlan;
                if (!string.IsNullOrEmpty(postCourseID))
                {
                    product.InProductID = postCourseID;
                    base.BindControl(ProductBLL.SearchProductList(product), this.RecordList, this.MyPager);
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string selectID = RequestHelper.GetIntsForm("SelectID");
            if (!string.IsNullOrEmpty(selectID))
            {
                DeletePostCourseID(selectID);
            }
        }

        private void DeletePostCourseID(string selectID)
        {
            string CourseContent = PostBLL.ReadPost(postID).PostPlan;
            CourseContent = StringHelper.SubString(CourseContent, selectID);
            PostBLL.UpdatePostPlan(postID, CourseContent);
            AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("PostCourse"), selectID);
            ScriptHelper.Alert("É¾³ý³É¹¦£¡", Request.RawUrl);
        }
    }
}
