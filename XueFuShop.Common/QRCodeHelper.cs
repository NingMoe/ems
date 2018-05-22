using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System.IO;
using System.Web;
using System.Drawing.Imaging;
using XueFu.EntLib;

namespace XueFuShop.Common
{
    public class QRCodeHelper
    {

        public static string CreateCode(string strData, int scale)
        {
            return CreateCode(strData, "Byte", "H", 7, scale);
        }

        /// ���ɶ�ά��
        /// </summary>
        /// <param name="strData">Ҫ���ɵ����ֻ������֣�֧�����ġ��磺 "4408810820 ���ڣ�����" ���ߣ�4444444444</param>
        /// <param name="qrEncoding">���ֳߴ磺BYTE ��ALPHA_NUMERIC��NUMERIC</param>
        /// <param name="level">��С��L M Q H</param>
        /// <param name="version">�汾���� 8</param>
        /// <param name="scale">�������� 4</param>
        /// <returns></returns>
        public static string CreateCode(string strData, string qrEncoding, string level, int version, int scale)
        {
            Image image = CreateCodeImage(strData, qrEncoding, level, version, scale);
            string filename = DateTime.Now.ToString("yyyymmddhhmmssfff").ToString() + ".jpg";
            string filepath = ServerHelper.MapPath(@"~\Upload\QRCode");
            //����ļ��в����ڣ��򴴽�
            if (!Directory.Exists(filepath))
                Directory.CreateDirectory(filepath);
            filepath = filepath + "\\" + filename;
            System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
            fs.Close();
            image.Dispose();
            return (@"/Upload/QRCode/" + filename);
        }

        public static Image CreateCodeImage(string strData, string qrEncoding, string level, int version, int scale)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            string encoding = qrEncoding;
            switch (encoding)
            {
                case "Byte":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
                case "AlphaNumeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                    break;
                case "Numeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
                    break;
                default:
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
            }

            qrCodeEncoder.QRCodeScale = scale;
            qrCodeEncoder.QRCodeVersion = version;
            switch (level)
            {
                case "L":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                    break;
                case "M":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                    break;
                case "Q":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                    break;
                default:
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
            }
            //��������ͼƬ
            return qrCodeEncoder.Encode(strData);
        }
    }
}
