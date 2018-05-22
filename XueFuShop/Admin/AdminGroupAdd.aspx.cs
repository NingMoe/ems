using System;
using XueFuShop.Common;
using System.Xml;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Admin
{
    public partial class AdminGroupAdd : AdminBasePage
    {
        protected List<PowerInfo> channelPowerList = new List<PowerInfo>();
        protected string power = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
                    this.State.Text = info.State.ToString();
                item.XML = node2.InnerXml;

        protected List<PowerInfo> ReadPowerBlock(string xml)
                item.XML = node.InnerXml;

        protected List<PowerInfo> ReadPowerItem(string xml)
                item.Value = node.Attributes["Value"].Value;

        protected void SubmitButton_Click(object sender, EventArgs E)
            adminGroup.Note = this.Note.Text;
            adminGroup.State = int.Parse(this.State.Text);
            adminGroup.Power = RequestHelper.GetForm<string>("Rights").Replace(",", "|");
    }
}