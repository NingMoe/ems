using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace XueFu.EntLib
{
    [ToolboxData("<{0}:AjaxPager runat=server></{0}:AjaxPager>"), DefaultProperty("")]
    public class AjaxPager : BasePager
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AjaxPagerClass class2 = new AjaxPagerClass();
            base.BasePagerClass = class2;
        }

        protected override void Render(HtmlTextWriter output)
        {
            output.Write(base.BasePagerClass.ShowPage());
        }
    }
}
