using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using System.Collections;

namespace XueFuShop.Pages
{
    public class StaffEvaluateReport : UserManageBasePage
    {
        protected StringBuilder tableContent = new StringBuilder();
        protected string action = string.Empty;
        protected string userIdStr = RequestHelper.GetQueryString<string>("UserId");
        protected int evaluateNameId = RequestHelper.GetForm<int>("EvaluationName");
        protected int evaluateType = RequestHelper.GetForm<int>("EvaluationType");
        protected int companyID = RequestHelper.GetForm<int>("CompanyId");
        protected UserInfo user = new UserInfo();
        protected EvaluateNameInfo evaluateName = new EvaluateNameInfo();
        protected string companyIDString = string.Empty;
        protected int postID = RequestHelper.GetForm<int>("PostID");

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "综合能力评估";
            base.CheckUserPower("ReadTPRReport", PowerCheckType.Single);

            action = Request["Action"];
            if (action == "Detail")
            {
                evaluateNameId = RequestHelper.GetQueryString<int>("EvaluateNameId");
                evaluateType = RequestHelper.GetQueryString<int>("EvaluateType");
            }
            companyIDString = base.UserCompanyID.ToString();
            if (companyID == 0)
                companyIDString = base.SonCompanyID;
            else if (companyID > 0)
                companyIDString = companyID.ToString();
            if (!base.ExistsSonCompany) companyID = base.UserCompanyID;

