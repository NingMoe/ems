using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.DAL
{
    public class DALMXNews
    {
        public int InsertUserInfor(News Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into [MXNews] ( [NewsTitle],[CateId],[KeyWords],[Author],[OverView],[NewsContent],[IsLink],[WebUrl],[IsPic],[IsTop],[IsBest],[SmallPic],[BigPic],[IsChecked] ) values(@NewsTitle,@CateId,@KeyWords,@Author,@OverView,@NewsContent,@IsLink,@WebUrl,@IsPic,@IsTop,@IsBest,@SmallPic,@BigPic,@IsChecked)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        //读取新闻信息
        public SqlDataReader GetUserInfor(News Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [MXNews] where  [MXNewsId]=@NewsId");
            SqlParameter[] par ={
                                    new SqlParameter("@NewsId",SqlDbType .Int,4)
                                };
            par[0].Value = Model.NewsId;
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        public SqlDataReader GetUserInfor(int CateId,int Num)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select top "+Num+" * from [MXNews] where  [CateId]=@CateId order by MXNewsId desc");
            SqlParameter[] par ={
                                    new SqlParameter("@CateId",SqlDbType .Int,4)
                                };
            par[0].Value = CateId;
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        public SqlDataReader PicDiv(int CateId,int Num,bool IsBest,bool IsTop)
        {
        StringBuilder sql = new StringBuilder();
            sql.Append("select top "+Num+" * from [MXNews] where  [CateId]=@CateId");
            if(IsBest) sql.Append(" and IsBest=1");
            if(IsTop) sql.Append(" and IsTop=1");
            sql.Append(" order by MXNewsId desc");
            SqlParameter[] par ={
                                    new SqlParameter("@CateId",SqlDbType .Int,4)
                                };
            par[0].Value = CateId;
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        //更新新闻信息
        public int UpdateUserInfor(News Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update MXNews set NewsTitle=@NewsTitle,CateId=@CateId,KeyWords=@KeyWords,Author=@Author,OverView=@OverView,NewsContent=@NewsContent,IsLink=@IsLink,WebUrl=@WebUrl,IsPic=@IsPic,IsTop=@IsTop,IsBest=@IsBest,SmallPic=@SmallPic,BigPic=@BigPic,IsChecked=@IsChecked  where MXNewsId=@NewsId");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        //更新新闻信息
        public int UpdateUserInfor(int NewsId,string Field,string Value)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update MXNews set " + Field + "=" + Value + " where MXNewsId=" + NewsId);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString());
        }

        /// <summary>
        /// 该数据访问对象的属性值装载到数据库更新参数数组
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(News Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@NewsId",SqlDbType.Int,50),
                                    new SqlParameter ("@NewsTitle",SqlDbType.VarChar,50),
                                    new SqlParameter ("@CateId",SqlDbType.Int,50),
                                    new SqlParameter ("@KeyWords",SqlDbType .VarChar,50),
                                    new SqlParameter ("@Author",SqlDbType .VarChar,50),
                                    new SqlParameter ("@NewsContent",SqlDbType .VarChar,50),
                                    new SqlParameter ("@IsLink",SqlDbType.Bit,50),
                                    new SqlParameter ("@WebUrl",SqlDbType .VarChar,100),
                                    new SqlParameter ("@IsPic",SqlDbType.Bit,50),
                                    new SqlParameter ("@IsTop",SqlDbType.Bit,50),
                                    new SqlParameter ("@IsBest",SqlDbType.Bit,50),
                                    new SqlParameter ("@SmallPic",SqlDbType .VarChar,50),
                                    new SqlParameter ("@BigPic",SqlDbType .VarChar,50),
                                    new SqlParameter ("@OverView",SqlDbType .VarChar,50)                                    
                                };
            par[0].Value = Model.NewsId;
            par[1].Value = Model.NewsTitle;
            par[2].Value = Model.CateId;
            par[3].Value = Model.KeyWords;
            par[4].Value = Model.Author;
            par[5].Value = Model.NewsContent;
            par[6].Value = Model.IsLink;
            par[7].Value = Model.WebUrl;
            par[8].Value = Model.IsPic;
            par[9].Value = Model.IsTop;
            par[10].Value = Model.IsBest;
            par[11].Value = Model.SmallPic;
            par[12].Value = Model.BigPic;
            par[13].Value = Model.IsChecked;
            par[14].Value = Model.OverView;
            return par;
        }
        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public News GetModel(System.Data.DataRow row)
        {
            News model = new News();
            if (row != null)
            {
                model.NewsId = int.Parse(row["NewsId"].ToString());
                model.NewsTitle = row["NewsTitle"].ToString();
                model.CateId = int.Parse(row["CateId"].ToString());
                model.KeyWords = row["KeyWords"].ToString();
                model.Author = row["Author"].ToString();
                model.OverView = row["OverView"].ToString();
                model.NewsContent = row["NewsContent"].ToString();
                model.IsLink = bool.Parse(row["IsLink"].ToString());
                model.WebUrl = row["WebUrl"].ToString();
                model.IsPic = bool.Parse(row["IsPic"].ToString());
                model.IsTop = bool.Parse(row["IsTop"].ToString());
                model.IsBest = bool.Parse(row["IsBest"].ToString());
                model.SmallPic = row["SmallPic"].ToString();
                model.BigPic = row["BigPic"].ToString();
                model.IsChecked = bool.Parse(row["IsChecked"].ToString());

                return model;
            }
            else
            {
                return null;
            }
        }
    
    }
}
