using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.IBLL;
using XueFuShop.Common;
using XueFu.EntLib;
using XueFuShop.IDAL;
using XueFuShop.Models;

namespace XueFuShop.BLL
{
    public class TeacherBLL : BaseBLL<TeacherInfo>, ITeacherBLL
    {
        private static readonly ITeacher dal = FactoryHelper.Instance<ITeacher>(Global.DataProvider, "TeacherDAL");

        public override void SetDal()
        {
            base.baseDAL = dal;
        }
    }
}
