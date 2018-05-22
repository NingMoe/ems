using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Pages
{
    public class EvaluateProReport : UserCommonBasePage
    {
        protected int userId = RequestHelper.GetQueryString<int>("UserId");
        protected List<KPIEvaluateReportInfo> reportList = new List<KPIEvaluateReportInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();
            base.Title = "���������б�";

            if (userId > 0)
            {
                KPIEvaluateSearchInfo kpiEvaluateSearch = new KPIEvaluateSearchInfo();
                kpiEvaluateSearch.UserId = userId.ToString();

                //������δɾ���ĸ�λ��Χ��
                string workingPostIdStr = string.Empty;
                WorkingPostSearchInfo workingPostSearch = new WorkingPostSearchInfo();
                workingPostSearch.CompanyId = base.UserCompanyID.ToString();
                workingPostSearch.IsPost = 1;
                foreach (WorkingPostInfo info in WorkingPostBLL.SearchWorkingPostList(workingPostSearch))
                {
                    if (string.IsNullOrEmpty(workingPostIdStr))
                        workingPostIdStr = info.ID.ToString();
                    else
                        workingPostIdStr += "," + info.ID.ToString();
                }
                kpiEvaluateSearch.PostId = workingPostIdStr;

                reportList = KPIEvaluateBLL.KPIEvaluateReportList(kpiEvaluateSearch);
                base.BindPageControl(ref base.CommonPager);
            }
        }
    }
}
