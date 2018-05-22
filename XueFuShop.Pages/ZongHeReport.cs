using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Pages
{
    public class ZongHeReport : UserManageBasePage
    {
        protected int CompanyID = RequestHelper.GetQueryString<int>("CompanyId");
        protected string Action = RequestHelper.GetQueryString<string>("Action");
        protected DateTime StartDate = RequestHelper.GetQueryString<DateTime>("SearchStartDate");
        protected DateTime EndDate = RequestHelper.GetQueryString<DateTime>("SearchEndDate");
        protected string ReportContentHtml = string.Empty;
        protected CompanyInfo CompanyModel = new CompanyInfo();

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "综合情况统计表";
            base.CheckUserPower("ReadEMSReport", PowerCheckType.Single);

            if (!base.ExistsSonCompany || Action == "Search" || !base.CompareUserPower("ManageGroupCompany", PowerCheckType.Single))
            {
                if (EndDate == DateTime.MinValue) EndDate = DateTime.Today;
                List<CompanyInfo> sonCompanyList = new List<CompanyInfo>();
                if (CompanyID <= 0) CompanyID = base.UserCompanyID;

                CompanyModel = CompanyBLL.ReadCompany(CompanyID);

                if (base.CompareUserPower("ManageGroupCompany", PowerCheckType.Single))
                {
                    //减少使用递归方法的频率
                    if (CompanyID != base.UserCompanyID)
                        sonCompanyList = CompanyBLL.ReadCompanyListByCompanyId(CompanyBLL.ReadCompanyIdList(CompanyID.ToString()));
                    else
                        sonCompanyList = base.SonCompanyList;

                }
                else
                {
                    sonCompanyList.Add(CompanyModel);
                }

                int AllPassCourseNum = 0, AllCourseNum = 0, AllUserNum = 0, TotalNum = 0, CompanyNum = 0;
                double PassAverage = 0.0, PerPassAverage = 0.0, PerAllAverage = 0.0;
                string NoPassCompanyName = string.Empty, AverageCourseNumCompanyName = string.Empty;
                StringBuilder MonthOut = new StringBuilder();

                MonthOut.Append("<table class=\"evaluation_sheet\">");
                MonthOut.Append("<tr><th colspan=\"11\">" + CompanyModel.CompanyName + " 在职人员数据统计 [");
                if (StartDate != DateTime.MinValue) MonthOut.Append("从" + StartDate.ToString("d"));
                else MonthOut.Append("从项目启动");
                MonthOut.Append("到" + EndDate.ToString("d") + "]</th></tr>");
                MonthOut.Append("<tr>");
                MonthOut.Append("<th>店名</th>");
                MonthOut.Append("<th>项目启动时间</th>");
                MonthOut.Append("<th>系统登记<br />人数</th>");
                MonthOut.Append("<th>累计参加<br />考试人数</th>");
                MonthOut.Append("<th>累计学习<br />考试次数</th>");
                MonthOut.Append("<th>考试通过<br />课程数量</th>");
                MonthOut.Append("<th>人均学习<br />课程数量</th>");
                MonthOut.Append("<th>人均通过<br />课程数量</th>");
                MonthOut.Append("<th>参考人员<br />人均学习<br />课程数量</th>");
                MonthOut.Append("<th>参考人员<br />人均通过<br />课程数量</th>");
                MonthOut.Append("<th>课程考试<br />通过率</th>");
                MonthOut.Append("</tr>");
                EndDate = EndDate.AddDays(1);

                foreach (CompanyInfo _CompanyModel in sonCompanyList)
                {
                    if (_CompanyModel.CompanyId != CompanyID || !base.IsGroupCompany(_CompanyModel.GroupId))
                    {
                        if (!string.IsNullOrEmpty(_CompanyModel.PostStartDate.ToString()) && Convert.ToDateTime(_CompanyModel.PostStartDate.ToString()) < EndDate)
                        {

                            string UserId = string.Empty;
                            int PassCourseNum = 0, CourseNum = 0, UserNum = 0, Num = 0;
                            UserSearchInfo userSearch = new UserSearchInfo();
                            userSearch.InCompanyID = _CompanyModel.CompanyId.ToString();
                            userSearch.StatusNoEqual = (int)UserState.Del;
                            //userSearch.GroupId = 36;
                            List<UserInfo> userList = UserBLL.SearchReportUserList(userSearch);
                            Num = userList.Count;
                            //应考人数（去除在待考分组的人）
                            //int YKNum = userList.FindAll(delegate(UserInfo TempModel) { return TempModel.Status == (int)UserState.Normal; }).Count;
                            //Num = YKNum;

                            TestPaperInfo TestPaperModel = new TestPaperInfo();
                            TestPaperModel.TestMinDate = StartDate;
                            TestPaperModel.TestMaxDate = EndDate;
                            TestPaperModel.UserIdCondition = UserBLL.ReadUserIdStr(userList);
                            TestPaperModel.CompanyIdCondition = _CompanyModel.CompanyId.ToString();
                            List<TestPaperInfo> TempList = TestPaperBLL.NewReadList(TestPaperModel);
                            foreach (TestPaperInfo Info in TempList)
                            {
                                if (!StringHelper.CompareSingleString(UserId, Info.UserId.ToString()))
                                {
                                    UserId += "," + Info.UserId.ToString();
                                }
                                if (Info.IsPass == 1)
                                {
                                    PassCourseNum += 1;
                                }
                            }
                            if (UserId.StartsWith(",")) UserId = UserId.Substring(1);
                            if (string.IsNullOrEmpty(UserId))
                            {
                                UserNum = 0;
                            }
                            else
                            {
                                UserNum = UserId.Split(',').Length;
                            }
                            CourseNum = TempList.Count;

                            double SinglePassRate; //单店课程通过率
                            if (CourseNum == 0)
                                SinglePassRate = 0;
                            else
                                SinglePassRate = (double)PassCourseNum / (double)CourseNum;

                            if (SinglePassRate >= 0.22 || _CompanyModel.CompanyId == CompanyID)
                            {
                                CompanyNum += 1; //通过率大于22%的公司数量
                                TotalNum += Num;  //总人数
                                AllUserNum += UserNum; //总参加考试人数
                                AllCourseNum += CourseNum; //总考试课程数
                                AllPassCourseNum += PassCourseNum;  //总通过考试课程数
                            }
                            else
                            {
                                NoPassCompanyName += _CompanyModel.CompanySimpleName + " ";
                            }

                            MonthOut.Append("<tr>");
                            MonthOut.Append("<td>" + _CompanyModel.CompanySimpleName + "</td>");
                            MonthOut.Append("<td>" + Convert.ToDateTime(_CompanyModel.PostStartDate.ToString()).ToString("yyyy年M月") + "</td>");
                            MonthOut.Append("<td>" + Num.ToString() + "</td>");
                            MonthOut.Append("<td>" + UserNum.ToString() + "</td>");
                            MonthOut.Append("<td>" + CourseNum + "</td>");
                            MonthOut.Append("<td>" + PassCourseNum + "</td>");
                            if (Num == 0)
                            {
                                MonthOut.Append("<td>0</td>");
                                MonthOut.Append("<td>0</td>");
                            }
                            else
                            {
                                if (StartDate == DateTime.MinValue || (EndDate - StartDate).Days > 7 || Math.Round(((double)CourseNum / Num), 1) >= 2.2)
                                {
                                    MonthOut.Append("<td>" + Math.Round(((double)CourseNum / Num), 1) + "</td>");
                                }
                                else
                                {
                                    AverageCourseNumCompanyName += _CompanyModel.CompanySimpleName + " ";
                                    MonthOut.Append("<td style=\"background:#FF0000;\">" + Math.Round(((double)CourseNum / Num), 1) + "</td>");
                                }
                                MonthOut.Append("<td>" + Math.Round(((double)PassCourseNum / Num), 1) + "</td>");
                                if (SinglePassRate >= 0.22 || _CompanyModel.CompanyId == CompanyID)
                                {
                                    PerAllAverage += Math.Round(((double)CourseNum / Num), 1);
                                    PerPassAverage += Math.Round(((double)PassCourseNum / Num), 1);
                                }
                            }
                            //计算参考人员人均值
                            if (UserNum == 0)
                            {
                                MonthOut.Append("<td>0</td>");
                                MonthOut.Append("<td>0</td>");
                            }
                            else
                            {
                                MonthOut.Append("<td>" + Math.Round(((double)CourseNum / UserNum), 1) + "</td>");
                                MonthOut.Append("<td>" + Math.Round(((double)PassCourseNum / UserNum), 1) + "</td>");
                            }
                            if (SinglePassRate >= 0.22 || _CompanyModel.CompanyId == CompanyID)
                            {
                                MonthOut.Append("<td>" + SinglePassRate.ToString("P") + "</td>");
                                PassAverage += SinglePassRate;
                            }
                            else
                            {
                                MonthOut.Append("<td style=\"background:#FF0000;\">" + SinglePassRate.ToString("P") + "</td>");
                            }
                            MonthOut.Append("</tr>");
                        }
                    }
                }
                MonthOut.Append("<tr>");
                MonthOut.Append("<td colspan=\"2\">" + CompanyModel.CompanySimpleName + "有效学习合计</td>");
                MonthOut.Append("<td>" + TotalNum.ToString() + "</td>");
                MonthOut.Append("<td>" + AllUserNum.ToString() + "</td>");
                MonthOut.Append("<td>" + AllCourseNum + "</td>");
                MonthOut.Append("<td>" + AllPassCourseNum + "</td>");
                if (TotalNum == 0)
                {
                    MonthOut.Append("<td>0</td>");
                    MonthOut.Append("<td>0</td>");
                }
                else
                {
                    //MonthOut.Append("<td>" + Math.Round((PerAllAverage / CompanyNum), 1) + "</td>");
                    //MonthOut.Append("<td>" + Math.Round((PerPassAverage / CompanyNum), 1) + "</td>");
                    //MonthOut.Append("<td>" + (PassAverage / CompanyNum).ToString("P") + "</td>");
                    MonthOut.Append("<td>" + Math.Round(((double)AllCourseNum / TotalNum), 1) + "</td>");
                    MonthOut.Append("<td>" + Math.Round(((double)AllPassCourseNum / TotalNum), 1) + "</td>");
                }
                if (AllUserNum == 0)
                {
                    MonthOut.Append("<td>0</td>");
                    MonthOut.Append("<td>0</td>");
                }
                else
                {
                    MonthOut.Append("<td>" + Math.Round(((double)AllCourseNum / AllUserNum), 1) + "</td>");
                    MonthOut.Append("<td>" + Math.Round(((double)AllPassCourseNum / AllUserNum), 1) + "</td>");
                }
                if (AllCourseNum == 0)
                    MonthOut.Append("<td>0</td>");
                else
                    MonthOut.Append("<td>" + ((double)AllPassCourseNum / (double)AllCourseNum).ToString("P") + "</td>");
                MonthOut.Append("</tr>");
                MonthOut.Append("</table>");
                MonthOut.Insert(0, "在孟特EMS考试系统中进行有效学习的人数为 <span style=\"color:#FF0000;\">" + TotalNum.ToString() + " 人</span></li><li>累计进行了 <span style=\"color:#FF0000;\">" + AllCourseNum.ToString() + " 次</span>课程的学习，人均进行了 <span style=\"color:#FF0000;\">" + Math.Round(((double)AllCourseNum / TotalNum), 1) + " 次</span>课程考试</li><li>其中学习通过 <span style=\"color:#FF0000;\">" + AllPassCourseNum + " 门</span>课程考试，人均通过 <span style=\"color:#FF0000;\">" + Math.Round(((double)AllPassCourseNum / TotalNum), 1) + " 门</span>课程</li><li>课程学习的考试通过率为 <span style=\"color:#FF0000;\">" + ((double)AllPassCourseNum / (double)AllCourseNum).ToString("P") + "</span></li></ul>");

                MonthOut.Insert(0, "到" + EndDate.AddDays(-1).ToString("d"));
                if (StartDate != DateTime.MinValue) MonthOut.Insert(0, "从" + StartDate.ToString("d"));
                else MonthOut.Insert(0, "从项目启动");
                if (CompanyModel.GroupId == 1 || CompanyModel.GroupId == 2)
                    MonthOut.Insert(0, "集团");
                else
                    MonthOut.Insert(0, "贵司");
                MonthOut.Insert(0, "<ul class=\"ReportList_count\"><li>");

                MonthOut.Append("<ul class=\"ReportList_count\"><li><font color=\"red\">人均学习课程数量：</font>代表团队对学习重视的程度，越高代表越重视学习，建议<span style=\"color:#FF0000;\">不低于每周人均2.2门课程,每月人均8.5门课程</span>，低于指标者请部门领导对学习加大关注。</li>");
                if (StartDate != DateTime.MinValue && (EndDate - StartDate).Days <= 7 && !string.IsNullOrEmpty(AverageCourseNumCompanyName) && (CompanyModel.GroupId == 1 || CompanyModel.GroupId == 2))
                {
                    MonthOut.Append("<li style=\"color:#3366ff;\">目前人均学习数量低于每周人均2.2门课程的团队有：<span style=\"color:#FF0000;\">" + AverageCourseNumCompanyName + "</span>；请相关领导予以关注。</li>");
                    // else if (string.IsNullOrEmpty(AverageCourseNumCompanyName) && (CompanyModel.GroupId == 1 || CompanyModel.GroupId == 2))
                    MonthOut.Append("<li style=\"color:#3366ff;\">目前有效人均学习数量为:  <span style=\"color:#FF0000;\">" + Math.Round(((double)AllCourseNum / TotalNum), 1) + " 次</span>，");
                    if (Math.Round(((double)AllCourseNum / TotalNum), 1) >= 2.2)
                        MonthOut.Append("表现良好，请再接再厉。</li>");
                    else
                        MonthOut.Append("请相关领导予以关注。</li>");
                }
                MonthOut.Append("<li><font color=\"red\">课程考试通过率：</font>通过率高代表团队学习认真，若<span style=\"color:#FF0000;\">通过率低于22%</span>，表示学员没有学习课程，只是通过不断考试来<span style=\"color:#FF0000;\">试图懵过考核</span>，请部门领导制止不当的学习行为。</li>");
                if (!string.IsNullOrEmpty(NoPassCompanyName) && (CompanyModel.GroupId == 1 || CompanyModel.GroupId == 2))
                    MonthOut.Append("<li style=\"color:#3366ff;\">目前通过率低于22%的团队有: <span style=\"color:#FF0000;\">" + NoPassCompanyName + "</span>，请相关领导予以关注，以上4S店视为无效学习，全部数据不列入集团学习效果统计。</li>");
                else if (string.IsNullOrEmpty(NoPassCompanyName) && (CompanyModel.GroupId == 1 || CompanyModel.GroupId == 2))
                    MonthOut.Append("<li style=\"color:#3366ff; \">目前通过率为: <span style=\"color:#FF0000;\">" + ((double)AllPassCourseNum / (double)AllCourseNum).ToString("P") + "</span>，表现良好，请再接再厉。</li>");
                MonthOut.Append("</ul>");

                //添加通过岗位报表
                MonthOut.Append("<div class=\"split_line\"></div>");
                MonthOut.Append("<table class=\"evaluation_sheet\">");
                MonthOut.Append("<tr><th colspan=\"9\">" + CompanyModel.CompanyName + " 岗位资质认证统计 [");
                if (StartDate != DateTime.MinValue) MonthOut.Append("从" + StartDate.ToString("d"));
                else MonthOut.Append("从项目启动");
                MonthOut.Append("到" + EndDate.AddDays(-1).ToString("d") + "]</th></tr>");
                MonthOut.Append("<tr>");
                MonthOut.Append("<th>店名</th>");
                MonthOut.Append("<th>姓名</th>");
                MonthOut.Append("<th>在职岗位</th>");
                MonthOut.Append("<th>已通过的<br />认证岗位</th>");
                //MonthOut.Append("<th>岗位考核<br />课程数量</th>");
                MonthOut.Append("<th>累计通过<br />岗位数量</th>");
                MonthOut.Append("<th>目前正在<br />学习岗位</th>");
                MonthOut.Append("</tr>");
                foreach (CompanyInfo _CompanyModel in sonCompanyList)
                {
                    if (_CompanyModel.State != 0) continue;
                    if (_CompanyModel.CompanyId != CompanyID || !base.IsGroupCompany(_CompanyModel.GroupId))
                    {
                        PostPassInfo PostPassModel = new PostPassInfo();
                        PostPassModel.SearchStartDate = StartDate;
                        PostPassModel.CreateDate = EndDate;
                        List<ReportPostPassInfo> PostPassList = PostPassBLL.PostPassReportList(PostPassModel, _CompanyModel.CompanyId.ToString());
                        foreach (ReportPostPassInfo Info in PostPassList)
                        {
                            MonthOut.Append("<tr>");
                            MonthOut.Append("<td>" + _CompanyModel.CompanySimpleName + "</td>");
                            MonthOut.Append("<td>" + Info.RealName + "</td>");
                            if (string.IsNullOrEmpty(Info.WorkingPostName))
                                MonthOut.Append("<td>" + PostBLL.ReadPost(Info.WorkingPostId).PostName + "</td>");
                            else
                                MonthOut.Append("<td>" + Info.WorkingPostName + "</td>");
                            MonthOut.Append("<td>" + Info.PassPostName + "</td>");
                            MonthOut.Append("<td>" + Info.PassPostNum.ToString() + "</td>");
                            MonthOut.Append("<td>" + PostBLL.ReadPost(Info.StudyPostId).PostName + "</td>");
                            MonthOut.Append("</tr>");
                        }
                    }
                }
                MonthOut.Append("</table>");

                ReportContentHtml = MonthOut.ToString();
                ReportContentHtml += HistoryData(sonCompanyList, EndDate);
            }
        }

        protected string HistoryData(List<CompanyInfo> sonCompanyList, DateTime EndDate)
        {
            int AllPassCourseNum = 0, AllCourseNum = 0, AllUserNum = 0, TotalNum = 0;

            StringBuilder MonthOut = new StringBuilder();
            MonthOut.Append("<div class=\"split_line\"></div>");
            MonthOut.Append("<table class=\"evaluation_sheet\">");
            MonthOut.Append("<tr><th colspan=\"5\">" + CompanyModel.CompanyName + " 历史所有数据 [");
            MonthOut.Append("从项目启动");
            MonthOut.Append("到" + EndDate.AddDays(-1).ToString("d") + "]</th></tr>");
            MonthOut.Append("<tr>");
            MonthOut.Append("<th>店名</th>");
            MonthOut.Append("<th>系统登记<br />使用人数</th>");
            MonthOut.Append("<th>累计参加<br />考试人数</th>");
            MonthOut.Append("<th>累计学习<br />考试次数</th>");
            MonthOut.Append("<th>考试通过<br />课程数量</th>");
            MonthOut.Append("</tr>");

            foreach (CompanyInfo _CompanyModel in sonCompanyList)
            {
                if (_CompanyModel.CompanyId != CompanyID || !base.IsGroupCompany(_CompanyModel.GroupId))
                {
                    if (!string.IsNullOrEmpty(_CompanyModel.PostStartDate.ToString()) && Convert.ToDateTime(_CompanyModel.PostStartDate.ToString()) < EndDate)
                    {
                        string UserId = string.Empty;
                        int PassCourseNum = 0, CourseNum = 0, UserNum = 0, Num = 0;
                        UserSearchInfo userSearch = new UserSearchInfo();
                        userSearch.InCompanyID = _CompanyModel.CompanyId.ToString();
                        //userSearch.GroupId = 36;
                        Num = UserBLL.SearchUserList(userSearch).Count;

                        TestPaperInfo TestPaperModel = new TestPaperInfo();
                        TestPaperModel.TestMaxDate = EndDate;
                        TestPaperModel.CompanyIdCondition = _CompanyModel.CompanyId.ToString();
                        TestPaperModel.Del = int.MinValue;
                        List<TestPaperInfo> TempList = TestPaperBLL.NewReadList(TestPaperModel);
                        foreach (TestPaperInfo Info in TempList)
                        {
                            if (!StringHelper.CompareSingleString(UserId, Info.UserId.ToString()))
                            {
                                UserId += "," + Info.UserId.ToString();
                            }
                            if (Info.IsPass == 1)
                            {
                                PassCourseNum += 1;
                            }
                        }
                        CourseNum = TempList.Count;
                        AllPassCourseNum += PassCourseNum;
                        AllCourseNum += CourseNum;
                        if (UserId.StartsWith(",")) UserId = UserId.Substring(1);
                        if (string.IsNullOrEmpty(UserId))
                        {
                            UserNum = 0;
                        }
                        else
                        {
                            UserNum = UserId.Split(',').Length;
                        }
                        if (Num < UserNum) Num = UserNum;  //如果系统历史记录人数小于历史考试记录人数，以历史考试记录人数为准
                        TotalNum += Num;
                        AllUserNum += UserNum;

                        MonthOut.Append("<tr>");
                        MonthOut.Append("<td>" + _CompanyModel.CompanySimpleName + "</td>");
                        MonthOut.Append("<td>" + Num.ToString() + "</td>");
                        MonthOut.Append("<td>" + UserNum.ToString() + "</td>");
                        MonthOut.Append("<td>" + CourseNum + "</td>");
                        MonthOut.Append("<td>" + PassCourseNum + "</td>");
                        MonthOut.Append("</tr>");
                    }
                }
            }
            MonthOut.Append("<tr>");
            MonthOut.Append("<td>" + CompanyModel.CompanySimpleName + "合计</td>");
            MonthOut.Append("<td>" + TotalNum.ToString() + "</td>");
            MonthOut.Append("<td>" + AllUserNum.ToString() + "</td>");
            MonthOut.Append("<td>" + AllCourseNum + "</td>");
            MonthOut.Append("<td>" + AllPassCourseNum + "</td>");
            MonthOut.Append("</tr>");
            MonthOut.Append("</table>");
            return MonthOut.ToString();
        }

    }
}
