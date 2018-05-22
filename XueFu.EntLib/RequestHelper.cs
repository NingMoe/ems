using System;
using System.Web;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace XueFu.EntLib
{
    public sealed class RequestHelper
    {
        public RequestHelper()
        {
        }

        public static T GetForm<T>(string key)
        {
            object typeDefaultValue = new object();
            Type conversionType = typeof(T);
            try
            {
                string str = HttpContext.Current.Request.Form[key];
                if ((conversionType.FullName == "System.String") && (str == null))
                {
                    str = string.Empty;
                }
                typeDefaultValue = Convert.ChangeType(str, conversionType);
            }
            catch
            {
                typeDefaultValue = GetTypeDefaultValue(conversionType);
            }
            return (T)typeDefaultValue;
        }

        public static string GetIntsForm(string key)
        {
            string str = string.Empty;
            try
            {
                string str2 = HttpContext.Current.Request.Form[key];
                foreach (string str3 in str2.Split(new char[] { ',' }))
                {
                    if (str == string.Empty)
                    {
                        str = Convert.ToInt32(str3).ToString();
                    }
                    else
                    {
                        str = str + "," + Convert.ToInt32(str3).ToString();
                    }
                }
            }
            catch
            {
            }
            return str;
        }

        public static string GetIntsQueryString(string key)
        {
            string str = string.Empty;
            try
            {
                string str2 = HttpContext.Current.Request.QueryString[key];
                foreach (string str3 in str2.Split(new char[] { ',' }))
                {
                    if (str == string.Empty)
                    {
                        str = Convert.ToInt32(str3).ToString();
                    }
                    else
                    {
                        str = str + "," + Convert.ToInt32(str3).ToString();
                    }
                }
            }
            catch
            {
            }
            return str;
        }

        public static T GetQueryString<T>(string key)
        {
            object typeDefaultValue = new object();
            Type conversionType = typeof(T);
            try
            {
                string str = HttpContext.Current.Request.QueryString[key];
                if ((conversionType.FullName == "System.String") && (str == null))
                {
                    str = string.Empty;
                }
                typeDefaultValue = Convert.ChangeType(str, conversionType);
            }
            catch
            {
                typeDefaultValue = GetTypeDefaultValue(conversionType);
            }
            return (T)typeDefaultValue;
        }

        private static object GetTypeDefaultValue(Type type)
        {
            object obj2 = new object();
            string fullName = type.FullName;
            if (fullName == null)
            {
                return obj2;
            }
            if (!(fullName == "System.String"))
            {
                if (fullName != "System.Int32")
                {
                    if (fullName == "System.Decimal")
                    {
                        return -79228162514264337593543950335M;
                    }
                    if (fullName == "System.Double")
                    {
                        return -1.7976931348623157E+308;
                    }
                    if (fullName != "System.DateTime")
                    {
                        return obj2;
                    }
                    return DateTime.MinValue;
                }
            }
            else
            {
                return string.Empty;
            }
            return -2147483648;
        }


        public static DateTime DateNow
        {
            get { return DateTime.Now; }
        }


        public static string RawUrl
        {
            get { return HttpContext.Current.Request.RawUrl; }
        }

        /// <summary>
        /// 获取请求，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public static SortedDictionary<string, string> GetRequestQueryString()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = HttpContext.Current.Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                if (!string.IsNullOrEmpty(requestItem[i]))
                    sArray.Add(requestItem[i], HttpContext.Current.Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }

        /// <summary>
        /// 获取Get请求地址，去除参数
        /// </summary>
        /// <param name="key">要去除的参数</param>
        /// <returns>参数连接字符串</returns>
        public static string FilterRequestQueryString(string[] keyArray)
        {
            string requestQueryString = string.Empty;
            SortedDictionary<string, string> requestArray = GetRequestQueryString();
            foreach (string key in keyArray)
            {
                if (requestArray.ContainsKey(key))
                    requestArray.Remove(key);
            }
            foreach (KeyValuePair<string, string> item in requestArray)
            {
                if (!string.IsNullOrEmpty(item.Value))
                    requestQueryString = string.Concat(requestQueryString, "&", item.Key, "=", ServerHelper.UrlEncode(item.Value));
            }
            if (requestQueryString.StartsWith("&")) requestQueryString = requestQueryString.Substring(1);
            return requestQueryString;
        }

    }
}
