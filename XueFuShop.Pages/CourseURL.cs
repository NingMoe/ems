using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace XueFuShop.Pages
{
    public class CourseURL : UserCommonBasePage
    {
        protected override void PageLoad()
        {
            string url = Request.RawUrl;       //Ҫ��ȡ����ҳ��ַ  

            url = Regex.Split(url, "Url=")[1];
            if (url.Contains("$$")) url = url.Replace("$$", "|");
            else url = url.Replace("%24%24", "|");
            string[] Url = url.Split('|');

            StringBuilder Introduct = new StringBuilder();
            Introduct.Append("<style>ul li{list-style:none; padding:0px;margin:0px; line-height:25px; padding-left:20px;}</style>");
            Introduct.Append("<FIELDSET style=\"padding:20px; FONT-SIZE: 13px;FONT-FAMILY: ΢���ź�, ����\"><LEGEND style=\"FONT-SIZE: 16px; \">����˵��</LEGEND>");
            Introduct.Append("<DIV id=intro style=\"margin-top:20px;\">");
            Introduct.Append("<UL>1�����ϴ�Ҫע����Щ���飿");
            Introduct.Append("<LI>���ˡ���������ר�á�����15�������������ȷ��������������³���������&�ۺ��������ܡ�����Ʒ��Ŀ¼���������Ĳ��϶��Ǽ��ܵģ������U��ʹ�á�</LI>");
            Introduct.Append("<LI>���û��U�ܣ�������˾����Ա�����á� </LI>");
            Introduct.Append("</UL>");
            Introduct.Append("<UL>2���μ������ϴ��С���Ƶ�������ģ���Ϊ��Ƶ�μ������ڿ������߹ۿ����������أ�ֻ�����ı����ϼ��ɡ�");
            Introduct.Append("</UL>");
            Introduct.Append("<UL>3��U�����ʹ�ã� ");
            Introduct.Append("<LI>���Ƕ�U�ܵİ�װ��������װ���������ʹ���Լ��ĵ��Ĵ򿪷��������˽��ܣ���ο���<A href=\"http://www.mostool.com/uploads/u/�ܶܣ�U�ܣ�ʹ�÷���.pdf\">�ܶܣ�U�ܣ�ʹ�÷���</A> </LI></UL>");
            Introduct.Append("<UL>4�����������˵�����в����֮��������ͨ�����·�ʽ��ϵ���ǣ���ʱ������ķ��գ� ");
            Introduct.Append("<LI>�绰��021-50620208 50620185 </LI>");
            Introduct.Append("<LI>Email��<A href=\"mailto:webmaster@mentoronsite.com\">webmaster@mentoronsite.com</A> </LI>");
            Introduct.Append("<LI>�ͷ�QQ��<A href=\"http://wpa.qq.com/msgrd?v=3&amp;uin=800052251&amp;site=qq&amp;menu=yes\" target=_blank>800052251</A> </LI></UL></DIV></FIELDSET> ");
            Response.Write(Introduct.ToString());
            foreach (string URL in Url)
            {
                try
                {
                    WebRequest req = WebRequest.Create(URL.Replace("mentoronsite", "mostool").Replace("anli_text.asp", "ems.asp"));
                    req.Method = "Get";
                    WebResponse res = req.GetResponse();
                    Stream resStream = res.GetResponseStream();
                    StreamReader sr = new StreamReader(resStream, Encoding.UTF8);
                    string contentHTML = sr.ReadToEnd();    //��ȡ��ҳ��Դ����    
                    contentHTML = contentHTML.Replace("href=\"", "href=\"http://mostool.com");
                    contentHTML = contentHTML.Replace("src=\"", "src=\"http://mostool.com");
                    Response.Write(contentHTML);
                    resStream.Close();
                    sr.Close();
                }
                catch
                {
                    Response.Write("���ص�ַ����");
                }
            }
        }
    }
}
