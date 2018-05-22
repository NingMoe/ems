using System;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.BLL
{
    public class ChangeNumBLL
    {
        private static readonly ICompanyChangeNum dal = FactoryHelper.Instance<ICompanyChangeNum>(Global.DataProvider, "CompanyRecordDAL");
        private static readonly IUserChangeNum UserDal = FactoryHelper.Instance<IUserChangeNum>(Global.DataProvider, "ProRecordDAL");
       
        /// <summary>
        /// 添加公司记录信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static int AddCompanyRecord(CompanyRecordInfo Model)
        {
            return dal.AddCompanyRecord(Model);
        }


        /// <summary>
        /// 更新公司记录信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static int UpdateCompanyRecord(CompanyRecordInfo Model)
        {
            return dal.UpdateCompanyRecord(Model);
        }


        /// <summary>
        /// 删除公司记录信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteCompanyRecord(int Id)
        {
            return DeleteCompanyRecord(Id.ToString());
        }

        /// <summary>
        /// 删除公司记录信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteCompanyRecord(string Id)
        {
            return dal.DeleteCompanyRecord(Id);
        }

        /// <summary>
        /// 查询公司变化总数
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public static int CompanyChangeNum(int CompanyId)
        {
            return dal.CompanyChangeNum(CompanyId);
        }

        /// <summary>
        /// 查询时间段内公司变化总数
        /// </summary>
        /// <param name="CompanyId">公司Id</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns></returns>
        public static int CompanyChangeNum(int CompanyId, DateTime StartDate, DateTime EndDate)
        {
            return dal.CompanyChangeNum(CompanyId, StartDate, EndDate);
        }

        /// <summary>
        /// 查询公司及岗位时间段内变化总数
        /// </summary>
        /// <param name="CompanyId">公司Id</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns></returns>
        public static int CompanyChangeNum(int CompanyId, int PostId, DateTime StartDate, DateTime EndDate)
        {
            return dal.CompanyChangeNum(CompanyId, PostId, StartDate, EndDate);
        }

        /// <summary>
        /// 查询公司变化记录 带分页
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="RecordSearch"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<CompanyRecordInfo> CompanyRecordList(int currentPage, int pageSize, CompanyRecordInfo RecordSearch, ref int count)
        {
            return dal.CompanyRecordList(currentPage, pageSize, RecordSearch,ref count);
        }



        /// <summary>
        /// 添加个人记录信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static int AddUserRecord(ProRecordInfo Model)
        {
            return UserDal.AddUserRecord(Model);
        }


        /// <summary>
        /// 更新个人记录信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static int UpdateUserRecord(ProRecordInfo Model)
        {
            return UserDal.UpdateUserRecord(Model);
        }


        /// <summary>
        /// 删除个人记录信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteUserRecord(int Id)
        {
            return DeleteUserRecord(Id.ToString());
        }

        /// <summary>
        /// 删除个人记录信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteUserRecord(string Id)
        {
            return UserDal.DeleteUserRecord(Id);
        }

        /// <summary>
        /// 查询个人变化总数
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public static int UserChangeNum(int UserId)
        {
            return UserDal.UserChangeNum(UserId);
        }

        /// <summary>
        /// 查询时间段内个人变化总数
        /// </summary>
        /// <param name="CompanyId">公司Id</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns></returns>
        public static int UserChangeNum(int UserId, DateTime StartDate, DateTime EndDate)
        {
            return UserDal.UserChangeNum(UserId, StartDate, EndDate);
        }

        /// <summary>
        /// 查询个人变化记录 带分页
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="RecordSearch"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<ProRecordInfo> UserRecordList(int currentPage, int pageSize, ProRecordInfo RecordSearch, ref int count)
        {
            return UserDal.UserRecordList(currentPage, pageSize, RecordSearch, ref count);
        }
    }
}
