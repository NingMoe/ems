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
        /// 添加公司岗位计划信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static int AddCompanyPostPlan(CompanyPostPlanInfo Model)
        {
            return dal.AddCompanyPostPlan(Model);
        }


        /// <summary>
        /// 更新公司岗位计划信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static int UpdateCompanyPostPlan(CompanyPostPlanInfo Model)
        {
            return dal.UpdateCompanyPostPlan(Model);
        }


        /// <summary>
        /// 删除公司岗位计划信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteCompanyPostPlan(int Id)
        {
            return DeleteCompanyPostPlan(Id.ToString());
        }

        /// <summary>
        /// 删除公司岗位计划信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteCompanyPostPlan(string Id)
        {
            return dal.DeleteCompanyPostPlan(Id);
        }

        /// <summary>
        /// 读取公司岗位计划开始时间
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static DateTime ReadCompanyPostPlan(int CompanyId, int PostId)
        {
            return dal.ReadCompanyPostPlan(CompanyId, PostId);
        }


        /// <summary>
        /// 查询公司岗位计划记录
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
        /// 查询公司岗位计划记录 带分页
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
