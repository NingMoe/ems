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
    public class RenZhengCateDAL : IRenZhengCate
    {
        public List<RenZhengCateInfo> ReadRenZhengCateList()
        {
            List<RenZhengCateInfo> RenZhengCateList = new List<RenZhengCateInfo>();
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [" + ShopMssqlHelper.TablePrefix + "RenZhengCate] Where Del=0 Order by [Order]");
            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                this.PrepareModel(reader, RenZhengCateList);
            }
            return RenZhengCateList;
        }

        public void PrepareModel(SqlDataReader dr, List<RenZhengCateInfo> RenZhengCateList)
        {
            while (dr.Read())
            {
                RenZhengCateInfo item = new RenZhengCateInfo();
                item.Id = int.Parse(dr["Id"].ToString());
                item.CateId = int.Parse(dr["CateId"].ToString());
                item.PostId = int.Parse(dr["PostId"].ToString());
                item.Del = int.Parse(dr["Del"].ToString());
                item.Order = int.Parse(dr["Order"].ToString());
                RenZhengCateList.Add(item);
            }
            dr.Close();
            dr.Dispose();
        }
    }
}
