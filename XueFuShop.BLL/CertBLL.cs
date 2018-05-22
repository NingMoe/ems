using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;
using System.IO;
using XueFuShop.Models;

namespace XueFuShop.BLL
{
    public class CertBLL
    {
        private string templatePath = "/zs/Template/1.jpg";

        private string certRootPath = "/zs/";

        public CertBLL(int companyID)
        {
            string companyTemplatePath = "/zs/Template/" + companyID.ToString() + ".jpg";
            if (File.Exists(ServerHelper.MapPath(companyTemplatePath)))
            {
                this.templatePath = companyTemplatePath;
            }

            this.certRootPath += companyID.ToString();
            if (!Directory.Exists(ServerHelper.MapPath(this.certRootPath)))
            {
                Directory.CreateDirectory(ServerHelper.MapPath(this.certRootPath));
            }
        }

        public string GetCertPath(int userID, int postID)
        {
            string certPath = string.Concat(this.certRootPath, "/", userID.ToString(), "_", postID.ToString(), ".jpg");
            if (!File.Exists(ServerHelper.MapPath(certPath)))
            {
                return string.Empty;
            }
            return certPath;
        }

        public void Create(int userID, string userName, int postID, string postName)
        {
            Watermark NewPic = new Watermark();
            NewPic.BackgroundImage = ServerHelper.MapPath(this.templatePath);
            NewPic.Text = userName;
            NewPic.DateText = DateTime.Today.Year + "     " + DateTime.Today.Month + "    " + DateTime.Today.Day;
            NewPic.PostName = postName;
            string OutPutName = string.Empty;
            switch (postID)
            {
                case 7:
                    OutPutName = "基础知识入门";
                    break;
                case 37:
                    OutPutName = "市场营销";
                    break;
                case 64:
                    OutPutName = "销售内训师";
                    break;
                case 42:
                    OutPutName = "基础电学入门";
                    break;
                case 82:
                    OutPutName = "电学暨电机";
                    break;
                case 83:
                    OutPutName = "售后前台接待";
                    break;
                case 84:
                case 85:
                    OutPutName = "服务主管";
                    break;
                case 87:
                    OutPutName = "售后服务业务入门";
                    break;
                case 88:
                    OutPutName = "售后服务管理入门";
                    break;
                case 168:
                    OutPutName = "精品业务";
                    break;
                case 166:
                    OutPutName = "洗车暨美容";
                    break;
                case 89:
                    OutPutName = "客户关系提升";
                    break;
                case 167:
                    OutPutName = "高级财务";
                    break;
                case 158:
                    OutPutName = "4S店业务入门";
                    break;
                case 11:
                    OutPutName = "高级运营管理专家";
                    break;
                default:
                    OutPutName = postName;
                    break;
            }
            NewPic.PostText = "“汽车" + OutPutName + "专业”";
            NewPic.Left = 205;
            NewPic.Top = 210;
            NewPic.ResultImage = ServerHelper.MapPath(string.Concat(this.certRootPath, "/", userID.ToString(), "_", postID.ToString(), ".jpg"));
            NewPic.Create();
        }
    }
}
