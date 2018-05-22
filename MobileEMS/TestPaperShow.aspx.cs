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
            //textOut.AppendLine("<div class=\"exam\">本次考试总分：" + testSetting.PaperScore + "分，你的得分是：<span class=\"active\">" + score + "</span>分</div>");
            string tempOut = string.Empty;
            if (score >= 60 && score < testSetting.LowScore)
            {
                tempOut = "<font color=\"#000\">好可惜哦，通过线是 " + testSetting.LowScore.ToString() + " 分，就差一点点了！<br> 复习后可以再试一下的哟~~</font>";
            }
            else if (score >= testSetting.LowScore)
            {
                tempOut = "<font color=\"#000\">恭喜你通过了~，你太厉害了！</font>";
            }
            else
            {
                tempOut = "<font color=\"#000\">成绩不太理想喔!通过线是 " + testSetting.LowScore.ToString() + " 分。<br> 努力一下，学习后来征服它~~</font>";
            }
            textOut.Append("<div id=\"emptyTip\" class=\"pt40\"><p style=\"color:#a6a6a6; line-height:30px; font-weight:bold; font-size:14px;\"><span style='font-size:50px; color:red; font-family:黑体;'>" + score.ToString() + "</span>分<br/>" + tempOut + "</p>\r\n");

            textOut.Append("<br />返回 <a href=\"CourseCenter.aspx?Action=PostCourse\" class=\"green\">我的岗位课程</a></div>");
            textOut.Append("<div style=\"margin:50px auto;\"><table style=\"width:200px; margin:0px auto;\"><tr><td style=\"padding-right:10px; text-align:right;\">上海加禾汽修服务<br>上海孟特管理咨询</td><td style=\" border-left:1px solid #CCC; text-align: left; padding-left:10px;\">联合<br>出品</td></tr></table></div>");
            paperContent.InnerHtml = textOut.ToString();
        }
    }
}
