using System;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.IDAL;
using XueFuShop.Common;
using XueFuShop.Models;

namespace XueFuShop.BLL
{
    public sealed class CompanyBLL
    {
        private static readonly ICompany dal = FactoryHelper.Instance<ICompany>(Global.DataProvider, "CompanyDAL");

        private static Dictionary<int, CompanyInfo> companyDic = new Dictionary<int, CompanyInfo>();
        /// <summary>
        /// 系统ID
        /// </summary>
        public static int SystemCompanyId
        {
            get { return 0; }
        }

        public static int AddCompany(CompanyInfo Model)
        {
            companyDic[Model.CompanyId] = Model;
            return dal.AddCompany(Model);
        }

        /// <summary>
        /// 读取公司信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static CompanyInfo ReadCompany(int Id)//根据实际情况，升级为把使用到的公司信息缓存起来，减少数据库的访问次数
        {
            if (!companyDic.ContainsKey(Id))
            {
                companyDic[Id] = dal.ReadCompany(Id);
            }
            return companyDic[Id];
        }

        public static void UpdateCompany(CompanyInfo Model)
        {
            dal.UpdateCompany(Model);
            companyDic[Model.CompanyId] = Model;
        }

        public static void UpdateCompany(string Field, string Value, string Id)
        {
            dal.UpdateCompany(Field, Value, Id);
            companyDic.Clear();
        }


