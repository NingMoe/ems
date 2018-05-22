using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.IDAL;
using XueFuShop.Models;

namespace XueFuShop.DAL
{
    public class CompanyRuleDAL : ICompanyRule
    {
        public int AddCompanyRule(CompanyRuleInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into CompanyRule (CompanyId,CourseNum,Frequency,StartDate,EndDate,PostId) values(@CompanyId,@CourseNum,@Frequency,@StartDate,@EndDate,@PostId)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }

        /// <summary>
        /// 更新记录信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int UpdateCompanyRule(CompanyRuleInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update CompanyRule set CourseNum=" + Model.CourseNum + ",Frequency=" + Model.Frequency + ",StartDate=" + Model.StartDate + ",EndDate=" + Model.EndDate + ",PostId=" + Model.PostId);
            return DbSQLHelper.ExecuteSql(sql.ToString());
        }

        /// <summary>
        /// 删除记录信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int DeleteCompanyRule(string Id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Delete From CompanyRule Where Id in (" + Id + ")");
            return DbSQLHelper.ExecuteSql(sql.ToString());
        }

        public List<CompanyRuleInfo> CompanyRuleList(CompanyRuleInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "CompanyRule] ");
            List<CompanyRuleInfo> CompanyRuleList = new List<CompanyRuleInfo>();
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, Model);

            if (mssqlCondition.ToString() != string.Empty)
            {
                sql.Append("where " + mssqlCondition.ToString());
                sql.Append(" Order by StartDate");
                using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
                {
                    this.PrepareModel(reader, CompanyRuleList);
                }
            }
            return CompanyRuleList;
        }

        /// <summary>
        /// 查询公司变化记录 带分页
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="RecordSearch"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CompanyRuleInfo> CompanyRuleList(int currentPage, int pageSize, CompanyRuleInfo RuleSearch, ref int count)
        {
            List<CompanyRuleInfo> RuleList = new List<CompanyRuleInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = ShopMssqlHelper.TablePrefix + "CompanyRule";
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

        public void PrepareCondition(MssqlCondition mssqlCondition, CompanyRuleInfo RuleSearch)
        {
            mssqlCondition.Add("[CompanyId]", RuleSearch.CompanyId, ConditionType.Equal);
            mssqlCondition.Add("[PostId]", "|" + RuleSearch.PostId + "|", ConditionType.Like);
        }

        public void PrepareModel(SqlDataReader dr, List<CompanyRuleInfo> CompanyRuleList)
        {
            while (dr.Read())
            {
                CompanyRuleInfo Item = new CompanyRuleInfo();
                Item.CompanyId = int.Parse(dr["CompanyId"].ToString());
                Item.CourseNum = int.Parse(dr["CourseNum"].ToString());
                Item.Frequency = int.Parse(dr["Frequency"].ToString());
                Item.StartDate = DateTime.Parse(dr["StartDate"].ToString());
                Item.EndDate = DateTime.Parse(dr["EndDate"].ToString());
                Item.PostId = dr["PostId"].ToString();
                Item.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
                CompanyRuleList.Add(Item);
            }
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(CompanyRuleInfo Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@CompanyId",SqlDbType .Int),
                                    new SqlParameter ("@ChangeNum",SqlDbType .Int),
                                    new SqlParameter ("@Frequency",SqlDbType .VarChar,50),
                                    new SqlParameter ("@StartDate",SqlDbType .VarChar,50),
                                    new SqlParameter ("@EndDate",SqlDbType .VarChar,50),
                                    new SqlParameter ("@PostId",SqlDbType .VarChar,50)
                                };
            par[0].Value = Model.CompanyId;
            par[1].Value = Model.CourseNum;
            par[2].Value = Model.Frequency;
            par[3].Value = Model.StartDate;
            par[4].Value = Model.EndDate;
            par[5].Value = Model.PostId;
            return par;
        }
    }
}
