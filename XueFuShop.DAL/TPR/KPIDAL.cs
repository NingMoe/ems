using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.DAL
{
    public sealed class KPIDAL : IKPI
    {
        public int AddKPI(KPIInfo kpi)
        {
            SqlParameter[] pt = new SqlParameter[] {
                new SqlParameter("@name", SqlDbType.NVarChar),
                new SqlParameter("@parentId", SqlDbType.Int),
                new SqlParameter("@evaluateInfo", SqlDbType.NVarChar),
                new SqlParameter("@method", SqlDbType.NVarChar),
                new SqlParameter("@scorse", SqlDbType.Float),
                new SqlParameter("@other", SqlDbType.NVarChar),
                new SqlParameter("@companyID", SqlDbType.Int),
                new SqlParameter("@state", SqlDbType.Int),
                new SqlParameter("@sort", SqlDbType.Int),
                new SqlParameter("@refPostId", SqlDbType.Int),
                new SqlParameter("@type", SqlDbType.Int)
            };

            pt[0].Value = kpi.Name;
            pt[1].Value = kpi.ParentId;
            pt[2].Value = kpi.EvaluateInfo;
            pt[3].Value = kpi.Method;
            pt[4].Value = kpi.Scorse;
            pt[5].Value = kpi.Other;
            pt[6].Value = kpi.CompanyID;
            pt[7].Value = kpi.State;
            pt[8].Value = kpi.Sort;
            pt[9].Value = kpi.RefPostId;
            pt[10].Value = kpi.Type;
            return Convert.ToInt32(ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "AddKPI", pt));
        }

        public void UpdateKPI(KPIInfo kpi)
        {
            SqlParameter[] pt = new SqlParameter[] {
                new SqlParameter("@id", SqlDbType.Int),
                new SqlParameter("@name", SqlDbType.NVarChar),
                new SqlParameter("@parentId", SqlDbType.Int),
                new SqlParameter("@evaluateInfo", SqlDbType.NVarChar),
                new SqlParameter("@method", SqlDbType.NVarChar),
                new SqlParameter("@scorse", SqlDbType.Float),
                new SqlParameter("@other", SqlDbType.NVarChar),
                new SqlParameter("@companyID", SqlDbType.Int),
                new SqlParameter("@state", SqlDbType.Int),
                new SqlParameter("@sort", SqlDbType.Int),
                new SqlParameter("@refPostId", SqlDbType.Int),
                new SqlParameter("@type", SqlDbType.Int)
            };

            pt[0].Value = kpi.ID;
            pt[1].Value = kpi.Name;
            pt[2].Value = kpi.ParentId;
            pt[3].Value = kpi.EvaluateInfo;
            pt[4].Value = kpi.Method;
            pt[5].Value = kpi.Scorse;
            pt[6].Value = kpi.Other;
            pt[7].Value = kpi.CompanyID;
            pt[8].Value = kpi.State;
            pt[9].Value = kpi.Sort;
            pt[10].Value = kpi.RefPostId;
            pt[11].Value = kpi.Type;

            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "UpdateKPI", pt);
        }

        public void DeleteKPI(string strID)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@strID", SqlDbType.NVarChar) };
            pt[0].Value = strID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteKPI", pt);
        }

        public void DeleteKPIByCompanyID(string companyID)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@companyID", SqlDbType.VarChar) };
            pt[0].Value = companyID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteKPIByCompanyID", pt);
        }

        public KPIInfo ReadKPI(int id)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@id", SqlDbType.NVarChar) };
            pt[0].Value = id;
            KPIInfo info = new KPIInfo();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadKPI", pt))
            {
                if (dr.Read())
                {
                    info.ID = int.Parse(dr["ID"].ToString());
                    info.Name = dr["Name"].ToString();
                    info.ParentId = int.Parse(dr["ParentId"].ToString());
                    info.EvaluateInfo = dr["EvaluateInfo"].ToString();
                    info.Method = dr["Method"].ToString();
                    info.Scorse = float.Parse(dr["Scorse"].ToString());
                    info.Other = dr["Other"].ToString();
                    info.CompanyID = int.Parse(dr["CompanyID"].ToString());
                    info.State = int.Parse(dr["State"].ToString());
                    info.Sort = int.Parse(dr["Sort"].ToString());
                    info.AddDate = Convert.ToDateTime(dr["AddDate"].ToString());
                    info.RefPostId = int.Parse(dr["RefPostId"].ToString());
                    info.Type = (KPIType)dr["Type"];
                }
            }
            return info;
        }
        
        public void PrepareModel(SqlDataReader dr, List<KPIInfo> kpiList)
        {
            while (dr.Read())
            {
                KPIInfo info = new KPIInfo();
                {
                    info.ID = int.Parse(dr["ID"].ToString());
                    info.Name = dr["Name"].ToString();
                    info.ParentId = int.Parse(dr["ParentId"].ToString());
                    info.EvaluateInfo = dr["EvaluateInfo"].ToString();
                    info.Method = dr["Method"].ToString();
                    info.Scorse = float.Parse(dr["Scorse"].ToString());
                    info.Other = dr["Other"].ToString();
                    info.CompanyID = int.Parse(dr["CompanyID"].ToString());
                    info.State = int.Parse(dr["State"].ToString());
                    info.Sort = int.Parse(dr["Sort"].ToString());
                    info.AddDate = Convert.ToDateTime(dr["AddDate"].ToString());
                    info.RefPostId = int.Parse(dr["RefPostId"].ToString());
                    info.Type = (KPIType)dr["Type"];
                }
                kpiList.Add(info);
            }
        }

        public List<KPIInfo> SearchKPIList(KPISearchInfo kpiSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, kpiSearch);
            List<KPIInfo> kpiList = new List<KPIInfo>();
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@condition", SqlDbType.NVarChar) };
            pt[0].Value = mssqlCondition.ToString();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "SearchKPIList", pt))
            {
                this.PrepareModel(dr, kpiList);
            }
            return kpiList;
        }

        public List<KPIInfo> SearchKPIList(KPISearchInfo kpiSearch, int currentPage, int pageSize, ref int count)
        {
            List<KPIInfo> kpiList = new List<KPIInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = ShopMssqlHelper.TablePrefix + "KPI";
            class2.Fields = "*";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[ID]";
            class2.OrderType = OrderType.Desc;
            this.PrepareCondition(class2.MssqlCondition, kpiSearch);
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader dr = class2.ExecuteReader())
            {
                this.PrepareModel(dr, kpiList);
            }
            return kpiList;
        }

        public void PrepareCondition(MssqlCondition mssqlCondition, KPISearchInfo kpiSearch)
        {
            mssqlCondition.Add("[Id]", kpiSearch.IdStr, ConditionType.In);
            mssqlCondition.Add("[ParentID]", kpiSearch.ParentId, ConditionType.In);
            mssqlCondition.Add("[Name]", kpiSearch.Name, ConditionType.Like);
            mssqlCondition.Add("[CompanyId]", kpiSearch.CompanyID, ConditionType.In);
            mssqlCondition.Add("[State]", kpiSearch.State, ConditionType.Equal);
        }
    }
}
