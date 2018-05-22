using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.IBLL;

namespace XueFuShop.Pages
{
    public class CourseDetail : CommonBasePage
    {
        protected List<AttributeRecordInfo> attributeRecordList = new List<AttributeRecordInfo>();
        protected decimal currentMemberPrice = 0M;
        protected int leftStorageCount = 0;
        protected List<MemberPriceInfo> memberPriceList = new List<MemberPriceInfo>();
        protected ProductInfo product = new ProductInfo();
        protected List<ArticleInfo> productArticleList = new List<ArticleInfo>();
        protected List<ProductPhotoInfo> productPhotoList = new List<ProductPhotoInfo>();
        protected List<TagsInfo> productTagsList = new List<TagsInfo>();
        protected List<StandardInfo> standardList = new List<StandardInfo>();
        protected List<StandardRecordInfo> standardRecordList = new List<StandardRecordInfo>();
        protected string standardRecordValueList = "|";
        protected string strHistoryProduct = string.Empty;
        protected List<MemberPriceInfo> tempMemberPriceList = new List<MemberPriceInfo>();
        protected List<ProductInfo> tempProductList = new List<ProductInfo>();
        protected List<UserGradeInfo> userGradeList = new List<UserGradeInfo>();
        protected List<TeacherInfo> teacherList = new List<TeacherInfo>();


        protected override void PageLoad()
        {
            base.PageLoad();
            int queryString = RequestHelper.GetQueryString<int>("ID");
            this.product = ProductBLL.ReadProduct(queryString);
            if (this.product.IsSale == 0)
            {
                ScriptHelper.Alert("该产品未上市，不能查看");
            }
            ProductBLL.ChangeProductViewCount(queryString, 1);
            ProductPhotoInfo item = new ProductPhotoInfo();
            item.Name = this.product.Name;
            item.Photo = this.product.Photo;
            this.productPhotoList.Add(item);
            this.productPhotoList.AddRange(ProductPhotoBLL.ReadProductPhotoByProduct(queryString));
            
            this.attributeRecordList = AttributeRecordBLL.ReadAttributeRecordByProduct(queryString);

            TeacherSearchInfo teacherSearch = new TeacherSearchInfo();
            teacherSearch.InProductID = product.ID.ToString();
            ITeacherBLL teacherBLL = new TeacherBLL();
            teacherList = teacherBLL.ReadList(teacherSearch);

            if (ShopConfig.ReadConfigInfo().ProductStorageType == 1)
            {
                this.leftStorageCount = this.product.TotalStorageCount - this.product.OrderCount;
            }
            else
            {
                this.leftStorageCount = this.product.ImportVirtualStorageCount;
            }
            base.Title = this.product.Name;
            base.Keywords = (this.product.Keywords == string.Empty) ? this.product.Name : this.product.Keywords;
            base.Description = (this.product.Summary == string.Empty) ? StringHelper.Substring(StringHelper.KillHTML(this.product.Introduction), 200) : this.product.Summary;
        }
    }
}
