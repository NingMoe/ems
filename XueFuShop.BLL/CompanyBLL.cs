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
        /// ϵͳID
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
        /// ��ȡ��˾��Ϣ
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static CompanyInfo ReadCompany(int Id)//����ʵ�����������Ϊ��ʹ�õ��Ĺ�˾��Ϣ�����������������ݿ�ķ��ʴ���
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
        /// ���ݹ�˾IDɾ����˾���Լ�����ɾ���Լ���
        /// </summary>
        /// <param name="Id">��˾ID��</param>
        public static void DeleteCompany(int Id)
        {
            CompanyInfo Model = new CompanyInfo();
            Model.ParentIdCondition = Id.ToString();
            //Model.CompanyId = CompanyId;
            if (ReadCompanyList(Model).Count > 0)
            {
                ScriptHelper.Alert("�������ӹ�˾������ɾ���ӹ�˾���ٽ���ɾ����");
            }
            else
            {
                UserBLL.DeleteUserByCompanyID(Id);
                dal.DeleteCompany(Id);
                companyDic.Remove(Id);
            }
        }
        /// <summary>
        /// ��ȡ���µĹ�˾ID(��������˾ID)
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
        /// ȡ�ù�˾�����¹�˾�б�
        /// </summary>
        /// <param name="CompanyId">��˾ID�ַ���</param>
        /// <returns></returns>
        public static List<CompanyInfo> ReadCompanyList(string CompanyId)
        {
            return ReadCompanyListByCompanyId(ReadCompanyIdList(CompanyId));
        }

        /// <summary>
        /// ��ȡ��˾�б�
        /// </summary>
        /// <param name="CompanyId">��˾ID�ַ���</param>
        /// <returns></returns>
        public static List<CompanyInfo> ReadCompanyListByCompanyId(string companyID)
        {
            return dal.ReadCompanyListByCompanyID(companyID);
        }

        /// <summary>
        /// ��ȡ��˾�б�
        /// </summary>
        /// <param name="CompanyId">��˾ID�ַ���</param>
        /// <returns></returns>
        public static List<CompanyInfo> ReadCompanyListByParentID(string parentID)
        {
            return dal.ReadCompanyListByParentID(parentID);
        }

        /// <summary>
        /// ���ظ���˾ID�ַ���
        /// </summary>
        /// <param name="CompanyId">��ʼ�Ĺ�˾ID</param>
        /// <returns>���ظ���˾��ID�ַ������ԡ�,���ָ�</returns>
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
        /// ���ظ���˾ID�ַ���(������ǰ��˾ID)
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
        /// ����Ȩ���ڸ���˾����
        /// </summary>
        /// <param name="CompanyId">��ʼ�Ĺ�˾ID</param>
        /// <returns>���ظ���˾��ID�ַ������ԡ�,���ָ�</returns>
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
        /// ���ع�˾ID��Ӧ�Ĺ�˾����
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
        /// ����ָ��ID���µĹ�˾ID���Լ�ָ��ID��ID�ַ���
        /// </summary>
        /// <param name="CompanyId">ָ���Ĺ�˾ID</param>
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
        /// ���ݹ�˾���ƹؼ��ַ��ع�˾ID�ַ���
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string ReadCompanyIdStr(string Key)
        {
            return ReadCompanyIdStr(Key, 0);
        }

        /// <summary>
        /// ���ݹ�˾���ƹؼ��ַ��ع�˾ID�ַ���
        /// </summary>
        /// <param name="Key">���Ϊ1�����ǲ��ҹ�˾ȫ��һ�µģ�����ģ����ѯ</param>
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
        /// ��ȡ��˾�б�
        /// </summary>
        /// <param name="Model">�ṩ��ѯ����</param>
        /// <returns>���������µ�list�б�</returns>
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
