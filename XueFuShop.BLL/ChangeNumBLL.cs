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
        /// ��ӹ�˾��¼��Ϣ
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static int AddCompanyRecord(CompanyRecordInfo Model)
        {
            return dal.AddCompanyRecord(Model);
        }


        /// <summary>
        /// ���¹�˾��¼��Ϣ
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static int UpdateCompanyRecord(CompanyRecordInfo Model)
        {
            return dal.UpdateCompanyRecord(Model);
        }


        /// <summary>
        /// ɾ����˾��¼��Ϣ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteCompanyRecord(int Id)
        {
            return DeleteCompanyRecord(Id.ToString());
        }

        /// <summary>
        /// ɾ����˾��¼��Ϣ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteCompanyRecord(string Id)
        {
            return dal.DeleteCompanyRecord(Id);
        }

        /// <summary>
        /// ��ѯ��˾�仯����
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public static int CompanyChangeNum(int CompanyId)
        {
            return dal.CompanyChangeNum(CompanyId);
        }

        /// <summary>
        /// ��ѯʱ����ڹ�˾�仯����
        /// </summary>
        /// <param name="CompanyId">��˾Id</param>
        /// <param name="StartDate">��ʼ����</param>
        /// <param name="EndDate">��������</param>
        /// <returns></returns>
        public static int CompanyChangeNum(int CompanyId, DateTime StartDate, DateTime EndDate)
        {
            return dal.CompanyChangeNum(CompanyId, StartDate, EndDate);
        }

        /// <summary>
        /// ��ѯ��˾����λʱ����ڱ仯����
        /// </summary>
        /// <param name="CompanyId">��˾Id</param>
        /// <param name="StartDate">��ʼ����</param>
        /// <param name="EndDate">��������</param>
        /// <returns></returns>
        public static int CompanyChangeNum(int CompanyId, int PostId, DateTime StartDate, DateTime EndDate)
        {
            return dal.CompanyChangeNum(CompanyId, PostId, StartDate, EndDate);
        }

        /// <summary>
        /// ��ѯ��˾�仯��¼ ����ҳ
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
        /// ��Ӹ��˼�¼��Ϣ
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static int AddUserRecord(ProRecordInfo Model)
        {
            return UserDal.AddUserRecord(Model);
        }


        /// <summary>
        /// ���¸��˼�¼��Ϣ
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static int UpdateUserRecord(ProRecordInfo Model)
        {
            return UserDal.UpdateUserRecord(Model);
        }


        /// <summary>
        /// ɾ�����˼�¼��Ϣ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteUserRecord(int Id)
        {
            return DeleteUserRecord(Id.ToString());
        }

        /// <summary>
        /// ɾ�����˼�¼��Ϣ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteUserRecord(string Id)
        {
            return UserDal.DeleteUserRecord(Id);
        }

        /// <summary>
        /// ��ѯ���˱仯����
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public static int UserChangeNum(int UserId)
        {
            return UserDal.UserChangeNum(UserId);
        }

        /// <summary>
        /// ��ѯʱ����ڸ��˱仯����
        /// </summary>
        /// <param name="CompanyId">��˾Id</param>
        /// <param name="StartDate">��ʼ����</param>
        /// <param name="EndDate">��������</param>
        /// <returns></returns>
        public static int UserChangeNum(int UserId, DateTime StartDate, DateTime EndDate)
        {
            return UserDal.UserChangeNum(UserId, StartDate, EndDate);
        }

        /// <summary>
        /// ��ѯ���˱仯��¼ ����ҳ
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
