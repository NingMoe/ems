using System;
using System.Data.SqlClient;

namespace XueFuShop.BLL
{
    public class BllMXCommon
    {
        public string GetCateName(string ItemName,int CatId)
        {
            DALMXCourseCate dal = new DALMXCourseCate();
            Models.Course.MXCourseCate model=new XueFuShop.Models.Course.MXCourseCate();
            model.CateId = CatId;
            SqlDataReader dr = dal.GetInfor(model);
            if (dr.Read())
            {
                return dr[ItemName].ToString();
            }
            else
            {
                return null;
            }
        }
        
    }
}
