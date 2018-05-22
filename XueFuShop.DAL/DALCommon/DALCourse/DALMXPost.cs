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
    public class DALMXPost
    {
        //读取信息
        public SqlDataReader GetInfor(Models.Course.MXPost Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [MXPost] where  PostId=@PostId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        public int InsertInfor(Models.Course.MXPost Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into [MXPost] ( PostName,PostPlan,Locked ) values(@BrandName,@PostPlan,@Locked)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return XueFuShop.Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        public int UpdateInfor(Models.Course.MXPost Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [MXPost] set PostName=@PostName,PostPlan=@PostPlan,Locked=@Locked where PostId=@PostId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(Models.Course.MXPost Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@PostId",SqlDbType .Int,50),
                                    new SqlParameter ("@PostName",SqlDbType .VarChar,50),
                                    new SqlParameter ("@PostPlan",SqlDbType .VarChar,1000),
                                    new SqlParameter ("@Locked",SqlDbType .Bit,50)
                                };
            par[0].Value = Model.PostId;
            par[1].Value = Model.PostName;
            par[2].Value = Model.PostPlan;
            par[3].Value = Model.Locked;
            return par;
        }
    }
}
