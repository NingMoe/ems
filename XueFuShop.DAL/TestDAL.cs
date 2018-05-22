using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.IDAL;

namespace XueFuShop.DAL
{
    public class TestDAL : BaseDAL<GiftInfo>, ITest
    {
        public override void PrepareCondition(MssqlCondition mssqlCondition, GiftInfo teacher)
        {
            mssqlCondition.Add("[UserName]", teacher.Name, ConditionType.Like);
        }
    }
}
