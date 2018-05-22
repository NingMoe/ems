using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.DAL
{
    public sealed class EvaluateNameDAL : IEvaluateName
    {
        public int AddEvaluateName(EvaluateNameInfo evaluateName)
        {
            SqlParameter[] pt = new SqlParameter[] {
                new SqlParameter("@evaluateName", SqlDbType.NVarChar),
                new SqlParameter("@startDate", SqlDbType.DateTime),
                new SqlParameter("@endDate", SqlDbType.DateTime),
                new SqlParameter("@companyId", SqlDbType.Int),
                new SqlParameter("@state", SqlDbType.Int)
            };

            pt[0].Value = evaluateName.EvaluateName;
            pt[1].Value = evaluateName.StartDate;
            pt[2].Value = evaluateName.EndDate;
            pt[3].Value = evaluateName.CompanyId;
            pt[4].Value = evaluateName.State;
            return Convert.ToInt32(ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "AddEvaluateName", pt));
        }

        public void UpdateEvaluateName(EvaluateNameInfo evaluateName)
        {
            SqlParameter[] pt = new SqlParameter[] {
                new SqlParameter("@id", SqlDbType.Int),
                new SqlParameter("@evaluateName", SqlDbType.NVarChar),
                new SqlParameter("@startDate", SqlDbType.DateTime),
                new SqlParameter("@endDate", SqlDbType.DateTime),
                new SqlParameter("@companyId", SqlDbType.Int),
                new SqlParameter("@state", SqlDbType.Int)
            };

            pt[0].Value = evaluateName.ID;
            pt[1].Value = evaluateName.EvaluateName;
            pt[2].Value = evaluateName.StartDate;
            pt[3].Value = evaluateName.EndDate;
            pt[4].Value = evaluateName.CompanyId;
            pt[5].Value = evaluateName.State;

            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "UpdateEvaluateName", pt);
        }

        public void DeleteEvaluateName(string strID)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@strID", SqlDbType.NVarChar) };
            pt[0].Value = strID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteEvaluateName", pt);
        }

        public void DeleteEvaluateNameByCompanyID(string companyID)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@companyID", SqlDbType.VarChar) };
            pt[0].Value = companyID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteEvaluateNameByCompanyID", pt);
        }

        public EvaluateNameInfo ReadEvaluateName(int id)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@id", SqlDbType.NVarChar) };
            pt[0].Value = id;
            EvaluateNameInfo info = new EvaluateNameInfo();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadEvaluateName", pt))
            {
                if (dr.Read())
                {
                    info.ID = int.Parse(dr["ID"].ToString());
                    info.EvaluateName = dr["EvaluateName"].ToString();
                    info.StartDate = dr["StartDate"].ToString();
                    info.EndDate = dr["EndDate"].ToString();
                    info.Date = dr["Date"].ToString();
                    info.CompanyId = int.Parse(dr["CompanyId"].ToString());
                    info.State = int.Parse(dr["State"].ToString());
                }
            }
            return info;
        }

        public List<EvaluateNameInfo> SearchEvaluateNameList(EvaluateNameInfo evaluateName)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, evaluateName);
            List<EvaluateNameInfo> evaluateNameList = new List<EvaluateNameInfo>();
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@condition", SqlDbType.NVarChar) };
            pt[0].Value = mssqlCondition.ToString();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "SearchEvaluateNameList", pt))
            {
                this.PrepareModel(dr, evaluateNameList);
            }
            return evaluateNameList;
        }

        public List<EvaluateNameInfo> SearchEvaluateNameList(EvaluateNameInfo evaluateName, int currentPage, int pageSize, ref int count)
        {
            List<EvaluateNameInfo> evaluateNameList = new List<EvaluateNameInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = ShopMssqlHelper.TablePrefix + "EvaluateName";
            class2.Fields = "[Id],[EvaluateName],CONVERT(varchar(100), [StartDate], 23) as [StartDate],CONVERT(varchar(100), [EndDate], 23) as [EndDate],CONVERT(varchar(100), [Date], 23) as [Date],[CompanyId],[State]";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[ID]";
            class2.OrderType = OrderType.Desc;
            this.PrepareCondition(class2.MssqlCondition, evaluateName);
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader dr = class2.ExecuteReader())
            {
                this.PrepareModel(dr, evaluateNameList);
            }
            return evaluateNameList;
        }

        public void PrepareCondition(MssqlCondition mssqlCondition, EvaluateNameInfo evaluateName)
        {
            mssqlCondition.Add("[CompanyId]", evaluateName.CompanyIdCondition, ConditionType.In);
            mssqlCondition.Add("[State]", evaluateName.State, ConditionType.Equal);
        }

        public void PrepareModel(SqlDataReader dr, List<EvaluateNameInfo> evaluateNameList)
        {
            while (dr.Read())
            {
                EvaluateNameInfo info = new EvaluateNameInfo();
                {
                    info.ID = int.Parse(dr["ID"].ToString());
                    info.EvaluateName = dr["EvaluateName"].ToString();
                    info.StartDate = dr["StartDate"].ToString();
                    info.EndDate = dr["EndDate"].ToString();
                    info.Date = dr["Date"].ToString();
                    info.CompanyId = int.Parse(dr["CompanyId"].ToString());
                    info.State = int.Parse(dr["State"].ToString());
                }
                evaluateNameList.Add(info);
            }
        }
    }
}
