using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.IDAL;
using XueFuShop.Models;

namespace XueFuShop.BLL
{
    public sealed class PostBLL
    {
        private static readonly string cacheKey = CacheKey.ReadCacheKey("PostList");
        private static readonly string postCourseCacheKey = CacheKey.ReadCacheKey("PostCourse");
        private static readonly IPost dal = FactoryHelper.Instance<IPost>(Global.DataProvider, "PostDAL");        

        public static PostInfo ReadPost(int Id)
        {
            List<PostInfo> list2 = ReadPostCateCacheList();
            foreach (PostInfo Info in list2)
            {
                if (Info.PostId == Id)
                {
                    return Info;
                }
            }
            return new PostInfo();
        }

        /// <summary>
        /// 根据PostId字符串获取岗位列表
        /// </summary>
        /// <param name="IdStr">PostId字符串，用“,”隔开</param>
        /// <returns></returns>
        public static List<PostInfo> ReadPostListByPostId(string IdStr)
        {
            List<PostInfo> postList = new List<PostInfo>();
            foreach (PostInfo Info in ReadPostCateCacheList())
            {
                if (StringHelper.CompareSingleString(IdStr, Info.PostId.ToString()))
                {
                    postList.Add(Info);
                }
            }
            return postList;
        }

        /// <summary>
        /// 根据PostId字符串获取父级（上级）岗位列表
        /// </summary>
        /// <param name="IdStr">PostId字符串，用“,”隔开</param>
        /// <returns></returns>
        public static List<PostInfo> ReadParentPostListByPostId(string IdStr)
        {
            return ReadPostListByPostId(ReadDepartmentIdStrByPostId(IdStr));
        }

        /// <summary>
        /// 根据PostId字符串获取父级（上级）字符串
        /// </summary>
        /// <param name="IdStr">PostId字符串，用“,”隔开</param>
        /// <returns></returns>
        public static string ReadDepartmentIdStrByPostId(string IdStr)
        {
            ArrayList PostIdArray = new ArrayList();
            foreach (PostInfo Info in ReadPostListByPostId(IdStr))
            {
                if (!PostIdArray.Contains(Info.ParentId.ToString()))
                {
                    PostIdArray.Add(Info.ParentId.ToString());
                }
            }
            return string.Join(",", (string[])PostIdArray.ToArray(typeof(string)));
        }

        //public static PostInfo ReadPost(string PostName, int ParentId)
        //{
        //    List<PostInfo> list2 = ReadPostCateCacheList();
        //    foreach (PostInfo Info in list2)
        //    {
        //        if (Info.PostName == PostName)
        //        {
        //            if (Info.ParentId == ParentId) return Info;
        //        }
        //    }
        //    return null;
        //}

        /// <summary>
        /// 根据岗位名称从postList中读取信息
        /// </summary>
        /// <param name="postList"></param>
        /// <param name="postName"></param>
        /// <returns></returns>
        public static PostInfo ReadPost(List<PostInfo> postList, string postName)
        {
            foreach (PostInfo Info in postList)
            {
                if (Info.PostName == postName)
                {
                    return Info;
                }
            }
            return null;
        }

        public static int AddPost(PostInfo Model)
        {
            CacheHelper.Remove(cacheKey);
            return dal.AddPost(Model);
        }
        public static void UpdatePost(PostInfo Model)
        {
            dal.UpdatePost(Model);
            CacheHelper.Remove(cacheKey);
        }
        public static void DeletePost(int Id)
        {
            dal.DeletePost(Id);
            CacheHelper.Remove(cacheKey);
            CacheHelper.Remove(postCourseCacheKey);
        }

        /// <summary>
        /// 更新岗位课程
        /// </summary>
        /// <param name="PostId"></param>
        /// <param name="PostPlan"></param>
        public static void UpdatePostPlan(int PostId, string PostPlan)
        {
            dal.UpdatePostPlan(PostId, PostPlan);
            CacheHelper.Remove(cacheKey);
            CacheHelper.Remove(postCourseCacheKey);
        }

        /// <summary>
        /// 更新岗位课程(去除课程)
        /// </summary>
        /// <param name="CourseId"></param>
        public static void UpdatePostPlan(int CourseId)
        {
            UpdatePostPlan(CourseId.ToString());
        }

