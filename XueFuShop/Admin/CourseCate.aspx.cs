using System;
using XueFu.EntLib;
using XueFuShop.Pages;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;

namespace XueFuShop.Admin
{
    public partial class CourseCate : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckAdminPower("ReadCourseCate", PowerCheckType.Single);
            string action = RequestHelper.GetQueryString<string>("Action");
            int id = RequestHelper.GetQueryString<int>("ID");
            if ((!string.IsNullOrEmpty(action)) && id > 0)
            {
                if (action == "Down")
                {
                    base.CheckAdminPower("OrderCourseCate", PowerCheckType.Single);
                    CourseCateBLL.MoveDownCourseCate(id);
                    AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("MoveRecord"), ShopLanguage.ReadLanguage("CourseCate"), id);
                }
                else if (action == "Up")
                {
                    base.CheckAdminPower("OrderCourseCate", PowerCheckType.Single);
                    CourseCateBLL.MoveUpCourseCate(id);
                    AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("MoveRecord"), ShopLanguage.ReadLanguage("CourseCate"), id);
                }
                else if (action == "Delete")
                {
                    base.CheckAdminPower("DeleteCourseCate", PowerCheckType.Single);
                    CourseCateBLL.DeleteCourseCate(id);
                    AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("DeleteRecord"), ShopLanguage.ReadLanguage("CourseCate"), id);
                }
            }

            CourseCateInfo CourseCateModel = new CourseCateInfo();
            CourseCateModel.Condition = CompanyBLL.SystemCompanyId.ToString();
            base.BindControl(CourseCateBLL.ReadCourseCateNamedList(CourseCateModel), this.RecordList);
        }
    }
}
