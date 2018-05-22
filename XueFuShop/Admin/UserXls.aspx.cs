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
                //取得文件的扩展名,并转换成小写
                string fileExtension = Path.GetExtension(FilePath.FileName).ToLower();
                //限定只能上传xls文件，多种类型的话，用"，"分隔
                string[] allowExtension = { ".xls" };
                //对上传的文件的类型进行一个个匹对
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
                        ScriptHelper.Alert("上传失败！");
                    }
                    ExcelIn(path);
                }
                else
                {
                    ScriptHelper.Alert("请选择2003版的Excel文件");
                }

            }
            else
            {
                ScriptHelper.Alert("请选择要导入的文件！");
            }
        }


        protected void ExcelIn(string Path)
        {
            //导入Excel
            UserList.Clear();
            ErrUserList.Clear();
            string companyPostID=CompanyBLL.ReadCompany(CompanyId).Post;
            List<PostInfo> companyPostList = PostBLL.ReadPostListByPostId(companyPostID);
            List<PostInfo> companyDepartmentList = PostBLL.ReadParentPostListByPostId(companyPostID);

            //加载要导入的Excel
            XlsDocument xls = new XlsDocument(Path);//加载外部Excel
            //获得Excel中的指定一个工作页

            Worksheet sheet = xls.Workbook.Worksheets[0];
            //int StyleNum = 0; 
            bool Checked = true;
            NamePrefix = Prefix.Text.ToLower();

            //读取数据 循环每sheet工作页的每一行,不读取前一行
            for (int i = 2; i < sheet.Rows.Count; i++)
            {
                //int PostId = 0;//题型号
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
                    //当前行的都不为空时才执行
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
                                //部门
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

                                //姓名
                                if (string.IsNullOrEmpty(RealName))
                                {
                                    Checked = false;
                                }
                                user.RealName = RealName;

                                //性别
                                switch (Sex)
                                {
                                    case "男":
                                        user.Sex = 1;
                                        break;

                                    case "女":
                                        user.Sex = 2;
                                        break;

                                    default:
                                        user.Sex = 1;
                                        break;
                                }

                                if (!Regex.IsMatch(Mobile, @"^(1[345789][0-9]{9})$"))
                                {
                                    Checked = false;
                                    Mobile += "[格式不正确]";
                                }
                                //if (UserBLL.IsExistMobile(Mobile, int.MinValue))
                                //{
                                //    Checked = false;
                                //    Mobile += "[重复]";
                                //}
                                user.Mobile = Mobile;

                                if (!Regex.IsMatch(Email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                                {
                                    Checked = false;
                                    Email += "[格式不正确]";
                                }
                                user.Email = Email;

                                //帐号类型处理
                                switch (AccountStyle)
                                {
                                    case "考试人员":
                                        user.GroupID = 36;
                                        break;

                                    case "公司管理员":
                                        user.GroupID = 37;
                                        break;

                                    case "集团管理员":
                                        user.GroupID = 8;
                                        break;

                                    case "内训师管理员":
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

                                //生成名字全拼
                                if (string.IsNullOrEmpty(UserId))
                                {
                                    UserId = PinYinConverter.Get(RealName).ToLower();
                                }
                                user.UserName = NamePrefix + UserId;

                                //处理密码
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
                                        user.UserName += "[重名]";
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
                    ScriptHelper.Alert("导入成功！");
                }
                else
                {
                    ScriptHelper.Alert("异常错误，请重新导入！");
                }
            }
            else
            {
                ScriptHelper.Alert("请修正完错误后，再入库！");
            }
        }


        protected void ExcelOut(int CompanyId, string CompanyName)
        {
            string fileName = "Demo" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            //生成Excel开始
            string FilePath = "~/xml/" + fileName;

            XlsDocument xls = new XlsDocument();//创建空xls文档

            xls.FileName = fileName;//保存路径，如果直接发送到客户端的话只需要名称 生成名称
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

            //设置指定工作页跨行跨列
            MergeArea ma = new MergeArea(1, 1, 1, 11);//合并单元格 行与列

            //创建数据
            string sonCompanyID = CompanyBLL.ReadCompanyIdList(CompanyId.ToString());

            List<CompanyInfo> companyList = CompanyBLL.ReadCompanyListByCompanyId(sonCompanyID);
            foreach (CompanyInfo company in companyList)
            {

                Worksheet sheet = xls.Workbook.Worksheets.AddNamed(company.CompanySimpleName); //创建一个工作页为Dome                
                sheet.AddMergeArea(ma);

                //创建列
                Cells cells = sheet.Cells; //获得指定工作页列集合
                //列操作基本
                Cell cell = cells.Add(1, 1, company.CompanyName, cellXF);

                //设置XY居中
                cell.HorizontalAlignment = HorizontalAlignments.Centered;
                cell.VerticalAlignment = VerticalAlignments.Centered;
                //设置字体
                cell.Font.Bold = true;//设置粗体
                cell.Font.ColorIndex = 0;//设置颜色码
                cell.Font.FontFamily = FontFamilies.Roman;//设置字体 默认为宋体
                //创建列结束

                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.InCompanyID = company.CompanyId.ToString();
                userSearch.StatusNoEqual = (int)UserState.Del;
                List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
                int LineNum = 3;
                if (userList.Count > 0)
                {
                    cells.Add(2, 1, "序号");
                    cells.Add(2, 2, "部门");
                    cells.Add(2, 3, "原始岗位");
                    cells.Add(2, 4, "姓名");
                    cells.Add(2, 5, "性别");
                    cells.Add(2, 6, "Email");
                    cells.Add(2, 7, "手机");
                    cells.Add(2, 8, "用户ID");
                    //cells.Add(2, 9, "工作岗位");
                    cells.Add(2, 9, "学习岗位");
                    cells.Add(2, 10, "帐户类型");
                    //cells.Add(2, 11, "最近登陆时间");
                    cells.Add(2, 11, "状态");
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


        protected void ExcelOut1(int CompanyId)
        {
            string fileName = "Demo" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            //生成Excel开始
            string FilePath = "~/xml/" + fileName;

            XlsDocument xls = new XlsDocument();//创建空xls文档

            xls.FileName = fileName;//保存路径，如果直接发送到客户端的话只需要名称 生成名称
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

            //设置指定工作页跨行跨列
            MergeArea ma = new MergeArea(1, 1, 1, 8);//合并单元格 行与列

            //创建数据
            string sonCompanyID = CompanyBLL.ReadCompanyIdList(CompanyId.ToString());

            List<CompanyInfo> companyList = CompanyBLL.ReadCompanyListByCompanyId(sonCompanyID);
            foreach (CompanyInfo company in companyList)
            {

                Worksheet sheet = xls.Workbook.Worksheets.AddNamed(company.CompanySimpleName); //创建一个工作页为Dome                
                sheet.AddMergeArea(ma);

                //创建列
                Cells cells = sheet.Cells; //获得指定工作页列集合
                //列操作基本
                Cell cell = cells.Add(1, 1, company.CompanyName, cellXF);

                //设置XY居中
                cell.HorizontalAlignment = HorizontalAlignments.Centered;
                cell.VerticalAlignment = VerticalAlignments.Centered;
                //设置字体
                cell.Font.Bold = true;//设置粗体
                cell.Font.ColorIndex = 0;//设置颜色码
                cell.Font.FontFamily = FontFamilies.Roman;//设置字体 默认为宋体
                //创建列结束

                UserSearchInfo userSearch = new UserSearchInfo();
                userSearch.InCompanyID = company.CompanyId.ToString();
                userSearch.StatusNoEqual = (int)UserState.Del;
                List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
                int LineNum = 3;
                if (userList.Count > 0)
                {
                    cells.Add(2, 1, "序号");
                    cells.Add(2, 2, "姓名");
                    cells.Add(2, 3, "手机");
                    cells.Add(2, 4, "用户ID");
                    cells.Add(2, 5, "学习岗位");
                    cells.Add(2, 6, "帐户类型");
                    cells.Add(2, 7, "最近登陆时间");
                    cells.Add(2, 8, "状态");
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
    }
}