        /// <summary>
        /// 更新岗位课程(去除课程)
        /// </summary>
        /// <param name="CourseId"></param>
        public static void UpdatePostPlan(string CourseId)
        {
            //返回含有指定的课程Id的岗位列表
            List<PostInfo> PostList = ReadPostCateCacheList().FindAll(delegate(PostInfo Info) { return StringHelper.CompareString(Info.PostPlan, CourseId); });
            if (PostList.Count > 0)
            {
                foreach (PostInfo Info in PostList)
                {
                    dal.UpdatePostPlan(Info.PostId, StringHelper.SubString(Info.PostPlan, CourseId));
                }
                CacheHelper.Remove(cacheKey);
                CacheHelper.Remove(postCourseCacheKey);
            }
        }

        /// <summary>
        /// 读取符合品牌范围内的字符数组
        /// </summary>
        /// <param name="PostPlan"></param>
        /// <returns></returns>
        //public static string ReadBrandStr(string PostPlan)
        //{
        //    return ReadBrandStr(PostPlan, BLLCompany.BrandId);
        //}

        /// <summary>
        /// 读取符合品牌范围内的字符数组
        /// </summary>
        /// <param name="PostPlan"></param>
        /// <returns></returns>
        //public static string ReadBrandStr(string PostPlan,string BrandId)
        //{
        //    if (!string.IsNullOrEmpty(PostPlan))
        //    {
        //        string ReturnStr = string.Empty;
        //        string CompanyBrandId=BrandId;
        //        List<TestCateInfo> ALLlist = BLLTestCate.ReadTestCateList(PostPlan);
        //        foreach (TestCateInfo Info in ALLlist)
        //        {
        //            if (CompareStr.comparebrand(Info.BrandId, CompanyBrandId))
        //            {
        //                ReturnStr = ReturnStr + "," + Info.CateId.ToString();
        //            }
        //        }
        //        if (ReturnStr.StartsWith(",")) return ReturnStr.Substring(1);
        //        else return ReturnStr;
        //    }
        //    return PostPlan;
        //}

