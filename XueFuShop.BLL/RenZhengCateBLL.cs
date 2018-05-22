using System;
using System.Collections.Generic;
using XueFuShop.Common;
using XueFu.EntLib;
using XueFuShop.IDAL;
using XueFuShop.Models;

namespace XueFuShop.BLL
{
    public class RenZhengCateBLL
    {
        private static readonly string cacheKey = CacheKey.ReadCacheKey("RenZhengCate");
        private static readonly IRenZhengCate dal = FactoryHelper.Instance<IRenZhengCate>(Global.DataProvider, "RenZhengCateDAL");

        /// <summary>
        /// 读取缓存中的认证总表
        /// </summary>
        /// <returns></returns>
        public static List<RenZhengCateInfo> ReadTestCateCacheList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadRenZhengCateList());
            }
            return (List<RenZhengCateInfo>)CacheHelper.Read(cacheKey);
        }

        public static string ReadTestCateID(RenZhengCateInfo rzCate)
        {
            string cateID = string.Empty;
            List<RenZhengCateInfo> list = ReadTestCateCacheList();
            foreach (RenZhengCateInfo info in list)
            {
                if (StringHelper.CompareSingleString(rzCate.InPostID, info.PostId.ToString()))
                    cateID += "," + info.CateId.ToString();
            }
            if (cateID.StartsWith(",")) cateID = cateID.Substring(1);
            return cateID;
        }

        public static RenZhengCateInfo ReadTestCateByID(int CateId)
        {
            List<RenZhengCateInfo> list = ReadTestCateCacheList();
            foreach (RenZhengCateInfo info in list)
            {
                if (info.CateId == CateId) return info;
            }
            return null;
        }

        /// <summary>
        /// 读取认证岗位
        /// </summary>
        /// <param name="CateId">认证课程ID</param>
        /// <param name="companyPostID">公司的岗位ID</param>
        /// <returns></returns>
        public static RenZhengCateInfo ReadTestCateByID(int CateId, string companyPostID)
        {
            //string companyPost = CompanyBLL.ReadCompany(companyId).Post;
            List<RenZhengCateInfo> list = ReadTestCateCacheList();
            foreach (RenZhengCateInfo info in list)
            {
                if (info.CateId == CateId && StringHelper.CompareSingleString(companyPostID, info.PostId.ToString()))
                    return info;
            }
            return new RenZhengCateInfo();
        }
    }
}
