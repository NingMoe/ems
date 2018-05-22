using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;
using XueFuShop.Pages;

namespace XueFuShop.Admin
{
    public partial class HrReport : AdminBasePage
    {
        protected string postIdCondition = RequestHelper.GetQueryString<string>("PostIdCondition");
        protected int companyID = RequestHelper.GetQueryString<int>("CompanyId");
        protected string action = RequestHelper.GetQueryString<string>("Action");
        protected DateTime endDate = DateTime.MinValue;
        protected string groupID = RequestHelper.GetQueryString<string>("GroupID");
        protected string state = RequestHelper.GetQueryString<string>("State");
        protected string passState = RequestHelper.GetQueryString<string>("PassState");
        protected CompanyInfo CompanyModel = new CompanyInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //����Ĭ��ֵ
                if (string.IsNullOrEmpty(action))
                {
                    groupID = "36";
                    state = ((int)UserState.Normal).ToString();
                    passState = "1,2,3";
                }

                try
                {
                    endDate = Convert.ToDateTime(RequestHelper.GetQueryString<string>("SearchEndDate"));
                    if (endDate > DateTime.Today) endDate = DateTime.Today;
                }
                catch
                {
                    endDate = DateTime.Today;
                }
                SearchEndDate.Text = endDate.ToString("d");
                if (action == "search")
                {
                    CompanyModel = CompanyBLL.ReadCompany(companyID);
                    CompanyName.Value = CompanyModel.CompanyName;
                    HtmlOut();
                }
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            companyID = RequestHelper.GetForm<int>("CompanyID");
            if (!string.IsNullOrEmpty(CompanyName.Value) && companyID > 0)
            {
                ResponseHelper.Redirect("HrReport.aspx?Action=search&CompanyId=" + companyID.ToString() + "&PostIdCondition=" + RequestHelper.GetForm<string>("PostIdCondition") + "&GroupID=" + RequestHelper.GetForm<string>("GroupID") + "&State=" + RequestHelper.GetForm<string>("State") + "&SearchEndDate=" + this.SearchEndDate.Text + "&PassState=" + RequestHelper.GetForm<string>("PassState"));
            }
            else
            {
                ScriptHelper.Alert("��ѡ�������Ĺ�˾���ƣ�");
            }
        }

