using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class CheckOutAddressAjax : AjaxBasePage
    {
        
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
        protected UserAddressInfo userAddress = new UserAddressInfo();

        
        protected override void PageLoad()
        {
            base.PageLoad();
            this.singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();
            this.singleUnlimitClass.FunctionName = "readShippingList()";
            int queryString = RequestHelper.GetQueryString<int>("ID");
            if (queryString > 0)
            {
                this.userAddress = UserAddressBLL.ReadUserAddress(queryString, base.UserID);
                this.singleUnlimitClass.ClassID = this.userAddress.RegionID;
            }
        }
    }

 

}
