using System;
using System.Collections.Generic;
using XueFuShop.Common;
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Admin
{
    public partial class RegionAjax : AdminBasePage
    {
        
        protected int id = 0;
        protected string name = string.Empty;
        protected List<RegionInfo> regionList = new List<RegionInfo>();

        
        protected void AddRegion()
        {
            base.CheckAdminPower("AddRegion", PowerCheckType.Single);
            RegionInfo region = new RegionInfo();
            region.FatherID = RequestHelper.GetQueryString<int>("FatherID");
            region.RegionName = RequestHelper.GetQueryString<string>("RegionName");
            int id = RegionBLL.AddRegion(region);
            AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Region"), id);
            ResponseHelper.End();
        }

        protected void DeleteRegion()
        {
            base.CheckAdminPower("DeleteRegion", PowerCheckType.Single);
            this.id = RequestHelper.GetQueryString<int>("ID");
            if (RegionBLL.ReadRegionChildList(this.id).Count > 0)
            {
                base.Response.Write("error");
                base.Response.End();
            }
            else
            {
                RegionBLL.DeleteRegion(this.id);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("Region"), this.id);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.ClearCache();
            string queryString = RequestHelper.GetQueryString<string>("Action");
            if (queryString != null)
            {
                if (!(queryString == "Read"))
                {
                    if (queryString == "Add")
                    {
                        this.AddRegion();
                    }
                    else if (queryString == "Delete")
                    {
                        this.DeleteRegion();
                    }
                    else if (queryString == "Update")
                    {
                        this.UpdateRegion();
                    }
                }
                else
                {
                    this.ReadRegion();
                }
            }
        }

        protected void ReadRegion()
        {
            base.CheckAdminPower("ReadRegion", PowerCheckType.Single);
            this.id = RequestHelper.GetQueryString<int>("ID");
            if (this.id > 0)
            {
                this.name = RegionBLL.ReadRegionCache(this.id).RegionName;
            }
            this.regionList = RegionBLL.ReadRegionChildList(this.id);
        }

        protected void UpdateRegion()
        {
            RegionInfo info;
            base.CheckAdminPower("UpdateRegion", PowerCheckType.Single);
            info = new RegionInfo();
            info.ID = RequestHelper.GetQueryString<int>("ID");
            info.FatherID = RegionBLL.ReadRegionCache(info.ID).FatherID;
            info.RegionName = RequestHelper.GetQueryString<string>("Name");
            RegionBLL.UpdateRegion(info);
            AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("Region"), info.ID);
            base.Response.End();
        }
    }
}
