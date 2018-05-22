using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.DAL
{
    public sealed class KPIEvaluateDAL : IKPIEvaluate
    {
        public int AddKPIEvaluate(KPIEvaluateInfo kpiEvaluate)
        {
            SqlParameter[] pt = new SqlParameter[] {
                new SqlParameter("@userId", SqlDbType.Int),
                new SqlParameter("@postId", SqlDbType.Int),
                new SqlParameter("@kPIId", SqlDbType.Int),
                new SqlParameter("@scorse", SqlDbType.Float),
                new SqlParameter("@rate", SqlDbType.Int),
                new SqlParameter("@state", SqlDbType.Int),
                //new SqlParameter("@evaluateDate", SqlDbType.NVarChar),
                new SqlParameter("@evaluateNameId", SqlDbType.Int),
                new SqlParameter("@evaluateUserId", SqlDbType.Int)
            };

            pt[0].Value = kpiEvaluate.UserId;
            pt[1].Value = kpiEvaluate.PostId;
            pt[2].Value = kpiEvaluate.KPIId;
            pt[3].Value = kpiEvaluate.Scorse;
            pt[4].Value = kpiEvaluate.Rate;
            pt[5].Value = kpiEvaluate.State;
            //pt[6].Value = kpiEvaluate.EvaluateDate;
            pt[6].Value = kpiEvaluate.EvaluateNameId;
            pt[7].Value = kpiEvaluate.EvaluateUserId;
            return Convert.ToInt32(ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "AddKPIEvaluate", pt));
        }

        public void UpdateKPIEvaluate(KPIEvaluateInfo kpiEvaluate)
        {
            SqlParameter[] pt = new SqlParameter[] {
                new SqlParameter("@id", SqlDbType.Int),
                new SqlParameter("@scorse", SqlDbType.Float),
                new SqlParameter("@rate", SqlDbType.Int),
                new SqlParameter("@state", SqlDbType.Int),
                //new SqlParameter("@evaluateDate", SqlDbType.NVarChar),
                new SqlParameter("@evaluateNameId", SqlDbType.Int),
                new SqlParameter("@evaluateUserId", SqlDbType.Int)
            };

            pt[0].Value = kpiEvaluate.ID;
            pt[1].Value = kpiEvaluate.Scorse;
            pt[2].Value = kpiEvaluate.Rate;
            pt[3].Value = kpiEvaluate.State;
            //pt[4].Value = kpiEvaluate.EvaluateDate;
            pt[4].Value = kpiEvaluate.EvaluateNameId;
            pt[5].Value = kpiEvaluate.EvaluateUserId;

            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "UpdateKPIEvaluate", pt);
        }

        public void DeleteKPIEvaluate(string strID)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@strID", SqlDbType.NVarChar) };
            pt[0].Value = strID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteKPIEvaluate", pt);
        }

        /// <summary>
        /// ¸ü¸ÄÆÀ¹À¼ÇÂ¼×´Ì¬
        /// </summary>
        /// <param name="evaluateNameId"></param>
        /// <param name="state"></param>
        public void ChangeKPIEvaluateState(string evaluateNameId, DataState state)
        {
            SqlParameter[] pt = new SqlParameter[] { 
                new SqlParameter("@evaluateNameId", SqlDbType.VarChar),
                new SqlParameter("@state", SqlDbType.Int)
            };
            pt[0].Value = evaluateNameId;
            pt[1].Value = (int)state;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "ChangeKPIEvaluateStateByEvaluateNameId", pt);
        }

        public void DeleteKPIEvaluate(int userId, int postId, int evaluateNameId)
        {
            SqlParameter[] pt = new SqlParameter[] { 
                new SqlParameter("@userId", SqlDbType.Int),
                new SqlParameter("@postId", SqlDbType.Int),
                new SqlParameter("@evaluateNameId", SqlDbType.Int)
            };
            pt[0].Value = userId;
            pt[1].Value = postId;
            pt[2].Value = evaluateNameId;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "CompletelyDeleteKPIEvaluate", pt);
        }

        public void DeleteKPIEvaluateByUserID(string userID)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@userID", SqlDbType.VarChar) };
            pt[0].Value = userID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteKPIEvaluateByuserID", pt);
        }

        public KPIEvaluateInfo ReadKPIEvaluate(int id)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@id", SqlDbType.NVarChar) };
            pt[0].Value = id;
            KPIEvaluateInfo info = new KPIEvaluateInfo();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadKPIEvaluate", pt))
            {
                if (dr.Read())
                {
                    info.ID = int.Parse(dr["ID"].ToString());
                    info.UserId = int.Parse(dr["ParentId"].ToString());
                    info.PostId = int.Parse(dr["PostId"].ToString());
                    info.KPIId = int.Parse(dr["KPIId"].ToString());
                    info.Scorse = float.Parse(dr["Scorse"].ToString());
                    info.Rate = int.Parse(dr["Rate"].ToString());
                    info.State = int.Parse(dr["State"].ToString());
                    info.EvaluateDate = dr["EvaluateDate"].ToString();
                    info.EvaluateNameId = int.Parse(dr["EvaluateNameId"].ToString());
                    info.AddDate = Convert.ToDateTime(dr["AddDate"].ToString());
                    info.EvaluateUserId = int.Parse(dr["EvaluateUserId"].ToString());
                }
            }
            return info;
        }

        public void PrepareModel(SqlDataReader dr, List<KPIEvaluateInfo> kpiEvaluateList)
        {
            while (dr.Read())
            {
                KPIEvaluateInfo info = new KPIEvaluateInfo();
                {
                    info.ID = int.Parse(dr["ID"].ToString());
                    info.UserId = int.Parse(dr["UserId"].ToString());
                    info.PostId = int.Parse(dr["PostId"].ToString());
                    info.KPIId = int.Parse(dr["KPIId"].ToString());
                    info.Scorse = float.Parse(dr["Scorse"].ToString());
                    info.Rate = int.Parse(dr["Rate"].ToString());
                    info.State = int.Parse(dr["State"].ToString());
                    info.EvaluateDate = dr["EvaluateDate"].ToString();
                    info.EvaluateNameId = int.Parse(dr["EvaluateNameId"].ToString());
                    info.AddDate = Convert.ToDateTime(dr["AddDate"].ToString());
                    info.EvaluateUserId = int.Parse(dr["EvaluateUserId"].ToString());
                }
                kpiEvaluateList.Add(info);
            }
        }

        public List<KPIEvaluateReportInfo> KPIEvaluateReportList(KPIEvaluateSearchInfo kpiEvaluateSearch)
        {
            List<KPIEvaluateReportInfo> reportList = new List<KPIEvaluateReportInfo>();
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, kpiEvaluateSearch);
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@condition", SqlDbType.NVarChar) };
            pt[0].Value = mssqlCondition.ToString();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "NewKPIEvaluateReport", pt))
            {
                while (dr.Read())
                {
                    KPIEvaluateReportInfo info = new KPIEvaluateReportInfo();
                    {
                        info.EvaluateNameId = int.Parse(dr["EvaluateNameId"].ToString());
                        //if (dr["UserId"].ToString() == null)
                        //    info.EvaluationName = string.Empty;
                        //else
                        info.EvaluateName = dr["EvaluateName"].ToString();
                        info.UserId = int.Parse(dr["UserId"].ToString());
                        info.UserName = dr["UserName"].ToString();
                        info.PostId = int.Parse(dr["PostId"].ToString());
                        info.PostName = dr["PostName"].ToString();
                        info.CompleteNum = int.Parse(dr["CompleteNum"].ToString());
                        if (dr["WorkingPostKPINum"] == null)
                        {
                            info.WorkingPostKPINum = 0;
                            info.Rate = 0;
                        }
                        else
                        {
                            info.WorkingPostKPINum = int.Parse(dr["WorkingPostKPINum"].ToString());
                            info.Rate = info.CompleteNum * 100 / info.WorkingPostKPINum;
                        }
                        info.TempNum = int.Parse(dr["TempKPINum"].ToString());
                        info.FixedNum = int.Parse(dr["FixKPINum"].ToString());
                        info.TotalScore = int.Parse(dr["TotalScore"].ToString());
                    }
                    reportList.Add(info);
                }
            }
            return reportList;
        }

        //public List<KPIEvaluateReportInfo> KPIEvaluateReportList(int userId, int postId)
        //{
        //    List<KPIEvaluateReportInfo> reportList = new List<KPIEvaluateReportInfo>();
        //    SqlParameter[] pt = new SqlParameter[] {
        //        new SqlParameter("@userId", SqlDbType.Int),
        //        new SqlParameter("@postId", SqlDbType.Int)
        //    };
        //    pt[0].Value = userId;
        //    pt[1].Value = postId;

        //    using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "KPIEvaluateReport", pt))
        //    {
        //        while (dr.Read())
        //        {
        //            KPIEvaluateReportInfo info = new KPIEvaluateReportInfo();
        //            {
        //                if (postId > 0)
        //                {
        //                    info.PostId = postId;
        //                    info.UserId = int.Parse(dr["UserId"].ToString());
        //                }
        //                else
        //                {
        //                    info.PostId = int.Parse(dr["PostId"].ToString());
        //                    info.UserId = userId;
        //                }
        //                info.TempNum = int.Parse(dr["TempNum"].ToString());
        //                info.FixedNum = int.Parse(dr["FixedNum"].ToString());
        //                info.EvaluationName = dr["EvaluateName"].ToString();
        //            }
        //            reportList.Add(info);
        //        }
        //    }
        //    return reportList;
        //}

        public List<KPIEvaluateInfo> SearchKPIEvaluateList(KPIEvaluateSearchInfo kpiEvaluateSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, kpiEvaluateSearch);
            List<KPIEvaluateInfo> kpiEvaluateList = new List<KPIEvaluateInfo>();
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@condition", SqlDbType.NVarChar) };
            pt[0].Value = mssqlCondition.ToString();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "SearchKPIEvaluateList", pt))
            {
                this.PrepareModel(dr, kpiEvaluateList);
            }
            return kpiEvaluateList;
        }

        public List<KPIEvaluateInfo> SearchFixedKPIEvaluateList(KPIEvaluateSearchInfo kpiEvaluateSearch)
        {
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, kpiEvaluateSearch);
            List<KPIEvaluateInfo> kpiEvaluateList = new List<KPIEvaluateInfo>();
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@condition", SqlDbType.NVarChar) };
            pt[0].Value = mssqlCondition.ToString();
            using (SqlDataReader dr = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "SearchFixedKPIEvaluateList", pt))
            {
                while (dr.Read())
                {
                    KPIEvaluateInfo info = new KPIEvaluateInfo();
                    {
                        info.KPIId = int.Parse(dr["KPIId"].ToString());
                        info.Rate = int.Parse(dr["Rate"].ToString());
                    }
                    kpiEvaluateList.Add(info);
                }
            }
            return kpiEvaluateList;
        }

        public List<KPIEvaluateInfo> SearchKPIEvaluateList(KPIEvaluateSearchInfo kpiEvaluateSearch, int currentPage, int pageSize, ref int count)
        {
            List<KPIEvaluateInfo> kpiEvaluateList = new List<KPIEvaluateInfo>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = ShopMssqlHelper.TablePrefix + "KPIEvaluate";
            class2.Fields = "*";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[Id]";
            class2.OrderType = OrderType.Asc;
            this.PrepareCondition(class2.MssqlCondition, kpiEvaluateSearch);
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader dr = class2.ExecuteReader())
            {
                this.PrepareModel(dr, kpiEvaluateList);
            }
            return kpiEvaluateList;
        }

        public void PrepareCondition(MssqlCondition mssqlCondition, KPIEvaluateSearchInfo kpiEvaluateSearch)
        {
            mssqlCondition.Add("[KPIId]", kpiEvaluateSearch.KPIdStr, ConditionType.In);
            mssqlCondition.Add("[UserId]", kpiEvaluateSearch.UserId, ConditionType.In);
            mssqlCondition.Add("[EvaluateDate]", kpiEvaluateSearch.EvaluateDate, ConditionType.Equal);
            mssqlCondition.Add("[EvaluateNameId]", kpiEvaluateSearch.EvaluateNameId, ConditionType.Equal);
            mssqlCondition.Add("[EvaluateUserId]", kpiEvaluateSearch.EvaluateUserId, ConditionType.Equal);
            mssqlCondition.Add("[PostId]", kpiEvaluateSearch.PostId, ConditionType.In);
            mssqlCondition.Add(kpiEvaluateSearch.Condition);
        }
    }
}
