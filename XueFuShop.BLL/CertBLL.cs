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
                    OutPutName = "����֪ʶ����";
                    break;
                case 37:
                    OutPutName = "�г�Ӫ��";
                    break;
                case 64:
                    OutPutName = "������ѵʦ";
                    break;
                case 42:
                    OutPutName = "������ѧ����";
                    break;
                case 82:
                    OutPutName = "��ѧ�ߵ��";
                    break;
                case 83:
                    OutPutName = "�ۺ�ǰ̨�Ӵ�";
                    break;
                case 84:
                case 85:
                    OutPutName = "��������";
                    break;
                case 87:
                    OutPutName = "�ۺ����ҵ������";
                    break;
                case 88:
                    OutPutName = "�ۺ�����������";
                    break;
                case 168:
                    OutPutName = "��Ʒҵ��";
                    break;
                case 166:
                    OutPutName = "ϴ��������";
                    break;
                case 89:
                    OutPutName = "�ͻ���ϵ����";
                    break;
                case 167:
                    OutPutName = "�߼�����";
                    break;
                case 158:
                    OutPutName = "4S��ҵ������";
                    break;
                case 11:
                    OutPutName = "�߼���Ӫ����ר��";
                    break;
                default:
                    OutPutName = postName;
                    break;
            }
            NewPic.PostText = "������" + OutPutName + "רҵ��";
            NewPic.Left = 205;
            NewPic.Top = 210;
            NewPic.ResultImage = ServerHelper.MapPath(string.Concat(this.certRootPath, "/", userID.ToString(), "_", postID.ToString(), ".jpg"));
            NewPic.Create();
        }
    }
}
