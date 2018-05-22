using System;
using System.Collections.Generic;
using XueFuShop.Common;
using XueFu.EntLib;
using XueFuShop.IDAL;
using XueFuShop.Models;
using System.Web;

namespace XueFuShop.BLL
{
    public class CourseCateBLL
    {
        private static readonly string cacheKey = CacheKey.ReadCacheKey("CourseCate");
        private static readonly ICourseCate dal = FactoryHelper.Instance<ICourseCate>(Global.DataProvider, "CourseCateDAL");

        public static int AddCourseCate(CourseCateInfo CourseCate)
        {
            CourseCate.CateId = dal.AddCourseCate(CourseCate);
            CacheHelper.Remove(cacheKey);
            return CourseCate.CateId;
        }

        public static void UpdateCourseCate(CourseCateInfo CourseCate)
        {
            dal.UpdateCourseCate(CourseCate);
            CacheHelper.Remove(cacheKey);
        }

        public static void DeleteCourseCate(int id)
        {
            if (ReadCourseCateChildList(id).Count <= 0)
            {
                dal.DeleteCourseCate(id);
                CourseBLL.DeleteCourseByCateId(id);
                CacheHelper.Remove(cacheKey);
            }
        }


        public static string CourseCateNameList(string idList)
        {
            string str = string.Empty;
            if (idList != string.Empty)
            {
                idList = idList.Substring(1, idList.Length - 2);
            }
            idList = idList.Replace("||", "#");
            if (idList.Length > 0)
            {
                foreach (string str2 in idList.Split(new char[] { '#' }))
                {
                    string CateName = string.Empty;
                    foreach (string str4 in str2.Split(new char[] { '|' }))
                    {
                        if (CateName == string.Empty)
                        {
                            CateName = ReadCourseCateCache(Convert.ToInt32(str4)).CateName;
                        }
                        else
                        {
                            CateName = CateName + " > " + ReadCourseCateCache(Convert.ToInt32(str4)).CateName;
                        }
                    }
                    if (CateName != string.Empty)
                    {
                        if (str == string.Empty)
                        {
                            str = CateName;
                        }
                        else
                        {
                            str = str + "，" + CateName;
                        }
                    }
                }
            }
            return str;
        }


        public static void MoveDownCourseCate(int id)
        {
            dal.MoveDownCourseCate(id);
            CacheHelper.Remove(cacheKey);
        }

        public static void MoveUpCourseCate(int id)
        {
            dal.MoveUpCourseCate(id);
            CacheHelper.Remove(cacheKey);
        }

        public static CourseCateInfo ReadCourseCateCache(int id)
        {
            CourseCateInfo info = new CourseCateInfo();
            List<CourseCateInfo> list = ReadCourseCateCacheList();
            foreach (CourseCateInfo info2 in list)
            {
                if (info2.CateId == id)
                {
                    return info2;
                }
            }
            return info;
        }

