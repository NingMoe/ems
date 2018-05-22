using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Net;
using System.Web.Security;
using LitJson;

namespace WxPayAPI
{
    public class JsApiPay
    {
        /// <summary>
        /// ����ҳ�������ΪҪ����ķ�����ʹ��Page��Request����
        /// </summary>
        private Page _page;
        private Page page
        {
            get { return _page; }
            set { _page = value; }
        }

        /// <summary>
        /// openid���ڵ���ͳһ�µ��ӿ�
        /// </summary>
        private string _openid;
        public string openid
        {
            get { return _openid; }
            set { _openid = value; }
        }

        /// <summary>
        /// access_token���ڻ�ȡ�ջ���ַjs������ڲ���
        /// </summary>
        private string _access_token;
        public string access_token
        {
            get { return _access_token; }
            set { _access_token = value; }
        }

        /// <summary>
        /// ��Ʒ������ͳһ�µ�
        /// </summary>
        private int _total_fee;
        public int total_fee
        {
            get { return _total_fee; }
            set { _total_fee = value; }
        }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        private string _body;
        public string body
        {
            get { return _body; }
            set { _body = value; }
        }

        /// <summary>
        /// �̻�������
        /// </summary>
        private string _out_trade_no = WxPayApi.GenerateTimeStamp();
        public string out_trade_no
        {
            get { return _out_trade_no; }
            set { _out_trade_no = value; }
        }

        /// <summary>
        /// ͳһ�µ��ӿڷ��ؽ��
        /// </summary>
        private WxPayData _unifiedOrderResult;
        public WxPayData unifiedOrderResult
        {
            get { return _unifiedOrderResult; }
            set { _unifiedOrderResult = value; }
        }
        //public WxPayData unifiedOrderResult;

        public JsApiPay(Page page)
        {
            this.page = page;
        }


        /**
        * 
        * ��ҳ��Ȩ��ȡ�û�������Ϣ��ȫ������
        * ������ο���ҳ��Ȩ��ȡ�û�������Ϣ��http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
        * ��һ��������url��ת��ȡcode
        * �ڶ���������codeȥ��ȡopenid��access_token
        * 
        */
        public void GetOpenidAndAccessToken()
        {
            if (!string.IsNullOrEmpty(page.Request.QueryString["code"]))
            {
                //��ȡcode�룬�Ի�ȡopenid��access_token
                string code = page.Request.QueryString["code"];
                Log.Debug(this.GetType().ToString(), "Get code : " + code);
                GetOpenidAndAccessTokenFromCode(code);
            }
            else
            {
                //������ҳ��Ȩ��ȡcode��URL
                string host = page.Request.Url.Host;
                string path = page.Request.Path;
                string redirect_uri = page.Request.Url.ToString();//HttpUtility.UrlEncode("http://" + host + path);
                WxPayData data = new WxPayData();
                data.SetValue("appid", WxPayConfig.APPID);
                data.SetValue("redirect_uri", redirect_uri);
                data.SetValue("response_type", "code");
                data.SetValue("scope", "snsapi_base");
                data.SetValue("state", "STATE" + "#wechat_redirect");
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();
                Log.Debug(this.GetType().ToString(), "Will Redirect to URL : " + url);
                try
                {
                    //����΢�ŷ���code��         
                    page.Response.Redirect(url);//Redirect�������׳�ThreadAbortException�쳣�����ô�������쳣
                }
                catch (System.Threading.ThreadAbortException ex)
                {
                }
            }
        }


        /**
	    * 
	    * ͨ��code��ȡ��ҳ��Ȩaccess_token��openid�ķ������ݣ���ȷʱ���ص�JSON���ݰ����£�
	    * {
	    *  "access_token":"ACCESS_TOKEN",
	    *  "expires_in":7200,
	    *  "refresh_token":"REFRESH_TOKEN",
	    *  "openid":"OPENID",
	    *  "scope":"SCOPE",
	    *  "unionid": "o6_bmasdasdsad6_2sgVt7hMZOPfL"
	    * }
	    * ����access_token�����ڻ�ȡ�����ջ���ַ
	    * openid��΢��֧��jsapi֧���ӿ�ͳһ�µ�ʱ����Ĳ���
        * ����ϸ��˵����ο���ҳ��Ȩ��ȡ�û�������Ϣ��http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
        * @ʧ��ʱ���쳣WxPayException
	    */
        public void GetOpenidAndAccessTokenFromCode(string code)
        {
            try
            {
                //�����ȡopenid��access_token��url
                WxPayData data = new WxPayData();
                data.SetValue("appid", WxPayConfig.APPID);
                data.SetValue("secret", WxPayConfig.APPSECRET);
                data.SetValue("code", code);
                data.SetValue("grant_type", "authorization_code");
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?" + data.ToUrl();

                //����url�Ի�ȡ����
                string result = HttpService.Get(url);

                Log.Debug(this.GetType().ToString(), "GetOpenidAndAccessTokenFromCode response : " + result);

                //����access_token�������ջ���ַ��ȡ
                JsonData jd = JsonMapper.ToObject(result);
                access_token = (string)jd["access_token"];

                //��ȡ�û�openid
                openid = (string)jd["openid"];

                Log.Debug(this.GetType().ToString(), "Get openid : " + openid);
                Log.Debug(this.GetType().ToString(), "Get access_token : " + access_token);
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex.ToString());
                throw new WxPayException(ex.ToString());
            }
        }

