using System;
using System.Data;
using XueFuShop.Pages;
using System.Collections.Generic;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;

namespace XueFuShop.Admin
{
    public partial class AttributeRecordAjax : AdminBasePage
    {
        protected List<AttributeInfo> attributeList = new List<AttributeInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            base.ClearCache();
            int queryString = RequestHelper.GetQueryString<int>("AttributeClassID");
            int productID = RequestHelper.GetQueryString<int>("ProductID");
            this.attributeList = AttributeBLL.JoinAttribute(queryString, productID);
        }
    }
}
