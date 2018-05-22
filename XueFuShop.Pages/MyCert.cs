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
                            OutPutName = post.PostName;
                            break;
                    }
                    NewPic.PostText = "“汽车" + OutPutName + "专业”";
                    NewPic.Left = 205;
                    NewPic.Top = 210;
                    string PicPath = Server.MapPath("/zs/") + @"/" + base.UserCompanyID.ToString();
                    if (!Directory.Exists(PicPath)) Directory.CreateDirectory(PicPath);
                    PicPath += "/" + base.UserID.ToString() + "_" + Info.PostId.ToString() + ".jpg";
                    NewPic.ResultImage = PicPath;
                    if (!File.Exists(PicPath)) NewPic.Create();
                    PicPath = "/zs/" + base.UserCompanyID.ToString() + "/" + base.UserID.ToString() + "_" + Info.PostId.ToString() + ".jpg";
                    HtmlOutPut.Append("<div style=\"float:left; text-align:center; line-height:30px; margin-top:20px;\"><a href=\"" + PicPath + "\"  target=\"_blank\"><img src=\"" + PicPath + "\" style=\"width:350px;\"></a><h3 style=\"font-size:16px;\">" + post.PostName + "证书</h3></div>");
                }
            }
        }
    }
}
