using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace XueFu.EntLib
{
    public class UploadHelper
    {
        private string fileExtension;
        private FileNameType fileNameType = FileNameType.Date;
        private string fileType = string.Empty;
        private long localFileLength;
        private string localFileName;
        private string localFilePath;
        private string path = string.Empty;
        private HttpPostedFile postedFile;
        private string saveFileFolderPath;
        private string saveFileFullPath;
        private string saveFileName;
        private int sizes = 0x7d0;

        private string GetSaveFileFolderPath()
        {
            string path = string.Empty;
            path = ServerHelper.MapPath(this.path);
            DirectoryInfo info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                info.Create();
            }
            return path;
        }

        public FileInfo SaveAs()
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            FileInfo info = null;
            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    this.postedFile = files[i];
                    this.localFilePath = this.postedFile.FileName;
                    if ((this.localFilePath == null) || (this.localFilePath == ""))
                    {
                        throw new Exception("�����ϴ����ļ�");
                    }
                    this.localFileLength = this.postedFile.ContentLength;
                    if (this.localFileLength >= (this.sizes * 0x400))
                    {
                        throw new Exception("�ϴ���ͼƬ���ܴ���:" + this.sizes + "KB");
                    }
                    this.saveFileFolderPath = this.GetSaveFileFolderPath();
                    this.localFileName = System.IO.Path.GetFileName(this.postedFile.FileName);
                    this.fileExtension = FileHelper.GetFileExtension(this.localFileName);
                    if (this.fileType.ToLower().IndexOf(this.fileExtension) == -1)
                    {
                        throw new Exception("Ŀǰ��ϵͳ֧�ֵĸ�ʽΪ:" + this.fileType);
                    }
                    this.saveFileName = FileHelper.CreateFileName(this.fileNameType, this.localFileName, this.fileExtension);
                    this.saveFileFullPath = this.saveFileFolderPath + this.saveFileName;
                    this.postedFile.SaveAs(this.saveFileFullPath);
                    info = new FileInfo(this.saveFileFolderPath + this.saveFileName);
                }
            }
            catch
            {
                throw;
            }
            return info;
        }

        public FileNameType FileNameType
        {
            get
            {
                return this.fileNameType;
            }
            set
            {
                this.fileNameType = value;
            }
        }

        public string FileType
        {
            get
            {
                return this.fileType;
            }
            set
            {
                this.fileType = value;
            }
        }

        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
            }
        }

        public int Sizes
        {
            get
            {
                return this.sizes;
            }
            set
            {
                this.sizes = value;
            }
        }
    }
}
