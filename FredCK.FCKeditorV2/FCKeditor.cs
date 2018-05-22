using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;
using System.Configuration;

namespace FredCK.FCKeditorV2
{
    [ValidationProperty("Value"), ParseChildren(false), Designer("FredCK.FCKeditorV2.FCKeditorDesigner"), DefaultProperty("Value"), ToolboxData("<{0}:FCKeditor runat=server></{0}:FCKeditor>")]
    public class FCKeditor : Control, IPostBackDataHandler
    {
        private bool _IsCompatible;

        public bool CheckBrowserCompatibility()
        {
            return IsCompatibleBrowser();
        }

        public string CreateHtml()
        {
            StringWriter writer = new StringWriter();
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);
            this.Render(writer2);
            return writer.ToString();
        }

        public static bool IsCompatibleBrowser()
        {
            return IsCompatibleBrowser(HttpContext.Current.Request);
        }

        public static bool IsCompatibleBrowser(HttpRequest request)
        {
            Match match;
            HttpBrowserCapabilities browser = request.Browser;
            if (((browser.Browser == "IE") && ((browser.MajorVersion >= 6) || ((browser.MajorVersion == 5) && (browser.MinorVersion >= 0.5)))) && browser.Win32)
            {
                return true;
            }
            string userAgent = request.UserAgent;
            if (userAgent.IndexOf("Gecko/") >= 0)
            {
                match = Regex.Match(request.UserAgent, @"(?<=Gecko/)\d{8}");
                return (match.Success && (int.Parse(match.Value, CultureInfo.InvariantCulture) >= 0x131a302));
            }
            if (userAgent.IndexOf("Opera/") >= 0)
            {
                match = Regex.Match(request.UserAgent, @"(?<=Opera/)[\d\.]+");
                return (match.Success && (float.Parse(match.Value, CultureInfo.InvariantCulture) >= 9.5));
            }
            if (userAgent.IndexOf("AppleWebKit/") >= 0)
            {
                match = Regex.Match(request.UserAgent, @"(?<=AppleWebKit/)\d+");
                return (match.Success && (int.Parse(match.Value, CultureInfo.InvariantCulture) >= 0x20a));
            }
            return false;
        }

        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string str = postCollection[postDataKey];
            if (this.Config["HtmlEncodeOutput"] != "false")
            {
                str = str.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
            }
            if (str != this.Value)
            {
                this.Value = str;
                return true;
            }
            return false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this._IsCompatible = this.CheckBrowserCompatibility();
            if (this._IsCompatible)
            {
                object obj2 = null;
                for (Control control = this.Parent; control != null; control = control.Parent)
                {
                    foreach (object obj3 in control.Controls)
                    {
                        if (obj3.GetType().FullName == "System.Web.UI.ScriptManager")
                        {
                            obj2 = obj3;
                            break;
                        }
                    }
                    if (obj2 != null)
                    {
                        break;
                    }
                }
                if (obj2 != null)
                {
                    try
                    {
                        if ((bool)obj2.GetType().GetProperty("SupportsPartialRendering").GetValue(obj2, null))
                        {
                            string str = "(function()\n{\n\tvar editor = FCKeditorAPI.GetInstance('" + this.ClientID + "');\n\tif (editor)\n\t\teditor.UpdateLinkedField();\n})();\n";
                            obj2.GetType().GetMethod("RegisterOnSubmitStatement", new Type[] { typeof(Control), typeof(Type), typeof(string), typeof(string) }).Invoke(obj2, new object[] { this, base.GetType(), "FCKeditorAjaxOnSubmit_" + this.ClientID, str });
                            this.Config["PreventSubmitHandler"] = "true";
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void RaisePostDataChangedEvent()
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<div>");
            if (this._IsCompatible)
            {
                string basePath = this.BasePath;
                if (basePath.StartsWith("~"))
                {
                    basePath = base.ResolveUrl(basePath);
                }
                string str2 = (HttpContext.Current.Request.QueryString["fcksource"] == "true") ? "fckeditor.original.html" : "fckeditor.html";
                string str3 = basePath;
                basePath = str3 + "editor/" + str2 + "?InstanceName=" + this.ClientID;
                if (this.ToolbarSet.Length > 0)
                {
                    basePath = basePath + "&amp;Toolbar=" + this.ToolbarSet;
                }
                writer.Write("<input type=\"hidden\" id=\"{0}\" name=\"{1}\" value=\"{2}\" />", this.ClientID, this.UniqueID, HttpUtility.HtmlEncode(this.Value));
                writer.Write("<input type=\"hidden\" id=\"{0}___Config\" value=\"{1}\" />", this.ClientID, this.Config.GetHiddenFieldString());
                writer.Write("<iframe id=\"{0}___Frame\" src=\"{1}\" width=\"{2}\" height=\"{3}\" frameborder=\"no\" scrolling=\"no\"></iframe>", new object[] { this.ClientID, basePath, this.Width, this.Height });
            }
            else
            {
                writer.Write("<textarea name=\"{0}\" rows=\"4\" cols=\"40\" style=\"width: {1}; height: {2}\" wrap=\"virtual\">{3}</textarea>", new object[] { this.UniqueID, this.Width, this.Height, HttpUtility.HtmlEncode(this.Value) });
            }
            writer.Write("</div>");
        }

        // Properties
        [Category("Configurations")]
        public bool AutoDetectLanguage
        {
            set
            {
                this.Config["AutoDetectLanguage"] = value ? "true" : "false";
            }
        }

        [Category("Configurations")]
        public string BaseHref
        {
            set
            {
                this.Config["BaseHref"] = value;
            }
        }

        [DefaultValue("/fckeditor/")]
        public string BasePath
        {
            get
            {
                object obj2 = this.ViewState["BasePath"];
                if (obj2 == null)
                {
                    obj2 = ConfigurationSettings.AppSettings["FCKeditor:BasePath"];
                }
                return ((obj2 == null) ? "/fckeditor/" : ((string)obj2));
            }
            set
            {
                this.ViewState["BasePath"] = value;
            }
        }

        [Browsable(false)]
        public FCKeditorConfigurations Config
        {
            get
            {
                if (this.ViewState["Config"] == null)
                {
                    this.ViewState["Config"] = new FCKeditorConfigurations();
                }
                return (FCKeditorConfigurations)this.ViewState["Config"];
            }
        }

        [Category("Configurations")]
        public LanguageDirection ContentLangDirection
        {
            set
            {
                this.Config["ContentLangDirection"] = (value == LanguageDirection.LeftToRight) ? "ltr" : "rtl";
            }
        }

        [Category("Configurations")]
        public string CustomConfigurationsPath
        {
            set
            {
                this.Config["CustomConfigurationsPath"] = value;
            }
        }

        [Category("Configurations")]
        public bool Debug
        {
            set
            {
                this.Config["Debug"] = value ? "true" : "false";
            }
        }

        [Category("Configurations")]
        public string DefaultLanguage
        {
            set
            {
                this.Config["DefaultLanguage"] = value;
            }
        }

        [Category("Configurations")]
        public string EditorAreaCSS
        {
            set
            {
                this.Config["EditorAreaCSS"] = value;
            }
        }

        [Category("Configurations")]
        public bool EnableSourceXHTML
        {
            set
            {
                this.Config["EnableSourceXHTML"] = value ? "true" : "false";
            }
        }

        [Category("Configurations")]
        public bool EnableXHTML
        {
            set
            {
                this.Config["EnableXHTML"] = value ? "true" : "false";
            }
        }

        [Category("Configurations")]
        public bool FillEmptyBlocks
        {
            set
            {
                this.Config["FillEmptyBlocks"] = value ? "true" : "false";
            }
        }

        [Category("Configurations")]
        public string FontColors
        {
            set
            {
                this.Config["FontColors"] = value;
            }
        }

        [Category("Configurations")]
        public string FontFormats
        {
            set
            {
                this.Config["FontFormats"] = value;
            }
        }

        [Category("Configurations")]
        public string FontNames
        {
            set
            {
                this.Config["FontNames"] = value;
            }
        }

        [Category("Configurations")]
        public string FontSizes
        {
            set
            {
                this.Config["FontSizes"] = value;
            }
        }

        [Category("Configurations")]
        public bool ForcePasteAsPlainText
        {
            set
            {
                this.Config["ForcePasteAsPlainText"] = value ? "true" : "false";
            }
        }

        [Category("Configurations")]
        public bool ForceSimpleAmpersand
        {
            set
            {
                this.Config["ForceSimpleAmpersand"] = value ? "true" : "false";
            }
        }

        [Category("Configurations")]
        public string FormatIndentator
        {
            set
            {
                this.Config["FormatIndentator"] = value;
            }
        }

        [Category("Configurations")]
        public bool FormatOutput
        {
            set
            {
                this.Config["FormatOutput"] = value ? "true" : "false";
            }
        }

        [Category("Configurations")]
        public bool FormatSource
        {
            set
            {
                this.Config["FormatSource"] = value ? "true" : "false";
            }
        }

        [Category("Configurations")]
        public bool FullPage
        {
            set
            {
                this.Config["FullPage"] = value ? "true" : "false";
            }
        }

        [Category("Configurations")]
        public bool GeckoUseSPAN
        {
            set
            {
                this.Config["GeckoUseSPAN"] = value ? "true" : "false";
            }
        }

        [DefaultValue("200px"), Category("Appearence")]
        public Unit Height
        {
            get
            {
                object obj2 = this.ViewState["Height"];
                return ((obj2 == null) ? Unit.Pixel(200) : ((Unit)obj2));
            }
            set
            {
                this.ViewState["Height"] = value;
            }
        }

        [Category("Configurations")]
        public bool HtmlEncodeOutput
        {
            set
            {
                this.Config["HtmlEncodeOutput"] = value ? "true" : "false";
            }
        }

        [Category("Configurations")]
        public string ImageBrowserURL
        {
            set
            {
                this.Config["ImageBrowserURL"] = value;
            }
        }

        [Category("Configurations")]
        public string LinkBrowserURL
        {
            set
            {
                this.Config["LinkBrowserURL"] = value;
            }
        }

        [Category("Configurations")]
        public string PluginsPath
        {
            set
            {
                this.Config["PluginsPath"] = value;
            }
        }

        [Category("Configurations")]
        public string SkinPath
        {
            set
            {
                this.Config["SkinPath"] = value;
            }
        }

        [Category("Configurations")]
        public bool StartupFocus
        {
            set
            {
                this.Config["StartupFocus"] = value ? "true" : "false";
            }
        }

        [Category("Configurations")]
        public string StylesXmlPath
        {
            set
            {
                this.Config["StylesXmlPath"] = value;
            }
        }

        [Category("Configurations")]
        public int TabSpaces
        {
            set
            {
                this.Config["TabSpaces"] = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        [Category("Configurations")]
        public bool ToolbarCanCollapse
        {
            set
            {
                this.Config["ToolbarCanCollapse"] = value ? "true" : "false";
            }
        }

        [DefaultValue("Default")]
        public string ToolbarSet
        {
            get
            {
                object obj2 = this.ViewState["ToolbarSet"];
                return ((obj2 == null) ? "Default" : ((string)obj2));
            }
            set
            {
                this.ViewState["ToolbarSet"] = value;
            }
        }

        [Category("Configurations")]
        public bool ToolbarStartExpanded
        {
            set
            {
                this.Config["ToolbarStartExpanded"] = value ? "true" : "false";
            }
        }

        [Category("Configurations")]
        public bool UseBROnCarriageReturn
        {
            set
            {
                this.Config["UseBROnCarriageReturn"] = value ? "true" : "false";
            }
        }

        [DefaultValue("")]
        public string Value
        {
            get
            {
                object obj2 = this.ViewState["Value"];
                return ((obj2 == null) ? "" : ((string)obj2));
            }
            set
            {
                this.ViewState["Value"] = value;
            }
        }

        [Category("Appearence"), DefaultValue("100%")]
        public Unit Width
        {
            get
            {
                object obj2 = this.ViewState["Width"];
                return ((obj2 == null) ? Unit.Percentage(100.0) : ((Unit)obj2));
            }
            set
            {
                this.ViewState["Width"] = value;
            }
        }
    }
}
