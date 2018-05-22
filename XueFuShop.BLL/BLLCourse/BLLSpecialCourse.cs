using System;
using System.Data.SqlClient;

namespace XueFuShop.BLL.BLLCourse
{
    public class BLLSpecialCourse
    {
        private DAL.DALCourse.DALMXSpecialCourse dal = new DAL.DALCourse.DALMXSpecialCourse();
        private Models.Course.MXSpecialCourse Model = new Models.Course.MXSpecialCourse();

        public SqlDataReader GetInfor(int SpecialCourseId)
        {
            Model.SpecialCourseId = SpecialCourseId;
            return dal.GetInfor(Model);
        }

        public int InsertInfor(Models.Course.MXSpecialCourse Model)
        {
            return dal.InsertInfor(Model);
        }

        public int UpdateInfor(Models.Course.MXSpecialCourse Model)
        {
            return dal.UpdateInfor(Model);
        }
    }
}
