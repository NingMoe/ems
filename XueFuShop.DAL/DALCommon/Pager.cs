using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using XueFuShop.Models;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web;
using System.Xml;
using XueFu.EntLib;
using XueFuShop.Common;

namespace XueFuShop.DAL
{
    public class ShopMssqlPagerClass : MssqlPagerClass
    {

        private int count = 0;


        public override DataTable ExecuteDataTable()
        {
            return ShopMssqlHelper.ExecuteDataTable(ShopMssqlHelper.TablePrefix + "ReadPageList", base.PrepareParameter

    ());
        }

        public override SqlDataReader ExecuteReader()
        {
            return ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadPageList", base.PrepareParameter());
        }


        public int Count
        {
            get
            {
                int num = 0;
                if (this.count != -2147483648)
                {
                    object obj2 = ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "ReadCount", base.PrepareCountParameter());
                    if ((obj2 == null) || (obj2 == DBNull.Value))
                    {
                        return num;
                    }
                    if (obj2.ToString() != "0")
                    {
                        num = Convert.ToInt32(obj2);
                    }
                }
                return num;
            }
            set
            {
                this.count = value;
            }
        }
    }
}
