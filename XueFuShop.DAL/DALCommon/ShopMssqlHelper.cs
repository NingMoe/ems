using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XueFuShop.Common;
using System.Data.SqlClient;

namespace XueFuShop.DAL.DALCommon
{
    public sealed class ShopMssqlHelper
    {
        // Fields
        private static MssqlHelper mssqlHelper = new MssqlHelper();
        private static string tablePrefix = string.Empty;

        // Methods
        static ShopMssqlHelper()
        {
            mssqlHelper.ConnectionString = PubConstant.ConnectionString; //ConfigurationManager.AppSettings["ConnectionString"];
            tablePrefix = ConfigurationManager.AppSettings["TablePrefix"];
        }

        public static DataTable ExecuteDataTable(string storedProcName)
        {
            return mssqlHelper.ExecuteDataTable(storedProcName);
        }

        public static DataTable ExecuteDataTable(string storedProcName, SqlParameter[] pt)
        {
            return mssqlHelper.ExecuteDataTable(storedProcName, pt);
        }

        public static void ExecuteNonQuery(string storedProcName)
        {
            mssqlHelper.ExecuteNonQuery(storedProcName);
        }

        public static void ExecuteNonQuery(string storedProcName, SqlParameter[] pt)
        {
            mssqlHelper.ExecuteNonQuery(storedProcName, pt);
        }

        public static SqlDataReader ExecuteReader(string storedProcName)
        {
            return mssqlHelper.ExecuteReader(storedProcName);
        }

        public static SqlDataReader ExecuteReader(string storedProcName, SqlParameter[] pt)
        {
            return mssqlHelper.ExecuteReader(storedProcName, pt);
        }

        public static object ExecuteScalar(string storedProcName)
        {
            return mssqlHelper.ExecuteScalar(storedProcName);
        }

        public static object ExecuteScalar(string storedProcName, SqlParameter[] pt)
        {
            return mssqlHelper.ExecuteScalar(storedProcName, pt);
        }

        // Properties
        public static string TablePrefix
        {
            get { return tablePrefix; }
            set { tablePrefix = value; }
        }
    }
}
