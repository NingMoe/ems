using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.DAL
{
    public class DALMXItem
    {
        public int insert(Item mym)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("insert into [MXItem] ([ItemName],[ParentId]) values (@ItemName,@ParentId)");

            SqlParameter[] par = {
                             
                              new SqlParameter("@ItemName",SqlDbType.VarChar,50),

                              new SqlParameter("@ParentId",SqlDbType.Int,4),

                              new SqlParameter("@OrderNum",SqlDbType.Int,4),

                              new SqlParameter("@State",SqlDbType.Int,4)
                           
                             };



            par[0].Value = mym.ItemName;

            par[1].Value = mym.ParentId;

            par[2].Value = mym.OrderNum;

            par[3].Value = mym.State;

            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        public SqlDataReader GetItemName(int ItemId)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("select * from [MXItem] where [MXItemId]=" + ItemId + " order by [OrderNum]");

            return Common.DbHelperSQL.ExecuteReader(sql.ToString());
        }

        public SqlDataReader GetItemInfor(int ParentId)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("select * from [MXItem] where [ParentId]=" + ParentId+" order by [OrderNum]");

            return Common.DbHelperSQL.ExecuteReader(sql.ToString());
        }

        public SqlDataReader GetItemInfor(int ParentId, int State)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("select * from [MXItem] where [ParentId]=" + ParentId + " and [State]=" + State + " order by [OrderNum]");

            return Common.DbHelperSQL.ExecuteReader(sql.ToString());
        }

        public int UpdateItem(Item mym)
        {
            string sql = "update MXItem set ItemName='" + mym.ItemName + "' where MXItemId=" + mym.ItemId + "";
            return Common.DB.ExecuteSql(sql);
        }

        public int DeleteItem(int ItemId)
        {
            string sql = "delete from MXItem where MXItemId=" + ItemId;
            return Common.DB.ExecuteSql(sql);
        }
    }
}
