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
                //����Ĭ��ֵ
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

                    //��������
                    WeekNum = (endDate - startDate).Days / 7;
                    if ((endDate - startDate).Days % 7 > 0)
                    {
                        WeekNum = WeekNum + 1;
                    }


                    if (base.IsGroupCompany(company.GroupId))
                    {
                        groupResult.Add(company.CompanySimpleName + "�ϼ�");
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
                        TotalTable.Append("<td colspan=\"7\">" + company.CompanySimpleName + " [" + startDate.ToString("d") + "��" + endDate.AddDays(-1).ToString("d") + "]");
                        TotalTable.Append("</td></tr>\r\n");
                        TotalTable.Append("<tr class=\"listTableHead\">\r\n");
                        TotalTable.Append("<td>��˾��</td>");
                        TotalTable.Append("<td>�μӸ�λ��������</td>");
                        TotalTable.Append("<td>��Сѧϰ��<br />ѧϰ�������</td>");
                        TotalTable.Append("<td>��Сѧϰ��<br />ѧϰδ�������</td>");
                        TotalTable.Append("<td>��Сѧϰ��<br />ѧϰ�����</td>");
                        TotalTable.Append("<td>ѧ��</td>");
                        TotalTable.Append("<td>ѧϰ����</td>");
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
                        TotalTable.Append("<div style=\"line-height:25px; text-align:left; font-size:14px; margin-bottom:50px;\"><p style=\"text-align:left;  font-size:14px; line-heigh:30px; margin-top:20px;\">4S��ÿ��ÿ�����1-2Сʱ�Ļ���ѧϰ����������˾�����������ϵꡱ��Ӫ������߱�Ϊ100%���ͱ�Ϊ70%��");
                        if (!string.IsNullOrEmpty(groupResult[4].ToString()))
                            TotalTable.Append("<br /><br />Ŀǰ��<span style=\"color:#00b050;\">" + groupResult[4] + "</span> ���� <span style=\"color:#00b050;\">100%</span> ���������ã����Կ϶���");
                        if (!string.IsNullOrEmpty(groupResult[5].ToString()))
                            TotalTable.Append("<br /><br /><span style=\"color:#FF0000;\">" + groupResult[5] + "</span> ������в��� <span style=\"color:#FF0000;\">70%</span> ������ز����쵼��ע��������ѧϰʱ������ư��š�");
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
                        ScriptHelper.Alert("����ȷѡ��ʱ��Σ�");
                    ResponseHelper.Redirect("PostPlanReport2.aspx?Action=search&CompanyId=" + companyID.ToString() + "&SearchStartDate=" + StartDate + "&SearchEndDate=" + EndDate + "&StudyPostIdCondition=" + RequestHelper.GetForm<string>("StudyPostIdCondition") + "&PostIdCondition=" + RequestHelper.GetForm<string>("PostIdCondition") + "&GroupID=" + RequestHelper.GetForm<string>("GroupID") + "&State=" + RequestHelper.GetForm<string>("State"));
                }
                catch
                {
                    ScriptHelper.Alert("����ȷѡ��ʱ��Σ�");
                }
            }
            else
            {
                ScriptHelper.Alert("��ѡ�������Ĺ�˾���ƣ�");
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
            //ͳ��Ŀ��γ���
            int TargetNum = 0, CompanyDelayNum = 0;
            //ͳ��ͨ���γ�������һ��������Ϣ
            string PassCourseFirstUser = string.Empty, GoodStudent = string.Empty; int FirstPassCourseNum = 0;
            //������λ�ĸ�λ�ƻ���ʼʱ��
            DateTime PostStartDate = DateTime.MinValue;
            int PostCourseNum = 0;//��λ�γ���
            string AllPostPlan = "0"; //��λ�����м���ĸ�λ�γ�
            int RedNum = 0, YellowNum = 0, GreenNum = 0;

            //�ѹ�˾����Ա���Ŀ��Լ�¼��һ�����
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
            //    userSearch.InStudyPostID = "45";//���Ŵ�Ĭ����ʾѧϰ��λ
            //}
            //else
            {
                userSearch.InCompanyID = company.CompanyId.ToString();
                userSearch.InStudyPostID = studyPostIdCondition; //ֻ��ʾ��ѧϰ��λ�µ���Ա
            }
            userSearch.InGroupID = groupID;
            List<UserInfo> userList = UserBLL.SearchReportUserList(userSearch);
            //��¼��λ�γ�ID��
            Dictionary<int, string[]> postCourseDic = new Dictionary<int, string[]>();
            foreach (UserInfo Info in userList)
            {
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
                        CompanyDelayNum = ChangeNumBLL.CompanyChangeNum(company.CompanyId, PostId, PostStartDate, endDate);
                        TargetNum = CompanyRuleBLL.GetCourseNum(company.CompanyId, PostId, PostStartDate, endDate); //����ֵ��ÿ������
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
                        CompanyDelayNum = ChangeNumBLL.CompanyChangeNum(company.CompanyId, PostId, PostStartDate, endDate);
                        TargetNum = CompanyRuleBLL.GetCourseNum(company.CompanyId, PostId, PostStartDate, endDate); //����ֵ��ÿ������
                        TargetNum = TargetNum - CompanyDelayNum;
                    }
                    TargetNum = TargetNum - ChangeNumBLL.UserChangeNum(Info.ID, PostStartDate, endDate);


                    //PassCateId ���λͨ���Ŀγ�  PostResidueCourse ��λ��ʣ��γ�
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
            }
            TestPaperList = null;

            //�������ʲ���ʾ�ܽ�����
            if (TotalTable == null)
            {
                if (FirstPassCourseNum > 0)
                    TextOut.Append("<tr class=\"listTableHead\"><th colspan=\"" + (ColNum + WeekNum * 3) + "\">�γ̿���ͨ����������ѧ��Ϊ�� <font color=red>" + PassCourseFirstUser + "</font>  ,����Ϊ�� <font color=red>" + FirstPassCourseNum.ToString() + "</font> ��</th></tr>");
                if (!string.IsNullOrEmpty(GoodStudent))
                    TextOut.Append("<tr class=\"listTableHead\"><th colspan=\"" + (ColNum + WeekNum * 3) + "\">�¶ȿ�����������30�ŵ�Ϊ��ѧ�����˴κ�ѧ��Ϊ�� <font color=red>" + GoodStudent + "</font> ��</th></tr>");
                TextOut.Append("<tr class=\"listTableHead\"><th colspan=\"" + (ColNum + WeekNum * 3) + "\">δ���������" + RedNum.ToString() + " �պô��������" + YellowNum.ToString() + " ��ԽĿ��������" + GreenNum.ToString() + "</th></tr>");
                TextOut.Append("<tr class=\"listTableHead\"><th colspan=\"" + (ColNum + WeekNum * 3) + "\">δ���������" + RedNum.ToString() + " ���������" + (YellowNum + GreenNum).ToString() + " ����ʣ�" + ((double)(YellowNum + GreenNum) / (double)PeoperNum).ToString("P") + "</th></tr>");
            }

            //���ɼ��ű�������
            if (TotalTable != null)
            {
                //���ű������ݿ�ʼ
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
            theadHtml.Append("<th colspan=\"" + (ColNum + WeekNum * 3) + "\">" + company.CompanySimpleName + " [" + startDate.ToString("d") + "��" + endDate.AddDays(-1).ToString("d") + "]");
            theadHtml.Append(" <input type=\"button\" onclick=\"javascript:preview(\'ctl00_ContentPlaceHolder_ReportList\');\" class=\"button\" style=\" width:100px;\"  value=\"������EXCEL\">");
            theadHtml.Append("</th>");
            theadHtml.Append("</tr>\r\n");
            theadHtml.Append("<tr class=\"listTableHead\">\r\n");
            theadHtml.Append("<th rowspan=\"3\" class=\"id\">���</th>");
            theadHtml.Append("<th rowspan=\"3\" data-sort=\"string\">��˾��</th>");
            theadHtml.Append("<th rowspan=\"3\" class=\"name\">����</th>");
            theadHtml.Append("<th rowspan=\"3\" class=\"post\" data-sort=\"string\">������λ</th>");
            theadHtml.Append("<th rowspan=\"3\" class=\"post\" data-sort=\"string\">ѧϰ��λ</th>");
            theadHtml.Append("<th colspan=\"" + (WeekNum * 3).ToString() + "\">ѧϰ��ͨ��</th>");
            theadHtml.Append("<th colspan=\"3\">�ϼ�<br />(");
            theadHtml.Append(startDate.ToString("d") + "��" + endDate.AddDays(-1).ToString("d"));
            theadHtml.Append(")</th>");
            theadHtml.Append("<th colspan=\"3\">����Ŀ������ʼ</th>");
            theadHtml.Append("</tr>\r\n");
            theadHtml.Append("<tr class=\"listTableHead\">\r\n");
            for (int j = 1; j <= WeekNum; j++)
            {
                theadHtml.Append("<th colspan=\"3\">��" + j.ToString() + "��<br>" + startDate.AddDays(7 * (j - 1)).ToString("M-d") + "��");
                if (j == WeekNum) theadHtml.Append(endDate.AddDays(-1).ToString("M-d") + "</th>");
                else theadHtml.Append(startDate.AddDays((7 * j) - 1).ToString("M-d") + "</th>");
            }
            theadHtml.Append("<th rowspan=\"2\" class=\"total\">�ۼ�ѧϰ<br />���Դ���</th>");
            theadHtml.Append("<th rowspan=\"2\" class=\"total\">����ͨ��<br />�γ�����</th>");
            theadHtml.Append("<th rowspan=\"2\" class=\"total\">�γ̿���<br />ͨ����</th>");
            theadHtml.Append("<th rowspan=\"2\" class=\"total\">�ۼ�<br>�������</th>");
            theadHtml.Append("<th rowspan=\"2\" class=\"total\">Ŀ��<br>�������<br>��ʵ������</th>");
            theadHtml.Append("<th rowspan=\"2\" class=\"total\">ѧϰ����<br>��ɷ���<br>����ǰ/���</th>");
            theadHtml.Append("</tr>\r\n");
            theadHtml.Append("<tr class=\"listTableHead\">\r\n");
            for (int j = 1; j <= WeekNum; j++)
            {
                theadHtml.Append("<th class=\"total\">�ۼ�ѧϰ<br />���Դ���</th>");
                theadHtml.Append("<th class=\"total\">����ͨ��<br />�γ�����</th>");
                theadHtml.Append("<th class=\"total\">�γ̿���<br />ͨ����</th>");
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
