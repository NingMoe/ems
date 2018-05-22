using System;
using System.Web.Caching;
using XueFu.EntLib;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.Common
{    
    public sealed class ShopConfig
    {        
        private static string configCacheKey = CacheKey.ReadCacheKey("Config");
                
        public static ShopConfigInfo ReadConfigInfo()
        {
            //if (!string.IsNullOrEmpty(CookiesHelper.ReadCookieValue("LanguagePack")))
            //{
            //    configCacheKey = "EnglishConfig";
            //}
            //else
            //{
            //    configCacheKey = CacheKey.ReadCacheKey("Config");
            //}
            //ResponseHelper.Write("<script>alert('1|" + CookiesHelper.ReadCookieValue("LanguagePack") + "|" + CacheKey.ReadCacheKey("Config")+"|" + configCacheKey + "');</script>");
            //ִ�в����ǣ�ϵͳ�Զ�������CacheKey.config����CacheKey.ReadCacheKey("Config")�����Ѽ��ص������CacheKey.config�����Config���ֵ��Ȼ���жϻ������Ƿ���� CacheKey.ReadCacheKey("Config") ����ֵ�Ļ�������û�оͼ���ShopConfig.config�����档
            if (CacheHelper.Read(configCacheKey) == null)
            {
                RefreshConfigCache();
            }
            return (ShopConfigInfo)CacheHelper.Read(configCacheKey);
        }

        public static void RefreshApp()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add(".aspx", ".ashx");
            URLRewriterModule.ReplaceFileTypeDic = dictionary;
            URLRewriterModule.Path = "/Ashx/" + ReadConfigInfo().TemplatePath;
            URLRewriterModule.ForbidFolder = "|/Admin/|/Plugins/|/Install/|/API/|";
            CheckCode.CodeDot = ReadConfigInfo().CodeDot;
            CheckCode.CodeLength = ReadConfigInfo().CodeLength;
            CheckCode.CodeType = (CodeType)ReadConfigInfo().CodeType;
            CheckCode.Key = ReadConfigInfo().SecureKey;
        }

        public static void RefreshConfigCache()
        {
            string fileName = ServerHelper.MapPath(GetFileName());
            ShopConfigInfo cacheValue = ConfigHelper.ReadPropertyFromXml<ShopConfigInfo>(fileName);
            CacheDependency cd = new CacheDependency(fileName);
            CacheHelper.Write(configCacheKey, cacheValue, cd);
            RefreshApp();
        }

        public static void UpdateConfigInfo(ShopConfigInfo config)
        {
            ConfigHelper.UpdatePropertyToXml<ShopConfigInfo>(ServerHelper.MapPath(GetFileName()), config);
        }

        private static string GetFileName()
        {
            string fileName = "/Config/ShopConfig.config";
            //if (!string.IsNullOrEmpty(CookiesHelper.ReadCookieValue("LanguagePack")))
            //{
            //    fileName = "/Config/" + CookiesHelper.ReadCookieValue("LanguagePack") + "/ShopConfig.config";
            //}
            return fileName;
        }
    }
}
