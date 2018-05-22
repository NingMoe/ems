using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class UserAddress : UserBasePage
    {
        
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
        protected UserAddressInfo userAddress = new UserAddressInfo();
        protected List<UserAddressInfo> userAddressList = new List<UserAddressInfo>();

        
        protected override void PageLoad()
        {
            base.PageLoad();
            this.userAddressList = UserAddressBLL.ReadUserAddressByUser(base.UserID);
            this.singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();
            string queryString = RequestHelper.GetQueryString<string>("Action");
            if (queryString != null)
            {
                if (!(queryString == "Update"))
                {
                    if (queryString == "Delete")
                    {
                        UserAddressBLL.DeleteUserAddress(RequestHelper.GetQueryString<string>("ID"), base.UserID);
                        ResponseHelper.Write("ok");
                        ResponseHelper.End();
                    }
                }
                else
                {
                    int id = RequestHelper.GetQueryString<int>("ID");
                    this.userAddress = UserAddressBLL.ReadUserAddress(id, base.UserID);
                    this.singleUnlimitClass.ClassID = this.userAddress.RegionID;
                }
            }
        }

        protected override void PostBack()
        {
            UserAddressInfo userAddress = new UserAddressInfo();
            userAddress.ID = RequestHelper.GetForm<int>("ID");
            userAddress.Consignee = StringHelper.AddSafe(RequestHelper.GetForm<string>("Consignee"));
            userAddress.RegionID = this.singleUnlimitClass.ClassID;
            userAddress.Address = StringHelper.AddSafe(RequestHelper.GetForm<string>("Address"));
            userAddress.ZipCode = StringHelper.AddSafe(RequestHelper.GetForm<string>("ZipCode"));
            userAddress.Tel = StringHelper.AddSafe(RequestHelper.GetForm<string>("Tel"));
            userAddress.Mobile = StringHelper.AddSafe(RequestHelper.GetForm<string>("Mobile"));
            userAddress.IsDefault = RequestHelper.GetForm<int>("IsDefault");
            userAddress.UserID = base.UserID;
            userAddress.UserName = base.UserName;
            if (userAddress.IsDefault == 1)
            {
                UserAddressBLL.UpdateUserAddressIsDefault(0, base.UserID);
            }
            string message = "��ӳɹ�";
            if (userAddress.ID == 0)
            {
                UserAddressBLL.AddUserAddress(userAddress);
            }
            else
            {
                UserAddressBLL.UpdateUserAddress(userAddress);
                message = "�޸ĳɹ�";
            }
            ScriptHelper.Alert(message, RequestHelper.RawUrl);
        }
    }

 

}
