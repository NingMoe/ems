using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.IDAL;
using XueFuShop.Common;

namespace XueFuShop.BLL
{
    public class CompanyPostPlanBLL
    {
        private static readonly ICompanyPostPlan dal = FactoryHelper.Instance<ICompanyPostPlan>(Global.DataProvider, "CompanyPostPlanDAL");

        /// <summary>
        /// ��ӹ�˾��λ�ƻ���Ϣ
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static int AddCompanyPostPlan(CompanyPostPlanInfo Model)
        {
            return dal.AddCompanyPostPlan(Model);
        }


        /// <summary>
        /// ���¹�˾��λ�ƻ���Ϣ
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static int UpdateCompanyPostPlan(CompanyPostPlanInfo Model)
        {
            return dal.UpdateCompanyPostPlan(Model);
        }


        /// <summary>
        /// ɾ����˾��λ�ƻ���Ϣ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteCompanyPostPlan(int Id)
        {
            return DeleteCompanyPostPlan(Id.ToString());
        }

        /// <summary>
        /// ɾ����˾��λ�ƻ���Ϣ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteCompanyPostPlan(string Id)
        {
            return dal.DeleteCompanyPostPlan(Id);
        }

        /// <summary>
        /// ��ȡ��˾��λ�ƻ���ʼʱ��
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static DateTime ReadCompanyPostPlan(int CompanyId, int PostId)
        {
            return dal.ReadCompanyPostPlan(CompanyId, PostId);
        }


        /// <summary>
        /// ��ѯ��˾��λ�ƻ���¼
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="RecordSearch"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<CompanyPostPlanInfo> CompanyPostPlanList(CompanyPostPlanInfo RuleSearch)
        {
            return dal.CompanyPostPlanList(RuleSearch);
        }

        /// <summary>
        /// ��ѯ��˾��λ�ƻ���¼ ����ҳ
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="RecordSearch"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<CompanyPostPlanInfo> CompanyPostPlanList(int currentPage, int pageSize, CompanyPostPlanInfo RuleSearch, ref int count)
        {
            return dal.CompanyPostPlanList(currentPage, pageSize, RuleSearch, ref count);
        }
    }
}
