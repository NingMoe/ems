using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

namespace XueFuShop.DAL.DALCourse
{
    public class DALMXBrand
    {
        //读取信息
        public SqlDataReader GetInfor(Models.Course.MXBrand Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [MXBrand] where  BrandId=@BrandId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        public int InsertInfor(Models.Course.MXBrand Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into [MXBrand] ( BrandName ) values(@BrandName)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return XueFuShop.Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        public int UpdateInfor(Models.Course.MXBrand Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [MXBrand] set BrandName=@BrandName where BrandId=@BrandId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(Models.Course.MXBrand Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@BrandId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@BrandName",SqlDbType .VarChar,50)
                                };
            par[0].Value = Model.BrandId;
            par[1].Value = Model.BrandName;
            return par;
        }
    }
}
