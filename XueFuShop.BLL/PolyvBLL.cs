using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;

namespace XueFuShop.BLL
{
    public class PolyvBLL
    {
        public static Dictionary<string, string> GetVideoDic(string vid)
        {
            Dictionary<string, string> resultDic = new Dictionary<string, string>();
            string secretKey = "6NrHe1WPPO";
            string ts = TimeHelper.DateTimeToStamp(DateTime.Now).ToString().PadRight(13, '0');  //10位的秒级时间戳，后面加多3个0，最后为13位的数值
            string sign = MD5Helper.MD5(string.Format("{0}{1}{2}", secretKey, vid, ts));
            resultDic.Add("vid", vid);
            resultDic.Add("ts", ts);
            resultDic.Add("sign", sign);
            return resultDic;
        }
    }
}
