using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace XueFuShop.Pages
{
    public abstract class UserManageBasePage : UserCommonBasePage
    {
        protected bool ExistsSonCompany = false;
        protected List<CompanyInfo> SonCompanyList = new List<CompanyInfo>();
        protected string currentFileName = string.Empty;
        protected bool isBase = false;
        protected bool isEMS = false;
        protected bool isTPR = false;

        protected override void PageLoad()
        {
            base.PageLoad();
            if (base.UserID > 0)
                ExistsSonCompany = IsGroupCompany(int.Parse(CookiesHelper.ReadCookieValue("UserCompanyType")));

            //���ݵ�ǰ�û��жϲ���ҳ�湫˾�б��������
            if (ExistsSonCompany)
            {
                SonCompanyList = CompanyBLL.ReadCompanyListByCompanyId(SonCompanyID);
            }

            //��ֹͨ���޸�CompanyIDֵ���Ƿ���ȡ������˾��Ϣ
            string companyID = Request["CompanyID"];
            if (!string.IsNullOrEmpty(companyID) && companyID != "0" && companyID != int.MinValue.ToString() && !string.IsNullOrEmpty(StringHelper.SubString(companyID, this.SonCompanyID)))
            {
                ScriptHelper.Alert("��˾��Ϣ����");
            }

            isBase = IsCurrentMenu("UserAdd.aspx,ChangePassword.aspx,CompanyAdd.aspx,CompanyList.aspx,UserList.aspx,UserAdd.aspx");
            if (!isBase)
            {
                isEMS = IsCurrentMenu("HRReport.aspx,PostPlanReport.aspx,ZongHeReport.aspx,TestPaperRecord.aspx,CourseReport.aspx,PostPlanRate.aspx,Training.aspx,TrainingAdd.aspx,TrainingCourseAdd.aspx");
                if (!isEMS) isTPR = IsCurrentMenu("KPI.aspx,KPIAdd.aspx,WorkingPost.aspx,WorkingPostAdd.aspx,EvaluateNameAdd.aspx,EvaluateName.aspx,EvaluateAdd.aspx,EvaluateShow.aspx,StaffEvaluateAdd.aspx,EvaluateReport.aspx,StaffEvaluateReport.aspx");
            }
        }

        //�ж��Ƿ�Ϊ��������
        protected bool IsGroupCompany(int groupID)
        {
            if (groupID == (int)CompanyType.Group || groupID == (int)CompanyType.SubGroup)
                return true;
            else
                return false;
        }

        protected bool IsCurrentMenu(string menuNameString)
        {
            if (string.IsNullOrEmpty(currentFileName))
            {
                string[] urlInfo = Request.Url.ToString().Split(new string[] { ".ashx" }, StringSplitOptions.RemoveEmptyEntries);
                currentFileName = urlInfo.GetValue(0).ToString();
                currentFileName = currentFileName.Substring(currentFileName.LastIndexOf('/') + 1) + ".aspx";
            }
            if (StringHelper.CompareSingleString(menuNameString.ToLower(), currentFileName.ToLower()))
                return true;
            else
                return false;
        }

        protected string GetAddUpdate()
        {
            string str = "���";
            if (RequestHelper.GetQueryString<int>("ID") > 0)
            {
                str = "�޸�";
            }
            return str;
        }

        protected string GetCompanyIDString(int companyID)
        {
            string parentCompanyIDString = string.Empty;
            if (companyID == 0) companyID = base.UserCompanyID;
            if (companyID == base.UserCompanyID)
            {
                parentCompanyIDString = StringHelper.SubString(this.ParentCompanyID, "0");
            }
            else
            {
                parentCompanyIDString = StringHelper.SubString(CompanyBLL.ReadParentCompanyId(companyID), "0");
            }
            if (!string.IsNullOrEmpty(parentCompanyIDString))
            {
                return parentCompanyIDString + "," + companyID.ToString();
            }
            return companyID.ToString();
        }

        //��֤�û�ID���Լ��Ĺ���Χ֮��
        protected void CheckUserIDInCompany(int userID)
        {
            if (!StringHelper.CompareSingleString(this.SonCompanyID, UserBLL.ReadUser(UserID).CompanyID.ToString()))
            {
                ScriptHelper.Alert("�û�ID����");
            }
        }
    }
}
