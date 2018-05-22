using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Caching;
using XueFu.EntLib;

namespace XueFuShop.Common
{    
    public sealed class CacheKey    {
        
        private static string cacheKey = "SocoShop_CacheKey";
        
        public static string ReadCacheKey(string key)
        {
            //先读取cacheKey的缓存项，如果没有，就加载CacheKey.config到缓存, 再读取cacheKey的缓存项
            if (CacheHelper.Read(cacheKey) == null)
            {
                RefreshCacheKey();
            }
            //把SocoShop_CacheKey缓存依赖项读回给Dictionary
            Dictionary<string, string> dictionary = (Dictionary<string, string>)CacheHelper.Read(cacheKey);
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            return Guid.NewGuid().ToString();
        }

        public static void RefreshCacheKey()
        {
            string xmlFile = ServerHelper.MapPath("/Config/CacheKey.config");
            Dictionary<string, string> cacheValue = new Dictionary<string, string>();
            using (XmlHelper helper = new XmlHelper(xmlFile))
            {
                //把CacheKeys里的每一行Node的Key和Value的值写入Dictionary序列中
                foreach (XmlNode node in helper.ReadNode("CacheKeys").ChildNodes)
                {
                    cacheValue.Add(node.Attributes["Key"].Value, node.Attributes["Value"].Value);
                }
               
            }
            //创建缓存依赖项实例
            CacheDependency cd = new CacheDependency(xmlFile);
            //通过CacheHelper添加依赖项
            CacheHelper.Write(cacheKey, cacheValue, cd);
        }
    }
}
