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
            string url = Request.RawUrl;       //要获取的网页地址  

            url = Regex.Split(url, "Url=")[1];
            if (url.Contains("$$")) url = url.Replace("$$", "|");
            else url = url.Replace("%24%24", "|");
            string[] Url = url.Split('|');

            StringBuilder Introduct = new StringBuilder();
            Introduct.Append("<style>ul li{list-style:none; padding:0px;margin:0px; line-height:25px; padding-left:20px;}</style>");
            Introduct.Append("<FIELDSET style=\"padding:20px; FONT-SIZE: 13px;FONT-FAMILY: 微软雅黑, 宋体\"><LEGEND style=\"FONT-SIZE: 16px; \">下载说明</LEGEND>");
            Introduct.Append("<DIV id=intro style=\"margin-top:20px;\">");
            Introduct.Append("<UL>1、材料打开要注意哪些事情？");
            Introduct.Append("<LI>除了《车企人物专访》、《15大厂汽车销量环比分析》、《厂家新车广宣促销&售后活动促销汇总》、《品牌目录》外其他的材料都是加密的，请配合U盾使用。</LI>");
            Introduct.Append("<LI>如果没有U盾，请至公司管理员处借用。 </LI>");
            Introduct.Append("</UL>");
            Introduct.Append("<UL>2、课件名称上带有“视频”字样的，均为视频课件，现在可以在线观看，不需下载，只下载文本材料即可。");
            Introduct.Append("</UL>");
            Introduct.Append("<UL>3、U盾如何使用？ ");
            Introduct.Append("<LI>我们对U盾的安装环境、安装方法、如何使用以及文档的打开方法都做了介绍，请参考：<A href=\"http://www.mostool.com/uploads/u/密盾（U盾）使用方法.pdf\">密盾（U盾）使用方法</A> </LI></UL>");
            Introduct.Append("<UL>4、如果对以上说明还有不清楚之处，请您通过以下方式联系我们，及时解决您的烦恼： ");
            Introduct.Append("<LI>电话：021-50620208 50620185 </LI>");
            Introduct.Append("<LI>Email：<A href=\"mailto:webmaster@mentoronsite.com\">webmaster@mentoronsite.com</A> </LI>");
            Introduct.Append("<LI>客服QQ：<A href=\"http://wpa.qq.com/msgrd?v=3&amp;uin=800052251&amp;site=qq&amp;menu=yes\" target=_blank>800052251</A> </LI></UL></DIV></FIELDSET> ");
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
                    string contentHTML = sr.ReadToEnd();    //读取网页的源代码    
                    contentHTML = contentHTML.Replace("href=\"", "href=\"http://mostool.com");
                    contentHTML = contentHTML.Replace("src=\"", "src=\"http://mostool.com");
                    Response.Write(contentHTML);
                    resStream.Close();
                    sr.Close();
                }
                catch
                {
                    Response.Write("下载地址出错！");
                }
            }
        }
    }
}
