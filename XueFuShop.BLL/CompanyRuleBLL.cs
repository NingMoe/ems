using System;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.IDAL;
using XueFuShop.Models;

namespace XueFuShop.BLL
{
    public class CompanyRuleBLL
    {
        private static readonly ICompanyRule dal = FactoryHelper.Instance<ICompanyRule>(Global.DataProvider, "CompanyRuleDAL");

        /// <summary>
        /// 添加公司规则信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static int AddCompanyRule(CompanyRuleInfo Model)
        {
            return dal.AddCompanyRule(Model);
        }


        /// <summary>
        /// 更新公司规则信息
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static int UpdateCompanyRule(CompanyRuleInfo Model)
        {
            return dal.UpdateCompanyRule(Model);
        }


        /// <summary>
        /// 删除公司规则信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteCompanyRule(int Id)
        {
            return DeleteCompanyRule(Id.ToString());
        }

        /// <summary>
        /// 删除公司规则信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteCompanyRule(string Id)
        {
            return dal.DeleteCompanyRule(Id);
        }


        /// <summary>
        /// 查询公司规则记录
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="RecordSearch"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<CompanyRuleInfo> CompanyRuleList(CompanyRuleInfo RuleSearch)
        {
            return dal.CompanyRuleList(RuleSearch);
        }

        /// <summary>
        /// 查询公司规则记录 带分页
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="RecordSearch"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<CompanyRuleInfo> CompanyRuleList(int currentPage, int pageSize, CompanyRuleInfo RuleSearch, ref int count)
        {
            return dal.CompanyRuleList(currentPage, pageSize, RuleSearch, ref count);
        }


        /// <summary>
        /// 取得时间段内通一停课周数
        /// </summary>
        /// <param name="PostId"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public static int GetPublicWeekNum(int PostId, DateTime StartDate, DateTime EndDate)
        {
            int WeekNum = 0;//取整数
            int DifferDays = 0;
            CompanyRuleInfo RuleModel = new CompanyRuleInfo();
            RuleModel.CompanyId = 0;
            RuleModel.PostId = PostId.ToString();
            List<CompanyRuleInfo> TempList = CompanyRuleList(RuleModel);
            if (TempList != null)
            {
                foreach (CompanyRuleInfo Item in TempList)
                {
                    if (Item.StartDate >= EndDate || Item.EndDate <= StartDate) continue;
                    if (Item.StartDate >= StartDate && Item.EndDate <= EndDate)
                    {
                        DifferDays = (Item.EndDate - Item.StartDate).Days;
                    }
                    else if (Item.StartDate < StartDate && Item.EndDate > EndDate)
                    {
                        DifferDays = (EndDate - StartDate).Days;
                    }
                    else
                    {
                        if (Item.StartDate >= StartDate && Item.StartDate <= EndDate)
                        {
                            DifferDays = (EndDate - Item.StartDate).Days;
                        }
                        else if (Item.EndDate >= StartDate && Item.EndDate <= EndDate)
                        {
                            DifferDays = (Item.EndDate - StartDate).Days;
                        }
                    }
                    WeekNum += DifferDays / 7;
                    if (DifferDays % 7 > 0)
                    {
                        WeekNum += 1;
                    }

                }
            }
            return WeekNum;
        }

        /// <summary>
        /// 取得时间段内的课程数
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="PostId"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public static int GetCourseNum(int CompanyId, int PostId, DateTime StartDate, DateTime EndDate)
        {
            int PostWeekNum = 0;
            int CourseNum = 0;


            PostWeekNum = (EndDate - StartDate).Days / 7;
            if ((EndDate - StartDate).Days % 7 > 0)
            {
                PostWeekNum += 1;
            }

            CompanyRuleInfo RuleModel = new CompanyRuleInfo();
            RuleModel.CompanyId = CompanyId;
            RuleModel.PostId = PostId.ToString();
            List<CompanyRuleInfo> TempList = CompanyRuleList(RuleModel);
            if (TempList != null && TempList.Count > 0)
            {
                int TotalWeekNum = 0;
                foreach (CompanyRuleInfo Item in TempList)
                {
                    if (Item.StartDate >= EndDate || Item.EndDate <= StartDate) continue;
                    int WeekNum = 0, PublicWeekNum = 0;//取整数
                    int DifferDays = 0;

                    if (Item.StartDate >= StartDate && Item.EndDate <= EndDate)
                    {
                        DifferDays = (Item.EndDate - Item.StartDate).Days;
                        PublicWeekNum = GetPublicWeekNum(PostId, Item.StartDate, Item.EndDate);
                    }
                    else if (Item.StartDate < StartDate && Item.EndDate > EndDate)
                    {
                        DifferDays = (EndDate - StartDate).Days;
                        PublicWeekNum = GetPublicWeekNum(PostId, StartDate, EndDate);
                    }
                    else
                    {
                        if (Item.StartDate >= StartDate && Item.StartDate <= EndDate)
                        {
                            DifferDays = (EndDate - Item.StartDate).Days;
                            PublicWeekNum = GetPublicWeekNum(PostId, Item.StartDate, EndDate);
                        }
                        else if (Item.EndDate >= StartDate && Item.EndDate <= EndDate)
                        {
                            DifferDays = (Item.EndDate - StartDate).Days;
                            PublicWeekNum = GetPublicWeekNum(PostId, StartDate, Item.EndDate);
                        }
                    }

                    WeekNum = DifferDays / 7;
                    if (DifferDays % 7 > 0)
                    {
                        WeekNum += 1;
                    }
                    //公共停课周数
                    //WeekNum = WeekNum + PublicWeekNum;
                    TotalWeekNum += PublicWeekNum;


                    CourseNum = CourseNum + (WeekNum * 2 - ((Item.CourseNum * (WeekNum - PublicWeekNum)) / Item.Frequency));

                }
                TotalWeekNum = GetPublicWeekNum(PostId, StartDate, EndDate) - TotalWeekNum;
                if (TotalWeekNum < 0) TotalWeekNum = 0;
                CourseNum = CourseNum + (TotalWeekNum * 2);
            }
            else
            {
                //周数
                PostWeekNum = PostWeekNum - GetPublicWeekNum(PostId, StartDate, EndDate);
                //默认销售部门两门，其他部门一门
                if (!StringHelper.CompareString("4,5,7,8,64", PostId.ToString()))
                    CourseNum = PostWeekNum;
            }
            return PostWeekNum * 2 - CourseNum;
        }

    }
}
