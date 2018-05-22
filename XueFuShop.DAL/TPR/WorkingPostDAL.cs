using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.DAL
{
    public sealed class WorkingPostDAL : IWorkingPost
    {
        public int AddWorkingPost(WorkingPostInfo workingPost)
        {
            SqlParameter[] pt = new SqlParameter[] {
                new SqlParameter("@postName", SqlDbType.NVarChar),
                new SqlParameter("@parentId", SqlDbType.Int),
                new SqlParameter("@companyId", SqlDbType.Int),
                new SqlParameter("@state", SqlDbType.Int),
                new SqlParameter("@isPost", SqlDbType.Int),
                new SqlParameter("@kpiTempletId", SqlDbType.Int),
                new SqlParameter("@sort", SqlDbType.Int),
                new SqlParameter("@path", SqlDbType.VarChar)
            };

            pt[0].Value = workingPost.PostName;
            pt[1].Value = workingPost.ParentId;
            pt[2].Value = workingPost.CompanyId;
            pt[3].Value = workingPost.State;
            pt[4].Value = workingPost.IsPost;
            pt[5].Value = workingPost.KPITempletId;
            pt[6].Value = workingPost.Sort;
            pt[7].Value = workingPost.Path;
            return Convert.ToInt32(ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "AddWorkingPost", pt));
        }

        public void UpdateWorkingPost(WorkingPostInfo workingPost)
        {
            SqlParameter[] pt = new SqlParameter[] {
                new SqlParameter("@id", SqlDbType.Int),
                new SqlParameter("@postName", SqlDbType.NVarChar),
                new SqlParameter("@parentId", SqlDbType.Int),
                new SqlParameter("@companyId", SqlDbType.Int),
                new SqlParameter("@state", SqlDbType.Int),
                new SqlParameter("@isPost", SqlDbType.Int),
                new SqlParameter("@kpiTempletId", SqlDbType.Int),
                new SqlParameter("@sort", SqlDbType.Int),
                new SqlParameter("@path", SqlDbType.VarChar)
            };

            pt[0].Value = workingPost.ID;
            pt[1].Value = workingPost.PostName;
            pt[2].Value = workingPost.ParentId;
            pt[3].Value = workingPost.CompanyId;
            pt[4].Value = workingPost.State;
            pt[5].Value = workingPost.IsPost;
            pt[6].Value = workingPost.KPITempletId;
            pt[7].Value = workingPost.Sort;
            pt[8].Value = workingPost.Path;

            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "UpdateWorkingPost", pt);
        }

        public void DeleteWorkingPost(string strID)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@strID", SqlDbType.NVarChar) };
            pt[0].Value = strID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteWorkingPost", pt);
        }

        public void DeleteWorkingPostByCompanyID(string companyID)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@companyID", SqlDbType.VarChar) };
            pt[0].Value = companyID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteWorkingPostByCompanyID", pt);
        }

        public WorkingPostInfo ReadWorkingPost(int id)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@id", SqlDbType.NVarChar) };
            pt[0].Value = id;
            WorkingPostInfo info = new WorkingPostInfo();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadWorkingPost", pt))
            {
                if (dr.Read())
                {
                    info.ID = int.Parse(dr["ID"].ToString());
                    info.PostName = dr["PostName"].ToString();
                    info.ParentId = int.Parse(dr["ParentId"].ToString());
                    info.CompanyId = int.Parse(dr["CompanyId"].ToString());
                    info.State = int.Parse(dr["State"].ToString());
                    info.IsPost = int.Parse(dr["IsPost"].ToString());
                    info.KPITempletId = int.Parse(dr["KPITempletId"].ToString());
                    info.Sort = int.Parse(dr["Sort"].ToString());
                    info.Path = dr["Path"].ToString();
                }
            }
            return info;
        }

        public WorkingPostViewInfo ReadWorkingPostView(int id)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@id", SqlDbType.NVarChar) };
            pt[0].Value = id;
            WorkingPostViewInfo info = new WorkingPostViewInfo();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadWorkingPostView", pt))
            {
                if (dr.Read())
                {
                    info.PostId = int.Parse(dr["PostId"].ToString());
                    info.PostName = dr["PostName"].ToString();
                    info.KPIContent = dr["KPIContent"].ToString();
                    info.CompanyId = int.Parse(dr["CompanyId"].ToString());
                }
            }
            return info;
        }

        public List<WorkingPostViewInfo> SearchWorkingPostViewList(WorkingPostSearchInfo workingPostSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, workingPostSearch);
            List<WorkingPostViewInfo> workingPostViewList = new List<WorkingPostViewInfo>();
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@condition", SqlDbType.NVarChar) };
            pt[0].Value = mssqlCondition.ToString();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "SearchWorkingPostViewList", pt))
            {
                while (dr.Read())
                {
                    WorkingPostViewInfo info = new WorkingPostViewInfo();
                    {
                        info.PostId = int.Parse(dr["PostId"].ToString());
                        info.PostName = dr["PostName"].ToString();
                        info.KPIContent = dr["KPIContent"].ToString();
                        info.CompanyId = int.Parse(dr["CompanyId"].ToString());
                    }
                    workingPostViewList.Add(info);
                }
            }
            return workingPostViewList;
        }

        public void PrepareModel(SqlDataReader dr, List<WorkingPostInfo> workingPostList)
        {
            while (dr.Read())
            {
                WorkingPostInfo info = new WorkingPostInfo();
                {
                    info.ID = int.Parse(dr["ID"].ToString());
                    info.PostName = dr["PostName"].ToString();
                    info.ParentId = int.Parse(dr["ParentId"].ToString());
                    info.CompanyId = int.Parse(dr["CompanyId"].ToString());
                    info.State = int.Parse(dr["State"].ToString());
                    info.IsPost = int.Parse(dr["IsPost"].ToString());
                    info.KPITempletId = int.Parse(dr["KPITempletId"].ToString());
                    info.Sort = int.Parse(dr["Sort"].ToString());
                    info.Path = dr["Path"].ToString();
                }
                workingPostList.Add(info);
            }
        }

        public List<WorkingPostInfo> SearchWorkingPostList(WorkingPostSearchInfo workingPostSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, workingPostSearch);
            List<WorkingPostInfo> workingPostList = new List<WorkingPostInfo>();
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@condition", SqlDbType.NVarChar) };
            pt[0].Value = mssqlCondition.ToString();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "SearchWorkingPostList", pt))
            {
                this.PrepareModel(dr, workingPostList);
            }
            return workingPostList;
        }

        public List<WorkingPostInfo> SearchWorkingPostList(WorkingPostSearchInfo workingPostSearch, int currentPage, int pageSize, ref int count)
        {
            List<WorkingPostInfo> workingPostList = new List<WorkingPostInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = ShopMssqlHelper.TablePrefix + "WorkingPost";
            class2.Fields = "*";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[Id]";
            class2.OrderType = OrderType.Asc;
            this.PrepareCondition(class2.MssqlCondition, workingPostSearch);
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader dr = class2.ExecuteReader())
            {
                this.PrepareModel(dr, workingPostList);
            }
            return workingPostList;
        }

        public void PrepareCondition(MssqlCondition mssqlCondition, WorkingPostSearchInfo workingPostSearch)
        {
            mssqlCondition.Add("[ParentId]", workingPostSearch.ParentId, ConditionType.In);
            mssqlCondition.Add("[CompanyId]", workingPostSearch.CompanyId, ConditionType.In);
            mssqlCondition.Add("[IsPost]", workingPostSearch.IsPost, ConditionType.Equal);
            mssqlCondition.Add("[State]", workingPostSearch.State, ConditionType.Equal);
            mssqlCondition.Add("[PostName]", workingPostSearch.PostName, ConditionType.Equal);
            mssqlCondition.Add(workingPostSearch.Condition);
        }
    }
}
