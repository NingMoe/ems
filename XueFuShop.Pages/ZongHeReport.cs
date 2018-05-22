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
            base.Title = "�ۺ����ͳ�Ʊ�";
            base.CheckUserPower("ReadEMSReport", PowerCheckType.Single);

            if (!base.ExistsSonCompany || Action == "Search" || !base.CompareUserPower("ManageGroupCompany", PowerCheckType.Single))
            {
                if (EndDate == DateTime.MinValue) EndDate = DateTime.Today;
                List<CompanyInfo> sonCompanyList = new List<CompanyInfo>();
                if (CompanyID <= 0) CompanyID = base.UserCompanyID;

                CompanyModel = CompanyBLL.ReadCompany(CompanyID);

                if (base.CompareUserPower("ManageGroupCompany", PowerCheckType.Single))
                {
                    //����ʹ�õݹ鷽����Ƶ��
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
                MonthOut.Append("<tr><th colspan=\"11\">" + CompanyModel.CompanyName + " ��ְ��Ա����ͳ�� [");
                if (StartDate != DateTime.MinValue) MonthOut.Append("��" + StartDate.ToString("d"));
                else MonthOut.Append("����Ŀ����");
                MonthOut.Append("��" + EndDate.ToString("d") + "]</th></tr>");
                MonthOut.Append("<tr>");
                MonthOut.Append("<th>����</th>");
                MonthOut.Append("<th>��Ŀ����ʱ��</th>");
                MonthOut.Append("<th>ϵͳ�Ǽ�<br />����</th>");
                MonthOut.Append("<th>�ۼƲμ�<br />��������</th>");
                MonthOut.Append("<th>�ۼ�ѧϰ<br />���Դ���</th>");
                MonthOut.Append("<th>����ͨ��<br />�γ�����</th>");
                MonthOut.Append("<th>�˾�ѧϰ<br />�γ�����</th>");
                MonthOut.Append("<th>�˾�ͨ��<br />�γ�����</th>");
                MonthOut.Append("<th>�ο���Ա<br />�˾�ѧϰ<br />�γ�����</th>");
                MonthOut.Append("<th>�ο���Ա<br />�˾�ͨ��<br />�γ�����</th>");
                MonthOut.Append("<th>�γ̿���<br />ͨ����</th>");
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
                            //Ӧ��������ȥ���ڴ���������ˣ�
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

                            double SinglePassRate; //����γ�ͨ����
                            if (CourseNum == 0)
                                SinglePassRate = 0;
                            else
                                SinglePassRate = (double)PassCourseNum / (double)CourseNum;

                            if (SinglePassRate >= 0.22 || _CompanyModel.CompanyId == CompanyID)
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

                            MonthOut.Append("<tr>");
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
                MonthOut.Append("<td colspan=\"2\">" + CompanyModel.CompanySimpleName + "��Чѧϰ�ϼ�</td>");
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
                MonthOut.Insert(0, "������EMS����ϵͳ�н�����Чѧϰ������Ϊ <span style=\"color:#FF0000;\">" + TotalNum.ToString() + " ��</span></li><li>�ۼƽ����� <span style=\"color:#FF0000;\">" + AllCourseNum.ToString() + " ��</span>�γ̵�ѧϰ���˾������� <span style=\"color:#FF0000;\">" + Math.Round(((double)AllCourseNum / TotalNum), 1) + " ��</span>�γ̿���</li><li>����ѧϰͨ�� <span style=\"color:#FF0000;\">" + AllPassCourseNum + " ��</span>�γ̿��ԣ��˾�ͨ�� <span style=\"color:#FF0000;\">" + Math.Round(((double)AllPassCourseNum / TotalNum), 1) + " ��</span>�γ�</li><li>�γ�ѧϰ�Ŀ���ͨ����Ϊ <span style=\"color:#FF0000;\">" + ((double)AllPassCourseNum / (double)AllCourseNum).ToString("P") + "</span></li></ul>");

                MonthOut.Insert(0, "��" + EndDate.AddDays(-1).ToString("d"));
                if (StartDate != DateTime.MinValue) MonthOut.Insert(0, "��" + StartDate.ToString("d"));
                else MonthOut.Insert(0, "����Ŀ����");
                if (CompanyModel.GroupId == 1 || CompanyModel.GroupId == 2)
                    MonthOut.Insert(0, "����");
                else
                    MonthOut.Insert(0, "��˾");
                MonthOut.Insert(0, "<ul class=\"ReportList_count\"><li>");

                MonthOut.Append("<ul class=\"ReportList_count\"><li><font color=\"red\">�˾�ѧϰ�γ�������</font>�����ŶӶ�ѧϰ���ӵĳ̶ȣ�Խ�ߴ���Խ����ѧϰ������<span style=\"color:#FF0000;\">������ÿ���˾�2.2�ſγ�,ÿ���˾�8.5�ſγ�</span>������ָ�����벿���쵼��ѧϰ�Ӵ��ע��</li>");
                if (StartDate != DateTime.MinValue && (EndDate - StartDate).Days <= 7 && !string.IsNullOrEmpty(AverageCourseNumCompanyName) && (CompanyModel.GroupId == 1 || CompanyModel.GroupId == 2))
                {
                    MonthOut.Append("<li style=\"color:#3366ff;\">Ŀǰ�˾�ѧϰ��������ÿ���˾�2.2�ſγ̵��Ŷ��У�<span style=\"color:#FF0000;\">" + AverageCourseNumCompanyName + "</span>��������쵼���Թ�ע��</li>");
                    // else if (string.IsNullOrEmpty(AverageCourseNumCompanyName) && (CompanyModel.GroupId == 1 || CompanyModel.GroupId == 2))
                    MonthOut.Append("<li style=\"color:#3366ff;\">Ŀǰ��Ч�˾�ѧϰ����Ϊ:  <span style=\"color:#FF0000;\">" + Math.Round(((double)AllCourseNum / TotalNum), 1) + " ��</span>��");
                    if (Math.Round(((double)AllCourseNum / TotalNum), 1) >= 2.2)
                        MonthOut.Append("�������ã����ٽ�������</li>");
                    else
                        MonthOut.Append("������쵼���Թ�ע��</li>");
                }
                MonthOut.Append("<li><font color=\"red\">�γ̿���ͨ���ʣ�</font>ͨ���ʸߴ����Ŷ�ѧϰ���棬��<span style=\"color:#FF0000;\">ͨ���ʵ���22%</span>����ʾѧԱû��ѧϰ�γ̣�ֻ��ͨ�����Ͽ�����<span style=\"color:#FF0000;\">��ͼ�¹�����</span>���벿���쵼��ֹ������ѧϰ��Ϊ��</li>");
                if (!string.IsNullOrEmpty(NoPassCompanyName) && (CompanyModel.GroupId == 1 || CompanyModel.GroupId == 2))
                    MonthOut.Append("<li style=\"color:#3366ff;\">Ŀǰͨ���ʵ���22%���Ŷ���: <span style=\"color:#FF0000;\">" + NoPassCompanyName + "</span>��������쵼���Թ�ע������4S����Ϊ��Чѧϰ��ȫ�����ݲ����뼯��ѧϰЧ��ͳ�ơ�</li>");
                else if (string.IsNullOrEmpty(NoPassCompanyName) && (CompanyModel.GroupId == 1 || CompanyModel.GroupId == 2))
                    MonthOut.Append("<li style=\"color:#3366ff; \">Ŀǰͨ����Ϊ: <span style=\"color:#FF0000;\">" + ((double)AllPassCourseNum / (double)AllCourseNum).ToString("P") + "</span>���������ã����ٽ�������</li>");
                MonthOut.Append("</ul>");

                //���ͨ����λ����
                MonthOut.Append("<div class=\"split_line\"></div>");
                MonthOut.Append("<table class=\"evaluation_sheet\">");
                MonthOut.Append("<tr><th colspan=\"9\">" + CompanyModel.CompanyName + " ��λ������֤ͳ�� [");
                if (StartDate != DateTime.MinValue) MonthOut.Append("��" + StartDate.ToString("d"));
                else MonthOut.Append("����Ŀ����");
                MonthOut.Append("��" + EndDate.AddDays(-1).ToString("d") + "]</th></tr>");
                MonthOut.Append("<tr>");
                MonthOut.Append("<th>����</th>");
                MonthOut.Append("<th>����</th>");
                MonthOut.Append("<th>��ְ��λ</th>");
                MonthOut.Append("<th>��ͨ����<br />��֤��λ</th>");
                //MonthOut.Append("<th>��λ����<br />�γ�����</th>");
                MonthOut.Append("<th>�ۼ�ͨ��<br />��λ����</th>");
                MonthOut.Append("<th>Ŀǰ����<br />ѧϰ��λ</th>");
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
            MonthOut.Append("<tr><th colspan=\"5\">" + CompanyModel.CompanyName + " ��ʷ�������� [");
            MonthOut.Append("����Ŀ����");
            MonthOut.Append("��" + EndDate.AddDays(-1).ToString("d") + "]</th></tr>");
            MonthOut.Append("<tr>");
            MonthOut.Append("<th>����</th>");
            MonthOut.Append("<th>ϵͳ�Ǽ�<br />ʹ������</th>");
            MonthOut.Append("<th>�ۼƲμ�<br />��������</th>");
            MonthOut.Append("<th>�ۼ�ѧϰ<br />���Դ���</th>");
            MonthOut.Append("<th>����ͨ��<br />�γ�����</th>");
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
                        if (Num < UserNum) Num = UserNum;  //���ϵͳ��ʷ��¼����С����ʷ���Լ�¼����������ʷ���Լ�¼����Ϊ׼
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
            MonthOut.Append("<td>" + CompanyModel.CompanySimpleName + "�ϼ�</td>");
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
