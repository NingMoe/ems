using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Security;

namespace FredCK.FCKeditorV2.FileBrowser
{
    public class Connector : FileWorkerBase
    {
        private void CreateFolder(XmlNode connectorNode, string resourceType, string currentFolder)
        {
            string attributeValue = "0";
            string folderName = base.Request.QueryString["NewFolderName"];
            folderName = base.SanitizeFolderName(folderName);
            if ((folderName == null) || (folderName.Length == 0))
            {
                attributeValue = "102";
            }
            else
            {
                string str3 = base.ServerMapFolder(resourceType, currentFolder, false);
                try
                {
                    Util.CreateDirectory(Path.Combine(str3, folderName));
                }
                catch (ArgumentException)
                {
                    attributeValue = "102";
                }
                catch (PathTooLongException)
                {
                    attributeValue = "102";
                }
                catch (IOException)
                {
                    attributeValue = "101";
                }
                catch (SecurityException)
                {
                    attributeValue = "103";
                }
                catch (Exception)
                {
                    attributeValue = "110";
                }
            }
            XmlUtil.SetAttribute(XmlUtil.AppendElement(connectorNode, "Error"), "number", attributeValue);
        }

        private void GetFiles(XmlNode connectorNode, string resourceType, string currentFolder)
        {
            string path = base.ServerMapFolder(resourceType, currentFolder, false);
            XmlNode node = XmlUtil.AppendElement(connectorNode, "Files");
            FileInfo[] files = new DirectoryInfo(path).GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                decimal num2 = Math.Round((decimal)(files[i].Length / 1024M));
                if ((num2 < 1M) && (files[i].Length != 0))
                {
                    num2 = 1M;
                }
                XmlNode node2 = XmlUtil.AppendElement(node, "File");
                XmlUtil.SetAttribute(node2, "name", files[i].Name);
                XmlUtil.SetAttribute(node2, "size", num2.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void GetFolders(XmlNode connectorNode, string resourceType, string currentFolder)
        {
            string path = base.ServerMapFolder(resourceType, currentFolder, false);
            XmlNode node = XmlUtil.AppendElement(connectorNode, "Folders");
            DirectoryInfo[] directories = new DirectoryInfo(path).GetDirectories();
            for (int i = 0; i < directories.Length; i++)
            {
                XmlUtil.SetAttribute(XmlUtil.AppendElement(node, "Folder"), "name", directories[i].Name);
            }
        }

        internal string GetUrlFromPath(string resourceType, string folderPath)
        {
            if ((resourceType == null) || (resourceType.Length == 0))
            {
                return (base.Config.UserFilesPath.TrimEnd(new char[] { '/' }) + folderPath);
            }
            return (base.Config.TypeConfig[resourceType].GetFilesPath().TrimEnd(new char[] { '/' }) + folderPath);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.Config.LoadConfig();
            if (!base.Config.Enabled)
            {
                XmlResponseHandler.SendError(base.Response, 1, "This connector is disabled. Please check the \"editor/filemanager/connectors/aspx/config.ascx\" file.");
            }
            else
            {
                string command = base.Request.QueryString["Command"];
                string typeName = base.Request.QueryString["Type"];
                string currentFolder = base.Request.QueryString["CurrentFolder"];
                if (((command == null) || (typeName == null)) || (currentFolder == null))
                {
                    XmlResponseHandler.SendError(base.Response, 1, "Invalid request.");
                }
                else if (!base.Config.CheckIsTypeAllowed(typeName))
                {
                    XmlResponseHandler.SendError(base.Response, 1, "Invalid resource type specified.");
                }
                else
                {
                    if (!currentFolder.EndsWith("/"))
                    {
                        currentFolder = currentFolder + "/";
                    }
                    if (!currentFolder.StartsWith("/"))
                    {
                        currentFolder = "/" + currentFolder;
                    }
                    if ((currentFolder.IndexOf("..") >= 0) || (currentFolder.IndexOf(@"\") >= 0))
                    {
                        XmlResponseHandler.SendError(base.Response, 0x66, "");
                    }
                    else if (command == "FileUpload")
                    {
                        base.FileUpload(typeName, currentFolder, false);
                    }
                    else
                    {
                        XmlResponseHandler handler = new XmlResponseHandler(this, base.Response);
                        XmlNode connectorNode = handler.CreateBaseXml(command, typeName, currentFolder);
                        string str4 = command;
                        if (str4 != null)
                        {
                            if (!(str4 == "GetFolders"))
                            {
                                if (str4 == "GetFoldersAndFiles")
                                {
                                    this.GetFolders(connectorNode, typeName, currentFolder);
                                    this.GetFiles(connectorNode, typeName, currentFolder);
                                }
                                else if (str4 == "CreateFolder")
                                {
                                    this.CreateFolder(connectorNode, typeName, currentFolder);
                                }
                            }
                            else
                            {
                                this.GetFolders(connectorNode, typeName, currentFolder);
                            }
                        }
                        handler.SendResponse();
                    }
                }
            }
        }
    }
}
