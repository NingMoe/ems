using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using XueFuShop.BLL;

namespace XueFuShop.Admin
{
    public partial class Left1 : System.Web.UI.Page
    {
        private BLLMXAdminSysItem bll = new BLLMXAdminSysItem();

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        public StringBuilder CreateMenu()
        {
            
            string ID = Request.QueryString["ID"];
            if (ID == null)
            {
                ID = "1";
            }
            DataTable ds = bll.GetItemInforByParentItemId(int.Parse(ID));
            StringBuilder MenuString = new StringBuilder();
            int i=1;
            MenuString.Append("<div id=\"menu\">\r\n");
            MenuString.Append("<div class=\"menuBody\" id=\"MenuBody\">\r\n");
            foreach (DataRow Item in ds.Rows)
            {

                MenuString.Append("<div onclick=\"show('Default" + i + "')\" id=\"Default" + i + "Div\"><img src=\"Style/Icon/" + Item["ItemImage"].ToString() + "-icon.gif\" alt=\"\" />" + Item["ItemName"].ToString() + "</div>\r\n");
                MenuString.Append("<ul id=\"Default" + i + "Menu\"");
                if (i > 1)
                {
                    MenuString.Append(" style=\"display:none\"");
                }
                MenuString.Append(">\r\n");
                int j = 1;
                foreach (DataRow TempMenu in bll.GetItemInforByParentItemId(int.Parse(Item["ItemId"].ToString())).Rows)
                {
                    MenuString.Append("<li id=\"Default" + i + "Menu-" + j + "\" onclick=\"shwoSmall('Default" + i + "Menu-" + j + "')\"><img src=\"Style/Icon/" + TempMenu["ItemImage"].ToString() + "-icon.gif\" alt=\"\" /><a href=\"javascript:goUrl('" + TempMenu["ItemURL"].ToString() + "')\">" + TempMenu["ItemName"].ToString() + "</a></li>\r\n");
                    j += 1;
                }
                if (i < ds.Rows.Count)
                {
                    MenuString.Append("<li class=\"foot\"></li>\r\n");
                }
                MenuString.Append("</ul>\r\n");
                i += 1;
            }
            MenuString.Append("</div>\r\n");
            MenuString.Append("</div>\r\n");
            return MenuString;
        }

        
    }
}
