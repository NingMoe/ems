using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.IDAL;
using XueFu.EntLib;
using XueFuShop.Models;

namespace XueFuShop.DAL
{
    public sealed class TeacherDAL : BaseDAL<TeacherInfo>, ITeacher
    {
        //public override void PrepareCondition<T>(MssqlCondition mssqlCondition, T t)
        ////public override void PrepareCondition(MssqlCondition mssqlCondition, TeacherSearchInfo teacher)
        //{
        //    mssqlCondition.Add("[ID]",  teacher.InID, ConditionType.In);
        //    mssqlCondition.Add("[Name]",  teacher.Name, ConditionType.Like);
        //}
    }
}
