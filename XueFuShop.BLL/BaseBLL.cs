using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFuShop.Common;
using XueFu.EntLib;
using XueFuShop.IDAL;
using XueFuShop.IBLL;

namespace XueFuShop.BLL
{
    public abstract class BaseBLL<T> : IBaseBLL<T>
        where T : BaseModel
    {
        //private static readonly IBaseDAL<T> dal = FactoryHelper.Instance<IBaseDAL<T>>(Global.DataProvider, "BaseDAL`1");

        public IBaseDAL<T> baseDAL = null;

        public BaseBLL()
        {
            SetDal();
        }
 
        public abstract void SetDal();

        public bool Add(T t)
        {
            return baseDAL.Add(t);
        }

        public bool Update(T t)
        {
            return baseDAL.Update(t);
        }

        public bool Delete(int id)
        {
            return baseDAL.Delete(id);
        }

        public T Read(int id)
        {
            return baseDAL.Read(id);
        }

        public List<T> ReadList()
        {
            return baseDAL.ReadList();
        }

        public List<T> ReadList<S>(S s)
        {
            return baseDAL.ReadList(s);
        }

        public List<T> ReadList<S>(S s, int currentPage, int pageSize, ref int count)
        {
            return baseDAL.ReadList(s, currentPage, pageSize, ref count);
        }
    }
}
