using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using XueFuShop.Common;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFu.EntLib;

namespace XueFuShop.Web
{
    public partial class GroupBuy : XueFuShop.Pages.AdminBasePage
	{
		/// <summary>
		/// 页面加载方法
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				CheckAdminPower("ReadGroupBuy", PowerCheckType.Single);
                BindControl(GroupBuyBLL.ReadGroupBuyList(CurrentPage, PageSize, ref Count), RecordList, MyPager);
			}
		}
	
		/// <summary>
		/// 删除按钮点击方法
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void DeleteButton_Click(object sender, EventArgs e)
		{
			CheckAdminPower("DeleteGroupBuy", PowerCheckType.Single);
			string deleteID = RequestHelper.GetIntsForm("SelectID");
			if(deleteID != string.Empty)
			{
				GroupBuyBLL.DeleteGroupBuy(deleteID);
				AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("GroupBuy"), deleteID);
				ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
			}
		}
	}
}