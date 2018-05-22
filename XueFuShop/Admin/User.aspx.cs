using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XueFuShop.Common;
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.BLL;
using XueFuShop.Models;
using System.Collections.Generic;
using org.in2bits.MyXls;
using System.IO;

namespace XueFuShop.Admin
{
    public partial class User : AdminBasePage
    {
        protected int status = 0;
        protected int companyID = RequestHelper.GetQueryString<int>("CompanyID");
        protected Dictionary<int, CompanyInfo> companyDic = new Dictionary<int, CompanyInfo>();
        protected string url = RequestHelper.FilterRequestQueryString(new string[] { "Status", "Action" });

        protected void ActiveButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("ActiveUser", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                UserBLL.ChangeUserStatus(intsForm, (int)UserState.Normal);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("ActiveRecord"), ShopLanguage.ReadLanguage("User"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("ActiveOK"), RequestHelper.RawUrl);
            }
        }

        protected void FreezeButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("FreezeUser", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                UserBLL.ChangeUserStatus(intsForm, (int)UserState.Frozen);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("FreezeRecord"), ShopLanguage.ReadLanguage("User"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("FreezeOK"), RequestHelper.RawUrl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!this.Page.IsPostBack)
            {
                base.CheckAdminPower("ReadUser", PowerCheckType.Single);
                //this.Sex.DataSource = EnumHelper.ReadEnumList<SexType>();
                //this.Sex.DataValueField = "Value";
                //this.Sex.DataTextField = "ChineseName";
                //this.Sex.DataBind();
                //this.Sex.Items.Insert(0, new ListItem("所有", string.Empty));
                this.status = RequestHelper.GetQueryString<int>("Status"); 
                UserSearchInfo userSearch = SearchCondition();
                if (companyID >= 0)
                {
                    CompanyName.Value = CompanyBLL.ReadCompany(companyID).CompanyName;
                }              
                this.UserName.Text = userSearch.UserName;
                this.RealName.Text = userSearch.RealName;
                this.Mobile.Text = userSearch.Mobile;
                this.Email.Text = userSearch.Email;
                //this.Sex.SelectedValue = userSearch.Sex.ToString();
                this.StartRegisterDate.Text = RequestHelper.GetQueryString<string>("StartRegisterDate");
                this.EndRegisterDate.Text = RequestHelper.GetQueryString<string>("EndRegisterDate");
                base.BindControl(UserBLL.SearchUserList(base.CurrentPage, base.PageSize, userSearch, ref this.Count), this.RecordList, this.MyPager);
            }
        }

