using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace XueFu.EntLib
{
    public class MD5Helper
    {
        public static string MD5(string encypString, string _input_charset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] md5Byte = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(encypString));
            //StringBuilder sb = new StringBuilder(32);
            //for (int i = 0; i < md5Byte.Length; i++)
            //{
            //    sb.Append(md5Byte[i].ToString("x").PadLeft(2, '0'));
            //}
            return System.BitConverter.ToString(md5Byte).Replace("-", "").ToLower();
        }

        public static string MD5(string encypString)
        {
            return MD5(encypString, Encoding.Default.WebName);
        }
    }
}
