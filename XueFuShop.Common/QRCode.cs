using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Web;
using System.IO;
using ThoughtWorks.QRCode.Codec;
using XueFu.EntLib;

namespace XueFuShop.Common
{
    public class QRCode : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private void CreateMemoryStream(string strData, string qrEncoding, string level, int version, int scale)
        {
            Image image = QRCodeHelper.CreateCodeImage(strData, qrEncoding, level, version, scale);
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Gif);
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.Cache.SetNoStore();
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentEncoding = Encoding.Default;
            HttpContext.Current.Response.ContentType = "image/Gif";
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.BinaryWrite(stream.ToArray());
            image.Dispose();
            HttpContext.Current.Response.End();
        }

        public void ProcessRequest(HttpContext Context)
        {
            int scale = RequestHelper.GetQueryString<int>("scale");
            if (scale < 0) scale = 4;
            this.CreateMemoryStream(RequestHelper.GetQueryString<string>("s"), "Byte", "H", 7, scale);
        }
    }
}