        /// <summary>
        /// 读取缓存中的课表类别总表，然后过滤留下系统类别和权限内的课程类别
        /// </summary>
        /// <returns></returns>
        public static List<CourseCateInfo> ReadCourseCateCacheList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadCourseCateAllList());
            }
            return (List<CourseCateInfo>)CacheHelper.Read(cacheKey);
        }

        public static List<CourseCateInfo> ReadCourseCateChildList(int fatherID)
        {
            List<CourseCateInfo> list = new List<CourseCateInfo>();
            List<CourseCateInfo> list2 = ReadCourseCateCacheList();
            foreach (CourseCateInfo info in list2)
            {
                if (info.ParentCateId == fatherID)
                {
                    list.Add(info);
                }
            }
            return list;
        }

        private static List<CourseCateInfo> ReadCourseCateChildList(int fatherID, int depth)
        {
            List<CourseCateInfo> list = new List<CourseCateInfo>();
            List<CourseCateInfo> list2 = ReadCourseCateCacheList();
            foreach (CourseCateInfo info in list2)
            {
                if (info.ParentCateId == fatherID)
                {
                    CourseCateInfo item = (CourseCateInfo)ServerHelper.CopyClass(info);
                    string str = string.Empty;
                    for (int i = 1; i < depth; i++)
                    {
                        str = str + HttpContext.Current.Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├&nbsp;");
                    }
                    item.CateName = str + item.CateName;
                    list.Add(item);
                    list.AddRange(ReadCourseCateChildList(item.CateId, depth + 1));
                }
            }
            return list;
        }

        /// <summary>
        /// 求CateId=Id的所有的父ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static string ReadCourseCateParentCateId(int id)
        {
            string str = string.Empty;
            int fatherID = ReadCourseCateCache(id).ParentCateId;
            if (fatherID > 0)
            {
                str = ReadCourseCateParentCateId(fatherID) + "|" + fatherID;
            }
            return str;
        }

        public static string ReadCourseCateFullParentCateId(int id)
        {
            return (ReadCourseCateParentCateId(id) + "|" + id.ToString() + "|");
        }


        /// <summary>
        /// 求CateId=Id所有的子ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string ReadSonCourseCateCache(int id)
        {
            string TempStr = "0";
            List<CourseCateInfo> list = ReadCourseCateCacheList();
            foreach (CourseCateInfo info2 in list)
            {
                if (info2.ParentCateId == id)
                {
                    TempStr = TempStr + "," + ReadSonCourseCateCache(info2.CateId) + "," + info2.CateId;
                }
            }
            if (TempStr.StartsWith(",")) TempStr = TempStr.Substring(1);
            return TempStr.Replace(",0", "").Replace("0,", "");
        }


        /// <summary>
        /// 返回课程列表及子列表
        /// </summary>
        /// <returns></returns>
        public static List<CourseCateInfo> ReadCourseCateNamedList()
        {
            List<CourseCateInfo> list = new List<CourseCateInfo>();
            List<CourseCateInfo> list2 = ReadCourseCateRootList();
            foreach (CourseCateInfo info in list2)
            {
                list.Add(info);
                list.AddRange(ReadCourseCateChildList(info.CateId, 2));
            }
            return list;
        }

        /// <summary>
        /// 返回课程列表及子列表
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static List<CourseCateInfo> ReadCourseCateNamedList(CourseCateInfo Model)
        {
            List<CourseCateInfo> list = new List<CourseCateInfo>();
            List<CourseCateInfo> list2 = ReadCourseCateNamedList();
            foreach (CourseCateInfo info in list2)
            {
                if (StringHelper.CompareString(Model.Condition, info.CompanyId.ToString()))
                {
                    list.Add(info);
                }
            }
            return list;
        }

        /// <summary>
        /// 返回根列表
        /// </summary>
        /// <returns></returns>
        public static List<CourseCateInfo> ReadCourseCateRootList()
        {
            List<CourseCateInfo> list = new List<CourseCateInfo>();
            List<CourseCateInfo> list2 = ReadCourseCateCacheList();
            foreach (CourseCateInfo info in list2)
            {
                if (info.ParentCateId == 0)
                {
                    list.Add(info);
                }
            }
            return list;
        }

        /// <summary>
        /// 返回指定CateId值的列表
        /// </summary>
        /// <param name="CateId"></param>
        /// <returns></returns>
        public static CourseCateInfo ReadCourseCateInfo(int CateId)
        {
            CourseCateInfo Item = new CourseCateInfo();
            List<CourseCateInfo> list = ReadCourseCateCacheList();
            foreach (CourseCateInfo info in list)
            {
                if (info.CateId == CateId)
                {
                    return info;
                }
            }
            return Item;
        }

        public static List<CourseCateInfo> ReadCourseCateAllList(CourseCateInfo Model)
        {
            return dal.ReadCourseCateAllList(Model);
        }

    }
}
