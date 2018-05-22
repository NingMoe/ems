using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XueFuShop.Models;

namespace XueFuShop.DAL
{
    public class DALAdminSysItem
    {
        public int InsertInfor(MXAdminSysItem Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into [MXAdminSysItem] ([ItemName],[ItemURL],[OrderIndex],[ParentItemId],[ItemImage]) values (@ItemName,@ItemURL,@OrderIndex,@ParentItemId,@ItemImage)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        /// <summary>
        /// ����ϵͳ�˵�IDֵ������������
        /// </summary>
        /// <para name="ItemId">ϵͳ�˵�IDֵ</para>
        public DataSet GetItemInforByItemId(int ItemId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [MXAdminSysItem] where [MXItemId]=" + ItemId + " order by [OrderIndex]");
            return Common.DbHelperSQL.Query(sql.ToString());
        }

        /// <summary>
        /// ����ϵͳ�˵� ����ʶ��������
        /// </summary>
        /// <para name="ItemId">ϵͳ�˵�IDֵ</para>
        /// <returns>����SqlDataReader</returns>
        //public SqlDataReader GetItemInforByParentId(int ParentItemId)
        //{
        //    StringBuilder sql = new StringBuilder();
        //    sql.Append("select * from [MXAdminSysItem] where [ParentItemId]=" + ParentItemId + " order by [OrderIndex]");
        //    return Common.DbHelperSQL.ExecuteReader(sql.ToString());
        //}

        /// <summary>
        /// ����ϵͳ�˵� ����ʶ��������
        /// </summary>
        /// <para name="ItemId">ϵͳ�˵�IDֵ</para>
        /// <returns>����DataSet</returns>
        public DataSet GetItemInforByParentId(int ParentItemId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from [MXAdminSysItem] where [ParentItemId]=" + ParentItemId + " order by [OrderIndex]");
            return Common.DbHelperSQL.Query(sql.ToString());
        }

        public int UpdateItem(MXAdminSysItem Model)
        {
            string sql = "update [MXAdminSysItem] set [ItemName]=@ItemName,[ItemURL]=@ItemURL,[ItemImage]=@ItemImage,[OrderIndex]=@OrderIndex,[ParentItemId]=@ParentItemId where ItemId=@ItemId";
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(),par);
        }

        public int DeleteItem(int ItemId)
        {
            string sql = "delete from [MXAdminSysItem] where [ItemId]=" + ItemId;
            return Common.DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// �����ݷ��ʶ��������ֵװ�ص����ݿ���²�������
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(MXAdminSysItem Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@ItemId",SqlDbType .Int,4),
                                    new SqlParameter ("@ItemName",SqlDbType .VarChar,50),
                                    new SqlParameter ("@ParentItemId",SqlDbType .Int,4),
                                    new SqlParameter ("@OrderIndex",SqlDbType .Int,4),
                                    new SqlParameter ("@ItemImage",SqlDbType .Int,4),
                                    new SqlParameter ("@ItemURL",SqlDbType .VarChar,300)
                                };
            par[0].Value = Model.ItemId;
            par[1].Value = Model.ItemName;
            par[2].Value = Model.ParentItemId;
            par[3].Value = Model.OrderIndex;
            par[4].Value = Model.ItemImage;
            par[5].Value = Model.ItemURL;
            return par;
        }
    }
}
