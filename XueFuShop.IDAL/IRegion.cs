using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IRegion
    {        
        int AddRegion(RegionInfo region);
        void DeleteRegion(int id);
        void MoveDownRegion(int id);
        void MoveUpRegion(int id);
        List<RegionInfo> ReadRegionAllList();
        void UpdateRegion(RegionInfo region);
    }
}
