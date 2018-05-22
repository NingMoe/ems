using System;
using XueFuShop.Pages;
using XueFu.EntLib;
using XueFuShop.Models;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace XueFuShop.Admin
{
    public partial class PostPlanReport2 : AdminBasePage
    {
        protected string postIdCondition = RequestHelper.GetQueryString<string>("PostIdCondition");
        protected string studyPostIdCondition = RequestHelper.GetQueryString<string>("StudyPostIdCondition");
        protected int companyID = RequestHelper.GetQueryString<int>("CompanyId");
        protected DateTime startDate = DateTime.MinValue;
        protected DateTime endDate = DateTime.MinValue;
        protected ArrayList groupResult = new ArrayList();
        protected CompanyInfo company = new CompanyInfo();
        protected string companyName = string.Empty;
        protected string groupID = RequestHelper.GetQueryString<string>("GroupID");
        protected string action = RequestHelper.GetQueryString<string>("Action");
        protected string state = RequestHelper.GetQueryString<string>("State");
        private int WeekNum = 0, ColNum = 11, TotalPeoperNum = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //设置默认值
                if (string.IsNullOrEmpty(action))
                {
                    groupID = "36";
                    state = ((int)UserState.Normal).ToString();
                }

                if (!string.IsNullOrEmpty(action) && companyID > 0)
                {
                    ShowArea.Style.Add("display", "");
                    company = CompanyBLL.ReadCompany(companyID);
                    companyName = company.CompanyName;
                    startDate = Convert.ToDateTime(RequestHelper.GetQueryString<string>("SearchStartDate"));
                    endDate = Convert.ToDateTime(RequestHelper.GetQueryString<string>("SearchEndDate"));
                    SearchStartDate.Text = startDate.ToString("d");
                    SearchEndDate.Text = endDate.ToString("d");
                    endDate = ShopCommon.SearchEndDate(endDate);

                    //计算周数
                    WeekNum = (endDate - startDate).Days / 7;
                    if ((endDate - startDate).Days % 7 > 0)
                    {
                        WeekNum = WeekNum + 1;
                    }


                    if (base.IsGroupCompany(company.GroupId))
                    {
                        groupResult.Add(company.CompanySimpleName + "合计");
                        groupResult.Add(0);
                        groupResult.Add(0);
                        groupResult.Add(0);
                        groupResult.Add("");
                        groupResult.Add("");
                        groupResult.Add("");
                        groupResult.Add(0);
                        string dataHtml = GetTheadHtml(company);
                        StringBuilder TotalTable = new StringBuilder();
                        TotalTable.Append("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                        TotalTable.Append("<tr class=\"listTableHead\">");
                        TotalTable.Append("<td colspan=\"7\">" + company.CompanySimpleName + " [" + startDate.ToString("d") + "—" + endDate.AddDays(-1).ToString("d") + "]");
                        TotalTable.Append("</td></tr>\r\n");
                        TotalTable.Append("<tr class=\"listTableHead\">\r\n");
                        TotalTable.Append("<td>公司名</td>");
                        TotalTable.Append("<td>参加岗位考试人数</td>");
                        TotalTable.Append("<td>最小学习量<br />学习达标人数</td>");
                        TotalTable.Append("<td>最小学习量<br />学习未达标人数</td>");
                        TotalTable.Append("<td>最小学习量<br />学习达成率</td>");
                        TotalTable.Append("<td>学霸</td>");
                        TotalTable.Append("<td>学习数量</td>");
                        TotalTable.Append("</tr>\r\n");

                        CompanyInfo companySearch = new CompanyInfo();
                        companySearch.Field = "CompanyId";
                        companySearch.Condition = CompanyBLL.ReadCompanyIdList(companyID.ToString());
                        companySearch.State = 0;
                        companySearch.GroupIdCondition = "0,3";
                        List<CompanyInfo> sonCompanyList = CompanyBLL.ReadCompanyList(companySearch);
                        //List<CompanyInfo> sonCompanyList = CompanyBLL.ReadCompanyListByCompanyId(CompanyBLL.ReadCompanyIdList(companyID.ToString()));
                        foreach (CompanyInfo info in sonCompanyList)
                        {
                            if (info.CompanyId != companyID && !string.IsNullOrEmpty(info.PostStartDate.ToString()) && Convert.ToDateTime(info.PostStartDate) < endDate)
                            {
                                dataHtml += HtmlOut1(info, ref TotalTable);
                            }
                        }
                        TotalTable.Append("<tr class=\"listTableFoot\">\r\n");
                        TotalTable.Append("<td>" + groupResult[0] + "</td>");
                        TotalTable.Append("<td>" + groupResult[1] + "</td>");
                        TotalTable.Append("<td>" + groupResult[2] + "</td>");
                        TotalTable.Append("<td>" + groupResult[3] + "</td>");
                        TotalTable.Append("<td>" + (double.Parse(groupResult[2].ToString()) / double.Parse(groupResult[1].ToString())).ToString("P") + "</td>");
                        TotalTable.Append("<td>" + groupResult[6] + "</td>");
                        TotalTable.Append("<td>" + groupResult[7] + "</td>");
                        TotalTable.Append("</tr>\r\n");
                        TotalTable.Append("</table>\r\n");
                        TotalTable.Append("<div style=\"line-height:25px; text-align:left; font-size:14px; margin-bottom:50px;\"><p style=\"text-align:left;  font-size:14px; line-heigh:30px; margin-top:20px;\">4S店每人每周完成1-2小时的基础学习，将带动公司提升“百年老店”运营，建议高标为100%，低标为70%。");
                        if (!string.IsNullOrEmpty(groupResult[4].ToString()))
                            TotalTable.Append("<br /><br />目前：<span style=\"color:#00b050;\">" + groupResult[4] + "</span> 达至 <span style=\"color:#00b050;\">100%</span> ，表现良好，予以肯定。");
                        if (!string.IsNullOrEmpty(groupResult[5].ToString()))
                            TotalTable.Append("<br /><br /><span style=\"color:#FF0000;\">" + groupResult[5] + "</span> 达成率尚不足 <span style=\"color:#FF0000;\">70%</span> ，请相关部门领导关注下属自主学习时间的妥善安排。");
                        TotalTable.Append("</p></div>\r\n");
                        this.ReportList.InnerHtml = TotalTable.ToString() + dataHtml + GetTfootHtml();
                    }
                    else
                    {
                        StringBuilder TotalTable = null;
                        this.ReportList.InnerHtml = GetTheadHtml(company) + HtmlOut1(company, ref TotalTable) + GetTfootHtml();
                    }
                }
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            companyName = RequestHelper.GetForm<string>("CompanyName");
            companyID = RequestHelper.GetForm<int>("CompanyID");
            if (companyID > 0)
            {
                string StartDate = SearchStartDate.Text;
                string EndDate = SearchEndDate.Text;
                try
                {
                    if (Convert.ToDateTime(EndDate) <= Convert.ToDateTime(StartDate))
                        ScriptHelper.Alert("请正确选择时间段！");
                    ResponseHelper.Redirect("PostPlanReport2.aspx?Action=search&CompanyId=" + companyID.ToString() + "&SearchStartDate=" + StartDate + "&SearchEndDate=" + EndDate + "&StudyPostIdCondition=" + RequestHelper.GetForm<string>("StudyPostIdCondition") + "&PostIdCondition=" + RequestHelper.GetForm<string>("PostIdCondition") + "&GroupID=" + RequestHelper.GetForm<string>("GroupID") + "&State=" + RequestHelper.GetForm<string>("State"));
                }
                catch
                {
                    ScriptHelper.Alert("请正确选择时间段！");
                }
            }
            else
            {
                ScriptHelper.Alert("请选择完整的公司名称！");
            }
        }

        protected string HtmlOut1(CompanyInfo company, ref StringBuilder TotalTable)
        {
            StringBuilder TextOut = new StringBuilder();
            DateTime PostPlanStartDate = DateTime.MinValue;
            if (string.IsNullOrEmpty(company.PostStartDate.ToString()))
                PostPlanStartDate = Convert.ToDateTime("2013-7-1");
            else
                PostPlanStartDate = Convert.ToDateTime(company.PostStartDate.ToString());

            int PeoperNum = 0;
            //统计目标课程数
            int TargetNum = 0, CompanyDelayNum = 0;
            //统计通过课程数量第一的数据信息
            string PassCourseFirstUser = string.Empty, GoodStudent = string.Empty; int FirstPassCourseNum = 0;
            //单个岗位的岗位计划开始时间
            DateTime PostStartDate = DateTime.MinValue;
            int PostCourseNum = 0;//岗位课程数
            string AllPostPlan = "0"; //岗位下所有级别的岗位课程
            int RedNum = 0, YellowNum = 0, GreenNum = 0;

            //把公司所有员工的考试记录都一起调起
            TestPaperInfo TestPaperModel = new TestPaperInfo();
            if (!string.IsNullOrEmpty(company.PostStartDate.ToString()))
                TestPaperModel.TestMinDate = Convert.ToDateTime(company.PostStartDate);
            TestPaperModel.TestMaxDate = endDate;
            TestPaperModel.CompanyIdCondition = company.CompanyId.ToString();
            //TestPaperModel.Field = "UserID";
            TestPaperModel.Condition = "[UserID] in (select [ID] from [_User] where [companyID]=" + company.CompanyId.ToString();
            if (!string.IsNullOrEmpty(groupID))
                TestPaperModel.Condition += " and [GroupID] in (" + groupID + ")";
            if (!string.IsNullOrEmpty(postIdCondition))
                TestPaperModel.Condition += " and [WorkingPostID] in (" + postIdCondition + ")";
            if (!string.IsNullOrEmpty(studyPostIdCondition))
                TestPaperModel.Condition += " and [StudyPostId] in (" + studyPostIdCondition + ")";
            if (!string.IsNullOrEmpty(state))
                TestPaperModel.Condition += " and [Status] in (" + state + ")";
            TestPaperModel.Condition += ")";
            List<TestPaperInfo> TestPaperList = TestPaperBLL.NewReadList(TestPaperModel);

            UserSearchInfo userSearch = new UserSearchInfo();
            userSearch.InStatus = state;
            userSearch.InWorkingPostID = postIdCondition;
            //if (base.IsGroupCompany(company.GroupId))
            //{
            //    userSearch.InCompanyID = CompanyBLL.ReadCompanyIdList(company.CompanyId.ToString());
            //    ColNum = 11;
            //    userSearch.InStudyPostID = "45";//集团打开默认显示学习岗位
            //}
            //else
            {
                userSearch.InCompanyID = company.CompanyId.ToString();
                userSearch.InStudyPostID = studyPostIdCondition; //只显示该学习岗位下的人员
            }
            userSearch.InGroupID = groupID;
            List<UserInfo> userList = UserBLL.SearchReportUserList(userSearch);
            //记录岗位课程ID串
            Dictionary<int, string[]> postCourseDic = new Dictionary<int, string[]>();
            foreach (UserInfo Info in userList)
            {
                int PostId = int.MinValue, PerPassCourseNum = 0, PerCourseNum = 0;

                PostId = Info.StudyPostID;

                PostInfo PostModel = PostBLL.ReadPost(PostId);
                if (PostModel != null) //排除掉没有设置岗位的人
                {
                    //筛选出当前用户ID的成绩列表
                    List<TestPaperInfo> currentUserPaperList = TestPaperList.FindAll(delegate(TestPaperInfo TempModel) { return TempModel.UserId == Info.ID; });
                    List<TestPaperInfo> currentUserPassPaperList = currentUserPaperList.FindAll(delegate(TestPaperInfo TempModel) { return TempModel.IsPass == 1; });

                    int ResidueCourseNum = 0;//岗位内剩余岗位课程数

                    if (!postCourseDic.ContainsKey(PostId))
                    {
                        //岗位数据信息 1：岗位课程ID串；2：岗位课程数量；3：岗位开始时间；4：目标课程数量
                        string[] postData = { "0", "0", DateTime.MinValue.ToString(), "0" };
                        AllPostPlan = PostBLL.ReadPostCourseID(company.CompanyId, PostId);
                        postData[0] = AllPostPlan;

                        if (!string.IsNullOrEmpty(AllPostPlan))
                        {
                            PostCourseNum = AllPostPlan.Split(',').Length;
                            postData[1] = PostCourseNum.ToString();
                        }

                        //公司有事耽误的数量  由于岗位不一样，岗位计划开始的时间也不一样，所以有事耽误的数量也是不一致的，按岗位计算
                        PostStartDate = CompanyPostPlanBLL.ReadCompanyPostPlan(company.CompanyId, PostId);
                        //如果没有设置岗位开始时间或者岗位开始时间比公司开始时间早，统一使用公司开始时间
                        if (PostStartDate == DateTime.MinValue || PostStartDate < PostPlanStartDate) PostStartDate = PostPlanStartDate;

                        //根据统一的岗位开始时间调取数据
                        CompanyDelayNum = ChangeNumBLL.CompanyChangeNum(company.CompanyId, PostId, PostStartDate, endDate);
                        TargetNum = CompanyRuleBLL.GetCourseNum(company.CompanyId, PostId, PostStartDate, endDate); //理论值是每周两门
                        TargetNum = TargetNum - CompanyDelayNum;

                        postData[2] = PostStartDate.ToString();
                        postData[3] = TargetNum.ToString();
                        postCourseDic.Add(PostId, postData);
                    }
                    else
                    {
                        AllPostPlan = postCourseDic[PostId][0];
                        PostCourseNum = int.Parse(postCourseDic[PostId][1]);
                        PostStartDate = Convert.ToDateTime(postCourseDic[PostId][2]);
                        TargetNum = int.Parse(postCourseDic[PostId][3]);
                    }

                    //如果员工是后来的，要以员工进来的时间为起始点
                    if (PostStartDate < Info.PostStartDate)//Info.RegisterDate
                    {
                        PostStartDate = Info.PostStartDate;
                        CompanyDelayNum = ChangeNumBLL.CompanyChangeNum(company.CompanyId, PostId, PostStartDate, endDate);
                        TargetNum = CompanyRuleBLL.GetCourseNum(company.CompanyId, PostId, PostStartDate, endDate); //理论值是每周两门
                        TargetNum = TargetNum - CompanyDelayNum;
                    }
                    TargetNum = TargetNum - ChangeNumBLL.UserChangeNum(Info.ID, PostStartDate, endDate);


                    //PassCateId 跨岗位通过的课程  PostResidueCourse 岗位内剩余课程
                    string PassCateId = string.Empty, NoPassCateId = string.Empty, PostResidueCourse = string.Empty;
                    if (!string.IsNullOrEmpty(company.PostStartDate.ToString()))
                    {
                        PassCateId = TestPaperBLL.ReadCourseIDStr(currentUserPassPaperList.FindAll(delegate(TestPaperInfo TempModel) { return (TempModel.TestDate >= PostStartDate && TempModel.TestDate <= endDate); }));
                        PostResidueCourse = StringHelper.SubString(AllPostPlan, TestPaperBLL.ReadCourseIDStr(currentUserPassPaperList.FindAll(delegate(TestPaperInfo TempModel) { return (TempModel.TestDate <= endDate); })));
                    }
                    else
                    {
                        PassCateId = TestPaperBLL.ReadCourseIDStr(currentUserPassPaperList.FindAll(delegate(TestPaperInfo TempModel) { return (TempModel.TestDate <= endDate); }));
                        PostResidueCourse = StringHelper.SubString(AllPostPlan, PassCateId);
                    }

                    //跨岗位通过的课程数（全岗位计划开始后，累计完成的课程数）
                    int AllPassCourseNum = 0;
                    if (!string.IsNullOrEmpty(PassCateId))
                    {
                        AllPassCourseNum = PassCateId.Split(',').Length;
                    }
                    //获取岗位内剩余课程数
                    if (!string.IsNullOrEmpty(PostResidueCourse))
                    {
                        ResidueCourseNum = PostResidueCourse.Split(',').Length;
                    }

                    string PastPassCourse = string.Empty;
                    PeoperNum = PeoperNum + 1;
                    TotalPeoperNum += 1;
                    TextOut.Append("<tr class=\"listTableMain\">\r\n");
                    TextOut.Append("<td>" + TotalPeoperNum + "</td>");
                    TextOut.Append("<td>" + company.CompanySimpleName + "</td>");
                    TextOut.Append("<td>" + Info.RealName + "</td>");
                    if (!string.IsNullOrEmpty(Info.PostName))
                    {
                        TextOut.Append("<td>" + Info.PostName + "</td>");
                    }
                    else
                    {
                        TextOut.Append("<td>" + PostBLL.ReadPost(Info.WorkingPostID).PostName + "</td>");
                    }
                    TextOut.Append("<td>" + PostModel.PostName + "</td>");
                    for (int j = 1; j <= WeekNum; j++)
                    {
                        int WeekCourseNum = 0;
                        DateTime _StartDate = startDate.AddDays(7 * (j - 1));
                        DateTime _EndDate = startDate.AddDays(7 * j);
                        if (j == WeekNum) _EndDate = endDate;
                        if (_StartDate < PostStartDate) _StartDate = PostStartDate;
                        if (_EndDate > PostStartDate)
                        {
                            PastPassCourse = TestPaperBLL.ReadCourseIDStr(currentUserPassPaperList.FindAll(delegate(TestPaperInfo TempModel) { return (TempModel.TestDate >= _StartDate && TempModel.TestDate <= _EndDate); }));
                            WeekCourseNum = currentUserPaperList.FindAll(delegate(TestPaperInfo TempModel) { return (TempModel.TestDate >= _StartDate && TempModel.TestDate <= _EndDate); }).Count;
                        }
                        int PastPassCourseNum = 0;
                        if (!string.IsNullOrEmpty(PastPassCourse))
                        {
                            PastPassCourseNum = PastPassCourse.Split(',').Length;
                        }
                        PerPassCourseNum += PastPassCourseNum;
                        PerCourseNum += WeekCourseNum;
                        TextOut.Append("<td>" + WeekCourseNum + "</td>");
                        TextOut.Append("<td>" + (PastPassCourseNum) + "</td>");
                        if (PastPassCourseNum == 0)
                            TextOut.Append("<td>0</td>");
                        else
                            TextOut.Append("<td>" + ((double)PastPassCourseNum / (double)WeekCourseNum).ToString("P") + "</td>");
                    }

                    //统计好学生 学习课程超过30门的人
                    if (PerCourseNum >= 30) GoodStudent += " " + Info.RealName;

                    //统计通过课程数量第一的数据
                    if (PerPassCourseNum > 0 && PerPassCourseNum >= FirstPassCourseNum)
                    {
                        if (PerPassCourseNum > FirstPassCourseNum)
                        {
                            PassCourseFirstUser = Info.RealName;
                            FirstPassCourseNum = PerPassCourseNum;
                        }
                        else
                            PassCourseFirstUser = PassCourseFirstUser + " " + Info.RealName;
                    }
                    TextOut.Append("<td>" + PerCourseNum + "</td>");
                    TextOut.Append("<td>" + PerPassCourseNum + "</td>");
                    if (PerCourseNum <= 0)
                        TextOut.Append("<td>0</td>");
                    else
                        TextOut.Append("<td>" + ((double)PerPassCourseNum / (double)PerCourseNum).ToString("P") + "</td>");
                    TextOut.Append("<td>" + (AllPassCourseNum) + "</td>");
                    TextOut.Append("<td>" + (TargetNum) + "</td>");
                    TargetNum = AllPassCourseNum - TargetNum;
                    TextOut.Append("<td style=\"");
                    if (TargetNum > 0)
                    {
                        TextOut.Append("background: #00b050;");
                        GreenNum += 1;
                    }
                    else if (TargetNum < 0)
                    {
                        TextOut.Append("background: #ff0000;");
                        RedNum += 1;
                    }
                    else
                    {
                        TextOut.Append("background: #ffff00;");
                        YellowNum += 1;
                    }
                    TextOut.Append(" color:#000;\">" + (TargetNum) + "</td>");
                    TextOut.Append("</tr>\r\n");

                    currentUserPaperList = null;
                    currentUserPassPaperList = null;
                }
                PostModel = null;
            }
            TestPaperList = null;

            //集团性质不显示总结描述
            if (TotalTable == null)
            {
                if (FirstPassCourseNum > 0)
                    TextOut.Append("<tr class=\"listTableHead\"><th colspan=\"" + (ColNum + WeekNum * 3) + "\">课程考试通过数量最多的学霸为： <font color=red>" + PassCourseFirstUser + "</font>  ,数量为： <font color=red>" + FirstPassCourseNum.ToString() + "</font> 。</th></tr>");
                if (!string.IsNullOrEmpty(GoodStudent))
                    TextOut.Append("<tr class=\"listTableHead\"><th colspan=\"" + (ColNum + WeekNum * 3) + "\">月度考试数量超过30门的为好学生，此次好学生为： <font color=red>" + GoodStudent + "</font> 。</th></tr>");
                TextOut.Append("<tr class=\"listTableHead\"><th colspan=\"" + (ColNum + WeekNum * 3) + "\">未达标人数：" + RedNum.ToString() + " 刚好达标人数：" + YellowNum.ToString() + " 超越目标人数：" + GreenNum.ToString() + "</th></tr>");
                TextOut.Append("<tr class=\"listTableHead\"><th colspan=\"" + (ColNum + WeekNum * 3) + "\">未达标人数：" + RedNum.ToString() + " 达标人数：" + (YellowNum + GreenNum).ToString() + " 达成率：" + ((double)(YellowNum + GreenNum) / (double)PeoperNum).ToString("P") + "</th></tr>");
            }

            //生成集团报表内容
            if (TotalTable != null)
            {
                //集团报表内容开始
                groupResult[1] = (int)groupResult[1] + PeoperNum;
                groupResult[2] = (int)groupResult[2] + (YellowNum + GreenNum);
                groupResult[3] = (int)groupResult[3] + RedNum;
                if (((double)(YellowNum + GreenNum) / (double)PeoperNum) == 1.0)
                    groupResult[4] = groupResult[4] + "<span style=\"margin:0px 10px;\">" + company.CompanySimpleName + "</span>";
                else if (((double)(YellowNum + GreenNum) / (double)PeoperNum) < 0.7)
                    groupResult[5] = groupResult[5] + "<span style=\"margin:0px 10px;\">" + company.CompanySimpleName + "</span>";
                //GroupResult[4] += ((double)(YellowNum + GreenNum) / (double)PeoperNum);
                if (FirstPassCourseNum > 0 && FirstPassCourseNum >= (int)groupResult[7])
                {
                    if (FirstPassCourseNum == (int)groupResult[7])
                    {
                        groupResult[6] = string.IsNullOrEmpty(groupResult[6].ToString()) ? PassCourseFirstUser : groupResult[6] + " " + PassCourseFirstUser;
                    }
                    else
                    {
                        groupResult[6] = PassCourseFirstUser;
                        groupResult[7] = FirstPassCourseNum;
                    }
                }
                TotalTable.AppendLine("<tr class=\"listTableMain\"><td>" + company.CompanySimpleName + "</td><td>" + PeoperNum + "</td><td>" + (YellowNum + GreenNum).ToString() + "</td><td>" + RedNum.ToString() + "</td><td>" + ((double)(YellowNum + GreenNum) / (double)PeoperNum).ToString("P") + "</td><td>" + PassCourseFirstUser + "</td><td>" + FirstPassCourseNum.ToString() + "</td></tr>");
            }
            return TextOut.ToString();
        }

        private string GetTheadHtml(CompanyInfo company)
        {
            StringBuilder theadHtml = new StringBuilder();
            theadHtml.AppendLine("<table class=\"listTable\" cellpadding=\"0\">");
            theadHtml.Append("<thead>");
            theadHtml.AppendLine("<tr class=\"listTableHead\">");
            theadHtml.Append("<th colspan=\"" + (ColNum + WeekNum * 3) + "\">" + company.CompanySimpleName + " [" + startDate.ToString("d") + "—" + endDate.AddDays(-1).ToString("d") + "]");
            theadHtml.Append(" <input type=\"button\" onclick=\"javascript:preview(\'ctl00_ContentPlaceHolder_ReportList\');\" class=\"button\" style=\" width:100px;\"  value=\"导出到EXCEL\">");
            theadHtml.Append("</th>");
            theadHtml.Append("</tr>\r\n");
            theadHtml.Append("<tr class=\"listTableHead\">\r\n");
            theadHtml.Append("<th rowspan=\"3\" class=\"id\">序号</th>");
            theadHtml.Append("<th rowspan=\"3\" data-sort=\"string\">公司名</th>");
            theadHtml.Append("<th rowspan=\"3\" class=\"name\">姓名</th>");
            theadHtml.Append("<th rowspan=\"3\" class=\"post\" data-sort=\"string\">工作岗位</th>");
            theadHtml.Append("<th rowspan=\"3\" class=\"post\" data-sort=\"string\">学习岗位</th>");
            theadHtml.Append("<th colspan=\"" + (WeekNum * 3).ToString() + "\">学习已通过</th>");
            theadHtml.Append("<th colspan=\"3\">合计<br />(");
            theadHtml.Append(startDate.ToString("d") + "—" + endDate.AddDays(-1).ToString("d"));
            theadHtml.Append(")</th>");
            theadHtml.Append("<th colspan=\"3\">从项目启动开始</th>");
            theadHtml.Append("</tr>\r\n");
            theadHtml.Append("<tr class=\"listTableHead\">\r\n");
            for (int j = 1; j <= WeekNum; j++)
            {
                theadHtml.Append("<th colspan=\"3\">第" + j.ToString() + "周<br>" + startDate.AddDays(7 * (j - 1)).ToString("M-d") + "—");
                if (j == WeekNum) theadHtml.Append(endDate.AddDays(-1).ToString("M-d") + "</th>");
                else theadHtml.Append(startDate.AddDays((7 * j) - 1).ToString("M-d") + "</th>");
            }
            theadHtml.Append("<th rowspan=\"2\" class=\"total\">累计学习<br />考试次数</th>");
            theadHtml.Append("<th rowspan=\"2\" class=\"total\">考试通过<br />课程数量</th>");
            theadHtml.Append("<th rowspan=\"2\" class=\"total\">课程考试<br />通过率</th>");
            theadHtml.Append("<th rowspan=\"2\" class=\"total\">累计<br>完成总数</th>");
            theadHtml.Append("<th rowspan=\"2\" class=\"total\">目标<br>完成总数<br>（实际数）</th>");
            theadHtml.Append("<th rowspan=\"2\" class=\"total\">学习进度<br>达成分析<br>（超前/落后）</th>");
            theadHtml.Append("</tr>\r\n");
            theadHtml.Append("<tr class=\"listTableHead\">\r\n");
            for (int j = 1; j <= WeekNum; j++)
            {
                theadHtml.Append("<th class=\"total\">累计学习<br />考试次数</th>");
                theadHtml.Append("<th class=\"total\">考试通过<br />课程数量</th>");
                theadHtml.Append("<th class=\"total\">课程考试<br />通过率</th>");
            }
            theadHtml.Append("</tr>\r\n");
            theadHtml.Append("</thead>");
            theadHtml.Append("<tbody>");
            return theadHtml.ToString();
        }

        private string GetTfootHtml()
        {
            StringBuilder tfootHtml = new StringBuilder();
            tfootHtml.AppendLine("<tbody>");
            tfootHtml.AppendLine("</table>");
            return tfootHtml.ToString();
        }
    }
}
