using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Collections;
using System.Configuration;

namespace FredCK.FCKeditorV2.FileBrowser
{
    public class Config : UserControl
    {
        public string[] AllowedTypes;
        private const string DEFAULT_USER_FILES_PATH = "/userfiles/";
        public bool Enabled;
        public bool ForceSingleExtension;
        public string[] HtmlExtensions;
        private string sUserFilesDirectory;
        public TypeConfigList TypeConfig;
        public string UserFilesAbsolutePath;
        public string UserFilesPath;

        internal bool CheckIsNonHtmlExtension(string extension)
        {
            return ((this.HtmlExtensions.Length == 0) || !Util.ArrayContains(this.HtmlExtensions, extension, CaseInsensitiveComparer.DefaultInvariant));
        }

        internal bool CheckIsTypeAllowed(string typeName)
        {
            return (Array.IndexOf<string>(this.AllowedTypes, typeName) >= 0);
        }

        private void DefaultSettings()
        {
            this.Enabled = false;
            this.UserFilesPath = "/userfiles/";
            this.UserFilesAbsolutePath = "";
            this.ForceSingleExtension = true;
            this.AllowedTypes = new string[] { "File", "Image", "Flash", "Media" };
            this.HtmlExtensions = new string[] { "html", "htm", "xml", "xsd", "txt", "js" };
            this.TypeConfig = new TypeConfigList((FileWorkerBase)this.Page);
            this.TypeConfig["File"].AllowedExtensions = new string[] { 
            "7z", "aiff", "asf", "avi", "bmp", "csv", "doc", "fla", "flv", "gif", "gz", "gzip", "jpeg", "jpg", "mid", "mov", 
            "mp3", "mp4", "mpc", "mpeg", "mpg", "ods", "odt", "pdf", "png", "ppt", "pxd", "qt", "ram", "rar", "rm", "rmi", 
            "rmvb", "rtf", "sdc", "sitd", "swf", "sxc", "sxw", "tar", "tgz", "tif", "tiff", "txt", "vsd", "wav", "wma", "wmv", 
            "xls", "xml", "zip"
         };
            this.TypeConfig["File"].DeniedExtensions = new string[0];
            this.TypeConfig["File"].FilesPath = "%UserFilesPath%file/";
            this.TypeConfig["File"].FilesAbsolutePath = (this.UserFilesAbsolutePath == "") ? "" : "%UserFilesAbsolutePath%file/";
            this.TypeConfig["File"].QuickUploadPath = "%UserFilesPath%";
            this.TypeConfig["File"].QuickUploadAbsolutePath = "%UserFilesAbsolutePath%";
            this.TypeConfig["Image"].AllowedExtensions = new string[] { "bmp", "gif", "jpeg", "jpg", "png" };
            this.TypeConfig["Image"].DeniedExtensions = new string[0];
            this.TypeConfig["Image"].FilesPath = "%UserFilesPath%image/";
            this.TypeConfig["Image"].FilesAbsolutePath = (this.UserFilesAbsolutePath == "") ? "" : "%UserFilesAbsolutePath%image/";
            this.TypeConfig["Image"].QuickUploadPath = "%UserFilesPath%";
            this.TypeConfig["Image"].QuickUploadAbsolutePath = "%UserFilesAbsolutePath%";
            this.TypeConfig["Flash"].AllowedExtensions = new string[] { "swf", "flv" };
            this.TypeConfig["Flash"].DeniedExtensions = new string[0];
            this.TypeConfig["Flash"].FilesPath = "%UserFilesPath%flash/";
            this.TypeConfig["Flash"].FilesAbsolutePath = (this.UserFilesAbsolutePath == "") ? "" : "%UserFilesAbsolutePath%flash/";
            this.TypeConfig["Flash"].QuickUploadPath = "%UserFilesPath%";
            this.TypeConfig["Flash"].QuickUploadAbsolutePath = "%UserFilesAbsolutePath%";
            this.TypeConfig["Media"].AllowedExtensions = new string[] { 
            "aiff", "asf", "avi", "bmp", "fla", "flv", "gif", "jpeg", "jpg", "mid", "mov", "mp3", "mp4", "mpc", "mpeg", "mpg", 
            "png", "qt", "ram", "rm", "rmi", "rmvb", "swf", "tif", "tiff", "wav", "wma", "wmv"
         };
            this.TypeConfig["Media"].DeniedExtensions = new string[0];
            this.TypeConfig["Media"].FilesPath = "%UserFilesPath%media/";
            this.TypeConfig["Media"].FilesAbsolutePath = (this.UserFilesAbsolutePath == "") ? "" : "%UserFilesAbsolutePath%media/";
            this.TypeConfig["Media"].QuickUploadPath = "%UserFilesPath%";
            this.TypeConfig["Media"].QuickUploadAbsolutePath = "%UserFilesAbsolutePath%";
        }

        internal void LoadConfig()
        {
            this.DefaultSettings();
            this.SetConfig();
            string relativeUrl = base.Session["FCKeditor:UserFilesPath"] as string;
            if ((relativeUrl == null) || (relativeUrl.Length == 0))
            {
                relativeUrl = base.Application["FCKeditor:UserFilesPath"] as string;
            }
            if ((relativeUrl == null) || (relativeUrl.Length == 0))
            {
                relativeUrl = ConfigurationSettings.AppSettings["FCKeditor:UserFilesPath"];
            }
            if ((relativeUrl == null) || (relativeUrl.Length == 0))
            {
                relativeUrl = this.UserFilesPath;
            }
            if ((relativeUrl == null) || (relativeUrl.Length == 0))
            {
                relativeUrl = "/userfiles/";
            }
            if (!relativeUrl.EndsWith("/"))
            {
                relativeUrl = relativeUrl + "/";
            }
            relativeUrl = base.ResolveUrl(relativeUrl);
            this.UserFilesPath = relativeUrl;
        }

        public virtual void SetConfig()
        {
        }

        // Properties
        internal string UserFilesDirectory
        {
            get
            {
                if (this.sUserFilesDirectory == null)
                {
                    if (this.UserFilesAbsolutePath.Length > 0)
                    {
                        this.sUserFilesDirectory = this.UserFilesAbsolutePath;
                        this.sUserFilesDirectory = this.sUserFilesDirectory.TrimEnd(new char[] { '\\', '/' }) + '/';
                    }
                    else
                    {
                        this.sUserFilesDirectory = base.Server.MapPath(this.UserFilesPath);
                    }
                }
                return this.sUserFilesDirectory;
            }
        }
    }
}
