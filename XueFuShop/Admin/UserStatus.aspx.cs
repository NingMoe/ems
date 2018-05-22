using System;
using XueFuShop.Common;
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.BLL;
using XueFuShop.Models;
using System.Data;

namespace XueFuShop.Admin
{

    public partial class UserStatus : AdminBasePage
    {
        protected string result = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("StatisticsUser", PowerCheckType.Single);
                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.StartRegisterDate = RequestHelper.GetQueryString<DateTime>("StartRegisterDate");
                userSearch.EndRegisterDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndRegisterDate"));
                this.StartRegisterDate.Text = RequestHelper.GetQueryString<string>("StartRegisterDate");
                this.EndRegisterDate.Text = RequestHelper.GetQueryString<string>("EndRegisterDate");
                DataTable table = UserBLL.StatisticsUserStatus(userSearch);
                string[] strArray = new string[] { "33FF66", "FF6600", "FFCC33", "CC3399" };
                int index = 0;
                bool flag = false;
                foreach (EnumInfo info2 in EnumHelper.ReadEnumList<UserStatus>())
                {
                    flag = false;
                    foreach (DataRow row in table.Rows)
                    {
                        if (Convert.ToInt16(row["Status"]) == info2.Value)
                        {
                            object result = this.result;
                            this.result = string.Concat(new object[] { result, " <set value='", row["Count"], "' name='", info2.ChineseName, "' color='", strArray[index], "' />" });
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        string str = this.result;
                        this.result = str + " <set value='0' name='" + info2.ChineseName + "' color='" + strArray[index] + "' />";
                    }
                    index++;
                }
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ResponseHelper.Redirect(("UserStatus.aspx?Action=search&" + "StartRegisterDate=" + this.StartRegisterDate.Text + "&") + "EndRegisterDate=" + this.EndRegisterDate.Text);
        }
    }
}
