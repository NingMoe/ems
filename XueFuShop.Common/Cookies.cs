using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using XueFu.EntLib;

namespace XueFuShop.Common
{    
    public sealed class Cookies
    {
        // Nested Types
        public sealed class Admin
        {            
            private static string cookiesName = ShopConfig.ReadConfigInfo().AdminCookies;
            
            private static bool CheckCookies()
            {
                string str = CookiesHelper.ReadCookieValue(cookiesName);
                if (str != string.Empty)
                {
                    try
                    {
                        string[] strArray = str.Split(new char[] { '|' });
                        string str2 = strArray[0];
                        string str3 = strArray[1];
                        string str4 = strArray[2];
                        string str5 = strArray[3];
                        string str6 = strArray[4];
                        if (FormsAuthentication.HashPasswordForStoringInConfigFile(str3 + str4 + str5 + str6 + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent, "MD5").ToLower() == str2.ToLower())
                        {
                            return true;
                        }
                        CookiesHelper.DeleteCookie(cookiesName);
                    }
                    catch
                    {
                        CookiesHelper.DeleteCookie(cookiesName);
                    }
                }
                return false;
            }

            public static int GetAdminID(bool check)
            {
                int num = 0;
                if (!check || (check && CheckCookies()))
                {
                    string str = CookiesHelper.ReadCookieValue(cookiesName);
                    if (!(str != string.Empty))
                    {
                        return num;
                    }
                    try
                    {
                        num = Convert.ToInt32(str.Split(new char[] { '|' })[1]);
                    }
                    catch
                    {
                    }
                }
                return num;
            }

            public static string GetAdminName(bool check)
            {
                string str = string.Empty;
                if (!check || (check && CheckCookies()))
                {
                    string str2 = CookiesHelper.ReadCookieValue(cookiesName);
                    if (!(str2 != string.Empty))
                    {
                        return str;
                    }
                    try
                    {
                        str = str2.Split(new char[] { '|' })[2];
                    }
                    catch
                    {
                    }
                }
                return str;
            }

            public static int GetGroupID(bool check)
            {
                int num = 0;
                if (!check || (check && CheckCookies()))
                {
                    string str = CookiesHelper.ReadCookieValue(cookiesName);
                    if (!(str != string.Empty))
                    {
                        return num;
                    }
                    try
                    {
                        num = Convert.ToInt32(str.Split(new char[] { '|' })[3]);
                    }
                    catch
                    {
                    }
                }
                return num;
            }

            public static string GetRandomNumber(bool check)
            {
                string str = string.Empty;
                if (!check || (check && CheckCookies()))
                {
                    string str2 = CookiesHelper.ReadCookieValue(cookiesName);
                    if (!(str2 != string.Empty))
                    {
                        return str;
                    }
                    try
                    {
                        str = str2.Split(new char[] { '|' })[4];
                    }
                    catch
                    {
                    }
                }
                return str;
            }
        }

        public sealed class Common
        {            
            public static string checkcode
            {
                get
                {
                    return StringHelper.Decode(CookiesHelper.ReadCookieValue(CheckCode.CookiesName), CheckCode.Key);
                }
            }
        }

        public sealed class User
        {            
            private static string cookiesName = ShopConfig.ReadConfigInfo().UserCookies;
            
            private static bool CheckCookies()
            {
                string str = CookiesHelper.ReadCookieValue(cookiesName);
                if (str != string.Empty)
                {
                    try
                    {
                        string[] strArray = str.Split(new char[] { '|' });
                        string str2 = strArray[0];
                        string str3 = strArray[1];
                        string str4 = strArray[2];
                        string str5 = strArray[3];
                        string str6 = strArray[4];
                        string str7 = strArray[5];
                        string str8 = strArray[6];
                        string str9 = strArray[7];
                        string str10 = strArray[8];
                        if (FormsAuthentication.HashPasswordForStoringInConfigFile(str3 + str4 + str5 + str6 + str7 + str8 + str9 + str10 + ShopConfig.ReadConfigInfo().SecureKey + ClientHelper.Agent, "MD5").ToLower() == str2.ToLower())
                        {
                            return true;
                        }
                        CookiesHelper.DeleteCookie(cookiesName);
                    }
                    catch
                    {
                        CookiesHelper.DeleteCookie(cookiesName);
                    }
                }
                return false;
            }

