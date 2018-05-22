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
    public class DALMXCourseProcess
    {
        //读取信息
        public SqlDataReader GetInfor(Models.Course.MXCourseProcess Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [MXCourseProcess] where  CourseProcessId=@CourseProcessId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        public int InsertInfor(Models.Course.MXCourseProcess Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into [MXCourseProcess] ( CourseId,UserId,State,Locked ) values(@CourseId,@UserId,@State,@Locked)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return XueFuShop.Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        public int UpdateInfor(Models.Course.MXCourseProcess Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [MXCourseProcess] set PostName=@PostName,UserId=@UserId,State=@State,Locked=@Locked where CourseProcessId=@CourseProcessId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(Models.Course.MXCourseProcess Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@CourseProcessId",SqlDbType .Int,50),
                                    new SqlParameter ("@CourseId",SqlDbType .Int,50),
                                    new SqlParameter ("@UserId",SqlDbType .Int,50),
                                    new SqlParameter ("@State",SqlDbType .Int,50),
                                    new SqlParameter ("@Locked",SqlDbType .Bit,50)
                                };
            par[0].Value = Model.CourseProcessId;
            par[1].Value = Model.CourseId;
            par[2].Value = Model.UserId;
            par[3].Value = Model.State;
            par[4].Value = Model.Locked;
            return par;
        }
    }
}
