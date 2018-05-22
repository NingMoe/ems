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
    public sealed class CompanyDAL : ICompany
    {
        public int AddCompany(CompanyInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into [" + ShopMssqlHelper.TablePrefix + "Company] ( [CompanyName],[CompanySimpleName],[CompanyAddress],[CompanyTel],[CompanyPost],[BrandId],[GroupId],[State],[ParentId],[PostStartDate],[Sort],[UserNum],[IsTest],[EndDate],[Post]) values(@CompanyName,@CompanySimpleName,@CompanyAddress,@CompanyTel,@CompanyPost,@BrandId,@GroupId,@State,@ParentId,@PostStartDate,@Sort,@UserNum,@IsTest,@EndDate,@Post)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }


        public CompanyInfo ReadCompany(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "Company] where [CompanyId]=" + id);
            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                return GetModel(reader);
            }
        }

        public void UpdateCompany(CompanyInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update [" + ShopMssqlHelper.TablePrefix + "Company] set [CompanyName]=@CompanyName,[CompanySimpleName]=@CompanySimpleName,[CompanyAddress]=@CompanyAddress,[CompanyTel]=@CompanyTel,[CompanyPost]=@CompanyPost,[BrandId]=@BrandId,[GroupId]=@GroupId,[State]=@State,[ParentId]=@ParentId,[PostStartDate]=@PostStartDate,[Sort]=@Sort,[UserNum]=@UserNum,[IsTest]=@IsTest,[EndDate]=@EndDate,[Post]=@Post where [CompanyId]=@CompanyId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }

        public void UpdateCompany(string Field, string Value, string Id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update [" + ShopMssqlHelper.TablePrefix + "Company] set [" + Field + "]=" + Value + " where [CompanyId] in (" + Id + ")");
            DbSQLHelper.ExecuteSql(sql.ToString());
        }

        public void DeleteCompany(int Id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("delete from [" + ShopMssqlHelper.TablePrefix + "Company] where [CompanyId]=" + Id);
            DbSQLHelper.ExecuteSql(sql.ToString());
        }

        public List<CompanyInfo> ReadCompanyListByCompanyID(string companyID)
        {
            List<CompanyInfo> CompanyList = new List<CompanyInfo>();
            if (!string.IsNullOrEmpty(companyID) && companyID != ",")
            {
                StringBuilder sql = new StringBuilder();
                companyID = companyID.Replace(" ", "").Replace(",,", ",");
                sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "Company] ");
                sql.Append("where CompanyID in (" + companyID + ") Order By Sort,CompanyID Desc");
                using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
                {
                    this.PrepareModel(reader, CompanyList);
                }
            }
            return CompanyList;
        }

        public List<CompanyInfo> ReadCompanyListByParentID(string parentID)
        {
            List<CompanyInfo> CompanyList = new List<CompanyInfo>();
            if (!string.IsNullOrEmpty(parentID) && parentID != ",")
            {
                StringBuilder sql = new StringBuilder();
                parentID = parentID.Replace(" ", "").Replace(",,", ",");
                sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "Company] ");
                sql.Append("where [dbo].[" + ShopMssqlHelper.TablePrefix + "CompareSTR](ParentID,'" + parentID + "',',')=1 And State=0 Order By Sort,CompanyID Desc");
                using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
                {
                    this.PrepareModel(reader, CompanyList);
                }
            }
            return CompanyList;
        }

        public List<CompanyInfo> ReadCompanyList(CompanyInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "Company] ");
            List<CompanyInfo> CompanyList = new List<CompanyInfo>();
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, Model);

            if (mssqlCondition.ToString() != string.Empty)
            {
                sql.Append("where " + mssqlCondition.ToString());
                sql.Append(" Order by Sort,CompanyID Desc");
                using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
                {
                    this.PrepareModel(reader, CompanyList);
                }
            }
            return CompanyList;
        }

        public List<CompanyInfo> ReadCompanyList(int currentPage, int pageSize, ref int count)
        {
            List<CompanyInfo> CompanyList = new List<CompanyInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = "[" + ShopMssqlHelper.TablePrefix + "Company]";
            class2.Fields = "*";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[CompanyId]";
            class2.OrderType = OrderType.Desc;
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader reader = class2.ExecuteReader())
            {
                this.PrepareModel(reader, CompanyList);
            }
            return CompanyList;
        }

        public List<CompanyInfo> ReadCompanyList(CompanyInfo Model, int currentPage, int pageSize, ref int count)
        {
            List<CompanyInfo> CompanyList = new List<CompanyInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = "[" + ShopMssqlHelper.TablePrefix + "Company]";
            class2.Fields = "*";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[CompanyId]";
            class2.OrderType = OrderType.Desc;
            this.PrepareCondition(class2.MssqlCondition, Model);
            // class2.MssqlCondition.Add("[State]", State, ConditionType.Equal);
            class2.Count = count;
            count = class2.Count;
            //ResponseHelper.Write(class2.MssqlCondition.ToString());
            using (SqlDataReader reader = class2.ExecuteReader())
            {
                this.PrepareModel(reader, CompanyList);
            }
            return CompanyList;
        }


        public void PrepareCondition(MssqlCondition mssqlCondition, CompanyInfo Model)
        {
            mssqlCondition.Add("[CompanyName]", Model.CompanyName, ConditionType.Like);
            mssqlCondition.Add("[CompanySimpleName]", Model.CompanySimpleName, ConditionType.Like);
            mssqlCondition.Add("[CompanyTel]", Model.CompanyTel, ConditionType.Like);
            //mssqlCondition.Add("[CompanyAddress]", Model.CompanySimpleName, ConditionType.Like);
            mssqlCondition.Add("[State]", Model.State, ConditionType.Equal);
            //mssqlCondition.Add("[ParentId]", Model.ParentIdCondition, ConditionType.In);
            mssqlCondition.Add("[GroupId]", Model.GroupIdCondition, ConditionType.In);
            mssqlCondition.Add("[BrandId]", Model.BrandId, ConditionType.In);
            mssqlCondition.Add("[CompanyId]", Model.CompanyId, ConditionType.NoEqual);
            if (!string.IsNullOrEmpty(Model.ParentIdCondition))
                mssqlCondition.Add(" [dbo].[" + ShopMssqlHelper.TablePrefix + "CompareSTR](ParentId,'" + Model.ParentIdCondition + "',',')=1 ");
            if (Model.Field != string.Empty)
            {
                mssqlCondition.Add("[" + Model.Field + "]", Model.Condition, ConditionType.In);
            }
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(CompanyInfo Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@CompanyId",SqlDbType.Int),
                                    new SqlParameter ("@CompanyName",SqlDbType.VarChar),
                                    new SqlParameter ("@CompanySimpleName",SqlDbType.VarChar),
                                    new SqlParameter ("@CompanyAddress",SqlDbType .VarChar),
                                    new SqlParameter ("@CompanyTel",SqlDbType .VarChar),
                                    new SqlParameter ("@CompanyPost",SqlDbType .VarChar),
                                    new SqlParameter ("@BrandId",SqlDbType .VarChar),
                                    new SqlParameter ("@GroupId",SqlDbType .Int),
                                    new SqlParameter ("@State",SqlDbType.Int),
                                    new SqlParameter ("@ParentId",SqlDbType .VarChar),
                                    new SqlParameter ("@PostStartDate",SqlDbType .VarChar),
                                    new SqlParameter ("@Sort",SqlDbType .VarChar),
                                    new SqlParameter ("@UserNum",SqlDbType.Int),
                                    new SqlParameter ("@IsTest",SqlDbType.Bit),
                                    new SqlParameter ("@EndDate",SqlDbType.DateTime),
                                    new SqlParameter ("@Post",SqlDbType.VarChar)
                                };
            par[0].Value = Model.CompanyId;
            par[1].Value = Model.CompanyName;
            par[2].Value = Model.CompanySimpleName;
            par[3].Value = Model.CompanyAddress;
            par[4].Value = Model.CompanyTel;
            par[5].Value = Model.CompanyPost;
            par[6].Value = Model.BrandId;
            par[7].Value = Model.GroupId;
            par[8].Value = Model.State;
            par[9].Value = Model.ParentId;
            par[10].Value = Model.PostStartDate;
            par[11].Value = Model.Sort;
            par[12].Value = Model.UserNum;
            par[13].Value = Model.IsTest;
            par[14].Value = Model.EndDate;
            par[15].Value = Model.Post;
            return par;
        }


        public void PrepareModel(SqlDataReader dr, List<CompanyInfo> CompanyList)
        {
            while (dr.Read())
            {
                CompanyInfo Model = new CompanyInfo();
                {
                    Model.CompanyId = dr.GetInt32(0);
                    Model.CompanyName = dr[1].ToString();
                    Model.CompanySimpleName = dr[2].ToString();
                    Model.CompanyAddress = dr[3].ToString();
                    Model.CompanyTel = dr[4].ToString();
                    Model.CompanyPost = dr[5].ToString();
                    Model.BrandId = dr[6].ToString();
                    Model.GroupId = dr.GetInt32(7);
                    Model.State = dr.GetInt32(8);
                    Model.ParentId = dr["ParentId"].ToString();
                    Model.Sort = Convert.ToInt32(dr["Sort"]);
                    if (dr["PostStartDate"] == DBNull.Value) Model.PostStartDate = DBNull.Value;
                    else Model.PostStartDate = dr["PostStartDate"];
                    Model.UserNum = Convert.ToInt32(dr["UserNum"]);
                    Model.IsTest = Convert.ToBoolean(dr["IsTest"]);
                    Model.Post = dr["Post"].ToString();
                }
                CompanyList.Add(Model);
            }
        }


        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        public CompanyInfo GetModel(SqlDataReader dr)
        {
            CompanyInfo Model = new CompanyInfo();
            if (dr.Read())
            {
                Model.CompanyId = dr.GetInt32(0);
                Model.CompanyName = dr[1].ToString();
                Model.CompanySimpleName = dr[2].ToString();
                Model.CompanyAddress = dr[3].ToString();
                Model.CompanyTel = dr[4].ToString();
                Model.CompanyPost = dr[5].ToString();
                Model.BrandId = dr[6].ToString();
                Model.GroupId = dr.GetInt32(7);
                Model.State = dr.GetInt32(8);
                Model.ParentId = dr["ParentId"].ToString();
                Model.Sort = Convert.ToInt32(dr["Sort"]);
                if (dr["PostStartDate"] == DBNull.Value) Model.PostStartDate = DBNull.Value;
                else Model.PostStartDate = dr["PostStartDate"];
                Model.UserNum = Convert.ToInt32(dr["UserNum"]);
                Model.IsTest = Convert.ToBoolean(dr["IsTest"]);
                if (dr["EndDate"] == DBNull.Value) Model.EndDate = DBNull.Value;
                else Model.EndDate = Convert.ToDateTime(dr["EndDate"]);
                Model.Post = dr["Post"].ToString();
            }
            return Model;
        }
    }
}
