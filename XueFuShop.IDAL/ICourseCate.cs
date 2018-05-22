using System;
using System.Collections.Generic;
using XueFuShop.Models;

namespace XueFuShop.IDAL
{
    public interface ICourseCate
    {
        int AddCourseCate(CourseCateInfo CourseCate);
        void DeleteCourseCate(int id);
        void MoveDownCourseCate(int id);
        void MoveUpCourseCate(int id);
        List<CourseCateInfo> ReadCourseCateAllList();
        List<CourseCateInfo> ReadCourseCateAllList(CourseCateInfo Model);
        void UpdateCourseCate(CourseCateInfo CourseCate);
    }
}
