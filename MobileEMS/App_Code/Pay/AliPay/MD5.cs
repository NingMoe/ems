using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace MobileEMS.Pay.AliPay
{
    /// <summary>
    /// ������MD5
    /// ���ܣ�MD5����
    /// �汾��3.3
    /// �޸����ڣ�2012-07-05
    /// ˵����
    /// ���´���ֻ��Ϊ�˷����̻����Զ��ṩ���������룬�̻����Ը����Լ���վ����Ҫ�����ռ����ĵ���д,����һ��Ҫʹ�øô��롣
    /// �ô������ѧϰ���о�֧�����ӿ�ʹ�ã�ֻ���ṩһ���ο���
    /// </summary>
    public sealed class AlipayMD5
    {
        public AlipayMD5()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        /// <summary>
        /// ǩ���ַ���
        /// </summary>
        /// <param name="prestr">��Ҫǩ�����ַ���</param>
        /// <param name="key">��Կ</param>
        /// <param name="_input_charset">�����ʽ</param>
        /// <returns>ǩ�����</returns>
        public static string Sign(string prestr, string key, string _input_charset)
        {
            StringBuilder sb = new StringBuilder(32);

            prestr = prestr + key;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }

        /// <summary>
        /// ��֤ǩ��
        /// </summary>
        /// <param name="prestr">��Ҫǩ�����ַ���</param>
        /// <param name="sign">ǩ�����</param>
        /// <param name="key">��Կ</param>
        /// <param name="_input_charset">�����ʽ</param>
        /// <returns>��֤���</returns>
        public static bool Verify(string prestr, string sign, string key, string _input_charset)
        {
            string mysign = Sign(prestr, key, _input_charset);
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