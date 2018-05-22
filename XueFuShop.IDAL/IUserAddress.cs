using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IUserAddress
    {        
        int AddUserAddress(UserAddressInfo userAddress);
        void DeleteUserAddress(string strID, int userID);
        void DeleteUserAddressByUserID(string strUserID);
        UserAddressInfo ReadUserAddress(int id, int userID);
        List<UserAddressInfo> ReadUserAddressByUser(int userID);
        void UpdateUserAddress(UserAddressInfo userAddress);
        void UpdateUserAddressIsDefault(int isDefault, int userID);
    }
}
