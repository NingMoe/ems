using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFu.EntLib;
using System.Web.UI.WebControls;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class TrainingCourseAdd : UserManageBasePage
    {
        protected List<ProductInfo> trainingProductList = new List<ProductInfo>();
        protected List<ProductClassInfo> productClassList = new List<ProductClassInfo>();
        protected Dictionary<string, Dictionary<string, string>> rootProductClassList = new Dictionary<string, Dictionary<string, string>>();
        protected PostInfo post = new PostInfo();
        protected int postID = RequestHelper.GetQueryString<int>("PostID");

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "培训课程设置";
            if (postID <= 0 && Session["TrainingID"] != null)
                postID = Convert.ToInt32(Session["TrainingID"]);
            if (postID > 0)
                post = PostBLL.ReadPost(postID);
            else
            {
                ScriptHelper.Alert("页面参数错误！");
            }
            this.productClassList = ProductClassBLL.ReadProductClassRootList();

            #region 产品分类设置
            {
                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.InBrandID = CookiesHelper.ReadCookieValue("UserCompanyBrandID");
                productSearch.IsSale = 1;
                productSearch.InCompanyID = CompanyBLL.ReadParentCompanyIDWithSelf(base.UserCompanyID);
                string productIDStr = ProductBLL.ReadProductIdStr(ProductBLL.SearchProductList(productSearch));
                rootProductClassList = ProductClassBLL.ReadProductClassListByProductID(productIDStr, 1);
            }
            #endregion

            if (!string.IsNullOrEmpty(post.PostPlan))
            {
                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.InProductID = post.PostPlan;
                trainingProductList = ProductBLL.SearchProductList(productSearch);
            }
        }

        protected override void PostBack()
        {
            postID = RequestHelper.GetForm<int>("PostID");
            if (postID > 0 && PostBLL.ReadPost(postID).CompanyID == base.UserCompanyID)
            {
                string trainingCourseID = RequestHelper.GetForm<string>("TrainingCourseID");
                PostBLL.UpdatePostPlan(postID, trainingCourseID);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("PostCourse"), trainingCourseID);
                string alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
                string returnURL = ServerHelper.UrlDecode(RequestHelper.GetQueryString<string>("ReturnURL"));
                if (string.IsNullOrEmpty(returnURL))
                    ScriptHelper.Alert(alertMessage, "/User/Training.aspx");
                else
                    ScriptHelper.Alert(alertMessage, returnURL);
            }
        }
    }
}
