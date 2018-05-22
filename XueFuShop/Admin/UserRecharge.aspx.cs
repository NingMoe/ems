using System;
using System.Data;
using XueFuShop.Pages;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Admin
{
    public partial class UserRecharge : AdminBasePage
    {

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("DeleteUserRecharge", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                UserRechargeBLL.DeleteUserRecharge(intsForm, 0);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("UserRecharge"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("ReadUserRecharge", PowerCheckType.Single);
                UserRechargeSearchInfo userRecharge = new UserRechargeSearchInfo();
                userRecharge.Number = RequestHelper.GetQueryString<string>("Number");
                userRecharge.StartRechargeDate = RequestHelper.GetQueryString<DateTime>("StartRechargeDate");
                userRecharge.EndRechargeDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndRechargeDate"));
                userRecharge.IsFinish = RequestHelper.GetQueryString<int>("IsFinish");
                userRecharge.UserName = RequestHelper.GetQueryString<string>("UserName");
                this.Number.Text = userRecharge.Number;
                this.StartRechargeDate.Text = RequestHelper.GetQueryString<string>("StartRechargeDate");
                this.EndRechargeDate.Text = RequestHelper.GetQueryString<string>("EndRechargeDate");
                this.IsFinish.Text = RequestHelper.GetQueryString<string>("IsFinish");
                this.UserName.Text = RequestHelper.GetQueryString<string>("UserName");
                base.BindControl(UserRechargeBLL.SearchUserRechargeList(base.CurrentPage, base.PageSize, userRecharge, ref this.Count), this.RecordList, this.MyPager);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ResponseHelper.Redirect((((("UserRecharge.aspx?Action=search&" + "Number=" + this.Number.Text + "&") + "StartRechargeDate=" + this.StartRechargeDate.Text + "&") + "EndRechargeDate=" + this.EndRechargeDate.Text + "&") + "IsFinish=" + this.IsFinish.Text + "&") + "UserName=" + this.UserName.Text + "&");
        }
    }
}
