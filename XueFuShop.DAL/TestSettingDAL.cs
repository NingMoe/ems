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
    public sealed class DALTestSetting : ITestSetting
    {
        //读取信息
        public TestSettingInfo ReadTestSetting(int companyId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "TestSetting] where  CompanyId=" + companyId);
            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                return GetModel(reader);
            }
        }

        public int AddTestSetting(TestSettingInfo Model)
        {
            SqlParameter[] pt = (SqlParameter[])this.ValueParas(Model);
            return Convert.ToInt32(ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "AddTestSetting", pt));
        }

        public void UpdateTestSetting(TestSettingInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [" + ShopMssqlHelper.TablePrefix + "TestSetting] set [CourseId]=@courseId,[PaperScore]=@paperScore,[TestQuestionsCount]=@testQuestionsCount,[TestTimeLength]=@testTimeLength,[LowScore]=@lowScore,[TestStartTime]=@testStartTime,[TestEndTime]=@testEndTime,[CompanyId]=@companyId,[TestInterval]=@testInterval where [Id]=@id");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }

        public void DeleteTestSetting(int Id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Delete from [" + ShopMssqlHelper.TablePrefix + "TestSetting] where Id=" + Id);
            DbSQLHelper.ExecuteSql(sql.ToString());
        }

        public List<TestSettingInfo> ReadList()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "TestSetting]  Order by [Id] desc");
            List<TestSettingInfo> TempList = new List<TestSettingInfo>();
            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                this.PrepareModel(reader, TempList);
            }
            return TempList;
        }

        public List<TestSettingInfo> ReadList(TestSettingInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "TestSetting] ");
            List<TestSettingInfo> TempList = new List<TestSettingInfo>();
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, Model);

            if (mssqlCondition.ToString() != string.Empty)
            {
                sql.Append("where " + mssqlCondition.ToString());
                sql.Append(" Order by [Id] desc");
                using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
                {
                    this.PrepareModel(reader, TempList);
                }
            }

            return TempList;
        }


        public void PrepareCondition(MssqlCondition mssqlCondition, TestSettingInfo Model)
        {
            mssqlCondition.Add("[CompanyId]", Model.CompanyId, ConditionType.Equal);
        }


        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(TestSettingInfo Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@id",SqlDbType.Int),
                                    new SqlParameter ("@courseId",SqlDbType.Int),
                                    new SqlParameter ("@paperScore",SqlDbType.Int),
                                    new SqlParameter ("@testQuestionsCount",SqlDbType.Int),
                                    new SqlParameter ("@testTimeLength",SqlDbType.Int),
                                    new SqlParameter ("@lowScore",SqlDbType.Int),
                                    new SqlParameter ("@testStartTime",SqlDbType.DateTime),
                                    new SqlParameter ("@testEndTime",SqlDbType.DateTime),
                                    new SqlParameter ("@companyId",SqlDbType.Int),
                                    new SqlParameter("@testInterval",SqlDbType.Int)
                                };
            par[0].Value = Model.Id;
            par[1].Value = Model.CourseId;
            par[2].Value = Model.PaperScore;
            par[3].Value = Model.TestQuestionsCount;
            par[4].Value = Model.TestTimeLength;
            par[5].Value = Model.LowScore;
            if (Model.TestStartTime.HasValue)
            {
                par[6].Value = Model.TestStartTime;
            }
            else
            {
                par[6].Value = DBNull.Value;
            }
            if (Model.TestEndTime.HasValue)
            {
                par[7].Value = Model.TestEndTime;
            }
            else
            {
                par[7].Value = DBNull.Value;
            }
            par[8].Value = Model.CompanyId;
            par[9].Value = Model.TestInterval;
            return par;
        }

        public void PrepareModel(SqlDataReader dr, List<TestSettingInfo> List)
        {
            while (dr.Read())
            {
                TestSettingInfo Model = new TestSettingInfo();
                {
                    Model.Id = dr.GetInt32(0);
                    Model.CourseId = int.Parse(dr["CourseId"].ToString());
                    Model.PaperScore = dr.GetInt32(2);
                    Model.TestQuestionsCount = dr.GetInt32(3);
                    Model.TestTimeLength = dr.GetInt32(4);
                    if (dr["TestStartTime"] == DBNull.Value)
                    {
                        Model.TestStartTime = null;
                    }
                    else
                    {
                        Model.TestStartTime = Convert.ToDateTime(dr["TestStartTime"]);
                    }
                    if (dr["TestEndTime"] == DBNull.Value)
                    {
                        Model.TestEndTime = null;
                    }
                    else
                    {
                        Model.TestEndTime = Convert.ToDateTime(dr["TestEndTime"]);
                    }
                    Model.LowScore = dr.GetInt32(7);
                    Model.CompanyId = dr.GetInt32(8);
                    Model.TestInterval = dr.GetInt32(9);
                }
                List.Add(Model);
            }
        }


        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public TestSettingInfo GetModel(SqlDataReader dr)
        {
            TestSettingInfo Model = new TestSettingInfo();
            if (dr.Read())
            {
                Model.Id = dr.GetInt32(0);
                Model.CourseId = int.Parse(dr["CourseId"].ToString());
                Model.PaperScore = dr.GetInt32(2);
                Model.TestQuestionsCount = dr.GetInt32(3);
                Model.TestTimeLength = dr.GetInt32(4);
                //Model.TestStartTime = dr[6].ToString();
                //Model.TestEndTime = dr[7].ToString();
                if (dr["TestStartTime"] == DBNull.Value)
                {
                    Model.TestStartTime = null;
                }
                else
                {
                    Model.TestStartTime = Convert.ToDateTime(dr["TestStartTime"]);
                }
                if (dr["TestEndTime"] == DBNull.Value)
                {
                    Model.TestEndTime = null;
                }
                else
                {
                    Model.TestEndTime = Convert.ToDateTime(dr["TestEndTime"]);
                }
                Model.LowScore = dr.GetInt32(5);
                Model.CompanyId = dr.GetInt32(8);
                Model.TestInterval = dr.GetInt32(9);
            }
            return Model;
        }
    }
}
