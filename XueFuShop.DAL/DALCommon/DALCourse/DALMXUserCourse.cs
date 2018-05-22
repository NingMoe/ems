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
    public class DALMXUserCourse
    {
        //读取信息
        public SqlDataReader GetInfor(Models.Course.MXUserCourse Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [MXUserCourse] where  UserCourseId=@UserCourseId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        public int InsertInfor(Models.Course.MXUserCourse Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into [MXUserCourse] ( UserId,TrainingName,UserCourse,Locked ) values(@UserId,@TrainingName,@UserCourse,@Locked)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return XueFuShop.Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        public int UpdateInfor(Models.Course.MXUserCourse Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [MXUserCourse] set UserId=@UserId,TrainingName=@TrainingName,UserCourse=@UserCourse,Locked=@Locked where UserCourseId=@UserCourseId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(Models.Course.MXUserCourse Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@UserCourseId",SqlDbType .Int,50),
                                    new SqlParameter ("@UserId",SqlDbType .Int,50),
                                    new SqlParameter ("@TrainingName",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserCourse",SqlDbType .VarChar,1000),
                                    new SqlParameter ("@Locked",SqlDbType .Bit,50)
                                };
            par[0].Value = Model.UserCourseId;
            par[1].Value = Model.UserId;
            par[2].Value = Model.TrainingName;
            par[3].Value = Model.UserCourse;
            par[4].Value = Model.Locked;
            return par;
        }
    }
}
