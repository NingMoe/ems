using System;
using System.Data.SqlClient;

namespace XueFuShop.BLL.BLLCourse
{
    public class BLLMXBrand
    {
        private DAL.DALCourse.DALMXBrand dal = new DAL.DALCourse.DALMXBrand();
        private Models.Course.MXBrand Model = new Models.Course.MXBrand();

        public SqlDataReader GetInfor(int BrandId)
        {
            Model.BrandId = BrandId;
            return dal.GetInfor(Model);
        }

        public int InsertInfor(Models.Course.MXBrand Model)
        {
            return dal.InsertInfor(Model);
        }

        public int UpdateInfor(Models.Course.MXBrand Model)
        {
            return dal.UpdateInfor(Model);
        }
    }
}
