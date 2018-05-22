using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;

namespace XueFuShop.DAL.DALCourse
{
    public class DALMXCourse
    {
        //读取指定信息
        public SqlDataReader GetInfor(Models.Course.MXCourse Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [MXCourse] where  CourseId=@CourseId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        public SqlDataReader ReadInfor()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [MXCourse]");
            return Common.DbHelperSQL.ExecuteReader(sql.ToString());
        }

        public SqlDataReader InsertInfor(Models.Course.MXCourse Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into [MXCourse] ( CourseName,CourseScore,CateId,OrderIndex ) values(@CourseName,@CourseScore,@CateId,@OrderIndex)");
            sql.Append("SELECT SCOPE_IDENTITY()");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
           // return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        public int UpdateInfor(Models.Course.MXCourse Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [MXCourse] set  CourseName=@CourseName,CourseScore=@CourseScore,CateId=@CateId,OrderIndex=@OrderIndex where CourseId=@CourseId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(Models.Course.MXCourse Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@CourseId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@CourseName",SqlDbType .VarChar,50),
                                    new SqlParameter ("@CourseScore",SqlDbType .VarChar,50),
                                    new SqlParameter ("@CateId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@OrderIndex",SqlDbType .VarChar,50)
                                };
            par[0].Value = Model.CourseId;
            par[1].Value = Model.CourseName;
            par[2].Value = Model.CourseScore;
            par[3].Value = Model.CateId;
            par[4].Value = Model.OrderIndex;
            return par;
        }
    }
}
