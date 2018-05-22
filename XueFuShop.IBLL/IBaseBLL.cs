using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.IBLL
{
    public interface IBaseBLL<T> where T : BaseModel
    {
        //bool Add<T>(T t) where T : BaseModel;
        //bool Update<T>(T model) where T : BaseModel;
        //bool Delete<T>(int id) where T : BaseModel;
        //T Read<T>(int id) where T : BaseModel;
        //List<T> ReadList<T>() where T : BaseModel;
        //List<T> ReadList<T>(T t, int currentPage, int pageSize, ref int count) where T : BaseModel;

        bool Add(T t);
        bool Update(T t);
        bool Delete(int id);
        T Read(int id);
        List<T> ReadList();
        List<T> ReadList<S>(S s);
        List<T> ReadList<S>(S s, int currentPage, int pageSize, ref int count);
    }
}
