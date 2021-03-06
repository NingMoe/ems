using System;
using XueFuShop.Common;
using System.Web.UI.WebControls;
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Admin
{
    public partial class UserConsume : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("StatisticsUser", PowerCheckType.Single);
                this.Sex.DataSource = EnumHelper.ReadEnumList<SexType>();
                this.Sex.DataValueField = "Value";
                this.Sex.DataTextField = "ChineseName";
                this.Sex.DataBind();
                this.Sex.Items.Insert(0, new ListItem("����", string.Empty));
                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.UserName = RequestHelper.GetQueryString<string>("UserName");
                userSearch.Sex = RequestHelper.GetQueryString<int>("Sex");
                DateTime queryString = RequestHelper.GetQueryString<DateTime>("StartDate");
                DateTime endDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndDate"));
                this.UserName.Text = userSearch.UserName;
                this.StartDate.Text = RequestHelper.GetQueryString<string>("StartDate");
                this.EndDate.Text = RequestHelper.GetQueryString<string>("EndDate");
                this.Sex.Text = RequestHelper.GetQueryString<int>("Sex").ToString();
                string orderField = RequestHelper.GetQueryString<string>("UserConsumeType");
                orderField = (orderField == string.Empty) ? "OrderCount" : orderField;
                this.UserConsumeType.Text = orderField;
                base.BindControl(UserBLL.StatisticsUserConsume(base.CurrentPage, base.PageSize, userSearch, ref this.Count, orderField, queryString, endDate), this.RecordList, this.MyPager);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ResponseHelper.Redirect((((("UserConsume.aspx?Action=search&" + "UserName=" + this.UserName.Text + "&") + "Sex=" + this.Sex.Text + "&") + "UserConsumeType=" + this.UserConsumeType.Text + "&") + "StartDate=" + this.StartDate.Text + "&") + "EndDate=" + this.EndDate.Text);
        }
    }
}
