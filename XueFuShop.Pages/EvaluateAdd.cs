using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;
using System.Web;

namespace XueFuShop.Pages
{
    public class EvaluateAdd : UserManageBasePage
    {
        protected string Action = HttpContext.Current.Request["Action"];
        protected int userId = RequestHelper.GetQueryString<int>("UserId");
        protected int workPostId = RequestHelper.GetQueryString<int>("PostId");
        protected int evaluateNameId = RequestHelper.GetQueryString<int>("EvaluateNameId");
        protected int companyID = RequestHelper.GetForm<int>("CompanyID");
        protected StringBuilder trHtml = new StringBuilder();
        protected string userName = RequestHelper.GetQueryString<string>("UserName");
        protected string kpiIdStr = string.Empty;

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "���KPIָ��";

            base.CheckUserPower("AddEvaluate", PowerCheckType.Single);

            if (companyID <= 0)
                companyID = base.UserCompanyID;

            string tempKPI = string.Empty;  //��ʱָ��ID
            string fixedKPI = string.Empty; //����ָ��ID

            //��ȡ��λID�󣬼��ظ�λKPI�б�
            if (Action == "step1")
            {
                if (userId == int.MinValue)
                {
                    userName = RequestHelper.GetForm<string>("UserName");
                    UserSearchInfo user = new UserSearchInfo();
                    user.EqualRealName = userName;
                    user.InCompanyID = companyID.ToString();
                    List<UserInfo> userList = UserBLL.SearchUserList(user);
                    if (userList.Count > 0)
                        userId = userList[0].ID;
                    else
                        ScriptHelper.Alert("�Ҳ������û�");
                }
                workPostId = RequestHelper.GetForm<int>("PostId");
                evaluateNameId = RequestHelper.GetForm<int>("EvaluationName");

                //��ȡUserIdֵ

                if (workPostId > 0)
                {
                    KPISearchInfo kpiSearch = new KPISearchInfo();
                    kpiSearch.IdStr = WorkingPostBLL.ReadWorkingPostView(workPostId).KPIContent;
                    if (!string.IsNullOrEmpty(kpiSearch.IdStr))
                    {
                        List<KPIInfo> kpiList = KPIBLL.SearchKPIList(kpiSearch);

                        List<KPIInfo> tempList1 = new List<KPIInfo>();
                        List<KPIInfo> tempList2 = new List<KPIInfo>();
                        List<KPIInfo> tempList3 = new List<KPIInfo>();
                        foreach (KPIInfo info in kpiList)
                        {
                            if (info.Type == KPIType.Fixed)
                            {
                                if (string.IsNullOrEmpty(fixedKPI))
                                    fixedKPI = info.ID.ToString();
                                else
                                    fixedKPI = fixedKPI + "," + info.ID.ToString();
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(tempKPI))
                                    tempKPI = info.ID.ToString();
                                else
                                    tempKPI = tempKPI + "," + info.ID.ToString();
                            }

                            switch (info.ParentId)
                            {
                                case 1:
                                    tempList1.Add(info);
                                    break;
                                case 2:
                                    tempList2.Add(info);
                                    break;
                                case 3:
                                    tempList3.Add(info);
                                    break;
                            }
                        }

                        int i = 1;
                        foreach (KPIInfo info in tempList1)
                        {
                            trHtml.AppendLine("<tr>");
                            if (i == 1)
                                trHtml.AppendLine("	<td rowspan=\"" + tempList1.Count + "\" class=\"indicator_name\">" + KPIBLL.ReadKPI(info.ParentId).Name + "</td>");
                            if (info.Type == KPIType.Fixed)
                                trHtml.AppendLine("	<td  class=\"forever\">" + EnumHelper.ReadEnumChineseName<KPIType>((int)info.Type) + "</td>");
                            else
                                trHtml.AppendLine("	<td>" + EnumHelper.ReadEnumChineseName<KPIType>((int)info.Type) + "</td>");
                            trHtml.AppendLine("	<td class=\"evaluation_content\" data-id=\"" + info.ID + "\">" + i.ToString() + "." + info.Name + "</td>");
                            trHtml.AppendLine("	<td class=\"schedule\"></td>");
                            trHtml.AppendLine("	<td class=\"schedule\"></td>");
                            trHtml.AppendLine("	<td class=\"schedule\"></td>");
                            trHtml.AppendLine("	<td class=\"schedule\"></td>");
                            trHtml.AppendLine("	<td class=\"schedule\"></td>");
                            trHtml.AppendLine("</tr>");
                            i++;
                        }

                        i = 1;
                        foreach (KPIInfo info in tempList2)
                        {
                            trHtml.AppendLine("<tr>");
                            if (i == 1)
                                trHtml.AppendLine("	<td rowspan=\"" + tempList2.Count + "\" class=\"indicator_name\">" + KPIBLL.ReadKPI(info.ParentId).Name + "</td>");
                            if (info.Type == KPIType.Fixed)
                                trHtml.AppendLine("	<td  class=\"forever\">" + EnumHelper.ReadEnumChineseName<KPIType>((int)info.Type) + "</td>");
                            else
                                trHtml.AppendLine("	<td>" + EnumHelper.ReadEnumChineseName<KPIType>((int)info.Type) + "</td>");
                            trHtml.AppendLine("	<td class=\"evaluation_content\" data-id=\"" + info.ID + "\">" + i.ToString() + "." + info.Name + "</td>");
                            trHtml.AppendLine("	<td class=\"schedule\"></td>");
                            trHtml.AppendLine("	<td class=\"schedule\"></td>");
                            trHtml.AppendLine("	<td class=\"schedule\"></td>");
                            trHtml.AppendLine("	<td class=\"schedule\"></td>");
                            trHtml.AppendLine("	<td class=\"schedule\"></td>");
                            trHtml.AppendLine("</tr>");
                            i++;
                        }

                        i = 1;
                        foreach (KPIInfo info in tempList3)
                        {
                            trHtml.AppendLine("<tr>");
                            if (i == 1)
                                trHtml.AppendLine("	<td rowspan=\"" + tempList3.Count + "\" class=\"indicator_name\">" + KPIBLL.ReadKPI(info.ParentId).Name + "</td>");
                            if (info.Type == KPIType.Fixed)
                                trHtml.AppendLine("	<td  class=\"forever\">" + EnumHelper.ReadEnumChineseName<KPIType>((int)info.Type) + "</td>");
                            else
                                trHtml.AppendLine("	<td>" + EnumHelper.ReadEnumChineseName<KPIType>((int)info.Type) + "</td>");
                            trHtml.AppendLine("	<td class=\"evaluation_content\" data-id=\"" + info.ID + "\">" + i.ToString() + "." + info.Name + "</td>");
                            trHtml.AppendLine("	<td class=\"schedule\"></td>");
                            trHtml.AppendLine("	<td class=\"schedule\"></td>");
                            trHtml.AppendLine("	<td class=\"schedule\"></td>");
                            trHtml.AppendLine("	<td class=\"schedule\"></td>");
                            trHtml.AppendLine("	<td class=\"schedule\"></td>");
                            trHtml.AppendLine("</tr>");
                            i++;
                        }
                    }
                }

