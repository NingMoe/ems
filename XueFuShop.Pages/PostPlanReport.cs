using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;

namespace XueFuShop.Pages
{
    public class PostPlanReport : UserManageBasePage
    {
        protected int CompanyID = RequestHelper.GetQueryString<int>("CompanyID");
        protected DateTime StartDate = RequestHelper.GetQueryString<DateTime>("SearchStartDate");
        protected DateTime EndDate = RequestHelper.GetQueryString<DateTime>("SearchEndDate");
        protected string PostIdCondition = RequestHelper.GetQueryString<string>("PostIdCondition");
        protected string StudyPostIdCondition = RequestHelper.GetQueryString<string>("StudyPostIdCondition");
        protected string Action = RequestHelper.GetQueryString<string>("Action");
        protected ArrayList GroupResult = new ArrayList();
        protected List<PostInfo> PostList = new List<PostInfo>();
        protected string ReportContentHtml = string.Empty;
        protected List<AdminGroupInfo> userGroupList = new List<AdminGroupInfo>();
        protected string groupID = RequestHelper.GetQueryString<string>("GroupID");

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "最小学习量达成分析表";
            base.CheckUserPower("ReadEMSReport", PowerCheckType.Single);

            string logPath = ServerHelper.MapPath(@"\Log\");
            TxtLog log = new TxtLog(logPath);
            Stopwatch time2 = new Stopwatch();
            time2.Start();

            if (CompanyID < 0) CompanyID = base.UserCompanyID;
            CompanyInfo company = CompanyBLL.ReadCompany(CompanyID);
            PostList = PostBLL.ReadPostListByPostId(company.Post);
            if (base.ExistsSonCompany)
                userGroupList = AdminGroupBLL.ReadAdminGroupList(CompanyID, UserBLL.ReadUserGroupIDByCompanyID(base.SonCompanyID));
            else
                userGroupList = AdminGroupBLL.ReadAdminGroupList(CompanyID, UserBLL.ReadUserGroupIDByCompanyID(CompanyID.ToString()));

            //设置默认岗位(去除待考岗位)
            //if (string.IsNullOrEmpty(PostIdCondition)) PostIdCondition = StringHelper.SubString(company.Post, "197");
            //if (string.IsNullOrEmpty(StudyPostIdCondition)) StudyPostIdCondition = StringHelper.SubString(company.Post, "197");
            //用户权限组默认给考试人员
            if (string.IsNullOrEmpty(groupID))
                groupID = "36";

            if (Action == "Search")
            {
                if (EndDate == DateTime.MinValue) EndDate = DateTime.Today;
                EndDate = ShopCommon.SearchEndDate(EndDate);

                if (base.CompareUserPower("ManageGroupCompany", PowerCheckType.Single) && base.IsGroupCompany(company.GroupId))
                {
                    List<CompanyInfo> sonCompanyList = base.SonCompanyList;
                    //减少使用递归方法的频率
                    if (CompanyID != base.UserCompanyID)
                        sonCompanyList = CompanyBLL.ReadCompanyListByCompanyId(CompanyBLL.ReadCompanyIdList(CompanyID.ToString()));

                    GroupResult.Add(company.CompanySimpleName + "合计");
                    GroupResult.Add(0);
                    GroupResult.Add(0);
                    GroupResult.Add(0);
                    GroupResult.Add("");
                    GroupResult.Add("");
                    StringBuilder TotalTable = new StringBuilder();
                    TotalTable.AppendLine("<table class=\"evaluation_sheet\">");
                    TotalTable.AppendLine("<tr>");
                    TotalTable.AppendLine("<th colspan=\"5\">" + company.CompanySimpleName + " [" + StartDate.ToString("d") + "—" + EndDate.AddDays(-1).ToString("d") + "]");
                    TotalTable.Append("</th>");
                    TotalTable.AppendLine("</tr>");
                    TotalTable.AppendLine("<tr>");
                    TotalTable.AppendLine("<th>公司名</th>");
                    TotalTable.AppendLine("<th>参加岗位考试人数</th>");
                    TotalTable.AppendLine("<th>最小学习量<br />学习达标人数</th>");
                    TotalTable.AppendLine("<th>最小学习量<br />学习未达标人数</th>");
                    TotalTable.AppendLine("<th>最小学习量<br />学习达成率</th>");
                    TotalTable.AppendLine("</tr>");
                    foreach (CompanyInfo info in sonCompanyList)
                    {
                        if (info.CompanyId != CompanyID && !string.IsNullOrEmpty(info.PostStartDate.ToString()) && Convert.ToDateTime(info.PostStartDate) < EndDate)
                        {
                            TotalTable.Append(HtmlOut1(info, "TotalTable"));
                        }
                    }
                    TotalTable.AppendLine("<tr>");
                    TotalTable.AppendLine("<td>" + GroupResult[0] + "</td>");
                    TotalTable.AppendLine("<td>" + GroupResult[1] + "</td>");
                    TotalTable.AppendLine("<td>" + GroupResult[2] + "</td>");
                    TotalTable.AppendLine("<td>" + GroupResult[3] + "</td>");
                    TotalTable.AppendLine("<td>" + (double.Parse(GroupResult[2].ToString()) / double.Parse(GroupResult[1].ToString())).ToString("P") + "</td>");
                    TotalTable.AppendLine("</tr>");
                    TotalTable.AppendLine("</table>");
                    TotalTable.Append("<div style=\"line-height:25px; text-align:left; font-size:14px;\"><p style=\"text-align:left;  font-size:14px; line-heigh:30px; margin-top:20px;\">4S店每人每周完成1-2小时的基础学习，将带动公司提升“百年老店”运营，建议高标为100%，低标为70%。");
                    if (!string.IsNullOrEmpty(GroupResult[4].ToString()))
                        TotalTable.Append("<br /><br />目前：<span style=\"color:#00b050;\">" + GroupResult[4] + "</span> 达至 <span style=\"color:#00b050;\">100%</span> ，表现良好，予以肯定。");
                    if (!string.IsNullOrEmpty(GroupResult[5].ToString()))
                        TotalTable.Append("<br /><br /><span style=\"color:#FF0000;\">" + GroupResult[5] + "</span> 达成率尚不足 <span style=\"color:#FF0000;\">70%</span> ，请相关部门领导关注下属自主学习时间的妥善安排。");
                    TotalTable.Append("</p></div>\r\n");
                    ReportContentHtml = TotalTable.ToString();
                }
                else
                {
                    ReportContentHtml = HtmlOut1(company, "");
                }
            }

            time2.Stop();
            log.Write("总执行时间为：" + time2.Elapsed.TotalSeconds);
        }


