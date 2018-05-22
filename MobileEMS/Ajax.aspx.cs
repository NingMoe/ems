using System;
using XueFu.EntLib;
using System.Collections.Generic;
using Newtonsoft.Json;
using XueFuShop.BLL;
using MobileEMS.BLL;

namespace MobileEMS
{
    public partial class Ajax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string Action = RequestHelper.GetQueryString<string>("Action");
            switch (Action)
            {
                case "Login":
                    GetResult<Dictionary<string, string>>(Login());
                    break;

                case "ChangeVideo":
                    Response.Write(JsonConvert.SerializeObject(ChangeVideo()));
                    break;
            }

        }

        private Dictionary<string, string> ChangeVideo()
        {
            string vid = StringHelper.SearchSafe(RequestHelper.GetQueryString<string>("vid"));
            return PolyvBLL.GetVideoDic(vid);
        }

        private Dictionary<string, string> Login()
        {
            string loginName = StringHelper.SearchSafe(RequestHelper.GetForm<string>("UserName"));
            string content = StringHelper.SearchSafe(RequestHelper.GetForm<string>("PassWord"));

            return BLLCommon.Login(loginName, content);
        }

        private void GetResult<T>(T Content)
        {
            ResultInfo ResultModel = new ResultInfo();
            ResultModel.d = JsonConvert.SerializeObject(Content);
            Response.Write(JsonConvert.SerializeObject(ResultModel));
        }
    }
}
