using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.Design;
using System.Globalization;

namespace FredCK.FCKeditorV2
{
    public class FCKeditorDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            FCKeditor component = (FCKeditor)base.Component;
            return string.Format(CultureInfo.InvariantCulture, "<div><table width=\"{0}\" height=\"{1}\" bgcolor=\"#f5f5f5\" bordercolor=\"#c7c7c7\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\"><tr><td valign=\"middle\" align=\"center\">FCKeditor V2 - <b>{2}</b></td></tr></table></div>", new object[] { component.Width, component.Height, component.ID });
        }
    }
}
