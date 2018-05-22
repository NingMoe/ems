using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.Pages;
using System.Text;

namespace XueFuShop.Admin
{
    public partial class TestCourse : AdminBasePage
    {
        protected List<AttributeRecordInfo> attributeRecordList = new List<AttributeRecordInfo>();
        protected int isSale = RequestHelper.GetQueryString<int>("IsSale");

        protected void OffSaleButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("OffSaleProduct", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                ProductBLL.OffSaleProduct(intsForm);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("OffSaleRecord"), ShopLanguage.ReadLanguage("Product"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("OffSaleOK"), RequestHelper.RawUrl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("ReadProduct", PowerCheckType.Single);

                if (isSale < 0) isSale = 1;//默认为上架课程

                foreach (ProductClassInfo info in ProductClassBLL.ReadProductClassNamedList())
                {
                    this.ClassID.Items.Add(new ListItem(info.ClassName, "|" + info.ID + "|"));
                }
                this.ClassID.Items.Insert(0, new ListItem("所有分类", string.Empty));
                //this.BrandID.DataSource = ProductBrandBLL.ReadProductBrandCacheList();
                //this.BrandID.DataTextField = "Name";
                //this.BrandID.DataValueField = "ID";
                //this.BrandID.DataBind();
                //this.BrandID.Items.Insert(0, new ListItem("所有品牌", string.Empty));
                //this.BrandID.Text = RequestHelper.GetQueryString<string>("BrandID");
                this.ClassID.Text = RequestHelper.GetQueryString<string>("ClassID");
                this.Key.Text = RequestHelper.GetQueryString<string>("Key");
                this.IsSpecial.Text = RequestHelper.GetQueryString<string>("IsSpecial");
                this.IsNew.Text = RequestHelper.GetQueryString<string>("IsNew");
                this.IsHot.Text = RequestHelper.GetQueryString<string>("IsHot");
                this.IsTop.Text = RequestHelper.GetQueryString<string>("IsTop");
                ProductSearchInfo product = new ProductSearchInfo();
                product.Key = RequestHelper.GetQueryString<string>("Key");
                product.ClassID = RequestHelper.GetQueryString<string>("ClassID");
                product.InBrandID = RequestHelper.GetQueryString<string>("BrandID");
                product.IsSpecial = RequestHelper.GetQueryString<int>("IsSpecial");
                product.IsNew = RequestHelper.GetQueryString<int>("IsNew");
                product.IsHot = RequestHelper.GetQueryString<int>("IsHot");
                product.IsSale = isSale;
                product.IsTop = RequestHelper.GetQueryString<int>("IsTop");
                product.StartAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                product.EndAddDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndAddDate"));
                product.IsSpecialTestSetting = RequestHelper.GetQueryString<int>("IsSpecialTestSetting");
                //product.IsComplete = RequestHelper.GetQueryString<int>("IsComplete");
                List<ProductInfo> dataSource = ProductBLL.SearchProductList(base.CurrentPage, base.PageSize, product, ref this.Count);
                //把列表中的产品的更新时间属性都读取出来
                attributeRecordList = AttributeRecordBLL.ReadList("3", ProductBLL.ReadProductIdStr(dataSource));
                base.BindControl(dataSource, this.RecordList, this.MyPager);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ResponseHelper.Redirect(((((("TestCourse.aspx?Action=search&" + "Key=" + this.Key.Text + "&") + "ClassID=" + this.ClassID.Text + "&") + "IsSpecial=" + this.IsSpecial.Text + "&") + "IsNew=" + this.IsNew.Text + "&") + "IsHot=" + this.IsHot.Text + "&") + "IsTop=" + this.IsTop.Text + "&IsSale=" + RequestHelper.GetQueryString<int>("IsSale"));
        }

        protected void OnSaleButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("OnSaleProduct", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                ProductBLL.OnSaleProduct(intsForm);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("OnSaleRecord"), ShopLanguage.ReadLanguage("Product"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("OnSaleOK"), RequestHelper.RawUrl);
            }
        }

        protected int CalcItemNum(string itemString, char splitchar)
        {
            int resultNum = 0;
            if (!string.IsNullOrEmpty(itemString))
            {
                resultNum = itemString.Split(splitchar).Length;
                if (itemString.StartsWith(splitchar.ToString())) resultNum--;
                if (itemString.EndsWith(splitchar.ToString())) resultNum--;
            }
            return resultNum;
        }

        protected string GetSpecialTestTime(int companyID, int courseID)
        {
            StringBuilder resultHtml = new StringBuilder();
            TestSettingInfo testSetting = TestSettingBLL.ReadCompanyTestSetting(int.Parse(Eval("CompanyID").ToString()), int.Parse(Eval("ID").ToString()));
            if (testSetting != null)
            {
                resultHtml.AppendLine("<br>");
                resultHtml.AppendLine("<span style=\"margin-left:10px;\">" + testSetting.TestStartTime + "—" + testSetting.TestEndTime + "</span>");
            }
            return resultHtml.ToString();
        }
    }
}
