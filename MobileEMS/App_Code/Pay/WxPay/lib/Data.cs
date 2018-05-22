using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Security.Cryptography;
using System.Text;
using LitJson;

namespace WxPayAPI
{
    /// <summary>
    /// ΢��֧��Э��ӿ������࣬���е�API�ӿ�ͨ�Ŷ�����������ݽṹ��
    /// �ڵ��ýӿ�֮ǰ���������ֶε�ֵ��Ȼ����нӿ�ͨ�ţ�
    /// ������Ƶĺô��ǿ���չ��ǿ���û��������Э����и��Ķ���������������ݽṹ��
    /// ������������ϳ���ͬ��Э�����ݰ�������Ϊÿ��Э�����һ�����ݰ��ṹ
    /// </summary>
    public class WxPayData
    {
        public WxPayData()
        {

        }

        //���������Dictionary�ĺô��Ƿ�������ݰ�����ǩ����������ǩ��֮ǰ����һ������
        private SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();

        /**
        * ����ĳ���ֶε�ֵ
        * @param key �ֶ���
         * @param value �ֶ�ֵ
        */
        public void SetValue(string key, object value)
        {
            m_values[key] = value;
        }

        /**
        * �����ֶ�����ȡĳ���ֶε�ֵ
        * @param key �ֶ���
         * @return key��Ӧ���ֶ�ֵ
        */
        public object GetValue(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            return o;
        }

        /**
         * �ж�ĳ���ֶ��Ƿ�������
         * @param key �ֶ���
         * @return ���ֶ�key�ѱ����ã��򷵻�true�����򷵻�false
         */
        public bool IsSet(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            if (null != o)
                return true;
            else
                return false;
        }

        /**
        * @��Dictionaryת��xml
        * @return ��ת���õ���xml��
        * @throws WxPayException
        **/
        public string ToXml()
        {
            //����Ϊ��ʱ����ת��Ϊxml��ʽ
            if (0 == m_values.Count)
            {
                Log.Error(this.GetType().ToString(), "WxPayData����Ϊ��!");
                throw new WxPayException("WxPayData����Ϊ��!");
            }

            string xml = "<xml>";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                //�ֶ�ֵ����Ϊnull����Ӱ���������
                if (pair.Value == null)
                {
                    Log.Error(this.GetType().ToString(), "WxPayData�ڲ�����ֵΪnull���ֶ�!");
                    throw new WxPayException("WxPayData�ڲ�����ֵΪnull���ֶ�!");
                }

                if (pair.Value.GetType() == typeof(int))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else if (pair.Value.GetType() == typeof(string))
                {
                    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                }
                else//����string��int���Ͳ��ܺ���������������
                {
                    Log.Error(this.GetType().ToString(), "WxPayData�ֶ��������ʹ���!");
                    throw new WxPayException("WxPayData�ֶ��������ʹ���!");
                }
            }
            xml += "</xml>";
            return xml;
        }

        /**
        * @��xmlתΪWxPayData���󲢷��ض����ڲ�������
        * @param string ��ת����xml��
        * @return ��ת���õ���Dictionary
        * @throws WxPayException
        */
        public SortedDictionary<string, object> FromXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                Log.Error(this.GetType().ToString(), "���յ�xml��ת��ΪWxPayData���Ϸ�!");
                throw new WxPayException("���յ�xml��ת��ΪWxPayData���Ϸ�!");
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;//��ȡ�����ڵ�<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                m_values[xe.Name] = xe.InnerText;//��ȡxml�ļ�ֵ�Ե�WxPayData�ڲ���������
            }

            try
            {
                //2015-06-29 ������û��ǩ��
                if (m_values["return_code"] != "SUCCESS")
                {
                    return m_values;
                }
                CheckSign();//��֤ǩ��,��ͨ�������쳣
            }
            catch (WxPayException ex)
            {
                throw new WxPayException(ex.Message);
            }

            return m_values;
        }

        /**
        * @Dictionary��ʽת����url������ʽ
        * @ return url��ʽ��, �ô�������sign�ֶ�ֵ
        */
        public string ToUrl()
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                if (pair.Value == null)
                {
                    Log.Error(this.GetType().ToString(), "WxPayData�ڲ�����ֵΪnull���ֶ�!");
                    throw new WxPayException("WxPayData�ڲ�����ֵΪnull���ֶ�!");
                }

                if (pair.Key != "sign" && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');
            return buff;
        }


        /**
        * @Dictionary��ʽ����Json
         * @return json������
        */
        public string ToJson()
        {
            string jsonStr = JsonMapper.ToJson(m_values);
            return jsonStr;
        }

        /**
        * @values��ʽ��������Webҳ������ʾ�Ľ������Ϊwebҳ���ϲ���ֱ�����xml��ʽ���ַ�����
        */
        public string ToPrintStr()
        {
            string str = "";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                if (pair.Value == null)
                {
                    Log.Error(this.GetType().ToString(), "WxPayData�ڲ�����ֵΪnull���ֶ�!");
                    throw new WxPayException("WxPayData�ڲ�����ֵΪnull���ֶ�!");
                }

                str += string.Format("{0}={1}<br>", pair.Key, pair.Value.ToString());
            }
            Log.Debug(this.GetType().ToString(), "Print in Web Page : " + str);
            return str;
        }

        /**
        * @����ǩ�������ǩ�������㷨
        * @return ǩ��, sign�ֶβ��μ�ǩ��
        */
        public string MakeSign()
        {
            //תurl��ʽ
            string str = ToUrl();
            //��string�����API KEY
            str += "&key=" + WxPayConfig.KEY;
            //MD5����
            MD5 md5 = MD5.Create();
            byte[] bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //�����ַ�תΪ��д
            return sb.ToString().ToUpper();
        }

        /**
        * 
        * ���ǩ���Ƿ���ȷ
        * ��ȷ����true���������쳣
        */
        public bool CheckSign()
        {
            //���û������ǩ�������������
            if (!IsSet("sign"))
            {
                Log.Error(this.GetType().ToString(), "WxPayDataǩ�����ڵ����Ϸ�!");
                throw new WxPayException("WxPayDataǩ�����ڵ����Ϸ�!");
            }
            //���������ǩ������ǩ��Ϊ�գ������쳣
            else if (GetValue("sign") == null || GetValue("sign").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "WxPayDataǩ�����ڵ����Ϸ�!");
                throw new WxPayException("WxPayDataǩ�����ڵ����Ϸ�!");
            }

            //��ȡ���յ���ǩ��
            string return_sign = GetValue("sign").ToString();

            //�ڱ��ؼ����µ�ǩ��
            string cal_sign = MakeSign();

            if (cal_sign == return_sign)
            {
                return true;
            }

            Log.Error(this.GetType().ToString(), "WxPayDataǩ����֤����!");
            throw new WxPayException("WxPayDataǩ����֤����!");
        }

        /**
        * @��ȡDictionary
        */
        public SortedDictionary<string, object> GetValues()
        {
            return m_values;
        }
    }
}
