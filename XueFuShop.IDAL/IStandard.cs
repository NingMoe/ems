using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IStandard
    {
        int AddStandard(StandardInfo standard);
        void DeleteStandard(string strID);
        List<StandardInfo> ReadStandardAllList();
        void UpdateStandard(StandardInfo standard);
    }
}
