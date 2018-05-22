using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{    
    public interface IUserGrade
    {        
        int AddUserGrade(UserGradeInfo userGrade);
        void DeleteUserGrade(string strID);
        List<UserGradeInfo> ReadUserGradeAllList();
        void UpdateUserGrade(UserGradeInfo userGrade);
    }
}
