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
    public class DALMXSpecialCourse
    {
        //读取信息
        public SqlDataReader GetInfor(Models.Course.MXSpecialCourse Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [MXSpecialCourse] where  SpecialCourseId=@SpecialCourseId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        public int InsertInfor(Models.Course.MXSpecialCourse Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into [MXSpecialCourse] ( SpecialCourseId,CompanyId,CourseContent,StartDate,EndDate,Locked ) values(@SpecialCourseId,@CompanyId,@CourseContent,@StartDate,@EndDate,@Locked)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return XueFuShop.Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        public int UpdateInfor(Models.Course.MXSpecialCourse Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [MXSpecialCourse] set CompanyId=@CompanyId,CourseContent=@CourseContent,StartDate=@StartDate,EndDate=@EndDate,Locked=@Locked where SpecialCourseId=@SpecialCourseId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(Models.Course.MXSpecialCourse Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@SpecialCourseId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@CompanyId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@CourseContent",SqlDbType .VarChar,50),
                                    new SqlParameter ("@StartDate",SqlDbType .VarChar,50),
                                    new SqlParameter ("@EndDate",SqlDbType .VarChar,50),
                                    new SqlParameter ("@Locked",SqlDbType .VarChar,50)
                                };
            par[0].Value = Model.SpecialCourseId;
            par[1].Value = Model.CompanyId;
            par[2].Value = Model.CourseContent;
            par[3].Value = Model.StartDate;
            par[4].Value = Model.EndDate;
            par[5].Value = Model.Locked;
            return par;
        }
    }
}