        protected string HtmlOut(CompanyInfo company, string Type)
        {
            int WeekNum = 0, ColNum = 10, PeoperNum = 0;
            string logPath = ServerHelper.MapPath(@"\Log\");
            TxtLog log = new TxtLog(logPath);
            Stopwatch time1 = new Stopwatch();
            time1.Start();
            StringBuilder TextOut = new StringBuilder();
            string CompanyBrandId = company.BrandId;
            DateTime PostPlanStartDate = DateTime.MinValue;
            if (string.IsNullOrEmpty(company.PostStartDate.ToString()))            
                PostPlanStartDate = Convert.ToDateTime("2013-7-1");            
            else            
                PostPlanStartDate = Convert.ToDateTime(company.PostStartDate.ToString());            
            
            string rowspan = string.Empty;
            TextOut.Append("<table class=\"evaluation_sheet\">");
            TextOut.Append("<tr>");
            if (StartDate != DateTime.MinValue)
            {
                WeekNum = (EndDate - StartDate).Days / 7;
                if ((EndDate - StartDate).Days % 7 > 0)
                {
                    WeekNum = WeekNum + 1;
                }
                TextOut.Append("<th colspan=\"" + (ColNum + WeekNum * 3) + "\">" + company.CompanySimpleName + " [" + StartDate.ToString("d") + "—" + EndDate.AddDays(-1).ToString("d") + "]");
                rowspan = " rowspan=\"3\"";
            }
            else
            {
                TextOut.Append("<th colspan=\"" + ColNum + "\">" + company.CompanySimpleName + " [ 截止到：" + EndDate.AddDays(-1).ToString("d") + " ]");
                rowspan = " rowspan=\"3\"";
            }
            TextOut.Append("</th></tr>\r\n");
            TextOut.Append("<tr>\r\n");
            TextOut.Append("<th" + rowspan + " class=\"id\">序号</th>");
            //if (base.IsGroupCompany(company.GroupId)) TextOut.Append("<th" + rowspan + ">公司名</th>");
            TextOut.Append("<th" + rowspan + " class=\"name\">姓名</th>");
            TextOut.Append("<th" + rowspan + " class=\"post\">工作岗位</th>");
            TextOut.Append("<th" + rowspan + " class=\"post\">学习岗位</th>");
            TextOut.Append("<th colspan=\"" + (WeekNum * 3).ToString() + "\">学习已通过</th>");
            TextOut.Append("<th colspan=\"3\">合计<br />(");
            if (StartDate != DateTime.MinValue)
                TextOut.Append(StartDate.ToString("d") + "—" + EndDate.AddDays(-1).ToString("d"));
            else
            {
                TextOut.Append("截止到：" + EndDate.AddDays(-1).ToString("d"));
                StartDate = PostPlanStartDate;
            }
            TextOut.Append(")</th>");
            TextOut.Append("<th colspan=\"3\">从项目启动开始</th>");
            TextOut.Append("</tr>\r\n");
            TextOut.Append("<tr>\r\n");
            for (int j = 1; j <= WeekNum; j++)
            {
                TextOut.Append("<th colspan=\"3\">第" + j.ToString() + "周<br>" + StartDate.AddDays(7 * (j - 1)).ToString("M-d") + "—");
                if (j == WeekNum) TextOut.Append(EndDate.AddDays(-1).ToString("M-d") + "</th>");
                else TextOut.Append(StartDate.AddDays((7 * j) - 1).ToString("M-d") + "</th>");
            }
            TextOut.Append("<th rowspan=\"2\" class=\"total\">累计学习<br />考试次数</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">考试通过<br />课程数量</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">课程考试<br />通过率</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">累计<br>完成总数</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">目标<br>完成总数<br>（实际数）</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">学习进度<br>达成分析<br>（超前/落后）</th>");
            TextOut.Append("</tr>\r\n");
            TextOut.Append("<tr class=\"listTableHead\">\r\n");
            for (int j = 1; j <= WeekNum; j++)
            {
                TextOut.Append("<th class=\"total\">累计学习<br />考试次数</th>");
                TextOut.Append("<th class=\"total\">考试通过<br />课程数量</th>");
                TextOut.Append("<th class=\"total\">课程考试<br />通过率</th>");
            }
            TextOut.Append("</tr>\r\n");

            //统计目标课程数
            int TargetNum = 0, CompanyDelayNum = 0;
            //统计通过课程数量第一的数据信息
            string PassCourseFirstUser = string.Empty, GoodStudent = string.Empty; int FirstPassCourseNum = 0;
            //单个岗位的岗位计划开始时间
            DateTime PostStartDate = DateTime.MinValue;
            int PostSign = 0;//是否重新计算岗位总数的标记
            int PostCourseNum = 0;//岗位课程数
            string AllPostPlan = "0"; //岗位下所有级别的岗位课程
            int RedNum = 0, YellowNum = 0, GreenNum = 0;

            //增加通过率所用补丁部分
            TestPaperInfo TestPaperModel = new TestPaperInfo();
            TestPaperModel.TestMinDate = StartDate;
            TestPaperModel.TestMaxDate = EndDate;
            TestPaperModel.CompanyIdCondition = company.CompanyId.ToString();
            List<TestPaperInfo> TestPaperList = TestPaperBLL.NewReadList(TestPaperModel);

            UserSearchInfo userSearch = new UserSearchInfo();
            userSearch.Status = (int)UserState.Normal;
            userSearch.InWorkingPostID = PostIdCondition;
            //if (base.IsGroupCompany(company.GroupId))
            //{
            //    userSearch.InCompanyID = CompanyBLL.ReadCompanyIdList(company.CompanyId.ToString());
            //    ColNum = 11;
            //    userSearch.InStudyPostID = "45";//集团打开默认显示学习岗位
            //}
            //else
            {
                userSearch.InCompanyID = company.CompanyId.ToString();
                userSearch.InStudyPostID = StudyPostIdCondition; //只显示该学习岗位下的人员
            }
            List<UserInfo> userList = UserBLL.SearchReportUserList(userSearch);
            //记录岗位课程ID串
            Dictionary<int, string[]> postCourseDic = new Dictionary<int, string[]>();
            time1.Stop();
            log.Write("执行到会员开始循环的时间为：" + time1.Elapsed.TotalSeconds);
            time1.Reset();
            foreach (UserInfo Info in userList)
            {
                time1.Reset();
            time1.Start();
                int PostId = int.MinValue, PerPassCourseNum = 0, PerCourseNum = 0;

                PostId = Info.StudyPostID;
                
                PostInfo PostModel = PostBLL.ReadPost(PostId);
                if (PostModel != null) //排除掉没有设置岗位的人
                {

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
                        CompanyDelayNum = ChangeNumBLL.CompanyChangeNum(company.CompanyId, PostId, PostStartDate, EndDate);
                        TargetNum = CompanyRuleBLL.GetCourseNum(company.CompanyId, PostId, PostStartDate, EndDate); //理论值是每周两门
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
                        CompanyDelayNum = ChangeNumBLL.CompanyChangeNum(company.CompanyId, PostId, PostStartDate, EndDate);
                        TargetNum = CompanyRuleBLL.GetCourseNum(company.CompanyId, PostId, PostStartDate, EndDate); //理论值是每周两门
                        TargetNum = TargetNum - CompanyDelayNum;
                    }
                    TargetNum = TargetNum - ChangeNumBLL.UserChangeNum(Info.ID, PostStartDate, EndDate);


                    //PassCateId 跨岗位通过的课程  PostResidueCourse 岗位内剩余课程
                    string PassCateId = string.Empty, NoPassCateId = string.Empty, PostResidueCourse = string.Empty;
                    if (!string.IsNullOrEmpty(company.PostStartDate.ToString()))
                    {
                        PassCateId = TestPaperBLL.ReadCourseIDStr(TestPaperBLL.ReadList(Info.ID, PostStartDate, EndDate, 1));
                        PostResidueCourse = StringHelper.SubString(AllPostPlan, TestPaperBLL.ReadCourseIDStr(TestPaperBLL.ReadList(Info.ID, EndDate, 1)));
                    }
                    else
                    {
                        PassCateId = TestPaperBLL.ReadCourseIDStr(TestPaperBLL.ReadList(Info.ID, EndDate, 1));
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
                    TextOut.Append("<tr>\r\n");
                    TextOut.Append("<td>" + PeoperNum + "</td>");
                    //if (base.IsGroupCompany(company.GroupId)) TextOut.Append("<td>" + CompanyBLL.ReadCompany(Info.CompanyID).CompanySimpleName + "</td>");
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
                        DateTime _StartDate = StartDate.AddDays(7 * (j - 1));
                        DateTime _EndDate = StartDate.AddDays(7 * j);
                        if (j == WeekNum) _EndDate = EndDate;
                        if (_StartDate < PostStartDate) _StartDate = PostStartDate;
                        if (_EndDate > PostStartDate)
                        {
                            PastPassCourse = TestPaperBLL.ReadCourseIDStr(TestPaperBLL.ReadList(Info.ID, _StartDate, _EndDate, 1));
                            WeekCourseNum = TestPaperList.FindAll(delegate(TestPaperInfo TempModel) { return (TempModel.UserId == Info.ID && TempModel.TestDate >= _StartDate && TempModel.TestDate <= _EndDate); }).Count;
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
                }
                PostModel = null;
                time1.Stop();
                log.Write(Info.RealName + "执行时间为：" + time1.Elapsed.Milliseconds);
            }
            if (FirstPassCourseNum > 0)
                TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">课程考试通过数量最多的学霸为： <font color=red>" + PassCourseFirstUser + "</font>  ,数量为： <font color=red>" + FirstPassCourseNum.ToString() + "</font> 。</th></tr>");
            if (!string.IsNullOrEmpty(GoodStudent))
                TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">月度考试数量超过30门的为好学生，此次好学生为： <font color=red>" + GoodStudent + "</font> 。</th></tr>");
            TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">未达标人数：" + RedNum.ToString() + " 刚好达标人数：" + YellowNum.ToString() + " 超越目标人数：" + GreenNum.ToString() + "</th></tr>");
            TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">未达标人数：" + RedNum.ToString() + " 达标人数：" + (YellowNum + GreenNum).ToString() + " 达成率：" + ((double)(YellowNum + GreenNum) / (double)PeoperNum).ToString("P") + "</th></tr>");
            TextOut.Append("</table>");
            if (string.IsNullOrEmpty(Type))
                return TextOut.ToString();
            else
            {
                GroupResult[1] = (int)GroupResult[1] + PeoperNum;
                GroupResult[2] = (int)GroupResult[2] + (YellowNum + GreenNum);
                GroupResult[3] = (int)GroupResult[3] + RedNum;
                if (((double)(YellowNum + GreenNum) / (double)PeoperNum) == 1.0)
                    GroupResult[4] = GroupResult[4] + "<span style=\"margin:0px 10px;\">" + company.CompanySimpleName + "</span>";
                else if (((double)(YellowNum + GreenNum) / (double)PeoperNum) < 0.7)
                    GroupResult[5] = GroupResult[5] + "<span style=\"margin:0px 10px;\">" + company.CompanySimpleName + "</span>";
                //GroupResult[4] += ((double)(YellowNum + GreenNum) / (double)PeoperNum);
                return "<tr><td>" + company.CompanySimpleName + "</td><td>" + PeoperNum + "</td><td>" + (YellowNum + GreenNum).ToString() + "</td><td>" + RedNum.ToString() + "</td><td>" + ((double)(YellowNum + GreenNum) / (double)PeoperNum).ToString("P") + "</td></tr>";
            }           
        }

