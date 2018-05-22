using System;
using System.Collections.Generic;
using System.Text;

namespace LetvAPI
{
    public class LetvConfig
    {
        private static string user_unique;
        private static string format;
        private static string ver;
        private static string secretkey;
        private static string input_charset;
        private static string gateway;

        /// <summary>
        /// 构造函数
        /// </summary>
        static LetvConfig()
        {
            gateway = "http://api.letvcloud.com/open.php?";
            user_unique = "debb2235d3";
            format = "json";
            secretkey = "514add0d4ab0dc5befa928c5e5af2a9e";
            ver = "2.0";
            input_charset = "utf-8";
        }

        /// <summary>
        /// 乐视网关地址
        /// </summary>
        public static string GateWay
        {
            get { return gateway; }
        }

        /// <summary>
        /// 用户唯一标识码
        /// </summary>
        public static string User_Unique
        {
            get { return user_unique; }
        }

        /// <summary>
        /// 返回参数格式：支持json和xml两种方式
        /// </summary>
        public static string Format
        {
            get { return format; }
        }

        /// <summary>
        /// 协议版本号，统一取值为2.0
        /// </summary>
        public static string Ver
        {
            get { return ver; }
        }

        /// <summary>
        /// 私钥
        /// </summary>
        public static string SecretKey
        {
            get { return secretkey; }
        }

        /// <summary>
        /// 私钥
        /// </summary>
        public static string Input_Charset
        {
            get { return input_charset; }
        }
    }
}