        /// <summary>
        /// 根据公司ID删除公司（自己不能删除自己）
        /// </summary>
        /// <param name="Id">公司ID号</param>
        public static void DeleteCompany(int Id)
        {
            CompanyInfo Model = new CompanyInfo();
            Model.ParentIdCondition = Id.ToString();
            //Model.CompanyId = CompanyId;
            if (ReadCompanyList(Model).Count > 0)
            {
                ScriptHelper.Alert("旗下有子公司，请先删除子公司后再进行删除！");
            }
            else
            {
                UserBLL.DeleteUserByCompanyID(Id);
                dal.DeleteCompany(Id);
                companyDic.Remove(Id);
            }
        }
        /// <summary>
        /// 读取旗下的公司ID(包含本公司ID)
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public static string ReadCompanyIdList(string CompanyId)
        {
            string TempStr = string.Empty, Result = string.Empty;
            TempStr = CompanyId;
            List<CompanyInfo> CompanyList = ReadCompanyListByParentID(CompanyId);
            if (CompanyList.Count > 0)
            {
                foreach (CompanyInfo Item in CompanyList)
                {
                    if (!StringHelper.CompareSingleString(TempStr, Item.CompanyId.ToString()))
                    {
                        if (Item.GroupId == 1 || Item.GroupId == 2)
                        {
                            TempStr = TempStr + "," + ReadCompanyIdList(Item.CompanyId.ToString());
                        }
                        else
                        {
                            TempStr = TempStr + "," + Item.CompanyId;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(TempStr))
            {
                foreach (string Info in TempStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!StringHelper.CompareSingleString(Result, Info))
                    {
                        Result = Result + "," + Info;
                    }
                }
            }
            if (!string.IsNullOrEmpty(Result) && Result.StartsWith(",")) return Result.Substring(1);
            return Result;
        }

        /// <summary>
        /// 取得公司及旗下公司列表
        /// </summary>
        /// <param name="CompanyId">公司ID字符串</param>
        /// <returns></returns>
        public static List<CompanyInfo> ReadCompanyList(string CompanyId)
        {
            return ReadCompanyListByCompanyId(ReadCompanyIdList(CompanyId));
        }

        /// <summary>
        /// 读取公司列表
        /// </summary>
        /// <param name="CompanyId">公司ID字符串</param>
        /// <returns></returns>
        public static List<CompanyInfo> ReadCompanyListByCompanyId(string companyID)
        {
            return dal.ReadCompanyListByCompanyID(companyID);
        }

        /// <summary>
        /// 读取公司列表
        /// </summary>
        /// <param name="CompanyId">公司ID字符串</param>
        /// <returns></returns>
        public static List<CompanyInfo> ReadCompanyListByParentID(string parentID)
        {
            return dal.ReadCompanyListByParentID(parentID);
        }

        /// <summary>
        /// 返回父公司ID字符串
        /// </summary>
        /// <param name="CompanyId">开始的公司ID</param>
        /// <returns>返回父公司的ID字符串，以“,”分隔</returns>
        public static string ReadParentCompanyId(int CompanyId)
        {
            if (CompanyId > 0)
            {
                string TempStr = string.Empty;
                CompanyInfo CompanyModel = ReadCompany(CompanyId);
                if (!string.IsNullOrEmpty(CompanyModel.ParentId) && CompanyModel.ParentId != CompanyModel.CompanyId.ToString())
                {
                    TempStr = TempStr + "," + CompanyModel.ParentId;
                    if (CompanyModel.ParentId != "0")
                    {
                        foreach (string Item in CompanyModel.ParentId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            TempStr = TempStr + "," + ReadParentCompanyId(int.Parse(Item));
                        }
                    }
                }
                if (!string.IsNullOrEmpty(TempStr) && TempStr.StartsWith(",")) return TempStr.Substring(1);
                return TempStr;
            }
            return string.Empty;
        }

        /// <summary>
        /// 返回父公司ID字符串(包含当前公司ID)
        /// </summary>
        /// <returns></returns>
        public static string ReadParentCompanyIDWithSelf(int companyID)
        {
            string parentCompanyID = companyID.ToString();
            if (companyID > 0)
            {
                parentCompanyID = ReadParentCompanyId(companyID);
                parentCompanyID = string.IsNullOrEmpty(parentCompanyID) ? companyID.ToString() : string.Concat(companyID.ToString(), ",", parentCompanyID);
            }
            return parentCompanyID;
        }

        /// <summary>
        /// 返回权限内父公司名称
        /// </summary>
        /// <param name="CompanyId">开始的公司ID</param>
        /// <returns>返回父公司的ID字符串，以“,”分隔</returns>
        public static string ReadParentCompanyName(int CompanyId)
        {
            if (CompanyId != int.MinValue && CompanyId != 0)
            {
                string TempStr = string.Empty;
                CompanyInfo CompanyModel = dal.ReadCompany(CompanyId);
                if (CompanyModel != null && !string.IsNullOrEmpty(CompanyModel.ParentId))
                {
                    TempStr = TempStr + "-->" + CompanyModel.CompanyName;
                    if (CompanyModel.ParentId != "0")
                    {
                        foreach (string Item in CompanyModel.ParentId.Split(','))
                        {
                            if (!string.IsNullOrEmpty(Item))
                                TempStr = TempStr + "-->" + ReadParentCompanyName(int.Parse(Item));
                        }
                    }
                }
                if (TempStr != string.Empty && TempStr.StartsWith("-")) return TempStr.Substring(3);
                return TempStr;
            }
            return string.Empty;
        }

        /// <summary>
        /// 返回公司ID对应的公司名称
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public static string ReadParentCompanyName(string CompanyId)
        {
            string TempStr = string.Empty;
            if (!string.IsNullOrEmpty(CompanyId) && CompanyId != "0")
            {
                foreach (string Item in CompanyId.Split(','))
                {
                    TempStr += "<br>" + ReadParentCompanyName(int.Parse(Item));
                }
                if (TempStr.Length > 4 && TempStr.StartsWith("<br>")) return TempStr.Substring(4);
            }
            return TempStr;
        }

        /// <summary>
        /// 返回指定ID旗下的公司ID，以及指定ID的ID字符串
        /// </summary>
        /// <param name="CompanyId">指定的公司ID</param>
        /// <returns></returns>
        public static string ReadParentCompanyIdList(int CompanyId)
        {
            if (CompanyId != int.MinValue && CompanyId != 0)
            {
                string TempStr = string.Empty;
                List<CompanyInfo> CompanyIdList = ReadCompanyListByParentID(ReadParentCompanyId(CompanyId));
                if (CompanyIdList.Count > 0)
                {
                    foreach (CompanyInfo Item in CompanyIdList)
                    {

                        TempStr = TempStr + "," + Item.CompanyId;
                    }
                }
                return TempStr;
            }
            return string.Empty;
        }


        /// <summary>
        /// 根据公司名称关键字返回公司ID字符串
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string ReadCompanyIdStr(string Key)
        {
            return ReadCompanyIdStr(Key, 0);
        }

        /// <summary>
        /// 根据公司名称关键字返回公司ID字符串
        /// </summary>
        /// <param name="Key">如果为1，就是查找公司全称一致的，否则模糊查询</param>
        /// <returns></returns>
        public static string ReadCompanyIdStr(string Key, int Type)
        {
            string CompanyIdStr = string.Empty;
            CompanyInfo CompanyModel = new CompanyInfo();
            CompanyModel.CompanyName = Key;
            List<CompanyInfo> CompanyList = ReadCompanyList(CompanyModel);
            if (CompanyList != null && CompanyList.Count > 0)
            {
                foreach (CompanyInfo Item in CompanyList)
                {
                    if (Type == 1)
                    {
                        if (Item.CompanyName == Key)
                        {
                            CompanyIdStr = Item.CompanyId.ToString();
                            break;
                        }
                    }
                    else
                    {
                        CompanyIdStr = CompanyIdStr + "," + Item.CompanyId.ToString();
                    }
                }
            }
            if (CompanyIdStr.StartsWith(",")) CompanyIdStr = CompanyIdStr.Substring(1);
            return CompanyIdStr;
        }

        /// <summary>
        /// 读取公司列表
        /// </summary>
        /// <param name="Model">提供查询条件</param>
        /// <returns>返回条件下的list列表</returns>
        public static List<CompanyInfo> ReadCompanyList(CompanyInfo Model)
        {
            return dal.ReadCompanyList(Model);
        }

        public static List<CompanyInfo> ReadCompanyList(int currentPage, int pageSize, ref int count)
        {
            return dal.ReadCompanyList(currentPage, pageSize, ref count);
        }

        public static List<CompanyInfo> ReadCompanyList(CompanyInfo Model, int currentPage, int pageSize, ref int count)
        {
            return dal.ReadCompanyList(Model, currentPage, pageSize, ref count);
        }
    }
}
