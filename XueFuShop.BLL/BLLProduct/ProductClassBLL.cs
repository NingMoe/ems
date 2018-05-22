using System;
using System.Collections.Generic;
using XueFuShop.Models;
using XueFuShop.IDAL;
using XueFuShop.Common;
using System.Web;
using XueFu.EntLib;
using System.Collections;

namespace XueFuShop.BLL
{
    public sealed class ProductClassBLL
    {
        private static readonly string cacheKey = CacheKey.ReadCacheKey("ProductClass");
        private static readonly IProductClass dal = FactoryHelper.Instance<IProductClass>(Global.DataProvider, "ProductClassDAL");

        public static int AddProductClass(ProductClassInfo productClass)
        {
            productClass.ID = dal.AddProductClass(productClass);
            CacheHelper.Remove(cacheKey);
            return productClass.ID;
        }

        public static void DeleteProductClass(int id)
        {
            dal.DeleteProductClass(id);
            CacheHelper.Remove(cacheKey);
        }

        public static void DeleteTaobaoProductClass()
        {
            dal.DeleteTaobaoProductClass();
            CacheHelper.Remove(cacheKey);
        }

        public static void MoveDownProductClass(int id)
        {
            dal.MoveDownProductClass(id);
            CacheHelper.Remove(cacheKey);
        }

        public static void MoveUpProductClass(int id)
        {
            dal.MoveUpProductClass(id);
            CacheHelper.Remove(cacheKey);
        }

