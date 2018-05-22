using System;
using System.Collections.Generic;
using System.Web;
using XueFu.EntLib;

namespace MobileEMS.Pay.AliPay
{
    public class PayConfig
    {
        private string partner = string.Empty;
        private string sellerId = string.Empty;
        private string sellerEmail = string.Empty;
        private string securityKey = string.Empty;
        private string privateKey = string.Empty;
        private string publicKey = string.Empty;
        private string service = string.Empty;
        private string inputcharset = string.Empty;
        private string signtype = string.Empty;
        /// <summary>
        /// 合作伙伴ID
        /// </summary>
        public string Partner
        {
            get { return this.partner; }
        }
        /// <summary>
        /// 获取合作者身份ID
        /// </summary>
        public string SellerId
        {
            get { return this.sellerId; }
        }
        /// <summary>
        /// 卖家帐号
        /// </summary>
        public string SellerEmail
        {
            get { return this.sellerEmail; }
        }
        /// <summary>
        /// partner账户的支付宝安全校验码
        /// </summary>
        public string SecurityKey
        {
            get { return this.securityKey; }
        }
        /// <summary>
        /// 获取商户的私钥
        /// </summary>
        public string PrivateKey
        {
            get { return this.privateKey; }
        }
        /// <summary>
        /// 获取支付宝的公钥
        /// </summary>
        public string PublicKey
        {
            get { return this.publicKey; }
        }
        /// <summary>
        /// 服务参数
        /// </summary>
        public string Service
        {
            get { return this.service; }
        }

        /// <summary>
        /// 获取字符编码格式
        /// </summary>
        public string InputCharset
        {
            get { return inputcharset; }
        }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public string SignType
        {
            get { return signtype; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PayConfig()
        {
            using (XmlHelper xh = new XmlHelper(ServerHelper.MapPath("/Plugins/Pay/AliPay/AliPay.Config")))
            {
                this.partner = xh.ReadAttribute("Pay/Partner", "Value");
                this.sellerId = partner;
                this.sellerEmail = xh.ReadAttribute("Pay/SellerEmail", "Value");
                this.securityKey = xh.ReadAttribute("Pay/SecurityKey", "Value");
                this.privateKey = xh.ReadAttribute("Pay/PrivateKey", "Value");
                this.publicKey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";
                this.service = xh.ReadAttribute("Pay/Service", "Value");
                this.inputcharset = xh.ReadAttribute("Pay/InputCharset", "Value");
                this.signtype = xh.ReadAttribute("Pay/SignType", "Value");
            }
        }
    }
}
