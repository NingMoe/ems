using System;
using XueFuShop.Common;
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Admin
{
    public partial class AdminAdd : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.GroupID.DataSource = AdminGroupBLL.ReadAdminGroupList();
                this.GroupID.DataTextField = "Name";
                this.GroupID.DataValueField = "ID";
                this.GroupID.DataBind();
                int queryString = RequestHelper.GetQueryString<int>("ID");
                if (queryString != -2147483648)
                {
                    base.CheckAdminPower("ReadAdmin", PowerCheckType.Single);
                    AdminInfo info = AdminBLL.ReadAdmin(queryString);
                    this.GroupID.Text = info.GroupID.ToString();
                    this.Name.Text = info.Name;
                    this.Name.Enabled = false;
                    this.Email.Text = info.Email;
                    this.Add.Visible = false;
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
            admin.ID = RequestHelper.GetQueryString<int>("ID");
    }
}