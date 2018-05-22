using System;
using System.Data;
using XueFuShop.Pages;
using XueFu.EntLib;
using System.Collections.Generic;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace XueFuShop.Admin
{
    public partial class Ad : AdminBasePage
    {
        protected int classID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("ReadAd", PowerCheckType.Single);
                this.classID = RequestHelper.GetQueryString<int>("ClassID");
                if (this.classID == -2147483648)
                {
                    this.classID = 1;
                }
                List<AdInfo> dataSource = AdBLL.ReadAdList(this.classID, base.CurrentPage, base.PageSize, ref this.Count);
                base.BindControl(dataSource, this.RecordList, this.MyPager);
            }
        }
    }
}