        /**
         * ����ͳһ�µ�������µ����
         * @return ͳһ�µ����
         * @ʧ��ʱ���쳣WxPayException
         */
        public WxPayData GetUnifiedOrderResult()
        {
            //ͳһ�µ�
            WxPayData data = new WxPayData();
            data.SetValue("body", body);
            data.SetValue("attach", "test");
            data.SetValue("out_trade_no", out_trade_no);
            data.SetValue("total_fee", total_fee);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("goods_tag", "test");
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", openid);

            WxPayData result = WxPayApi.UnifiedOrder(data);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error!");
            }

            unifiedOrderResult = result;
            return result;
        }

        /**
        *  
        * ��ͳһ�µ��ɹ����ص������л�ȡ΢�����������jsapi֧������Ĳ�����
        * ΢�����������JSAPIʱ�����������ʽ���£�
        * {
        *   "appId" : "wx2421b1c4370ec43b",     //���ں����ƣ����̻�����     
        *   "timeStamp":" 1395712654",         //ʱ�������1970������������     
        *   "nonceStr" : "e61463f8efa94090b1f366cccfbbb444", //�����     
        *   "package" : "prepay_id=u802345jgfjsdfgsdg888",     
        *   "signType" : "MD5",         //΢��ǩ����ʽ:    
        *   "paySign" : "70EA570631E4BB79628FBCA90534C63FF7FADD89" //΢��ǩ�� 
        * }
        * @return string ΢�����������JSAPIʱ�����������json��ʽ����ֱ����������
        * ����ϸ��˵����ο���ҳ�˵���֧��API��http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7
        * 
        */
        public string GetJsApiParameters()
        {
            Log.Debug(this.GetType().ToString(), "JsApiPay::GetJsApiParam is processing...");

            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", unifiedOrderResult.GetValue("appid"));
            jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
            jsApiParam.SetValue("package", "prepay_id=" + unifiedOrderResult.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

            string parameters = jsApiParam.ToJson();

            Log.Debug(this.GetType().ToString(), "Get jsApiParam : " + parameters);
            return parameters;
        }


        /**
	    * 
	    * ��ȡ�ջ���ַjs������ڲ���,������ο��ջ���ַ����ӿڣ�http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_9
	    * @return string �����ջ���ַjs������Ҫ�Ĳ�����json��ʽ����ֱ��������ʹ��
	    */
        public string GetEditAddressParameters()
        {
            string parameter = "";
            try
            {
                string host = page.Request.Url.Host;
                string path = page.Request.Path;
                string queryString = page.Request.Url.Query;
                //����ط�Ҫע�⣬����ǩ��������ҳ��Ȩ��ȡ�û���Ϣʱ΢�ź�̨�ش�������url
                string url = "http://" + host + path + queryString;

                //������Ҫ��SHA1�㷨���ܵ�����
                WxPayData signData = new WxPayData();
                signData.SetValue("appid", WxPayConfig.APPID);
                signData.SetValue("url", url);
                signData.SetValue("timestamp", WxPayApi.GenerateTimeStamp());
                signData.SetValue("noncestr", WxPayApi.GenerateNonceStr());
                signData.SetValue("accesstoken", access_token);
                string param = signData.ToUrl();

                Log.Debug(this.GetType().ToString(), "SHA1 encrypt param : " + param);
                //SHA1����
                string addrSign = FormsAuthentication.HashPasswordForStoringInConfigFile(param, "SHA1");
                Log.Debug(this.GetType().ToString(), "SHA1 encrypt result : " + addrSign);

                //��ȡ�ջ���ַjs������ڲ���
                WxPayData afterData = new WxPayData();
                afterData.SetValue("appId", WxPayConfig.APPID);
                afterData.SetValue("scope", "jsapi_address");
                afterData.SetValue("signType", "sha1");
                afterData.SetValue("addrSign", addrSign);
                afterData.SetValue("timeStamp", signData.GetValue("timestamp"));
                afterData.SetValue("nonceStr", signData.GetValue("noncestr"));

                //תΪjson��ʽ
                parameter = afterData.ToJson();
                Log.Debug(this.GetType().ToString(), "Get EditAddressParam : " + parameter);
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex.ToString());
                throw new WxPayException(ex.ToString());
            }

            return parameter;
        }
    }
}
