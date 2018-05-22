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
                    CourseName.Text = CourseBLL.ReadCourse(CateId).CourseName + "��������Ŀ������" + QuestionBLL.ReadList(CateId).Count.ToString() + "��";
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
                    CourseName.Text = "��������";
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
                XlsDocument xls = new XlsDocument();//������xls�ĵ�
                xls.FileName = Server.MapPath(FilePath);//����·�������ֱ�ӷ��͵��ͻ��˵Ļ�ֻ��Ҫ���� ��������

                foreach (string courseID in courseIDStr.Split(','))
                {
                    string courseName = CourseBLL.ReadCourse(int.Parse(courseID)).CourseName;
                    ExcelOut(xls, courseID, courseName, QuestionOutNum, 1);
                }

                //���ɱ��浽������������ڲ��Ḳ�ǲ��ұ��쳣������ɾ���ڱ����µ�
                if (File.Exists(Server.MapPath(FilePath)))
                {
                    File.Delete(Server.MapPath(FilePath));//ɾ��
                }
                //�����ĵ�
                xls.Save(Server.MapPath(FilePath));//���浽������
                xls.Send();//���͵��ͻ���
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
            //����Excel
            QuestionList.Clear();
            ErrQuestionList.Clear();

            //����Ҫ�����Excel
            XlsDocument xls = new XlsDocument(Path);//�����ⲿExcel
            //���Excel�е�ָ��һ������ҳ

            Worksheet sheet = xls.Workbook.Worksheets[0];
            //List<QuestionInfo> QuestionList = new List<QuestionInfo>();
            //List<QuestionInfo> ErrQuestionList = new List<QuestionInfo>();
            int StyleNum = 0;//���ͺ�
            string cell2 = string.Empty;
            string cellA = string.Empty;
            string cellB = string.Empty;
            string cellC = string.Empty;
            string cellD = string.Empty;
            string cellAnswer = string.Empty;
            bool Checked = true;

            //��ȡ���� ѭ��ÿsheet����ҳ��ÿһ��,����ȡǰһ��
            for (int i = 2; i < (sheet.Rows.Count + 10); i++)
            {
                Checked = true;
                try
                {
                    //��ǰ�еĶ���Ϊ��ʱ��ִ��
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
                            case "����ѡ����":
                                StyleNum = 1;
                                break;
                            case "����ѡ����":
                                StyleNum = 2;
                                break;
                            case "�ж���":
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

                                //���ж��Ƿ����A B C Dѡ��
                                if (cellA.Contains("A") && cellB.Contains("B") && cellC.Contains("C") && cellD.Contains("D"))
                                {
                                    //�жϴ��Ƿ���ϱ�׼
                                    if (StyleNum == 1)//��Ϊ��ѡʱ���𰸳���Ϊ1
                                    {
                                        if (cellAnswer.Length != 1)
                                        {
                                            Checked = false;
                                        }
                                    }
                                    else if (StyleNum == 2)
                                    {
                                        if (cellAnswer.Length > 4 || cellAnswer.Length <= 1)//��ѡʱ�𰸳��Ȳ�����4
                                        {
                                            Checked = false;
                                        }
                                    }

                                    //��û������ټ���ĸ�ѡ��ֵ��������
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
                                            QuestionModel.Answer = "ѡ�����������";
                                            ErrQuestionList.Add(QuestionModel);
                                        }
                                    }
                                    else
                                    {
                                        QuestionInfo QuestionModel = new QuestionInfo();
                                        QuestionModel.Question = cell2.Trim();
                                        QuestionModel.Answer = "�𰸺���������";
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
                                if (cellA.Contains("�Ĵ�"))
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
                                    //�����ڴ������иĴ����� ���� û�д���Ҳû�иĴ����� ʱ����Ϊ����
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
                                        QuestionModel.Answer = "~���������Ի���𰸲�һ��";
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
            Worksheet sheet = xls.Workbook.Worksheets.AddNamed(TestName); //����һ������ҳΪDome  

            //�����ĵ������� 
            //ColumnInfo cinfo = new ColumnInfo(xls, sheet);//����xls�ĵ���ָ������ҳ��������
            //cinfo.Collapsed = true;
            ////�����еķ�Χ �� 0��-10��
            //cinfo.ColumnIndexStart = 0;//�п�ʼ
            //cinfo.ColumnIndexEnd = 10;//�н���
            //cinfo.Collapsed = true;
            //cinfo.Width = 90 * 60;//�п��
            //sheet.AddColumnInfo(cinfo);
            //�����ĵ������Խ���

            //����ָ������ҳ���п���
            MergeArea ma = new MergeArea(1, 1, 1, 2);//�ӵ�1�п絽�ڶ��У��ӵ�һ�п絽��5��
            sheet.AddMergeArea(ma);
            ma = new MergeArea(1, 1, 1, 3);
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
            //Cell cell = cells.Add(1, 1, TestName, cellXF);//��ӱ����з���һ����  �������� �� ���� ��ʽ����
            Cell cell = cells.Add(1, 1, TestName, cellXF);

            //����XY����
            cell.HorizontalAlignment = HorizontalAlignments.Centered;
            cell.VerticalAlignment = VerticalAlignments.Centered;
            //��������
            cell.Font.Bold = true;//���ô���
            cell.Font.ColorIndex = 0;//������ɫ��           
            cell.Font.FontFamily = FontFamilies.Roman;//�������� Ĭ��Ϊ����               
            //�����н���  

            //��������
            QuestionInfo QuestionModel = new QuestionInfo();
            QuestionModel.IdCondition = CateId;
            if (Num != 0) QuestionModel.QuestionNum = " Top " + Num.ToString() + " * ";
            List<QuestionInfo> QuestionList = QuestionBLL.ReadList(QuestionModel);
            int LineNum = 1;
            int LineNum2 = 1;
            //���͵����
            int StyleNum = 1;
            //���ͺ�תΪ����
            string StyleCNNum = string.Empty;
            //���͵���������
            string StyleName = string.Empty;

            for (int j = 1; j <= 3; j++)
            {

                List<QuestionInfo> StyleList = QuestionList.FindAll(delegate(QuestionInfo TempModel) { return TempModel.Style == j.ToString(); });
                if (StyleList.Count > 0)
                {
                    switch (StyleNum)
                    {
                        case 1:
                            StyleCNNum = "һ";
                            break;
                        case 2:
                            StyleCNNum = "��";
                            break;
                        case 3:
                            StyleCNNum = "��";
                            break;
                    }
                    switch (j)
                    {
                        case 1:
                            StyleName = "����ѡ����";
                            break;
                        case 2:
                            StyleName = "����ѡ����";
                            break;
                        case 3:
                            StyleName = "�ж���";
                            break;
                    }
                    if (Type == 2)
                    {
                        LineNum2 = LineNum2 + 2;
                    }
                    cells.Add(LineNum + 1, 1, StyleCNNum + "��");
                    cells.Add(LineNum + 1, 2, StyleName);
                    StyleNum++;
                    LineNum = LineNum + 2;


                    for (int i = 0; i < StyleList.Count; i++)
                    {
                        cells.Add(LineNum + 1, 1, i + 1 + "��");//����У������������ԾͲ��÷����ж���
                        if (Type == 1)
                        {
                            cells.Add(LineNum + 1, 2, StyleList[i].Question);//����У������������ԾͲ��÷����ж���

                            if (j != 3)
                            {
                                cells.Add(LineNum + 2, 1, "A��");
                                cells.Add(LineNum + 2, 2, StyleList[i].A);

                                cells.Add(LineNum + 3, 1, "B��");
                                cells.Add(LineNum + 3, 2, StyleList[i].B);

                                cells.Add(LineNum + 4, 1, "C��");
                                cells.Add(LineNum + 4, 2, StyleList[i].C);

                                cells.Add(LineNum + 5, 1, "D��");
                                cells.Add(LineNum + 5, 2, StyleList[i].D);

                                cells.Add(LineNum + 6, 1, "�𰸣�");
                                cells.Add(LineNum + 6, 2, StyleList[i].Answer.ToUpper());

                                LineNum = LineNum + 7;
                            }
                            else
                            {
                                cells.Add(LineNum + 2, 1, "�Ĵ�");
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
            //����Excel��ʼ
            string FilePath = "~/xml/Demo.xls";
            string NewTestName = "Sheet2";
            if (Type == 2) NewTestName = TestName + "��";

            XlsDocument xls = new XlsDocument();//������xls�ĵ�

            xls.FileName = Server.MapPath(FilePath);//����·�������ֱ�ӷ��͵��ͻ��˵Ļ�ֻ��Ҫ���� ��������

            Worksheet sheet = xls.Workbook.Worksheets.AddNamed(TestName); //����һ������ҳΪDome  
            Worksheet sheet2 = xls.Workbook.Worksheets.AddNamed(NewTestName);

            //�����ĵ������� 
            //ColumnInfo cinfo = new ColumnInfo(xls, sheet);//����xls�ĵ���ָ������ҳ��������
            //cinfo.Collapsed = true;
            ////�����еķ�Χ �� 0��-10��
            //cinfo.ColumnIndexStart = 0;//�п�ʼ
            //cinfo.ColumnIndexEnd = 10;//�н���
            //cinfo.Collapsed = true;
            //cinfo.Width = 90 * 60;//�п��
            //sheet.AddColumnInfo(cinfo);
            //�����ĵ������Խ���

            //����ָ������ҳ���п���
            MergeArea ma = new MergeArea(1, 1, 1, 2);//�ӵ�1�п絽�ڶ��У��ӵ�һ�п絽��5��
            sheet.AddMergeArea(ma);
            ma = new MergeArea(1, 1, 1, 3);
            sheet2.AddMergeArea(ma);
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
            Cells cells2 = sheet2.Cells; //���ָ������ҳ�м���
            //�в�������
            //Cell cell = cells.Add(1, 1, TestName, cellXF);//��ӱ����з���һ����  �������� �� ���� ��ʽ����
            Cell cell = cells.Add(1, 1, TestName, cellXF);
            if (Type == 2) cells2.Add(1, 1, NewTestName);

            //����XY����
            cell.HorizontalAlignment = HorizontalAlignments.Centered;
            cell.VerticalAlignment = VerticalAlignments.Centered;
            //��������
            cell.Font.Bold = true;//���ô���
            cell.Font.ColorIndex = 0;//������ɫ��           
            cell.Font.FontFamily = FontFamilies.Roman;//�������� Ĭ��Ϊ����               
            //�����н���  


            //��������

            QuestionInfo QuestionModel = new QuestionInfo();
            QuestionModel.IdCondition = CateId;
            if (Num != 0) QuestionModel.QuestionNum = " Top " + Num.ToString() + " * ";
            List<QuestionInfo> QuestionList = QuestionBLL.ReadList(QuestionModel);
            int LineNum = 1;
            int LineNum2 = 1;
            //���͵����
            int StyleNum = 1;
            //���ͺ�תΪ����
            string StyleCNNum = string.Empty;
            //���͵���������
            string StyleName = string.Empty;

            for (int j = 1; j <= 3; j++)
            {

                List<QuestionInfo> StyleList = QuestionList.FindAll(delegate(QuestionInfo TempModel) { return TempModel.Style == j.ToString(); });
                if (StyleList.Count > 0)
                {
                    switch (StyleNum)
                    {
                        case 1:
                            StyleCNNum = "һ";
                            break;
                        case 2:
                            StyleCNNum = "��";
                            break;
                        case 3:
                            StyleCNNum = "��";
                            break;
                    }
                    switch (j)
                    {
                        case 1:
                            StyleName = "����ѡ����";
                            break;
                        case 2:
                            StyleName = "����ѡ����";
                            break;
                        case 3:
                            StyleName = "�ж���";
                            break;
                    }
                    if (Type == 2)
                    {
                        cells2.Add(LineNum2 + 1, 1, StyleCNNum + "��");
                        cells2.Add(LineNum2 + 1, 2, StyleName);
                        LineNum2 = LineNum2 + 2;
                    }
                    cells.Add(LineNum + 1, 1, StyleCNNum + "��");
                    cells.Add(LineNum + 1, 2, StyleName);
                    StyleNum++;
                    LineNum = LineNum + 2;


                    for (int i = 0; i < StyleList.Count; i++)
                    {
                        cells.Add(LineNum + 1, 1, i + 1 + "��");//����У������������ԾͲ��÷����ж���
                        if (Type == 2) cells2.Add(LineNum2 + 1, 2, i + 1 + "��");
                        if (Type == 1)
                        {
                            cells.Add(LineNum + 1, 2, StyleList[i].Question);//����У������������ԾͲ��÷����ж���

                            if (j != 3)
                            {
                                cells.Add(LineNum + 2, 1, "A��");
                                cells.Add(LineNum + 2, 2, StyleList[i].A);

                                cells.Add(LineNum + 3, 1, "B��");
                                cells.Add(LineNum + 3, 2, StyleList[i].B);

                                cells.Add(LineNum + 4, 1, "C��");
                                cells.Add(LineNum + 4, 2, StyleList[i].C);

                                cells.Add(LineNum + 5, 1, "D��");
                                cells.Add(LineNum + 5, 2, StyleList[i].D);

                                cells.Add(LineNum + 6, 1, "�𰸣�");
                                cells.Add(LineNum + 6, 2, StyleList[i].Answer.ToUpper());

                                LineNum = LineNum + 7;
                            }
                            else
                            {
                                cells.Add(LineNum + 2, 1, "�Ĵ�");
                                cells.Add(LineNum + 2, 2, StyleList[i].A);
                                LineNum = LineNum + 3;
                            }

                        }
                        else if (Type == 2)
                        {
                            cells.Add(LineNum + 1, 2, StyleList[i].Question.Replace("~", ""));//����У������������ԾͲ��÷����ж���

                            if (j != 3)
                            {
                                cells.Add(LineNum + 2, 2, "A��" + StyleList[i].A);

                                cells.Add(LineNum + 3, 2, "B��" + StyleList[i].B);

                                cells.Add(LineNum + 4, 2, "C��" + StyleList[i].C);

                                cells.Add(LineNum + 5, 2, "D��" + StyleList[i].D);

                                LineNum = LineNum + 6;


                                cells2.Add(LineNum2 + 1, 3, StyleList[i].Answer);
                            }
                            else
                            {
                                cells.Add(LineNum + 2, 2, "�Ĵ�");
                                LineNum = LineNum + 3;

                                if (StyleList[i].Answer == "0")
                                {
                                    cells2.Add(LineNum2 + 1, 3, StyleList[i].A);
                                }
                                else
                                {
                                    cells2.Add(LineNum2 + 1, 3, "��");
                                }
                            }

                            LineNum2 = LineNum2 + 1;

                        }
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
                    ScriptHelper.Alert("�ɹ�����" + QuestionNum.ToString() + "�����⣡");
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


    }
}
