using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using XueFuShop.Common;
using XueFu.EntLib;

namespace FredCK.FCKeditorV2.FileBrowser
{
    public abstract class FileWorkerBase : Page
    {
        public Config Config;

        protected FileWorkerBase()
        {
        }

        private bool CheckNonHtmlFile(HttpPostedFile file)
        {
            byte[] buffer = new byte[0x400];
            file.InputStream.Read(buffer, 0, 0x400);
            string input = Encoding.ASCII.GetString(buffer);
            if (Regex.IsMatch(input, @"<!DOCTYPE\W*X?HTML", RegexOptions.Singleline | RegexOptions.IgnoreCase))
            {
                return false;
            }
            if (Regex.IsMatch(input, "<(?:body|head|html|img|pre|script|table|title)", RegexOptions.Singleline | RegexOptions.IgnoreCase))
            {
                return false;
            }
            if (Regex.IsMatch(input, "type\\s*=\\s*[\\'\"]?\\s*(?:\\w*/)?(?:ecma|java)", RegexOptions.Singleline | RegexOptions.IgnoreCase))
            {
                return false;
            }
            if (Regex.IsMatch(input, "(?:href|src|data)\\s*=\\s*[\\'\"]?\\s*(?:ecma|java)script:", RegexOptions.Singleline | RegexOptions.IgnoreCase))
            {
                return false;
            }
            if (Regex.IsMatch(input, "url\\s*\\(\\s*[\\'\"]?\\s*(?:ecma|java)script:", RegexOptions.Singleline | RegexOptions.IgnoreCase))
            {
                return false;
            }
            return true;
        }

        protected void FileUpload(string resourceType, string currentFolder, bool isQuickUpload)
        {
            HttpPostedFile file = base.Request.Files["NewFile"];
            string fileName = "";
            if (file == null)
            {
                this.SendFileUploadResponse(0xca, isQuickUpload);
            }
            else
            {
                string str2 = this.ServerMapFolder(resourceType, currentFolder, isQuickUpload);
                fileName = Path.GetFileName(file.FileName);
                fileName = this.SanitizeFileName(fileName);
                string extension = Path.GetExtension(file.FileName).TrimStart(new char[] { '.' });
                if (!this.Config.TypeConfig[resourceType].CheckIsAllowedExtension(extension))
                {
                    this.SendFileUploadResponse(0xca, isQuickUpload);
                }
                else if (!(!this.Config.CheckIsNonHtmlExtension(extension) || this.CheckNonHtmlFile(file)))
                {
                    this.SendFileUploadResponse(0xca, isQuickUpload);
                }
                else
                {
                    int errorNumber = 0;
                    int num2 = 0;
                    while (true)
                    {
                        string path = Path.Combine(str2, fileName);
                        if (File.Exists(path))
                        {
                            num2++;
                            fileName = string.Concat(new object[] { Path.GetFileNameWithoutExtension(file.FileName), "(", num2, ").", extension });
                            errorNumber = 0xc9;
                        }
                        else
                        {
                            file.SaveAs(path);
                            int waterType = ShopConfig.ReadConfigInfo().WaterType;
                            int waterPossition = ShopConfig.ReadConfigInfo().WaterPossition;
                            string text = ShopConfig.ReadConfigInfo().Text;
                            string textFont = ShopConfig.ReadConfigInfo().TextFont;
                            int textSize = ShopConfig.ReadConfigInfo().TextSize;
                            string textColor = ShopConfig.ReadConfigInfo().TextColor;
                            string waterImage = base.Server.MapPath(ShopConfig.ReadConfigInfo().WaterPhoto);
                            switch (waterType)
                            {
                                case 2:
                                case 3:
                                    {
                                        fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.ToString("yyMMddhhmmss") + "." + extension;
                                        string newImage = Path.Combine(str2, fileName);
                                        if (waterType == 2)
                                        {
                                            ImageHelper.AddTextWater(path, newImage, waterPossition, text, textFont, textColor, textSize);
                                        }
                                        else
                                        {
                                            ImageHelper.AddImageWater(path, newImage, waterPossition, waterImage);
                                        }
                                        if (File.Exists(path))
                                        {
                                            File.Delete(path);
                                        }
                                        break;
                                    }
                            }
                            TypeConfig config = this.Config.TypeConfig[resourceType];
                            string fileUrl = isQuickUpload ? config.GetQuickUploadPath() : config.GetFilesPath();
                            fileUrl = fileUrl + fileName;
                            this.SendFileUploadResponse(errorNumber, isQuickUpload, fileUrl, fileName);
                            return;
                        }
                    }
                }
            }
        }

        private string SanitizeFileName(string fileName)
        {
            if (this.Config.ForceSingleExtension)
            {
                fileName = Regex.Replace(fileName, @"\.(?![^.]*$)", "_", RegexOptions.None);
            }
            return Regex.Replace(fileName, "[\\\\/|:?*\"<>\\p{C}]", "_", RegexOptions.None);
        }

        protected string SanitizeFolderName(string folderName)
        {
            return Regex.Replace(folderName, "[.\\\\/|:?*\"<>\\p{C}]", "_", RegexOptions.None);
        }

        private void SendFileUploadResponse(int errorNumber, bool isQuickUpload)
        {
            this.SendFileUploadResponse(errorNumber, isQuickUpload, "", "", "");
        }

        private void SendFileUploadResponse(int errorNumber, bool isQuickUpload, string fileUrl, string fileName)
        {
            this.SendFileUploadResponse(errorNumber, isQuickUpload, fileUrl, fileName, "");
        }

        protected void SendFileUploadResponse(int errorNumber, bool isQuickUpload, string fileUrl, string fileName, string customMsg)
        {
            base.Response.Clear();
            base.Response.Write("<script type=\"text/javascript\">");
            //base.Response.Write(@"(function(){var d=document.domain;while (true){try{var A=window.top.opener.document.domain;break;}catch(e) {};d=d.replace(/.*?(?:\.|$)/,'');if (d.length==0) break;try{document.domain=d;}catch (e){break;}}})();");
            if (isQuickUpload)
            {
                base.Response.Write(string.Concat(new object[] { "window.parent.OnUploadCompleted(", errorNumber, ",'", fileUrl.Replace("'", @"\'"), "','", fileName.Replace("'", @"\'"), "','", customMsg.Replace("'", @"\'"), "') ;" }));
            }
            else
            {
                base.Response.Write(string.Concat(new object[] { "window.parent.frames['frmUpload'].OnUploadCompleted(", errorNumber, ",'", fileName.Replace("'", @"\'"), "') ;" }));
            }
            base.Response.Write("</script>");
            base.Response.End();
        }

        protected string ServerMapFolder(string resourceType, string folderPath, bool isQuickUpload)
        {
            TypeConfig config = this.Config.TypeConfig[resourceType];
            string path = isQuickUpload ? config.GetQuickUploadDirectory() : config.GetFilesDirectory();
            Util.CreateDirectory(path);
            return Path.Combine(path, folderPath.TrimStart(new char[] { '/' }));
        }
    }
}
