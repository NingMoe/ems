using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using System.Collections;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class PostPlanRate : UserManageBasePage
    {
        protected int CompanyID = RequestHelper.GetQueryString<int>("CompanyID");
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
            base.Title = "��λѧϰ���ȷ���";
            base.CheckUserPower("ReadEMSReport", PowerCheckType.Single);

            if (CompanyID < 0) CompanyID = base.UserCompanyID;
            CompanyInfo company = CompanyBLL.ReadCompany(CompanyID);
            PostList = PostBLL.ReadPostListByPostId(company.Post);
            if (base.ExistsSonCompany)
                userGroupList = AdminGroupBLL.ReadAdminGroupList(CompanyID, UserBLL.ReadUserGroupIDByCompanyID(base.SonCompanyID));
            else
                userGroupList = AdminGroupBLL.ReadAdminGroupList(CompanyID, UserBLL.ReadUserGroupIDByCompanyID(CompanyID.ToString()));

            //�û�Ȩ����Ĭ�ϸ�������Ա
            if (string.IsNullOrEmpty(groupID))
                groupID = "36";

            if (Action == "Search")
            {
                ReportContentHtml = HtmlOut1(company);
            }
        }

        protected string HtmlOut1(CompanyInfo company)
        {
            int ColNum = 8;
            StringBuilder TextOut = new StringBuilder();
            string CompanyBrandId = company.BrandId;

            string rowspan = string.Empty;
            TextOut.Append("<table class=\"evaluation_sheet\">");
            TextOut.AppendLine("<thead>");
            TextOut.Append("<tr>");
            TextOut.Append("<th colspan=\"" + ColNum + "\">" + company.CompanySimpleName + "</th>");
            TextOut.Append("</tr>\r\n");
            TextOut.Append("<tr>\r\n");
            TextOut.Append("<th class=\"id\">���</th>");
            TextOut.Append("<th class=\"name\">����</th>");
            TextOut.Append("<th class=\"post\" style=\"width:auto;\">������λ</th>");
            TextOut.Append("<th class=\"post\" data-sort=\"string\" style=\"width:auto;\">��ǰѧϰ��λ<i class=\"icon_arrow\"></i></th>");
            TextOut.Append("<th>��λ�γ���</th>");
            TextOut.Append("<th data-sort=\"int\">��ͨ���γ���<i class=\"icon_arrow\"></i></th>");
            TextOut.Append("<th data-sort=\"int\">ʣ��γ���<i class=\"icon_arrow\"></i></th>");
            TextOut.Append("<th data-sort=\"float\">��ɽ���<i class=\"icon_arrow\"></i></th>");
            TextOut.Append("</tr>\r\n");
            TextOut.AppendLine("</thead>");
            TextOut.AppendLine("<tbody>");

            TestPaperInfo testPaperSearch = new TestPaperInfo();
            testPaperSearch.IsPass = 1;
            testPaperSearch.Condition = "[UserID] in (select [ID] from [_User] where [companyID]=" + company.CompanyId.ToString() + " and [status]=" + (int)UserState.Normal;
            if (!string.IsNullOrEmpty(groupID))
                testPaperSearch.Condition += " and [GroupID] in (" + groupID + ")";
            if (!string.IsNullOrEmpty(PostIdCondition))
                testPaperSearch.Condition += " and [WorkingPostID] in (" + PostIdCondition + ")";
            if (!string.IsNullOrEmpty(StudyPostIdCondition))
                testPaperSearch.Condition += " and [StudyPostId] in (" + StudyPostIdCondition + ")";
            testPaperSearch.Condition += ")";
            List<TestPaperInfo> allTestPaperList = TestPaperBLL.NewReadList(testPaperSearch);

            UserSearchInfo userSearch = new UserSearchInfo();
            userSearch.Status = (int)UserState.Normal;
            userSearch.InWorkingPostID = PostIdCondition;
            userSearch.InCompanyID = company.CompanyId.ToString();
            userSearch.InStudyPostID = StudyPostIdCondition; //ֻ��ʾ��ѧϰ��λ�µ���Ա
            userSearch.InGroupID = groupID;
            List<UserInfo> userList = UserBLL.SearchReportUserList(userSearch);

            //��¼��λ�γ�ID��
            Dictionary<int, string> postCourseDic = new Dictionary<int, string>();
            int peoperNum = 0;
            foreach (UserInfo Info in userList)
            {
                int PostId = Info.StudyPostID;

                PostInfo PostModel = PostBLL.ReadPost(PostId);
                if (PostModel != null) //�ų���û�����ø�λ����
                {
                    if (!postCourseDic.ContainsKey(PostId))
                    {
                        postCourseDic.Add(PostId, PostBLL.ReadPostCourseID(company.CompanyId, PostId));
                    }
                    string postCourseID = postCourseDic[PostId];

                    //ɸѡ����ǰ�û�ID�ĳɼ��б�
                    List<TestPaperInfo> currentUserPassPaperList = allTestPaperList.FindAll(delegate(TestPaperInfo TempModel) { return TempModel.UserId == Info.ID && StringHelper.CompareSingleString(postCourseID, TempModel.CateId.ToString()); });

                    int postCourseNum = string.IsNullOrEmpty(postCourseID) ? 0 : postCourseID.Split(',').Length;
                    int passCourseNum = DistinctList(currentUserPassPaperList).Count;

                    peoperNum = peoperNum + 1;
                    TextOut.Append("<tr>");
                    TextOut.Append("<td>" + peoperNum + "</td>");
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
                    TextOut.Append("<td>" + postCourseNum + "</td>");
                    TextOut.Append("<td>" + passCourseNum + "</td>");
                    TextOut.Append("<td>" + (postCourseNum - passCourseNum) + "</td>");
                    TextOut.Append("<td>" + (postCourseNum > 0 ? ((double)passCourseNum / (double)postCourseNum).ToString("P") : "0.00%") + "</td>");
                    TextOut.Append("</tr>\r\n");

                    currentUserPassPaperList = null;
                }
                PostModel = null;
            }
            TextOut.AppendLine("</tbody>");
            TextOut.AppendLine("</table>");
            allTestPaperList = null;
            return TextOut.ToString();
        }

        /// <summary>
        /// ȥ���ظ�
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<TestPaperInfo> DistinctList(List<TestPaperInfo> list)
        {
            List<TestPaperInfo> resultList = new List<TestPaperInfo>();
            foreach (TestPaperInfo info in list)
            {
                if (!resultList.Contains(info))
                    resultList.Add(info);
            }
            return resultList;  //���ؼ���  
        }
    }
}
