using System;
using System.Data;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections;
using XueFu.EntLib;
using XueFuShop.Common;

namespace XueFuShop
{
    public partial class JiaoHao : System.Web.UI.Page
    {
        protected int optionType = 0;
        protected string resultNameList = string.Empty;

        private XmlHelper xmldoc = new XmlHelper();
        private string rootName = "NameList";
        private string nodeName = "BaseNameList", sortNodeName = "ResultNameList";
        private string currentDate = DateTime.Now.Date.ToString("yyyy-M-d");
        private string path = ServerHelper.MapPath("~/xml/zhuan.xml");

        protected void Page_Load(object sender, EventArgs e)
        {
            m_IsWorkingTime();

            if (optionType == 1 || optionType == 2)
            {
                xmlInit(); //初始化
                nodeName = rootName + "/" + nodeName;
                sortNodeName = rootName + "/" + sortNodeName;
                //ScriptHelper.Alert(xmldoc.ReadInnerText(nodeName + "/Name"));
                if (optionType == 1) //报名
                {
                    string userName = RequestHelper.GetForm<string>("UserName").Trim();
                    if (CookiesHelper.ReadCookieValue("zhuan") != "true")
                    {
                        if (!string.IsNullOrEmpty(userName))
                        {
                            ArrayList nameList = ReadNameList();
                            //判断名字是否合法以及重复
                            if (nameList.Contains(userName.ToLower()) && !IsContainsNodeValue(userName))
                            {
                                xmldoc.InsertNode(nodeName, "Name", userName);
                                CookiesHelper.AddCookie("zhuan", "true", 90, TimeType.Minute);
                                //时间段设为0，不显示报名表单
                                optionType = 0;
                                showTips("[已报名]静待11:45的奇妙时刻");
                            }
                            else
                            {
                                showTips("走错了？怎么不认识你呢！");
                            }
                        }
                    }
                    else
                    {
                        //时间段设为0，不显示报名表单
                        optionType = 0;
                        showTips("静待45分时那奇妙的时刻吧");
                    }
                }
                else //排序
                {
                    //如果还未排序，则开始排序
                    if (xmldoc.ReadChildNodes(sortNodeName).Count <= 0)
                    {
                        XmlNodeList nodeList = xmldoc.ReadChildNodes(nodeName);
                        string[] nameArray = new string[nodeList.Count];
                        if (nodeList != null)
                        {
                            for (int i = 0; i < nodeList.Count; i++)
                            {
                                nameArray[i] = nodeList[i].InnerText;
                            }
                        }

                        nameArray = RandomHelper.GetRandomOptionArray<string>(nameArray);

                        for (int i = 0; i < nameArray.Length; i++)
                        {
                            xmldoc.InsertNode(sortNodeName, "Name", nameArray[i]);
                        }
                    }
                    resultNameList = showResult();
                }
                xmldoc.Save(path);
            }
            else
            {
                showTips("11:30分开始报名");
            }
        }

        private bool IsContainsNodeValue(string value)
        {
            XmlNodeList nodeList = xmldoc.ReadChildNodes(nodeName);
            if (nodeList != null)
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    if (nodeList[i].InnerText.ToLower() == value.ToLower())
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 创建抽号人员列表
        /// </summary>
        /// <returns></returns>
        private ArrayList ReadNameList()
        {
            string cacheKey = "RealNameList";
            if (CacheHelper.Read(cacheKey) == null)
            {
                ArrayList nameArray = new ArrayList(29);
                nameArray.Add("lanny");
                nameArray.Add("lisa");
                nameArray.Add("joyce");
                nameArray.Add("joan");
                nameArray.Add("vivi");
                nameArray.Add("amy");
                nameArray.Add("kevin");
                nameArray.Add("ricky");
                nameArray.Add("吴佳兰");
                nameArray.Add("汤莉琼");
                nameArray.Add("尚慧敏");
                nameArray.Add("张晓青");
                nameArray.Add("向竞衡");
                nameArray.Add("郭佳音");
                nameArray.Add("莫云杰");
                nameArray.Add("翁锡羊");
                nameArray.Add("傅晶");
                nameArray.Add("郑志杰");
                nameArray.Add("徐鑫超");
                nameArray.Add("高扬");
                nameArray.Add("张玥珉");
                nameArray.Add("薛长富");
                nameArray.Add("杨勇");
                nameArray.Add("陈烨文");
                nameArray.Add("郑伟");
                nameArray.Add("郑佳");
                CacheHelper.Write(cacheKey, nameArray);
            }
            return (ArrayList)CacheHelper.Read(cacheKey);
        }

        private void showTips(string content)
        {
            Tips.Text = "<h3 class=\"tips\">" + content + "</h3>";
        }

        /// <summary>
        /// 生成排序结果字符串
        /// </summary>
        /// <returns></returns>
        private string showResult()
        {
            StringBuilder resultHtml = new StringBuilder();
            XmlNodeList sortNodeList = xmldoc.ReadChildNodes(sortNodeName);
            if (sortNodeList != null)
            {
                for (int i = 0; i < sortNodeList.Count; i++)
                {
                    resultHtml.Append("<dd><span>" + (i + 1).ToString() + "/</span>" + sortNodeList[i].InnerText + "</dd>");
                }
            }
            return resultHtml.ToString();
        }

        private void xmlInit()
        {
            if (File.Exists(path))
            {
                xmldoc = new XmlHelper(path);
                if (currentDate != xmldoc.ReadAttribute(rootName, "Date"))
                {
                    xmldoc.UpdateAttribute(rootName, "Date", currentDate);
                    if (xmldoc.ReadChildNodes(rootName + "/" + nodeName).Count > 0) xmldoc.DeleteNodes(rootName + "/" + nodeName + "/Name");
                    if (xmldoc.ReadChildNodes(rootName + "/" + sortNodeName).Count > 0) xmldoc.DeleteNodes(rootName + "/" + sortNodeName + "/Name");
                }
            }
            else
            {
                xmldoc.InsertNode(rootName);
                xmldoc.InsertElement(rootName, "Date", currentDate);

                xmldoc.InsertNode(rootName, nodeName, "");
                xmldoc.InsertNode(rootName, sortNodeName, "");
               // xmldoc.Save(path);
            }
        }

        /// <summary>
        /// 判断当前时间类型
        /// </summary>
        /// <returns>1:报名时间；2:显示结果时间</returns>
        private void m_IsWorkingTime()
        {

            string _strRegAM = "11:30"; //报名时间
            string _strStartAM = "11:45"; //抽号时间
            string _strOverPM = "13:00"; //结束时间

            TimeSpan dspRegAM = DateTime.Parse(_strRegAM).TimeOfDay;
            TimeSpan dspStartAM = DateTime.Parse(_strStartAM).TimeOfDay;
            TimeSpan dspOverPM = DateTime.Parse(_strOverPM).TimeOfDay;

            TimeSpan dspNow = DateTime.Now.TimeOfDay;
            if (dspNow > dspRegAM && dspNow < dspStartAM)
            {
                optionType = 1;
            }
            else if (dspNow > dspStartAM && dspNow < dspOverPM)
            {
                optionType = 2;
            }
        }
    }
}
