using System;
using System.Text;
using System.Data.SqlClient;

namespace XueFuShop.BLL
{
    public class BLLMXCourse
    {
        private DALMXCourse dal = new DALMXCourse();
        private MXCourse Model = new MXCourse();
        private BllMXCommon bll = new BllMXCommon();

        public SqlDataReader GetInfor(int CourseId)
        {
            Model.CourseId = CourseId;
            return dal.GetInfor(Model);
        }

        public int InsertInfor(Models.Course.MXCourse Model)
        {
            SqlDataReader dr = dal.InsertInfor(Model);
            int Identity = 0;
            if (dr.Read())
            {
                Identity = int.Parse(dr[0].ToString());
            }
            return Identity;        
        }

        public int UpdateInfor(Models.Course.MXCourse Model)
        {
            return dal.UpdateInfor(Model);
        }

        public StringBuilder CourseTable(string TrClass,string TdClass)
        {
            string TrClassName = "";
            string TdClassName = "";
            if (TrClass != "")
            {
                TrClassName = " class=" + TrClass;
            }
            if (TdClass != "")
            {
                TdClassName = " class=" + TdClass;
            }

            StringBuilder Temp = new StringBuilder();
            SqlDataReader dr = dal.ReadInfor();
            Temp.Append("<table>");
            while (dr.Read())
            {
                Temp.Append("<tr"+TrClassName+">");
                Temp.Append("<td" + TdClassName + "><input type=\"checkbox\" name=\"CourseId\" id=\"CourseId\" value=\""+dr["CourseId"]+"\" ></td>");
                Temp.Append("<td"+TdClassName+"><A href=\"#\">" + dr["CourseName"] + "</A></td>");
                Temp.Append("<td" + TdClassName + "><A href=\"#\">" + bll.GetCateName("CateName", int.Parse(dr["CateId"].ToString())) + "</A></td>");
                Temp.Append("<td" + TdClassName + "><A href=\"#\">" +dr["CourseScore"] + "</A></td>");
                Temp.Append("</tr>");
            }
            Temp.Append("</table>");
            return Temp;
        }
    }
}
