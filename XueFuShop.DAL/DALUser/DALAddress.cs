using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.DAL
{
    public class DALAddress
    {
        //填写送货地址
        public int InsertAddressInfor(MXAddress aa)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into MXAddress (UserId,Tel,Mobile,Address,Name,Post) values(@UserId,@Tel,@Mobile,@Address,@Name,@Post)");
            SqlParameter[] par ={                                   
                                    new   SqlParameter("@UserId",SqlDbType.Int,4),
                                    new   SqlParameter("@Tel",SqlDbType.VarChar,50),
                                    new   SqlParameter("@Mobile",SqlDbType.VarChar,50),
                                    new   SqlParameter("@Address",SqlDbType.VarChar,50),
                                    new   SqlParameter("@Name",SqlDbType.VarChar,50),
                                    new   SqlParameter("@Post",SqlDbType.VarChar,50)
                                    
                                };
            par[0].Value = aa.UserId;
            par[1].Value = aa.Tel;
            par[2].Value = aa.Mobile;
            par[3].Value = aa.Address;
            par[4].Value = aa.Name;
            par[5].Value = aa.Post;
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }
        //更新地址
        public int UpdateAddressInfor(MXAddress aa)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update MXAddress set Post=@Post,Name=@Name,Mobile=@Mobile,Tel=@Tel,Address=@Address where UserId=@UserId");
            SqlParameter[] par ={
                                    new   SqlParameter("@UserId",SqlDbType.Int,4),
                                    new   SqlParameter("@Tel",SqlDbType.VarChar,50),
                                    new   SqlParameter("@Mobile",SqlDbType.VarChar,50),
                                    new   SqlParameter("@Address",SqlDbType.VarChar,50),
                                    new   SqlParameter("@Name",SqlDbType.VarChar,50),
                                    new   SqlParameter("@Post",SqlDbType.VarChar,50)
                                    
                                };
            par[0].Value = aa.UserId;
            par[1].Value = aa.Tel;
            par[2].Value = aa.Mobile;
            par[3].Value = aa.Address;
            par[4].Value = aa.Name;
            par[5].Value = aa.Post;
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        public int DeleteAddressInfor(MXAddress aa)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("delete from MXAddress where UserId=@UserId");
            SqlParameter[] par = { new SqlParameter("@UserId", SqlDbType.Int, 4) };
            par[0].Value = aa.UserId;
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }
        //查看送货地址
        public SqlDataReader GetAddressUserId(MXAddress aa)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select * from MXAddress where UserId=@UserId ");
            SqlParameter[] par ={
                                    new SqlParameter("@UserId",SqlDbType.Int ,4)
                                };
            par[0].Value = aa.UserId;
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }
        
        //插入用户号于地址表
        public int InsertAddressUserId(MXAddress aa)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into MXAddress (UserId) values(@UserId)");
            SqlParameter[] par = {
                                     new SqlParameter("@UserId",SqlDbType.Int,4)
                                 };
            par[0].Value = aa.UserId;
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }
        
    }
}
