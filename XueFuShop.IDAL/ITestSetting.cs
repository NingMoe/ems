using System;
using XueFuShop.Models;
using System.Collections.Generic;

namespace XueFuShop.IDAL
{
    public interface ITestSetting
    {
        TestSettingInfo ReadTestSetting(int companyId);
        int AddTestSetting(TestSettingInfo Model);
        void UpdateTestSetting(TestSettingInfo Model);
        void DeleteTestSetting(int Id);
        List<TestSettingInfo> ReadList();
    }
}
