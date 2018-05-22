using System;
using System.Data.SqlClient;

namespace XueFuShop.BLL.BLLCourse
{
    public class BLLCourseCate
    {
        private DAL.DALCourse.DALMXCourseCate dal = new DAL.DALCourse.DALMXCourseCate();
        private Models.Course.MXCourseCate Model = new Models.Course.MXCourseCate();

        public SqlDataReader GetInfor(int CateId)
        {
            Model.CateId = CateId;
            return dal.GetInfor(Model);
        }

        public int InsertInfor(Models.Course.MXCourseCate Model)
        {
            return dal.InsertInfor(Model);
        }

        public int UpdateInfor(Models.Course.MXCourseCate Model)
        {
            return dal.UpdateInfor(Model);
        }
    }
}
