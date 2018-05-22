using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFuShop.IDAL;
using XueFuShop.Common;
using XueFu.EntLib;
using XueFuShop.IBLL;

namespace XueFuShop.BLL
{
    public class TestBLL : BaseBLL<GiftInfo>, ITestBLL
    {
        private static readonly ITest dal = FactoryHelper.Instance<ITest>(Global.DataProvider, "TestDAL");

        public override void SetDal()
        {
            base.baseDAL = dal;
        }
    }
}
