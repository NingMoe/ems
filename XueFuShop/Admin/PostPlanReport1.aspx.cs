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
                        Info.PostName = Info.PostName.Trim().Replace("��", "").Trim();
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
                if (string.IsNullOrEmpty(SearchStartDate.Text)) ScriptHelper.Alert("��ѡ��ʼ���ڣ�");
                if (string.IsNullOrEmpty(SearchEndDate.Text)) ScriptHelper.Alert("��ѡ��������ڣ�");

                ResponseHelper.Redirect("PostPlanReport1.aspx?Action=search&CompanyId=" + companyID.ToString() + "&SelectMonth=Other&SearchStartDate=" + this.SearchStartDate.Text + "&SearchEndDate=" + this.SearchEndDate.Text + "&RZ=" + RequestHelper.GetForm<string>("RZ") + "&RZDate=" + RequestHelper.GetForm<string>("RZDate") + "&State=" + RequestHelper.GetForm<string>("State") + "&GroupID=" + RequestHelper.GetForm<string>("GroupID") + "&Del=" + RequestHelper.GetForm<string>("Del") + "&PostIdCondition=" + RequestHelper.GetForm<string>("WorkingPostID") + "&StudyPostIdCondition=" + RequestHelper.GetForm<string>("StudyPostID"));
            }
            else
            {
                ScriptHelper.Alert("��ѡ�������Ĺ�˾���ƣ�");
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

            //ѭ����ʼ����
            DateTime loopStartDate = startDate;
            if (SelectMonth == "Other")
            {
                //ĿǰֻҪһ�ܵ�����
                loopStartDate = endDate.AddDays(-7);
                WeekNum = (endDate - loopStartDate).Days / 7;
                if ((endDate - loopStartDate).Days % 7 > 0)
                {
                    WeekNum = WeekNum + 1;
                }
                TextOut.Append("<th colspan=\"" + (ColNum + WeekNum) + "\">" + company.CompanySimpleName);//+ " [" + loopStartDate.ToString("d") + "��" + EndDate.AddDays(-1).ToString("d") + "]"
                rowspan = " rowspan=\"2\"";
            }
            TextOut.Append("</th></tr>\r\n");
            TextOut.Append("<tr class=\"listTableHead\">\r\n");
            TextOut.Append("<th" + rowspan + ">���</th>");
            if (isGroupCompany) TextOut.Append("<td" + rowspan + ">��˾��</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"string\">����</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"string\">������λ</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"string\">ѧϰ��λ</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"string\">״̬</th>");
            TextOut.Append("<th" + rowspan + ">����ʱ��</th>");
            TextOut.Append("<th" + rowspan + ">��λ�γ���</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"int\">��λ�γ�<br>�������</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"int\">��λʣ��<br>�γ�����</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"int\">ѧϰ�����</th>");
            TextOut.Append("<th" + rowspan + ">�����ѹ���λ</th>");
            TextOut.Append("<th" + rowspan + " data-sort=\"int\">�ѹ���λ����<i class=\"icon_arrow\"></i></th>");
            TextOut.Append("<th" + rowspan + ">��λ��ѧϰ<br>����δͨ��</th>");
            TextOut.Append("<th" + rowspan + ">��ͨ������</th>");
            if (SelectMonth == "Other")
            {
                TextOut.Append("<th colspan=\"" + WeekNum.ToString() + "\">ѧϰ��ͨ��</th></tr>\r\n");
                TextOut.Append("<tr class=\"listTableHead\">\r\n");
                for (int j = 1; j <= WeekNum; j++)
                {
                    TextOut.Append("<th>��" + j.ToString() + "��<br>" + loopStartDate.AddDays(7 * (j - 1)).ToString("M-d") + "��");
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
            user.InStudyPostID = studyPostIdCondition; //ֻ��ʾ��ѧϰ��λ�µ���Ա
            if (isGroupCompany)
            {
                user.InCompanyID = CompanyBLL.ReadCompanyIdList(companyID.ToString());
                //user.StudyPostIdCondition = "45";//���Ŵ�Ĭ����ʾѧϰ��λ
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

            //�ѹ�˾����Ա���ĵ�һ�ο��Լ�¼��һ�����
            List<TestPaperReportInfo> userFirstTestRecordList = TestPaperBLL.ReadTheFirstRecordList(user.InCompanyID);

            //�ѹ�˾����Ա���Ŀ��Լ�¼��һ�����
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
                if (PostModel != null) //�ų���û�����ø�λ����
                {
                    //string PassCateId = TestPaperBLL.ReadListStr(Info.ID, endDate, 1);
                    //string NoPassCateId = TestPaperBLL.ReadListStr(Info.ID, endDate, 0);
                    //ɸѡ����ǰ�û�ID�ĳɼ��б�
                    List<TestPaperInfo> currentUserPaperList = TestPaperList.FindAll(delegate(TestPaperInfo TempModel) { return TempModel.UserId == Info.ID; });
                    List<TestPaperInfo> currentUserPassPaperList = currentUserPaperList.FindAll(delegate(TestPaperInfo TempModel) { return TempModel.IsPass == 1; });
                    List<TestPaperInfo> currentUserNoPassPaperList = currentUserPaperList.FindAll(delegate(TestPaperInfo TempModel) { return TempModel.IsPass == 0; });
                    string PassCateId = TestPaperBLL.ReadCourseIDStr(currentUserPassPaperList);
                    string NoPassCateId = TestPaperBLL.ReadCourseIDStr(currentUserNoPassPaperList);

                    //��δͨ���ļ�¼��ȥ�������ֲ���ͨ���ļ�¼
                    NoPassCateId = StringHelper.SubString(NoPassCateId, PassCateId);
                    int PostCourseNum = 0;
                    int PassCourseNum = 0;
                    int NoPassCourseNum = 0;

                    //��λ�������ѹ�����ĸ�λ�γ�
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

                    //ѡȡʱ�����
                    if (rzDate == 0)
                    {
                        //ʱ����ڵ�ѧϰ�еĸ�λ�γ̲�����ȥ����Ϊ����ֹʱ�����е�ѧϰ�еĸ�λ�γ�
                        if (SelectMonth == "Other") NoPassPostCateId = StringHelper.EqualString(NoPassPostCateId, TestPaperBLL.ReadCourseIDStr(currentUserNoPassPaperList.FindAll(delegate(TestPaperInfo TempModel) { return (TempModel.TestDate >= startDate && TempModel.TestDate <= endDate); })));
                        //��������
                    }

                    if (string.IsNullOrEmpty(NoPassPostCateId))
                    {
                        NoPassCourseNum = 0;
                    }
                    else
                    {
                        NoPassCourseNum = NoPassPostCateId.Split(',').Length;
                    }

                    //�����˾ID��ͬ���ٴλ�ȡ��˾��Ϣ�������ظ�����
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
                        //���㵽�ϸ�ʱ��ѧ���Ŀγ���
                        //�㷨(����������ͨ���Ŀγ�-���ϸ�ʱ���������ɵĿγ�=ʱ����������γ�)
                        //ʱ����������γ����λ�ƻ���ͬ�Ĳ���=ʱ����������γ�
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
