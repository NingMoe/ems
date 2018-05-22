using System;
using System.Data;
using System.Web;
using XueFu.EntLib;
using XueFuShop.Common;

namespace MobileEMS
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
