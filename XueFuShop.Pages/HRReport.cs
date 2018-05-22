using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;

namespace XueFuShop.Pages
{
    public class HRReport : UserManageBasePage
    {
        protected int CompanyID = RequestHelper.GetQueryString<int>("CompanyID");
        protected string PostIdCondition = RequestHelper.GetQueryString<string>("PostIdCondition");
        protected string Action = RequestHelper.GetQueryString<string>("Action");
        protected List<PostInfo> PostList = new List<PostInfo>();
        protected DateTime EndDate = RequestHelper.GetQueryString<DateTime>("SearchEndDate");
        protected CompanyInfo Company = new CompanyInfo();
        protected StringBuilder ReportContentHtml = new StringBuilder();

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "HR���ʱ���";
            base.CheckUserPower("ReadEMSReport", PowerCheckType.Single);

            if (EndDate == DateTime.MinValue) EndDate = DateTime.Today;

            if (!base.ExistsSonCompany || Action == "Search" || !base.CompareUserPower("ManageGroupCompany", PowerCheckType.Single))
            {
                if (CompanyID < 0) CompanyID = base.UserCompanyID;
                Company = CompanyBLL.ReadCompany(CompanyID);
                PostList = PostBLL.ReadPostListByPostId(Company.Post);
                HtmlOut();
            }
        }

