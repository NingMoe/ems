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
    public class CompanyRecordDAL : ICompanyChangeNum
    {
        /// <summary>
        /// 添加记录信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int AddCompanyRecord(CompanyRecordInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into [" + ShopMssqlHelper.TablePrefix + "CompanyRecord] (CompanyId,ChangeNum,PostId,Reson) values(@CompanyId,@ChangeNum,@PostId,@Reson)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }


        /// <summary>
        /// 更新记录信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int UpdateCompanyRecord(CompanyRecordInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update [" + ShopMssqlHelper.TablePrefix + "CompanyRecord] set ChangeNum=" + Model.ChangeNum + ",Reason=" + Model.Reason + ",PostId=" + Model.PostId);
            return DbSQLHelper.ExecuteSql(sql.ToString());
        }

        /// <summary>
        /// 删除记录信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int DeleteCompanyRecord(string Id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Delete From [" + ShopMssqlHelper.TablePrefix + "CompanyRecord] Where Id in (" + Id + ")");
            return DbSQLHelper.ExecuteSql(sql.ToString());
        }

        /// <summary>
        /// 查询公司变化总数
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public int CompanyChangeNum(int CompanyId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Select Sum(ChangeNum) From [" + ShopMssqlHelper.TablePrefix + "CompanyRecord] Where CompanyId=" + CompanyId);
            object Result = DbSQLHelper.GetSingle(sql.ToString());
            if (object.Equals(Result, null))
            {
                Result = 0;
            }
            return int.Parse(Result.ToString());
        }

        /// <summary>
        /// 查询时间段内公司变化总数
        /// </summary>
        /// <param name="CompanyId">公司Id</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns></returns>
        public int CompanyChangeNum(int CompanyId, DateTime StartDate, DateTime EndDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Select Sum(ChangeNum) From [" + ShopMssqlHelper.TablePrefix + "CompanyRecord] Where CompanyId=" + CompanyId + " and CreateDate Between '" + StartDate + "' and '" + EndDate + "'");
            object Result = DbSQLHelper.GetSingle(sql.ToString());
            if (object.Equals(Result, null))
            {
                Result = 0;
            }
            return int.Parse(Result.ToString());
        }

        /// <summary>
        /// 查询时间段内公司变化总数
        /// </summary>
        /// <param name="CompanyId">公司Id</param>
        /// <param name="PostId">结束日期</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns></returns>
        public int CompanyChangeNum(int CompanyId, int PostId, DateTime StartDate, DateTime EndDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Select Sum(ChangeNum) From [" + ShopMssqlHelper.TablePrefix + "CompanyRecord] Where CompanyId=" + CompanyId + " and PostId like '%|" + PostId + "|%' and CreateDate Between '" + StartDate + "' and '" + EndDate + "'");
            object Result = DbSQLHelper.GetSingle(sql.ToString());
            if (object.Equals(Result, null))
            {
                Result = 0;
            }
            return int.Parse(Result.ToString());
        }

        /// <summary>
        /// 查询公司变化记录 带分页
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="RecordSearch"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CompanyRecordInfo> CompanyRecordList(int currentPage, int pageSize, CompanyRecordInfo RecordSearch, ref int count)
        {
            List<CompanyRecordInfo> RecordList = new List<CompanyRecordInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = ShopMssqlHelper.TablePrefix + "CompanyRecord";
            class2.Fields = "[CompanyID],[ChangeNum],[PostId],[Reason],[CreateDate]";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[CreateDate]";
            class2.OrderType = OrderType.Desc;
            this.PrepareCondition(class2.MssqlCondition, RecordSearch);
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader reader = class2.ExecuteReader())
            {
                this.PrepareUserModel(reader, RecordList);
            }
            return RecordList;
        }

        public void PrepareCondition(MssqlCondition mssqlCondition, CompanyRecordInfo RecordSearch)
        {
            mssqlCondition.Add("[CompanyId]", RecordSearch.CompanyId, ConditionType.In);
        }

        public void PrepareUserModel(SqlDataReader dr, List<CompanyRecordInfo> CompanyRecordList)
        {
            while (dr.Read())
            {
                CompanyRecordInfo Item = new CompanyRecordInfo();
                Item.CompanyId = int.Parse(dr["ComapnyId"].ToString());
                Item.ChangeNum = int.Parse(dr["ChangeNum"].ToString());
                Item.PostId = dr["PostId"].ToString();
                Item.Reason = dr["Reason"].ToString();
                Item.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
                CompanyRecordList.Add(Item);
            }
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(CompanyRecordInfo Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@CompanyId",SqlDbType .Int),
                                    new SqlParameter ("@ChangeNum",SqlDbType .Int),
                                    new SqlParameter ("@PostId",SqlDbType .VarChar),
                                    new SqlParameter ("@Reason",SqlDbType .VarChar,50)
                                };
            par[0].Value = Model.CompanyId;
            par[1].Value = Model.ChangeNum;
            par[2].Value = Model.PostId;
            par[3].Value = Model.Reason;
            return par;
        }

    }





    public class ProRecordDAL : IUserChangeNum
    {
        /// <summary>
        /// 添加记录信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int AddUserRecord(ProRecordInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into [" + ShopMssqlHelper.TablePrefix + "ProRecord] (UserId,ChangeNum,Reson) values(@UserId,@ChangeNum,@Reson)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }


        /// <summary>
        /// 更新记录信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int UpdateUserRecord(ProRecordInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update [" + ShopMssqlHelper.TablePrefix + "ProRecord] set ChangeNum=" + Model.ChangeNum + ",Reason=" + Model.Reason);
            return DbSQLHelper.ExecuteSql(sql.ToString());
        }

        /// <summary>
        /// 删除记录信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int DeleteUserRecord(string Id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Delete From [" + ShopMssqlHelper.TablePrefix + "ProRecord] Where Id in (" + Id + ")");
            return DbSQLHelper.ExecuteSql(sql.ToString());
        }

        /// <summary>
        /// 查询个人变化总数
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public int UserChangeNum(int UserId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Select Sum(ChangeNum) From [" + ShopMssqlHelper.TablePrefix + "ProRecord] Where UserId=" + UserId);
            object Result = DbSQLHelper.GetSingle(sql.ToString());
            if (object.Equals(Result, null))
            {
                Result = 0;
            }
            return int.Parse(Result.ToString());
        }

        /// <summary>
        /// 查询时间段内公司变化总数
        /// </summary>
        /// <param name="CompanyId">公司Id</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns></returns>
        public int UserChangeNum(int UserId, DateTime StartDate, DateTime EndDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Select Sum(ChangeNum) From [" + ShopMssqlHelper.TablePrefix + "ProRecord] Where UserId=" + UserId + " and CreateDate Between '" + StartDate + "' and '" + EndDate + "'");
            object Result = DbSQLHelper.GetSingle(sql.ToString());
            if (object.Equals(Result, null))
            {
                Result = 0;
            }
            return int.Parse(Result.ToString());
        }


        /// <summary>
        /// 查询公司变化记录 带分页
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="RecordSearch"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ProRecordInfo> UserRecordList(int currentPage, int pageSize, ProRecordInfo RecordSearch, ref int count)
        {
            List<ProRecordInfo> RecordList = new List<ProRecordInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = ShopMssqlHelper.TablePrefix + "ProRecord";
            class2.Fields = "[Id],[UserID],[ChangeNum],[Reason],[CreateDate]";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[CreateDate]";
            class2.OrderType = OrderType.Desc;
            this.PrepareCondition(class2.MssqlCondition, RecordSearch);
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader reader = class2.ExecuteReader())
            {
                this.PrepareUserModel(reader, RecordList);
            }
            return RecordList;
        }

        public void PrepareCondition(MssqlCondition mssqlCondition, ProRecordInfo RecordSearch)
        {
            mssqlCondition.Add("[UserId]", RecordSearch.UserId, ConditionType.In);
        }

        public void PrepareUserModel(SqlDataReader dr, List<ProRecordInfo> ProRecordList)
        {
            while (dr.Read())
            {
                ProRecordInfo Item = new ProRecordInfo();
                Item.UserId = int.Parse(dr["UserId"].ToString());
                Item.ChangeNum = int.Parse(dr["ChangeNum"].ToString());
                Item.Reason = dr["Reason"].ToString();
                Item.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
                ProRecordList.Add(Item);
            }
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(ProRecordInfo Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@UserId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@ChangeNum",SqlDbType .VarChar,50),
                                    new SqlParameter ("@Reason",SqlDbType .VarChar,50)
                                };
            par[0].Value = Model.UserId;
            par[1].Value = Model.ChangeNum;
            par[2].Value = Model.Reason;
            return par;
        }
    }
}
