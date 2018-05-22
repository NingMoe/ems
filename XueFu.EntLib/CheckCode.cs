using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace XueFu.EntLib
{
    public enum CodeType
    {
        Letter = 2,
        Mixed = 3,
        Number = 1
    }

    public class CheckCode : IHttpHandler, IRequiresSessionState
    {

        private static string checkCode = string.Empty;
        private static int codeDot = 6;
        private static int codeLength = 4;
        private static CodeType codeType = CodeType.Number;
        private static string cookiesName = "CheckCode";
        private static string key = string.Empty;


        public void MakeCode()
        {
            string str = string.Empty;
            string str2 = "0,1,2,3,4,5,6,7,8,9,0";
            string str3 = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string str4 = str2;
            switch (codeType)
            {
                case CodeType.Number:
                    str4 = str2;
                    break;

                case CodeType.Letter:
                    str4 = str3;
                    break;

                case CodeType.Mixed:
                    str4 = str2 + str3;
                    break;
            }
            string[] strArray = str4.Split(new char[] { ',' });
            Random random = new Random();
            for (int i = 1; i <= codeLength; i++)
            {
                str = str + strArray[random.Next(strArray.Length)];
            }
            checkCode = str.ToUpper();
        }

        private void MakeImg()
        {
            int num3;
            int width = checkCode.Length * 13;
            int height = 20;
            Bitmap image = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(image);
            Rectangle rect = new Rectangle(0, 0, width, height);
            graphics.FillRectangle(new SolidBrush(Color.White), rect);
            Color[] colorArray = new Color[] { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Brown, Color.Purple };
            Color[] colorArray2 = new Color[] { Color.LightBlue, Color.LightCoral, Color.LightCyan, Color.LightGoldenrodYellow, Color.LightGray, Color.LightGreen, Color.LightPink, Color.LightSalmon, Color.LightSeaGreen, Color.LightSkyBlue, Color.LightYellow };
            Random random = new Random();
            Pen pen = new Pen(Color.LightBlue, 0f);
            for (num3 = 0; num3 < CodeDot; num3++)
            {
                //pen = new Pen(colorArray2[random.Next(11)], 5f);
                pen = new Pen(Color.Black, 2f);
                Point point = new Point(0, random.Next(height / 5, height * 4 / 5));
                Point point2 = new Point(random.Next(width), random.Next(height));
                Point point3 = new Point(random.Next(width), random.Next(height));
                Point point4 = new Point(width, random.Next(height / 5, height * 4 / 5));
                graphics.DrawBezier(pen, point, point2, point3, point4);
            }
            int num4 = 0;
            for (num3 = 0; num3 < checkCode.Length; num3++)
            {
                //graphics.DrawString(checkCode.Substring(num3, 1), new Font("Arial", 12f, FontStyle.Bold), new SolidBrush(colorArray[random.Next(6)]), (float)num4, 2f);
                graphics.DrawString(checkCode.Substring(num3, 1), new Font("Arial", 12f, FontStyle.Bold), new SolidBrush(Color.Black), (float)num4, 2f);
                num4 += 12;
            }
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Gif);
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.Cache.SetNoStore();
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentEncoding = Encoding.Default;
            HttpContext.Current.Response.ContentType = "image/Gif";
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.BinaryWrite(stream.ToArray());
            graphics.Dispose();
            image.Dispose();
            CookiesHelper.AddCookie(cookiesName, StringHelper.Encode(checkCode, key));
            HttpContext.Current.Response.End();
        }

        public void ProcessRequest(HttpContext Context)
        {
            this.MakeCode();
            this.MakeImg();
        }


        public static int CodeDot
        {
            get
            {
                return codeDot;
            }
            set
            {
                codeDot = value;
            }
        }

        public static int CodeLength
        {
            get
            {
                return codeLength;
            }
            set
            {
                codeLength = value;
            }
        }

        public static CodeType CodeType
        {
            get
            {
                return codeType;
            }
            set
            {
                codeType = value;
            }
        }

        public static string CookiesName
        {
            get
            {
                return cookiesName;
            }
            set
            {
                cookiesName = value;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public static string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }

        /// <summary>
        /// ∂¡»°—È÷§¬Î
        /// </summary>
        public string Code
        {
            get { return checkCode; }
        }
    }
}
