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
                //this.Sex.Items.Insert(0, new ListItem("����", string.Empty));
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
        /// ��ȡ��˾��Ϣ
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
            //����Excel��ʼ
            string FilePath = "~/xml/Demo.xls";

            XlsDocument xls = new XlsDocument();//������xls�ĵ�

            xls.FileName = Server.MapPath(FilePath);//����·�������ֱ�ӷ��͵��ͻ��˵Ļ�ֻ��Ҫ���� ��������

            Worksheet sheet = xls.Workbook.Worksheets.AddNamed("��Ա�б�"); //����һ������ҳΪDome

            //����ָ������ҳ���п���
            MergeArea ma = new MergeArea(1, 1, 1, 6);//�ϲ���Ԫ�� ������
            sheet.AddMergeArea(ma);

            //����ָ������ҳ���п��н���

            //��������ʽ������ʱ����
            XF cellXF = xls.NewXF();
            cellXF.VerticalAlignment = VerticalAlignments.Centered;
            cellXF.HorizontalAlignment = HorizontalAlignments.Centered;
            cellXF.Font.Height = 24 * 12;
            cellXF.Font.Bold = true;
            //cellXF.Pattern = 1;//�趨��Ԫ�����������趨Ϊ0�����Ǵ�ɫ���
            //cellXF.PatternBackgroundColor = Colors.Red;//���ı�����ɫ
            //cellXF.PatternColor = Colors.Red;//�趨�����������ɫ
            //��������ʽ����

            //������
            Cells cells = sheet.Cells; //���ָ������ҳ�м���
            //�в�������
            Cell cell = cells.Add(1, 1, "��Ա�б�", cellXF);

            //����XY����
            cell.HorizontalAlignment = HorizontalAlignments.Centered;
            cell.VerticalAlignment = VerticalAlignments.Centered;
            //��������
            cell.Font.Bold = true;//���ô���
            cell.Font.ColorIndex = 0;//������ɫ��           
            cell.Font.FontFamily = FontFamilies.Roman;//�������� Ĭ��Ϊ����               
            //�����н���  


            //��������
            int LineNum = 3;
            if (userList.Count > 0)
            {
                cells.Add(2, 1, "���");
                cells.Add(2, 2, "����");
                cells.Add(2, 3, "��˾���");
                cells.Add(2, 4, "Ʒ��");
                cells.Add(2, 5, "�ֻ�");
                cells.Add(2, 6, "Email");
                //cells.Add(2, 7, "ԭʼ��λ");
                //cells.Add(2, 5, "�Ա�");
                //cells.Add(2, 8, "�û�ID");
                //cells.Add(2, 9, "������λ");
                //cells.Add(2, 9, "ѧϰ��λ");
                //cells.Add(2, 10, "�ʻ�����");
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
            //���ɱ��浽������������ڲ��Ḳ�ǲ��ұ��쳣������ɾ���ڱ����µ�
            //ScriptHelper.Alert(Server.MapPath("~/Demo.xls"));
            if (File.Exists(Server.MapPath(FilePath)))
            {
                File.Delete(Server.MapPath(FilePath));//ɾ��
            }
            //�����ĵ�
            xls.Save(Server.MapPath(FilePath));//���浽������
            xls.Send();//���͵��ͻ���
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
