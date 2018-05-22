using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFu.EntLib;
using System.IO;

namespace XueFuShop.Pages
{
    public class MyCert : UserCommonBasePage
    {
        protected StringBuilder HtmlOutPut = new StringBuilder();

        protected override void PageLoad()
        {
            base.PageLoad();
            ListShow();
        }

        protected void ListShow()
        {
            PostPassInfo postPassModel = new PostPassInfo();
            postPassModel.UserId = base.UserID;
            List<PostPassInfo> postPassList = PostPassBLL.ReadPostPassList(postPassModel);
            if (postPassList.Count > 0)
            {
                foreach (PostPassInfo Info in postPassList)
                {
                    PostInfo post = PostBLL.ReadPost(Info.PostId);
                    Watermark NewPic = new Watermark();
                    if (File.Exists(Server.MapPath("/zs/Template/" + base.UserCompanyID.ToString() + ".jpg")))
                    {
                        NewPic.BackgroundImage = Server.MapPath("/zs/Template/" + base.UserCompanyID.ToString() + ".jpg");
                    }
                    else
                        NewPic.BackgroundImage = Server.MapPath("/zs/Template/1.jpg");
                    NewPic.Text = base.UserRealName;
                    NewPic.DateText = DateTime.Today.Year + "     " + DateTime.Today.Month + "    " + DateTime.Today.Day;
                    NewPic.PostName = post.PostName;
                    string OutPutName = string.Empty;
                    switch (Info.PostId)
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
                            OutPutName = post.PostName;
                            break;
                    }
                    NewPic.PostText = "������" + OutPutName + "רҵ��";
                    NewPic.Left = 205;
                    NewPic.Top = 210;
                    string PicPath = Server.MapPath("/zs/") + @"/" + base.UserCompanyID.ToString();
                    if (!Directory.Exists(PicPath)) Directory.CreateDirectory(PicPath);
                    PicPath += "/" + base.UserID.ToString() + "_" + Info.PostId.ToString() + ".jpg";
                    NewPic.ResultImage = PicPath;
                    if (!File.Exists(PicPath)) NewPic.Create();
                    PicPath = "/zs/" + base.UserCompanyID.ToString() + "/" + base.UserID.ToString() + "_" + Info.PostId.ToString() + ".jpg";
                    HtmlOutPut.Append("<div style=\"float:left; text-align:center; line-height:30px; margin-top:20px;\"><a href=\"" + PicPath + "\"  target=\"_blank\"><img src=\"" + PicPath + "\" style=\"width:350px;\"></a><h3 style=\"font-size:16px;\">" + post.PostName + "֤��</h3></div>");
                }
            }
        }
    }
}
