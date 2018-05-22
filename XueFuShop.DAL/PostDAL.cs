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
    public sealed class PostDAL : IPost
    {
        //读取信息
        public PostInfo ReadPost(int Id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "Post] where  [PostId]="+Id);
            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                return GetModel(reader);
            }
        }

        public int AddPost(PostInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into [" + ShopMssqlHelper.TablePrefix + "Post] ( [PostName],[PostPlan],[ParentId],[OrderIndex],[Locked],[IsPost],[CompanyID]) values(@PostName,@PostPlan,@ParentId,@OrderIndex,@Locked,@IsPost,@CompanyID)");
            sql.Append("SELECT @@identity");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Convert.ToInt32(DbSQLHelper.GetSingle(sql.ToString(), par));
        }

        public void UpdatePost(PostInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [" + ShopMssqlHelper.TablePrefix + "Post] set [PostName]=@PostName,[PostPlan]=@PostPlan,[ParentId]=@ParentId,[OrderIndex]=@OrderIndex,[Locked]=@Locked,[IsPost]=@IsPost,[CompanyID]=@CompanyID where [PostId]=@PostId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }

        public void UpdatePostPlan(int PostId, string PostPlan)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@PostId", SqlDbType.Int), new SqlParameter("@PostPlan", SqlDbType.VarChar) };
            pt[0].Value = PostId;
            pt[1].Value = PostPlan;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "UpdatePostPlan", pt);
        }

        public void DeletePost(int Id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Delete from [" + ShopMssqlHelper.TablePrefix + "Post] where [PostId]=" + Id);
            DbSQLHelper.ExecuteSql(sql.ToString());
        }


        public List<PostInfo> ReadPostCateAllList()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "Post] ");
            sql.Append(" Order by [CompanyID],[ParentId],[OrderIndex],[PostName],[PostId] desc");
            List<PostInfo> List = new List<PostInfo>();
            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                this.PrepareModel(reader, List);
            }
            return List;
        }

        public List<PostInfo> ReadPostList(PostInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "Post] ");
            List<PostInfo> List = new List<PostInfo>();
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, Model);

            if (mssqlCondition.ToString() != string.Empty)
            {
                sql.Append("where " + mssqlCondition.ToString());
            }
            sql.Append(" Order by [CompanyID],[OrderIndex],[PostName],[PostId] desc");
            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                this.PrepareModel(reader, List);
            }


            return List;
        }

        public void MoveDown(int id)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int) };
            pt[0].Value = id;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "MoveDownPost", pt);
        }

        public void MoveUp(int id)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int) };
            pt[0].Value = id;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "MoveUpPost", pt);
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(PostInfo Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@PostId",SqlDbType .Int),
                                    new SqlParameter ("@PostName",SqlDbType .VarChar),
                                    new SqlParameter ("@PostPlan",SqlDbType .VarChar),
                                    new SqlParameter ("@ParentId",SqlDbType .Int),
                                    new SqlParameter ("@OrderIndex",SqlDbType .Int),
                                    new SqlParameter ("@Locked",SqlDbType .Bit,50),
                                    new SqlParameter ("@IsPost",SqlDbType .Int),
                                    new SqlParameter ("@CompanyID",SqlDbType .Int)
                                };
            par[0].Value = Model.PostId;
            par[1].Value = Model.PostName;
            par[2].Value = Model.PostPlan;
            par[3].Value = Model.ParentId;
            par[4].Value = Model.OrderIndex;
            par[5].Value = Model.Locked;
            par[6].Value = Model.IsPost;
            par[7].Value = Model.CompanyID;
            return par;
        }

        public void PrepareCondition(MssqlCondition mssqlCondition, PostInfo Model)
        {
            mssqlCondition.Add("[PostName]", Model.PostName, ConditionType.Like);
            //mssqlCondition.Add("[PostPlan]", Model.PostPlan, ConditionType.In);
            mssqlCondition.Add("[Locked]", Model.Locked, ConditionType.Equal);
            mssqlCondition.Add("[ParentId]", Model.ParentId, ConditionType.Equal);
            mssqlCondition.Add("[PostId]", Model.PostId, ConditionType.NoEqual);
            mssqlCondition.Add("[IsPost]", Model.IsPost, ConditionType.Equal);
            mssqlCondition.Add("[CompanyID]", Model.CompanyID, ConditionType.Equal);
            mssqlCondition.Add("[CompanyID]", Model.InCompanyID, ConditionType.In);
            if (!string.IsNullOrEmpty(Model.PostPlan))
            {
                mssqlCondition.Add(" [dbo].[" + ShopMssqlHelper.TablePrefix + "CompareSTR](PostPlan,'" + Model.PostPlan + "',',')=1 ");
            }
        }

        public void PrepareModel(SqlDataReader dr, List<PostInfo> List)
        {
            while (dr.Read())
            {
                PostInfo Model = new PostInfo();
                {
                    Model.PostId = dr.GetInt32(0);
                    Model.PostName = dr[1].ToString();
                    Model.PostPlan = dr[2].ToString();
                    Model.ParentId = dr.GetInt32(3);
                    Model.OrderIndex = dr.GetInt32(4);
                    Model.Locked = dr.GetInt32(5);
                    Model.IsPost = dr.GetInt32(7);
                    Model.CompanyID = dr.GetInt32(8);
                }
                List.Add(Model);
            }
        }


        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public PostInfo GetModel(SqlDataReader dr)
        {
            PostInfo Model = new PostInfo();
            if (dr.Read())
            {
                Model.PostId = dr.GetInt32(0);
                Model.PostName = dr[1].ToString();
                Model.PostPlan = dr[2].ToString();
                Model.ParentId = dr.GetInt32(3);
                Model.OrderIndex = dr.GetInt32(4);
                Model.Locked = dr.GetInt32(5);
                Model.IsPost = dr.GetInt32(7);
                Model.CompanyID = dr.GetInt32(8);
            }
            return Model;
        }
    }
}
