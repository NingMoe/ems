using System.Web;

namespace XueFu.EntLib
{
    public sealed class ResponseHelper
    {
        public static void End()
        {
            HttpContext.Current.Response.End();
        }

        public static void Redirect(string url)
        {
            HttpContext.Current.Response.Redirect(url);
        }

        public static void Write(string content)
        {
            HttpContext.Current.Response.Write(content);
        }
    }
}
