using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public class EvaluateShow : UserManageBasePage
    {
        protected int userId = RequestHelper.GetQueryString<int>("UserId");
        protected int workPostId = RequestHelper.GetQueryString<int>("PostId");
        protected int evaluateNameId = RequestHelper.GetQueryString<int>("EvaluateNameId");
        protected StringBuilder trHtml = new StringBuilder();
        protected string kpiIdStr = string.Empty;

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "评估结果";

            string tempKPI = string.Empty;  //临时指标ID
            string fixedKPI = string.Empty; //永久指标ID

            if (workPostId > 0)
            {
                KPISearchInfo kpiSearch = new KPISearchInfo();
                kpiSearch.IdStr = WorkingPostBLL.ReadWorkingPostView(workPostId).KPIContent;
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

            if (userId > 0)
            {
                KPIEvaluateSearchInfo kpiEvaluate = new KPIEvaluateSearchInfo();
                kpiEvaluate.UserId = userId.ToString();
                kpiEvaluate.PostId = workPostId.ToString();
                kpiEvaluate.EvaluateNameId = evaluateNameId;

                //取得永久指标的值
                if (!string.IsNullOrEmpty(fixedKPI))
                {
                    kpiEvaluate.KPIdStr = fixedKPI;
                    foreach (KPIEvaluateInfo info in KPIEvaluateBLL.SearchKPIEvaluateList(kpiEvaluate))//BLLKPIEvaluate.SearchFixedKPIEvaluateList(kpiEvaluate)
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

                //取得临时指标的值
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
            }
        }
    }
}
