using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Web;

namespace LetvAPI
{
    /// <summary>
    /// 类名：Core
    /// 功能：乐视接口公用函数类
    /// 详细：该类是请求、通知返回两个文件所调用的公用函数核心处理文件，不需要修改
    /// 版本：1.0
    /// 修改日期：2014-12-10
    /// 说明：
    /// 该代码仅供乐视接口使用。
    /// </summary>
    /// 
    public class LetvCore
    {
        #region 字段

        //乐视网关地址
        private static string _gateway = "";

        //编码格式
        private static string _input_charset = "";

        //私钥
        private static string _secretkey = "";

        #endregion

        static LetvCore()
        {
            _gateway = LetvConfig.GateWay;
            _input_charset = LetvConfig.Input_Charset;
            _secretkey = LetvConfig.SecretKey;
        }

        // <summary>
        /// 除去数组中签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        private static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key.ToLower() != "sign")
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }

        /// <summary>
        /// 把数组所有元素，按照“参数参数值”的模式不用任何字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        private static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + temp.Value);
            }

            return prestr.ToString();
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>拼接完成以后的字符串</returns>
        private static string CreateLinkString(Dictionary<string, string> dicArray, Encoding code)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + HttpUtility.UrlEncode(temp.Value, code) + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// 写日志，方便测试（看网站需求，也可以改成把记录存入数据库）
        /// </summary>
        /// <param name="sWord">要写入日志里的文本内容</param>
        public static void LogResult(string sWord)
        {
            string strPath = HttpContext.Current.Server.MapPath("log");
            strPath = strPath + "\\" + DateTime.Now.ToString().Replace(":", "") + ".txt";
            StreamWriter fs = new StreamWriter(strPath, false, System.Text.Encoding.Default);
            fs.Write(sWord);
            fs.Close();
        }


        /// <summary>
        /// 生成要请求给乐视的系统参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <returns>要请求的参数数组</returns>
        private static string BuildSignToString(SortedDictionary<string, string> sParaTemp)
        {
            //待签名请求参数数组
            Dictionary<string, string> sPara = new Dictionary<string, string>();

            //过滤签名参数数组
            sPara = FilterPara(sParaTemp);

            //把参数组中所有元素拼接成字符串
            string strRequestData = CreateLinkString(sPara);

            //加入私钥
            strRequestData += _secretkey;

            //获得签名结果
            strRequestData = BuildRequestMysign(strRequestData);

            return strRequestData;
        }

        /// <summary>
        /// 生成要请求给乐视的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <returns>要请求的参数数组字符串</returns>
        public static string BuildRequestParaToString(SortedDictionary<string, string> sParaTemp)
        {
            return BuildRequestParaToString(sParaTemp, Encoding.GetEncoding(_input_charset));
        }

        /// <summary>
        /// 生成要请求给乐视的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>要请求的参数数组字符串</returns>
        public static string BuildRequestParaToString(SortedDictionary<string, string> sParaTemp, Encoding code)
        {
            //待签名请求参数数组
            Dictionary<string, string> sPara = new Dictionary<string, string>();

            //过滤签名参数数组
            sPara = FilterPara(sParaTemp);

            //把参数组中所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
            string strRequestData = CreateLinkString(sPara, code);

            strRequestData += "&sign=" + BuildSignToString(sParaTemp);
            strRequestData = _gateway + strRequestData;

            return strRequestData;
        }

        /// <summary>
        /// 生成请求时的签名
        /// </summary>
        /// <param name="sPara">请求给乐视的参数数组</param>
        /// <returns>签名结果</returns>
        private static string BuildRequestMysign(string prestr)
        {
            StringBuilder mysign = new StringBuilder(32);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                mysign.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return mysign.ToString();
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="sParaTemp">请求的参数数组</param>
        /// <param name="sign">签名结果</param>
        /// <returns>验证结果</returns>
        public static bool Verify(SortedDictionary<string, string> sParaTemp, string sign)
        {
            string mysign = BuildSignToString(sParaTemp);
            if (mysign == sign)
            {
                return true;
            }
            else
            {
                return false;
            }
        }        
    }
}