        /// <summary>
        /// 读取缓存中的岗位总表
        /// </summary>
        /// <returns></returns>
        public static List<PostInfo> ReadPostCateCacheList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadPostCateAllList());
            }
            return (List<PostInfo>)CacheHelper.Read(cacheKey);
        }



        private static List<PostInfo> ReadPostCateChildList(int fatherID, int depth)
        {
            List<PostInfo> list = new List<PostInfo>();
            List<PostInfo> list2 = ReadPostCateCacheList();
            foreach (PostInfo info in list2)
            {
                if (info.ParentId == fatherID)
                {
                    PostInfo item = (PostInfo)ServerHelper.CopyClass(info);
                    string str = string.Empty;
                    for (int i = 1; i < depth; i++)
                    {
                        str = str + HttpContext.Current.Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                    }
                    str = str + HttpContext.Current.Server.HtmlDecode("├&nbsp;");
                    item.PostName = str + item.PostName;
                    list.Add(item);
                    list.AddRange(ReadPostCateChildList(item.PostId, depth + 1));
                }
            }
            return list;
        }

        private static List<PostInfo> ReadPostCateChildList(int fatherID, int depth, int Level)
        {
            List<PostInfo> list = new List<PostInfo>();
            List<PostInfo> list2 = ReadPostCateCacheList();
            Level = Level - 1;
            foreach (PostInfo info in list2)
            {
                if (info.ParentId == fatherID)
                {
                    PostInfo item = (PostInfo)ServerHelper.CopyClass(info);
                    string str = string.Empty;
                    for (int i = 1; i < depth; i++)
                    {
                        str = str + HttpContext.Current.Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                    }
                    str = str + HttpContext.Current.Server.HtmlDecode("├&nbsp;");
                    item.PostName = str + item.PostName;
                    list.Add(item);
                    if (Level > 0)
                    {
                        list.AddRange(ReadPostCateChildList(item.PostId, depth + 1, Level));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 返回根列表
        /// </summary>
        /// <returns></returns>
        public static List<PostInfo> ReadPostCateRootList()
        {
            List<PostInfo> list = new List<PostInfo>();
            List<PostInfo> list2 = ReadPostCateCacheList();
            foreach (PostInfo info in list2)
            {
                if (info.ParentId == 0)
                {
                    list.Add(info);
                }
            }
            return list;
        }

        /// <summary>
        /// 返回根列表
        /// </summary>
        /// <param name="companyID">公司ID串</param>
        /// <returns></returns>
        public static List<PostInfo> ReadPostCateRootList(string companyID)
        {
            List<PostInfo> list = new List<PostInfo>();
            List<PostInfo> list2 = ReadPostCateCacheList();
            foreach (PostInfo info in list2)
            {
                if (info.ParentId == 0 && StringHelper.CompareSingleString(companyID, info.CompanyID.ToString()))
                {
                    list.Add(info);
                }
            }
            return list;
        }

        /// <summary>
        /// 作为岗位的岗位列表
        /// </summary>
        /// <returns></returns>
        public static List<PostInfo> ReadPostList()
        {
            List<PostInfo> list = new List<PostInfo>();
            List<PostInfo> list2 = ReadPostCateCacheList();
            foreach (PostInfo info in list2)
            {
                if (info.IsPost == 1)
                {
                    list.Add(info);
                }
            }
            return list;
        }

        /// <summary>
        /// 返回指定父ID的列表
        /// </summary>
        /// <returns></returns>
        public static List<PostInfo> ReadPostListByParentID(int parentId)
        {
            return ReadPostList(parentId);
        }

        /// <summary>
        /// 返回指定父ID的列表
        /// </summary>
        /// <returns></returns>
        public static List<PostInfo> ReadPostList(int ParentId)
        {
            List<PostInfo> list = new List<PostInfo>();
            List<PostInfo> list2 = ReadPostCateCacheList();
            foreach (PostInfo info in list2)
            {
                if (info.ParentId == ParentId)
                {
                    list.Add(info);
                }
            }
            return list;
        }

        /// <summary>
        /// 返回列表
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static List<PostInfo> ReadPostList(PostInfo Model)
        {
            return dal.ReadPostList(Model);
        }
        /// <summary>
        /// 返回课程列表及子列表
        /// </summary>
        /// <returns></returns>
        public static List<PostInfo> ReadPostCateNamedList()
        {
            List<PostInfo> list = new List<PostInfo>();
            List<PostInfo> list2 = ReadPostCateRootList();
            foreach (PostInfo info in list2)
            {
                list.Add(info);
                list.AddRange(ReadPostCateChildList(info.PostId, 2));
            }
            return list;
        }

        public static List<PostInfo> ReadPostCateNamedList(string companyID)
        {
            List<PostInfo> list = new List<PostInfo>();
            List<PostInfo> list2 = ReadPostCateRootList(companyID);
            foreach (PostInfo info in list2)
            {
                list.Add(info);
                list.AddRange(ReadPostCateChildList(info.PostId, 2));
            }
            return list;
        }

        /// <summary>
        /// 返回level层课程列表
        /// </summary>
        /// <returns></returns>
        public static List<PostInfo> ReadPostCateNamedList(int level)
        {
            List<PostInfo> list = new List<PostInfo>();
            List<PostInfo> list2 = ReadPostCateRootList();
            level = level - 1;
            foreach (PostInfo info in list2)
            {
                list.Add(info);
                list.AddRange(ReadPostCateChildList(info.PostId, 2, level));
            }
            return list;
        }

        /// <summary>
        /// 读取指定岗位下的[Level]层列表
        /// </summary>
        /// <param name="PostId">岗位ID字符串，以","分隔</param>
        /// <param name="level">层数</param>
        /// <returns></returns>
        public static List<PostInfo> ReadPostCateNamedList(string PostId, int level)
        {
            List<PostInfo> list = new List<PostInfo>();
            List<PostInfo> list2 = ReadPostCateRootList();
            level = level - 1;
            foreach (PostInfo info in list2)
            {
                if (StringHelper.CompareString(PostId, info.PostId.ToString()))
                {
                    list.Add(info);
                    list.AddRange(ReadPostCateChildList(info.PostId, 2, level));
                }
            }
            return list;
        }

        /// <summary>
        /// 返回level层课程列表
        /// </summary>
        /// <param name="level">返回的层数 从顶层开始</param>
        /// <param name="PartmentId">部门ID</param>
        /// <param name="Sign">取值范围 值为1时，取部门下的岗位信息，为0时，取除了该部门之外的信息</param>
        /// <returns></returns>
        public static List<PostInfo> ReadPostCateNamedList(int level, int PartmentId, int Sign)
        {
            List<PostInfo> list = new List<PostInfo>();
            List<PostInfo> list2 = ReadPostCateRootList();
            level = level - 1;
            foreach (PostInfo info in list2)
            {
                if ((Sign == 1 && info.PostId == PartmentId) || (Sign != 1 && info.PostId != PartmentId))
                {
                    list.Add(info);
                    list.AddRange(ReadPostCateChildList(info.PostId, 2, level));
                }
            }
            return list;
        }

        /// <summary>
        /// 返回单独的岗位列表
        /// </summary>
        /// <returns></returns>
        public static List<PostInfo> ReadPostName()
        {
            List<PostInfo> list = new List<PostInfo>();
            List<PostInfo> list2 = ReadPostCateRootList();
            foreach (PostInfo info in list2)
            {
                list.AddRange(ReadPostList(info.PostId));
            }
            return list;
        }


        public static void MoveDown(int id)
        {
            dal.MoveDown(id);
            CacheHelper.Remove(cacheKey);
        }

        public static void MoveUp(int id)
        {
            dal.MoveUp(id);
            CacheHelper.Remove(cacheKey);
        }

        /// <summary>
        /// 判断是否通过岗位
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="userID"></param>
        /// <param name="postID"></param>
        /// <returns></returns>
        public static bool IsPassPost(int companyID, int userID, int postID)
        {
            bool isPass = false;
            string postCourseID = ReadPostCourseID(companyID, postID);
            if (!string.IsNullOrEmpty(postCourseID))
            {
                string passCourseID = TestPaperBLL.ReadCourseIDStr(TestPaperBLL.ReadList(userID, postCourseID, 1));
                postCourseID = StringHelper.SubString(postCourseID, passCourseID);
                if (string.IsNullOrEmpty(postCourseID)) isPass = true;
            }
            return isPass;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="Type">等于1时会加个头</param>
        /// <returns></returns>
        //public static string PostPlanList(int ParentId, string BrandId, int Type)
        //{
        //    StringBuilder TempOut = new StringBuilder();
        //    List<PostInfo> TempList = ReadPostList(ParentId);
        //    PostInfo PostModel = ReadPost(ParentId);
        //    if (PostModel != null && Type == 1)
        //    {
        //        TempOut.Append("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
        //        TempOut.Append("<tr>");
        //        TempOut.Append("<td width=\"100\">" + PostModel.PostName + "</td>");
        //        TempOut.Append("<td>");
        //    }
        //    if (TempList != null && TempList.Count > 0)
        //    {
        //        TempOut.Append("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
        //        foreach (PostInfo Info in TempList)
        //        {
        //            TempOut.Append("<tr>");
        //            TempOut.Append("<td width=\"100\">" + Info.PostName + "</td>");
        //            List<PostInfo> TempSubList = ReadPostList(Info.PostId);
        //            if (TempSubList != null && TempSubList.Count > 0)
        //            {
        //                TempOut.Append("<td>" + PostPlanList(Info.PostId, BrandId, 0) + "</td>");
        //            }
        //            else
        //            {
        //                if (Info.PostId == 48 || Info.PostId == 51) TempOut.Append("<td>" + PostPlanContent(BrandId) + "</td>");
        //                else TempOut.Append("<td>" + PostPlanContent(Info.PostId, BrandId) + "</td>");
        //            }
        //            TempOut.Append("</tr>");
        //        }
        //        TempOut.Append("</table>");
        //    }
        //    else
        //    {
        //        TempOut.Append("<td>" + PostPlanContent(ParentId, BrandId) + "</td>");
        //    }
        //    if (PostModel != null && Type == 1)
        //    {
        //        TempOut.Append("</td>");
        //        TempOut.Append("<tr>");
        //        TempOut.Append("</table>");
        //    }
        //    return TempOut.ToString();
        //}

        //public static string PostPlanContent(int PostId, string BrandId)
        //{
        //    StringBuilder TempOut = new StringBuilder();
        //    PostInfo PostModel = ReadPost(PostId);
        //    if (PostModel != null)
        //    {
        //        List<TestCateInfo> TestCateList = BLLTestCate.ReadTestCateList(ReadBrandStr(PostModel.PostPlan, BrandId));
        //        if (TestCateList != null && TestCateList.Count > 0)
        //        {
        //            TempOut.Append("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
        //            foreach (TestCateInfo Info in TestCateList)
        //            {
        //                TempOut.Append("<tr>");
        //                TempOut.Append("<td >" + Info.CateName + "</td>");
        //                TempOut.Append("<td style=\"width:80px;\"><a href=\"javascript:popPageOnly(\'CourseInfor.aspx?CateId=" + Info.CateId.ToString() + "\',818,578,\'课程简介\',\'Test" + Info.CateId.ToString() + "\')\">课程简介</a></td>");
        //                TempOut.Append("<td style=\"width:80px;\">"+ Info.CourseDate.ToString() + "</td>");
        //                TempOut.Append("</tr>");
        //            }
        //            TempOut.Append("</table>");
        //        }
        //    }
        //    return TempOut.ToString();
        //}

        //private static string PostPlanContent(string BrandId)
        //{
        //    StringBuilder TempOut = new StringBuilder();
        //    List<TestCateInfo> TempList = BLLTestCate.ReadTestCateByParentID(6);
        //    TempOut.Append("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
        //    foreach (TestCateInfo Info in TempList)
        //    {
        //        List<TestCateInfo> TempSonList = BLLTestCate.ReadTestCateByParentID(Info.CateId);
        //        foreach (TestCateInfo Item in TempSonList)
        //        {
        //            if (CompareStr.comparebrand(BrandId, Item.BrandId))
        //            {
        //                TempOut.Append("<tr><td style=\"text-align:left; padding-left:5px;\"><a href=\"javascript:GetPostList('" + Item.CateId.ToString() + "');\" onclick=\"replacetext(this);\">" + Item.CateName + " （选考） [点击展开]</a></td></tr>");
        //                List<TestCateInfo> TestCateList = BLLTestCate.ReadTestCateByParentID(Item.CateId);
        //                foreach (TestCateInfo _Info in TestCateList)
        //                {
        //                    TempOut.Append("<tr class=\"Post" + _Info.ParentCateId.ToString() + "\" style=\"display:none;\"><td style=\"text-align:left; padding-left:20px;\">" + _Info.CateName + "</td></tr>");
        //                }
        //            }
        //        }
        //    }
        //    TempOut.Append("</table>");
        //    return TempOut.ToString();
        //}

        /// <summary>
        /// 返回岗位筛选时的岗位名称
        /// </summary>
        /// <returns></returns>
        public static List<PostInfo> PostSearchName()
        {
            List<PostInfo> PostList = ReadPostCateNamedList(2);
            List<PostInfo> NewPostList = new List<PostInfo>();
            foreach (PostInfo Info in PostList)
            {
                List<PostInfo> TempPostList = ReadPostList(Info.PostId);
                if (TempPostList == null || TempPostList.Count <= 0 || Info.ParentId > 0)
                {
                    Info.PostName = Info.PostName.Trim().Replace("├", "").Trim();
                    NewPostList.Add(Info);
                }
            }
            return NewPostList;
        }

        /// <summary>
        /// 过滤包含在公司ID以及父公司ID串下的列表信息
        /// </summary>
        /// <param name="postList"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static List<PostInfo> FilterPostListByCompanyID(List<PostInfo> postList, int companyID)
        {
            string parentCompanyID = CompanyBLL.ReadParentCompanyIDWithSelf(companyID);
            return FilterPostListByCompanyID(postList, parentCompanyID);
        }

        /// <summary>
        /// 过滤包含在公司ID串下的列表信息
        /// </summary>
        /// <param name="postList"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static List<PostInfo> FilterPostListByCompanyID(List<PostInfo> postList, string companyID)
        {
            List<PostInfo> list = new List<PostInfo>();
            foreach (PostInfo info in postList)
            {
                if (StringHelper.CompareSingleString(companyID, info.CompanyID.ToString()))
                    list.Add(info);
            }
            return list;
        }

        /// <summary>
        /// 在postList查找父ID为parentID的岗位列表
        /// </summary>
        /// <param name="postList"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public static List<PostInfo> FilterPostListByParentID(List<PostInfo> postList, int parentID)
        {
            List<PostInfo> resultList = new List<PostInfo>();
            foreach (PostInfo info in postList)
            {
                if (info.ParentId == parentID)
                    resultList.Add(info);
            }
            return resultList;
        }

        /// <summary>
        /// 通过课程ID读取相关的岗位列表
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public static List<PostInfo> FilterPostListByCourseID(int courseID)
        {
            string postCourseID = courseID.ToString(); //string.Empty;
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.RelationProduct = courseID.ToString();
            productSearch.IsSale = 1;
            productSearch.InCompanyID = CompanyBLL.SystemCompanyId.ToString();
            List<ProductInfo> productList = ProductBLL.SearchProductList(productSearch);
            foreach (ProductInfo product in productList)
            {
                postCourseID += "," + product.ID.ToString();
            }
            if (postCourseID.StartsWith(",")) postCourseID = postCourseID.Substring(1);

            PostInfo post = new PostInfo();
            post.PostPlan = postCourseID;
            post.IsPost = 1;
            return ReadPostList(post);
        }

        /// <summary>
        /// 读取岗位课程ID串(不区分品牌)
        /// </summary>
        /// <param name="postID"></param>
        /// <returns></returns>
        public static string ReadPostCourseID(int postID)
        {
            string postCourseID = string.Empty;
            PostInfo post = ReadPost(postID);
            if (post != null && !string.IsNullOrEmpty(post.PostPlan))
            {
                ProductSearchInfo productSearch = new ProductSearchInfo();
                productSearch.InProductID = post.PostPlan;
                productSearch.IsSale = 1;
                //productSearch.InCompanyID = CompanyBLL.SystemCompanyId.ToString();
                List<ProductInfo> productList = ProductBLL.SearchProductList(productSearch);
                foreach (ProductInfo product in productList)
                {
                    if (!string.IsNullOrEmpty(product.RelationProduct))
                        postCourseID += "," + product.RelationProduct;
                    else
                        postCourseID += "," + product.ID.ToString();
                }
                if (postCourseID.StartsWith(",")) postCourseID = postCourseID.Substring(1);
            }
            return postCourseID;
        }

        /// <summary>
        /// 读取品牌内的岗位课程ID串
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="brandID"></param>
        /// <returns></returns>
        public static string ReadPostCourseID(int postID, string brandID)
        {
            string postCourseID = string.Empty;
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.InProductID = ReadPostCourseID(postID);
            productSearch.IsSale = 1;
            productSearch.InBrandID = brandID;
            //productSearch.InCompanyID = CompanyBLL.SystemCompanyId.ToString();

            if (!string.IsNullOrEmpty(productSearch.InProductID))
            {
                List<ProductInfo> productList = ProductBLL.SearchProductList(productSearch);
                foreach (ProductInfo product in productList)
                {
                    postCourseID += "," + product.ID.ToString();
                }
                if (postCourseID.StartsWith(",")) postCourseID = postCourseID.Substring(1);
            }
            return postCourseID;
        }

        public static string ReadPostCourseID(int companyID, int postID)
        {
            string postKey = companyID.ToString() + postID.ToString();
            Dictionary<string, string> postCourseDic = new Dictionary<string, string>();
            if (CacheHelper.Read(postCourseCacheKey) != null)
            {
                postCourseDic = (Dictionary<string, string>)CacheHelper.Read(postCourseCacheKey);
                if (postCourseDic.ContainsKey(postKey))
                {
                    return postCourseDic[postKey];
                }
            }
            string postCourseID = ReadPostCourseID(postID, CompanyBLL.ReadCompany(companyID).BrandId);
            postCourseDic.Add(postKey, postCourseID);
            CacheHelper.Write(postCourseCacheKey, postCourseDic);
            return postCourseID;
        }

        /// <summary>
        /// 读取指定分类的岗位课程ID串
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="postID"></param>
        /// <param name="classID"></param>
        /// <param name="postProductList"></param>
        /// <returns></returns>
        public static string ReadPostCourseID(int companyID, int postID, int classID, ref List<ProductInfo> postProductList)
        {
            string postCourseID = string.Empty;
            ProductSearchInfo productSearch = new ProductSearchInfo();
            productSearch.InProductID = ReadPostCourseID(companyID, postID);
            productSearch.ClassID = "|" + classID.ToString() + "|";
            //productSearch.InCompanyID = CompanyBLL.SystemCompanyId.ToString();
            productSearch.OrderField = "Order by [Sort],[Name],[ID] desc";

            if (!string.IsNullOrEmpty(productSearch.InProductID))
            {
                postProductList = ProductBLL.SearchProductList(productSearch);
                foreach (ProductInfo product in postProductList)
                {
                    postCourseID += "," + product.ID.ToString();
                }
                if (postCourseID.StartsWith(",")) postCourseID = postCourseID.Substring(1);
            }
            return postCourseID;
        }
    }
}
