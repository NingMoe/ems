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
    public class DALMXQuestion
    {
        //读取信息
        public SqlDataReader GetInfor(Models.Course.MXQuestion Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [MXQuestion] where  QuestionId=@QuestionId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        public int InsertInfor(Models.Course.MXQuestion Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into [MXQuestion] ( CateId,Style,Score,Question,A,B,C,D,Answer,Remarks,Checked ) values(@CateId,@Style,@Score,@Question,@A,@B,@C,@D,@Answer,@Remarks,@Checked)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return XueFuShop.Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        public int UpdateInfor(Models.Course.MXQuestion Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update [MXQuestion] set CateId=@CateId,Style=@Style,Score=@Score,Question=@Question,A=@A,B=@B,C=@C,D=@D,Answer=@Answer,Remarks=@Remarks,Checked=@Checked where QuestionId=@QuestionId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(Models.Course.MXQuestion Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@QuestionId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@CateId",SqlDbType .VarChar,50),
                                    new SqlParameter ("@Style",SqlDbType .VarChar,50),
                                    new SqlParameter ("@Score",SqlDbType .VarChar,50),
                                    new SqlParameter ("@Question",SqlDbType .VarChar,50),
                                    new SqlParameter ("@A",SqlDbType .VarChar,50),
                                    new SqlParameter ("@B",SqlDbType .VarChar,50),
                                    new SqlParameter ("@C",SqlDbType .VarChar,50),
                                    new SqlParameter ("@D",SqlDbType .VarChar,50),
                                    new SqlParameter ("@Remarks",SqlDbType .VarChar,1000),
                                    new SqlParameter ("@Checked",SqlDbType .VarChar,50)
                                };
            par[0].Value = Model.QuestionId;
            par[1].Value = Model.CateId;
            par[2].Value = Model.Style;
            par[3].Value = Model.Score;
            par[4].Value = Model.Question;
            par[5].Value = Model.A;
            par[6].Value = Model.B;
            par[7].Value = Model.C;
            par[8].Value = Model.D;
            par[9].Value = Model.Checked;
            return par;
        }
    }
}
