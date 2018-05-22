using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace XueFu.EntLib
{
    public class TagComposite : BaseTag
    {
        
        private List<BaseTag> baseTagList = new List<BaseTag>();

        
        public void AddTag(BaseTag baseTag)
        {
            this.baseTagList.Add(baseTag);
        }

        public override void TagHandler(ref string content)
        {
            foreach (BaseTag tag in this.baseTagList)
            {
                tag.TagHandler(ref content);
            }
        }
    }

    public abstract class BaseTag
    {
        
        protected BaseTag()
        {
        }

        public abstract void TagHandler(ref string content);
    }

    public class CsharpTag : BaseTag
    {
        
        private Regex rg = new Regex(@"<html:csharp>([\s\S]+?)</html:csharp>", RegexOptions.None);

        
        public override void TagHandler(ref string content)
        {
            foreach (Match match in this.rg.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%" + match.Groups[1].ToString() + "%>");
            }
        }
    }




    public class SetTag : BaseTag
    {
        
        private Regex rg = new Regex(@"<\$([\s\S]+?)\$>", RegexOptions.None);

        
        public override void TagHandler(ref string content)
        {
            foreach (Match match in this.rg.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%=" + match.Groups[1].ToString() + "%>");
            }
        }
    }



    public class ForeachTag : BaseTag
    {
        
        private Regex rg1 = new Regex("<html:foreach expression=\"([\\s\\S]+?)\">", RegexOptions.None);
        private Regex rg2 = new Regex("</html:foreach>", RegexOptions.None);

        
        public override void TagHandler(ref string content)
        {
            foreach (Match match in this.rg1.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%foreach(" + match.Groups[1].ToString() + ")\r\n{%>");
            }
            foreach (Match match in this.rg2.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%}%>");
            }
        }
    }




    public class IfTag : BaseTag
    {
        
        private Regex rg1 = new Regex("<html:if expression=\"([\\s\\S]+?)\">", RegexOptions.None);
        private Regex rg2 = new Regex("<html:elseif expression=\"([\\s\\S]+?)\">", RegexOptions.None);
        private Regex rg3 = new Regex(@"<html:else>([\s\S]+?)", RegexOptions.None);
        private Regex rg4 = new Regex("</html:if>", RegexOptions.None);

        
        public override void TagHandler(ref string content)
        {
            foreach (Match match in this.rg1.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%if(" + match.Groups[1].ToString() + ")\r\n{%>");
            }
            foreach (Match match in this.rg2.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%}\r\nelse if(" + match.Groups[1].ToString() + ")\r\n{%>");
            }
            foreach (Match match in this.rg3.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%}\r\nelse\r\n{%>" + match.Groups[1].ToString());
            }
            foreach (Match match in this.rg4.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<% }%>");
            }
        }
    }



    public class SwitchTag : BaseTag
    {
        
        private Regex rg1 = new Regex("<html:switch name=\"([\\s\\S]+?)\">", RegexOptions.None);
        private Regex rg2 = new Regex("<html:case value=\"([\\s\\S]+?)\">", RegexOptions.None);
        private Regex rg3 = new Regex("<html:default>", RegexOptions.None);
        private Regex rg4 = new Regex("</html:swith>", RegexOptions.None);

        
        public override void TagHandler(ref string content)
        {
            foreach (Match match in this.rg1.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%switch(" + match.Groups[1].ToString() + ")\r\n{%>");
            }
            foreach (Match match in this.rg2.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%case " + match.Groups[1].ToString() + ":%>");
            }
            foreach (Match match in this.rg3.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%default:%>");
            }
            foreach (Match match in this.rg4.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%\r\n }%>");
            }
        }
    }




    public class BreakTag : BaseTag
    {
        
        private Regex rg = new Regex(@"<html:break[\S]*?>", RegexOptions.None);

        
        public override void TagHandler(ref string content)
        {
            foreach (Match match in this.rg.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%break;%>");
            }
        }
    }



    public class WhileTag : BaseTag
    {
        
        private Regex rg1 = new Regex("<html:while expression=\"([\\s\\S]+?)\">", RegexOptions.None);
        private Regex rg2 = new Regex("</html:while>", RegexOptions.None);

        
        public override void TagHandler(ref string content)
        {
            foreach (Match match in this.rg1.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%while(" + match.Groups[1].ToString() + ")\r\n{%>");
            }
            foreach (Match match in this.rg2.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<% }%>");
            }
        }
    }




    public class ForTag : BaseTag
    {
        
        private Regex rg1 = new Regex("<html:for init=\"([\\s\\S]+?)\" condtion=\"([\\s\\S]+?)\" expression=\"([\\s\\S]+?)\">", RegexOptions.None);
        private Regex rg2 = new Regex("</html:for>", RegexOptions.None);

        
        public override void TagHandler(ref string content)
        {
            foreach (Match match in this.rg1.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%for(" + match.Groups[1].ToString() + ";" + match.Groups[2].ToString() + ";" + match.Groups[3].ToString() + ")\r\n{%>");
            }
            foreach (Match match in this.rg2.Matches(content))
            {
                content = content.Replace(match.Groups[0].ToString(), "<%}%>");
            }
        }
    }

 




}
