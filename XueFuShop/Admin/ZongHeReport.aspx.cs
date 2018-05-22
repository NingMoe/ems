using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class ZongHeReport : AdminBasePage
    {
        protected int companyID = RequestHelper.GetQueryString<int>("CompanyId");
        protected string action = RequestHelper.GetQueryString<string>("Action");
        protected string postIdCondition = RequestHelper.GetQueryString<string>("PostIdCondition");
        protected string studyPostIdCondition = RequestHelper.GetQueryString<string>("StudyPostIdCondition");
        protected DateTime startDate = DateTime.MinValue;
        protected DateTime endDate = DateTime.MinValue;
        protected string companyName = string.Empty;
        protected string groupID = RequestHelper.GetQueryString<string>("GroupID");
        protected string state = RequestHelper.GetQueryString<string>("State");
        protected CompanyInfo companyModel = new CompanyInfo();

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
                if (action == "search" && companyID > 0)
                {
                    startDate = RequestHelper.GetQueryString<DateTime>("SearchStartDate");
                    endDate = RequestHelper.GetQueryString<DateTime>("SearchEndDate");
                    SearchStartDate.Text = startDate.ToString("d");
                    SearchEndDate.Text = endDate.ToString("d");
                    companyModel = CompanyBLL.ReadCompany(companyID);
                    companyName = companyModel.CompanyName;

                    CompanyInfo companySearch = new CompanyInfo();
                    companySearch.Field = "CompanyId";
                    companySearch.Condition = CompanyBLL.ReadCompanyIdList(companyID.ToString());
                    companySearch.State = 0;
                    companySearch.GroupIdCondition = "0,3";
                    List<CompanyInfo> companyList = CompanyBLL.ReadCompanyList(companySearch);

                    //��Чѧϰͳ�ƿ�ʼ
                    int AllPassCourseNum = 0, AllCourseNum = 0, AllUserNum = 0, TotalNum = 0, CompanyNum = 0;
                    double PassAverage = 0.0, PerPassAverage = 0.0, PerAllAverage = 0.0;
                    string NoPassCompanyName = string.Empty, AverageCourseNumCompanyName = string.Empty;

                    StringBuilder MonthOut = new StringBuilder();
                    MonthOut.Append("<table class=\"listTable\">");
                    MonthOut.Append("<tr class=\"listTableHead\"><td colspan=\"11\">" + companyModel.CompanyName + " ��Ա����ͳ��[");
                    if (startDate != DateTime.MinValue) MonthOut.Append("��" + startDate.ToString("d"));
                    else MonthOut.Append("����Ŀ����");
                    MonthOut.Append("��" + endDate.ToString("d") + "]</td></tr>");
                    MonthOut.Append("<tr class=\"listTableHead\">");
                    MonthOut.Append("<td>����</td>");
                    MonthOut.Append("<td>��Ŀ����ʱ��</td>");
                    MonthOut.Append("<td>ϵͳ�Ǽ�<br />Ӧ������</td>");
                    MonthOut.Append("<td>�ۼƲμ�<br />��������</td>");
                    MonthOut.Append("<td>�ۼ�ѧϰ<br />���Դ���</td>");
                    MonthOut.Append("<td>����ͨ��<br />�γ�����</td>");
                    MonthOut.Append("<td>�˾�ѧϰ<br />�γ�����</td>");
                    MonthOut.Append("<td>�˾�ͨ��<br />�γ�����</td>");
                    MonthOut.Append("<td>�ο���Ա<br />�˾�ѧϰ<br />�γ�����</td>");
                    MonthOut.Append("<td>�ο���Ա<br />�˾�ͨ��<br />�γ�����</td>");
                    MonthOut.Append("<td>�γ̿���<br />ͨ����</td>");
                    MonthOut.Append("</tr>");

                    endDate = endDate.AddDays(1);
                    foreach (CompanyInfo _CompanyModel in companyList)
                    {
                        if (!string.IsNullOrEmpty(_CompanyModel.PostStartDate.ToString()) && Convert.ToDateTime(_CompanyModel.PostStartDate.ToString()) < endDate)
                        {
                            int PassCourseNum = 0, CourseNum = 0, UserNum = 0, Num = 0;
                            UserSearchInfo userSearch = new UserSearchInfo();
                            userSearch.InCompanyID = _CompanyModel.CompanyId.ToString();
                            userSearch.InStatus = state;
                            userSearch.InGroupID = groupID;
                            userSearch.InWorkingPostID = postIdCondition;
                            userSearch.InStudyPostID = studyPostIdCondition;
                            List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
                            Num = userList.Count;

                            TestPaperInfo TestPaperModel = new TestPaperInfo();
                            TestPaperModel.TestMinDate = startDate;
                            TestPaperModel.TestMaxDate = endDate;
                            TestPaperModel.UserIdCondition = UserBLL.ReadUserIdStr(userList);
                            TestPaperModel.CompanyIdCondition = _CompanyModel.CompanyId.ToString();
                            List<TestPaperInfo> TempList = TestPaperBLL.NewReadList(TestPaperModel);
                            string UserId = string.Empty;
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
                            double SinglePassRate; //����γ�ͨ����
                            if (CourseNum == 0)
                                SinglePassRate = 0;
                            else
                                SinglePassRate = (double)PassCourseNum / (double)CourseNum;

                            if (SinglePassRate >= 0.22 || _CompanyModel.CompanyId == companyID)
                            {
                                CompanyNum += 1; //ͨ���ʴ���22%�Ĺ�˾����
                                TotalNum += Num;  //������
                                AllUserNum += UserNum; //�ܲμӿ�������
                                AllCourseNum += CourseNum; //�ܿ��Կγ���
                                AllPassCourseNum += PassCourseNum;  //��ͨ�����Կγ���
                            }
                            else
                            {
                                NoPassCompanyName += _CompanyModel.CompanySimpleName + " ";
                            }

                            MonthOut.Append("<tr class=\"listTableMain\">");
                            MonthOut.Append("<td>" + _CompanyModel.CompanySimpleName + "</td>");
                            MonthOut.Append("<td>" + Convert.ToDateTime(_CompanyModel.PostStartDate.ToString()).ToString("yyyy��M��") + "</td>");
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
                                if (startDate == DateTime.MinValue || (endDate - startDate).Days > 7 || Math.Round(((double)CourseNum / Num), 1) >= 2.2)
                                {
                                    MonthOut.Append("<td>" + Math.Round(((double)CourseNum / Num), 1) + "</td>");
                                }
                                else
                                {
                                    AverageCourseNumCompanyName += _CompanyModel.CompanySimpleName + " ";
                                    MonthOut.Append("<td style=\"background:#FF0000;\">" + Math.Round(((double)CourseNum / Num), 1) + "</td>");
                                }
                                MonthOut.Append("<td>" + Math.Round(((double)PassCourseNum / Num), 1) + "</td>");
                                if (SinglePassRate >= 0.22 || _CompanyModel.CompanyId == companyID)
                                {
                                    PerAllAverage += Math.Round(((double)CourseNum / Num), 1);
                                    PerPassAverage += Math.Round(((double)PassCourseNum / Num), 1);
                                }
                            }
                            //����ο���Ա�˾�ֵ
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
                            if (SinglePassRate >= 0.22 || _CompanyModel.CompanyId == companyID)
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
                    MonthOut.Append("<tr class=\"listTableHead\">");
                    MonthOut.Append("<td colspan=\"2\">" + companyModel.CompanySimpleName + "��Чѧϰ�ϼ�</td>");
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
                    if (AllCourseNum <= 0)
                        MonthOut.Insert(0, "<p>�γ�ѧϰ�Ŀ���ͨ����Ϊ <span style=\"color:#FF0000;\"> 0 </span></p><br /></div>");
                    else
                        MonthOut.Insert(0, "<p>�γ�ѧϰ�Ŀ���ͨ����Ϊ <span style=\"color:#FF0000;\">" + ((double)AllPassCourseNum / (double)AllCourseNum).ToString("P") + "</span></p><br /></div>");
                    MonthOut.Insert(0, "������EMS����ϵͳ�н�����Чѧϰ������Ϊ <span style=\"color:#FF0000;\">" + TotalNum.ToString() + " ��</span></p><br /><p>�ۼƽ����� <span style=\"color:#FF0000;\">" + AllCourseNum.ToString() + " ��</span>�γ̵�ѧϰ���˾������� <span style=\"color:#FF0000;\">" + Math.Round(((double)AllCourseNum / TotalNum), 1) + " ��</span>�γ̿���</p><br /><p>����ѧϰͨ�� <span style=\"color:#FF0000;\">" + AllPassCourseNum + " ��</span>�γ̿��ԣ��˾�ͨ�� <span style=\"color:#FF0000;\">" + Math.Round(((double)AllPassCourseNum / TotalNum), 1) + " ��</span>�γ�</p><br />");

                    MonthOut.Insert(0, "��" + endDate.AddDays(-1).ToString("d"));
                    if (startDate != DateTime.MinValue) MonthOut.Insert(0, "��" + startDate.ToString("d"));
                    else MonthOut.Insert(0, "����Ŀ����");
                    if (companyModel.GroupId == 1 || companyModel.GroupId == 2)
                        MonthOut.Insert(0, "����");
                    else
                        MonthOut.Insert(0, "��˾");
                    MonthOut.Insert(0, "<div style=\" text-align:left; font-size:14px;\"><p>");

                    MonthOut.Append("<div style=\"line-height:25px; text-align:left; font-size:14px;\"><p style=\"text-align:left; font-size:14px;\"><font color=\"red\">�˾�ѧϰ�γ�������</font>�����ŶӶ�ѧϰ���ӵĳ̶ȣ�Խ�ߴ���Խ����ѧϰ������<span style=\"color:#FF0000;\">������ÿ���˾�2.2�ſγ�,ÿ���˾�8.5�ſγ�</span>������ָ�����벿���쵼��ѧϰ�Ӵ��ע��</p>");
                    if (startDate != DateTime.MinValue && (endDate - startDate).Days <= 7 && !string.IsNullOrEmpty(AverageCourseNumCompanyName) && (companyModel.GroupId == 1 || companyModel.GroupId == 2))
                    {
                        MonthOut.Append("<p style=\"text-align:left; color:#3366ff; font-size:14px;\">Ŀǰ�˾�ѧϰ��������ÿ���˾�2.2�ſγ̵��Ŷ��У�<span style=\"color:#FF0000;\">" + AverageCourseNumCompanyName + "</span>��������쵼���Թ�ע��</p>");
                        // else if (string.IsNullOrEmpty(AverageCourseNumCompanyName) && (CompanyModel.GroupId == 1 || CompanyModel.GroupId == 2))
                        MonthOut.Append("<p style=\"text-align:left; color:#3366ff; font-size:14px;\">Ŀǰ��Ч�˾�ѧϰ����Ϊ:  <span style=\"color:#FF0000;\">" + Math.Round(((double)AllCourseNum / TotalNum), 1) + " ��</span>��");
                        if (Math.Round(((double)AllCourseNum / TotalNum), 1) >= 2.2)
                            MonthOut.Append("�������ã����ٽ�������</p>");
                        else
                            MonthOut.Append("������쵼���Թ�ע��</p>");
                    }
                    MonthOut.Append("<br />");
                    MonthOut.Append("<p style=\"text-align:left; font-size:14px;\"><font color=\"red\">�γ̿���ͨ���ʣ�</font>ͨ���ʸߴ����Ŷ�ѧϰ���棬��<span style=\"color:#FF0000;\">ͨ���ʵ���22%</span>����ʾѧԱû��ѧϰ�γ̣�ֻ��ͨ�����Ͽ�����<span style=\"color:#FF0000;\">��ͼ�¹�����</span>���벿���쵼��ֹ������ѧϰ��Ϊ��</p>");
                    if (!string.IsNullOrEmpty(NoPassCompanyName) && (companyModel.GroupId == 1 || companyModel.GroupId == 2))
                        MonthOut.Append("<p style=\"text-align:left; color:#3366ff; font-size:14px;\">Ŀǰͨ���ʵ���22%���Ŷ���: <span style=\"color:#FF0000;\">" + NoPassCompanyName + "</span>��������쵼���Թ�ע������4S����Ϊ��Чѧϰ��ȫ�����ݲ����뼯��ѧϰЧ��ͳ�ơ�</p>");
                    else if (string.IsNullOrEmpty(NoPassCompanyName) && (companyModel.GroupId == 1 || companyModel.GroupId == 2))
                        MonthOut.Append("<p style=\"text-align:left; color:#3366ff; font-size:14px;\">Ŀǰͨ����Ϊ: <span style=\"color:#FF0000;\">" + ((double)AllPassCourseNum / (double)AllCourseNum).ToString("P") + "</span>���������ã����ٽ�������</p>");
                    MonthOut.Append("</div>");

                    //��Чѧϰͳ�ƽ���


                    //��Чѧϰͳ�ƿ�ʼ
                    //AllPassCourseNum = 0;
                    //AllCourseNum = 0;
                    //AllUserNum = 0;
                    //TotalNum = 0;
                    //CompanyNum = 0;

                    //MonthOut.Append("<br><br>");
                    //MonthOut.Append("<table class=\"listTable\">");
                    //MonthOut.Append("<tr class=\"listTableHead\"><td colspan=\"11\">" + companyModel.CompanyName + " ��Ա����ͳ��(����ɾ����Ա��������п�����Ա) [");
                    //if (startDate != DateTime.MinValue) MonthOut.Append("��" + startDate.ToString("d"));
                    //else MonthOut.Append("����Ŀ����");
                    //MonthOut.Append("��" + endDate.AddDays(-1).ToString("d") + "]</td></tr>");
                    //MonthOut.Append("<tr class=\"listTableHead\">");
                    //MonthOut.Append("<td>����</td>");
                    //MonthOut.Append("<td>��Ŀ����ʱ��</td>");
                    //MonthOut.Append("<td>ϵͳ�Ǽ�<br />����</td>");
                    //MonthOut.Append("<td>�ۼƲμ�<br />��������</td>");
                    //MonthOut.Append("<td>�ۼ�ѧϰ<br />���Դ���</td>");
                    //MonthOut.Append("<td>����ͨ��<br />�γ�����</td>");
                    //MonthOut.Append("<td>�˾�ѧϰ<br />�γ�����</td>");
                    //MonthOut.Append("<td>�˾�ͨ��<br />�γ�����</td>");
                    //MonthOut.Append("<td>�ο���Ա<br />�˾�ѧϰ<br />�γ�����</td>");
                    //MonthOut.Append("<td>�ο���Ա<br />�˾�ͨ��<br />�γ�����</td>");
                    //MonthOut.Append("<td>�γ̿���<br />ͨ����</td>");
                    //MonthOut.Append("</tr>");

                    //foreach (CompanyInfo _CompanyModel in companyList)
                    //{
                    //    if (!string.IsNullOrEmpty(_CompanyModel.PostStartDate.ToString()) && Convert.ToDateTime(_CompanyModel.PostStartDate.ToString()) < endDate)
                    //    {
                    //        int PassCourseNum = 0, CourseNum = 0, UserNum = 0, Num = 0;
                    //        UserSearchInfo userSearch = new UserSearchInfo();
                    //        userSearch.InCompanyID = _CompanyModel.CompanyId.ToString();
                    //        userSearch.InStatus = state;
                    //        userSearch.InGroupID = groupID;
                    //        userSearch.InWorkingPostID = postIdCondition;
                    //        userSearch.InStudyPostID = studyPostIdCondition;
                    //        List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
                    //        Num = userList.Count;
                    //        //Ӧ��������ȥ���ڴ���������ˣ�
                    //        //int YKNum = userList.FindAll(delegate(UserInfo TempModel) { return TempModel.Status == (int)UserState.Normal; }).Count;
                    //        //Num = YKNum;

                    //        TestPaperInfo TestPaperModel = new TestPaperInfo();
                    //        TestPaperModel.TestMinDate = startDate;
                    //        TestPaperModel.TestMaxDate = endDate;
                    //        TestPaperModel.UserIdCondition = UserBLL.ReadUserIdStr(userList);
                    //        TestPaperModel.CompanyIdCondition = _CompanyModel.CompanyId.ToString();
                    //        List<TestPaperInfo> TempList = TestPaperBLL.NewReadList(TestPaperModel);
                    //        string UserId = string.Empty;
                    //        foreach (TestPaperInfo Info in TempList)
                    //        {
                    //            if (!StringHelper.CompareSingleString(UserId, Info.UserId.ToString()))
                    //            {
                    //                UserId += "," + Info.UserId.ToString();
                    //            }
                    //            if (Info.IsPass == 1)
                    //            {
                    //                PassCourseNum += 1;
                    //            }
                    //        }
                    //        if (UserId.StartsWith(",")) UserId = UserId.Substring(1);
                    //        if (string.IsNullOrEmpty(UserId))
                    //        {
                    //            UserNum = 0;
                    //        }
                    //        else
                    //        {
                    //            UserNum = UserId.Split(',').Length;
                    //        }

                    //        CourseNum = TempList.Count;
                    //        double SinglePassRate; //����γ�ͨ����
                    //        if (CourseNum == 0)
                    //            SinglePassRate = 0;
                    //        else
                    //            SinglePassRate = (double)PassCourseNum / (double)CourseNum;

                    //        if (SinglePassRate >= 0.22 || _CompanyModel.CompanyId == companyID)
                    //        {
                    //            CompanyNum += 1; //ͨ���ʴ���22%�Ĺ�˾����
                    //            TotalNum += Num;  //������
                    //            AllUserNum += UserNum; //�ܲμӿ�������
                    //            AllCourseNum += CourseNum; //�ܿ��Կγ���
                    //            AllPassCourseNum += PassCourseNum;  //��ͨ�����Կγ���
                    //        }
                    //        else
                    //        {
                    //            NoPassCompanyName += _CompanyModel.CompanySimpleName + " ";
                    //        }

                    //        MonthOut.Append("<tr class=\"listTableMain\">");
                    //        MonthOut.Append("<td>" + _CompanyModel.CompanySimpleName + "</td>");
                    //        MonthOut.Append("<td>" + Convert.ToDateTime(_CompanyModel.PostStartDate.ToString()).ToString("yyyy��M��") + "</td>");
                    //        MonthOut.Append("<td>" + Num.ToString() + "</td>");
                    //        MonthOut.Append("<td>" + UserNum.ToString() + "</td>");
                    //        MonthOut.Append("<td>" + CourseNum + "</td>");
                    //        MonthOut.Append("<td>" + PassCourseNum + "</td>");
                    //        if (Num == 0)
                    //        {
                    //            MonthOut.Append("<td>0</td>");
                    //            MonthOut.Append("<td>0</td>");
                    //        }
                    //        else
                    //        {
                    //            if (startDate == DateTime.MinValue || (endDate - startDate).Days > 7 || Math.Round(((double)CourseNum / Num), 1) >= 2.2)
                    //            {
                    //                MonthOut.Append("<td>" + Math.Round(((double)CourseNum / Num), 1) + "</td>");
                    //            }
                    //            else
                    //            {
                    //                AverageCourseNumCompanyName += _CompanyModel.CompanySimpleName + " ";
                    //                MonthOut.Append("<td style=\"background:#FF0000;\">" + Math.Round(((double)CourseNum / Num), 1) + "</td>");
                    //            }
                    //            MonthOut.Append("<td>" + Math.Round(((double)PassCourseNum / Num), 1) + "</td>");
                    //            if (SinglePassRate >= 0.22 || _CompanyModel.CompanyId == companyID)
                    //            {
                    //                PerAllAverage += Math.Round(((double)CourseNum / Num), 1);
                    //                PerPassAverage += Math.Round(((double)PassCourseNum / Num), 1);
                    //            }
                    //        }
                    //        //����ο���Ա�˾�ֵ
                    //        if (UserNum == 0)
                    //        {
                    //            MonthOut.Append("<td>0</td>");
                    //            MonthOut.Append("<td>0</td>");
                    //        }
                    //        else
                    //        {
                    //            MonthOut.Append("<td>" + Math.Round(((double)CourseNum / UserNum), 1) + "</td>");
                    //            MonthOut.Append("<td>" + Math.Round(((double)PassCourseNum / UserNum), 1) + "</td>");
                    //        }
                    //        if (SinglePassRate >= 0.22 || _CompanyModel.CompanyId == companyID)
                    //        {
                    //            MonthOut.Append("<td>" + SinglePassRate.ToString("P") + "</td>");
                    //            PassAverage += SinglePassRate;
                    //        }
                    //        else
                    //        {
                    //            MonthOut.Append("<td style=\"background:#FF0000;\">" + SinglePassRate.ToString("P") + "</td>");
                    //        }
                    //        MonthOut.Append("</tr>");
                    //    }
                    //}
                    //MonthOut.Append("<tr class=\"listTableHead\">");
                    //MonthOut.Append("<td colspan=\"2\">" + companyModel.CompanySimpleName + "��Чѧϰ�ϼ�</td>");
                    //MonthOut.Append("<td>" + TotalNum.ToString() + "</td>");
                    //MonthOut.Append("<td>" + AllUserNum.ToString() + "</td>");
                    //MonthOut.Append("<td>" + AllCourseNum + "</td>");
                    //MonthOut.Append("<td>" + AllPassCourseNum + "</td>");
                    //if (TotalNum == 0)
                    //{
                    //    MonthOut.Append("<td>0</td>");
                    //    MonthOut.Append("<td>0</td>");
                    //}
                    //else
                    //{
                    //    //MonthOut.Append("<td>" + Math.Round((PerAllAverage / CompanyNum), 1) + "</td>");
                    //    //MonthOut.Append("<td>" + Math.Round((PerPassAverage / CompanyNum), 1) + "</td>");
                    //    //MonthOut.Append("<td>" + (PassAverage / CompanyNum).ToString("P") + "</td>");
                    //    MonthOut.Append("<td>" + Math.Round(((double)AllCourseNum / TotalNum), 1) + "</td>");
                    //    MonthOut.Append("<td>" + Math.Round(((double)AllPassCourseNum / TotalNum), 1) + "</td>");
                    //}
                    //if (AllUserNum == 0)
                    //{
                    //    MonthOut.Append("<td>0</td>");
                    //    MonthOut.Append("<td>0</td>");
                    //}
                    //else
                    //{
                    //    MonthOut.Append("<td>" + Math.Round(((double)AllCourseNum / AllUserNum), 1) + "</td>");
                    //    MonthOut.Append("<td>" + Math.Round(((double)AllPassCourseNum / AllUserNum), 1) + "</td>");
                    //}
                    //if (AllCourseNum == 0)
                    //    MonthOut.Append("<td>0</td>");
                    //else
                    //    MonthOut.Append("<td>" + ((double)AllPassCourseNum / (double)AllCourseNum).ToString("P") + "</td>");
                    //MonthOut.Append("</tr>");
                    //MonthOut.Append("</table>");
                    //MonthOut.Append("<br><br>");
                    //��Чѧϰͳ�ƽ���


                    //��λ������֤ͳ�ƿ�ʼ
                    MonthOut.Append("<table class=\"listTable\">");
                    MonthOut.Append("<tr class=\"listTableHead\"><td colspan=\"9\">" + companyModel.CompanyName + " ��λ������֤ͳ�� [");
                    if (startDate != DateTime.MinValue) MonthOut.Append("��" + startDate.ToString("d"));
                    else MonthOut.Append("����Ŀ����");
                    MonthOut.Append("��" + endDate.AddDays(-1).ToString("d") + "]</td></tr>");
                    MonthOut.Append("<tr class=\"listTableHead\">");
                    MonthOut.Append("<td>����</td>");
                    MonthOut.Append("<td>����</td>");
                    MonthOut.Append("<td>��ְ��λ</td>");
                    MonthOut.Append("<td>��ͨ����<br />��֤��λ</td>");
                    //MonthOut.Append("<td>��λ����<br />�γ�����</td>");
                    MonthOut.Append("<td>�ۼ�ͨ��<br />��λ����</td>");
                    MonthOut.Append("<td>Ŀǰ����<br />ѧϰ��λ</td>");
                    MonthOut.Append("</tr>");
                    foreach (CompanyInfo _CompanyModel in companyList)
                    {
                        PostPassInfo PostPassModel = new PostPassInfo();
                        PostPassModel.SearchStartDate = startDate;
                        PostPassModel.CreateDate = endDate;
                        List<ReportPostPassInfo> PostPassList = PostPassBLL.PostPassReportList(PostPassModel, _CompanyModel.CompanyId.ToString());
                        foreach (ReportPostPassInfo Info in PostPassList)
                        {
                            MonthOut.Append("<tr class=\"listTableMain\">");
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
                    MonthOut.Append("</table>");
                    //��λ������֤ͳ�ƽ���

                    ReportList.InnerHtml = MonthOut.ToString();
                    ReportList.InnerHtml += HistoryData(companyList, endDate);
                }
            }

        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            companyID = RequestHelper.GetForm<int>("CompanyID");
            if (companyID > 0)
            {
                string StartDate = SearchStartDate.Text;
                string EndDate = SearchEndDate.Text;
                try
                {
                    if (Convert.ToDateTime(EndDate) <= Convert.ToDateTime(StartDate))
                        ScriptHelper.Alert("����ȷѡ��ʱ��Σ�");
                    ResponseHelper.Redirect("ZongHeReport.aspx?Action=search&CompanyId=" + companyID.ToString() + "&SearchStartDate=" + StartDate + "&SearchEndDate=" + EndDate + "&StudyPostIdCondition=" + RequestHelper.GetForm<string>("StudyPostIdCondition") + "&PostIdCondition=" + RequestHelper.GetForm<string>("PostIdCondition") + "&GroupID=" + RequestHelper.GetForm<string>("GroupID") + "&State=" + RequestHelper.GetForm<string>("State"));
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

        protected string HistoryData(List<CompanyInfo> companyList, DateTime EndDate)
        {
            int AllPassCourseNum = 0, AllCourseNum = 0, AllUserNum = 0, TotalNum = 0;

            StringBuilder MonthOut = new StringBuilder();
            MonthOut.Append("<br /><table class=\"listTable\">");
            MonthOut.Append("<tr class=\"listTableHead\"><td colspan=\"5\">" + companyModel.CompanyName + " ��ʷ�������� [");
            MonthOut.Append("����Ŀ����");
            MonthOut.Append("��" + EndDate.AddDays(-1).ToString("d") + "]</td></tr>");
            MonthOut.Append("<tr class=\"listTableHead\">");
            MonthOut.Append("<td>����</td>");
            MonthOut.Append("<td>ϵͳ�Ǽ�<br />ʹ������</td>");
            MonthOut.Append("<td>�ۼƲμ�<br />��������</td>");
            MonthOut.Append("<td>�ۼ�ѧϰ<br />���Դ���</td>");
            MonthOut.Append("<td>����ͨ��<br />�γ�����</td>");
            MonthOut.Append("</tr>");

            foreach (CompanyInfo _CompanyModel in companyList)
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
                    if (Num < UserNum) Num = UserNum;  //���ϵͳ��ʷ��¼����С����ʷ���Լ�¼����������ʷ���Լ�¼����Ϊ׼
                    TotalNum += Num;
                    AllUserNum += UserNum;

                    MonthOut.Append("<tr class=\"listTableMain\">");
                    MonthOut.Append("<td>" + _CompanyModel.CompanySimpleName + "</td>");
                    MonthOut.Append("<td>" + Num.ToString() + "</td>");
                    MonthOut.Append("<td>" + UserNum.ToString() + "</td>");
                    MonthOut.Append("<td>" + CourseNum + "</td>");
                    MonthOut.Append("<td>" + PassCourseNum + "</td>");
                    MonthOut.Append("</tr>");
                }
            }
            MonthOut.Append("<tr class=\"listTableHead\">");
            MonthOut.Append("<td>" + companyModel.CompanySimpleName + "�ϼ�</td>");
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