        private UserSearchInfo SearchCondition()
        {
            UserSearchInfo userSearch = new UserSearchInfo();
            if (companyID >= 0)
            {
                userSearch.InCompanyID = companyID.ToString();
            }
            userSearch.RealName = RequestHelper.GetQueryString<string>("RealName");
            userSearch.UserName = RequestHelper.GetQueryString<string>("UserName");
            userSearch.Email = RequestHelper.GetQueryString<string>("Email");
            userSearch.Mobile = RequestHelper.GetQueryString<string>("Mobile");
            userSearch.Sex = RequestHelper.GetQueryString<int>("Sex");
            userSearch.StartRegisterDate = RequestHelper.GetQueryString<DateTime>("StartRegisterDate");
            userSearch.EndRegisterDate = ShopCommon.SearchEndDate(RequestHelper.GetQueryString<DateTime>("EndRegisterDate"));
            userSearch.Status = RequestHelper.GetQueryString<int>("Status");
            return userSearch;
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CompanyName.Value))
                companyID = int.MinValue;
            else
                companyID = RequestHelper.GetForm<int>("CompanyID");
            ResponseHelper.Redirect("User.aspx?Action=search&CompanyID=" + companyID + "&UserName=" + this.UserName.Text + "&RealName=" + this.RealName.Text + "&Mobile=" + this.Mobile.Text + "&Email=" + this.Email.Text + "&Status=" + RequestHelper.GetQueryString<string>("Status")+ "&StartRegisterDate=" + this.StartRegisterDate.Text + "&EndRegisterDate=" + this.EndRegisterDate.Text); //+ "&Sex=" + this.Sex.Text
        }

        protected void UnActiveButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("UnActiveUser", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                UserBLL.ChangeUserStatus(intsForm, (int)UserState.NoCheck);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UnActiveRecord"), ShopLanguage.ReadLanguage("User"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("UnActiveOK"), RequestHelper.RawUrl);
            }
        }

        protected void UnFreezeButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("UnFreezeUser", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                UserBLL.ChangeUserStatus(intsForm, (int)UserState.Normal);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UnFreezeRecord"), ShopLanguage.ReadLanguage("User"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("UnFreezeOK"), RequestHelper.RawUrl);
            }
        }

        protected void FreeButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("FreeUser", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                UserBLL.ChangeUserStatus(intsForm, (int)UserState.Free);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("FreeRecord"), ShopLanguage.ReadLanguage("User"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("FreeOK"), RequestHelper.RawUrl);
            }
        }

        protected void OtherButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("FreeUser", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                UserBLL.ChangeUserStatus(intsForm, (int)UserState.Other);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("FreeRecord"), ShopLanguage.ReadLanguage("User"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("FreeOK"), RequestHelper.RawUrl);
            }
        }

        protected void RecoveryButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("RecoveryUser", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                UserBLL.ChangeUserStatus(intsForm, (int)UserState.Normal);
                TestPaperBLL.RecoveryPaperByUserID(intsForm);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("RecoveryRecord"), ShopLanguage.ReadLanguage("User"), intsForm);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("RecoveryRecord"), ShopLanguage.ReadLanguage("TestPaperRecord"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("RecoveryOK"), RequestHelper.RawUrl);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            base.CheckAdminPower("DeleteUser", PowerCheckType.Single);
            string intsForm = RequestHelper.GetIntsForm("SelectID");
            if (intsForm != string.Empty)
            {
                UserBLL.ChangeUserStatus(intsForm, (int)UserState.Del);
                TestPaperBLL.DeletePaperByUserID(intsForm);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("User"), intsForm);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("TestPaperRecord"), intsForm);
                ScriptHelper.Alert(ShopLanguage.ReadLanguage("DeleteOK"), RequestHelper.RawUrl);
            }
        }

        /// <summary>
        /// 读取公司信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        protected CompanyInfo ReadCompany(int companyID)
        {
            if (!companyDic.ContainsKey(companyID))
                companyDic.Add(companyID, CompanyBLL.ReadCompany(companyID));
            return companyDic[companyID];
        }

        protected void UserOutXls_Click(object sender, EventArgs e)
        {
            UserSearchInfo userSearch = SearchCondition();
            userSearch.Condition = "Order by [CompanyId]";
            List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
            ExcelOut(userList);
        }

        protected void ExcelOut(List<UserInfo> userList)
        {
            //生成Excel开始
            string FilePath = "~/xml/Demo.xls";

            XlsDocument xls = new XlsDocument();//创建空xls文档

            xls.FileName = Server.MapPath(FilePath);//保存路径，如果直接发送到客户端的话只需要名称 生成名称

            Worksheet sheet = xls.Workbook.Worksheets.AddNamed("会员列表"); //创建一个工作页为Dome

            //设置指定工作页跨行跨列
            MergeArea ma = new MergeArea(1, 1, 1, 6);//合并单元格 行与列
            sheet.AddMergeArea(ma);

            //设置指定工作页跨行跨列结束

            //创建列样式创建列时引用
            XF cellXF = xls.NewXF();
            cellXF.VerticalAlignment = VerticalAlignments.Centered;
            cellXF.HorizontalAlignment = HorizontalAlignments.Centered;
            cellXF.Font.Height = 24 * 12;
            cellXF.Font.Bold = true;
            //cellXF.Pattern = 1;//设定单元格填充风格。如果设定为0，则是纯色填充
            //cellXF.PatternBackgroundColor = Colors.Red;//填充的背景底色
            //cellXF.PatternColor = Colors.Red;//设定填充线条的颜色
            //创建列样式结束

            //创建列
            Cells cells = sheet.Cells; //获得指定工作页列集合
            //列操作基本
            Cell cell = cells.Add(1, 1, "会员列表", cellXF);

            //设置XY居中
            cell.HorizontalAlignment = HorizontalAlignments.Centered;
            cell.VerticalAlignment = VerticalAlignments.Centered;
            //设置字体
            cell.Font.Bold = true;//设置粗体
            cell.Font.ColorIndex = 0;//设置颜色码           
            cell.Font.FontFamily = FontFamilies.Roman;//设置字体 默认为宋体               
            //创建列结束  


            //创建数据
            int LineNum = 3;
            if (userList.Count > 0)
            {
                cells.Add(2, 1, "序号");
                cells.Add(2, 2, "姓名");
                cells.Add(2, 3, "公司简称");
                cells.Add(2, 4, "品牌");
                cells.Add(2, 5, "手机");
                cells.Add(2, 6, "Email");
                //cells.Add(2, 7, "原始岗位");
                //cells.Add(2, 5, "性别");
                //cells.Add(2, 8, "用户ID");
                //cells.Add(2, 9, "工作岗位");
                //cells.Add(2, 9, "学习岗位");
                //cells.Add(2, 10, "帐户类型");
                foreach (UserInfo Info in userList)
                {
                    cells.Add(LineNum, 1, LineNum - 2);
                    cells.Add(LineNum, 2, Info.RealName);
                    cells.Add(LineNum, 3, ReadCompany(Info.CompanyID).CompanySimpleName);
                    cells.Add(LineNum, 4, ReadBrandName(ProductBrandBLL.ReadProductBrandCacheList(StringHelper.SubString(ReadCompany(Info.CompanyID).BrandId, "17"))));                 
                    cells.Add(LineNum, 5, Info.Mobile);
                    cells.Add(LineNum, 6, Info.Email);
                    //cells.Add(LineNum, 7, Info.PostName);
                    //cells.Add(LineNum, 5, EnumHelper.ReadEnumChineseName<SexType>(Info.Sex));
                    //cells.Add(LineNum, 2, PostBLL.ReadPost(Info.Department).PostName);
                    //cells.Add(LineNum, 8, Info.UserName);
                    //cells.Add(LineNum, 9, BLLPost.ReadPost(Info.PostId).PostName);
                    //cells.Add(LineNum, 9, PostBLL.ReadPost(Info.StudyPostID).PostName);
                    //cells.Add(LineNum, 10, AdminGroupBLL.ReadAdminGroupCache(Info.GroupID).Name);

                    LineNum = LineNum + 1;

                }
            }
            //
            //生成保存到服务器如果存在不会覆盖并且报异常所以先删除在保存新的
            //ScriptHelper.Alert(Server.MapPath("~/Demo.xls"));
            if (File.Exists(Server.MapPath(FilePath)))
            {
                File.Delete(Server.MapPath(FilePath));//删除
            }
            //保存文档
            xls.Save(Server.MapPath(FilePath));//保存到服务器
            xls.Send();//发送到客户端
        }

       private string ReadBrandName(List<ProductBrandInfo> productBrandList)
        {
            string brandName = string.Empty;
            foreach (ProductBrandInfo info in productBrandList)
            {
                brandName += " " + info.Name;
            }
            if (brandName.StartsWith(" ")) return brandName.Substring(1);
            return brandName;
        }
    }
}
