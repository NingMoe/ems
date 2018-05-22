using System;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.Models;
using XueFuShop.IDAL;

namespace XueFuShop.BLL
{
    public sealed class CourseBLL
    {
        private static readonly ICourse dal = FactoryHelper.Instance<ICourse>(Global.DataProvider, "CourseDAL");


        public static CourseInfo ReadCourse(int Id)
        {
            return dal.ReadCourse(Id);
        }

        public static int AddCourse(CourseInfo Model)
        {
            return dal.AddCourse(Model);
        }

        public static void UpdateCourse(CourseInfo Model)
        {
            dal.UpdateCourse(Model);
        }

        public static void UpdateCourse(string courseID, int parentID)
        {
            if (!string.IsNullOrEmpty(courseID))
            {
                dal.UpdateCourse(courseID, parentID);
            }
        }

        /// <summary>
        /// 删除题库名称及题库
        /// </summary>
        /// <param name="Id"></param>
        public static void DeleteCourse(int Id)
        {
            ProductBLL.UpdateProductAccessoryByCourseId(Id);
            QuestionBLL.DeleteQuestionByCateId(Id);
            dal.DeleteCourse(Id);
        }

        /// <summary>
        /// 根据分类删除题库名称及题库
        /// </summary>
        /// <param name="cateID"></param>
        public static void DeleteCourseByCateId(int cateID)
        {
            //dal.DeleteCourseByCatId(Id);
            List<CourseInfo> courseList = ReadList(cateID);
            foreach (CourseInfo info in courseList)
            {
                DeleteCourse(info.CourseId);
            }
        }

        /// <summary>
        /// 返回指定CateId下的课程数
        /// </summary>
        /// <param name="CateId"></param>
        /// <returns></returns>
        public static int ReadCourseNumByCateID(int cateID)
        {
            CourseInfo Model = new CourseInfo();
            Model.CateId = cateID;
            return ReadList(Model).Count;
        }

        public static List<CourseInfo> ReadList()
        {
            return dal.ReadList();
        }

        public static List<CourseInfo> ReadList(int cateID)
        {
            CourseInfo course = new CourseInfo();
            course.CateId = cateID;
            return ReadList(course);
        }

        public static List<CourseInfo> ReadList(CourseInfo Model)
        {
            return dal.ReadList(Model);
        }
        public static List<CourseInfo> ReadList(int currentPage, int pageSize, ref int count)
        {
            return dal.ReadList(currentPage, pageSize, ref count);
        }
        public static List<CourseInfo> ReadList(CourseInfo Model, int currentPage, int pageSize, ref int count)
        {
            return dal.ReadList(Model, currentPage, pageSize, ref count);
        }
    }
}