            public static string GetRealName(bool check)
            {
                string str = string.Empty;
                if (!check || (check && CheckCookies()))
                {
                    string str2 = CookiesHelper.ReadCookieValue(cookiesName);
                    if (!string.IsNullOrEmpty(str2))
                    {
                        try
                        {
                            str = HttpContext.Current.Server.UrlDecode(str2.Split(new char[] { '|' })[8]);
                        }
                        catch
                        {
                        }
                    }
                }
                return str;
            }

            public static int GetCompanyID(bool check)
            {
                int num = 0;
                if (!check || (check && CheckCookies()))
                {
                    string str = CookiesHelper.ReadCookieValue(cookiesName);
                    if (!(str != string.Empty))
                    {
                        return num;
                    }
                    try
                    {
                        num = Convert.ToInt32(str.Split(new char[] { '|' })[7]);
                    }
                    catch
                    {
                    }
                }
                return num;
            }

            public static int GetGroupID(bool check)
            {
                int num = 0;
                if (!check || (check && CheckCookies()))
                {
                    string str = CookiesHelper.ReadCookieValue(cookiesName);
                    if (!(str != string.Empty))
                    {
                        return num;
                    }
                    try
                    {
                        num = Convert.ToInt32(str.Split(new char[] { '|' })[6]);
                    }
                    catch
                    {
                    }
                }
                return num;
            }

            public static string GetMobile(bool check)
            {
                string str = string.Empty;
                if (!check || (check && CheckCookies()))
                {
                    string str2 = CookiesHelper.ReadCookieValue(cookiesName);
                    if (!string.IsNullOrEmpty(str2))
                    {
                        try
                        {
                            str = HttpContext.Current.Server.UrlDecode(str2.Split(new char[] { '|' })[5]);
                        }
                        catch
                        {
                        }
                    }
                }
                return str;
            }

            public static int GetGradeID(bool check)
            {
                int num = 0;
                if (!check || (check && CheckCookies()))
                {
                    string str = CookiesHelper.ReadCookieValue(cookiesName);
                    if (!(str != string.Empty))
                    {
                        return num;
                    }
                    try
                    {
                        num = Convert.ToInt32(str.Split(new char[] { '|' })[4]);
                    }
                    catch
                    {
                    }
                }
                return num;
            }

            public static decimal GetMoneyUsed(bool check)
            {
                decimal num = 0M;
                if (!check || (check && CheckCookies()))
                {
                    string str = CookiesHelper.ReadCookieValue(cookiesName);
                    if (!(str != string.Empty))
                    {
                        return num;
                    }
                    try
                    {
                        num = Convert.ToDecimal(str.Split(new char[] { '|' })[3]);
                    }
                    catch
                    {
                    }
                }
                return num;
            }

            public static string GetUserName(bool check)
            {
                string str = string.Empty;
                if (!check || (check && CheckCookies()))
                {
                    string str2 = CookiesHelper.ReadCookieValue(cookiesName);
                    if (!(str2 != string.Empty))
                    {
                        return str;
                    }
                    try
                    {
                        str = HttpContext.Current.Server.UrlDecode(str2.Split(new char[] { '|' })[2]);
                    }
                    catch
                    {
                    }
                }
                return str;
            }

            public static int GetUserID(bool check)
            {
                int num = 0;
                if (!check || (check && CheckCookies()))
                {
                    string str = CookiesHelper.ReadCookieValue(cookiesName);
                    if (!(str != string.Empty))
                    {
                        return num;
                    }
                    try
                    {
                        num = Convert.ToInt32(str.Split(new char[] { '|' })[1]);
                    }
                    catch
                    {
                    }
                }
                return num;
            }
        }
    }
}
