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
    public sealed class QuestionDAL : IQuestion
    {
        //读取信息
        public QuestionInfo ReadQuestion(int Id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "Question] where  QuestionId=" + Id);
            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                return GetModel(reader);
            }
        }

        public int AddQuestion(QuestionInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into [" + ShopMssqlHelper.TablePrefix + "Question] ( CateId,Style,Score,Question,A,B,C,D,Answer,Remarks,Checked,CompanyId ) values(@CateId,@Style,@Score,@Question,@A,@B,@C,@D,@Answer,@Remarks,@Checked,@CompanyId)");
            sql.Append("SELECT @@identity");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }

        public void UpdateQuestion(QuestionInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [" + ShopMssqlHelper.TablePrefix + "Question] set CateId=@CateId,Style=@Style,Score=@Score,Question=@Question,A=@A,B=@B,C=@C,D=@D,Answer=@Answer,Remarks=@Remarks,Checked=@Checked,CompanyId=@CompanyId where QuestionId=@QuestionId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }

        public void DeleteQuestion(string IdStr)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Delete from [" + ShopMssqlHelper.TablePrefix + "Question] where QuestionId in (" + IdStr + ")");
            DbSQLHelper.ExecuteSql(sql.ToString());
        }

        /// <summary>
        /// 通过课程ID删除考题
        /// </summary>
        /// <param name="Id"></param>
        public void DeleteQuestionByCateId(int CateId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Delete from [" + ShopMssqlHelper.TablePrefix + "Question] where CateId=" + CateId);
            DbSQLHelper.ExecuteSql(sql.ToString());
        }

        public void UpdateQuestionChecked(string Id, string Value)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [" + ShopMssqlHelper.TablePrefix + "Question] set Checked=" + Value + " where QuestionId in (" + Id + ")");
            DbSQLHelper.ExecuteSql(sql.ToString());
        }

        public int ReadQuestionNum(string courseID)
        {
            int questionsNum = 0;
            if (!string.IsNullOrEmpty(courseID))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select Count(QuestionId) as [QuestionsNum] from [" + ShopMssqlHelper.TablePrefix + "Question] where [CateId] in (" + courseID + ")");
                using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            return questionsNum;
        }

        public List<QuestionInfo> ReadList(int CateId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "Question] Where [CateId]=" + CateId.ToString());
            List<QuestionInfo> TempList = new List<QuestionInfo>();
            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                this.PrepareModel(reader, TempList);
            }

            return TempList;
        }


        public List<QuestionInfo> ReadList(QuestionInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select "+Model.QuestionNum+" from [" + ShopMssqlHelper.TablePrefix + "Question] ");
            List<QuestionInfo> TempList = new List<QuestionInfo>();
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, Model);           
             
            if (mssqlCondition.ToString() != string.Empty)
            {
                sql.Append("where " + mssqlCondition.ToString());
            }
            sql.Append(" Order by newid()");
            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                this.PrepareModel(reader, TempList);
            }

            return TempList;
        }

        public List<QuestionInfo> ReadList(int currentPage, int pageSize, ref int count)
        {
            List<QuestionInfo> TempList = new List<QuestionInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = "[" + ShopMssqlHelper.TablePrefix + "Question]";
            class2.Fields = "*";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[QuestionId]";
            class2.OrderType = OrderType.Desc;
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader reader = class2.ExecuteReader())
            {
                this.PrepareModel(reader, TempList);
            }
            return TempList;
        }

        public List<QuestionInfo> ReadList(QuestionInfo Model, int currentPage, int pageSize, ref int count)
        {
            List<QuestionInfo> TempList = new List<QuestionInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = "[" + ShopMssqlHelper.TablePrefix + "Question]";
            class2.Fields = "*";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[QuestionId]";
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
        protected IDbDataParameter[] ValueParas(QuestionInfo Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@QuestionId",SqlDbType.Int),
                                    new SqlParameter ("@CateId",SqlDbType.Int),
                                    new SqlParameter ("@Style",SqlDbType .VarChar),
                                    new SqlParameter ("@Score",SqlDbType.Decimal),
                                    new SqlParameter ("@Question",SqlDbType .VarChar),
                                    new SqlParameter ("@A",SqlDbType .VarChar),
                                    new SqlParameter ("@B",SqlDbType .VarChar),
                                    new SqlParameter ("@C",SqlDbType .VarChar),
                                    new SqlParameter ("@D",SqlDbType .VarChar),
                                    new SqlParameter ("@Answer",SqlDbType .VarChar),
                                    new SqlParameter ("@Remarks",SqlDbType .VarChar),
                                    new SqlParameter ("@Checked",SqlDbType.Int),
                                    new SqlParameter ("@CompanyId",SqlDbType.Int)
                                };
            par[0].Value = Model.QuestionId;
            par[1].Value = Model.CateId;
            par[2].Value = Model.Style;
            par[3].Value = Model.Score;
            par[4].Value = Model.Question;
            par[5].Value = Model.A;
            par[6].Value = Model.B;
            par[7].Value = Model.C;
            par[8].Value = Model.D;
            par[9].Value = Model.Answer;
            par[10].Value = Model.Remarks;
            par[11].Value =Model.Checked;
            par[12].Value = Model.CompanyId;
            return par;
        }

        public void PrepareCondition(MssqlCondition mssqlCondition, QuestionInfo Model)
        {
            mssqlCondition.Add("[CateId]", Model.IdCondition, ConditionType.In);
            mssqlCondition.Add("[Style]", Model.Style, ConditionType.Equal);
            //mssqlCondition.Add("[Checked]", Model.Checked, ConditionType.Equal);
            mssqlCondition.Add("[Question]", Model.Question, ConditionType.Like);
            mssqlCondition.Add("[CompanyId]", Model.CompanyId, ConditionType.NoEqual);
            if (Model.Field != string.Empty)
            {
                mssqlCondition.Add("["+Model.Field+"]", Model.Condition, ConditionType.In);
            }
        }

        public void PrepareModel(SqlDataReader dr, List<QuestionInfo> List)
        {
            while (dr.Read())
            {
                QuestionInfo Model = new QuestionInfo();
                {
                    Model.QuestionId = dr.GetInt32(0);
                    Model.CateId = dr.GetInt32(1);
                    Model.Style = dr[2].ToString();
                    Model.Score = dr.GetDecimal(3);
                    Model.Question = dr[4].ToString();
                    Model.A = dr[5].ToString();
                    Model.B = dr[6].ToString();
                    Model.C = dr[7].ToString();
                    Model.D = dr[8].ToString();
                    Model.Answer = dr[9].ToString();
                    Model.Remarks = dr[10].ToString();
                    Model.Checked = dr.GetInt32(11);
                    Model.CompanyId = dr.GetInt32(12);
                }
                List.Add(Model);
            }
        }


        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public QuestionInfo GetModel(SqlDataReader dr)
        {
            QuestionInfo Model = new QuestionInfo();
            if (dr.Read())
            {
                Model.QuestionId = dr.GetInt32(0);
                Model.CateId = dr.GetInt32(1);
                Model.Style = dr[2].ToString();
                Model.Score = dr.GetDecimal(3);
                Model.Question = dr[4].ToString();
                Model.A = dr[5].ToString();
                Model.B = dr[6].ToString();
                Model.C = dr[7].ToString();
                Model.D = dr[8].ToString();
                Model.Answer = dr[9].ToString();
                Model.Remarks = dr[10].ToString();
                Model.Checked = dr.GetInt32(11);
                Model.CompanyId = dr.GetInt32(12);
                dr.Close();
                dr.Dispose();
            }
            return Model;
        }
    }
}
