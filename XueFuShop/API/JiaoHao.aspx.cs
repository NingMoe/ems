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
                xmlInit(); //��ʼ��
                nodeName = rootName + "/" + nodeName;
                sortNodeName = rootName + "/" + sortNodeName;
                //ScriptHelper.Alert(xmldoc.ReadInnerText(nodeName + "/Name"));
                if (optionType == 1) //����
                {
                    string userName = RequestHelper.GetForm<string>("UserName").Trim();
                    if (CookiesHelper.ReadCookieValue("zhuan") != "true")
                    {
                        if (!string.IsNullOrEmpty(userName))
                        {
                            ArrayList nameList = ReadNameList();
                            //�ж������Ƿ�Ϸ��Լ��ظ�
                            if (nameList.Contains(userName.ToLower()) && !IsContainsNodeValue(userName))
                            {
                                xmldoc.InsertNode(nodeName, "Name", userName);
                                CookiesHelper.AddCookie("zhuan", "true", 90, TimeType.Minute);
                                //ʱ�����Ϊ0������ʾ������
                                optionType = 0;
                                showTips("[�ѱ���]����11:45������ʱ��");
                            }
                            else
                            {
                                showTips("�ߴ��ˣ���ô����ʶ���أ�");
                            }
                        }
                    }
                    else
                    {
                        //ʱ�����Ϊ0������ʾ������
                        optionType = 0;
                        showTips("����45��ʱ�������ʱ�̰�");
                    }
                }
                else //����
                {
                    //�����δ������ʼ����
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
                showTips("11:30�ֿ�ʼ����");
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
        /// ���������Ա�б�
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
                nameArray.Add("�����");
                nameArray.Add("������");
                nameArray.Add("�л���");
                nameArray.Add("������");
                nameArray.Add("�򾺺�");
                nameArray.Add("������");
                nameArray.Add("Ī�ƽ�");
                nameArray.Add("������");
                nameArray.Add("����");
                nameArray.Add("֣־��");
                nameArray.Add("���γ�");
                nameArray.Add("����");
                nameArray.Add("�ūh��");
                nameArray.Add("Ѧ����");
                nameArray.Add("����");
                nameArray.Add("������");
                nameArray.Add("֣ΰ");
                nameArray.Add("֣��");
                CacheHelper.Write(cacheKey, nameArray);
            }
            return (ArrayList)CacheHelper.Read(cacheKey);
        }

        private void showTips(string content)
        {
            Tips.Text = "<h3 class=\"tips\">" + content + "</h3>";
        }

        /// <summary>
        /// �����������ַ���
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
        /// �жϵ�ǰʱ������
        /// </summary>
        /// <returns>1:����ʱ�䣻2:��ʾ���ʱ��</returns>
        private void m_IsWorkingTime()
        {

            string _strRegAM = "11:30"; //����ʱ��
            string _strStartAM = "11:45"; //���ʱ��
            string _strOverPM = "13:00"; //����ʱ��

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
