using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using System.Xml;
using System.Web.Caching;

namespace XueFuShop.Common
{
    public sealed class URLClass
    {
        private static string fileName = ServerHelper.MapPath("/Config/URLRewriter.config");
        private static string urlCacheKey = CacheKey.ReadCacheKey("URL");

        public static int AddURL(URLInfo url)
        {
            using (XmlHelper helper = new XmlHelper(fileName))
            {
                string[] attrib = new string[] { "ID", "VitualPath", "RealPath", "IsEffect" };
                int num = Convert.ToInt32(helper.ReadNode("URLRewriters").LastChild.Attributes["ID"].Value);
                string[] attribContent = new string[] { (num + 1).ToString(), url.VitualPath, url.RealPath, url.IsEffect.ToString() };
                helper.InsertElement("URLRewriters", "URLRewriter", attrib, attribContent, string.Empty);
                helper.Save();
                return (num + 1);
            }
        }

        public static void DeleteURL(string strID)
        {
            using (XmlHelper helper = new XmlHelper(fileName))
            {
                XmlNodeList childNodes = helper.ReadNode("URLRewriters").ChildNodes;
                foreach (XmlNode node in childNodes)
                {
                    if (strID.IndexOf(node.Attributes["ID"].Value) > -1)
                    {
                        helper.ReadNode("URLRewriters").RemoveChild(node);
                    }
                }
                helper.Save();
            }
        }

        public static URLInfo ReadURL(int id)
        {
            URLInfo info = new URLInfo();
            foreach (URLInfo info2 in ReadURLList())
            {
                if (info2.ID == id)
                {
                    return info2;
                }
            }
            return info;
        }

        public static List<URLInfo> ReadURLList()
        {
            if (CacheHelper.Read(urlCacheKey) == null)
            {
                RefreshURLLCache();
            }
            return (List<URLInfo>)CacheHelper.Read(urlCacheKey);
        }

        public static void RefreshURLLCache()
    {
        List<URLInfo> cacheValue = new List<URLInfo>();
        using (XmlHelper helper = new XmlHelper(fileName))
        {
            XmlNodeList childNodes = helper.ReadNode("URLRewriters").ChildNodes;
            foreach (XmlNode node in childNodes)
            {
                URLInfo Item = new URLInfo();
                Item.ID = Convert.ToInt32(node.Attributes["ID"].Value);
                Item.VitualPath = node.Attributes["VitualPath"].Value;
                Item.RealPath = node.Attributes["RealPath"].Value;
                Item.IsEffect = Convert.ToBoolean(node.Attributes["IsEffect"].Value);
                cacheValue.Add(Item);
            }
        }
        CacheDependency cd = new CacheDependency(fileName);
        CacheHelper.Write(urlCacheKey, cacheValue, cd);
    }

        public static void UpdateURL(URLInfo url)
        {
            using (XmlHelper helper = new XmlHelper(fileName))
            {
                XmlNodeList childNodes = helper.ReadNode("URLRewriters").ChildNodes;
                foreach (XmlNode node in childNodes)
                {
                    if (url.ID == Convert.ToInt32(node.Attributes["ID"].Value))
                    {
                        node.Attributes["VitualPath"].Value = url.VitualPath;
                        node.Attributes["RealPath"].Value = url.RealPath;
                        node.Attributes["IsEffect"].Value = url.IsEffect.ToString();
                    }
                }
                helper.Save();
            }
        }
    }
}
