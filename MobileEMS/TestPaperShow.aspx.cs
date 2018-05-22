using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using XueFuShop.Pages;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFu.EntLib;

namespace MobileEMS
{
    public partial class TestPaperShow : MobileUserBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.ClearCache();
            int productID = RequestHelper.GetQueryString<int>("ProductID");
            decimal score = RequestHelper.GetQueryString<decimal>("Scorse");
            TestSettingInfo testSetting = TestSettingBLL.ReadTestSetting(base.UserCompanyID, productID);

            StringBuilder textOut = new StringBuilder();
            //textOut.AppendLine("<div class=\"exam\">���ο����ܷ֣�" + testSetting.PaperScore + "�֣���ĵ÷��ǣ�<span class=\"active\">" + score + "</span>��</div>");
            string tempOut = string.Empty;
            if (score >= 60 && score < testSetting.LowScore)
            {
                tempOut = "<font color=\"#000\">�ÿ�ϧŶ��ͨ������ " + testSetting.LowScore.ToString() + " �֣��Ͳ�һ����ˣ�<br> ��ϰ���������һ�µ�Ӵ~~</font>";
            }
            else if (score >= testSetting.LowScore)
            {
                tempOut = "<font color=\"#000\">��ϲ��ͨ����~����̫�����ˣ�</font>";
            }
            else
            {
                tempOut = "<font color=\"#000\">�ɼ���̫�����!ͨ������ " + testSetting.LowScore.ToString() + " �֡�<br> Ŭ��һ�£�ѧϰ����������~~</font>";
            }
            textOut.Append("<div id=\"emptyTip\" class=\"pt40\"><p style=\"color:#a6a6a6; line-height:30px; font-weight:bold; font-size:14px;\"><span style='font-size:50px; color:red; font-family:����;'>" + score.ToString() + "</span>��<br/>" + tempOut + "</p>\r\n");

            textOut.Append("<br />���� <a href=\"CourseCenter.aspx?Action=PostCourse\" class=\"green\">�ҵĸ�λ�γ�</a></div>");
            textOut.Append("<div style=\"margin:50px auto;\"><table style=\"width:200px; margin:0px auto;\"><tr><td style=\"padding-right:10px; text-align:right;\">�Ϻ��Ӻ����޷���<br>�Ϻ����ع�����ѯ</td><td style=\" border-left:1px solid #CCC; text-align: left; padding-left:10px;\">����<br>��Ʒ</td></tr></table></div>");
            paperContent.InnerHtml = textOut.ToString();
        }
    }
}
