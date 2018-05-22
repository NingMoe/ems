using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace XueFu.EntLib
{
    public abstract class ExcelHelper
    {
        
        private Dictionary<int[], string> cellParameters = new Dictionary<int[], string>();
        private System.Data.DataTable dt = new System.Data.DataTable();
        private int left = 0;
        private object missing = Missing.Value;
        private string outputFile = string.Empty;
        private int rows = 10;
        private string sheetPrefixName = "Sheet";
        private string templetFile = string.Empty;
        private int top = 0;
        private Dictionary<string, string> variableParameters = new Dictionary<string, string>();

        
        public ExcelHelper(string templetFilePath, string outputFilePath)
        {
            if (templetFilePath == string.Empty)
            {
                throw new Exception("Excelģ���ļ�·������Ϊ�գ�");
            }
            if (outputFilePath == string.Empty)
            {
                throw new Exception("���Excel�ļ�·������Ϊ�գ�");
            }
            string path = outputFilePath.Substring(0, outputFilePath.LastIndexOf(@"\"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(templetFilePath))
            {
                throw new Exception("ָ��·����Excelģ���ļ������ڣ�");
            }
            this.templetFile = templetFilePath;
            this.outputFile = outputFilePath;
        }

        public void DataTableToExcel()
        {
            int count = this.dt.Rows.Count;
            int sheetCount = this.GetSheetCount(count);
            DateTime now = DateTime.Now;
            Application o = new ApplicationClass();
            o.Visible = false;
            DateTime time2 = DateTime.Now;
            Workbook workBook = o.Workbooks.Open(this.templetFile, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing, this.missing);
            Worksheet worksheet = (Worksheet)workBook.Sheets.get_Item(1);
            for (int i = 1; i < sheetCount; i++)
            {
                worksheet.Copy(this.missing, workBook.Worksheets[i]);
            }
            this.FillData(workBook, sheetCount);
            worksheet.Activate();
            try
            {
                workBook.SaveAs(this.outputFile, this.missing, this.missing, this.missing, this.missing, this.missing, XlSaveAsAccessMode.xlExclusive, this.missing, this.missing, this.missing, this.missing, this.missing);
                workBook.Close(null, null, null);
                o.Workbooks.Close();
                o.Application.Quit();
                o.Quit();
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(workBook);
                Marshal.ReleaseComObject(o);
                worksheet = null;
                workBook = null;
                o = null;
                GC.Collect();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                Process[] processesByName = Process.GetProcessesByName("Excel");
                foreach (Process process in processesByName)
                {
                    DateTime startTime = process.StartTime;
                    if ((startTime > now) && (startTime < time2))
                    {
                        process.Kill();
                    }
                }
            }
        }

        protected abstract void FillData(Workbook workBook, int sheetCount);
        private int GetSheetCount(int rowCount)
        {
            int num = rowCount % this.rows;
            if (num == 0)
            {
                return (rowCount / this.rows);
            }
            return (Convert.ToInt32((int)(rowCount / this.rows)) + 1);
        }

        protected void SetCellParameters(Worksheet sheet)
        {
            foreach (KeyValuePair<int[], string> pair in this.cellParameters)
            {
                try
                {
                    sheet.Cells[(int)pair.Key[0], (int)pair.Key[1]] = pair.Value;
                }
                catch
                {
                    throw new Exception(string.Concat(new object[] { "��Ԫ��", (int)pair.Key[0], ",", (int)pair.Key[1], "��������" }));
                }
            }
        }

        protected void SetVariableParameters(Worksheet sheet)
        {
            foreach (KeyValuePair<string, string> pair in this.variableParameters)
            {
                try
                {
                    ((TextBox)sheet.TextBoxes(pair.Key)).Text = pair.Value;
                }
                catch
                {
                    throw new Exception("����Ϊ" + pair.Key + "�ĵ�Ԫ�񲻴���");
                }
            }
        }

        
        public Dictionary<int[], string> CellParameters
        {
            get
            {
                return this.cellParameters;
            }
            set
            {
                this.cellParameters = value;
            }
        }

        public System.Data.DataTable Dt
        {
            get
            {
                return this.dt;
            }
            set
            {
                this.dt = value;
            }
        }

        public int Left
        {
            get
            {
                return this.left;
            }
            set
            {
                this.left = value;
            }
        }

        public int Rows
        {
            get
            {
                return this.rows;
            }
            set
            {
                this.rows = value;
            }
        }

        public string SheetPrefixName
        {
            get
            {
                return this.sheetPrefixName;
            }
            set
            {
                this.sheetPrefixName = value;
            }
        }

        public int Top
        {
            get
            {
                return this.top;
            }
            set
            {
                this.top = value;
            }
        }

        public Dictionary<string, string> VariableParameters
        {
            get
            {
                return this.variableParameters;
            }
            set
            {
                this.variableParameters = value;
            }
        }
    }
}
