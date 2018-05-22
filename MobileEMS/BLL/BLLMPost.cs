using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;

namespace MobileEMS.BLL
{
    public sealed class BLLMPost
    {

        public static ArrayList ReadPostCourseArrayList(int PostId, int PageSize)
        {
            string CookiesKey = "PostCourseArray_" + PostId.ToString();
            if (string.IsNullOrEmpty(CookiesHelper.ReadCookieValue(CookiesKey)))
            {
                CookiesHelper.AddCookie(CookiesKey, JsonConvert.SerializeObject(CreatePostCoursePage(PostId, PageSize)));
            }
            return (ArrayList)JsonConvert.DeserializeObject(CookiesHelper.ReadCookieValue(CookiesKey), typeof(ArrayList));
        }

        public static ArrayList CreatePostCoursePage(int PostId, int PageSize)
        {
            ArrayList PageArray = new ArrayList();
            //string CourseId = GetPostCourseId(PostId);
            //if (!string.IsNullOrEmpty(CourseId))
            //{
            //    ArrayList CourseIdArrayList = new ArrayList(CourseId.Split(','));
            //    string CoursePage = string.Empty;
            //    int i = 0;
            //    foreach (string Item in CourseIdArrayList)
            //    {
            //        TestCateInfo TestCateModel = BLLTestCate.ReadTestCateCache(int.Parse(Item));
            //        if (TestCateModel.BrandId == "0" || CompareStr.comparebrand(TestCateModel.BrandId, BLLCompany.BrandId))
            //        {
            //            //没有关联考题或没下载地址的不作计算 补丁
            //            //if (!string.IsNullOrEmpty(TestCateModel.CourseContent) && !string.IsNullOrEmpty(TestCateModel.CourseUrl))
            //            {
            //                i++;
            //                if (!string.IsNullOrEmpty(CoursePage)) CoursePage += "," + Item;
            //                else CoursePage = Item;

            //                if (i % PageSize == 0)
            //                {
            //                    PageArray.Add(CoursePage);
            //                    CoursePage = string.Empty;
            //                }
            //            }
            //        }
            //    }
            //    if (i % PageSize != 0) PageArray.Add(CoursePage);
            //}
            return PageArray;
        }

        /// <summary>
        /// 取得岗位课程ID
        /// </summary>
        /// <param name="PostId"></param>
        /// <returns></returns>
        public static string GetPostCourseId(int PostId)
        {
            string CourseId = string.Empty;
            PostInfo Info = PostBLL.ReadPost(PostId);
            if (Info != null)
            {
                List<PostInfo> PostList = PostBLL.ReadPostList(Info.PostId);
                if (PostList != null && PostList.Count > 0)
                {
                    foreach (PostInfo Item in PostList)
                    {
                        if (!string.IsNullOrEmpty(Item.PostPlan))
                        {
                            CourseId += "," + Item.PostPlan;
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Info.PostPlan)) CourseId = Info.PostPlan;
                }

                if (CourseId.StartsWith(",")) CourseId = CourseId.Substring(1);
            }
            return CourseId;
        }


        /// <summary>
        /// 取得岗位课程ID(去除非本品牌和没有考题或下载地址的课程)
        /// </summary>
        /// <param name="PostId"></param>
        /// <returns></returns>
        public static string GetStudyPostCourseId(int PostId)
        {
            string returnCourseId = string.Empty;
            //string CourseId = GetPostCourseId(PostId);
            //if (!string.IsNullOrEmpty(CourseId))
            //{
            //    List<TestCateInfo> testCateList = BLLTestCate.ReadTestCateList(CourseId);
            //    foreach (TestCateInfo info in testCateList)
            //    {
            //        if (info.BrandId == "0" || CompareStr.comparebrand(info.BrandId, BLLCompany.BrandId))
            //        {
            //            //没有关联考题或没下载地址的不作计算 补丁
            //            if (!string.IsNullOrEmpty(info.CourseContent) && !string.IsNullOrEmpty(info.CourseUrl))
            //            {
            //                if (!string.IsNullOrEmpty(returnCourseId)) returnCourseId += "," + info.CateId.ToString();
            //                else returnCourseId = info.CateId.ToString();
            //            }
            //        }
            //    }
            //}
            return returnCourseId;
        }
    }
}
