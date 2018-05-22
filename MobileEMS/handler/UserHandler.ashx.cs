using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using MobileEMS.Models;
using XueFu.EntLib;
using XueFuShop.Models;
using XueFuShop.BLL;
using XueFuShop.Common;

namespace MobileEMS.handler
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UserHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //string strSerializeJSON = JsonConvert.SerializeObject("{ \"IsSuccess\" : false , \"ErrorCode\" : \"1\" , \"ErrorMessage\" : \"\u8BF7\u6C42\u53C2\u6570action\u7F3A\u5931\" , \"Content\" : null }");

            string Action = context.Request["action"];//外部请求
            switch (Action)
            {
                case "GetUserCenter":
                    context.Response.Write(GetResult(GetUserCenter()));
                    break;

                case "PunchCard":
                    context.Response.Write(GetResult("{\"xb\":\"0.1\"}"));
                    break;

                case"CheckMobile":
                    context.Response.Write(CheckMobile());
                    break;

                default:
                    context.Response.Write(GetResult(null));
                    break;
            }
        }
        public string CheckMobile()
        {
            string Result = string.Empty;
            string Mobile = RequestHelper.GetQueryString<string>("mobile");
            UserSearchInfo userSearch = new UserSearchInfo();
            userSearch.Mobile = Mobile;
            userSearch.GroupId = 36;
            try
            {
                Result = "{\"code\":0,";
                if (UserBLL.SearchUserList(userSearch).Count > 0)
                {
                    Result += "\"data\":false}";
                }
                else
                {
                    Result += "\"data\":true}";
                }
            }
            catch
            {
                Result = "{\"code\":1,\"message\":\"异常错误！\"}";
            }
            return Result;
        }

        public string GetUserCenter()
        {
            UserInfo user = UserBLL.ReadUser(Cookies.User.GetUserID(true));
            UserLoginInfo UserLoginModel = new UserLoginInfo();
            if (user != null)
            {
                UserLoginModel.Favicon = "/images/31959858.jpg";
                UserLoginModel.UserID = user.ID;
                UserLoginModel.PunchCardSate = 0;
                UserLoginModel.UserXB = "0";
                UserLoginModel.UserMark = "0";
                UserLoginModel.StudyedLessons = "0";
                UserLoginModel.IsHavePayOrder = false;
                UserLoginModel.OpenClassState = 1;
                UserLoginModel.UserName = user.RealName;
                UserLoginModel.StudyPostName = PostBLL.ReadPost(user.StudyPostID).PostName;
            }

            return JsonConvert.SerializeObject(UserLoginModel);
            
        }

        public string GetResult(string Content)
        {
            UserLoginStateInfo UserLoginStateModel = new UserLoginStateInfo();
            UserLoginStateModel.IsSuccess = true;
            UserLoginStateModel.ErrorCode = null;
            UserLoginStateModel.ErrorMessage = null;
            UserLoginStateModel.Content = Content;

            return JsonConvert.SerializeObject(UserLoginStateModel);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }


}
