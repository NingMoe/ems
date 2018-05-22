using System;
using System.Collections.Generic;
using System.Web;
using XueFuShop.Common;
using XueFu.EntLib;

namespace XueFuShop.Login.QQ
{
    /// <summary>
    /// 新浪微博登录配置
    /// </summary>
    public class LoginConfig
    {
        private string appKey = string.Empty;
        private string appSecret = string.Empty;
        /// <summary>
        /// AppKey
        /// </summary>
        public string AppKey
        {
            get { return this.appKey; }
        }
        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret
        {
            get { return this.appSecret; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginConfig()
        {
            using (XmlHelper xh = new XmlHelper(ServerHelper.MapPath("/Plugins/Login/QQ/Login.Config")))
            {
                this.appKey = xh.ReadAttribute("Login/AppKey", "Value");
                this.appSecret = xh.ReadAttribute("Login/AppSecret", "Value");           
            }
        }
    }
}
