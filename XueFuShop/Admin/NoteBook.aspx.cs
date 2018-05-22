using System;
using XueFuShop.Common;
using XueFuShop.Pages;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Admin
{
    public partial class NoteBook : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                AdminInfo info = AdminBLL.ReadAdmin(Cookies.Admin.GetAdminID(false));
                this.NoteBookContent.Text = info.NoteBook;
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            AdminInfo admin = AdminBLL.ReadAdmin(Cookies.Admin.GetAdminID(false));
            admin.NoteBook = this.NoteBookContent.Text;
            AdminBLL.UpdateAdmin(admin);
            AdminBasePage.Alert(ShopLanguage.ReadLanguage("UpdateOK"), RequestHelper.RawUrl);
        }
    }
}
