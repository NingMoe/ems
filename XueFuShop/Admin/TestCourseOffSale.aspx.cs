using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class TestCourseOffSale : AdminBasePage
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("ReadProduct", PowerCheckType.Single);
                foreach (ProductClassInfo info in ProductClassBLL.ReadProductClassNamedList())
                {
                    this.ClassID.Items.Add(new ListItem(info.ClassName, "|" + info.ID + "|"));
                }
                this.ClassID.Items.Insert(0, new ListItem("���з���", string.Empty));
                //this.BrandID.DataSource = ProductBrandBLL.ReadProductBrandCacheList();
                //this.BrandID.DataTextField = "Name";
                //this.BrandID.DataValueField = "ID";
                //this.BrandID.DataBind();
                //this.BrandID.Items.Insert(0, new ListItem("����Ʒ��", string.Empty));
                //this.ClassID.Text = RequestHelper.GetQueryString<string>("ClassID");
                //this.BrandID.Text = RequestHelper.GetQueryString<string>("BrandID");
                //this.Key.Text = RequestHelper.GetQueryString<string>("Key");
                //this.StartAddDate.Text = RequestHelper.GetQueryString<string>("StartAddDate");
                //this.EndAddDate.Text = RequestHelper.GetQueryString<string>("EndAddDate");
                ProductSearchInfo product = new ProductSearchInfo();
                product.Key = RequestHelper.GetQueryString<string>("Key");
                product.ClassID = RequestHelper.GetQueryString<string>("ClassID");
                product.InBrandID = RequestHelper.GetQueryString<string>("BrandID");
                product.IsSale = 0;
                product.StartAddDate = RequestHelper.GetQueryString<DateTime>("StartAddDate");
                product.EndAddDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndAddDate"));

                base.PageSize = 10;
                List<ProductInfo> dataSource = ProductBLL.SearchProductList(base.CurrentPage, base.PageSize, product, ref this.Count);
                base.BindControl(dataSource, this.RecordList, this.MyPager);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ResponseHelper.Redirect(("TestCourseOffSale.aspx?Action=search&" + "Key=" + this.Key.Text + "&") + "ClassID=" + this.ClassID.Text);// + "&") + "BrandID=" + this.BrandID.Text + "&") + "StartAddDate=" + this.StartAddDate.Text + "&") + "EndAddDate=" + this.EndAddDate.Text
        }

        protected int CalcItemNum(string itemString)
        {
            int resultNum = 0;
            if (!string.IsNullOrEmpty(itemString))
            {
                resultNum = itemString.Split('|').Length;
                if (itemString.StartsWith("|")) resultNum--;
                if (itemString.EndsWith("|")) resultNum--;
            }
            return resultNum;
        }
    }
}
