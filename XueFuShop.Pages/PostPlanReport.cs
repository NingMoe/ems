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
            base.Title = "��Сѧϰ����ɷ�����";
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

            //����Ĭ�ϸ�λ(ȥ��������λ)
            //if (string.IsNullOrEmpty(PostIdCondition)) PostIdCondition = StringHelper.SubString(company.Post, "197");
            //if (string.IsNullOrEmpty(StudyPostIdCondition)) StudyPostIdCondition = StringHelper.SubString(company.Post, "197");
            //�û�Ȩ����Ĭ�ϸ�������Ա
            if (string.IsNullOrEmpty(groupID))
                groupID = "36";

            if (Action == "Search")
            {
                if (EndDate == DateTime.MinValue) EndDate = DateTime.Today;
                EndDate = ShopCommon.SearchEndDate(EndDate);

                if (base.CompareUserPower("ManageGroupCompany", PowerCheckType.Single) && base.IsGroupCompany(company.GroupId))
                {
                    List<CompanyInfo> sonCompanyList = base.SonCompanyList;
                    //����ʹ�õݹ鷽����Ƶ��
                    if (CompanyID != base.UserCompanyID)
                        sonCompanyList = CompanyBLL.ReadCompanyListByCompanyId(CompanyBLL.ReadCompanyIdList(CompanyID.ToString()));

                    GroupResult.Add(company.CompanySimpleName + "�ϼ�");
                    GroupResult.Add(0);
                    GroupResult.Add(0);
                    GroupResult.Add(0);
                    GroupResult.Add("");
                    GroupResult.Add("");
                    StringBuilder TotalTable = new StringBuilder();
                    TotalTable.AppendLine("<table class=\"evaluation_sheet\">");
                    TotalTable.AppendLine("<tr>");
                    TotalTable.AppendLine("<th colspan=\"5\">" + company.CompanySimpleName + " [" + StartDate.ToString("d") + "��" + EndDate.AddDays(-1).ToString("d") + "]");
                    TotalTable.Append("</th>");
                    TotalTable.AppendLine("</tr>");
                    TotalTable.AppendLine("<tr>");
                    TotalTable.AppendLine("<th>��˾��</th>");
                    TotalTable.AppendLine("<th>�μӸ�λ��������</th>");
                    TotalTable.AppendLine("<th>��Сѧϰ��<br />ѧϰ�������</th>");
                    TotalTable.AppendLine("<th>��Сѧϰ��<br />ѧϰδ�������</th>");
                    TotalTable.AppendLine("<th>��Сѧϰ��<br />ѧϰ�����</th>");
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
                    TotalTable.Append("<div style=\"line-height:25px; text-align:left; font-size:14px;\"><p style=\"text-align:left;  font-size:14px; line-heigh:30px; margin-top:20px;\">4S��ÿ��ÿ�����1-2Сʱ�Ļ���ѧϰ����������˾�����������ϵꡱ��Ӫ������߱�Ϊ100%���ͱ�Ϊ70%��");
                    if (!string.IsNullOrEmpty(GroupResult[4].ToString()))
                        TotalTable.Append("<br /><br />Ŀǰ��<span style=\"color:#00b050;\">" + GroupResult[4] + "</span> ���� <span style=\"color:#00b050;\">100%</span> ���������ã����Կ϶���");
                    if (!string.IsNullOrEmpty(GroupResult[5].ToString()))
                        TotalTable.Append("<br /><br /><span style=\"color:#FF0000;\">" + GroupResult[5] + "</span> ������в��� <span style=\"color:#FF0000;\">70%</span> ������ز����쵼��ע��������ѧϰʱ������ư��š�");
                    TotalTable.Append("</p></div>\r\n");
                    ReportContentHtml = TotalTable.ToString();
                }
                else
                {
                    ReportContentHtml = HtmlOut1(company, "");
                }
            }

            time2.Stop();
            log.Write("��ִ��ʱ��Ϊ��" + time2.Elapsed.TotalSeconds);
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
                TextOut.Append("<th colspan=\"" + (ColNum + WeekNum * 3) + "\">" + company.CompanySimpleName + " [" + StartDate.ToString("d") + "��" + EndDate.AddDays(-1).ToString("d") + "]");
                rowspan = " rowspan=\"3\"";
            }
            else
            {
                TextOut.Append("<th colspan=\"" + ColNum + "\">" + company.CompanySimpleName + " [ ��ֹ����" + EndDate.AddDays(-1).ToString("d") + " ]");
                rowspan = " rowspan=\"3\"";
            }
            TextOut.Append("</th></tr>\r\n");
            TextOut.Append("<tr>\r\n");
            TextOut.Append("<th" + rowspan + " class=\"id\">���</th>");
            //if (base.IsGroupCompany(company.GroupId)) TextOut.Append("<th" + rowspan + ">��˾��</th>");
            TextOut.Append("<th" + rowspan + " class=\"name\">����</th>");
            TextOut.Append("<th" + rowspan + " class=\"post\">������λ</th>");
            TextOut.Append("<th" + rowspan + " class=\"post\">ѧϰ��λ</th>");
            TextOut.Append("<th colspan=\"" + (WeekNum * 3).ToString() + "\">ѧϰ��ͨ��</th>");
            TextOut.Append("<th colspan=\"3\">�ϼ�<br />(");
            if (StartDate != DateTime.MinValue)
                TextOut.Append(StartDate.ToString("d") + "��" + EndDate.AddDays(-1).ToString("d"));
            else
            {
                TextOut.Append("��ֹ����" + EndDate.AddDays(-1).ToString("d"));
                StartDate = PostPlanStartDate;
            }
            TextOut.Append(")</th>");
            TextOut.Append("<th colspan=\"3\">����Ŀ������ʼ</th>");
            TextOut.Append("</tr>\r\n");
            TextOut.Append("<tr>\r\n");
            for (int j = 1; j <= WeekNum; j++)
            {
                TextOut.Append("<th colspan=\"3\">��" + j.ToString() + "��<br>" + StartDate.AddDays(7 * (j - 1)).ToString("M-d") + "��");
                if (j == WeekNum) TextOut.Append(EndDate.AddDays(-1).ToString("M-d") + "</th>");
                else TextOut.Append(StartDate.AddDays((7 * j) - 1).ToString("M-d") + "</th>");
            }
            TextOut.Append("<th rowspan=\"2\" class=\"total\">�ۼ�ѧϰ<br />���Դ���</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">����ͨ��<br />�γ�����</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">�γ̿���<br />ͨ����</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">�ۼ�<br>�������</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">Ŀ��<br>�������<br>��ʵ������</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">ѧϰ����<br>��ɷ���<br>����ǰ/���</th>");
            TextOut.Append("</tr>\r\n");
            TextOut.Append("<tr class=\"listTableHead\">\r\n");
            for (int j = 1; j <= WeekNum; j++)
            {
                TextOut.Append("<th class=\"total\">�ۼ�ѧϰ<br />���Դ���</th>");
                TextOut.Append("<th class=\"total\">����ͨ��<br />�γ�����</th>");
                TextOut.Append("<th class=\"total\">�γ̿���<br />ͨ����</th>");
            }
            TextOut.Append("</tr>\r\n");

            //ͳ��Ŀ��γ���
            int TargetNum = 0, CompanyDelayNum = 0;
            //ͳ��ͨ���γ�������һ��������Ϣ
            string PassCourseFirstUser = string.Empty, GoodStudent = string.Empty; int FirstPassCourseNum = 0;
            //������λ�ĸ�λ�ƻ���ʼʱ��
            DateTime PostStartDate = DateTime.MinValue;
            int PostSign = 0;//�Ƿ����¼����λ�����ı��
            int PostCourseNum = 0;//��λ�γ���
            string AllPostPlan = "0"; //��λ�����м���ĸ�λ�γ�
            int RedNum = 0, YellowNum = 0, GreenNum = 0;

            //����ͨ�������ò�������
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
            //    userSearch.InStudyPostID = "45";//���Ŵ�Ĭ����ʾѧϰ��λ
            //}
            //else
            {
                userSearch.InCompanyID = company.CompanyId.ToString();
                userSearch.InStudyPostID = StudyPostIdCondition; //ֻ��ʾ��ѧϰ��λ�µ���Ա
            }
            List<UserInfo> userList = UserBLL.SearchReportUserList(userSearch);
            //��¼��λ�γ�ID��
            Dictionary<int, string[]> postCourseDic = new Dictionary<int, string[]>();
            time1.Stop();
            log.Write("ִ�е���Ա��ʼѭ����ʱ��Ϊ��" + time1.Elapsed.TotalSeconds);
            time1.Reset();
            foreach (UserInfo Info in userList)
            {
                time1.Reset();
            time1.Start();
                int PostId = int.MinValue, PerPassCourseNum = 0, PerCourseNum = 0;

                PostId = Info.StudyPostID;
                
                PostInfo PostModel = PostBLL.ReadPost(PostId);
                if (PostModel != null) //�ų���û�����ø�λ����
                {

                    int ResidueCourseNum = 0;//��λ��ʣ���λ�γ���

                    if (!postCourseDic.ContainsKey(PostId))
                    {
                        //��λ������Ϣ 1����λ�γ�ID����2����λ�γ�������3����λ��ʼʱ�䣻4��Ŀ��γ�����
                        string[] postData = { "0", "0", DateTime.MinValue.ToString(), "0" };
                        AllPostPlan = PostBLL.ReadPostCourseID(company.CompanyId, PostId);
                        postData[0] = AllPostPlan;

                        if (!string.IsNullOrEmpty(AllPostPlan))
                        {
                            PostCourseNum = AllPostPlan.Split(',').Length;
                            postData[1] = PostCourseNum.ToString();
                        }

                        //��˾���µ��������  ���ڸ�λ��һ������λ�ƻ���ʼ��ʱ��Ҳ��һ�����������µ��������Ҳ�ǲ�һ�µģ�����λ����
                        PostStartDate = CompanyPostPlanBLL.ReadCompanyPostPlan(company.CompanyId, PostId);
                        //���û�����ø�λ��ʼʱ����߸�λ��ʼʱ��ȹ�˾��ʼʱ���磬ͳһʹ�ù�˾��ʼʱ��
                        if (PostStartDate == DateTime.MinValue || PostStartDate < PostPlanStartDate) PostStartDate = PostPlanStartDate;
                        
                        //����ͳһ�ĸ�λ��ʼʱ���ȡ����
                        CompanyDelayNum = ChangeNumBLL.CompanyChangeNum(company.CompanyId, PostId, PostStartDate, EndDate);
                        TargetNum = CompanyRuleBLL.GetCourseNum(company.CompanyId, PostId, PostStartDate, EndDate); //����ֵ��ÿ������
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

                    //���Ա���Ǻ����ģ�Ҫ��Ա��������ʱ��Ϊ��ʼ��
                    if (PostStartDate < Info.PostStartDate)//Info.RegisterDate
                    {
                        PostStartDate = Info.PostStartDate;
                        CompanyDelayNum = ChangeNumBLL.CompanyChangeNum(company.CompanyId, PostId, PostStartDate, EndDate);
                        TargetNum = CompanyRuleBLL.GetCourseNum(company.CompanyId, PostId, PostStartDate, EndDate); //����ֵ��ÿ������
                        TargetNum = TargetNum - CompanyDelayNum;
                    }
                    TargetNum = TargetNum - ChangeNumBLL.UserChangeNum(Info.ID, PostStartDate, EndDate);


                    //PassCateId ���λͨ���Ŀγ�  PostResidueCourse ��λ��ʣ��γ�
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

                    //���λͨ���Ŀγ�����ȫ��λ�ƻ���ʼ���ۼ���ɵĿγ�����
                    int AllPassCourseNum = 0;
                    if (!string.IsNullOrEmpty(PassCateId))
                    {
                        AllPassCourseNum = PassCateId.Split(',').Length;
                    }
                    //��ȡ��λ��ʣ��γ���
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

                    //ͳ�ƺ�ѧ�� ѧϰ�γ̳���30�ŵ���
                    if (PerCourseNum >= 30) GoodStudent += " " + Info.RealName;

                    //ͳ��ͨ���γ�������һ������
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
                log.Write(Info.RealName + "ִ��ʱ��Ϊ��" + time1.Elapsed.Milliseconds);
            }
            if (FirstPassCourseNum > 0)
                TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">�γ̿���ͨ����������ѧ��Ϊ�� <font color=red>" + PassCourseFirstUser + "</font>  ,����Ϊ�� <font color=red>" + FirstPassCourseNum.ToString() + "</font> ��</th></tr>");
            if (!string.IsNullOrEmpty(GoodStudent))
                TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">�¶ȿ�����������30�ŵ�Ϊ��ѧ�����˴κ�ѧ��Ϊ�� <font color=red>" + GoodStudent + "</font> ��</th></tr>");
            TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">δ���������" + RedNum.ToString() + " �պô��������" + YellowNum.ToString() + " ��ԽĿ��������" + GreenNum.ToString() + "</th></tr>");
            TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">δ���������" + RedNum.ToString() + " ���������" + (YellowNum + GreenNum).ToString() + " ����ʣ�" + ((double)(YellowNum + GreenNum) / (double)PeoperNum).ToString("P") + "</th></tr>");
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
                TextOut.Append("<th colspan=\"" + (ColNum + WeekNum * 3) + "\">" + company.CompanySimpleName + " [" + StartDate.ToString("d") + "��" + EndDate.AddDays(-1).ToString("d") + "]");
                rowspan = " rowspan=\"3\"";
            }
            else
            {
                TextOut.Append("<th colspan=\"" + ColNum + "\">" + company.CompanySimpleName + " [ ��ֹ����" + EndDate.AddDays(-1).ToString("d") + " ]");
                rowspan = " rowspan=\"3\"";
            }
            TextOut.Append("</th></tr>\r\n");
            TextOut.Append("<tr>\r\n");
            TextOut.Append("<th" + rowspan + " class=\"id\">���</th>");
            //if (base.IsGroupCompany(company.GroupId)) TextOut.Append("<th" + rowspan + ">��˾��</th>");
            TextOut.Append("<th" + rowspan + " class=\"name\">����</th>");
            TextOut.Append("<th" + rowspan + " class=\"post\">������λ</th>");
            TextOut.Append("<th" + rowspan + " class=\"post\">ѧϰ��λ</th>");
            TextOut.Append("<th colspan=\"" + (WeekNum * 3).ToString() + "\">ѧϰ��ͨ��</th>");
            TextOut.Append("<th colspan=\"3\">�ϼ�<br />(");
            if (StartDate != DateTime.MinValue)
                TextOut.Append(StartDate.ToString("d") + "��" + EndDate.AddDays(-1).ToString("d"));
            else
            {
                TextOut.Append("��ֹ����" + EndDate.AddDays(-1).ToString("d"));
                StartDate = PostPlanStartDate;
            }
            TextOut.Append(")</th>");
            TextOut.Append("<th colspan=\"3\">����Ŀ������ʼ</th>");
            TextOut.Append("</tr>\r\n");
            TextOut.Append("<tr>\r\n");
            for (int j = 1; j <= WeekNum; j++)
            {
                TextOut.Append("<th colspan=\"3\">��" + j.ToString() + "��<br>" + StartDate.AddDays(7 * (j - 1)).ToString("M-d") + "��");
                if (j == WeekNum) TextOut.Append(EndDate.AddDays(-1).ToString("M-d") + "</th>");
                else TextOut.Append(StartDate.AddDays((7 * j) - 1).ToString("M-d") + "</th>");
            }
            TextOut.Append("<th rowspan=\"2\" class=\"total\">�ۼ�ѧϰ<br />���Դ���</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">����ͨ��<br />�γ�����</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">�γ̿���<br />ͨ����</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">�ۼ�<br>�������</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">Ŀ��<br>�������<br>��ʵ������</th>");
            TextOut.Append("<th rowspan=\"2\" class=\"total\">ѧϰ����<br>��ɷ���<br>����ǰ/���</th>");
            TextOut.Append("</tr>\r\n");
            TextOut.Append("<tr class=\"listTableHead\">\r\n");
            for (int j = 1; j <= WeekNum; j++)
            {
                TextOut.Append("<th class=\"total\">�ۼ�ѧϰ<br />���Դ���</th>");
                TextOut.Append("<th class=\"total\">����ͨ��<br />�γ�����</th>");
                TextOut.Append("<th class=\"total\">�γ̿���<br />ͨ����</th>");
            }
            TextOut.Append("</tr>\r\n");

            //ͳ��Ŀ��γ���
            int TargetNum = 0, CompanyDelayNum = 0;
            //ͳ��ͨ���γ�������һ��������Ϣ
            string PassCourseFirstUser = string.Empty, GoodStudent = string.Empty; int FirstPassCourseNum = 0;
            //������λ�ĸ�λ�ƻ���ʼʱ��
            DateTime PostStartDate = DateTime.MinValue;
            int PostSign = 0;//�Ƿ����¼����λ�����ı��
            int PostCourseNum = 0;//��λ�γ���
            string AllPostPlan = "0"; //��λ�����м���ĸ�λ�γ�
            int RedNum = 0, YellowNum = 0, GreenNum = 0;

            //����ͨ�������ò�������
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
            //    userSearch.InStudyPostID = "45";//���Ŵ�Ĭ����ʾѧϰ��λ
            //}
            //else
            {
                userSearch.InCompanyID = company.CompanyId.ToString();
                userSearch.InStudyPostID = StudyPostIdCondition; //ֻ��ʾ��ѧϰ��λ�µ���Ա
            }
            userSearch.InGroupID = groupID;
            List<UserInfo> userList = UserBLL.SearchReportUserList(userSearch);
            //��¼��λ�γ�ID��
            Dictionary<int, string[]> postCourseDic = new Dictionary<int, string[]>();
            time1.Stop();
            log.Write("ִ�е���Ա��ʼѭ����ʱ��Ϊ��" + time1.Elapsed.TotalSeconds);
            time1.Reset();
            foreach (UserInfo Info in userList)
            {
                time1.Reset();
                time1.Start();
                int PostId = int.MinValue, PerPassCourseNum = 0, PerCourseNum = 0;

                PostId = Info.StudyPostID;

                PostInfo PostModel = PostBLL.ReadPost(PostId);
                if (PostModel != null) //�ų���û�����ø�λ����
                {
                    //ɸѡ����ǰ�û�ID�ĳɼ��б�
                    List<TestPaperInfo> currentUserPaperList = TestPaperList.FindAll(delegate(TestPaperInfo TempModel) { return TempModel.UserId == Info.ID; });
                    List<TestPaperInfo> currentUserPassPaperList = currentUserPaperList.FindAll(delegate(TestPaperInfo TempModel) { return TempModel.IsPass == 1; });

                    int ResidueCourseNum = 0;//��λ��ʣ���λ�γ���

                    if (!postCourseDic.ContainsKey(PostId))
                    {
                        //��λ������Ϣ 1����λ�γ�ID����2����λ�γ�������3����λ��ʼʱ�䣻4��Ŀ��γ�����
                        string[] postData = { "0", "0", DateTime.MinValue.ToString(), "0" };
                        AllPostPlan = PostBLL.ReadPostCourseID(company.CompanyId, PostId);
                        postData[0] = AllPostPlan;

                        if (!string.IsNullOrEmpty(AllPostPlan))
                        {
                            PostCourseNum = AllPostPlan.Split(',').Length;
                            postData[1] = PostCourseNum.ToString();
                        }

                        //��˾���µ��������  ���ڸ�λ��һ������λ�ƻ���ʼ��ʱ��Ҳ��һ�����������µ��������Ҳ�ǲ�һ�µģ�����λ����
                        PostStartDate = CompanyPostPlanBLL.ReadCompanyPostPlan(company.CompanyId, PostId);
                        //���û�����ø�λ��ʼʱ����߸�λ��ʼʱ��ȹ�˾��ʼʱ���磬ͳһʹ�ù�˾��ʼʱ��
                        if (PostStartDate == DateTime.MinValue || PostStartDate < PostPlanStartDate) PostStartDate = PostPlanStartDate;

                        //����ͳһ�ĸ�λ��ʼʱ���ȡ����
                        CompanyDelayNum = ChangeNumBLL.CompanyChangeNum(company.CompanyId, PostId, PostStartDate, EndDate);
                        TargetNum = CompanyRuleBLL.GetCourseNum(company.CompanyId, PostId, PostStartDate, EndDate); //����ֵ��ÿ������
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

                    //���Ա���Ǻ����ģ�Ҫ��Ա��������ʱ��Ϊ��ʼ��
                    if (PostStartDate < Info.PostStartDate)//Info.RegisterDate
                    {
                        PostStartDate = Info.PostStartDate;
                        CompanyDelayNum = ChangeNumBLL.CompanyChangeNum(company.CompanyId, PostId, PostStartDate, EndDate);
                        TargetNum = CompanyRuleBLL.GetCourseNum(company.CompanyId, PostId, PostStartDate, EndDate); //����ֵ��ÿ������
                        TargetNum = TargetNum - CompanyDelayNum;
                    }
                    TargetNum = TargetNum - ChangeNumBLL.UserChangeNum(Info.ID, PostStartDate, EndDate);


                    //PassCateId ���λͨ���Ŀγ�  PostResidueCourse ��λ��ʣ��γ�
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

                    //���λͨ���Ŀγ�����ȫ��λ�ƻ���ʼ���ۼ���ɵĿγ�����
                    int AllPassCourseNum = 0;
                    if (!string.IsNullOrEmpty(PassCateId))
                    {
                        AllPassCourseNum = PassCateId.Split(',').Length;
                    }
                    //��ȡ��λ��ʣ��γ���
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

                    //ͳ�ƺ�ѧ�� ѧϰ�γ̳���30�ŵ���
                    if (PerCourseNum >= 30) GoodStudent += " " + Info.RealName;

                    //ͳ��ͨ���γ�������һ������
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
                log.Write(Info.RealName + "ִ��ʱ��Ϊ��" + time1.Elapsed.Milliseconds);
            }
            TestPaperList = null;
            if (FirstPassCourseNum > 0)
                TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">�γ̿���ͨ����������ѧ��Ϊ�� <font color=red>" + PassCourseFirstUser + "</font>  ,����Ϊ�� <font color=red>" + FirstPassCourseNum.ToString() + "</font> ��</th></tr>");
            if (!string.IsNullOrEmpty(GoodStudent))
                TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">�¶ȿ�����������30�ŵ�Ϊ��ѧ�����˴κ�ѧ��Ϊ�� <font color=red>" + GoodStudent + "</font> ��</th></tr>");
            TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">δ���������" + RedNum.ToString() + " �պô��������" + YellowNum.ToString() + " ��ԽĿ��������" + GreenNum.ToString() + "</th></tr>");
            TextOut.Append("<tr><th colspan=\"" + (ColNum + WeekNum * 3) + "\">δ���������" + RedNum.ToString() + " ���������" + (YellowNum + GreenNum).ToString() + " ����ʣ�" + ((double)(YellowNum + GreenNum) / (double)PeoperNum).ToString("P") + "</th></tr>");
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
