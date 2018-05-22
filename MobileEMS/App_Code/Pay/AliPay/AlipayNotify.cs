using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace MobileEMS.Pay.AliPay
{
    public class AlipayNotify
    {
        #region 字段
        private string _partner = "";               //合作身份者ID
        private string _key = "";                   //密钥
        private string _public_key = "";            //支付宝的公钥
        private string _input_charset = "";         //编码格式
        private string _sign_type = "";             //签名方式

        //支付宝消息验证地址
        private string Https_veryfy_url = "https://mapi.alipay.com/gateway.do?service=notify_verify&";
        #endregion


        /// <summary>
        /// 构造函数
        /// 从配置文件中初始化变量
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <param name="notify_id">通知验证ID</param>
        public AlipayNotify()
        {
            //初始化基础配置信息
            PayConfig payConfig = new PayConfig();
            _partner = payConfig.Partner;
            _key = payConfig.SecurityKey;
            _public_key = payConfig.PublicKey.Trim();
            _input_charset = payConfig.InputCharset.Trim().ToLower();
            _sign_type = payConfig.SignType.Trim().ToUpper();
        }

        /// <summary>
        ///  验证消息是否是支付宝发出的合法消息
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <param name="notify_id">通知验证ID</param>
        /// <param name="sign">支付宝生成的签名结果</param>
        /// <returns>验证结果</returns>
        public bool Verify(SortedDictionary<string, string> inputPara, string notify_id, string sign)
        {
            bool isSign = false;
            //获取是否是支付宝服务器发来的请求的验证结果
            string responseTxt = "true";
            if (!string.IsNullOrEmpty(notify_id)) { responseTxt = GetResponseTxt(notify_id); }

            //判断responsetTxt是否为true
            //responsetTxt的结果不是true，与服务器设置问题、合作身份者ID、notify_id一分钟失效有关
            if (responseTxt == "true")
            {
                //获取返回时的签名验证结果
                isSign = GetSignVeryfy(inputPara, sign);

                //写日志记录（若要调试，请取消下面两行注释）
                string sWord = "responseTxt=" + responseTxt + "\n isSign=" + isSign.ToString() + "\n 返回回来的参数：" + GetPreSignStr(inputPara) + "\n ";
                AliPay.LogResult(sWord);
            }
            return isSign;
        }

        /// <summary>
        /// 获取待签名字符串（调试用）
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <returns>待签名字符串</returns>
        private string GetPreSignStr(SortedDictionary<string, string> inputPara)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>();

            //过滤空值、sign与sign_type参数
            sPara = AliPay.FilterPara(inputPara);

            //获取待签名字符串
            string preSignStr = AliPay.CreateLinkString(sPara);

            return preSignStr;
        }

        /// <summary>
        /// 获取返回时的签名验证结果
        /// </summary>
        /// <param name="inputPara">通知返回参数数组</param>
        /// <param name="sign">对比的签名结果</param>
        /// <returns>签名验证结果</returns>
        private bool GetSignVeryfy(SortedDictionary<string, string> inputPara, string sign)
        {
            //bool isSign = false;
            //if (!string.IsNullOrEmpty(sign))
            //{
            //    //获得签名结果
            //    string mysign = AliPay.BuildRequestMysign(AliPay.FilterPara(inputPara));
            //    if (mysign == sign)
            //    {
            //        isSign = true;
            //    }
            //}

            //获取待签名字符串
            string preSignStr = AliPay.CreateLinkString(AliPay.FilterPara(inputPara));

            //获得签名验证结果
            bool isSign = false;
            if (!string.IsNullOrEmpty(sign))
            {
                switch (_sign_type)
                {
                    case "MD5":
                        isSign = AlipayMD5.Verify(preSignStr, sign, _key, _input_charset);
                        break;
                    case "RSA":
                        isSign = RSAFromPkcs8.verify(preSignStr, sign, _public_key, _input_charset);
                        break;
                    default:
                        break;
                }
            }

            return isSign;
        }

        /// <summary>
        /// 获取是否是支付宝服务器发来的请求的验证结果
        /// </summary>
        /// <param name="notify_id">通知验证ID</param>
        /// <returns>验证结果</returns>
        private string GetResponseTxt(string notify_id)
        {
            string veryfy_url = Https_veryfy_url + "partner=" + _partner + "&notify_id=" + notify_id;

            //获取远程服务器ATN结果，验证是否是支付宝服务器发来的请求
            string responseTxt = Get_Http(veryfy_url, 120000);

            return responseTxt;
        }

        /// <summary>
        /// 获取远程服务器ATN结果
        /// </summary>
        /// <param name="strUrl">指定URL路径地址</param>
        /// <param name="timeout">超时时间设置</param>
        /// <returns>服务器ATN结果</returns>
        private string Get_Http(string strUrl, int timeout)
        {
            string strResult;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Timeout = timeout;
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.Default);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }

                strResult = strBuilder.ToString();
            }
            catch (Exception exp)
            {
                strResult = "错误：" + exp.Message;
            }

            return strResult;
        }
        
        

        
    }
}
