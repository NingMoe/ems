using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Hosting;
using System.Web;
using System.Security.Permissions;
using XueFu.EntLib;
using System.Web.Caching;
using System.IO;

namespace XueFuShop.Common
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ShopVirtualFile : VirtualFile
    {
        
        private string content;
        private Dictionary<string, string> fileNameDic;

        
        public ShopVirtualFile(string virtualPath) : base(virtualPath)
        {
            this.fileNameDic = new Dictionary<string, string>();
            this.GetRightFileName();
            this.GetData();
        }

        protected void GetData()
        {
            string str = base.VirtualPath.Replace("/Ashx/" + ShopConfig.ReadConfigInfo().TemplatePath + "/", string.Empty).Replace(".ashx", ".htm");            
            try
            {
                XueFuTemplateFile file = new XueFuTemplateFile(this.fileNameDic[str.ToLower()], "/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/");
                file.InheritsNameSpace = "XueFuShop.Pages";
                file.NameSpace = "XueFuShop.Web";
                this.content = file.Process();
            }
            catch
            {
                StringBuilder Output = new StringBuilder();
                foreach (KeyValuePair<string, string> info in this.fileNameDic)
                {
                    Output.Append(info.Key + ":" + info.Value+"<br>");
                }
                Output.Append("原始地址:" + base.VirtualPath + "替换目标：" + "/Ashx/" + ShopConfig.ReadConfigInfo().TemplatePath + "/" + "替换后的地址：" + base.VirtualPath.Replace("/Ashx/" + ShopConfig.ReadConfigInfo().TemplatePath + "/", string.Empty) + "现在请求的地址为：" + str.ToLower() + "，路径为：" + "/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/");
                this.content = Output.ToString();
            }
        }

        private void GetRightFileName()
        {
            string cacheKey = "ShopTemplateFile";
            if (CacheHelper.Read(cacheKey) == null)
            {
                List<FileInfo> list = FileHelper.ListDirectory(ServerHelper.MapPath("/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/"), "|.htm|");
                foreach (FileInfo info in list)
                {
                    string str2 = info.FullName.Replace(ServerHelper.MapPath(@"\Plugins\Template\" + ShopConfig.ReadConfigInfo().TemplatePath + @"\"), string.Empty).Replace(@"\", "/");
                    this.fileNameDic.Add(str2.ToLower(), str2);
                }
                CacheDependency cd = new CacheDependency(ServerHelper.MapPath("/Plugins/Template/" + ShopConfig.ReadConfigInfo().TemplatePath + "/"));
                CacheHelper.Write(cacheKey, this.fileNameDic, cd);
            }
            else
            {
                this.fileNameDic = (Dictionary<string, string>)CacheHelper.Read(cacheKey);
            }
        }

        public override Stream Open()
        {
            this.content = this.content.Replace("</body>", ("<div id=\\\"autocopyright\\\" style=\\\"text-align:center; font-size:12px; margin-bottom:10px\\\"><a href=\\\"#\\\" target=\\\"_blank\\\" style=\\\"text-decoration:none; color:#4C5A62\\\">" + Global.ProductName + " " + Global.Version + "</a>&nbsp;&nbsp;<a href=\\\"#\\\" target=\\\"_blank\\\" style=\\\"text-decoration:none; color:#4C5A62\\\">" + Global.CopyRight + "</a></div>") + "</body>");
            Stream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(this.content);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        
        public bool Exists
        {
            get
            {
                return (this.content != null);
            }
        }
    }
}
