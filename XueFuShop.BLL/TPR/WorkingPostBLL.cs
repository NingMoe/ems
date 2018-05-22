using System;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.IDAL;
using System.Web;
using System.Text;

namespace XueFuShop.BLL
{
    public sealed class WorkingPostBLL
    {
        private static readonly string cacheKey = CacheKey.ReadCacheKey("WorkingPostList");
        private static readonly IWorkingPost dal = FactoryHelper.Instance<IWorkingPost>(Global.DataProvider, "WorkingPostDAL");

        public static int AddWorkingPost(WorkingPostInfo workingPost)
        {
            CacheHelper.Remove(cacheKey);
            return dal.AddWorkingPost(workingPost);
        }

        public static void UpdateWorkingPost(WorkingPostInfo workingPost)
        {
            dal.UpdateWorkingPost(workingPost);
            CacheHelper.Remove(cacheKey);
        }

        public static void DeleteWorkingPost(string strID)
        {
            dal.DeleteWorkingPost(strID);
            CacheHelper.Remove(cacheKey);
        }

        public static void DeleteWorkingPostByCompanyID(string companyID)
        {
            dal.DeleteWorkingPostByCompanyID(companyID);
            CacheHelper.Remove(cacheKey);
        }

        /// <summary>
        /// 从缓存里读取岗位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static WorkingPostInfo ReadWorkingPost(int id)
        {
            return ReadPostCateCacheList().Find(m => m.ID == id);
        }

        /// <summary>
        /// 从缓存里读取岗位列表
        /// </summary>
        /// <param name="postIDArray"></param>
        /// <returns></returns>
        public static List<WorkingPostInfo> ReadWorkingPost(int[] postIDArray)
        {
            if (postIDArray.Length > 0)
            {
                List<WorkingPostInfo> resultList = new List<WorkingPostInfo>();
                foreach (int postID in postIDArray)
                {
                    WorkingPostInfo workingPost = ReadWorkingPost(postID);
                    if (workingPost != null)
                        resultList.Add(workingPost);
                }
                return resultList;
            }
            return null;
        }

        public static List<WorkingPostInfo> ReadWorkingPostByCompanyID(int companyID)
        {
            return ReadPostCateCacheList().FindAll(m => m.CompanyId == companyID);
        }

        public static List<WorkingPostInfo> ReadWorkingPostByCompanyIDWithGroup(int companyID)
        {
            List<WorkingPostInfo> workingPostList = ReadWorkingPostList(ReadWorkingPostByCompanyID(companyID), 0);
            if (workingPostList.Count <= 0)
            {
                CompanyInfo company = CompanyBLL.ReadCompany(companyID);
                if (!string.IsNullOrEmpty(company.ParentId) && !company.ParentId.Contains(","))
                {
                    string parentIDString = CompanyBLL.ReadParentCompanyId(companyID);
                    if (!string.IsNullOrEmpty(parentIDString))
                    {
                        foreach (string item in parentIDString.Split(','))
                        {
                            workingPostList = ReadWorkingPostList(ReadWorkingPostByCompanyID(int.Parse(item)), 0);
                            if (workingPostList.Count > 0)
                                break;
                        }
                    }
                }
            }
            return workingPostList;
        }

        private static List<WorkingPostInfo> ReadWorkingPostList(List<WorkingPostInfo> workingPostList, int parentID)
        {
            List<WorkingPostInfo> resultList = new List<WorkingPostInfo>();
            foreach (WorkingPostInfo info in workingPostList)
            {
                if (info.ParentId == parentID)
                {
                    resultList.Add(info);
                    resultList.AddRange(ReadWorkingPostList(workingPostList, info.ID));
                }
            }
            return resultList;
        }

        /// <summary>
        /// 生成部门的树形图列表
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static List<WorkingPostInfo> CreateWorkingPostTreeList(int companyID)
        {
            List<WorkingPostInfo> resultList = new List<WorkingPostInfo>();
            if (companyID < 0)
                return resultList;
            foreach (var item in ReadWorkingPostByCompanyIDWithGroup(companyID))
            {
                if (item.IsPost == 0)
                {
                    if (!string.IsNullOrEmpty(item.Path))
                    {
                        int level = item.Path.Split('|').Length;
                        StringBuilder str = new StringBuilder();
                        for (int i = 0; i < level; i++)
                        {
                            str.Append(HttpContext.Current.Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"));
                        }
                        str.Append(HttpContext.Current.Server.HtmlDecode("├&nbsp;"));
                        str.Append(item.PostName);
                        item.PostName = str.ToString();
                    }
                    resultList.Add(item);
                }
            }
            return resultList;
        }


        public static List<WorkingPostInfo> SearchWorkingPostList(WorkingPostSearchInfo workingPostSearch)
        {
            return dal.SearchWorkingPostList(workingPostSearch);
        }

        public static List<WorkingPostInfo> SearchWorkingPostList(WorkingPostSearchInfo workingPostSearch, int currentPage, int pageSize, ref int count)
        {
            return dal.SearchWorkingPostList(workingPostSearch, currentPage, pageSize, ref count);
        }

        /// <summary>
        /// 读取缓存中的岗位总表
        /// </summary>
        /// <returns></returns>
        public static List<WorkingPostInfo> ReadPostCateCacheList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                WorkingPostSearchInfo workingPostSearch = new WorkingPostSearchInfo();
                workingPostSearch.State = 0;
                workingPostSearch.Condition = "[CompanyID] in (select [CompanyID] from [" + ShopMssqlHelper.TablePrefix + "Company] where [State]=0)";
                CacheHelper.Write(cacheKey, dal.SearchWorkingPostList(workingPostSearch));
            }
            return (List<WorkingPostInfo>)CacheHelper.Read(cacheKey);
        }


        public static WorkingPostViewInfo ReadWorkingPostView(int id)
        {
            return dal.ReadWorkingPostView(id);
        }
        public static List<WorkingPostViewInfo> SearchWorkingPostViewList(WorkingPostSearchInfo workingPostSearch)
        {
            return dal.SearchWorkingPostViewList(workingPostSearch);
        }
    }
}
