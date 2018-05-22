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
    public sealed class CourseDAL : ICourse
    {
        //读取指定信息
        public CourseInfo ReadCourse(int Id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "Course] where  CourseId="+Id);
            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                return GetModel(reader);
            }
        }

        public int AddCourse(CourseInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into [" + ShopMssqlHelper.TablePrefix + "Course] ( CourseName,CourseCode,CourseScore,CateId,OrderIndex,CompanyId,BrandId ) values(@CourseName,@CourseCode,@CourseScore,@CateId,@OrderIndex,@CompanyId,@BrandId)");
            sql.Append("SELECT SCOPE_IDENTITY()");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }

        public void UpdateCourse(CourseInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [" + ShopMssqlHelper.TablePrefix + "Course] set  CourseName=@CourseName,CourseCode=@CourseCode,CourseScore=@CourseScore,CateId=@CateId,OrderIndex=@OrderIndex,CompanyId=@CompanyId,BrandId=@BrandId where CourseId=@CourseId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }

        public void UpdateCourse(string courseID, int parentID)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [" + ShopMssqlHelper.TablePrefix + "Course] Set [CateId]=" + parentID + " Where [CourseId] in (" + courseID + ")");
            DbSQLHelper.ExecuteSql(sql.ToString());
        }

        public void DeleteCourse(int Id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Delete from [" + ShopMssqlHelper.TablePrefix + "Course] where CourseId=" + Id);
            DbSQLHelper.ExecuteSql(sql.ToString());
        }

        public void DeleteCourseByCatId(int Id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Delete from [" + ShopMssqlHelper.TablePrefix + "Course] where CateId=" + Id);
            DbSQLHelper.ExecuteSql(sql.ToString());
        }

        public List<CourseInfo> ReadList()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "Course]");
            List<CourseInfo> TempList = new List<CourseInfo>();
            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                this.PrepareModel(reader, TempList);
            }
            return TempList;
        }

        public List<CourseInfo> ReadList(CourseInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "Course] ");
            List<CourseInfo> TempList = new List<CourseInfo>();
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, Model);

            if (mssqlCondition.ToString() != string.Empty)
            {
                sql.Append("where " + mssqlCondition.ToString());
                sql.Append(" Order by [CourseId] desc");
                using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
                {
                    this.PrepareModel(reader, TempList);
                }
            }

            return TempList;
        }

        public List<CourseInfo> ReadList(int currentPage, int pageSize, ref int count)
        {
            List<CourseInfo> TempList = new List<CourseInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = "[" + ShopMssqlHelper.TablePrefix + "Course]";
            class2.Fields = "*";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[CourseId]";
            class2.OrderType = OrderType.Desc;
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader reader = class2.ExecuteReader())
            {
                this.PrepareModel(reader, TempList);
            }
            return TempList;
        }

        public List<CourseInfo> ReadList(CourseInfo Model, int currentPage, int pageSize, ref int count)
        {
            List<CourseInfo> TempList = new List<CourseInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = "[" + ShopMssqlHelper.TablePrefix + "Course]";
            class2.Fields = "*";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[CourseId]";
            class2.OrderType = OrderType.Desc;
            this.PrepareCondition(class2.MssqlCondition, Model);
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader reader = class2.ExecuteReader())
            {
                this.PrepareModel(reader, TempList);
            }
            return TempList;
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(CourseInfo Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@CourseId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@CourseName",SqlDbType .VarChar,50),
                                    new SqlParameter ("@CourseCode",SqlDbType .VarChar,50),
                                    new SqlParameter ("@CourseScore",SqlDbType .VarChar,50),
                                    new SqlParameter ("@CateId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@OrderIndex",SqlDbType .VarChar,50),
                                    new SqlParameter ("@CreateDate",SqlDbType .VarChar,50),
                                    new SqlParameter ("@CompanyId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@BrandId",SqlDbType .VarChar,50)

                                };
            par[0].Value = Model.CourseId;
            par[1].Value = Model.CourseName;
            par[2].Value = Model.CourseCode;
            par[3].Value = Model.CourseScore;
            par[4].Value = Model.CateId;
            par[5].Value = Model.OrderIndex;
            par[6].Value = Model.CreateDate;
            par[7].Value = Model.CompanyId;
            par[8].Value = Model.BrandId;
            return par;
        }

        public void PrepareCondition(MssqlCondition mssqlCondition, CourseInfo Model)
        {
            //mssqlCondition.Add("[CourseId]", Model.Condition, ConditionType.Like);
            mssqlCondition.Add("[CourseName]", Model.CourseName, ConditionType.Like);
            mssqlCondition.Add("[CateId]", Model.CateId, ConditionType.Equal);
            mssqlCondition.Add("[CateId]", Model.CateIdCondition, ConditionType.In);
            if (Model.Field != string.Empty)
            {
                mssqlCondition.Add("[" + Model.Field + "]", Model.Condition, ConditionType.In);
            }
        }

        public void PrepareModel(SqlDataReader dr, List<CourseInfo> List)
        {
            while (dr.Read())
            {
                CourseInfo Model = new CourseInfo();
                {
                    Model.CourseId = dr.GetInt32(0);
                    Model.CourseName = dr[1].ToString();
                    Model.CourseCode = dr[2].ToString();
                    Model.CourseScore = dr.GetDecimal(3); ;
                    Model.CateId = dr.GetInt32(4);
                    Model.OrderIndex = dr.GetInt32(5);
                    Model.CreateDate = dr.GetDateTime(6);
                    Model.CompanyId = dr.GetInt32(7);
                    Model.BrandId = dr["BrandId"].ToString();
                }
                List.Add(Model);
            }
        }


        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public CourseInfo GetModel(SqlDataReader dr)
        {
            CourseInfo Model = new CourseInfo();
            if (dr.Read())
            {
                Model.CourseId = dr.GetInt32(0);
                Model.CourseName = dr[1].ToString();
                Model.CourseCode = dr[2].ToString();
                Model.CourseScore = dr.GetDecimal(3); ;
                Model.CateId = dr.GetInt32(4);
                Model.OrderIndex = dr.GetInt32(5);
                Model.CreateDate = dr.GetDateTime(6);
                Model.CompanyId = dr.GetInt32(7);
                Model.BrandId = dr["BrandId"].ToString();
            }
            return Model;
        }
    }
}
