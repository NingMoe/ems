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
        /// ����PostId�ַ�����ȡ��λ�б�
        /// </summary>
        /// <param name="IdStr">PostId�ַ������á�,������</param>
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
        /// ����PostId�ַ�����ȡ�������ϼ�����λ�б�
        /// </summary>
        /// <param name="IdStr">PostId�ַ������á�,������</param>
        /// <returns></returns>
        public static List<PostInfo> ReadParentPostListByPostId(string IdStr)
        {
            return ReadPostListByPostId(ReadDepartmentIdStrByPostId(IdStr));
        }

        /// <summary>
        /// ����PostId�ַ�����ȡ�������ϼ����ַ���
        /// </summary>
        /// <param name="IdStr">PostId�ַ������á�,������</param>
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
        /// ���ݸ�λ���ƴ�postList�ж�ȡ��Ϣ
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
        /// ���¸�λ�γ�
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
        /// ���¸�λ�γ�(ȥ���γ�)
        /// </summary>
        /// <param name="CourseId"></param>
        public static void UpdatePostPlan(int CourseId)
        {
            UpdatePostPlan(CourseId.ToString());
        }

        /// <summary>
        /// ���¸�λ�γ�(ȥ���γ�)
        /// </summary>
        /// <param name="CourseId"></param>
        public static void UpdatePostPlan(string CourseId)
        {
            //���غ���ָ���Ŀγ�Id�ĸ�λ�б�
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
        /// ��ȡ����Ʒ�Ʒ�Χ�ڵ��ַ�����
        /// </summary>
        /// <param name="PostPlan"></param>
        /// <returns></returns>
        //public static string ReadBrandStr(string PostPlan)
        //{
        //    return ReadBrandStr(PostPlan, BLLCompany.BrandId);
        //}

        /// <summary>
        /// ��ȡ����Ʒ�Ʒ�Χ�ڵ��ַ�����
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
        /// ��ȡ�����еĸ�λ�ܱ�
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
                    str = str + HttpContext.Current.Server.HtmlDecode("��&nbsp;");
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
                    str = str + HttpContext.Current.Server.HtmlDecode("��&nbsp;");
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
        /// ���ظ��б�
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
        /// ���ظ��б�
        /// </summary>
        /// <param name="companyID">��˾ID��</param>
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
        /// ��Ϊ��λ�ĸ�λ�б�
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
        /// ����ָ����ID���б�
        /// </summary>
        /// <returns></returns>
        public static List<PostInfo> ReadPostListByParentID(int parentId)
        {
            return ReadPostList(parentId);
        }

        /// <summary>
        /// ����ָ����ID���б�
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
        /// �����б�
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static List<PostInfo> ReadPostList(PostInfo Model)
        {
            return dal.ReadPostList(Model);
        }
        /// <summary>
        /// ���ؿγ��б����б�
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
        /// ����level��γ��б�
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
        /// ��ȡָ����λ�µ�[Level]���б�
        /// </summary>
        /// <param name="PostId">��λID�ַ�������","�ָ�</param>
        /// <param name="level">����</param>
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
        /// ����level��γ��б�
        /// </summary>
        /// <param name="level">���صĲ��� �Ӷ��㿪ʼ</param>
        /// <param name="PartmentId">����ID</param>
        /// <param name="Sign">ȡֵ��Χ ֵΪ1ʱ��ȡ�����µĸ�λ��Ϣ��Ϊ0ʱ��ȡ���˸ò���֮�����Ϣ</param>
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
        /// ���ص����ĸ�λ�б�
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
        /// �ж��Ƿ�ͨ����λ
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
        /// <param name="Type">����1ʱ��Ӹ�ͷ</param>
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
        //                TempOut.Append("<td style=\"width:80px;\"><a href=\"javascript:popPageOnly(\'CourseInfor.aspx?CateId=" + Info.CateId.ToString() + "\',818,578,\'�γ̼��\',\'Test" + Info.CateId.ToString() + "\')\">�γ̼��</a></td>");
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
        //                TempOut.Append("<tr><td style=\"text-align:left; padding-left:5px;\"><a href=\"javascript:GetPostList('" + Item.CateId.ToString() + "');\" onclick=\"replacetext(this);\">" + Item.CateName + " ��ѡ���� [���չ��]</a></td></tr>");
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
        /// ���ظ�λɸѡʱ�ĸ�λ����
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
                    Info.PostName = Info.PostName.Trim().Replace("��", "").Trim();
                    NewPostList.Add(Info);
                }
            }
            return NewPostList;
        }

        /// <summary>
        /// ���˰����ڹ�˾ID�Լ�����˾ID���µ��б���Ϣ
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
        /// ���˰����ڹ�˾ID���µ��б���Ϣ
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
        /// ��postList���Ҹ�IDΪparentID�ĸ�λ�б�
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
        /// ͨ���γ�ID��ȡ��صĸ�λ�б�
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
        /// ��ȡ��λ�γ�ID��(������Ʒ��)
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
        /// ��ȡƷ���ڵĸ�λ�γ�ID��
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
        /// ��ȡָ������ĸ�λ�γ�ID��
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
