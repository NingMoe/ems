using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.DAL
{
    public class SMSRecordDAL : ISMSRecord
    {
        public int AddSMSRecord(SMSRecordInfo Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into [" + ShopMssqlHelper.TablePrefix + "SMSRecord]([Mobile],[VerCode],[DataCreateDate]) values(@Mobile,@VerCode,@DataCreateDate)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return DbSQLHelper.ExecuteSql(sql.ToString(), par);
        }

        public SMSRecordInfo ReadSMSRecord(string Mobile, string Code)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select top 1 * from [" + ShopMssqlHelper.TablePrefix + "SMSRecord] where [Mobile]='" + Mobile + "'");
            if (!string.IsNullOrEmpty(Code))
            {
                sql.Append(" and [VerCode]='" + Code + "'");
            }
            sql.Append(" order by [Id] desc");
            using (SqlDataReader dr = DbSQLHelper.ExecuteReader(sql.ToString()))
            {
                return GetModel(dr);
            }
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(SMSRecordInfo Model)
        {
            SqlParameter[] par ={
                new SqlParameter ("@Mobile",SqlDbType.VarChar),
                new SqlParameter ("@VerCode",SqlDbType.VarChar),
                new SqlParameter ("@DataCreateDate",SqlDbType.DateTime)
                                };
            par[0].Value = Model.Mobile;
            par[1].Value = Model.VerCode;
            par[2].Value = Model.DataCreateDate;
            return par;
        }

        /// <summary>
        /// 返回数据
        /// </summary>
        /// <returns></returns>
        public SMSRecordInfo GetModel(SqlDataReader dr)
        {
            SMSRecordInfo Model = new SMSRecordInfo();
            if (dr.Read())
            {
                Model.Mobile = dr["Mobile"].ToString();
                Model.VerCode = dr["VerCode"].ToString();
                Model.DataCreateDate = Convert.ToDateTime(dr["DataCreateDate"]);
            }
            return Model;
        }
    }
}
