using System;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace XueFu.EntLib
{
    public sealed class ServerHelper
    {
        public static object CopyClass(object objClass)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                stream.Position = 0;
                formatter.Serialize(stream, objClass);
                stream.Position = 0;
                return formatter.Deserialize(stream);
            }
        }

        public static string MapPath(string filePath)
        {
            string str = string.Empty;
            try
            {
                str = HttpContext.Current.Server.MapPath(filePath);
            }
            catch (Exception exception)
            {
                ResponseHelper.Write(exception.ToString());
                ResponseHelper.End();
            }
            return str;
        }

        public static string Path
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public static string UrlEncode(string Url)
        {
            return HttpContext.Current.Server.UrlEncode(Url);
        }

        public static string UrlDecode(string Url)
        {
            return HttpContext.Current.Server.UrlDecode(Url);
        }

        public static string UrlPathEncode(string Url)
        {
            return HttpContext.Current.Server.UrlPathEncode(Url);
        }

    }
}
