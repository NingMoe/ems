using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Web;

namespace LetvAPI
{
    /// <summary>
    /// ������Core
    /// ���ܣ����ӽӿڹ��ú�����
    /// ��ϸ������������֪ͨ���������ļ������õĹ��ú������Ĵ����ļ�������Ҫ�޸�
    /// �汾��1.0
    /// �޸����ڣ�2014-12-10
    /// ˵����
    /// �ô���������ӽӿ�ʹ�á�
    /// </summary>
    /// 
    public class LetvCore
    {
        #region �ֶ�

        //�������ص�ַ
        private static string _gateway = "";

        //�����ʽ
        private static string _input_charset = "";

        //˽Կ
        private static string _secretkey = "";

        #endregion

        static LetvCore()
        {
            _gateway = LetvConfig.GateWay;
            _input_charset = LetvConfig.Input_Charset;
            _secretkey = LetvConfig.SecretKey;
        }

        // <summary>
        /// ��ȥ������ǩ������������ĸa��z��˳������
        /// </summary>
        /// <param name="dicArrayPre">����ǰ�Ĳ�����</param>
        /// <returns>���˺�Ĳ�����</returns>
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
        /// ����������Ԫ�أ����ա���������ֵ����ģʽ�����κ��ַ�ƴ�ӳ��ַ���
        /// </summary>
        /// <param name="sArray">��Ҫƴ�ӵ�����</param>
        /// <returns>ƴ������Ժ���ַ���</returns>
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
        /// ����������Ԫ�أ����ա�����=����ֵ����ģʽ�á�&���ַ�ƴ�ӳ��ַ��������Բ���ֵ��urlencode
        /// </summary>
        /// <param name="sArray">��Ҫƴ�ӵ�����</param>
        /// <param name="code">�ַ�����</param>
        /// <returns>ƴ������Ժ���ַ���</returns>
        private static string CreateLinkString(Dictionary<string, string> dicArray, Encoding code)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + HttpUtility.UrlEncode(temp.Value, code) + "&");
            }

            //ȥ������һ��&�ַ�
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// д��־��������ԣ�����վ����Ҳ���ԸĳɰѼ�¼�������ݿ⣩
        /// </summary>
        /// <param name="sWord">Ҫд����־����ı�����</param>
        public static void LogResult(string sWord)
        {
            string strPath = HttpContext.Current.Server.MapPath("log");
            strPath = strPath + "\\" + DateTime.Now.ToString().Replace(":", "") + ".txt";
            StreamWriter fs = new StreamWriter(strPath, false, System.Text.Encoding.Default);
            fs.Write(sWord);
            fs.Close();
        }


        /// <summary>
        /// ����Ҫ��������ӵ�ϵͳ��������
        /// </summary>
        /// <param name="sParaTemp">����ǰ�Ĳ�������</param>
        /// <returns>Ҫ����Ĳ�������</returns>
        private static string BuildSignToString(SortedDictionary<string, string> sParaTemp)
        {
            //��ǩ�������������
            Dictionary<string, string> sPara = new Dictionary<string, string>();

            //����ǩ����������
            sPara = FilterPara(sParaTemp);

            //�Ѳ�����������Ԫ��ƴ�ӳ��ַ���
            string strRequestData = CreateLinkString(sPara);

            //����˽Կ
            strRequestData += _secretkey;

            //���ǩ�����
            strRequestData = BuildRequestMysign(strRequestData);

            return strRequestData;
        }

        /// <summary>
        /// ����Ҫ��������ӵĲ�������
        /// </summary>
        /// <param name="sParaTemp">����ǰ�Ĳ�������</param>
        /// <returns>Ҫ����Ĳ��������ַ���</returns>
        public static string BuildRequestParaToString(SortedDictionary<string, string> sParaTemp)
        {
            return BuildRequestParaToString(sParaTemp, Encoding.GetEncoding(_input_charset));
        }

        /// <summary>
        /// ����Ҫ��������ӵĲ�������
        /// </summary>
        /// <param name="sParaTemp">����ǰ�Ĳ�������</param>
        /// <param name="code">�ַ�����</param>
        /// <returns>Ҫ����Ĳ��������ַ���</returns>
        public static string BuildRequestParaToString(SortedDictionary<string, string> sParaTemp, Encoding code)
        {
            //��ǩ�������������
            Dictionary<string, string> sPara = new Dictionary<string, string>();

            //����ǩ����������
            sPara = FilterPara(sParaTemp);

            //�Ѳ�����������Ԫ�أ����ա�����=����ֵ����ģʽ�á�&���ַ�ƴ�ӳ��ַ��������Բ���ֵ��urlencode
            string strRequestData = CreateLinkString(sPara, code);

            strRequestData += "&sign=" + BuildSignToString(sParaTemp);
            strRequestData = _gateway + strRequestData;

            return strRequestData;
        }

        /// <summary>
        /// ��������ʱ��ǩ��
        /// </summary>
        /// <param name="sPara">��������ӵĲ�������</param>
        /// <returns>ǩ�����</returns>
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
        /// ��֤ǩ��
        /// </summary>
        /// <param name="sParaTemp">����Ĳ�������</param>
        /// <param name="sign">ǩ�����</param>
        /// <returns>��֤���</returns>
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
