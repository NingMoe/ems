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
    public class DALMXCourseCate
    {
        //读取信息
        public SqlDataReader GetInfor(Models.Course.MXCourseCate Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [MXCourseCate] where  CateId=@CateId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        public int InsertInfor(Models.Course.MXCourseCate Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into [MXCourseCate] ( CateName,ParentCateId,OrderIndex ) values(@CateName,@ParentCateId,@OrderIndex)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return XueFuShop.Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        public int UpdateInfor(Models.Course.MXCourseCate Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [MXCourseCate] set CateName=@CateName,ParentCateId=@ParentCateId,OrderIndex=@OrderIndex where CateId=@CateId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(Models.Course.MXCourseCate Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@CateId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@CateName",SqlDbType .VarChar,50),
                                    new SqlParameter ("@ParentCateId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@OrderIndex",SqlDbType .VarChar,50)
                                };
            par[0].Value = Model.CateId;
            par[1].Value = Model.CateName;
            par[2].Value = Model.ParentCateId;
            par[3].Value = Model.OrderIndex;
            return par;
        }
    }
}
