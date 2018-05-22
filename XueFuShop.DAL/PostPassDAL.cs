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
    public sealed class PostPassDAL : IPostPass
    {
        //读取信息
        public PostPassInfo ReadPostPassInfo(int Id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "PassPost] where  [Id]=" + Id);
            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                return this.GetModel(reader);
            }
        }

        //读取信息
        public PostPassInfo ReadPostPassInfo(int userID, int postID)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "PassPost] where  [UserID]=" + userID + " And [PostID]=" + postID);
            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                return this.GetModel(reader);
            }
        }

        public int AddPostPassInfo(PostPassInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into [" + ShopMssqlHelper.TablePrefix + "PassPost] ([UserId],[PostId],[IsRZ],[PostName]) values(@UserId,@PostId,@IsRZ,@PostName)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }

        public void UpdatePostPassInfo(PostPassInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [" + ShopMssqlHelper.TablePrefix + "PassPost] set [UserId]=@UserId,[PostId]=@PostId,[PostName]=@PostName where [Id]=@Id");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }

        public void UpdateCreateDate(PostPassInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [" + ShopMssqlHelper.TablePrefix + "PassPost] set [CreateDate]='" + Model.CreateDate.ToString() + "' where [UserId]=" + Model.UserId + " and [PostId]=" + Model.PostId);
            DbSQLHelper.ExecuteSql(sql.ToString());
        }

        public void UpdateIsRZ(PostPassInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [" + ShopMssqlHelper.TablePrefix + "PassPost] set [CreateDate]='" + DateTime.Now + "',[IsRZ]=" + Model.IsRZ + " where [UserId]=" + Model.UserId + " and [PostId]=" + Model.PostId);
            DbSQLHelper.ExecuteSql(sql.ToString());
        }

        public PostPassInfo ReadPostPassInfo()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select top 1 * from [" + ShopMssqlHelper.TablePrefix + "PassPost] where [IsRZ]=1 Order by id desc");
            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                return this.GetModel(reader);
            }
        }

        /// <summary>
        /// 获取通过的岗位数
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int ReadPostPassNum(int userID)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(1) from [" + ShopMssqlHelper.TablePrefix + "PassPost] where [UserID]=" + userID + " and [IsRZ]=1 and [postid] in (select [postid] from [_post])");
            return (int)DbSQLHelper.GetSingle(sql.ToString());
        }

        public List<PostPassInfo> ReadPostPassList(PostPassInfo Model)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "PassPost] ");
            List<PostPassInfo> TempList = new List<PostPassInfo>();
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, Model);

            if (mssqlCondition.ToString() != string.Empty)
            {
                sql.Append("where " + mssqlCondition.ToString());
                sql.Append(" Order By CreateDate desc");
                using (SqlDataReader dr = DbSQLHelper.ExecuteReader(sql.ToString()))
                {
                    this.PrepareModel(dr, TempList);
                }
            }

            return TempList;
        }

        public List<PostPassInfo> ReadPostPassList(PostPassInfo postpassSearch, int currentPage, int pageSize, ref int count)
        {
            List<PostPassInfo> TempList = new List<PostPassInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = "[" + ShopMssqlHelper.TablePrefix + "PassPost]";
            class2.Fields = "*";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[CreateDate]";
            class2.OrderType = OrderType.Desc;
            this.PrepareCondition(class2.MssqlCondition, postpassSearch);
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader dr = class2.ExecuteReader())
            {
                this.PrepareModel(dr, TempList);
            }
            return TempList;
        }


        public List<PostPassInfo> ReadPostPassList(PostPassInfo Model, int num)
        {
            string sqlNumString = "*";
            if (num > 0)
                sqlNumString = "top " + num + " " + sqlNumString;
            StringBuilder sql = new StringBuilder();
            sql.Append("select " + sqlNumString + " from [" + ShopMssqlHelper.TablePrefix + "PassPost] ");
            List<PostPassInfo> TempList = new List<PostPassInfo>();
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, Model);

            if (mssqlCondition.ToString() != string.Empty)
            {
                sql.Append("where " + mssqlCondition.ToString());
                sql.Append(" Order By CreateDate desc");
                using (SqlDataReader dr = DbSQLHelper.ExecuteReader(sql.ToString()))
                {
                    this.PrepareModel(dr, TempList);
                }
            }

            return TempList;
        }

        public List<ReportPostPassInfo> PostPassRepostList(PostPassInfo Model, string CompanyId)
        {
            if (Model.SearchStartDate == DateTime.MinValue) Model.SearchStartDate = Convert.ToDateTime("2013-1-1");
            List<ReportPostPassInfo> TempList = new List<ReportPostPassInfo>();
            StringBuilder sql = new StringBuilder();
            sql.Append("select [_PassPost].UserId as UserId,[_PassPost].[PostName] as [PassPostName],WorkingPostName,[_User].WorkingPostID as WorkPostId,[_PassPost].PostId as PostId,[_User].RealName as RealName,(select count(distinct PostId) from [" + ShopMssqlHelper.TablePrefix + "PassPost] Where UserId=[_User].ID and IsRZ=1) as PostNum,[_User].StudyPostId as StudyPostId from [" + ShopMssqlHelper.TablePrefix + "PassPost],[" + ShopMssqlHelper.TablePrefix + "User] ");
            sql.Append("where [_PassPost].UserId=[_User].ID and [_User].CompanyID in (" + CompanyId + ") and ([_PassPost].CreateDate between '" + Model.SearchStartDate.ToString() + "' and '" + Model.CreateDate.ToString() + "') and [IsRZ]=1");
            sql.Append(" Order By [_PassPost].id desc");
            using (SqlDataReader dr = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                while (dr.Read())
                {
                    ReportPostPassInfo ReportModel = new ReportPostPassInfo();
                    {
                        ReportModel.UserId = int.Parse(dr["UserId"].ToString());
                        ReportModel.PassPostId = dr["PostId"].ToString();
                        ReportModel.PassPostName = dr["PassPostName"].ToString();
                        ReportModel.PassPostNum = int.Parse(dr["PostNum"].ToString());
                        ReportModel.RealName = dr["RealName"].ToString();
                        ReportModel.WorkingPostName = dr["WorkingPostName"].ToString();
                        ReportModel.WorkingPostId = int.Parse(dr["WorkPostId"].ToString());
                        ReportModel.StudyPostId = int.Parse(dr["StudyPostId"].ToString());
                    }
                    TempList.Add(ReportModel);
                }
            }

            return TempList;
        }


        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(PostPassInfo Model)
        {
            SqlParameter[] par ={
                new SqlParameter ("@UserId",SqlDbType .Int),
                new SqlParameter ("@PostId",SqlDbType .Int),
                new SqlParameter ("@IsRZ",SqlDbType .Int),
                new SqlParameter("@PostName",SqlDbType.NVarChar)
            };
            par[0].Value = Model.UserId;
            par[1].Value = Model.PostId;
            par[2].Value = Model.IsRZ;
            par[3].Value = Model.PostName;
            return par;
        }

        public void PrepareCondition(MssqlCondition mssqlCondition, PostPassInfo Model)
        {
            mssqlCondition.Add("[UserId]", Model.UserId, ConditionType.Equal);
            mssqlCondition.Add("[UserId]", Model.InUserID, ConditionType.In);
            mssqlCondition.Add("[PostId]", Model.PostId, ConditionType.Equal);
            mssqlCondition.Add("[IsRZ]", Model.IsRZ, ConditionType.Equal);
            mssqlCondition.Add("[PostName]", Model.PostName, ConditionType.Like);
            mssqlCondition.Add("[CreateDate]", Model.CreateDate, ConditionType.LessOrEqual);
            mssqlCondition.Add("[CreateDate]", Model.SearchStartDate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[_passpost].[postid] in (select [postid] from [_post])");
            if (!string.IsNullOrEmpty(Model.InCompanyID))
                mssqlCondition.Add("[UserId] in (Select [ID] from [" + ShopMssqlHelper.TablePrefix + "User] where [CompanyID] in (" + Model.InCompanyID + "))");
        }

        public void PrepareModel(SqlDataReader dr, List<PostPassInfo> List)
        {
            while (dr.Read())
            {
                PostPassInfo Model = new PostPassInfo();
                {
                    Model.Id = int.Parse(dr["Id"].ToString());
                    Model.UserId = int.Parse(dr["UserId"].ToString());
                    Model.PostId = int.Parse(dr["PostId"].ToString());
                    Model.IsRZ = int.Parse(dr["IsRZ"].ToString());
                    Model.PostName = dr["PostName"].ToString();
                    Model.CreateDate = Convert.ToDateTime(dr["CreateDate"]);
                }
                List.Add(Model);
            }
        }


        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public PostPassInfo GetModel(SqlDataReader dr)
        {
            PostPassInfo Model = new PostPassInfo();
            if (dr.Read())
            {
                Model.Id = int.Parse(dr["Id"].ToString());
                Model.UserId = int.Parse(dr["UserId"].ToString());
                Model.PostId = int.Parse(dr["PostId"].ToString());
                Model.IsRZ = int.Parse(dr["IsRZ"].ToString());
                Model.PostName = dr["PostName"].ToString();
            }
            return Model;
        }
    }
}
