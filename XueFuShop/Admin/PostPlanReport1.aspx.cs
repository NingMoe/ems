using System;
using XueFuShop.Pages;
using XueFu.EntLib;
using System.Collections.Generic;
using XueFuShop.Models;
using XueFuShop.BLL;
using System.Text;

namespace XueFuShop.Admin
{
    public partial class PostPlanReport1 : AdminBasePage
    {

        protected string postIdCondition = RequestHelper.GetQueryString<string>("PostIdCondition");
        protected string studyPostIdCondition = RequestHelper.GetQueryString<string>("StudyPostIdCondition");
        protected int companyID = RequestHelper.GetQueryString<int>("CompanyId");
        protected string SelectMonth = RequestHelper.GetQueryString<string>("SelectMonth");
        protected DateTime startDate = DateTime.MinValue;
        protected DateTime endDate = DateTime.MinValue;
        protected string state = RequestHelper.GetQueryString<string>("State");
        protected int del = RequestHelper.GetQueryString<int>("Del");
        protected int rz = RequestHelper.GetQueryString<int>("RZ");
        protected int rzDate = RequestHelper.GetQueryString<int>("RZDate");
        protected string companyName = string.Empty;
        protected string groupID = RequestHelper.GetQueryString<string>("GroupID");
        protected string action = RequestHelper.GetQueryString<string>("Action");


        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(action))
            {
                groupID = "36";
                state = ((int)UserState.Normal).ToString();
                rzDate = 0;
            }

            if (!IsPostBack)
            {
                List<PostInfo> PostList = PostBLL.ReadPostCateNamedList(2);
                List<PostInfo> NewPostList = new List<PostInfo>();
                foreach (PostInfo Info in PostList)
                {
                    List<PostInfo> TempPostList = PostBLL.ReadPostList(Info.PostId);
                    if (TempPostList == null || TempPostList.Count <= 0 || Info.ParentId > 0)
                    {
                        Info.PostName = Info.PostName.Trim().Replace("├", "").Trim();
                        NewPostList.Add(Info);
                    }
                }
                //base.BindControl(NewPostList, PostNameList);
                //base.BindControl(NewPostList, StudyPostNameList);
                if (!string.IsNullOrEmpty(action) && companyID > 0)
                {
                    ShowArea.Style.Add("display", "");
                    HtmlOut();
                }
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            companyName = RequestHelper.GetForm<string>("CompanyName");
            companyID = RequestHelper.GetForm<int>("CompanyID");
            if (companyID > 0)
            {
                if (string.IsNullOrEmpty(SearchStartDate.Text)) ScriptHelper.Alert("请选择开始日期！");
                if (string.IsNullOrEmpty(SearchEndDate.Text)) ScriptHelper.Alert("请选择结束日期！");

                ResponseHelper.Redirect("PostPlanReport1.aspx?Action=search&CompanyId=" + companyID.ToString() + "&SelectMonth=Other&SearchStartDate=" + this.SearchStartDate.Text + "&SearchEndDate=" + this.SearchEndDate.Text + "&RZ=" + RequestHelper.GetForm<string>("RZ") + "&RZDate=" + RequestHelper.GetForm<string>("RZDate") + "&State=" + RequestHelper.GetForm<string>("State") + "&GroupID=" + RequestHelper.GetForm<string>("GroupID") + "&Del=" + RequestHelper.GetForm<string>("Del") + "&PostIdCondition=" + RequestHelper.GetForm<string>("WorkingPostID") + "&StudyPostIdCondition=" + RequestHelper.GetForm<string>("StudyPostID"));
            }
            else
            {
                ScriptHelper.Alert("请选择完整的公司名称！");
            }
        }

