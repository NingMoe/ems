using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Models;

namespace XueFuShop.Pages
{
    public class MyTestRecord : UserCommonBasePage
    {
        protected string action = RequestHelper.GetQueryString<string>("Action");
        protected List<TestPaperInfo> testPaperList = new List<TestPaperInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();
            TestPaperInfo testPaper = new TestPaperInfo();
            if (action != "All")
            {
                testPaper.TestMinDate = DateTime.Parse(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-1");
            }
            testPaper.UserId = base.UserID;
            base.PageSize = 15;
            testPaperList=TestPaperBLL.ReadList(testPaper, base.CurrentPage, base.PageSize, ref base.Count);
            base.BindPageControl(ref base.CommonPager);

        }
    }
}
