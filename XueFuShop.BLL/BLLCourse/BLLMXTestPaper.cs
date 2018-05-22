using System;
using System.Data.SqlClient;

namespace XueFuShop.BLL.BLLCourse
{
    public class BLLMXTestPaper
    {
        private DAL.DALCourse.DALMXTestPaper dal = new DAL.DALCourse.DALMXTestPaper();
        private Models.Course.MXTestPaper Model = new Models.Course.MXTestPaper();

        public SqlDataReader GetInfor(int TestPaperId)
        {
            Model.TestPaperId = TestPaperId;
            return dal.GetInfor(Model);
        }

        public int InsertInfor(Models.Course.MXTestPaper Model)
        {
            return dal.InsertInfor(Model);
        }

        public int UpdateInfor(Models.Course.MXTestPaper Model)
        {
            return dal.UpdateInfor(Model);
        }
    }
}
