using System;
using System.Reflection;

namespace XueFu.EntLib
{
    public sealed class FactoryHelper
    {
        private static object CreateObject(string path, string className)
        {
            className = path + "." + className;
            object cacheValue = CacheHelper.Read(className);
            if (cacheValue == null)
            {
                try
                {
                    cacheValue = Assembly.Load(path).CreateInstance(className);
                    CacheHelper.Write(className, cacheValue);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            return cacheValue;
        }

        private static object CreateObject<T>(string path, string className)
        {
            className = path + "." + className;
            object cacheValue = CacheHelper.Read(className);
            if (cacheValue == null)
            {
                try
                {
                    Assembly assembly = Assembly.Load(path);
                    Type typeGeneric = assembly.GetType(className).MakeGenericType(typeof(T).GetGenericArguments());
                    cacheValue = Activator.CreateInstance(typeGeneric); ;
                    CacheHelper.Write(className, cacheValue);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            return cacheValue;
        }

        public static T Instance<T>(string path, string className)
        {
            if (typeof(T).IsGenericType)
                return (T)CreateObject<T>(path, className);
            else
                return (T)CreateObject(path, className);
        }
    }
}
