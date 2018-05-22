using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web;

namespace FredCK.FCKeditorV2.FileBrowser
{
    public class TypeConfig
    {
        private FileWorkerBase _FileWorker;
        private string _QuickUploadDirectory;
        private string _QuickUploadPath;
        private string _UserFilesDirectory;
        private string _UserFilesPath;
        public string[] AllowedExtensions;
        public string[] DeniedExtensions;
        public string FilesAbsolutePath;
        public string FilesPath;
        public string QuickUploadAbsolutePath;
        public string QuickUploadPath;

        public TypeConfig(FileWorkerBase fileWorker)
        {
            this._FileWorker = fileWorker;
            this.AllowedExtensions = new string[0];
            this.DeniedExtensions = new string[0];
            this.FilesPath = "";
            this.FilesAbsolutePath = "";
            this.QuickUploadPath = "";
            this.QuickUploadAbsolutePath = "";
        }

        internal bool CheckIsAllowedExtension(string extension)
        {
            if ((this.AllowedExtensions.Length == 0) && (this.DeniedExtensions.Length == 0))
            {
                return false;
            }
            if (!((this.DeniedExtensions.Length <= 0) || Util.ArrayContains(this.DeniedExtensions, extension, CaseInsensitiveComparer.DefaultInvariant)))
            {
                return false;
            }
            if (!((this.AllowedExtensions.Length <= 0) || Util.ArrayContains(this.AllowedExtensions, extension, CaseInsensitiveComparer.DefaultInvariant)))
            {
                return false;
            }
            return true;
        }

        internal string GetFilesDirectory()
        {
            if (this._UserFilesDirectory == null)
            {
                if (this.FilesAbsolutePath.Length == 0)
                {
                    this._UserFilesDirectory = HttpContext.Current.Server.MapPath(this.GetFilesPath());
                }
                else
                {
                    this._UserFilesDirectory = this.FilesAbsolutePath.Replace("%UserFilesAbsolutePath%", this.FileWorker.Config.UserFilesDirectory);
                }
            }
            return this._UserFilesDirectory;
        }

        internal string GetFilesPath()
        {
            if (this._UserFilesPath == null)
            {
                this._UserFilesPath = this.FilesPath.Replace("%UserFilesPath%", this.FileWorker.Config.UserFilesPath);
            }
            return this._UserFilesPath;
        }

        internal string GetQuickUploadDirectory()
        {
            if (this._QuickUploadDirectory == null)
            {
                if (this.QuickUploadAbsolutePath.Length == 0)
                {
                    this._QuickUploadDirectory = HttpContext.Current.Server.MapPath(this.GetQuickUploadPath());
                }
                else
                {
                    this._QuickUploadDirectory = this.QuickUploadAbsolutePath.Replace("%UserFilesAbsolutePath%", this.FileWorker.Config.UserFilesDirectory);
                }
            }
            return this._QuickUploadDirectory;
        }

        internal string GetQuickUploadPath()
        {
            if (this._QuickUploadPath == null)
            {
                this._QuickUploadPath = this.QuickUploadPath.Replace("%UserFilesPath%", this.FileWorker.Config.UserFilesPath);
            }
            return this._QuickUploadPath;
        }

        // Properties
        private FileWorkerBase FileWorker
        {
            get
            {
                return this._FileWorker;
            }
        }
    }
}
