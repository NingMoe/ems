using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface IThemeActivity
    {
        int AddThemeActivity(ThemeActivityInfo themeActivity);
        void DeleteThemeActivity(string strID);
        ThemeActivityInfo ReadThemeActivity(int id);
        List<ThemeActivityInfo> ReadThemeActivityList(int currentPage, int pageSize, ref int count);
        void UpdateThemeActivity(ThemeActivityInfo themeActivity);
    }
}