                if (userId > 0)
                {
                    KPIEvaluateSearchInfo kpiEvaluate = new KPIEvaluateSearchInfo();
                    kpiEvaluate.UserId = userId.ToString();
                    kpiEvaluate.EvaluateNameId = evaluateNameId;
                    kpiEvaluate.PostId = workPostId.ToString();

                    //ȡ����ʱָ���ֵ
                    if (!string.IsNullOrEmpty(tempKPI))
                    {
                        kpiEvaluate.KPIdStr = tempKPI;
                        foreach (KPIEvaluateInfo info in KPIEvaluateBLL.SearchKPIEvaluateList(kpiEvaluate))
                        {
                            if (string.IsNullOrEmpty(kpiIdStr))
                            {
                                kpiIdStr = info.KPIId + ":" + info.Rate;
                            }
                            else
                            {
                                kpiIdStr = kpiIdStr + "," + info.KPIId + ":" + info.Rate;
                            }
                        }
                    }

                    //ȡ������ָ���ֵ
                    if (!string.IsNullOrEmpty(fixedKPI))
                    {
                        kpiEvaluate.KPIdStr = fixedKPI;
                        List<KPIEvaluateInfo> fixedKPIEValuateList = KPIEvaluateBLL.SearchKPIEvaluateList(kpiEvaluate);
                        //�����ʱָ�������ָ��ļ�¼��Ϊ�յĻ�����Ϊ������������ȡ����ָ���¼
                        if (string.IsNullOrEmpty(kpiIdStr) && fixedKPIEValuateList.Count <= 0)
                        {
                            kpiEvaluate.EvaluateNameId = int.MinValue;
                            kpiEvaluate.PostId = string.Empty; //���и�λ��������¼����Ч��������ɾ����λ��������¼��
                            fixedKPIEValuateList = KPIEvaluateBLL.SearchFixedKPIEvaluateList(kpiEvaluate);
                        }
                        foreach (KPIEvaluateInfo info in fixedKPIEValuateList)//BLLKPIEvaluate.SearchFixedKPIEvaluateList(kpiEvaluate)
                        {
                            if (string.IsNullOrEmpty(kpiIdStr))
                            {
                                kpiIdStr = info.KPIId + ":" + info.Rate;
                            }
                            else
                            {
                                kpiIdStr = kpiIdStr + "," + info.KPIId + ":" + info.Rate;
                            }
                        }
                    }

                }
            }
            else if (Action == "step2")
            {
                kpiIdStr = RequestHelper.GetForm<string>("kpiidstr");
                //workPostId = RequestHelper.GetForm<int>("PostId");
                string alertMessage = ShopLanguage.ReadLanguage("AddOK");
                base.CheckUserPower("AddEvaluate", PowerCheckType.Single);
                if (!string.IsNullOrEmpty(kpiIdStr))
                {
                    KPIEvaluateBLL.DeleteKPIEvaluate(userId, workPostId, evaluateNameId);
                    foreach (string item in kpiIdStr.Split(','))
                    {
                        KPIEvaluateInfo kpiEvaluate = new KPIEvaluateInfo();
                        kpiEvaluate.KPIId = int.Parse(item.Split(':')[0]);
                        kpiEvaluate.Scorse = float.Parse(item.Split(':')[1]);
                        kpiEvaluate.UserId = userId;
                        //kpiEvaluate.EvaluateDate = evaluateDate;
                        kpiEvaluate.EvaluateNameId = evaluateNameId;
                        kpiEvaluate.PostId = workPostId;
                        kpiEvaluate.Rate = int.Parse(item.Split(':')[1]);

                        int id = KPIEvaluateBLL.AddKPIEvaluate(kpiEvaluate);
                        AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("Evaluate"), id);
                    }
                }
                ScriptHelper.Alert(alertMessage, "EvaluateAdd.aspx");
            }

        }


        protected string GetDropDownListContent()
        {
            //װ�ظ�λ�����˵�
            WorkingPostSearchInfo workingPostSearch = new WorkingPostSearchInfo();
            workingPostSearch.CompanyId = companyID.ToString();
            workingPostSearch.IsPost = 1;
            StringBuilder DropDownListHtml = new StringBuilder();
            DropDownListHtml.AppendLine("<option value=\"\">��ѡ���λ</option>");
            foreach (WorkingPostViewInfo info in WorkingPostBLL.SearchWorkingPostViewList(workingPostSearch))
            {
                DropDownListHtml.AppendLine("<option value=\"" + info.PostId + "\">" + info.PostName + "</option>");
            }
            return DropDownListHtml.ToString();
        }

    }
}
