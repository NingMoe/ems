using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace XueFuShop.Pages
{
    public class CourseReport : UserManageBasePage
    {
        protected int companyID = RequestHelper.GetQueryString<int>("CompanyID");
        protected DateTime startDate = RequestHelper.GetQueryString<DateTime>("SearchStartDate");
        protected DateTime endDate = RequestHelper.GetQueryString<DateTime>("SearchEndDate");
        protected string postIdCondition = RequestHelper.GetQueryString<string>("PostIdCondition");
        protected string studyPostIdCondition = RequestHelper.GetQueryString<string>("StudyPostIdCondition");
        protected string action = RequestHelper.GetQueryString<string>("Action");
        protected List<PostInfo> postList = new List<PostInfo>();
        protected string reportContentHtml = string.Empty;
        protected List<AdminGroupInfo> userGroupList = new List<AdminGroupInfo>();
        protected string groupID = RequestHelper.GetQueryString<string>("GroupID"); 
        protected string searchCourseName = RequestHelper.GetQueryString<string>("CourseName");
        protected string companyName = string.Empty;

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "��Сѧϰ����ɷ�����";
            base.CheckUserPower("ReadEMSReport", PowerCheckType.Single);

            if (companyID < 0) companyID = base.UserCompanyID;
            CompanyInfo company = CompanyBLL.ReadCompany(companyID);
            companyName = company.CompanyName;
            postList = PostBLL.ReadPostListByPostId(company.Post);
            if (base.ExistsSonCompany)
                userGroupList = AdminGroupBLL.ReadAdminGroupList(companyID, UserBLL.ReadUserGroupIDByCompanyID(base.SonCompanyID));
            else
                userGroupList = AdminGroupBLL.ReadAdminGroupList(companyID, UserBLL.ReadUserGroupIDByCompanyID(companyID.ToString()));

            //����Ĭ�ϸ�λ(ȥ��������λ)
            //if (string.IsNullOrEmpty(PostIdCondition)) PostIdCondition = StringHelper.SubString(company.Post, "197");
            //if (string.IsNullOrEmpty(StudyPostIdCondition)) StudyPostIdCondition = StringHelper.SubString(company.Post, "197");
            //�û�Ȩ����Ĭ�ϸ�������Ա
            if (string.IsNullOrEmpty(groupID))
                groupID = "36";

            if (action == "Search")
            {

                //if (base.CompareUserPower("ManageGroupCompany", PowerCheckType.Single))
                if (!string.IsNullOrEmpty(searchCourseName))
                {
                    reportContentHtml = GetReportList();
                }
            }
        }

        #region ReportList

        private string GetReportList()
        {
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.Key = searchCourseName;
            productSearch.NotInClassID = "3644|5235|5298";
            productSearch.InCompanyID = CompanyBLL.SystemCompanyId.ToString() + "," + companyID.ToString();
            List<ProductInfo> productList = ProductBLL.SearchProductList(productSearch);

            decimal[,] averageInfo = new decimal[productList.Count, 6];
            StringBuilder TempStr = new StringBuilder();
            TempStr.Append("<table class=\"evaluation_sheet\">\n\r");
            TempStr.Append("<tr>\n\r");
            TempStr.Append("<th colspan=\"" + (productList.Count + 3) + "\">" + companyName);
            if (startDate != DateTime.MinValue || endDate != DateTime.MinValue)
            {
                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    TempStr.Append(" [ " + startDate.ToString("d") + "��" + endDate.ToString("d") + " ]");
                }
                else if (startDate == DateTime.MinValue)
                {
                    TempStr.Append(" [ ��ֹ����" + endDate.ToString("d") + " ]");
                }
                if (endDate != DateTime.MinValue)
                    endDate = ShopCommon.SearchEndDate(endDate);
            }

            //TempStr.Append(" <input id=\"Button2\" type=\"button\" value=\"��������\" Class=\"button\" OnClick=\"ExcelGetCatId();\" /></th>\n\r");
            TempStr.Append("</th>\n\r");
            TempStr.Append("</tr>\n\r");
            TempStr.Append("<tr>\n\r");
            TempStr.Append("<th style=\"min-width:60px\">���</th>\n\r");
            TempStr.Append("<th style=\"min-width:60px\">����</th>\n\r");
            TempStr.Append("<th style=\"min-width:60px\">ְλ</th>\n\r");
            for (int i = 0; i < productList.Count; i++)
            {
                TempStr.Append("<th style=\"min-width:100px;\">" + productList[i].Name + "</th>\n\r");
            }
            //TempStr.Append("<td>ƽ����</td>\n\r");
            TempStr.Append("</tr>\n\r");

            UserSearchInfo userSearch = new UserSearchInfo();
            userSearch.InCompanyID = companyID.ToString();
            userSearch.Status = (int)UserState.Normal;
            userSearch.InGroupID = groupID;
            userSearch.InWorkingPostID = postIdCondition;
            userSearch.InStudyPostID = studyPostIdCondition;
            List<UserInfo> userList = UserBLL.SearchUserList(userSearch);
            int userNum = 0;//���
            if (userList.Count > 0)
            {
                //ϵͳ�ȵ�ȡ��˾���ڵ����гɼ�������ѭ�����е��õĶ����ݿ�������ظ���
                List<TestPaperInfo> testReportList = TestPaperBLL.ReadReportList(companyID.ToString(), ProductBLL.ReadProductIdStr(productList), startDate, endDate);
                foreach (UserInfo user in userList)
                {
                    userNum = userNum + 1;
                    TempStr.Append("<tr>\n\r");
                    TempStr.Append("<td width=\"35px\">" + userNum.ToString() + "</td>\n\r");
                    TempStr.Append("<td width=\"100px\">" + user.RealName + "</td>\n\r");
                    TempStr.Append("<td width=\"200px\">" + PostBLL.ReadPost(user.WorkingPostID).PostName + "</td>\n\r");
                    decimal userAverageScore = 0;
                    int courseHasTest = 0;
                    for (int i = 0; i < productList.Count; i++)
                    {
                        ProductInfo product = productList[i];
                        string courseState = "--";
                        //if (user.RegisterDate < CateIdEndDate[i])
                        //{
                        averageInfo[i, 5] = averageInfo[i, 5] + 1;
                        //}
                        //else
                        //{
                        //    courseState = "��������";
                        //}
                        TestPaperInfo testPaperModel = TestPaperBLL.ReadReportInfo(testReportList, user.ID, product.ID);
                        if (testPaperModel != null)
                        {
                            TempStr.Append("<td>" + testPaperModel.Scorse.ToString() + "</td>\n\r");
                            userAverageScore = userAverageScore + testPaperModel.Scorse;
                            courseHasTest = courseHasTest + 1;
                            //��Ŀ�ּܷ���
                            averageInfo[i, 0] = averageInfo[i, 0] + testPaperModel.Scorse;
                            //�гɼ�����������
                            averageInfo[i, 1] = averageInfo[i, 1] + 1;
                            if (testPaperModel.IsPass == 1)
                            {
                                averageInfo[i, 2] = averageInfo[i, 2] + 1;
                            }
                        }
                        else
                        {
                            TempStr.Append("<td>" + courseState + "</td>\n\r");
                        }
                    }
                    //�˾���
                    //if (courseHasTest > 0)
                    //{
                    //    TempStr.Append("<td>" + Math.Round(userAverageScore / courseHasTest, 2) + "</td>\n\r");
                    //}
                    //else
                    //{
                    //    TempStr.Append("<td>--</td>\n\r");
                    //}
                    TempStr.Append("</tr>\n\r");
                }
            }

            TempStr.Append("<tr>\n\r");
            TempStr.Append("<th colspan=\"3\"><B>Ӧ������</B></th>\n\r");
            for (int i = 0; i < productList.Count; i++)
            {
                TempStr.Append("<th>" + averageInfo[i, 5].ToString() + "</th>\n\r");
            }
            TempStr.Append("</tr>\n\r");

            TempStr.Append("<tr>\n\r");
            TempStr.Append("<th colspan=\"3\"><B>ʵ�ʲμӿ�������</B></th>\n\r");
            for (int i = 0; i < productList.Count; i++)
            {
                TempStr.Append("<th>" + (averageInfo[i, 1] != 0 ? averageInfo[i, 1].ToString() : "0") + "</th>\n\r");
            }
            TempStr.Append("</tr>\n\r");

            TempStr.Append("<tr>\n\r");
            TempStr.Append("<th colspan=\"3\"><B>������</B></th>\n\r");
            for (int i = 0; i < productList.Count; i++)
            {
                TempStr.Append("<th>" + (averageInfo[i, 5] != 0 ? Math.Round(averageInfo[i, 1] / averageInfo[i, 5], 4).ToString("P") : "0") + "</th>\n\r");
            }
            TempStr.Append("</tr>\n\r");

            TempStr.Append("<tr>\n\r");
            TempStr.Append("<th colspan=\"3\"><B>����ƽ����</B></th>\n\r");
            for (int i = 0; i < productList.Count; i++)
            {
                TempStr.Append("<th>" + (averageInfo[i, 1] != 0 ? Math.Round(averageInfo[i, 0] / averageInfo[i, 1], 2) : 0) + "</th>\n\r");
            }
            TempStr.Append("</tr>\n\r");

            TempStr.Append("<tr>\n\r");
            TempStr.Append("<th colspan=\"3\"><B>����ͨ������</B></th>\n\r");
            for (int i = 0; i < productList.Count; i++)
            {
                TempStr.Append("<th>" + averageInfo[i, 2] + "</th>\n\r");
            }
            TempStr.Append("</tr>\n\r");

            TempStr.Append("<tr class=\"listTableFoot\">\n\r");
            TempStr.Append("<th colspan=\"3\"><B>�γ�ͨ����</B></th>\n\r");
            for (int i = 0; i < productList.Count; i++)
            {
                TempStr.Append("<th>" + (averageInfo[i, 5] != 0 ? Math.Round(averageInfo[i, 2] / averageInfo[i, 5], 4).ToString("P") : "0") + "</th>\n\r");
            }
            TempStr.Append("</tr>\n\r");

            TempStr.Append("</table>\n\r");

            return TempStr.ToString();
        }

        #endregion ReportList
    }
}
