using System;
using System.Web;

namespace XueFu.EntLib
{
    public enum TimeType
    {
        Year,
        Month,
        Day,
        Hour,
        Minute,
        Second,
        Millisecond
    }

    #region Cookies������
    public sealed class CookiesHelper
    {
        /// <summary>
        /// ���һ��Cookie[�ر����������]
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddCookie(string name, string value)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.Path = "/";
            cookie.Value = value;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// ���Cookies
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="time">����ʱ��</param>
        /// <param name="timeType">ʱ������</param>
        public static void AddCookie(string name, string value, int time, TimeType timeType)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.Path = "/";
            cookie.Value = value;
            switch (timeType)
            {
                case TimeType.Year:
                    cookie.Expires = DateTime.Now.AddYears(time);
                    break;

                case TimeType.Month:
                    cookie.Expires = DateTime.Now.AddMonths(time);
                    break;

                case TimeType.Day:
                    cookie.Expires = DateTime.Now.AddDays((double)time);
                    break;

                case TimeType.Hour:
                    cookie.Expires = DateTime.Now.AddHours((double)time);
                    break;

                case TimeType.Minute:
                    cookie.Expires = DateTime.Now.AddMinutes((double)time);
                    break;

                case TimeType.Second:
                    cookie.Expires = DateTime.Now.AddSeconds((double)time);
                    break;

                case TimeType.Millisecond:
                    cookie.Expires = DateTime.Now.AddMilliseconds((double)time);
                    break;
            }
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// ɾ��ָ��Cookie
        /// </summary>
        /// <param name="name"></param>
        public static void DeleteCookie(string name)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.Expires = DateTime.Now.AddDays(-10.0);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// ��ȡָ��cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HttpCookie ReadCookie(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }

        /// <summary>
        /// ��ȡָ��Cookieֵ
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ReadCookieValue(string name)
        {
            string str = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
            {
                return (str = cookie.Value);
            }
            return str;
        }
    }
    #endregion
}