        protected void HtmlOut()
        {
            
            //Table��ͷ�ں������
            ReportContentHtml.Append(Company.CompanySimpleName);
            ReportContentHtml.Append(" [ ��ֹ����" + EndDate.ToString("d") + " ]");
            ReportContentHtml.AppendLine("</th></tr>");
            ReportContentHtml.AppendLine("</tr>");
            ReportContentHtml.AppendLine("<tr>");
            ReportContentHtml.AppendLine("<th colspan=\"3\">����/��λ</th>");

            //��λ����
            ArrayList postArray = new ArrayList();
            //��Ҫ�ٴ�չ���ĸ�λ
            ArrayList postExpandArray = new ArrayList();
            foreach (PostInfo Info in PostBLL.ReadParentPostListByPostId(Company.Post))
            {
                List<PostInfo> sonPostList = PostBLL.FilterPostListByParentID(PostList, Info.PostId);
                int sonPostCount = sonPostList.Count;
                if (sonPostCount > 1)
                {
                    postArray.Add(Info.PostId);
                    postExpandArray.Add(Info.PostId);
                    ReportContentHtml.AppendLine("<th colspan=\"" + sonPostCount.ToString() + "\">" + Info.PostName + "</th>");
                }
                else
                {
                    ReportContentHtml.AppendLine("<th rowspan=\"2\" class=\"outside\"><div class=\"inside\">" + Info.PostName + "</div></th>");
                    if (sonPostCount == 1)
                    {
                        foreach (PostInfo Item in sonPostList)
                        {
                            postArray.Add(Item.PostId);
                        }
                    }
                    else
                    {
                        postArray.Add(Info.PostId);
                    }
                }
            }

            ReportContentHtml.AppendLine("</tr>");
            ReportContentHtml.AppendLine("<tr class=\"hoverwhite\">");
            ReportContentHtml.AppendLine("<td class=\"id\">���</td>");
            ReportContentHtml.AppendLine("<td class=\"name\">Ա������</td>");
            ReportContentHtml.AppendLine("<td class=\"post\">��ְ��λ</td>");
            foreach (int item in postExpandArray)
            {
                ArrayList tempArray = new ArrayList();
                foreach (PostInfo info in PostBLL.FilterPostListByParentID(PostList, item))
                {
                    ReportContentHtml.AppendLine("<td class=\"outside\"><div class=\"inside\">" + info.PostName + "</div></td>");
                    tempArray.Add(info.PostId);
                }
                int insertPos = postArray.IndexOf(item);
                postArray.RemoveAt(insertPos);
                postArray.InsertRange(insertPos, tempArray);
            }
            ReportContentHtml.AppendLine("</tr>");

            //ȡ�������������ͷ
            ReportContentHtml.Insert(0, "<table class=\"evaluation_sheet\"><tr><th colspan=\"" + (3 + postArray.Count) + "\">");

            int PNum = 0;
            //����һ�����飬�������ÿ����ͨ���ĸ�λ��
            string PassPostTotal = "|";

            UserSearchInfo userSearch = new UserSearchInfo();
            userSearch.InCompanyID = CompanyID.ToString();
            userSearch.InStatus = (int)UserState.Normal + "," + (int)UserState.Free;
            userSearch.InStudyPostID = PostIdCondition;
            List<UserInfo> userList = UserBLL.SearchReportUserList(userSearch);
            foreach (UserInfo user in userList)
            {
                PNum += 1;

                PostPassInfo postPassSearch = new PostPassInfo();
                postPassSearch.UserId = user.ID;
                postPassSearch.IsRZ = 1;
                postPassSearch.CreateDate = ShopCommon.SearchEndDate(EndDate);
                //��ȡͨ���ĸ�λ
                string PassPost = PostPassBLL.PassPostString(postPassSearch);
                //��ȡ��λѧ�굫��֤û��������
                postPassSearch.IsRZ = 0;
                string NoPassPost = PostPassBLL.PassPostString(postPassSearch);

                string WorkPostName = string.Empty;

                //ȡ�ù�����λ
                if (string.IsNullOrEmpty(user.PostName))
                {
                    WorkPostName = PostBLL.ReadPost(user.WorkingPostID).PostName;
                }
                else
                {
                    WorkPostName = user.PostName;
                }

                ReportContentHtml.AppendLine("<tr data-style=\"data\">");
                ReportContentHtml.AppendLine("<td>" + PNum.ToString() + "</td>");
                ReportContentHtml.AppendLine("<td>" + user.RealName + "</td>");
                ReportContentHtml.AppendLine("<td>" + WorkPostName + "</td>");
                int PassPostNum = 0;
                foreach (int Item in postArray)
                {
                    if (StringHelper.CompareSingleString(PassPost, Item.ToString()))
                    {
                        PassPostNum += 1;
                        ReportContentHtml.AppendLine("<td class=\"style1\">��</td>");
                    }
                    else if (StringHelper.CompareSingleString(NoPassPost, Item.ToString()))
                    {
                        ReportContentHtml.AppendLine("<td class=\"style2\">��</td>");
                    }
                    else if (Item.ToString() == user.StudyPostID.ToString())
                    {
                        ReportContentHtml.AppendLine("<td class=\"style3\">��</td>");
                    }
                    else
                    {
                        ReportContentHtml.AppendLine("<td>&nbsp;</td>");
                    }
                }
                if (PassPostNum > 0) PassPostTotal += PassPostNum.ToString() + "|";
                ReportContentHtml.AppendLine("</tr>");
            }
            ReportContentHtml.AppendLine("</table>");

            //ͳ��ͨ��������λ������
            if (PassPostTotal.Length > 1)
            {
                ReportContentHtml.AppendLine("<table class=\"evaluation_sheet count\">");
                for (int i = 1; i < postArray.Count + 1; i++)
                {
                    if (PassPostTotal.Contains("|" + i + "|"))
                    {
                        ReportContentHtml.AppendLine("<tr>");
                        ReportContentHtml.AppendLine("<td>ͨ��" + i.ToString() + "����λ������Ϊ��");
                        int PerNum = 0;
                        foreach (string Item in PassPostTotal.Split('|'))
                        {
                            if (Item == i.ToString())
                            {
                                PerNum += 1;
                            }
                        }
                        ReportContentHtml.AppendLine(PerNum.ToString() + "</td>");
                        ReportContentHtml.AppendLine("</tr>");
                    }
                }
                ReportContentHtml.AppendLine("</table>");
            }
        }
    }
}
