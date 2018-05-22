using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using XueFuShop.Pages.Controls;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public abstract class PluginsBasePage : Page
    {        
        protected Foot Foot = null;
        protected Head Head = null;
        protected Top Top = null;
        
        protected PluginsBasePage()
        {
        }

        private void LoadUserControl(string name)
        {
            PlaceHolder holder = (PlaceHolder)this.Page.FindControl("P" + name);
            if (holder != null)
            {
                string str = name;
                if (str != null)
                {
                    if (!(str == "Head"))
                    {
                        if (str == "Top")
                        {
                            this.Top = (Top)this.Page.LoadControl("/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/Controls/Top.ascx");
                            holder.Controls.Add(this.Top);
                        }
                        else if (str == "Foot")
                        {
                            this.Foot = (Foot)this.Page.LoadControl("/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/Controls/Foot.ascx");
                            holder.Controls.Add(this.Foot);
                        }
                    }
                    else
                    {
                        this.Head = (Head)this.Page.LoadControl("/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/Controls/Head.ascx");
                        holder.Controls.Add(this.Head);
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.LoadUserControl("Head");
            this.LoadUserControl("Top");
            this.LoadUserControl("Foot");
        }
    }
}
