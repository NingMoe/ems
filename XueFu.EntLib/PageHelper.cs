using System;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace XueFu.EntLib
{
    public enum OrderType
    {
        Desc,
        Asc
    }

    ///分页公用类
    public abstract class MssqlPagerClass
    {
        
        private int currentPage = 1;
        private string fields;
        private MssqlCondition mssqlCondition = new MssqlCondition();
        private string orderField = "ID";
        private OrderType orderType = OrderType.Desc;
        private int pageSize = 10;
        private string tableName;

        
        protected MssqlPagerClass()
        {
        }

        public abstract DataTable ExecuteDataTable();
        public abstract SqlDataReader ExecuteReader();
        protected SqlParameter[] PrepareCountParameter()
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@tableName", SqlDbType.NVarChar), new SqlParameter("@condition", SqlDbType.NVarChar) };
            parameterArray[0].Value = this.TableName;
            parameterArray[1].Value = this.mssqlCondition.ToString();
            return parameterArray;
        }

        protected SqlParameter[] PrepareParameter()
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@tableName", SqlDbType.NVarChar), new SqlParameter("@fields", SqlDbType.NVarChar), new SqlParameter("@pageSize", SqlDbType.Int), new SqlParameter("@currentPage", SqlDbType.Int), new SqlParameter("@fieldName", SqlDbType.NVarChar), new SqlParameter("@orderType", SqlDbType.Bit), new SqlParameter("@condition", SqlDbType.NVarChar) };
            parameterArray[0].Value = this.TableName;
            parameterArray[1].Value = this.Fields;
            parameterArray[2].Value = this.PageSize;
            parameterArray[3].Value = this.CurrentPage;
            parameterArray[4].Value = this.OrderField;
            parameterArray[5].Value = this.OrderType;
            parameterArray[6].Value = this.mssqlCondition.ToString();
            return parameterArray;
        }

        
        public int CurrentPage
        {
            get { return this.currentPage; }
            set
            {
                if (value > 0)
                {
                    this.currentPage = value;
                }
            }
        }

        public string Fields
        {
            get { return this.fields; }
            set { this.fields = value; }
        }

        public MssqlCondition MssqlCondition
        {
            get { return this.mssqlCondition; }
            set { this.mssqlCondition = value; }
        }

        public string OrderField
        {
            get { return this.orderField; }
            set { this.orderField = value; }
        }

        public OrderType OrderType
        {
            get { return this.orderType; }
            set { this.orderType = value; }
        }

        public int PageSize
        {
            get { return this.pageSize; }
            set { this.pageSize = value; }
        }

        public string TableName
        {
            get { return this.tableName; }
            set { this.tableName = value; }
        }
    }

    [DefaultProperty(""), ToolboxData("<{0}:Page runat=server></{0}:Page>")]
    public abstract class BasePager : Control
    {
        private BasePagerClass basePagerClass;

        protected BasePager()
        {
        }

        protected BasePagerClass BasePagerClass
        {
            get
            {
                return this.basePagerClass;
            }
            set
            {
                this.basePagerClass = value;
            }
        }

        [DefaultValue(""), Bindable(true), Category("Appearance")]
        public int Count
        {
            get
            {
                return this.basePagerClass.Count;
            }
            set
            {
                this.basePagerClass.Count = value;
            }
        }

        [Category("Appearance"), Bindable(true), DefaultValue("")]
        public int CurrentPage
        {
            get
            {
                return this.basePagerClass.CurrentPage;
            }
            set
            {
                this.basePagerClass.CurrentPage = value;
            }
        }

        [Bindable(true), DefaultValue(""), Category("Appearance")]
        public bool DisCount
        {
            get
            {
                return this.basePagerClass.DisCount;
            }
            set
            {
                this.basePagerClass.DisCount = value;
            }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string FirstPage
        {
            get
            {
                return this.basePagerClass.FirstPage;
            }
            set
            {
                this.basePagerClass.FirstPage = value;
            }
        }

        [Bindable(true), DefaultValue(""), Category("Appearance")]
        public string LastPage
        {
            get
            {
                return this.basePagerClass.LastPage;
            }
            set
            {
                this.basePagerClass.LastPage = value;
            }
        }

        [DefaultValue(""), Bindable(true), Category("Appearance")]
        public bool ListType
        {
            get
            {
                return this.basePagerClass.ListType;
            }
            set
            {
                this.basePagerClass.ListType = value;
            }
        }

        [DefaultValue(""), Category("Appearance"), Bindable(true)]
        public string NextPage
        {
            get
            {
                return this.basePagerClass.NextPage;
            }
            set
            {
                this.basePagerClass.NextPage = value;
            }
        }

        [Category("Appearance"), Bindable(true), DefaultValue("")]
        public bool NumType
        {
            get
            {
                return this.basePagerClass.NumType;
            }
            set
            {
                this.basePagerClass.NumType = value;
            }
        }

        public int PageCount
        {
            get
            {
                return this.basePagerClass.PageCount;
            }
        }

        [Category("Appearance"), Bindable(true), DefaultValue("")]
        public int PageSize
        {
            get
            {
                return this.basePagerClass.PageSize;
            }
            set
            {
                this.basePagerClass.PageSize = value;
            }
        }

        [Category("Appearance"), DefaultValue(""), Bindable(true)]
        public int PageStep
        {
            get
            {
                return this.basePagerClass.PageStep;
            }
            set
            {
                this.basePagerClass.PageStep = value;
            }
        }

        [Category("Appearance"), DefaultValue(""), Bindable(true)]
        public bool PrenextType
        {
            get
            {
                return this.basePagerClass.PrenextType;
            }
            set
            {
                this.basePagerClass.PrenextType = value;
            }
        }

        [DefaultValue(""), Category("Appearance"), Bindable(true)]
        public string PreviewPage
        {
            get
            {
                return this.basePagerClass.PreviewPage;
            }
            set
            {
                this.basePagerClass.PreviewPage = value;
            }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string URL
        {
            get
            {
                return this.basePagerClass.URL;
            }
            set
            {
                this.basePagerClass.URL = value;
            }
        }
    }


    [DefaultProperty(""), ToolboxData("<{0}:CommonPager runat=server></{0}:CommonPager>")]
    public class CommonPager : BasePager
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CommonPagerClass class2 = new CommonPagerClass();
            base.BasePagerClass = class2;
        }

        protected override void Render(HtmlTextWriter output)
        {
            output.Write(base.BasePagerClass.ShowPage());
        }
    }


    public abstract class BasePagerClass
    {
        private int count;
        private int currentPage = 1;
        private bool disCount = true;
        private int endPage;
        private string firstPage = "<<";
        private string lastPage = ">>";
        private bool listType = true;
        private string nextPage = ">";
        private bool numType = true;
        private int pageSize = 10;
        private int pageStep = 4;
        private bool prenextType = true;
        private string previewPage = "<";
        private int startPage;
        private string url = string.Empty;

        protected BasePagerClass()
        {
        }

        public void CountStartEndPage()
        {
            if (this.PageCount <= ((2 * this.pageStep) + 1))
            {
                this.startPage = 1;
                this.endPage = this.PageCount;
            }
            else
            {
                if (this.currentPage > this.pageStep)
                {
                    this.startPage = this.currentPage - this.pageStep;
                }
                else
                {
                    this.startPage = 1;
                }
                this.endPage = this.startPage + (2 * this.pageStep);
                if ((this.startPage + (2 * this.pageStep)) > this.PageCount)
                {
                    this.startPage = this.PageCount - (2 * this.pageStep);
                    this.endPage = this.PageCount;
                }
            }
        }

        public abstract string ShowPage();

        public int Count
        {
            get{ return this.count;}
            set{this.count = value;}
        }

        public int CurrentPage
        {
            get
            {
                return this.currentPage;
            }
            set
            {
                this.currentPage = value;
            }
        }

        public bool DisCount
        {
            get
            {
                return this.disCount;
            }
            set
            {
                this.disCount = value;
            }
        }

        public int EndPage
        {
            get
            {
                return this.endPage;
            }
        }

        public string FirstPage
        {
            get
            {
                return this.firstPage;
            }
            set
            {
                this.firstPage = value;
            }
        }

        public string LastPage
        {
            get
            {
                return this.lastPage;
            }
            set
            {
                this.lastPage = value;
            }
        }

        public bool ListType
        {
            get
            {
                return this.listType;
            }
            set
            {
                this.listType = value;
            }
        }

        public string NextPage
        {
            get
            {
                return this.nextPage;
            }
            set
            {
                this.nextPage = value;
            }
        }

        public bool NumType
        {
            get
            {
                return this.numType;
            }
            set
            {
                this.numType = value;
            }
        }

        public int PageCount
        {
            get
            {
                return (int)Math.Ceiling((decimal)this.Count / this.PageSize);
            }
        }

        public int PageSize
        {
            get
            {
                return this.pageSize;
            }
            set
            {
                this.pageSize = value;
            }
        }

        public int PageStep
        {
            get
            {
                return this.pageStep;
            }
            set
            {
                this.pageStep = value;
            }
        }

        public bool PrenextType
        {
            get
            {
                return this.prenextType;
            }
            set
            {
                this.prenextType = value;
            }
        }

        public string PreviewPage
        {
            get
            {
                return this.previewPage;
            }
            set
            {
                this.previewPage = value;
            }
        }

        public int StartPage
        {
            get
            {
                return this.startPage;
            }
        }

        public string URL
        {
            get
            {
                if (this.url != string.Empty)
                {
                    return this.url;
                }
                string rawUrl = HttpContext.Current.Request.RawUrl;
                if (rawUrl.ToLower().IndexOf("&page=") > -1)
                {
                    return (rawUrl.Substring(0, rawUrl.ToLower().IndexOf("&page=")) + "&Page=$Page");
                }
                if (rawUrl.ToLower().IndexOf("?page=") > -1)
                {
                    return (rawUrl.Substring(0, rawUrl.ToLower().IndexOf("?page=")) + "?Page=$Page");
                }
                if (rawUrl.ToLower().IndexOf("?") > -1)
                {
                    if (rawUrl.EndsWith("?"))
                    {
                        return (rawUrl + "Page=$Page");
                    }
                    return (rawUrl + "&Page=$Page");
                }
                return (rawUrl + "?Page=$Page");
            }
            set
            {
                this.url = value;
            }
        }
    }

    public class CommonPagerClass : BasePagerClass
    {
        public override string ShowPage()
        {
            StringBuilder builder = new StringBuilder("");
            if (base.Count > 0)
            {
                int num;
                string[] strArray;
                int num2;
                builder.Append("<div class=\"pageCss\">");
                if (base.DisCount)
                {
                    builder.Append(string.Concat(new object[] { "<ul class=\"disCount\"><li>共有", base.Count, "条</li><li>当前", base.CurrentPage, "/", base.PageCount, "页</li></ul>" }));
                }
                if (base.PrenextType)
                {
                    builder.Append("<ul class=\"prenextType\">");
                    if (base.CurrentPage > 1)
                    {
                        builder.Append("<li><a href=" + base.URL.Replace("$Page", "1") + ">" + base.FirstPage + "</a></li>");
                    }
                    else
                    {
                        builder.Append("<li>" + base.FirstPage + "</li>");
                    }
                    if ((base.CurrentPage - 1) > 0)
                    {
                        strArray = new string[5];
                        strArray[0] = "<li><a href=";
                        num2 = base.CurrentPage - 1;
                        strArray[1] = base.URL.Replace("$Page", num2.ToString());
                        strArray[2] = ">";
                        strArray[3] = base.PreviewPage;
                        strArray[4] = "</a></li>";
                        builder.Append(string.Concat(strArray));
                    }
                    else
                    {
                        builder.Append("<li>" + base.PreviewPage + "</li>");
                    }
                    builder.Append("</ul>");
                }
                if (base.NumType)
                {
                    base.CountStartEndPage();
                    builder.Append("<ul class=\"numType\">");
                    for (num = base.StartPage; num <= base.EndPage; num++)
                    {
                        if (base.CurrentPage != num)
                        {
                            builder.Append(string.Concat(new object[] { "<li><a href=", base.URL.Replace("$Page", num.ToString()), ">", num, "</a></li>" }));
                        }
                        else
                        {
                            builder.Append("<li id=\"currentPage\">" + num + "</li>");
                        }
                    }
                    builder.Append("</ul>");
                }
                if (base.PrenextType)
                {
                    builder.Append("<ul class=\"prenextType\">");
                    if ((base.CurrentPage + 1) <= base.PageCount)
                    {
                        strArray = new string[5];
                        strArray[0] = "<li><a href=";
                        num2 = base.CurrentPage + 1;
                        strArray[1] = base.URL.Replace("$Page", num2.ToString());
                        strArray[2] = ">";
                        strArray[3] = base.NextPage;
                        strArray[4] = "</a></li>";
                        builder.Append(string.Concat(strArray));
                    }
                    else
                    {
                        builder.Append("<li>" + base.NextPage + "</li>");
                    }
                    if (base.CurrentPage < base.PageCount)
                    {
                        builder.Append("<li><a href=" + base.URL.Replace("$Page", base.PageCount.ToString()) + ">" + base.LastPage + "</a></li>");
                    }
                    else
                    {
                        builder.Append("<li>" + base.LastPage + "</li>");
                    }
                    builder.Append("</ul>");
                }
                if (base.ListType)
                {
                    builder.Append("<ul class=\"listType\">");
                    //builder.Append("<li>");
                    //builder.Append("<select name=select onchange=\"window.location.href=this.value\">");
                    //for (num = 1; num <= base.PageCount; num++)
                    //{
                    //    if (num == base.CurrentPage)
                    //    {
                    //        builder.Append(string.Concat(new object[] { "<option value=", base.URL.Replace("$Page", num.ToString()), " selected=selected>", num, "</option>" }));
                    //    }
                    //    else
                    //    {
                    //        builder.Append(string.Concat(new object[] { "<option value=", base.URL.Replace("$Page", num.ToString()), ">", num, "</option>" }));
                    //    }
                    //}
                    //builder.Append("</select></li>");

                    builder.Append("<span>&nbsp;跳转</span>");
                    builder.Append("<li>");
                    builder.Append("<input type=\"number\" min=\"1\" id=\"page\" name=\"page\" class=\"page\">");
                    builder.Append("</li>");
                    builder.Append("<span>页</span>");
                    builder.Append("<input type=\"submit\" id=\"confirm\" name=\"confirm\" class=\"confirm\" onclick=\"window.location.href=('"+base.URL+"').replace('$Page',document.getElementById('page').value)\" value=\"确认\"> ");
                    builder.Append("</ul>");
                }
                builder.Append("</div>");
            }
            return builder.ToString();
        }
    }
}
