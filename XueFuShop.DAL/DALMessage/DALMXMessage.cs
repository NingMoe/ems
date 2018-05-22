using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.DAL
{
    public class DALMXMessage
    {
        public int InsertMessageInfor(Message mes)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into MXMessage(MessageTitle,MessageContent) values(@Messagetitle,@MessageContent)");

            SqlParameter[] pra = { 
                               new SqlParameter("@MessageTitle",SqlDbType.VarChar,50),
                               new SqlParameter("@MessageContent",SqlDbType.VarChar,250)
                               };
            pra[0].Value = mes.MessageTitle;
            pra[1].Value = mes.MessageContent;

            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), pra);
        }

        public int UpdateMessageInfor(Message mes)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update MXMessage set MessageTitle=@MessageTitle,MessageContent=@MessageContent where MXMessageId=@MessageId");

            SqlParameter[] pra = { 
                               new SqlParameter("@MessageId",SqlDbType.Int,4),
                               new SqlParameter("@MessageTitle",SqlDbType.VarChar,200),
                               new SqlParameter("@MessageContent",SqlDbType.Int,4)
                               
                               };
            pra[0].Value = mes.MessageId;
            pra[1].Value = mes.MessageTitle;
            pra[2].Value = mes.MessageContent;

            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), pra);

        }

        public int DeleteMessageInfor(Message mes)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("delete from MXMessage where MXMessageId=@MessageId");

            SqlParameter[] pra = { 
                               new SqlParameter("@MessageId",SqlDbType.Int,4)
                               };
            pra[0].Value = mes.MessageId;

            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), pra);

        }

        public DataSet ReadMessageInfor(int pageindex, int pagesize, string table)
        {
            string sql = "select * from MXMessage";
            DataSet ds = Common.DB.PagedataSet(sql, pageindex, pagesize, table);
            return ds;

        }

        public int Countmessage()
        {
            string sqlstr = "select count(*) from MXMessage ";
            int result;
            if (Common.DB.ExecuteScalar(sqlstr) == null)
            {
                result = 0;

            }
            else
            {
                result = (int)Common.DB.ExecuteScalar(sqlstr);
            }
            return result;

        }

        public DataSet GetMessageChecked(int pageindex, int pagesize, string table)
        {
            //StringBuilder sql = new StringBuilder();
            //sql.Append("select * from message where _state=" + 1 + "");
            //return Common.DbHelperSQL.Query(sql.ToString());

            string sql = "select * from MXMessage where IsChecked=1";
            DataSet ds = Common.DB.PagedataSet(sql, pageindex, pagesize, table);
            return ds;

        }

        public int CountMessageChecked()
        {
            string sqlstr = "select count(*) from MXMessage where IsChecked=1";
            int result;
            if (Common.DB.ExecuteScalar(sqlstr) == null)
            {
                result = 0;

            }
            else
            {
                result = (int)Common.DB.ExecuteScalar(sqlstr);
            }
            return result;
        }

        public DataSet GetMessageLikeTittle(int pageindex, int pagesize, string table, string title)
        {
            string sql = "select * from MXMessage where MessageTitle like '%" + title + "%'";
            DataSet ds = Common.DB.PagedataSet(sql, pageindex, pagesize, table);
            return ds;

        }

        public int CountMessageLikeTittle(string title)
        {

            string sqlstr = "select count(*) from MXMessage where MessageTitle like '%" + title + "%' ";
            int result;
            if (Common.DB.ExecuteScalar(sqlstr) == null)
            {
                result = 0;

            }
            else
            {
                result = (int)Common.DB.ExecuteScalar(sqlstr);
            }
            return result;
        }

        public SqlDataReader GetMessageInfor(Message mes)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from MXMessage where MXMessageId=@MessageId");

            SqlParameter[] pra = {                                 
                                 new SqlParameter("@MessageId",SqlDbType.Int,4)                                 
                                 };
            pra[0].Value = mes.MessageId;
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), pra);
        }

        public int UpdateMessageChecked(Message mes)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update MXMessage set IsChecked=@IsChecked where MXMessageId=@MessageId");

            SqlParameter[] pra = { 
                                 
                                     new SqlParameter("@IsChecked",SqlDbType.Int,4),
                                     new SqlParameter("@MessageId",SqlDbType.Int,4)
                                 };
            pra[0].Value = mes.IsChecked;
            pra[1].Value = mes.MessageId;
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), pra);
        }
    }
}