        public static string ProductClassNameList(string idList)
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
                    string className = string.Empty;
                    foreach (string str4 in str2.Split(new char[] { '|' }))
                    {
                        if (className == string.Empty)
                        {
                            className = ReadProductClassCache(Convert.ToInt32(str4)).ClassName;
                        }
                        else
                        {
                            className = className + " > " + ReadProductClassCache(Convert.ToInt32(str4)).ClassName;
                        }
                    }
                    if (className != string.Empty)
                    {
                        if (str == string.Empty)
                        {
                            str = className;
                        }
                        else
                        {
                            str = str + "，" + className;
                        }
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// 读取产品level级的分类名称
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static string ReadProductClassName(string idList, int level)
        {
            string str = string.Empty;
            if (idList.Length > 0)
            {
                idList = idList.Replace("||", "|#|");
                foreach (string str2 in idList.Split(new char[] { '#' }))
                {
                    string[] classIDArray = str2.Split(new char[] { '|' });
                    if (str == string.Empty)
                    {
                        str = ReadProductClassCache(Convert.ToInt32(classIDArray[level])).ClassName;
                    }
                    else
                    {
                        str = str + " " + ReadProductClassCache(Convert.ToInt32(classIDArray[level])).ClassName;
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// 从缓存列表中读取产品分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ProductClassInfo ReadProductClassCache(int id)
        {
            ProductClassInfo info = new ProductClassInfo();
            List<ProductClassInfo> list = ReadProductClassCacheList();
            foreach (ProductClassInfo info2 in list)
            {
                if (info2.ID == id)
                {
                    return info2;
                }
            }
            return info;
        }

        /// <summary>
        /// 从指定的分类列表中读取分类
        /// </summary>
        /// <param name="productClassList"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ProductClassInfo ReadProductClassByProductClassList(List<ProductClassInfo> productClassList, int id)
        {
            foreach (ProductClassInfo info in productClassList)
            {
                if (info.ID == id)
                {
                    return info;
                }
            }
            return new ProductClassInfo();
        }
        
        /// <summary>
        /// 通过产品ID读取产品分类字符串
        /// </summary>
        /// <param name="productID">产品ID字符串，以','隔开</param>
        /// <param name="inBrandID"></param>
        /// <returns></returns>
        public static List<string> ReadProductClassListByProductID(string productID, string inBrandID)
        {
            return dal.ReadProductClassListByProductID(productID, inBrandID);
        }

        /// <summary>
        /// 通过产品ID读取产品分类
        /// </summary>
        /// <param name="level">根据第几级来装载Dictionary</param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> ReadProductClassListByProductID(string productID, int level)
        {
            return ReadProductClassListByProductID(productID, "0", level);
       }

        /// <summary>
        /// 通过产品ID读取产品分类
        /// </summary>
        /// <param name="productID">产品ID字符串，以','隔开</param>
        /// <param name="level">根据第几级来装载Dictionary</param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> ReadProductClassListByProductID(string productID, string inBrandID, int level)
        {
            Dictionary<string, Dictionary<string, string>> resultList = new Dictionary<string, Dictionary<string, string>>();
            List<string> classList = ReadProductClassListByProductID(productID, inBrandID);
            foreach (string info in classList)
            {
                string[] classIDArray = info.Replace("||", "|$|").Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in classIDArray)
                {
                    string[] idArray = item.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (idArray.Length > level)
                    {
                        string classID = idArray[level - 1];
                        if (!resultList.ContainsKey(classID))
                        {
                            Dictionary<string, string> classDic = new Dictionary<string, string>();
                            resultList.Add(classID, classDic);
                        }
                        if (!resultList[classID].ContainsKey(idArray[level]))
                        {
                            resultList[classID].Add(idArray[level], ReadProductClassCache(int.Parse(idArray[level])).ClassName);
                        }
                    }
                }
            }
            return resultList;
        }

        public static ProductClassInfo ReadProductClassCacheByTaobaoID(long taobaoID)
        {
            ProductClassInfo info = new ProductClassInfo();
            List<ProductClassInfo> list = ReadProductClassCacheList();
            foreach (ProductClassInfo info2 in list)
            {
                if (info2.TaobaoID == taobaoID)
                {
                    return info2;
                }
            }
            return info;
        }

        public static List<ProductClassInfo> ReadProductClassCacheList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadProductClassAllList());
            }
            return (List<ProductClassInfo>)CacheHelper.Read(cacheKey);
        }

        public static List<ProductClassInfo> ReadProductClassChildList(int fatherID)
        {
            List<ProductClassInfo> list = new List<ProductClassInfo>();
            List<ProductClassInfo> list2 = ReadProductClassCacheList();
            foreach (ProductClassInfo info in list2)
            {
                if (info.FatherID == fatherID)
                {
                    list.Add(info);
                }
            }
            return list;
        }

        private static List<ProductClassInfo> ReadProductClassChildList(int fatherID, int depth)
        {
            List<ProductClassInfo> list = new List<ProductClassInfo>();
            List<ProductClassInfo> list2 = ReadProductClassCacheList();
            foreach (ProductClassInfo info in list2)
            {
                if (info.FatherID == fatherID)
                {
                    ProductClassInfo item = (ProductClassInfo)ServerHelper.CopyClass(info);
                    string str = string.Empty;
                    for (int i = 1; i < depth; i++)
                    {
                        str = str + HttpContext.Current.Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                    }
                    item.ClassName = str + item.ClassName;
                    list.Add(item);
                    list.AddRange(ReadProductClassChildList(item.ID, depth + 1));
                }
            }
            return list;
        }

        public static List<ProductClassInfo> ReadProductClassNamedList()
        {
            List<ProductClassInfo> list = new List<ProductClassInfo>();
            List<ProductClassInfo> list2 = ReadProductClassRootList();
            foreach (ProductClassInfo info in list2)
            {
                list.Add(info);
                list.AddRange(ReadProductClassChildList(info.ID, 2));
            }
            return list;
        }

        public static List<ProductClassInfo> ReadProductClassRootList()
        {
            List<ProductClassInfo> list = new List<ProductClassInfo>();
            List<ProductClassInfo> list2 = ReadProductClassCacheList();
            foreach (ProductClassInfo info in list2)
            {
                if (info.FatherID == 0)
                {
                    list.Add(info);
                }
            }
            return list;
        }

        public static List<UnlimitClassInfo> ReadProductClassUnlimitClass()
        {
            List<ProductClassInfo> list = ReadProductClassCacheList();
            List<UnlimitClassInfo> list2 = new List<UnlimitClassInfo>();
            foreach (ProductClassInfo info in list)
            {
                UnlimitClassInfo item = new UnlimitClassInfo();
                item.ClassID = info.ID;
                item.ClassName = info.ClassName;
                item.FatherID = info.FatherID;
                list2.Add(item);
            }
            return list2;
        }

        public static string SearchProductClassList(int fatherID)
        {
            string str = string.Empty;
            List<ProductClassInfo> list = ReadProductClassCacheList();
            foreach (ProductClassInfo info in list)
            {
                if (info.FatherID == fatherID)
                {
                    if (str == string.Empty)
                    {
                        str = info.ID.ToString() + "," + info.ClassName;
                    }
                    else
                    {
                        string str3 = str;
                        str = str3 + "|" + info.ID.ToString() + "," + info.ClassName;
                    }
                }
            }
            return str;
        }

        public static void UpdateProductClass(ProductClassInfo productClass)
        {
            dal.UpdateProductClass(productClass);
            CacheHelper.Remove(cacheKey);
        }

        public static void UpdateProductFatherID(Dictionary<long, int> fatherIDDic)
        {
            dal.UpdateProductFatherID(fatherIDDic);
            CacheHelper.Remove(cacheKey);
        }



        /// <summary>
        /// 一级分类排序
        /// </summary>
        /// <param name="productClassDic"></param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> productClassSort(Dictionary<string, Dictionary<string, string>> productClassDic)
        {
            List<ProductClassInfo> classList = new List<ProductClassInfo>();
            Dictionary<string, Dictionary<string, string>> sortProductClassDic = new Dictionary<string, Dictionary<string, string>>();
            foreach (string key in productClassDic.Keys)
            {
                classList.Add(ReadProductClassByProductClassList(ReadProductClassRootList(), int.Parse(key)));
            }

            classList.Sort(SortCompare);

            for (int i = 0; i < classList.Count; i++)
            {
                sortProductClassDic.Add(classList[i].ID.ToString(), productClassDic[classList[i].ID.ToString()]);
            }

            return sortProductClassDic;
        }

        /// <summary>
        /// 二级分类排序
        /// </summary>
        /// <param name="productClassDic"></param>
        /// <returns></returns>
        public static Dictionary<string, string> productClassSort(Dictionary<string, string> productClassDic)
        {
            List<ProductClassInfo> classList = new List<ProductClassInfo>();
            Dictionary<string, string> sortProductClassDic = new Dictionary<string, string>();
            foreach (string key in productClassDic.Keys)
            {
                classList.Add(ReadProductClassCache(int.Parse(key)));
            }

            classList.Sort(SortCompare);

            for (int i = 0; i < classList.Count; i++)
            {
                sortProductClassDic.Add(classList[i].ID.ToString(), productClassDic[classList[i].ID.ToString()]);
            }

            return sortProductClassDic;
        }

        #region SortCompare()函数，对List<ProductClassInfo>进行排序时作为参数使用

        /// <summary>
        /// 对List<ProductClassInfo>进行排序时作为参数使用（升序）
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        private static int SortCompare(ProductClassInfo c1, ProductClassInfo c2)
        {
            if (c1.OrderID < c2.OrderID)
            {
                return -1;
            }
            else if (c1.OrderID > c2.OrderID)
            {
                return 1;
            }
            return 0;
        }
        #endregion
    }
}
