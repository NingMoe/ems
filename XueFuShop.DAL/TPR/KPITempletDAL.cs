using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.IDAL;
using XueFuShop.Models;

namespace XueFuShop.DAL
{
    public sealed class KPITempletDAL : IKPITemplet
    {
        public int AddKPITemplet(KPITempletInfo kpiTemplet)
        {
            SqlParameter[] pt = new SqlParameter[] {
                new SqlParameter("@postId", SqlDbType.Int),
                new SqlParameter("@kpiContent", SqlDbType.NVarChar),
                new SqlParameter("@companyId", SqlDbType.NVarChar),
                new SqlParameter("@state", SqlDbType.Float)
            };

            pt[0].Value = kpiTemplet.PostId;
            pt[1].Value = kpiTemplet.KPIContent;
            pt[2].Value = kpiTemplet.CompanyId;
            pt[3].Value = kpiTemplet.State;
            return Convert.ToInt32(ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "AddKPITemplet", pt));
        }

        public void UpdateKPITemplet(KPITempletInfo kpiTemplet)
        {
            SqlParameter[] pt = new SqlParameter[] {
                new SqlParameter("@id", SqlDbType.Int),
                new SqlParameter("@postId", SqlDbType.NVarChar),
                new SqlParameter("@kpiContent", SqlDbType.Int),
                new SqlParameter("@companyId", SqlDbType.NVarChar),
                new SqlParameter("@state", SqlDbType.NVarChar)
            };

            pt[0].Value = kpiTemplet.ID;
            pt[1].Value = kpiTemplet.PostId;
            pt[2].Value = kpiTemplet.KPIContent;
            pt[3].Value = kpiTemplet.CompanyId;
            pt[4].Value = kpiTemplet.State;

            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "UpdateKPITemplet", pt);
        }

        public void DeleteKPITemplet(string strID)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@strID", SqlDbType.NVarChar) };
            pt[0].Value = strID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteKPITemplet", pt);
        }

        public void DeleteKPITempletByCompanyID(string companyID)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@companyID", SqlDbType.VarChar) };
            pt[0].Value = companyID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteKPITempletByCompanyID", pt);
        }

        public int ExistsKPITemplet(int companyID, string kpiContent)
        {
            SqlParameter[] pt = new SqlParameter[] {
                new SqlParameter("@companyID", SqlDbType.Int),
                new SqlParameter("@kpiContent", SqlDbType.VarChar)
            };
            pt[0].Value = companyID;
            pt[1].Value = kpiContent;
            return Convert.ToInt32(ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "ExistsKPITemplet", pt));
        }

        public KPITempletInfo ReadKPITemplet(int id)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@id", SqlDbType.NVarChar) };
            pt[0].Value = id;
            KPITempletInfo info = new KPITempletInfo();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadKPITemplet", pt))
            {
                if (dr.Read())
                {
                    info.ID = int.Parse(dr["ID"].ToString());
                    info.PostId = int.Parse(dr["PostId"].ToString());
                    info.KPIContent = dr["KPIContent"].ToString();
                    info.CompanyId = int.Parse(dr["CompanyId"].ToString());
                    info.State = int.Parse(dr["State"].ToString());
                }
            }
            return info;
        }

        public void PrepareCondition(MssqlCondition mssqlCondition, KPITempletInfo kpiTemplet)
        {
            //mssqlCondition.Add("[ParentID]", kpiTemplet.ParentId, ConditionType.In);
            //mssqlCondition.Add("[Name]", kpiTemplet.Name, ConditionType.Like);
            mssqlCondition.Add("[CompanyId]", kpiTemplet.CompanyId, ConditionType.Equal);
        }

        public List<KPITempletInfo> SearchKPITempletList(KPITempletInfo kpiTemplet)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, kpiTemplet);
            List<KPITempletInfo> kpiTempletList = new List<KPITempletInfo>();
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@condition", SqlDbType.NVarChar) };
            pt[0].Value = mssqlCondition.ToString();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "SearchKPITempletList", pt))
            {
                this.PrepareModel(dr, kpiTempletList);
            }
            return kpiTempletList;
        }

        public void PrepareModel(SqlDataReader dr, List<KPITempletInfo> kpiTempletList)
        {
            while (dr.Read())
            {
                KPITempletInfo info = new KPITempletInfo();
                {
                    info.ID = int.Parse(dr["ID"].ToString());
                    info.PostId = int.Parse(dr["PostId"].ToString());
                    info.KPIContent = dr["KPIContent"].ToString();
                    info.CompanyId = int.Parse(dr["CompanyId"].ToString());
                    info.State = int.Parse(dr["State"].ToString());
                }
                kpiTempletList.Add(info);
            }
        }

    }
}
