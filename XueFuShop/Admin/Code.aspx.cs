using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XueFu.EntLib;
using XueFuShop.Common;

namespace XueFuShop.Admin
{
    public partial class Code : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckCode.CodeDot = ShopConfig.ReadConfigInfo().CodeDot;
            CheckCode.CodeLength = ShopConfig.ReadConfigInfo().CodeLength;
            CheckCode.CodeType = (CodeType)ShopConfig.ReadConfigInfo().CodeType;
            CheckCode.Key = ShopConfig.ReadConfigInfo().SecureKey;
            CheckCode m = new CheckCode();
            m.ProcessRequest(HttpContext.Current);
            this.mm.Text = CheckCode.Key;
        }
    }
}