        protected void HtmlOut()
        {
            startDate = Convert.ToDateTime(RequestHelper.GetQueryString<string>("SearchStartDate"));
            endDate = Convert.ToDateTime(RequestHelper.GetQueryString<string>("SearchEndDate")).AddDays(1);
            SearchStartDate.Text = startDate.ToString("d");
            SearchEndDate.Text = endDate.AddDays(-1).ToShortDateString();
            int WeekNum = 0, ColNum = 14;
            int PeoperNum = 0;
            StringBuilder TextOut = new StringBuilder();
            CompanyInfo company = CompanyBLL.ReadCompany(companyID);
            companyName = company.CompanyName;
            //string CompanyBrandId = company.BrandId;
            bool isGroupCompany = base.IsGroupCompany(company.GroupId);
            if (isGroupCompany) ColNum = 15;

            string rowspan = string.Empty;
            TextOut.Append("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            TextOut.Append("<thead>");
            TextOut.Append("<tr class=\"listTableHead\">");

            //循环开始日期
            DateTime loopStartDate = startDate;
            if (SelectMonth == "Other")
            {
                //目前只要一周的数据
                loopStartDate = endDate.AddDays(-7);
                WeekNum = (endDate - loopStartDate).Days / 7;
                if ((endDate - loopStartDate).Days % 7 > 0)
                {
                    WeekNum = WeekNum + 1;
                }
                TextOut.Append("<th colspan=\"" + (ColNum + WeekNum) + "\">" + company.CompanySimpleName);//+ " [" + loopStartDate.ToString("d") + "—" + EndDate.AddDays(-1).ToString("d") + "]"
                rowspan = " rowspan=\"2\"";
            }
            TextOut.Append("</th></tr>\r\n");
            TextOut.Append("<tr class=\"listTableHead\">\r\n");
            TextOut.Append("<th" + rowspan + ">序号</th>");
            if (isGroupCompany) TextOut.Append("<td" + rowspan + ">公司名</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"string\">姓名</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"string\">工作岗位</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"string\">学习岗位</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"string\">状态</th>");
            TextOut.Append("<th" + rowspan + ">初考时间</th>");
            TextOut.Append("<th" + rowspan + ">岗位课程数</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"int\">岗位课程<br>完成总数</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"int\">岗位剩余<br>课程总数</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"int\">学习完成率</th>");
            TextOut.Append("<th" + rowspan + ">所有已过岗位</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"int\">已过岗位数量<i class=\"icon_arrow\"></i></th>");
            TextOut.Append("<th" + rowspan + ">岗位已学习<br>考试未通过</th>");
            TextOut.Append("<th" + rowspan + ">已通过数量</th>");
            if (SelectMonth == "Other")
            {
                TextOut.Append("<th colspan=\"" + WeekNum.ToString() + "\">学习已通过</th></tr>\r\n");
                TextOut.Append("<tr class=\"listTableHead\">\r\n");
                for (int j = 1; j <= WeekNum; j++)
                {
                    TextOut.Append("<th>第" + j.ToString() + "周<br>" + loopStartDate.AddDays(7 * (j - 1)).ToString("M-d") + "—");
                    if (j == WeekNum) TextOut.Append(endDate.AddDays(-1).ToString("M-d") + "</th>");
                    else TextOut.Append(loopStartDate.AddDays((7 * j) - 1).ToString("M-d") + "</th>");
                }
                TextOut.Append("</tr>\r\n");
            }
            TextOut.Append("</thead>");
            TextOut.Append("<tbody>");

            UserSearchInfo user = new UserSearchInfo();
            user.InStatus = state;
            user.InGroupID = groupID;
            user.InWorkingPostID = postIdCondition;
            user.InStudyPostID = studyPostIdCondition; //只显示该学习岗位下的人员
            if (isGroupCompany)
            {
                user.InCompanyID = CompanyBLL.ReadCompanyIdList(companyID.ToString());
                //user.StudyPostIdCondition = "45";//集团打开默认显示学习岗位
            }
            else
            {
                user.InCompanyID = companyID.ToString();
            }
            if (rzDate == 1)
            {
                if (rz == 0)
                    user.Condition = "[id] not in (select [userid] from [_passpost] where [CreateDate]<='" + endDate + "' and [isrz]=1 and [_passpost].[postid] in (select [postid] from [_post]))";
                else if (rz == 1)
                    user.Condition = "[id] in (select [userid] from [_passpost] where [CreateDate]<='" + endDate + "' and [isrz]=1 and [_passpost].[postid] in (select [postid] from [_post]))";
            }
            else if (rzDate == 0)
            {
                if (rz == 0)
                    user.Condition = "[id] not in (select [userid] from [_passpost] where [CreateDate]>='" + startDate + "' And [CreateDate]<='" + endDate + "' and [isrz]=1 and [_passpost].[postid] in (select [postid] from [_post]))";
                else if (rz == 1)
                    user.Condition = "[id] in (select [userid] from [_passpost] where [CreateDate]>='" + startDate + "' And [CreateDate]<='" + endDate + "' and [isrz]=1 and [_passpost].[postid] in (select [postid] from [_post]))";
            }
            //user.Condition = string.IsNullOrEmpty(user.Condition) ? "Order by [CompanyID] Desc" : user.Condition + " Order by [CompanyID] Desc";
            //user.PostIdCondition = PostIdStr;
            List<UserInfo> userList = UserBLL.SearchReportUserList(user);//UserBLL.SearchUserList(user);

            //把公司所有员工的第一次考试记录都一起调起
            List<TestPaperReportInfo> userFirstTestRecordList = TestPaperBLL.ReadTheFirstRecordList(user.InCompanyID);

            //把公司所有员工的考试记录都一起调起
            TestPaperInfo TestPaperModel = new TestPaperInfo();
            //if (!string.IsNullOrEmpty(company.PostStartDate.ToString()))
            //    TestPaperModel.TestMinDate = Convert.ToDateTime(company.PostStartDate);
            TestPaperModel.TestMaxDate = endDate;
            TestPaperModel.CompanyIdCondition = user.InCompanyID;
            //TestPaperModel.Field = "UserID";
            TestPaperModel.Condition = "[UserID] in (select [ID] from [_User] where [CompanyID] in (" + user.InCompanyID + ")";
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

            foreach (UserInfo Info in userList)
            {
                int PostId = int.MinValue;
                PostId = Info.StudyPostID;
                PostInfo PostModel = PostBLL.ReadPost(Info.StudyPostID);
                if (PostModel != null) //排除掉没有设置岗位的人
                {
                    //string PassCateId = TestPaperBLL.ReadListStr(Info.ID, endDate, 1);
                    //string NoPassCateId = TestPaperBLL.ReadListStr(Info.ID, endDate, 0);
                    //筛选出当前用户ID的成绩列表
                    List<TestPaperInfo> currentUserPaperList = TestPaperList.FindAll(delegate(TestPaperInfo TempModel) { return TempModel.UserId == Info.ID; });
                    List<TestPaperInfo> currentUserPassPaperList = currentUserPaperList.FindAll(delegate(TestPaperInfo TempModel) { return TempModel.IsPass == 1; });
                    List<TestPaperInfo> currentUserNoPassPaperList = currentUserPaperList.FindAll(delegate(TestPaperInfo TempModel) { return TempModel.IsPass == 0; });
                    string PassCateId = TestPaperBLL.ReadCourseIDStr(currentUserPassPaperList);
                    string NoPassCateId = TestPaperBLL.ReadCourseIDStr(currentUserNoPassPaperList);

                    //从未通过的记录中去除后期又补考通过的记录
                    NoPassCateId = StringHelper.SubString(NoPassCateId, PassCateId);
                    int PostCourseNum = 0;
                    int PassCourseNum = 0;
                    int NoPassCourseNum = 0;

                    //岗位下所有已过级别的岗位课程
                    //if (isGroupCompany)
                    //    CompanyBrandId = CompanyBLL.ReadCompany(Info.CompanyID).BrandId;
                    string AllPostPlan = PostBLL.ReadPostCourseID(Info.CompanyID, Info.StudyPostID);//PostBLL.ReadPostCourseID(Info.StudyPostID, CompanyBrandId);

                    if (string.IsNullOrEmpty(AllPostPlan))
                    {
                        PostCourseNum = 0;
                    }
                    else
                    {
                        PostCourseNum = AllPostPlan.Split(',').Length;
                    }
                    string PassPostCateId = StringHelper.EqualString(PassCateId, AllPostPlan);
                    string NoPassPostCateId = StringHelper.EqualString(AllPostPlan, NoPassCateId);
                    if (string.IsNullOrEmpty(PassPostCateId))
                    {
                        PassCourseNum = 0;
                    }
                    else
                    {
                        PassCourseNum = PassPostCateId.Split(',').Length;
                    }

                    //选取时间段内
                    if (rzDate == 0)
                    {
                        //时间段内的学习中的岗位课程补丁，去掉即为到截止时间所有的学习中的岗位课程
                        if (SelectMonth == "Other") NoPassPostCateId = StringHelper.EqualString(NoPassPostCateId, TestPaperBLL.ReadCourseIDStr(currentUserNoPassPaperList.FindAll(delegate(TestPaperInfo TempModel) { return (TempModel.TestDate >= startDate && TempModel.TestDate <= endDate); })));
                        //补丁结束
                    }

                    if (string.IsNullOrEmpty(NoPassPostCateId))
                    {
                        NoPassCourseNum = 0;
                    }
                    else
                    {
                        NoPassCourseNum = NoPassPostCateId.Split(',').Length;
                    }

                    //如果公司ID不同，再次获取公司信息，减少重复操作
                    if (Info.CompanyID != company.CompanyId) company = CompanyBLL.ReadCompany(Info.CompanyID);
                    string PastPassCourse = string.Empty;
                    PeoperNum = PeoperNum + 1;
                    TextOut.Append("<tr class=\"listTableMain\">\r\n");
                    TextOut.Append("<td>" + PeoperNum + "</td>");
                    if (isGroupCompany) TextOut.Append("<td>" + company.CompanySimpleName + "</td>");
                    TextOut.Append("<td>" + Info.RealName + "</td>");
                    if (!string.IsNullOrEmpty(Info.PostName))
                        TextOut.Append("<td>" + Info.PostName + "</td>");
                    else
                        TextOut.Append("<td>" + PostBLL.ReadPost(Info.WorkingPostID).PostName + "</td>");
                    TextOut.Append("<td>" + PostModel.PostName + "</td>");
                    TextOut.Append("<td>" + EnumHelper.ReadEnumChineseName<UserState>(Info.Status) + "</td>");
                    //DateTime firstTestDate = TestPaperBLL.ReadTheOldTestPaperInfo(Info.ID).TestDate;
                    TestPaperReportInfo currentUserFirstTestRecord = userFirstTestRecordList.Find(delegate(TestPaperReportInfo tempModel) { return tempModel.UserID == Info.ID; });
                    if (currentUserFirstTestRecord != null)
                    {
                        TextOut.Append("<td>" + currentUserFirstTestRecord.TestDate.ToString("d") + "</td>");
                    }
                    else
                    {
                        TextOut.Append("<td>&nbsp;</td>");
                    }
                    TextOut.Append("<td>" + PostCourseNum.ToString() + "</td>");
                    TextOut.Append("<td>" + PassCourseNum.ToString() + "</td>");
                    TextOut.Append("<td>" + (PostCourseNum - PassCourseNum) + "</td>");
                    if (PostCourseNum > 0)
                        TextOut.Append("<td>" + (Math.Round((double)PassCourseNum / PostCourseNum, 4) * 100) + "%</td>");
                    else
                        TextOut.Append("<td>0</td>");
                    int passPostNum = 0;
                    if (rzDate == 1)
                        TextOut.Append("<td>" + PostPassBLL.ReadPassPostName(Info.ID, endDate, ref passPostNum) + "</td>");
                    else
                        TextOut.Append("<td>" + PostPassBLL.ReadPassPostName(Info.ID, startDate, endDate, ref passPostNum) + "</td>");
                    TextOut.Append("<td>" + passPostNum + "</td>");
                    TextOut.Append("<td>" + (NoPassCourseNum) + "</td>");
                    if (rzDate == 0)
                    {
                        List<TestPaperInfo> tempList = currentUserPassPaperList.FindAll(m => m.TestDate >= startDate && m.TestDate <= endDate);
                        if (tempList.Count > 0)
                            TextOut.Append("<td>" + TestPaperBLL.ReadCourseIDStr(tempList).Split(',').Length + "</td>");
                        else
                            TextOut.Append("<td>0</td>");
                    }
                    else
                    {
                        TextOut.Append("<td>" + (string.IsNullOrEmpty(PassCateId) ? 0 : PassCateId.Split(',').Length) + "</td>");
                    }
                    if (SelectMonth == "Other")
                    {
                        for (int j = 1; j <= WeekNum; j++)
                        {
                            if (j == WeekNum)
                            {
                                PastPassCourse = StringHelper.EqualString(AllPostPlan, TestPaperBLL.ReadCourseIDStr(currentUserPassPaperList.FindAll(delegate(TestPaperInfo TempModel) { return (TempModel.TestDate >= loopStartDate.AddDays(7 * (j - 1)) && TempModel.TestDate <= endDate); })));//TestPaperBLL.ReadListStr(Info.ID, loopStartDate.AddDays(7 * (j - 1)), endDate, 1)
                            }
                            else
                            {
                                PastPassCourse = StringHelper.EqualString(AllPostPlan, TestPaperBLL.ReadCourseIDStr(currentUserPassPaperList.FindAll(delegate(TestPaperInfo TempModel) { return (TempModel.TestDate >= loopStartDate.AddDays(7 * (j - 1)) && TempModel.TestDate <= loopStartDate.AddDays(7 * j)); })));//TestPaperBLL.ReadListStr(Info.ID, loopStartDate.AddDays(7 * (j - 1)), loopStartDate.AddDays(7 * j), 1)
                            }
                            int PastPassCourseNum = 0;
                            if (!string.IsNullOrEmpty(PastPassCourse))
                            {
                                PastPassCourseNum = PastPassCourse.Split(',').Length;
                            }
                            TextOut.Append("<td>" + (PastPassCourseNum) + "</td>");
                        }
                    }
                    else
                    {
                        //计算到上个时间学过的课程数
                        //算法(到现在所有通过的课程-到上个时间点所有完成的课程=时间段内新增课程)
                        //时间段内新增课程与岗位计划相同的部分=时间段内新增课程
                        PastPassCourse = StringHelper.EqualString(AllPostPlan, StringHelper.SubString(PassCateId, TestPaperBLL.ReadCourseIDStr(currentUserPassPaperList.FindAll(delegate(TestPaperInfo TempModel) { return (TempModel.TestDate <= startDate); }))));//TestPaperBLL.ReadListStr(Info.ID, startDate, 1)
                        int PastPassCourseNum = 0;
                        if (!string.IsNullOrEmpty(PastPassCourse))
                        {
                            PastPassCourseNum = PastPassCourse.Split(',').Length;
                        }
                        TextOut.Append("<td>" + (PastPassCourseNum) + "</td>");
                    }
                    TextOut.Append("</tr>\r\n");
                }
            }
            TextOut.Append("</tbody>");
            TextOut.Append("</table>");
            this.ReportList.InnerHtml = TextOut.ToString();
        }
    }
}
