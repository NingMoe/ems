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
    public class DALMXTestPaper
    {
        //读取信息
        public SqlDataReader GetInfor(Models.Course.MXTestPaper Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [MXQuestion] where  TestPaperId=@TestPaperId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        public int InsertInfor(Models.Course.MXTestPaper Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into [MXQuestion] ( PaperName,CateId,QuestionStyle,QuestionId,Locked ) values(@PaperName,@CateId,@QuestionStyle,@QuestionId,@Locked)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return XueFuShop.Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        public int UpdateInfor(Models.Course.MXTestPaper Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [MXQuestion] set PaperName=@PaperName,CateId=@CateId,QuestionStyle=@QuestionStyle,QuestionId=@QuestionId,Locked=@Locked where TestPaperId=@TestPaperId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(Models.Course.MXTestPaper Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@TestPaperId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@PaperName",SqlDbType .VarChar,50),
                                    new SqlParameter ("@CateId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@QuestionStyle",SqlDbType .VarChar,50),
                                    new SqlParameter ("@QuestionId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@Locked",SqlDbType .VarChar,50)
                                };
            par[0].Value = Model.TestPaperId;
            par[1].Value = Model.PaperName;
            par[2].Value = Model.CateId;
            par[3].Value = Model.QuestionStyle;
            par[4].Value = Model.QuestionId;
            par[5].Value = Model.Locked;
            return par;
        }
    }
}
