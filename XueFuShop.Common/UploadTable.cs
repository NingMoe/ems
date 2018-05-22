using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Web.Caching;
using System.Xml;
using XueFu.EntLib;

namespace XueFuShop.Common
{
    public sealed class UploadTable
    {
        
        private static string uploadTableCacheKey = CacheKey.ReadCacheKey("UploadTable");

        
        public static int ReadTableID(string key)
        {
            if (CacheHelper.Read(uploadTableCacheKey) == null)
            {
                RefreshUploadTableCache();
            }
            return ((Dictionary<string, int>)CacheHelper.Read(uploadTableCacheKey))[key];
        }

        public static void RefreshUploadTableCache()
        {
            string xmlFile = ServerHelper.MapPath("/Config/UploadTable.config");
            Dictionary<string, int> cacheValue = new Dictionary<string, int>();
            using (XmlHelper helper = new XmlHelper(xmlFile))
            {
                foreach (XmlNode node in helper.ReadNode("UploadTable").ChildNodes)
                {
                    cacheValue.Add(node.Attributes["Key"].Value, Convert.ToInt32(node.Attributes["Value"].Value));
                }
            }
            CacheDependency cd = new CacheDependency(xmlFile);
            CacheHelper.Write(uploadTableCacheKey, cacheValue, cd);
        }
    }
}
