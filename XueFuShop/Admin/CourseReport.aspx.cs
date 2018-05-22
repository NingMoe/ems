using System;
using XueFuShop.Pages;
using XueFu.EntLib;
using System.Collections;
using XueFuShop.Models;
using System.Collections.Generic;
using XueFuShop.BLL;
using System.Text;
using XueFuShop.Common;

namespace XueFuShop.Admin
{
    public partial class CourseReport : AdminBasePage
    {
        protected string postIdCondition = RequestHelper.GetQueryString<string>("PostIdCondition");
        protected string studyPostIdCondition = RequestHelper.GetQueryString<string>("StudyPostIdCondition");
        protected int companyID = RequestHelper.GetQueryString<int>("CompanyID");
        protected DateTime startDate = DateTime.MinValue;
        protected DateTime endDate = DateTime.MinValue;
        protected string companyName = string.Empty;
        protected string searchCourseName = RequestHelper.GetQueryString<string>("CourseName");
        protected string groupID = RequestHelper.GetQueryString<string>("GroupID");
        protected string action = RequestHelper.GetQueryString<string>("Action");
        protected string state = RequestHelper.GetQueryString<string>("State");

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
                else
                {
                    startDate = string.IsNullOrEmpty(RequestHelper.GetQueryString<string>("SearchStartDate")) ? DateTime.MinValue : Convert.ToDateTime(RequestHelper.GetQueryString<string>("SearchStartDate"));
                    endDate = string.IsNullOrEmpty(RequestHelper.GetQueryString<string>("SearchEndDate")) ? DateTime.MinValue : Convert.ToDateTime(RequestHelper.GetQueryString<string>("SearchEndDate"));
                    companyName = CompanyBLL.ReadCompany(companyID).CompanyName;
                    if (startDate != DateTime.MinValue) SearchStartDate.Text = startDate.ToString("d");
                    if (endDate != DateTime.MinValue) SearchEndDate.Text = endDate.ToString("d");
                    if (!string.IsNullOrEmpty(searchCourseName) && companyID > 0)
                    {
                        CourseName.Text = searchCourseName;
                        ReportList.InnerHtml = GetReportList();
                        ShowArea.Style["display"] = "block";
                    }
                }
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            companyID = RequestHelper.GetForm<int>("CompanyID");
            if (companyID > 0)
            {
                string courseName = CourseName.Text.Trim();
                if (string.IsNullOrEmpty(courseName))
                    ScriptHelper.Alert("������γ����ƣ�");
                ResponseHelper.Redirect("CourseReport.aspx?Action=search&CompanyID=" + companyID.ToString() + "&CourseName=" + ServerHelper.UrlEncode(courseName) + "&SearchStartDate=" + SearchStartDate.Text + "&SearchEndDate=" + SearchEndDate.Text + "&StudyPostIdCondition=" + RequestHelper.GetForm<string>("StudyPostIdCondition") + "&PostIdCondition=" + RequestHelper.GetForm<string>("PostIdCondition") + "&GroupID=" + RequestHelper.GetForm<string>("GroupID") + "&State=" + RequestHelper.GetForm<string>("State"));
            }
            else
            {
                ScriptHelper.Alert("��ѡ�������Ĺ�˾���ƣ�");
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
            TempStr.Append("<table class=\"listTable\">\n\r");
            TempStr.Append("<tr class=\"listTableHead\">\n\r");
            TempStr.Append("<td colspan=\"" + (productList.Count + 3) + "\">" + companyName);
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

            TempStr.Append(" <input id=\"Button2\" type=\"button\" value=\"��������\" Class=\"button\" OnClick=\"ExcelGetCatId();\" /> <input id=\"Button1\" style=\"width:100px;\" type=\"button\" value=\"����ѩ��������\" Class=\"button\" OnClick=\"ExcelGetCatId1();\" /> <input id=\"Button1\" style=\"width:100px;\" type=\"button\" value=\"�����౨��\" Class=\"button\" OnClick=\"ExcelGetCatId2();\" /></td>\n\r");
            TempStr.Append("</tr>\n\r");
            TempStr.Append("<tr class=\"listTableHead\">\n\r");
            TempStr.Append("<td  style=\"min-width:60px\">���</td>\n\r");
            TempStr.Append("<td style=\"min-width:60px\">����</td>\n\r");
            TempStr.Append("<td style=\"min-width:60px\">��λ</td>\n\r");
            for (int i = 0; i < productList.Count; i++)
            {
                //ProductInfo product = productList[i];
                //if (product.TestEndTime != DBNull.Value)
                //{
                //    CateIdEndDate[i] = Convert.ToDateTime(product.TestEndTime);
                //}
                //else
                //{
                //    CateIdEndDate[i] = DateTime.Today;
                //}
                TempStr.Append("<td style=\"min-width:100px;\">" + productList[i].Name + "</td>\n\r");
            }
            //TempStr.Append("<td>ƽ����</td>\n\r");
            TempStr.Append("</tr>\n\r");

            UserSearchInfo userSearch = new UserSearchInfo();
            userSearch.InCompanyID = companyID.ToString();
            userSearch.InStatus = state;
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
                    TempStr.Append("<tr class=\"listTableMain\">\n\r");
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

            TempStr.Append("<tr class=\"listTableFoot\">\n\r");
            TempStr.Append("<td colspan=\"3\"><B>Ӧ������</B></td>\n\r");
            for (int i = 0; i < productList.Count; i++)
            {
                TempStr.Append("<td>" + averageInfo[i, 5].ToString() + "</td>\n\r");
            }
            TempStr.Append("</tr>\n\r");

            TempStr.Append("<tr class=\"listTableFoot\">\n\r");
            TempStr.Append("<td colspan=\"3\"><B>ʵ�ʲμӿ�������</B></td>\n\r");
            for (int i = 0; i < productList.Count; i++)
            {
                TempStr.Append("<td>" + (averageInfo[i, 1] != 0 ? averageInfo[i, 1].ToString() : "0") + "</td>\n\r");
            }
            TempStr.Append("</tr>\n\r");

            TempStr.Append("<tr class=\"listTableFoot\">\n\r");
            TempStr.Append("<td colspan=\"3\"><B>������</B></td>\n\r");
            for (int i = 0; i < productList.Count; i++)
            {
                TempStr.Append("<td>" + (averageInfo[i, 5] != 0 ? Math.Round(averageInfo[i, 1] / averageInfo[i, 5], 4).ToString("P") : "0") + "</td>\n\r");
            }
            TempStr.Append("</tr>\n\r");

            TempStr.Append("<tr class=\"listTableFoot\">\n\r");
            TempStr.Append("<td colspan=\"3\"><B>����ƽ����</B></td>\n\r");
            for (int i = 0; i < productList.Count; i++)
            {
                TempStr.Append("<td>" + (averageInfo[i, 1] != 0 ? Math.Round(averageInfo[i, 0] / averageInfo[i, 1], 2) : 0) + "</td>\n\r");
            }
            TempStr.Append("</tr>\n\r");

            TempStr.Append("<tr class=\"listTableFoot\">\n\r");
            TempStr.Append("<td colspan=\"3\"><B>����ͨ������</B></td>\n\r");
            for (int i = 0; i < productList.Count; i++)
            {
                TempStr.Append("<td>" + averageInfo[i, 2] + "</td>\n\r");
            }
            TempStr.Append("</tr>\n\r");

            TempStr.Append("<tr class=\"listTableFoot\">\n\r");
            TempStr.Append("<td colspan=\"3\"><B>�γ�ͨ����</B></td>\n\r");
            for (int i = 0; i < productList.Count; i++)
            {
                TempStr.Append("<td>" + (averageInfo[i, 5] != 0 ? Math.Round(averageInfo[i, 2] / averageInfo[i, 5], 4).ToString("P") : "0") + "</td>\n\r");
            }
            TempStr.Append("</tr>\n\r");
            TempStr.Append("</table>\n\r");

            return TempStr.ToString();
        }

        #endregion ReportList
    }
}
