using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.DAL
{
    public sealed class PostKPISettingDAL : IPostKPISetting
    {
        public int AddPostKPISetting(PostKPISettingInfo info)
        {
            SqlParameter[] pt = new SqlParameter[] {
                new SqlParameter("@name", SqlDbType.NVarChar),
                new SqlParameter("@kPIContent", SqlDbType.VarChar),
                new SqlParameter("@companyId", SqlDbType.Int),
                new SqlParameter("@state", SqlDbType.Int)
            };

            pt[0].Value = info.Name;
            pt[1].Value = info.KPIContent;
            pt[2].Value = info.CompanyId;
            pt[3].Value = info.State;
            return Convert.ToInt32(ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "AddPostKPISetting", pt));
        }

        public void UpdatePostKPISetting(PostKPISettingInfo info)
        {
            SqlParameter[] pt = new SqlParameter[] {
                new SqlParameter("@id", SqlDbType.Int),
                new SqlParameter("@name", SqlDbType.NVarChar),
                new SqlParameter("@kPIContent", SqlDbType.VarChar),
                new SqlParameter("@companyId", SqlDbType.Int),
                new SqlParameter("@state", SqlDbType.Int)
            };

            pt[0].Value = info.ID;
            pt[1].Value = info.Name;
            pt[2].Value = info.KPIContent;
            pt[3].Value = info.CompanyId;
            pt[4].Value = info.State;

            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "UpdatePostKPISetting", pt);
        }

        public void DeletePostKPISetting(string strID)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@strID", SqlDbType.NVarChar) };
            pt[0].Value = strID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeletePostKPISetting", pt);
        }

        public void DeletePostKPISettingByCompanyID(string companyID)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@companyID", SqlDbType.VarChar) };
            pt[0].Value = companyID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeletePostKPISettingBycompanyID", pt);
        }

        public PostKPISettingInfo ReadPostKPISetting(int id)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@id", SqlDbType.NVarChar) };
            pt[0].Value = id;
            PostKPISettingInfo info = new PostKPISettingInfo();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadPostKPISetting", pt))
            {
                if (dr.Read())
                {
                    info.ID = int.Parse(dr["ID"].ToString());
                    info.Name = dr["Name"].ToString();
                    info.KPIContent = dr["KPIContent"].ToString();
                    info.CompanyId = int.Parse(dr["CompanyId"].ToString());
                    info.State = int.Parse(dr["State"].ToString());
                }
            }
            return info;
        }

        public void PrepareModel(SqlDataReader dr, List<PostKPISettingInfo> infoList)
        {
            while (dr.Read())
            {
                PostKPISettingInfo info = new PostKPISettingInfo();
                {
                    info.ID = int.Parse(dr["ID"].ToString());
                    info.Name = dr["Name"].ToString();
                    info.KPIContent = dr["KPIContent"].ToString();
                    info.CompanyId = int.Parse(dr["CompanyId"].ToString());
                    info.State = int.Parse(dr["State"].ToString());
                }
                infoList.Add(info);
            }
        }

        public List<PostKPISettingInfo> SearchPostKPISettingList(PostKPISettingInfo info)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, info);
            List<PostKPISettingInfo> postKPISettingList = new List<PostKPISettingInfo>();
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@condition", SqlDbType.NVarChar) };
            pt[0].Value = mssqlCondition.ToString();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "SearchPostKPISettingList", pt))
            {
                this.PrepareModel(dr, postKPISettingList);
            }
            return postKPISettingList;
        }

        public List<PostKPISettingInfo> SearchPostKPISettingList(PostKPISettingInfo info, int currentPage, int pageSize, ref int count)
        {
            List<PostKPISettingInfo> postKPISettingList = new List<PostKPISettingInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = ShopMssqlHelper.TablePrefix + "PostKPISetting";
            class2.Fields = "*";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[Id]";
            class2.OrderType = OrderType.Asc;
            this.PrepareCondition(class2.MssqlCondition, info);
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader dr = class2.ExecuteReader())
            {
                this.PrepareModel(dr, postKPISettingList);
            }
            return postKPISettingList;
        }

        public void PrepareCondition(MssqlCondition mssqlCondition, PostKPISettingInfo info)
        {
            //mssqlCondition.Add("[PostKPISettingCateID]", info.ParentID, ConditionType.In);
            //mssqlCondition.Add(userSearch.Condition);
        }
    }
}
