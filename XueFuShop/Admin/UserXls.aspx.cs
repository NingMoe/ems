using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using org.in2bits.MyXls;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class UserXls : AdminBasePage
    {
        int CompanyId = RequestHelper.GetQueryString<int>("CompanyId");
        string CompanyAddress = string.Empty;
        string Action = RequestHelper.GetQueryString<string>("Action");
        static List<UserInfo> UserList = new List<UserInfo>();
        static List<UserInfo> ErrUserList = new List<UserInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CompanyInfo CompanyModel = CompanyBLL.ReadCompany(CompanyId);
                CompanyName.Text = CompanyModel.CompanyName;
                CompanyAddress = CompanyModel.CompanyAddress;

                if (Action == "ExcelOut")
                {
                    ExcelOut(CompanyId, CompanyModel.CompanyName);
                }
                else if (Action == "LoginOut")
                {
                    ExcelOut1(CompanyId);
                }
            }
        }


        protected void FileInButton_Click(object sender, EventArgs e)
        {
            Boolean fileOk = false;
            string path = Server.MapPath("~/xml/");

            if (this.FilePath.HasFile)
            {
                //ȡ���ļ�����չ��,��ת����Сд
                string fileExtension = Path.GetExtension(FilePath.FileName).ToLower();
                //�޶�ֻ���ϴ�xls�ļ����������͵Ļ�����"��"�ָ�
                string[] allowExtension = { ".xls" };
                //���ϴ����ļ������ͽ���һ����ƥ��
                for (int i = 0; i < allowExtension.Length; i++)
                {
                    if (fileExtension == allowExtension[i])
                    {
                        fileOk = true;
                        break;
                    }
                }
                path = path + FilePath.FileName;
                if (fileOk)
                {
                    try
                    {
                        FilePath.PostedFile.SaveAs(path);
                    }
                    catch
                    {
                        ScriptHelper.Alert("�ϴ�ʧ�ܣ�");
                    }
                    ExcelIn(path);
                }
                else
                {
                    ScriptHelper.Alert("��ѡ��2003���Excel�ļ�");
                }

            }
            else
            {
                ScriptHelper.Alert("��ѡ��Ҫ������ļ���");
            }
        }


        protected void ExcelIn(string Path)
        {
            //����Excel
            UserList.Clear();
            ErrUserList.Clear();
            string companyPostID=CompanyBLL.ReadCompany(CompanyId).Post;
            List<PostInfo> companyPostList = PostBLL.ReadPostListByPostId(companyPostID);
            List<PostInfo> companyDepartmentList = PostBLL.ReadParentPostListByPostId(companyPostID);

            //����Ҫ�����Excel
            XlsDocument xls = new XlsDocument(Path);//�����ⲿExcel
            //���Excel�е�ָ��һ������ҳ

            Worksheet sheet = xls.Workbook.Worksheets[0];
            //int StyleNum = 0; 
            bool Checked = true;
            NamePrefix = Prefix.Text.ToLower();

            //��ȡ���� ѭ��ÿsheet����ҳ��ÿһ��,����ȡǰһ��
            for (int i = 2; i < sheet.Rows.Count; i++)
            {
                //int PostId = 0;//���ͺ�
                string Department = string.Empty;
                string PostName = string.Empty;
                string RealName = string.Empty;
                string Sex = string.Empty;
                string UserId = string.Empty;
                string PassWord = string.Empty;
                string Mobile = string.Empty;
                string Email = string.Empty;
                string StudyPostName = string.Empty;
                string AccountStyle = string.Empty;
                UserInfo user = new UserInfo();
                Checked = true;
                //try
                {
                    //��ǰ�еĶ���Ϊ��ʱ��ִ��
                    if (sheet.Rows[ushort.Parse(i.ToString())].CellCount >= 2)
                    {
                        //for (int QuestionLine = 1; QuestionLine <= 5; QuestionLine++)
                        //{
                        //    if ((i + QuestionLine) > (sheet.Rows.Count + 10) || (sheet.Rows[ushort.Parse((i + QuestionLine).ToString())].CellCount < 2))
                        //    {
                        //        Checked = false;
                        //        break;
                        //    }
                        //}
                        if (Checked)
                        {
                            //try
                            {
                                try
                                {
                                    Department = sheet.Rows[ushort.Parse(i.ToString())].GetCell(1).Value.ToString().Trim();
                                    PostName = sheet.Rows[ushort.Parse(i.ToString())].GetCell(2).Value.ToString().Trim();
                                    RealName = sheet.Rows[ushort.Parse(i.ToString())].GetCell(3).Value.ToString().Trim().Replace(" ", "");
                                    Sex = sheet.Rows[ushort.Parse(i.ToString())].GetCell(4).Value.ToString().Trim();
                                    Mobile = sheet.Rows[ushort.Parse(i.ToString())].GetCell(5).Value.ToString().Trim();
                                    Email = sheet.Rows[ushort.Parse(i.ToString())].GetCell(6).Value.ToString().Trim();
                                    AccountStyle = sheet.Rows[ushort.Parse(i.ToString())].GetCell(7).Value.ToString().Trim();
                                    StudyPostName = sheet.Rows[ushort.Parse(i.ToString())].GetCell(8).Value.ToString().Trim();
                                }
                                catch
                                {
                                    Checked = false;
                                }
                                try
                                {
                                    UserId = sheet.Rows[ushort.Parse(i.ToString())].GetCell(9).Value.ToString();
                                    PassWord = sheet.Rows[ushort.Parse(i.ToString())].GetCell(10).Value.ToString();
                                }
                                catch
                                {

                                }
                                PostInfo postModel;
                                //����
                                if (string.IsNullOrEmpty(Department))
                                {
                                    Checked = false;
                                }
                                else
                                {
                                    postModel = PostBLL.ReadPost(companyDepartmentList, Department);
                                    if (postModel != null)
                                        user.Department = postModel.PostId;
                                    else
                                    {
                                        Checked = false;
                                    }
                                }                                

                                if (string.IsNullOrEmpty(PostName))
                                {
                                    Checked = false;
                                }
                                else
                                {
                                    user.PostName = PostName;
                                    postModel = PostBLL.ReadPost(companyPostList, PostName);
                                    if (postModel != null)
                                    {
                                        user.WorkingPostID = postModel.PostId;
                                    }
                                }

                                //����
                                if (string.IsNullOrEmpty(RealName))
                                {
                                    Checked = false;
                                }
                                user.RealName = RealName;

                                //�Ա�
                                switch (Sex)
                                {
                                    case "��":
                                        user.Sex = 1;
                                        break;

                                    case "Ů":
                                        user.Sex = 2;
                                        break;

                                    default:
                                        user.Sex = 1;
                                        break;
                                }

                                if (!Regex.IsMatch(Mobile, @"^(1[345789][0-9]{9})$"))
                                {
                                    Checked = false;
                                    Mobile += "[��ʽ����ȷ]";
                                }
                                //if (UserBLL.IsExistMobile(Mobile, int.MinValue))
                                //{
                                //    Checked = false;
                                //    Mobile += "[�ظ�]";
                                //}
                                user.Mobile = Mobile;

                                if (!Regex.IsMatch(Email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                                {
                                    Checked = false;
                                    Email += "[��ʽ����ȷ]";
                                }
                                user.Email = Email;

                                //�ʺ����ʹ���
                                switch (AccountStyle)
                                {
                                    case "������Ա":
                                        user.GroupID = 36;
                                        break;

                                    case "��˾����Ա":
                                        user.GroupID = 37;
                                        break;

                                    case "���Ź���Ա":
                                        user.GroupID = 8;
                                        break;

                                    case "��ѵʦ����Ա":
                                        user.GroupID = 44;
                                        break;

                                    default:
                                        user.GroupID = 36;
                                        break;
                                }


                                if (string.IsNullOrEmpty(StudyPostName))
                                {
                                    Checked = false;
                                }
                                else
                                {
                                    postModel = PostBLL.ReadPost(companyPostList, StudyPostName); //PostBLL.ReadPost(StudyPostName, user.Department);
                                    if (postModel != null)
                                    {
                                        user.StudyPostID = postModel.PostId;
                                        if (user.WorkingPostID <= 0)
                                            user.WorkingPostID = user.StudyPostID;
                                    }
                                    else
                                    {
                                        Checked = false;
                                        user.StudyPostID = 0;
                                    }
                                }

                                //��������ȫƴ
                                if (string.IsNullOrEmpty(UserId))
                                {
                                    UserId = PinYinConverter.Get(RealName).ToLower();
                                }
                                user.UserName = NamePrefix + UserId;

                                //��������
                                if (string.IsNullOrEmpty(PassWord))
                                {
                                    PassWord = "111111";
                                }
                                PassWord = StringHelper.Password(StringHelper.SearchSafe(PassWord), (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);
                                user.UserPassword = PassWord;
                                user.Status = 2;

                                if (!Checked)
                                {

                                    ErrUserList.Add(user);
                                    //break;
                                }
                                else
                                {
                                    UserSearchInfo TestModel = new UserSearchInfo();
                                    TestModel.UserName = user.UserName;
                                    if (UserBLL.SearchUserList(TestModel).Count > 0)
                                    {
                                        user.UserName += "[����]";
                                        ErrUserList.Add(user);
                                    }
                                    else
                                    {
                                        UserList.Add(user);
                                    }
                                }
                            }
                            //catch
                            //{
                            //    continue;
                            //}
                        }
                    }
                }
                //catch
                //{
                //    continue;
                //}
            }

            if (ErrUserList.Count > 0)
            {
                base.BindControl(ErrUserList, this.RecordList);
                ListShow.Style.Add("background", "yellow");
                UserOk.Visible = false;
            }
            else
            {
                base.BindControl(UserList, this.RecordList);
                ListShow.Style.Add("background", "");
                UserOk.Visible = true;
            }
        }


        protected void UserOk_Click(object sender, EventArgs e)
        {
            if (ErrUserList.Count == 0)
            {
                if (UserList.Count > 0)
                {
                    UserOk.Enabled = false;
                    foreach (UserInfo Item in UserList)
                    {
                        Item.CompanyID = CompanyId;
                        Item.Address = CompanyAddress;
                        UserBLL.AddUser(Item);
                    }
                    UserList.Clear();
                    ScriptHelper.Alert("����ɹ���");
                }
                else
                {
                    ScriptHelper.Alert("�쳣���������µ��룡");
                }
            }
            else
            {
                ScriptHelper.Alert("����������������⣡");
            }
        }


        protected void ExcelOut(int CompanyId, string CompanyName)
        {
            string fileName = "Demo" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            //����Excel��ʼ
            string FilePath = "~/xml/" + fileName;

            XlsDocument xls = new XlsDocument();//������xls�ĵ�

            xls.FileName = fileName;//����·�������ֱ�ӷ��͵��ͻ��˵Ļ�ֻ��Ҫ���� ��������
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

            //����ָ������ҳ���п���
            MergeArea ma = new MergeArea(1, 1, 1, 11);//�ϲ���Ԫ�� ������

            //��������
            string sonCompanyID = CompanyBLL.ReadCompanyIdList(CompanyId.ToString());

            List<CompanyInfo> companyList = CompanyBLL.ReadCompanyListByCompanyId(sonCompanyID);
            foreach (CompanyInfo company in companyList)
            {

                Worksheet sheet = xls.Workbook.Worksheets.AddNamed(company.CompanySimpleName); //����һ������ҳΪDome                
                sheet.AddMergeArea(ma);

                //������
                Cells cells = sheet.Cells; //���ָ������ҳ�м���
                //�в�������
                Cell cell = cells.Add(1, 1, company.CompanyName, cellXF);

                //����XY����
                cell.HorizontalAlignment = HorizontalAlignments.Centered;
                cell.VerticalAlignment = VerticalAlignments.Centered;
                //��������
                cell.Font.Bold = true;//���ô���
                cell.Font.ColorIndex = 0;//������ɫ��
                cell.Font.FontFamily = FontFamilies.Roman;//�������� Ĭ��Ϊ����
                //�����н���

                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.InCompanyID = company.CompanyId.ToString();
                userSearch.StatusNoEqual = (int)UserState.Del;
                List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
                int LineNum = 3;
                if (userList.Count > 0)
                {
                    cells.Add(2, 1, "���");
                    cells.Add(2, 2, "����");
                    cells.Add(2, 3, "ԭʼ��λ");
                    cells.Add(2, 4, "����");
                    cells.Add(2, 5, "�Ա�");
                    cells.Add(2, 6, "Email");
                    cells.Add(2, 7, "�ֻ�");
                    cells.Add(2, 8, "�û�ID");
                    //cells.Add(2, 9, "������λ");
                    cells.Add(2, 9, "ѧϰ��λ");
                    cells.Add(2, 10, "�ʻ�����");
                    //cells.Add(2, 11, "�����½ʱ��");
                    cells.Add(2, 11, "״̬");
                    foreach (UserInfo Info in userList)
                    {
                        cells.Add(LineNum, 1, LineNum - 2);
                        cells.Add(LineNum, 2, PostBLL.ReadPost(Info.Department).PostName);
                        cells.Add(LineNum, 3, Info.PostName);
                        cells.Add(LineNum, 4, Info.RealName);
                        cells.Add(LineNum, 5, EnumHelper.ReadEnumChineseName<SexType>(Info.Sex));
                        cells.Add(LineNum, 6, Info.Email);
                        cells.Add(LineNum, 7, Info.Mobile);
                        cells.Add(LineNum, 8, Info.UserName);
                        //cells.Add(LineNum, 9, BLLPost.ReadPost(Info.PostId).PostName);
                        cells.Add(LineNum, 9, PostBLL.ReadPost(Info.StudyPostID).PostName);
                        cells.Add(LineNum, 10, AdminGroupBLL.ReadAdminGroupCache(Info.GroupID).Name);
                        //cells.Add(LineNum, 11, Info.LastLoginDate.ToString());
                        cells.Add(LineNum, 11, EnumHelper.ReadEnumChineseName<UserState>(Info.Status));

                        LineNum = LineNum + 1;

                    }
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


        protected void ExcelOut1(int CompanyId)
        {
            string fileName = "Demo" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            //����Excel��ʼ
            string FilePath = "~/xml/" + fileName;

            XlsDocument xls = new XlsDocument();//������xls�ĵ�

            xls.FileName = fileName;//����·�������ֱ�ӷ��͵��ͻ��˵Ļ�ֻ��Ҫ���� ��������
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

            //����ָ������ҳ���п���
            MergeArea ma = new MergeArea(1, 1, 1, 8);//�ϲ���Ԫ�� ������

            //��������
            string sonCompanyID = CompanyBLL.ReadCompanyIdList(CompanyId.ToString());

            List<CompanyInfo> companyList = CompanyBLL.ReadCompanyListByCompanyId(sonCompanyID);
            foreach (CompanyInfo company in companyList)
            {

                Worksheet sheet = xls.Workbook.Worksheets.AddNamed(company.CompanySimpleName); //����һ������ҳΪDome                
                sheet.AddMergeArea(ma);

                //������
                Cells cells = sheet.Cells; //���ָ������ҳ�м���
                //�в�������
                Cell cell = cells.Add(1, 1, company.CompanyName, cellXF);

                //����XY����
                cell.HorizontalAlignment = HorizontalAlignments.Centered;
                cell.VerticalAlignment = VerticalAlignments.Centered;
                //��������
                cell.Font.Bold = true;//���ô���
                cell.Font.ColorIndex = 0;//������ɫ��
                cell.Font.FontFamily = FontFamilies.Roman;//�������� Ĭ��Ϊ����
                //�����н���

                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.InCompanyID = company.CompanyId.ToString();
                userSearch.StatusNoEqual = (int)UserState.Del;
                List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
                int LineNum = 3;
                if (userList.Count > 0)
                {
                    cells.Add(2, 1, "���");
                    cells.Add(2, 2, "����");
                    cells.Add(2, 3, "�ֻ�");
                    cells.Add(2, 4, "�û�ID");
                    cells.Add(2, 5, "ѧϰ��λ");
                    cells.Add(2, 6, "�ʻ�����");
                    cells.Add(2, 7, "�����½ʱ��");
                    cells.Add(2, 8, "״̬");
                    foreach (UserInfo Info in userList)
                    {
                        cells.Add(LineNum, 1, LineNum - 2);
                        cells.Add(LineNum, 2, Info.RealName);
                        cells.Add(LineNum, 3, Info.Mobile);
                        cells.Add(LineNum, 4, Info.UserName);
                        cells.Add(LineNum, 5, PostBLL.ReadPost(Info.StudyPostID).PostName);
                        cells.Add(LineNum, 6, AdminGroupBLL.ReadAdminGroupCache(Info.GroupID).Name);
                        cells.Add(LineNum, 7, Info.RegisterDate == Info.LastLoginDate ? "" : Info.LastLoginDate.ToString());
                        cells.Add(LineNum, 8, EnumHelper.ReadEnumChineseName<UserState>(Info.Status));

                        LineNum = LineNum + 1;

                    }
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
    }
}