        protected void HtmlOut()
        {
            StringBuilder TextOut = new StringBuilder();
            //Table��ͷ�ں������
            TextOut.Append(CompanyModel.CompanySimpleName);
            TextOut.Append(" [ ��ֹ����" + endDate.ToString("d") + " ]");
            TextOut.Append("</td></tr>\r\n");
            TextOut.Append("<tr class=\"listTableHead\">\r\n");
            TextOut.Append("<td colspan=\"3\">����/��λ</td>");
            List<PostInfo> DepartmentList = PostBLL.ReadParentPostListByPostId(CompanyModel.Post);
            //��λ����
            ArrayList PostArray = new ArrayList();
            //��Ҫ�ٴ�չ���ĸ�λ
            ArrayList PostExpandArray = new ArrayList();
            foreach (PostInfo Info in DepartmentList)
            {
                List<PostInfo> PostList=PostBLL.ReadPostList(Info.PostId);
                int SonPostCount = PostList.Count;
                if (SonPostCount > 1)
                {
                    PostArray.Add(Info.PostId);
                    PostExpandArray.Add(Info.PostId);
                    TextOut.Append("<td colspan=\"" + SonPostCount.ToString() + "\">" + Info.PostName + "</td>");
                }
                else
                {
                    TextOut.Append("<td rowspan=\"2\"  style=\"width:23px;text-align:center; \"><div style=\"width:15px; margin:0px auto;\">" + Info.PostName + "</div></td>");
                    if (SonPostCount == 1)
                    {
                        foreach (PostInfo Item in PostList)
                        {
                            PostArray.Add(Item.PostId);
                        }
                    }
                    else
                    {
                        PostArray.Add(Info.PostId);
                    }
                }
            }
            DepartmentList = null;

            TextOut.Append("</tr>");
            TextOut.Append("<tr class=\"listTableMain\">");
            TextOut.Append("<td style=\"min-width:80px;\">���</td>");
            TextOut.Append("<td style=\"min-width:60px;\">Ա��<br />����</td>");
            TextOut.Append("<td style=\"min-width:200px;\">��ְ<br />��λ</td>");
            foreach (int Item in PostExpandArray)
            {
                ArrayList TempArray = new ArrayList();
                foreach (PostInfo Info in PostBLL.ReadPostList(Item))
                {
                    TextOut.Append("<td style=\"width:23px;text-align:center; \"><div style=\"width:15px; margin:0px auto;\">" + Info.PostName + "</div></td>");
                    TempArray.Add(Info.PostId);
                }
                int InsertPos = PostArray.IndexOf(Item);
                PostArray.RemoveAt(InsertPos);
                PostArray.InsertRange(InsertPos, TempArray);
                TempArray = null;
            }
            PostExpandArray = null;
            TextOut.Append("</tr>\r\n");

            //ȡ�������������ͷ
            TextOut.Insert(0, "<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr><td align=left colspan=\"" + (3 + PostArray.Count) + "\"><ul style=\"margin:10px; line-height:180%;\">˵����<li>��ɫ������ͨ��</li><li>��ɫ��������ѧϰ�ĸ�λ</li><li>��ɫ�����λ�γ���ͨ�����ۺ���֤����δͨ��</li></ul></td></tr><tr class=\"listTableHead\"><td colspan=\"" + (3 + PostArray.Count) + "\">");

            int PNum = 0;
            //����һ�����飬�������ÿ����ͨ���ĸ�λ��
            string PassPostTotal = "|";
            endDate = endDate.AddDays(1);
            UserSearchInfo user = new UserSearchInfo();
            user.InCompanyID = companyID.ToString();
            user.InStatus = state;
            user.InGroupID = groupID;
            user.InWorkingPostID = postIdCondition;
            foreach (UserInfo Info in UserBLL.SearchReportUserList(user))
            {
                PNum += 1;

                PostPassInfo PostPassModel = new PostPassInfo();
                PostPassModel.UserId = Info.ID;
                PostPassModel.CreateDate = endDate;
                //��ȡͨ���ĸ�λ
                string PassPost = PostPassBLL.PassPostString(PostPassModel);
                //��ȡ��λѧ�굫��֤û��������
                PostPassModel.IsRZ = 0;
                string NoPassPost = PostPassBLL.PassPostString(PostPassModel);

                string WorkingPostName = string.Empty;

                //ȡ�ù�����λ
                if (string.IsNullOrEmpty(Info.PostName))
                {
                    WorkingPostName = PostBLL.ReadPost(Info.WorkingPostID).PostName;
                }
                else
                {
                    WorkingPostName = Info.PostName;
                }

                TextOut.Append("<tr class=\"listTableMain\" data-style=\"data\">\r\n");
                TextOut.Append("<td>" + PNum.ToString() + "</td>");
                TextOut.Append("<td>" + Info.RealName + "</td>");
                TextOut.Append("<td>" + WorkingPostName + "</td>");
                int PassPostNum = 0;
                foreach (int Item in PostArray)
                {
                    if (StringHelper.CompareSingleString(PassPost, Item.ToString()))
                    {
                        PassPostNum += 1;
                        TextOut.Append("<td class=\"style1\">��</td>");
                    }
                    else if (StringHelper.CompareSingleString(NoPassPost, Item.ToString()))
                    {
                        TextOut.Append("<td class=\"style2\">��</td>");
                    }
                    else if (Item.ToString() == Info.StudyPostID.ToString())
                    {
                        TextOut.Append("<td class=\"style3\">��</td>");
                    }
                    else
                    {
                        TextOut.Append("<td>&nbsp;</td>");
                    }
                }
                if (PassPostNum > 0) PassPostTotal += PassPostNum.ToString() + "|";
                TextOut.Append("</tr>\r\n");
            }
            TextOut.Append("</table>");

            //ͳ��ͨ��������λ������
            if (PassPostTotal.Length > 1)
            {
                TextOut.Append("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                for (int i = 1; i < PostArray.Count + 1; i++)
                {
                    if (PassPostTotal.Contains("|" + i + "|"))
                    {
                        TextOut.Append("<tr class=\"listTableMain\"><td>ͨ��" + i.ToString() + "����λ������Ϊ��");
                        int PerNum = 0;
                        foreach (string Item in PassPostTotal.Split('|'))
                        {
                            if (Item == i.ToString())
                            {
                                PerNum += 1;
                            }
                        }
                        TextOut.Append(PerNum.ToString() + "</td></tr>");
                    }
                }
                TextOut.Append("</table>");
            }
            this.ReportList.InnerHtml = TextOut.ToString();
        }
    }
}