            if (!string.IsNullOrEmpty(action))
            {
                KPISearchInfo kpiSearch = new KPISearchInfo();
                kpiSearch.ParentId = evaluateType.ToString();
                kpiSearch.CompanyID = CompanyBLL.SystemCompanyId.ToString() + "," + companyID.ToString();
                List<KPIInfo> kpiList = KPIBLL.SearchKPIList(kpiSearch);
                string kpiStr = string.Empty;
                //定义KPIId数组
                ArrayList IdArray = new ArrayList();
                if (kpiList.Count > 0)
                {
                    string[,] kpiArray = new string[kpiList.Count, 3];

                    foreach (KPIInfo info in kpiList)
                    {
                        IdArray.Add(info.ID);
                        if (string.IsNullOrEmpty(kpiStr))
                            kpiStr = info.ID.ToString();
                        else
                            kpiStr += "," + info.ID;
                        int i = IdArray.IndexOf(info.ID);
                        kpiArray[i, 0] = info.ID.ToString();
                        kpiArray[i, 1] = info.Name;
                        kpiArray[i, 2] = info.EvaluateInfo;
                    }

                    KPIEvaluateSearchInfo kpiEvaluate = new KPIEvaluateSearchInfo();
                    if (action == "Search" && string.IsNullOrEmpty(userIdStr))
                    {
                        //UserSearchInfo userSearch = new UserSearchInfo();
                        //userSearch.InCompanyID = base.UserCompanyID.ToString();
                        //userIdStr = UserBLL.ReadUserIdStr(UserBLL.SearchUserList(userSearch));

                        if (postID > 0 || !string.IsNullOrEmpty(companyIDString))
                        {
                            kpiEvaluate.Condition = "[UserID] in (select id from [_User] where [Status]!=" + (int)UserState.Del;
                            if (postID > 0)
                                kpiEvaluate.Condition += " And [WorkingPostID] in (" + postID.ToString() + ")";
                            if (!string.IsNullOrEmpty(companyIDString))
                                kpiEvaluate.Condition += " And [CompanyId] in (" + companyIDString + ")";
                            kpiEvaluate.Condition += ")";
                        }
                    }
                    else
                    {
                        user = UserBLL.ReadUser(int.Parse(userIdStr));
                        evaluateName = EvaluateNameBLL.ReadEvaluateName(evaluateNameId);
                        kpiEvaluate.UserId = userIdStr;
                    }
                    kpiEvaluate.EvaluateNameId = evaluateNameId;
                    kpiEvaluate.KPIdStr = kpiStr;
                    List<KPIEvaluateInfo> kpiEvaluateList = KPIEvaluateBLL.SearchKPIEvaluateList(kpiEvaluate);

                    ArrayList userIdArray = new ArrayList();//new ArrayList(userIdStr.Split(','));
                    if (kpiEvaluateList.Count > 0)
                    {
                        foreach (KPIEvaluateInfo info in kpiEvaluateList)
                        {
                            if (!userIdArray.Contains(info.UserId))
                                userIdArray.Add(info.UserId);
                        }
                    }
                    string[, ,] dataArray = new string[kpiList.Count + 1, userIdArray.Count, 4];
                    //评估人数组
                    ArrayList evaluateUserIdArray = new ArrayList();
                    if (kpiEvaluateList.Count > 0)
                    {
                        foreach (KPIEvaluateInfo info in kpiEvaluateList)
                        {
                            if (!evaluateUserIdArray.Contains(info.EvaluateUserId))
                                evaluateUserIdArray.Add(info.EvaluateUserId);
                            dataArray[IdArray.IndexOf(info.KPIId), userIdArray.IndexOf(info.UserId), evaluateUserIdArray.IndexOf(info.EvaluateUserId)] = info.Scorse.ToString();
                        }
                    }
                    if (action != "Detail")//companyID >= 0
                    {
                        tableContent.Append("<tr><th class=\"project\">项目</th><th class=\"content\">内容</th>");
                        foreach (int item in userIdArray)
                        {
                            tableContent.Append("<th class=\"roll\"><a href=\"?Action=Detail&UserId=" + item + "&EvaluateNameId=" + evaluateNameId + "&EvaluateType=" + evaluateType + "\">" + UserBLL.ReadUser(item).RealName + "</a></th>");
                        }
                        for (int i = 0; i < dataArray.GetLength(0) - 1; i++)
                        {

                            tableContent.Append("<tr><td>" + kpiArray[i, 1] + "</td><td>" + kpiArray[i, 2] + "</td>");
                            for (int j = 0; j < userIdArray.Count; j++)
                            {
                                double totalScore = 0;
                                int num = 0;
                                for (int k = 0; k < 4; k++)
                                {
                                    if (!string.IsNullOrEmpty(dataArray[i, j, k]) || dataArray[i, j, k] != null)
                                    {
                                        totalScore += double.Parse(dataArray[i, j, k]);
                                        num++;
                                    }
                                }
                                if (totalScore == 0)
                                    tableContent.Append("<td>&nbsp;</td>");
                                else
                                {
                                    tableContent.Append("<td>" + Math.Round((totalScore / num), 1) + "</td>");
                                    if (string.IsNullOrEmpty(dataArray[dataArray.GetLength(0) - 1, j, 0]) || dataArray[dataArray.GetLength(0) - 1, j, 0] == null)
                                        dataArray[dataArray.GetLength(0) - 1, j, 0] = Math.Round((totalScore / num), 1).ToString();
                                    else
                                        dataArray[dataArray.GetLength(0) - 1, j, 0] = (double.Parse(dataArray[dataArray.GetLength(0) - 1, j, 0]) + Math.Round((totalScore / num), 1)).ToString();
                                }
                            }
                            tableContent.Append("</tr>");
                        }
                        tableContent.Append("<tr class=\"count\"><td colspan=\"2\">合计</td>");
                        for (int j = 0; j < userIdArray.Count; j++)
                        {
                            //员工综合能力评估合计要除以2
                            if (evaluateType == 530)
                                tableContent.Append("<td>" + Math.Round(double.Parse(dataArray[dataArray.GetLength(0) - 1, j, 0]) / 2, 1) + "</td>");
                            else
                                tableContent.Append("<td>" + dataArray[dataArray.GetLength(0) - 1, j, 0] + "</td>");
                        }
                        tableContent.Append("</tr>");
                    }
                    else
                    {
                        tableContent.Append("<tr><th class=\"project\">项目</th><th class=\"content\">内容</th>");
                        foreach (int item in evaluateUserIdArray)
                        {
                            tableContent.Append("<th class=\"roll\">评估" + (evaluateUserIdArray.IndexOf(item) + 1) + "</th>");
                        }
                        tableContent.Append("<th class=\"average\">平均分</th></tr>");
                        for (int i = 0; i < dataArray.GetLength(0) - 1; i++)
                        {

                            tableContent.Append("<tr><td>" + kpiArray[i, 1] + "</td><td>" + kpiArray[i, 2] + "</td>");
                            double totalScore = 0;
                            int num = 0;
                            for (int j = evaluateUserIdArray.Count - 1; j >= 0; j--)//for (int j = 0; j < evaluateUserIdArray.Count; j++)
                            {
                                if (dataArray[i, 0, j] != null)
                                {
                                    tableContent.Append("<td>" + dataArray[i, 0, j] + "</td>");
                                    totalScore += double.Parse(dataArray[i, 0, j]);
                                    num++;

                                    if (string.IsNullOrEmpty(dataArray[dataArray.GetLength(0) - 1, 0, j]) || dataArray[dataArray.GetLength(0) - 1, 0, j] == null)
                                        dataArray[dataArray.GetLength(0) - 1, 0, j] = dataArray[i, 0, j];
                                    else
                                        dataArray[dataArray.GetLength(0) - 1, 0, j] = (double.Parse(dataArray[dataArray.GetLength(0) - 1, 0, j]) + double.Parse(dataArray[i, 0, j])).ToString();
                                }
                                else
                                    tableContent.Append("<td>&nbsp;</td>");
                            }
                            if (num == 0)
                                tableContent.Append("<td>&nbsp;</td>");
                            else
                                tableContent.Append("<td>" + Math.Round((totalScore / num), 1) + "</td></tr>");
                        }
                        //员工综合能力评估合计要除以2
                        if (evaluateType == 530)
                        {
                            tableContent.Append("<tr class=\"count\"><td colspan=\"2\">合计（总分200，除2后取得分数）</td>");
                            for (int j = evaluateUserIdArray.Count - 1; j >= 0; j--)
                            {
                                tableContent.Append("<td>" + Math.Round(double.Parse(dataArray[dataArray.GetLength(0) - 1, 0, j]) / 2, 1) + "</td>");
                            }
                        }
                        else
                        {
                            tableContent.Append("<tr class=\"count\"><td colspan=\"2\">合计</td>");
                            for (int j = evaluateUserIdArray.Count - 1; j >= 0; j--)
                            {
                                tableContent.Append("<td>" + Math.Round(double.Parse(dataArray[dataArray.GetLength(0) - 1, 0, j]), 1) + "</td>");
                            }
                        }
                        tableContent.Append("<td>&nbsp;</td></tr>");
                    }
                }
            }
        }

        protected string GetPostNameListHtml()
        {
            StringBuilder DropDownListHtml = new StringBuilder();
            DropDownListHtml.AppendLine("<option value=\"\">请选择岗位</option>");
            foreach (PostInfo info in PostBLL.ReadPostListByPostId(CookiesHelper.ReadCookieValue("UserCompanyPostSetting")))
            {
                if (info.PostId == postID)
                    DropDownListHtml.Append("<option value=\"" + info.PostId + "\" selected>" + info.PostName + "</option>");
                else
                    DropDownListHtml.Append("<option value=\"" + info.PostId + "\">" + info.PostName + "</option>");
            }
            return DropDownListHtml.ToString();
        }
    }
}