        protected string HtmlOut1(CompanyInfo company, string Type)
        {
            int WeekNum = 0, ColNum = 10, PeoperNum = 0;
            string logPath = ServerHelper.MapPath(@"\Log\");
            TxtLog log = new TxtLog(logPath);
            Stopwatch time1 = new Stopwatch();
            time1.Start();
            StringBuilder TextOut = new StringBuilder();
            string CompanyBrandId = company.BrandId;
            DateTime PostPlanStartDate = DateTime.MinValue;
            if (string.IsNullOrEmpty(company.PostStartDate.ToString()))
                PostPlanStartDate = Convert.ToDateTime("2013-7-1");
            else
                PostPlanStartDate = Convert.ToDateTime(company.PostStartDate.ToString());

            string rowspan = string.Empty;
            TextOut.Append("<table class=\"evaluation_sheet\">");
            TextOut.Append("<tr>");
            if (StartDate != DateTime.MinValue)
            {
                WeekNum = (EndDate - StartDate).Days / 7;
                if ((EndDate - StartDate).Days % 7 > 0)
                {
                    WeekNum = WeekNum + 1;
                }
                TextOut.Append("<th colspan=\"" + (ColNum + WeekNum * 3) + "\">" + company.CompanySimpleName + " [" + StartDate.ToString("d") + "—" + EndDate.AddDays(-1).ToString("d") + "]");
                rowspan = " rowspan=\"3\"";
            }
            else
            {
                TextOut.Append("<th colspan=\"" + ColNum + "\">" + company.CompanySimpleName + " [ 截止到：" + EndDate.AddDays(-1).ToString("d") + " ]");
                rowspan = " rowspan=\"3\"";
            }
            TextOut.Append("</th></tr>\r\n");
            TextOut.Append("<tr>\r\n");
            TextOut.Append("<th" + rowspan + " class=\"id\">序号</th>");
            //if (base.IsGroupCompany(company.GroupId)) TextOut.Append("<th" + rowspan + ">公司名</th>");
            TextOut.Append("<th" + rowspan + " class=\"name\">姓名</th>");
            TextOut.Append("<th" + rowspan + " class=\"post\">工作岗位</th>");
            TextOut.Append("<th" + rowspan + " class=\"post\">学习岗位</th>");
            TextOut.Append("<th colspan=\"" + (WeekNum * 3).ToString() + "\">学习已通过</th>");
            TextOut.Append("<th colspan=\"3\">合计<br />(");
            if (StartDate != DateTime.MinValue)
                TextOut.Append(StartDate.ToString("d") + "—" + EndDate.AddDays(-1).ToString("d"));
            else
            {
                TextOut.Append("截止到：" + EndDate.AddDays(-1).ToString("d"));
                StartDate = PostPlanStartDate;
            }
            TextOut.Append(")</th>");
            TextOut.Append("<th colspan=\"3\">从项目启动开始</th>");
            TextOut.Append("</tr>\r\n");
            TextOut.Append("<tr>\r\n");
            for (int j = 1; j <= WeekNum; j++)
            {
                TextOut.Append("<th colspan=\"3\">第" + j.ToString() + "周<br>" + StartDate.AddDays(7 * (j - 1)).ToString("M-d") + "—");
                if (j == WeekNum) TextOut.Append(EndDate.AddDays(-1).ToString("M-d") + "</th>");
                else TextOut.Append(StartDate.AddDays((7 * j) - 1).ToString("M-d") + "</th>");
            }
            TextOut.Append("<th rowspan=\"2\" class=\"total\">累计学习<br />考试次数</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">考试通过<br />课程数量</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">课程考试<br />通过率</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">累计<br>完成总数</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">目标<br>完成总数<br>（实际数）</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">学习进度<br>达成分析<br>（超前/落后）</th>");
            TextOut.Append("</tr>\r\n");
            TextOut.Append("<tr class=\"listTableHead\">\r\n");
            for (int j = 1; j <= WeekNum; j++)
            {
                TextOut.Append("<th class=\"total\">累计学习<br />考试次数</th>");
                TextOut.Append("<th class=\"total\">考试通过<br />课程数量</th>");
                TextOut.Append("<th class=\"total\">课程考试<br />通过率</th>");
            }
            TextOut.Append("</tr>\r\n");

            //统计目标课程数
            int TargetNum = 0, CompanyDelayNum = 0;
            //统计通过课程数量第一的数据信息
            string PassCourseFirstUser = string.Empty, GoodStudent = string.Empty; int FirstPassCourseNum = 0;
            //单个岗位的岗位计划开始时间
            DateTime PostStartDate = DateTime.MinValue;
            int PostSign = 0;//是否重新计算岗位总数的标记
            int PostCourseNum = 0;//岗位课程数
            string AllPostPlan = "0"; //岗位下所有级别的岗位课程
            int RedNum = 0, YellowNum = 0, GreenNum = 0;

            //增加通过率所用补丁部分
            TestPaperInfo TestPaperModel = new TestPaperInfo();
            if (!string.IsNullOrEmpty(company.PostStartDate.ToString()))
                TestPaperModel.TestMinDate = Convert.ToDateTime(company.PostStartDate);
            TestPaperModel.TestMaxDate = EndDate;
            TestPaperModel.CompanyIdCondition = company.CompanyId.ToString();
            TestPaperModel.Condition = "[UserID] in (select [ID] from [_User] where [companyID]=" + company.CompanyId.ToString() + " and [status]=" + (int)UserState.Normal;
            if (!string.IsNullOrEmpty(groupID))
                TestPaperModel.Condition += " and [GroupID] in (" + groupID + ")";
            if (!string.IsNullOrEmpty(PostIdCondition))
                TestPaperModel.Condition += " and [WorkingPostID] in (" + PostIdCondition + ")";
            if (!string.IsNullOrEmpty(StudyPostIdCondition))
                TestPaperModel.Condition += " and [StudyPostId] in (" + StudyPostIdCondition + ")";
            TestPaperModel.Condition += ")";
            List<TestPaperInfo> TestPaperList = TestPaperBLL.NewReadList(TestPaperModel);

            UserSearchInfo userSearch = new UserSearchInfo();
            userSearch.Status = (int)UserState.Normal;
            userSearch.InWorkingPostID = PostIdCondition;
            //if (base.IsGroupCompany(company.GroupId))
            //{
            //    userSearch.InCompanyID = CompanyBLL.ReadCompanyIdList(company.CompanyId.ToString());
            //    ColNum = 11;
            //    userSearch.InStudyPostID = "45";//集团打开默认显示学习岗位
            //}
            //else
            {
                userSearch.InCompanyID = company.CompanyId.ToString();
                userSearch.InStudyPostID = StudyPostIdCondition; //只显示该学习岗位下的人员
            }
            userSearch.InGroupID = groupID;
            List<UserInfo> userList = UserBLL.SearchReportUserList(userSearch);
            //记录岗位课程ID串
            Dictionary<int, string[]> postCourseDic = new Dictionary<int, string[]>();
            time1.Stop();
            log.Write("执行到会员开始循环的时间为：" + time1.Elapsed.TotalSeconds);
            time1.Reset();
            foreach (UserInfo Info in userList)
            {
                time1.Reset();
                time1.Start();
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
                        CompanyDelayNum = ChangeNumBLL.CompanyChangeNum(company.CompanyId, PostId, PostStartDate, EndDate);
                        TargetNum = CompanyRuleBLL.GetCourseNum(company.CompanyId, PostId, PostStartDate, EndDate); //理论值是每周两门
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
                        CompanyDelayNum = ChangeNumBLL.CompanyChangeNum(company.CompanyId, PostId, PostStartDate, EndDate);
                        TargetNum = CompanyRuleBLL.GetCourseNum(company.CompanyId, PostId, PostStartDate, EndDate); //理论值是每周两门
                        TargetNum = TargetNum - CompanyDelayNum;
                    }
                    TargetNum = TargetNum - ChangeNumBLL.UserChangeNum(Info.ID, PostStartDate, EndDate);


                    //PassCateId 跨岗位通过的课程  PostResidueCourse 岗位内剩余课程
                    string PassCateId = string.Empty, NoPassCateId = string.Empty, PostResidueCourse = string.Empty;
                    if (!string.IsNullOrEmpty(company.PostStartDate.ToString()))
                    {
                        PassCateId = TestPaperBLL.ReadCourseIDStr(currentUserPassPaperList.FindAll(delegate(TestPaperInfo TempModel) { return (TempModel.TestDate >= PostStartDate && TempModel.TestDate <= EndDate); }));
                        PostResidueCourse = StringHelper.SubString(AllPostPlan, TestPaperBLL.ReadCourseIDStr(currentUserPassPaperList.FindAll(delegate(TestPaperInfo TempModel) { return (TempModel.TestDate <= EndDate); })));
                    }
                    else
                    {
                        PassCateId = TestPaperBLL.ReadCourseIDStr(currentUserPassPaperList.FindAll(delegate(TestPaperInfo TempModel) { return (TempModel.TestDate <= EndDate); }));
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
                    TextOut.Append("<tr>\r\n");
                    TextOut.Append("<td>" + PeoperNum + "</td>");
                    //if (base.IsGroupCompany(company.GroupId)) TextOut.Append("<td>" + CompanyBLL.ReadCompany(Info.CompanyID).CompanySimpleName + "</td>");
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
                        DateTime _StartDate = StartDate.AddDays(7 * (j - 1));
                        DateTime _EndDate = StartDate.AddDays(7 * j);
                        if (j == WeekNum) _EndDate = EndDate;
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
                time1.Stop();
                log.Write(Info.RealName + "执行时间为：" + time1.Elapsed.Milliseconds);
            }
            TestPaperList = null;
            if (FirstPassCourseNum > 0)
                TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">课程考试通过数量最多的学霸为： <font color=red>" + PassCourseFirstUser + "</font>  ,数量为： <font color=red>" + FirstPassCourseNum.ToString() + "</font> 。</th></tr>");
            if (!string.IsNullOrEmpty(GoodStudent))
                TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">月度考试数量超过30门的为好学生，此次好学生为： <font color=red>" + GoodStudent + "</font> 。</th></tr>");
            TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">未达标人数：" + RedNum.ToString() + " 刚好达标人数：" + YellowNum.ToString() + " 超越目标人数：" + GreenNum.ToString() + "</th></tr>");
            TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">未达标人数：" + RedNum.ToString() + " 达标人数：" + (YellowNum + GreenNum).ToString() + " 达成率：" + ((double)(YellowNum + GreenNum) / (double)PeoperNum).ToString("P") + "</th></tr>");
            TextOut.Append("</table>");
            if (string.IsNullOrEmpty(Type))
                return TextOut.ToString();
            else
            {
                GroupResult[1] = (int)GroupResult[1] + PeoperNum;
                GroupResult[2] = (int)GroupResult[2] + (YellowNum + GreenNum);
                GroupResult[3] = (int)GroupResult[3] + RedNum;
                if (((double)(YellowNum + GreenNum) / (double)PeoperNum) == 1.0)
                    GroupResult[4] = GroupResult[4] + "<span style=\"margin:0px 10px;\">" + company.CompanySimpleName + "</span>";
                else if (((double)(YellowNum + GreenNum) / (double)PeoperNum) < 0.7)
                    GroupResult[5] = GroupResult[5] + "<span style=\"margin:0px 10px;\">" + company.CompanySimpleName + "</span>";
                //GroupResult[4] += ((double)(YellowNum + GreenNum) / (double)PeoperNum);
                return "<tr><td>" + company.CompanySimpleName + "</td><td>" + PeoperNum + "</td><td>" + (YellowNum + GreenNum).ToString() + "</td><td>" + RedNum.ToString() + "</td><td>" + ((double)(YellowNum + GreenNum) / (double)PeoperNum).ToString("P") + "</td></tr>";
            }
        }
    }
}
