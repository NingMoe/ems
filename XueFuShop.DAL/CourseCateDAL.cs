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
    public sealed class CourseCateDAL : ICourseCate
    {
        public int AddCourseCate(CourseCateInfo CourseCate)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@ParentCateId", SqlDbType.Int), new SqlParameter("@OrderIndex", SqlDbType.Int), new SqlParameter("@CateName", SqlDbType.NVarChar), new SqlParameter("@CompanyId", SqlDbType.Int), new SqlParameter("@BrandId", SqlDbType.VarChar) };
            pt[0].Value = CourseCate.ParentCateId;
            pt[1].Value = CourseCate.OrderIndex;
            pt[2].Value = CourseCate.CateName;
            pt[3].Value = CourseCate.CompanyId;
            pt[4].Value = CourseCate.BrandId;
            return Convert.ToInt32(ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "AddCourseCate", pt));
        }

        public void DeleteCourseCate(int CateId)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@CateId", SqlDbType.Int) };
            pt[0].Value = CateId;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteCourseCate", pt);
        }

        public void MoveDownCourseCate(int id)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int) };
            pt[0].Value = id;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "MoveDownCourseCate", pt);
        }

        public void MoveUpCourseCate(int id)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int) };
            pt[0].Value = id;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "MoveUpCourseCate", pt);
        }

        public void PrepareCourseCateModel(SqlDataReader dr, List<CourseCateInfo> CourseCateList)
        {
            while (dr.Read())
            {
                CourseCateInfo item = new CourseCateInfo();
                item.CateId = dr.GetInt32(0);
                item.CateName = dr[1].ToString();
                item.ParentCateId = dr.GetInt32(2);
                item.CompanyId = dr.GetInt32(3);
                item.OrderIndex = dr.GetInt32(4);
                item.BrandId = dr["BrandId"].ToString();
                CourseCateList.Add(item);
            }
        }

        public List<CourseCateInfo> ReadCourseCateAllList()
        {
            List<CourseCateInfo> CourseCateList = new List<CourseCateInfo>();
            using (SqlDataReader reader = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadCourseCateAllList"))
            {
                this.PrepareCourseCateModel(reader, CourseCateList);
            }
            return CourseCateList;
        }

        public List<CourseCateInfo> ReadCourseCateAllList(CourseCateInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "CourseCate] ");
            List<CourseCateInfo> CourseCateList = new List<CourseCateInfo>();
            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, Model);

            if (mssqlCondition.ToString() != string.Empty)
            {
                sql.Append("where " + mssqlCondition.ToString());
                sql.Append(" Order by [OrderIndex] ASC,CateId ASC");
                using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
                {
                    this.PrepareCourseCateModel(reader, CourseCateList);
                }
            }
            return CourseCateList;
        }

        public void UpdateCourseCate(CourseCateInfo CourseCate)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@CateId", SqlDbType.Int), new SqlParameter("@ParentCateId", SqlDbType.Int), new SqlParameter("@OrderIndex", SqlDbType.Int), new SqlParameter("@CateName", SqlDbType.NVarChar), new SqlParameter("@CompanyId", SqlDbType.Int), new SqlParameter("@BrandId", SqlDbType.VarChar) };
            pt[0].Value = CourseCate.CateId;
            pt[1].Value = CourseCate.ParentCateId;
            pt[2].Value = CourseCate.OrderIndex;
            pt[3].Value = CourseCate.CateName;
            pt[4].Value = CourseCate.CompanyId;
            pt[5].Value = CourseCate.BrandId;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "UpdateCourseCate", pt);
        }

        public void PrepareCondition(MssqlCondition mssqlCondition, CourseCateInfo Model)
        {
            //mssqlCondition.Add("[CourseId]", Model.Condition, ConditionType.Like);
            //mssqlCondition.Add("[CompanyId]", Model.CateId, ConditionType.Equal);
            if (Model.Field != string.Empty)
            {
                mssqlCondition.Add("[" + Model.Field + "]", Model.Condition, ConditionType.In);
            }
        }
    }
}
