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
            //�ȶ�ȡcacheKey�Ļ�������û�У��ͼ���CacheKey.config������, �ٶ�ȡcacheKey�Ļ�����
            if (CacheHelper.Read(cacheKey) == null)
            {
                RefreshCacheKey();
            }
            //��SocoShop_CacheKey������������ظ�Dictionary
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
                //��CacheKeys���ÿһ��Node��Key��Value��ֵд��Dictionary������
                foreach (XmlNode node in helper.ReadNode("CacheKeys").ChildNodes)
                {
                    cacheValue.Add(node.Attributes["Key"].Value, node.Attributes["Value"].Value);
                }
               
            }
            //��������������ʵ��
            CacheDependency cd = new CacheDependency(xmlFile);
            //ͨ��CacheHelper���������
            CacheHelper.Write(cacheKey, cacheValue, cd);
        }
    }
}
