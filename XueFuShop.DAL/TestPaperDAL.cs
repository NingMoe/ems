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
    public sealed class TestPaperDAL : ITestPaper
    {
        //读取信息
        public TestPaperInfo ReadPaper(TestPaperInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "TestPaper] where  TestPaperId=@TestPaperId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            using (SqlDataReader dr = DbSQLHelper.ExecuteReader(sql.ToString(), par))
            {
                return GetModel(dr);
            }
        }


        //读取信息
        public TestPaperInfo ReadPaper(int UserId, int CateId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select top 1 * from [" + ShopMssqlHelper.TablePrefix + "TestPaper] where  UserId=" + UserId + " and CateId=" + CateId + " and Del=0 Order by TestDate Desc");
            using (SqlDataReader dr = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                return GetModel(dr);
            }
        }

        //读取信息
        public TestPaperInfo ReadTheOldTestPaperInfo(int UserId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select top 1 * from [" + ShopMssqlHelper.TablePrefix + "TestPaper] where  UserId=" + UserId + " Order by TestDate");
            using (SqlDataReader dr = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                return GetModel(dr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int AddPaper(TestPaperInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into [" + ShopMssqlHelper.TablePrefix + "TestPaper] ( PaperName,CateId,QuestionStyle,QuestionId,Answer,Locked,CompanyId,UserId,Scorse,Point,IsPass ) values(@PaperName,@CateId,@QuestionStyle,@QuestionId,@Answer,@Locked,@CompanyId,@UserId,@Scorse,@Point,@IsPass)");
            sql.Append("SELECT @@identity");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }

        public void UpdatePaper(TestPaperInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [" + ShopMssqlHelper.TablePrefix + "TestPaper] set PaperName=@PaperName,CateId=@CateId,QuestionStyle=@QuestionStyle,QuestionId=@QuestionId,Answer=@Answer,Locked=@Locked,CompanyId=@CompanyId,UserId=@UserId,Scorse=@Scorse,Point=@Point,[IsPass]=@IsPass where TestPaperId=@TestPaperId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }


        public void UpdatePaperCompanyId(int UserId, int CompanyId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [" + ShopMssqlHelper.TablePrefix + "TestPaper] set CompanyId=" + CompanyId + " where UserId=" + UserId);
            DbSQLHelper.ExecuteSql(sql.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        public void DeletePaper(int Id)
        {
            StringBuilder sql = new StringBuilder();
            //sql.Append("Delete from [" + ShopMssqlHelper.TablePrefix + "TestPaper] where TestPaperId=" + Id);
            sql.Append("Update [" + ShopMssqlHelper.TablePrefix + "TestPaper] Set [Del]=1 where TestPaperId=" + Id);
            DbSQLHelper.ExecuteSql(sql.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        public void DeletePaperByUserID(string userID)
        {
            if (!string.IsNullOrEmpty(userID))
            {
                StringBuilder sql = new StringBuilder();
                //sql.Append("Delete from [" + ShopMssqlHelper.TablePrefix + "TestPaper] where UserId=" + UserId);
                sql.Append("Update [" + ShopMssqlHelper.TablePrefix + "TestPaper] Set [Del]=1 where UserId in (" + userID + ")");
                DbSQLHelper.ExecuteSql(sql.ToString());
            }
        }

        public void RecoveryPaperByUserID(string userID)
        {
            if (!string.IsNullOrEmpty(userID))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("Update [" + ShopMssqlHelper.TablePrefix + "TestPaper] Set [Del]=0 where UserId in (" + userID + ")");
                DbSQLHelper.ExecuteSql(sql.ToString());
            }
        }

        public Dictionary<int, float> ReadTheBestList(int userID, string courseID)
        {
            Dictionary<int, float> resultData = new Dictionary<int, float>();
            if (!string.IsNullOrEmpty(courseID))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from (");
                sql.Append("select [CateId],[Scorse],row_number() over(partition by [CateId] order by [Scorse] desc) as rownumber from [" + ShopMssqlHelper.TablePrefix + "TestPaper]  where [Del]=0 and [CateId] in (" + courseID + ") and [UserID]=" + userID);
                sql.Append(") as t where t.rownumber <=1");
                using (SqlDataReader dr = DbSQLHelper.ExecuteReader(sql.ToString()))
                {
                    while (dr.Read())
                    {
                        resultData.Add(int.Parse(dr["CateId"].ToString()), float.Parse(dr["Scorse"].ToString()));
                    }
                }
            }
            return resultData;
        }

        public List<TestPaperReportInfo> ReadThelatestList(int userID, string courseID)
        {
            List<TestPaperReportInfo> tempList = new List<TestPaperReportInfo>();
            if (!string.IsNullOrEmpty(courseID))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from (");
                sql.Append("select [UserID],[CateId],[PaperName],[Scorse],[TestDate],[IsPass],row_number() over(partition by [CateId] order by [TestDate] desc) as rownumber from [" + ShopMssqlHelper.TablePrefix + "TestPaper]  where [Del]=0 and [CateId] in (" + courseID + ") and [UserID]=" + userID);
                sql.Append(") as t where t.rownumber <=1");
                using (SqlDataReader dr = DbSQLHelper.ExecuteReader(sql.ToString()))
                {
                    while (dr.Read())
                    {
                        TestPaperReportInfo latestTestPaper = new TestPaperReportInfo();
                        latestTestPaper.UserID = int.Parse(dr["UserId"].ToString());
                        latestTestPaper.CourseID = int.Parse(dr["CateId"].ToString());
                        latestTestPaper.CourseName = dr["PaperName"].ToString();
                        latestTestPaper.Score = decimal.Parse(dr["Scorse"].ToString());
                        latestTestPaper.TestDate = Convert.ToDateTime(dr["TestDate"]);
                        latestTestPaper.IsPass = int.Parse(dr["IsPass"].ToString());
                        tempList.Add(latestTestPaper);
                    }
                }
            }
            return tempList;
        }

        public List<TestPaperReportInfo> ReadTheFirstRecordList(string companyID)
        {
            List<TestPaperReportInfo> tempList = new List<TestPaperReportInfo>();
            if (!string.IsNullOrEmpty(companyID))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from (");
                sql.Append("select [UserID],[CateId],[PaperName],[Scorse],[TestDate],[IsPass],row_number() over(partition by [UserID] order by [TestDate]) as rownumber from [" + ShopMssqlHelper.TablePrefix + "TestPaper]  where [companyID] in (" + companyID + ")");
                sql.Append(") as t where t.rownumber <=1");
                using (SqlDataReader dr = DbSQLHelper.ExecuteReader(sql.ToString()))
                {
                    while (dr.Read())
                    {
                        TestPaperReportInfo latestTestPaper = new TestPaperReportInfo();
                        latestTestPaper.UserID = int.Parse(dr["UserId"].ToString());
                        latestTestPaper.CourseID = int.Parse(dr["CateId"].ToString());
                        latestTestPaper.CourseName = dr["PaperName"].ToString();
                        latestTestPaper.Score = decimal.Parse(dr["Scorse"].ToString());
                        latestTestPaper.TestDate = Convert.ToDateTime(dr["TestDate"]);
                        latestTestPaper.IsPass = int.Parse(dr["IsPass"].ToString());
                        tempList.Add(latestTestPaper);
                    }
                }
            }
            return tempList;
        }


        public List<TestPaperInfo> ReadList(TestPaperInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(*) as TestNum,sum(point) as scorse");
            if (Model.GroupCondition != "") sql.Append("," + Model.GroupCondition);
            sql.Append(" from [" + ShopMssqlHelper.TablePrefix + "TestPaper] ");
            List<TestPaperInfo> TempList = new List<TestPaperInfo>();
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, Model);

            if (mssqlCondition.ToString() != string.Empty)
            {
                sql.Append("where " + mssqlCondition.ToString());
            }
            if (Model.GroupCondition != "") sql.Append(" Group by " + Model.GroupCondition);
            if (Model.GroupCondition == "cateid") sql.Append(" Order by Cateid ");
            using (SqlDataReader dr = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                //this.PrepareModel(reader, TempList);
                while (dr.Read())
                {
                    TestPaperInfo Model1 = new TestPaperInfo();
                    {
                        Model1.TestNum = dr.GetInt32(0);
                        Model1.Point = Convert.ToDecimal(dr[1].ToString());
                        Model1.UserId = dr.GetInt32(2);
                        Model1.CateId = dr.GetInt32(2);
                    }
                    TempList.Add(Model1);
                }
            }

            return TempList;
        }

        public List<TestPaperInfo> NewReadList(TestPaperInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "TestPaper] ");
            List<TestPaperInfo> TempList = new List<TestPaperInfo>();
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, Model);

            if (mssqlCondition.ToString() != string.Empty)
            {
                sql.Append("where " + mssqlCondition.ToString());
            }
            if (Model.OrderByCondition == string.Empty)
            {
                sql.Append(" Order by TestPaperId desc");
            }
            else
            {
                sql.Append(" Order by " + Model.OrderByCondition);
            }
            using (SqlDataReader dr = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                this.PrepareModel(dr, TempList);
            }

            return TempList;
        }

        public List<TestPaperInfo> ReadList(int currentPage, int pageSize, ref int count)
        {
            List<TestPaperInfo> TempList = new List<TestPaperInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = "[" + ShopMssqlHelper.TablePrefix + "TestPaper]";
            class2.Fields = "*";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[TestPaperId]";
            class2.OrderType = OrderType.Desc;
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader reader = class2.ExecuteReader())
            {
                this.PrepareModel(reader, TempList);
            }
            return TempList;
        }

        public List<TestPaperInfo> ReadList(TestPaperInfo Model, int currentPage, int pageSize, ref int count)
        {
            List<TestPaperInfo> TempList = new List<TestPaperInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = "[" + ShopMssqlHelper.TablePrefix + "TestPaper]";
            class2.Fields = "*";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[TestPaperId]";
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
        protected IDbDataParameter[] ValueParas(TestPaperInfo Model)
        {
            SqlParameter[] par ={
                new SqlParameter ("@TestPaperId",SqlDbType .VarChar),
                new SqlParameter ("@PaperName",SqlDbType .VarChar),
                new SqlParameter ("@CateId",SqlDbType .VarChar),
                new SqlParameter ("@QuestionStyle",SqlDbType .VarChar),
                new SqlParameter ("@QuestionId",SqlDbType .VarChar),
                new SqlParameter ("@Answer",SqlDbType .VarChar),
                new SqlParameter ("@Locked",SqlDbType .VarChar),
                new SqlParameter ("@CompanyId",SqlDbType.Int),
                new SqlParameter ("@UserId",SqlDbType .VarChar),
                new SqlParameter ("@Scorse",SqlDbType .VarChar),
                new SqlParameter ("@Point",SqlDbType .VarChar),
                new SqlParameter ("@IsPass",SqlDbType.Int)

                                };
            par[0].Value = Model.TestPaperId;
            par[1].Value = Model.PaperName;
            par[2].Value = Model.CateId;
            par[3].Value = Model.QuestionStyle;
            par[4].Value = Model.QuestionId;
            par[5].Value = Model.Answer;
            par[6].Value = Model.Locked;
            par[7].Value = Model.CompanyId;
            par[8].Value = Model.UserId;
            par[9].Value = Model.Scorse;
            par[10].Value = Model.Point;
            par[11].Value = Model.IsPass;

            return par;
        }

        public void PrepareCondition(MssqlCondition mssqlCondition, TestPaperInfo Model)
        {
            mssqlCondition.Add("[PaperName]", Model.PaperName, ConditionType.Like);
            mssqlCondition.Add("[Locked]", Model.Locked, ConditionType.Equal);
            mssqlCondition.Add("[TestDate]", Model.TestMinDate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[TestDate]", Model.TestMaxDate, ConditionType.LessOrEqual);
            mssqlCondition.Add("[UserId]", Model.UserId, ConditionType.Equal);
            mssqlCondition.Add("[UserId]", Model.UserIdCondition, ConditionType.In);
            mssqlCondition.Add("[CateId]", Model.CateIdCondition, ConditionType.In);
            mssqlCondition.Add("[CompanyId]", Model.CompanyIdCondition, ConditionType.In);
            mssqlCondition.Add("[Scorse]", Model.Scorse, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[Scorse]", Model.MaxScorse, ConditionType.Less);
            mssqlCondition.Add("[Del]", Model.Del, ConditionType.Equal);
            mssqlCondition.Add("[IsPass]", Model.IsPass, ConditionType.Equal);
            //if (Model.Field != string.Empty)
            //{
            //    mssqlCondition.Add("[" + Model.Field + "]", Model.Condition, ConditionType.In);
            //}
            if (!string.IsNullOrEmpty(Model.Condition))
                mssqlCondition.Add(Model.Condition);
        }

        public void PrepareModel(SqlDataReader dr, List<TestPaperInfo> List)
        {
            while (dr.Read())
            {
                TestPaperInfo Model = new TestPaperInfo();
                {
                    Model.TestPaperId = dr.GetInt32(0);
                    Model.PaperName = dr[1].ToString();
                    Model.CateId = dr.GetInt32(2);
                    Model.QuestionStyle = dr[3].ToString();
                    Model.QuestionId = dr[4].ToString();
                    Model.Answer = dr[5].ToString();
                    Model.Locked = dr.GetInt32(6);
                    Model.CompanyId = dr.GetInt32(7);
                    Model.UserId = dr.GetInt32(8);
                    Model.Scorse = dr.GetDecimal(9);
                    Model.TestDate = dr.GetDateTime(10);
                    Model.Point = dr.GetDecimal(11);
                    Model.IsPass = int.Parse(dr["IsPass"].ToString());
                }
                if (List.Find(s => s.UserId == Model.UserId && s.CateId == Model.CateId && s.Scorse == Model.Scorse && s.TestDate == Model.TestDate) == null)
                    List.Add(Model);
            }
        }


        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public TestPaperInfo GetModel(SqlDataReader dr)
        {
            TestPaperInfo Model = new TestPaperInfo();
            if (dr.Read())
            {
                Model.TestPaperId = dr.GetInt32(0);
                Model.PaperName = dr[1].ToString();
                Model.CateId = dr.GetInt32(2);
                Model.QuestionStyle = dr[3].ToString(); ;
                Model.QuestionId = dr[4].ToString();
                Model.Answer = dr[5].ToString();
                Model.Locked = dr.GetInt32(6);
                Model.CompanyId = dr.GetInt32(7);
                Model.UserId = dr.GetInt32(8);
                Model.Scorse = dr.GetDecimal(9);
                Model.TestDate = dr.GetDateTime(10);
                Model.Point = dr.GetDecimal(11);
                Model.IsPass = int.Parse(dr["IsPass"].ToString());
            }
            return Model;
        }
    }
}
