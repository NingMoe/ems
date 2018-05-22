using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface ICourse
    {
        CourseInfo ReadCourse(int Id);
        int AddCourse(CourseInfo Model);
        void UpdateCourse(CourseInfo Model);
        void UpdateCourse(string courseID, int parentID);
        void DeleteCourse(int Id);
        void DeleteCourseByCatId(int Id);
        List<CourseInfo> ReadList();
        List<CourseInfo> ReadList(CourseInfo Model);
        List<CourseInfo> ReadList(int currentPage, int pageSize, ref int count);
        List<CourseInfo> ReadList(CourseInfo Model, int currentPage, int pageSize, ref int count);
    }
}
