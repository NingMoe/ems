using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.DAL
{
    public class CompanyPostPlanDAL : ICompanyPostPlan
    {

        public int AddCompanyPostPlan(CompanyPostPlanInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into [" + ShopMssqlHelper.TablePrefix + "CompanyPostPlan] (CompanyId,StartDate,PostId) values(@CompanyId,@StartDate,@PostId)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }

        /// <summary>
        /// 更新记录信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int UpdateCompanyPostPlan(CompanyPostPlanInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update [" + ShopMssqlHelper.TablePrefix + "CompanyPostPlan] set StartDate=" + Model.StartDate + ",PostId=" + Model.PostId);
            return DbSQLHelper.ExecuteSql(sql.ToString());
        }

        /// <summary>
        /// 删除记录信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int DeleteCompanyPostPlan(string Id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Delete From [" + ShopMssqlHelper.TablePrefix + "CompanyPostPlan] Where Id in (" + Id + ")");
            return DbSQLHelper.ExecuteSql(sql.ToString());
        }

        /// <summary>
        /// 读取公司岗位开始时间
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public DateTime ReadCompanyPostPlan(int CompanyId, int PostId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Select top 1 StartDate From [" + ShopMssqlHelper.TablePrefix + "CompanyPostPlan] Where CompanyId in (" + CompanyId + ") and PostId Like '%|" + PostId + "|%' order by Id desc");
            object Result = DbSQLHelper.GetSingle(sql.ToString());
            if (object.Equals(Result, null))
            {
                Result = DateTime.MinValue;
            }
            return Convert.ToDateTime(Result.ToString());
        }

        public List<CompanyPostPlanInfo> CompanyPostPlanList(CompanyPostPlanInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "CompanyPostPlan] ");
            List<CompanyPostPlanInfo> CompanyPostPlanList = new List<CompanyPostPlanInfo>();
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, Model);

            if (mssqlCondition.ToString() != string.Empty)
            {
                sql.Append("where " + mssqlCondition.ToString());
                sql.Append(" Order by StartDate");
                using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
                {
                    this.PrepareModel(reader, CompanyPostPlanList);
                }
            }
            return CompanyPostPlanList;
        }

        /// <summary>
        /// 查询公司变化记录 带分页
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="RecordSearch"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CompanyPostPlanInfo> CompanyPostPlanList(int currentPage, int pageSize, CompanyPostPlanInfo RuleSearch, ref int count)
        {
            List<CompanyPostPlanInfo> RuleList = new List<CompanyPostPlanInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = ShopMssqlHelper.TablePrefix + "CompanyPostPlan";
            class2.Fields = "*";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[Id]";
            class2.OrderType = OrderType.Desc;
            this.PrepareCondition(class2.MssqlCondition, RuleSearch);
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader reader = class2.ExecuteReader())
            {
                this.PrepareModel(reader, RuleList);
            }
            return RuleList;
        }

        public void PrepareCondition(MssqlCondition mssqlCondition, CompanyPostPlanInfo RuleSearch)
        {
            mssqlCondition.Add("[CompanyId]", RuleSearch.CompanyId, ConditionType.Equal);
            mssqlCondition.Add("[PostId]", "|" + RuleSearch.PostId + "|", ConditionType.Like);
        }

        public void PrepareModel(SqlDataReader dr, List<CompanyPostPlanInfo> CompanyPostPlanList)
        {
            while (dr.Read())
            {
                CompanyPostPlanInfo Item = new CompanyPostPlanInfo();
                Item.Id = int.Parse(dr["Id"].ToString());
                Item.CompanyId = int.Parse(dr["ComapnyId"].ToString());
                Item.StartDate = DateTime.Parse(dr["StartDate"].ToString());
                Item.PostId = dr["PostId"].ToString();
                Item.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
                CompanyPostPlanList.Add(Item);
            }
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(CompanyPostPlanInfo Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@CompanyId",SqlDbType .Int),
                                    new SqlParameter ("@StartDate",SqlDbType .VarChar,50),
                                    new SqlParameter ("@PostId",SqlDbType .VarChar,50)
                                };
            par[0].Value = Model.CompanyId;
            par[1].Value = Model.StartDate;
            par[2].Value = Model.PostId;
            return par;
        }


        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public CompanyPostPlanInfo GetModel(SqlDataReader dr)
        {
            CompanyPostPlanInfo Model = new CompanyPostPlanInfo();
            if (dr.HasRows)
            {
                Model.Id = int.Parse(dr["Id"].ToString());
                Model.CompanyId = int.Parse(dr["CompanyId"].ToString());
                Model.StartDate = DateTime.Parse(dr["StartDate"].ToString());
                Model.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());

                return Model;
            }
            else
            {
                return null;
            }
        }
    }
}
