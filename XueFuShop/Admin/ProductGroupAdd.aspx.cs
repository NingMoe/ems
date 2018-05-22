using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using XueFuShop.Pages;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFu.EntLib;

namespace XueFuShop.Admin
{
    public partial class ProductGroupAdd : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                foreach (ProductClassInfo info in ProductClassBLL.ReadProductClassNamedList())
                {
                    this.RelationClassID.Items.Add(new ListItem(info.ClassName, "|" + info.ID + "|"));
                }
                this.RelationClassID.Items.Insert(0, new ListItem("���з���", string.Empty));
                List<ProductBrandInfo> list = ProductBrandBLL.ReadProductBrandCacheList();
                this.RelationBrandID.DataSource = list;
                this.RelationBrandID.DataTextField = "Name";
                this.RelationBrandID.DataValueField = "ID";
                this.RelationBrandID.DataBind();
                this.RelationBrandID.Items.Insert(0, new ListItem("ѡ��Ʒ��", string.Empty));
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
    {
        object obj2;
        string text = this.Photo.Text;
        string str2 = this.Link.Text;
        string form = RequestHelper.GetForm<string>("RelationProductID");
        string str4 = string.Empty;
        string str5 = string.Empty;
        List<ProductInfo> list = new List<ProductInfo>();
        if (form != string.Empty)
        {
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.InProductID = form;
            productSearch.IsSale = 1;
            list = ProductBLL.SearchProductList(productSearch);
            foreach (ProductInfo info2 in list)
            {
                str4 = str4 + info2.Name.Replace(",", "") + ",";
                str5 = str5 + info2.Photo.Replace(",", "") + ",";
            }
            if (str4.EndsWith(","))
            {
                str4 = str4.Substring(0, str4.Length - 1);
                str5 = str5.Substring(0, str5.Length - 1);
            }
        }
        string queryString = RequestHelper.GetQueryString<string>("Action");
        int num = RequestHelper.GetQueryString<int>("ID");
        int num2 = RequestHelper.GetQueryString<int>("ThemeActivityID");
        string str7 = "<script language=\"javascript\" type=\"text/javascript\"> var DG = frameElement.lhgDG;";
        if (queryString == "Update")
        {
            obj2 = str7;
            str7 = string.Concat(new object[] { 
                obj2, "DG.iWin(\"ThemeActivityAdd", num2, "\").updateProductGroup('", text, "','", str2, "','", form, "','", str4, "','", str5, "',", num, ",", 
                num2, ");"
             });
        }
        else
        {
            obj2 = str7;
            str7 = string.Concat(new object[] { obj2, "DG.iWin(\"ThemeActivityAdd", num2, "\").addProductGroup('", text, "','", str2, "','", form, "','", str4, "','", str5, "',", num2, ");" });
        }
        ResponseHelper.Write(str7 + "DG.cancel();</script>");
        ResponseHelper.End();
    }
    }
}
