using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace XueFu.EntLib
{
    public sealed class SingleUnlimitControl : WebControl
    {
        private SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();

        protected override void Render(HtmlTextWriter output)
        {
            output.Write(this.singleUnlimitClass.ShowContent());
        }

        public string ClassID
        {
            get
            {
                return this.singleUnlimitClass.ClassID;
            }
            set
            {
                this.singleUnlimitClass.ClassID = value;
            }
        }

        public List<UnlimitClassInfo> DataSource
        {
            get
            {
                return this.singleUnlimitClass.DataSource;
            }
            set
            {
                this.singleUnlimitClass.DataSource = value;
            }
        }

        public int FatherID
        {
            get
            {
                return this.singleUnlimitClass.FatherID;
            }
        }

        public string FunctionName
        {
            set
            {
                this.singleUnlimitClass.FunctionName = value;
            }
        }

        public string Prefix
        {
            set
            {
                this.singleUnlimitClass.Prefix = value;
            }
        }

        public int RootID
        {
            get
            {
                return this.singleUnlimitClass.RootID;
            }
        }
    }

    public sealed class SingleUnlimitClass
    {
        private string classID = string.Empty;
        private List<UnlimitClassInfo> dataSource = new List<UnlimitClassInfo>();
        private string functionName = string.Empty;
        private string prefix = string.Empty;

        private static string ReadUnlimitClassIsSelect(int classID, string classIDList)
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(classIDList))
            {
                return string.Empty;
            }
            if (classIDList.IndexOf("|" + classID.ToString() + "|") > -1)
            {
                str = "  selected=\"selected\"";
            }
            return str;
        }

        private List<UnlimitClassInfo> ReadUnlimitClassListByFatherID(int fatherID)
        {
            List<UnlimitClassInfo> list = new List<UnlimitClassInfo>();
            foreach (UnlimitClassInfo info in this.dataSource)
            {
                if (info.FatherID == fatherID)
                {
                    list.Add(info);
                }
            }
            return list;
        }

        public string ShowContent()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<span id=\"" + this.prefix + "FatherUnlimitClass\">");
            builder.Append("<select name=\"" + this.prefix + "UnlimitClass1\" id=\"" + this.prefix + "UnlimitClass1\" onchange=\"fatherUnlimitClassChange(1,'" + this.prefix + "','" + this.functionName + "')\">");
            builder.Append("<option value=\"0\">«Î—°‘Ò</option>");
            foreach (UnlimitClassInfo info in this.ReadUnlimitClassListByFatherID(0))
            {
                builder.Append(string.Concat(new object[] { "<option value=\"", info.ClassID, "\"", ReadUnlimitClassIsSelect(info.ClassID, this.classID), ">", info.ClassName, "</option>" }));
            }
            builder.Append("</select>");
            int num = 1;
            string[] strArray = this.classID.Split(new char[] { '|' });
            if (strArray.Length >= 3)
            {
                for (int i = 1; i < (strArray.Length - 1); i++)
                {
                    int fatherID = Convert.ToInt32(strArray[i]);
                    List<UnlimitClassInfo> list = this.ReadUnlimitClassListByFatherID(fatherID);
                    if (list.Count > 0)
                    {
                        num++;
                        builder.Append(string.Concat(new object[] { "<select  name=\"", this.prefix, "UnlimitClass", num.ToString(), "\" id=\"", this.prefix, "UnlimitClass", num, "\" onchange=\"fatherUnlimitClassChange(", num.ToString(), ",'", this.prefix, "','", this.functionName, "')\">" }));
                        builder.Append("<option value=\"0\" >«Î—°‘Ò</option>");
                        foreach (UnlimitClassInfo info in list)
                        {
                            builder.Append(string.Concat(new object[] { "<option value=\"", info.ClassID, "\"", ReadUnlimitClassIsSelect(info.ClassID, this.classID), ">", info.ClassName, "</option>" }));
                        }
                        builder.Append("</select>");
                    }
                }
            }
            builder.Append(string.Concat(new object[] { "<input type=\"hidden\" id=\"", this.prefix, "UnlimitClassGradeCount\" name=\"", this.prefix, "UnlimitClassGradeCount\" value=\"", num, "\" />" }));
            builder.Append("</span>");
            builder.Append("<script language=\"javascript\" type=\"text/javascript\">");
            builder.Append("if(unlimitClassData==null){");
            builder.Append("    var unlimitClassData = [];");
            builder.Append("}");
            StringBuilder builder2 = new StringBuilder();
            foreach (UnlimitClassInfo info in this.dataSource)
            {
                builder2.Append(string.Concat(new object[] { "[", info.ClassID, ",'", info.ClassName, "',", info.FatherID, ",'", this.prefix, "']," }));
            }
            string str = builder2.ToString();
            if (str != string.Empty)
            {
                str = str.Substring(0, str.Length - 1);
            }
            builder.Append("unlimitClassData.push(" + str + ");");
            builder.Append("</script>");
            return builder.ToString();
        }

        public string ClassID
        {
            get
            {
                this.classID = string.Empty;
                int form = RequestHelper.GetForm<int>(this.prefix + "UnlimitClassGradeCount");
                for (int i = 1; i <= form; i++)
                {
                    string str = RequestHelper.GetForm<string>(this.prefix + "UnlimitClass" + i);
                    if (str != "0")
                    {
                        if (this.classID == string.Empty)
                        {
                            this.classID = "|" + str + "|";
                        }
                        else
                        {
                            this.classID = this.classID + str + "|";
                        }
                    }
                }
                return this.classID;
            }
            set
            {
                this.classID = value;
            }
        }

        public List<UnlimitClassInfo> DataSource
        {
            get
            {
                return this.dataSource;
            }
            set
            {
                this.dataSource = value;
            }
        }

        public int FatherID
        {
            get
            {
                int form = 0;
                for (int i = RequestHelper.GetForm<int>(this.prefix + "UnlimitClassGradeCount"); i > 0; i--)
                {
                    form = RequestHelper.GetForm<int>(this.prefix + "UnlimitClass" + i);
                    if (form > 0)
                    {
                        return form;
                    }
                }
                return form;
            }
        }

        public string FunctionName
        {
            set
            {
                this.functionName = value;
            }
        }

        public string Prefix
        {
            set
            {
                this.prefix = value;
            }
        }

        public int RootID
        {
            get
            {
                return RequestHelper.GetForm<int>(this.prefix + "UnlimitClass1");
            }
        }
    }
}
