using System;
using System.Text.RegularExpressions;

namespace XueFu.EntLib
{
    public class UserAgentHelper
    {
        //关键字在UserAgent中的位置
        private static int keywordPos = 0;

        /// <summary>
        /// 是否微信浏览器
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static bool IsWx(string userAgent)
        {
            keywordPos = userAgent.Trim().ToLower().IndexOf("micromessenger");
            if (keywordPos >= 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否微信浏览器（如果是微信浏览器，则一并返回版本号）
        /// </summary>
        /// <param name="userAgent"></param>
        /// <param name="wxVersion"></param>
        /// <returns></returns>
        public static bool IsWxWithWxVersion(string userAgent, ref string wxVersion)
        {
            if (IsWx(userAgent))
            {
                string wxVersionInfor = userAgent.Trim().ToLower().Substring(keywordPos);
                try
                {
                    wxVersion = wxVersionInfor.Split('/')[1];
                }
                catch
                {
                    wxVersion = "1.0";
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否移动端
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static bool IsMobile(string userAgent)
        {
            userAgent = userAgent.Trim().ToLower();
            //string[] uAgentKeyword = new string[] {"Mobile","Windows Phone","iPhone","iPod","iPad","Android","BlackBerry","SymbianOS","TouchPad","UCWEB"};
            string uAgentKeywords = "Android|Windows Phone|iPhone|iPod|BlackBerry|SymbianOS|TouchPad|UCWEB|MQQBrowser|SAMSUNG";//|iPad|Mobile"mozilla|m3gate|winwap|openwave|Windows NT|Windows 3.1|95|Blackcomb|98|ME|X Window|Longhorn|ubuntu|AIX|Linux|AmigaOS|BEOS|HP-UX|OpenBSD|FreeBSD|NetBSD|OS/2|OSF1|SUN";
            Regex reg = new Regex(uAgentKeywords.ToLower());
            if (reg.IsMatch(userAgent))
            {
                return true;
            }
            return false;
        }
    }
}
