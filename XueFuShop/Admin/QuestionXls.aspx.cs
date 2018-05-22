using System;
using System.Collections.Generic;
using System.IO;
using org.in2bits.MyXls;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class QuestionXls : AdminBasePage
    {
        string Action = RequestHelper.GetQueryString<string>("Action");
        int CateId = RequestHelper.GetQueryString<int>("CateId");
        string courseIDStr = RequestHelper.GetQueryString<string>("CourseID");
        static List<QuestionInfo> QuestionList = new List<QuestionInfo>();
        static List<QuestionInfo> ErrQuestionList = new List<QuestionInfo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Action == "CourseFileOut")
                {
                    filein.Style["display"] = "none";
                    ListShow.Style["display"] = "none";
                    FileInButton.Visible = false;
                    CourseName.Text = CourseBLL.ReadCourse(CateId).CourseName;
                }
                else if (Action == "CourseFileIn")
                {
                    filein.Style["display"] = "";
                    fileout.Style["display"] = "none";
                    ListShow.Style["display"] = "";
                    FileOutButton.Visible = false;
                    QuestionOutButton.Visible = false;
                    CourseName.Text = CourseBLL.ReadCourse(CateId).CourseName + "【现有题目数量：" + QuestionBLL.ReadList(CateId).Count.ToString() + "】";
                }
                else if (Action == "TestCateFileOut")
                {
                    filein.Style["display"] = "none";
                    ListShow.Style["display"] = "none";
                    FileInButton.Visible = false;
                    CourseName.Text = ProductBLL.ReadProduct(CateId).Name;
                }
                else if (Action == "BatchFileOut")
                {
                    filein.Style["display"] = "none";
                    ListShow.Style["display"] = "none";
                    FileInButton.Visible = false;
                    QuestionOutButton.Visible = false;
                    CourseName.Text = "批量导出";
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

        protected void FileOutButton_Click(object sender, EventArgs e)
        {
            int QuestionOutNum = 0;
            if (!string.IsNullOrEmpty(QuestionNum.Text))
            {
                QuestionOutNum = int.Parse(QuestionNum.Text);
            }
            if (Action == "TestCateFileOut")
            {
                ProductInfo product = ProductBLL.ReadProduct(CateId);
                if (!string.IsNullOrEmpty(product.Accessory)) ExcelOut(product.Accessory, product.Name, QuestionOutNum, 1);
            }
            else if (Action == "BatchFileOut")
            {
                
                string FilePath = "~/xml/Demo.xls";
                XlsDocument xls = new XlsDocument();//创建空xls文档
                xls.FileName = Server.MapPath(FilePath);//保存路径，如果直接发送到客户端的话只需要名称 生成名称

                foreach (string courseID in courseIDStr.Split(','))
                {
                    string courseName = CourseBLL.ReadCourse(int.Parse(courseID)).CourseName;
                    ExcelOut(xls, courseID, courseName, QuestionOutNum, 1);
                }

                //生成保存到服务器如果存在不会覆盖并且报异常所以先删除在保存新的
                if (File.Exists(Server.MapPath(FilePath)))
                {
                    File.Delete(Server.MapPath(FilePath));//删除
                }
                //保存文档
                xls.Save(Server.MapPath(FilePath));//保存到服务器
                xls.Send();//发送到客户端
            }
            else
            {
                ExcelOut(CateId.ToString(), CourseName.Text, QuestionOutNum, 1);
            }
        }

        protected void QuestionOutButton_Click(object sender, EventArgs e)
        {
            int QuestionOutNum = 0;
            if (!string.IsNullOrEmpty(QuestionNum.Text))
            {
                QuestionOutNum = int.Parse(QuestionNum.Text);
            }
            if (Action == "TestCateFileOut")
            {
                ProductInfo product = ProductBLL.ReadProduct(CateId);
                if (!string.IsNullOrEmpty(product.Accessory)) ExcelOut(product.Accessory, product.Name, QuestionOutNum, 2);
            }
            else
            {
                ExcelOut(CateId.ToString(), CourseName.Text, QuestionOutNum, 2);
            }
        }

        protected void ExcelIn(string Path)
        {
            //导入Excel
            QuestionList.Clear();
            ErrQuestionList.Clear();

            //加载要导入的Excel
            XlsDocument xls = new XlsDocument(Path);//加载外部Excel
            //获得Excel中的指定一个工作页

            Worksheet sheet = xls.Workbook.Worksheets[0];
            //List<QuestionInfo> QuestionList = new List<QuestionInfo>();
            //List<QuestionInfo> ErrQuestionList = new List<QuestionInfo>();
            int StyleNum = 0;//题型号
            string cell2 = string.Empty;
            string cellA = string.Empty;
            string cellB = string.Empty;
            string cellC = string.Empty;
            string cellD = string.Empty;
            string cellAnswer = string.Empty;
            bool Checked = true;

            //读取数据 循环每sheet工作页的每一行,不读取前一行
            for (int i = 2; i < (sheet.Rows.Count + 10); i++)
            {
                Checked = true;
                try
                {
                    //当前行的都不为空时才执行
                    if (sheet.Rows[ushort.Parse(i.ToString())].CellCount >= 2)
                    {
                        try
                        {
                            cell2 = sheet.Rows[ushort.Parse(i.ToString())].GetCell(2).Value.ToString();
                        }
                        catch
                        {
                            continue;
                        }
                        switch (cell2)
                        {
                            case "单项选择题":
                                StyleNum = 1;
                                break;
                            case "多项选择题":
                                StyleNum = 2;
                                break;
                            case "判断题":
                                StyleNum = 3;
                                break;
                        }

                        if (StyleNum != 3)
                        {
                            if (StyleNum == 0) Checked = false;
                            for (int QuestionLine = 1; QuestionLine <= 5; QuestionLine++)
                            {
                                if ((i + QuestionLine) > (sheet.Rows.Count + 10) || (sheet.Rows[ushort.Parse((i + QuestionLine).ToString())].CellCount < 2))
                                {
                                    Checked = false;
                                    break;
                                }
                            }
                            if (Checked)
                            {
                                try
                                {
                                    cellA = sheet.Rows[ushort.Parse((i + 1).ToString())].GetCell(1).Value.ToString();
                                    cellB = sheet.Rows[ushort.Parse((i + 2).ToString())].GetCell(1).Value.ToString();
                                    cellC = sheet.Rows[ushort.Parse((i + 3).ToString())].GetCell(1).Value.ToString();
                                    cellD = sheet.Rows[ushort.Parse((i + 4).ToString())].GetCell(1).Value.ToString();
                                    cellAnswer = sheet.Rows[ushort.Parse((i + 5).ToString())].GetCell(2).Value.ToString();
                                }
                                catch
                                {
                                    continue;
                                }

                                //先判断是否存在A B C D选项
                                if (cellA.Contains("A") && cellB.Contains("B") && cellC.Contains("C") && cellD.Contains("D"))
                                {
                                    //判断答案是否符合标准
                                    if (StyleNum == 1)//当为单选时，答案长度为1
                                    {
                                        if (cellAnswer.Length != 1)
                                        {
                                            Checked = false;
                                        }
                                    }
                                    else if (StyleNum == 2)
                                    {
                                        if (cellAnswer.Length > 4 || cellAnswer.Length <= 1)//多选时答案长度不超过4
                                        {
                                            Checked = false;
                                        }
                                    }

                                    //答案没问题后，再检测四个选项值，最后添加
                                    if (Checked)
                                    {
                                        cellA = sheet.Rows[ushort.Parse((i + 1).ToString())].GetCell(2).Value.ToString();
                                        cellB = sheet.Rows[ushort.Parse((i + 2).ToString())].GetCell(2).Value.ToString();
                                        cellC = sheet.Rows[ushort.Parse((i + 3).ToString())].GetCell(2).Value.ToString();
                                        cellD = sheet.Rows[ushort.Parse((i + 4).ToString())].GetCell(2).Value.ToString();
                                        if (!string.IsNullOrEmpty(cellA.Trim()) && !string.IsNullOrEmpty(cellB.Trim()) && !string.IsNullOrEmpty(cellC.Trim()) && !string.IsNullOrEmpty(cellD.Trim()))
                                        {

                                            QuestionInfo QuestionModel = new QuestionInfo();
                                            QuestionModel.Question = cell2.Trim();
                                            QuestionModel.A = cellA.Trim();
                                            QuestionModel.B = cellB.Trim();
                                            QuestionModel.C = cellC.Trim();
                                            QuestionModel.D = cellD.Trim();
                                            QuestionModel.Answer = cellAnswer.Trim();
                                            QuestionModel.Style = StyleNum.ToString();
                                            QuestionModel.CateId = 0;

                                            QuestionList.Add(QuestionModel);
                                        }
                                        else
                                        {
                                            QuestionInfo QuestionModel = new QuestionInfo();
                                            QuestionModel.Question = cell2.Trim();
                                            QuestionModel.Answer = "选项好像有问题";
                                            ErrQuestionList.Add(QuestionModel);
                                        }
                                    }
                                    else
                                    {
                                        QuestionInfo QuestionModel = new QuestionInfo();
                                        QuestionModel.Question = cell2.Trim();
                                        QuestionModel.Answer = "答案好像有问题";
                                        ErrQuestionList.Add(QuestionModel);
                                    }
                                    i = i + 5;
                                }
                            }
                        }
                        else if (StyleNum == 3)
                        {

                            if ((i + 1) > (sheet.Rows.Count + 10))
                            {
                                Checked = false;
                            }
                            if (Checked)
                            {
                                cellA = sheet.Rows[ushort.Parse((i + 1).ToString())].GetCell(1).Value.ToString();
                                if (cellA.Contains("改错"))
                                {
                                    try
                                    {
                                        cellAnswer = sheet.Rows[ushort.Parse((i + 1).ToString())].GetCell(2).Value.ToString();
                                    }
                                    catch
                                    {
                                        cellAnswer = string.Empty;
                                    }

                                    int ErrNum = cell2.Split('~').Length - 1;
                                    //当存在错误且有改错内容 或者 没有错误也没有改错内容 时，才为正常
                                    if ((ErrNum > 0 && !string.IsNullOrEmpty(cellAnswer.Trim()) && ErrNum % 2 == 0 && ErrNum / 2 == cellAnswer.Split('~').Length) || (ErrNum == 0 && string.IsNullOrEmpty(cellAnswer.Trim())))
                                    {
                                        QuestionInfo QuestionModel = new QuestionInfo();
                                        QuestionModel.Question = cell2.Trim();
                                        if (string.IsNullOrEmpty(cellAnswer.Trim()))
                                        {
                                            QuestionModel.Answer = "1";
                                        }
                                        else
                                        {
                                            QuestionModel.A = cellAnswer.Trim();
                                            QuestionModel.Answer = "0";
                                        }
                                        QuestionModel.CateId = 0;
                                        QuestionModel.Style = StyleNum.ToString();
                                        QuestionList.Add(QuestionModel);
                                    }
                                    else
                                    {
                                        QuestionInfo QuestionModel = new QuestionInfo();
                                        QuestionModel.Question = cell2.Trim();
                                        QuestionModel.Answer = "~的数量不对或与答案不一致";
                                        ErrQuestionList.Add(QuestionModel);
                                    }
                                    i = i + 2;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }

            if (ErrQuestionList.Count > 0)
            {
                base.BindControl(ErrQuestionList, this.RecordList);
            }
            else
            {
                base.BindControl(QuestionList, this.RecordList);
                QuestionOk.Visible = true;
            }
        }

        protected void ExcelOut(XlsDocument xls,string CateId, string TestName, int Num, int Type)
        {            
            Worksheet sheet = xls.Workbook.Worksheets.AddNamed(TestName); //创建一个工作页为Dome  

            //设置文档列属性 
            //ColumnInfo cinfo = new ColumnInfo(xls, sheet);//设置xls文档的指定工作页的列属性
            //cinfo.Collapsed = true;
            ////设置列的范围 如 0列-10列
            //cinfo.ColumnIndexStart = 0;//列开始
            //cinfo.ColumnIndexEnd = 10;//列结束
            //cinfo.Collapsed = true;
            //cinfo.Width = 90 * 60;//列宽度
            //sheet.AddColumnInfo(cinfo);
            //设置文档列属性结束

            //设置指定工作页跨行跨列
            MergeArea ma = new MergeArea(1, 1, 1, 2);//从第1行跨到第二行，从第一列跨到第5列
            sheet.AddMergeArea(ma);
            ma = new MergeArea(1, 1, 1, 3);
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
            //Cell cell = cells.Add(1, 1, TestName, cellXF);//添加标题列返回一个列  参数：行 列 名称 样式对象
            Cell cell = cells.Add(1, 1, TestName, cellXF);

            //设置XY居中
            cell.HorizontalAlignment = HorizontalAlignments.Centered;
            cell.VerticalAlignment = VerticalAlignments.Centered;
            //设置字体
            cell.Font.Bold = true;//设置粗体
            cell.Font.ColorIndex = 0;//设置颜色码           
            cell.Font.FontFamily = FontFamilies.Roman;//设置字体 默认为宋体               
            //创建列结束  

            //创建数据
            QuestionInfo QuestionModel = new QuestionInfo();
            QuestionModel.IdCondition = CateId;
            if (Num != 0) QuestionModel.QuestionNum = " Top " + Num.ToString() + " * ";
            List<QuestionInfo> QuestionList = QuestionBLL.ReadList(QuestionModel);
            int LineNum = 1;
            int LineNum2 = 1;
            //题型的题号
            int StyleNum = 1;
            //题型号转为中文
            string StyleCNNum = string.Empty;
            //题型的中文名称
            string StyleName = string.Empty;

            for (int j = 1; j <= 3; j++)
            {

                List<QuestionInfo> StyleList = QuestionList.FindAll(delegate(QuestionInfo TempModel) { return TempModel.Style == j.ToString(); });
                if (StyleList.Count > 0)
                {
                    switch (StyleNum)
                    {
                        case 1:
                            StyleCNNum = "一";
                            break;
                        case 2:
                            StyleCNNum = "二";
                            break;
                        case 3:
                            StyleCNNum = "三";
                            break;
                    }
                    switch (j)
                    {
                        case 1:
                            StyleName = "单项选择题";
                            break;
                        case 2:
                            StyleName = "多项选择题";
                            break;
                        case 3:
                            StyleName = "判断题";
                            break;
                    }
                    if (Type == 2)
                    {
                        LineNum2 = LineNum2 + 2;
                    }
                    cells.Add(LineNum + 1, 1, StyleCNNum + "、");
                    cells.Add(LineNum + 1, 2, StyleName);
                    StyleNum++;
                    LineNum = LineNum + 2;


                    for (int i = 0; i < StyleList.Count; i++)
                    {
                        cells.Add(LineNum + 1, 1, i + 1 + "、");//添加列，不设置列属性就不用返回列对象
                        if (Type == 1)
                        {
                            cells.Add(LineNum + 1, 2, StyleList[i].Question);//添加列，不设置列属性就不用返回列对象

                            if (j != 3)
                            {
                                cells.Add(LineNum + 2, 1, "A、");
                                cells.Add(LineNum + 2, 2, StyleList[i].A);

                                cells.Add(LineNum + 3, 1, "B、");
                                cells.Add(LineNum + 3, 2, StyleList[i].B);

                                cells.Add(LineNum + 4, 1, "C、");
                                cells.Add(LineNum + 4, 2, StyleList[i].C);

                                cells.Add(LineNum + 5, 1, "D、");
                                cells.Add(LineNum + 5, 2, StyleList[i].D);

                                cells.Add(LineNum + 6, 1, "答案：");
                                cells.Add(LineNum + 6, 2, StyleList[i].Answer.ToUpper());

                                LineNum = LineNum + 7;
                            }
                            else
                            {
                                cells.Add(LineNum + 2, 1, "改错：");
                                cells.Add(LineNum + 2, 2, StyleList[i].A);
                                LineNum = LineNum + 3;
                            }

                        }
                    }
                }
            }
            
        }

        protected void ExcelOut(string CateId, string TestName, int Num, int Type)
        {
            //生成Excel开始
            string FilePath = "~/xml/Demo.xls";
            string NewTestName = "Sheet2";
            if (Type == 2) NewTestName = TestName + "答案";

            XlsDocument xls = new XlsDocument();//创建空xls文档

            xls.FileName = Server.MapPath(FilePath);//保存路径，如果直接发送到客户端的话只需要名称 生成名称

            Worksheet sheet = xls.Workbook.Worksheets.AddNamed(TestName); //创建一个工作页为Dome  
            Worksheet sheet2 = xls.Workbook.Worksheets.AddNamed(NewTestName);

            //设置文档列属性 
            //ColumnInfo cinfo = new ColumnInfo(xls, sheet);//设置xls文档的指定工作页的列属性
            //cinfo.Collapsed = true;
            ////设置列的范围 如 0列-10列
            //cinfo.ColumnIndexStart = 0;//列开始
            //cinfo.ColumnIndexEnd = 10;//列结束
            //cinfo.Collapsed = true;
            //cinfo.Width = 90 * 60;//列宽度
            //sheet.AddColumnInfo(cinfo);
            //设置文档列属性结束

            //设置指定工作页跨行跨列
            MergeArea ma = new MergeArea(1, 1, 1, 2);//从第1行跨到第二行，从第一列跨到第5列
            sheet.AddMergeArea(ma);
            ma = new MergeArea(1, 1, 1, 3);
            sheet2.AddMergeArea(ma);
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
            Cells cells2 = sheet2.Cells; //获得指定工作页列集合
            //列操作基本
            //Cell cell = cells.Add(1, 1, TestName, cellXF);//添加标题列返回一个列  参数：行 列 名称 样式对象
            Cell cell = cells.Add(1, 1, TestName, cellXF);
            if (Type == 2) cells2.Add(1, 1, NewTestName);

            //设置XY居中
            cell.HorizontalAlignment = HorizontalAlignments.Centered;
            cell.VerticalAlignment = VerticalAlignments.Centered;
            //设置字体
            cell.Font.Bold = true;//设置粗体
            cell.Font.ColorIndex = 0;//设置颜色码           
            cell.Font.FontFamily = FontFamilies.Roman;//设置字体 默认为宋体               
            //创建列结束  


            //创建数据

            QuestionInfo QuestionModel = new QuestionInfo();
            QuestionModel.IdCondition = CateId;
            if (Num != 0) QuestionModel.QuestionNum = " Top " + Num.ToString() + " * ";
            List<QuestionInfo> QuestionList = QuestionBLL.ReadList(QuestionModel);
            int LineNum = 1;
            int LineNum2 = 1;
            //题型的题号
            int StyleNum = 1;
            //题型号转为中文
            string StyleCNNum = string.Empty;
            //题型的中文名称
            string StyleName = string.Empty;

            for (int j = 1; j <= 3; j++)
            {

                List<QuestionInfo> StyleList = QuestionList.FindAll(delegate(QuestionInfo TempModel) { return TempModel.Style == j.ToString(); });
                if (StyleList.Count > 0)
                {
                    switch (StyleNum)
                    {
                        case 1:
                            StyleCNNum = "一";
                            break;
                        case 2:
                            StyleCNNum = "二";
                            break;
                        case 3:
                            StyleCNNum = "三";
                            break;
                    }
                    switch (j)
                    {
                        case 1:
                            StyleName = "单项选择题";
                            break;
                        case 2:
                            StyleName = "多项选择题";
                            break;
                        case 3:
                            StyleName = "判断题";
                            break;
                    }
                    if (Type == 2)
                    {
                        cells2.Add(LineNum2 + 1, 1, StyleCNNum + "、");
                        cells2.Add(LineNum2 + 1, 2, StyleName);
                        LineNum2 = LineNum2 + 2;
                    }
                    cells.Add(LineNum + 1, 1, StyleCNNum + "、");
                    cells.Add(LineNum + 1, 2, StyleName);
                    StyleNum++;
                    LineNum = LineNum + 2;


                    for (int i = 0; i < StyleList.Count; i++)
                    {
                        cells.Add(LineNum + 1, 1, i + 1 + "、");//添加列，不设置列属性就不用返回列对象
                        if (Type == 2) cells2.Add(LineNum2 + 1, 2, i + 1 + "、");
                        if (Type == 1)
                        {
                            cells.Add(LineNum + 1, 2, StyleList[i].Question);//添加列，不设置列属性就不用返回列对象

                            if (j != 3)
                            {
                                cells.Add(LineNum + 2, 1, "A、");
                                cells.Add(LineNum + 2, 2, StyleList[i].A);

                                cells.Add(LineNum + 3, 1, "B、");
                                cells.Add(LineNum + 3, 2, StyleList[i].B);

                                cells.Add(LineNum + 4, 1, "C、");
                                cells.Add(LineNum + 4, 2, StyleList[i].C);

                                cells.Add(LineNum + 5, 1, "D、");
                                cells.Add(LineNum + 5, 2, StyleList[i].D);

                                cells.Add(LineNum + 6, 1, "答案：");
                                cells.Add(LineNum + 6, 2, StyleList[i].Answer.ToUpper());

                                LineNum = LineNum + 7;
                            }
                            else
                            {
                                cells.Add(LineNum + 2, 1, "改错：");
                                cells.Add(LineNum + 2, 2, StyleList[i].A);
                                LineNum = LineNum + 3;
                            }

                        }
                        else if (Type == 2)
                        {
                            cells.Add(LineNum + 1, 2, StyleList[i].Question.Replace("~", ""));//添加列，不设置列属性就不用返回列对象

                            if (j != 3)
                            {
                                cells.Add(LineNum + 2, 2, "A、" + StyleList[i].A);

                                cells.Add(LineNum + 3, 2, "B、" + StyleList[i].B);

                                cells.Add(LineNum + 4, 2, "C、" + StyleList[i].C);

                                cells.Add(LineNum + 5, 2, "D、" + StyleList[i].D);

                                LineNum = LineNum + 6;


                                cells2.Add(LineNum2 + 1, 3, StyleList[i].Answer);
                            }
                            else
                            {
                                cells.Add(LineNum + 2, 2, "改错：");
                                LineNum = LineNum + 3;

                                if (StyleList[i].Answer == "0")
                                {
                                    cells2.Add(LineNum2 + 1, 3, StyleList[i].A);
                                }
                                else
                                {
                                    cells2.Add(LineNum2 + 1, 3, "√");
                                }
                            }

                            LineNum2 = LineNum2 + 1;

                        }
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

        protected void QuestionOk_Click(object sender, EventArgs e)
        {
            if (ErrQuestionList.Count == 0)
            {
                if (QuestionList.Count > 0)
                {
                    QuestionOk.Enabled = false;
                    int QuestionNum = 0;
                    foreach (QuestionInfo Item in QuestionList)
                    {
                        Item.CateId = CateId;
                        Item.CompanyId = 0;
                        if (Item.Style != "0" && !string.IsNullOrEmpty(Item.Style))
                        {
                            QuestionBLL.AddQuestion(Item);
                            QuestionNum++;
                        }
                    }
                    ScriptHelper.Alert("成功导入" + QuestionNum.ToString() + "道试题！");
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


    }
}
