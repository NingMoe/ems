using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace XueFu.EntLib
{
    public sealed class MultiUnlimitControl : Control
    {
        private MultiUnlimitClass multiUnlimitClass = new MultiUnlimitClass();

        protected override void Render(HtmlTextWriter output)
        {
            output.Write(this.multiUnlimitClass.ShowContent());
        }

        public string ClassIDList
        {
            get
            {
                return this.multiUnlimitClass.ClassIDList;
            }
            set
            {
                this.multiUnlimitClass.ClassIDList = value;
            }
        }

        public List<UnlimitClassInfo> DataSource
        {
            set
            {
                this.multiUnlimitClass.DataSource = value;
            }
        }

        public string Prefix
        {
            set
            {
                this.multiUnlimitClass.Prefix = value;
            }
        }
    }

    public sealed class MultiUnlimitClass
    {
        private string classIDList = string.Empty;
        private string prefix = string.Empty;
        private SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();

        private string ReadUnlimitClassName(int classID)
        {
            string className = string.Empty;
            foreach (UnlimitClassInfo info in this.singleUnlimitClass.DataSource)
            {
                if (info.ClassID == classID)
                {
                    className = info.ClassName;
                }
            }
            return className;
        }

        private string ReadUnlimitClassNameByIDList()
        {
            string str = string.Empty;
            string str2 = string.Empty;
            if (this.classIDList != string.Empty)
            {
                str2 = this.classIDList.Substring(1, this.classIDList.Length - 2);
            }
            str2 = str2.Replace("||", "#");
            if (str2.Length > 0)
            {
                foreach (string str3 in str2.Split(new char[] { '#' }))
                {
                    string str4 = string.Empty;
                    foreach (string str5 in str3.Split(new char[] { '|' }))
                    {
                        if (str4 == string.Empty)
                        {
                            str4 = this.ReadUnlimitClassName(Convert.ToInt32(str5));
                        }
                        else
                        {
                            str4 = str4 + " > " + this.ReadUnlimitClassName(Convert.ToInt32(str5));
                        }
                    }
                    if (str4 != string.Empty)
                    {
                        string str7 = str;
                        str = str7 + "<p id=\"" + this.prefix + "SelectUnlimitClass|" + str3 + "|\">" + str4 + "&nbsp;<a href=\"javascript:deleteUnlimitClass('|" + str3 + "|','" + this.prefix + "')\" title=\"É¾³ý\"><img src=\"images/delete.gif\" /></a></p>";
                    }
                }
            }
            return str;
        }

        public string ShowContent()
        {
            string str3 = string.Empty;
            str3 = str3 + "<p><span id=\"UnlimitClassContent\">" + this.singleUnlimitClass.ShowContent() + "</span>&nbsp;<input type=\"button\" class=\"button\" value=\"Ìí¼Ó\" onclick=\"addUnlimitClass('" + this.prefix + "')\" />&nbsp;</p>";
            str3 = str3 + "<input type=\"hidden\" value=\"" + this.classIDList + "\" id=\"" + this.prefix + "UnlimitClassValue\" name=\"" + this.prefix + "UnlimitClassValue\" />";
            return (str3 + "<p id=\"" + this.prefix + "SelectUnlimitClass\">" + this.ReadUnlimitClassNameByIDList() + "</p>");
        }

        public string ClassIDList
        {
            get
            {
                return RequestHelper.GetForm<string>(this.prefix + "UnlimitClassValue");
            }
            set
            {
                this.classIDList = value;
            }
        }

        public List<UnlimitClassInfo> DataSource
        {
            set
            {
                this.singleUnlimitClass.DataSource = value;
            }
        }

        public string Prefix
        {
            set
            {
                this.prefix = value;
                this.singleUnlimitClass.Prefix = value;
            }
        }
    }
        
    public class UnlimitClassInfo
    {
        private int classID = 0;
        private string className = string.Empty;
        private int fatherID = 0;

        public int ClassID
        {
            get
            {
                return this.classID;
            }
            set
            {
                this.classID = value;
            }
        }

        public string ClassName
        {
            get
            {
                return this.className;
            }
            set
            {
                this.className = value;
            }
        }

        public int FatherID
        {
            get
            {
                return this.fatherID;
            }
            set
            {
                this.fatherID = value;
            }
        }
    }
}
