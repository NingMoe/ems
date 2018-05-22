using System;
using System.Data.SqlClient;

namespace XueFuShop.BLL.BLLCourse
{
    public class BLLMXQuestion
    {
        private DAL.DALCourse.DALMXQuestion dal = new DAL.DALCourse.DALMXQuestion();
        private Models.Course.MXQuestion Model = new Models.Course.MXQuestion();

        public SqlDataReader GetInfor(int QuestionId)
        {
            Model.QuestionId = QuestionId;
            return dal.GetInfor(Model);
        }

        public int InsertInfor(Models.Course.MXQuestion Model)
        {
            return dal.InsertInfor(Model);
        }

        public int UpdateInfor(Models.Course.MXQuestion Model)
        {
            return dal.UpdateInfor(Model);
        }
    }
}
