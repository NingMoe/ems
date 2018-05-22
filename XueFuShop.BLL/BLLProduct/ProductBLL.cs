using System;
using System.Data;
using System.Collections.Generic;
using XueFuShop.Models;
using XueFuShop.IDAL;
using XueFuShop.Common;
using XueFu.EntLib;

namespace XueFuShop.BLL
{
    public sealed class ProductBLL
    {
        private static readonly string postCourseCacheKey = CacheKey.ReadCacheKey("PostCourse");
        private static readonly IProduct dal = FactoryHelper.Instance<IProduct>(Global.DataProvider, "ProductDAL");
        public static readonly int TableID = UploadTable.ReadTableID("Product");

        public static int AddProduct(ProductInfo product)
        {
            product.ID = dal.AddProduct(product);
            UploadBLL.UpdateUpload(TableID, 0, product.ID, Cookies.Admin.GetRandomNumber(false));
            return product.ID;
        }

        public static void ChangeProductCollectCount(int id, ChangeAction action)
        {
            dal.ChangeProductCollectCount(id, action);
        }

        public static void ChangeProductCollectCountByGeneral(string strID, ChangeAction action)
        {
            dal.ChangeProductCollectCountByGeneral(strID, action);
        }

        public static void ChangeProductCommentCountAndRank(int id, int rank, ChangeAction action)
        {
            dal.ChangeProductCommentCountAndRank(id, rank, action);
        }

        public static void ChangeProductCommentCountAndRankByGeneral(string strID, ChangeAction action)
        {
            dal.ChangeProductCommentCountAndRankByGeneral(strID, action);
        }

        public static void ChangeProductOrderCount(string strProductID, int changeCount)
        {
            dal.ChangeProductOrderCount(strProductID, changeCount);
        }

        public static void ChangeProductOrderCountByOrder(int orderID, ChangeAction changeAction)
        {
            dal.ChangeProductOrderCountByOrder(orderID, changeAction);
        }

        public static void ChangeProductPhotoCount(int id, ChangeAction action)
        {
            dal.ChangeProductPhotoCount(id, action);
        }

        public static void ChangeProductPhotoCountByGeneral(string strID, ChangeAction action)
        {
            dal.ChangeProductPhotoCountByGeneral(strID, action);
        }

        public static void ChangeProductSendCountByOrder(int orderID, ChangeAction changeAction)
        {
            dal.ChangeProductSendCountByOrder(orderID, changeAction);
        }

        public static void ChangeProductViewCount(int productID, int changeCount)
        {
            dal.ChangeProductViewCount(productID, changeCount);
        }

        public static void ChangProductAllowComment(int id, int status)
        {
            dal.ChangProductAllowComment(id, status);
        }

        public static void ChangProductIsHot(int id, int status)
        {
            dal.ChangProductIsHot(id, status);
        }

        public static void ChangProductIsNew(int id, int status)
        {
            dal.ChangProductIsNew(id, status);
        }

        public static void ChangProductIsSpecial(int id, int status)
        {
            dal.ChangProductIsSpecial(id, status);
        }

        public static void ChangProductIsTop(int id, int status)
        {
            dal.ChangProductIsTop(id, status);
        }

        public static void DeleteProduct(string strID)
        {
            UploadBLL.DeleteUploadByRecordID(TableID, strID);
            //ProductBrandBLL.ChangeProductBrandCountByGeneral(strID, ChangeAction.Minus);
            //ProductPhotoBLL.DeleteProductPhotoByProductID(strID);

            //������˲�Ʒ�й����Ĳ�Ʒ
            UpdateRelationProductByProductId(strID);
            //��������²�Ʒ�йصĸ�λ�ƻ�
            PostBLL.UpdatePostPlan(strID);
            //ɾ����Ʒ
            dal.DeleteProduct(strID);
        }

        public static DataTable NoHandlerStatistics()
        {
            return dal.NoHandlerStatistics();
        }

        public static void OffSaleProduct(string strID)
        {
            dal.OffSaleProduct(strID);
        }

        public static void OnSaleProduct(string strID)
        {
            dal.OnSaleProduct(strID);
        }

        public static ProductInfo ReadProduct(int id)
        {
            return dal.ReadProduct(id);
        }

        public static ProductInfo ReadProductByProductList(List<ProductInfo> productList, int productID)
        {
            ProductInfo info = new ProductInfo();
            foreach (ProductInfo info2 in productList)
            {
                if (info2.ID == productID)
                {
                    info = info2;
                }
            }
            return info;
        }

        /// <summary>
        /// ������ƷID��
        /// </summary>
        /// <param name="productList"></param>
        /// <returns></returns>
        public static string ReadProductIdStr(List<ProductInfo> productList)
        {
            string productIdStr = string.Empty;
            foreach (ProductInfo info in productList)
            {
                productIdStr += "," + info.ID;
            }
            if (productIdStr.StartsWith(",")) return productIdStr.Substring(1);
            return productIdStr;
        }

        /// <summary>
        /// ���ݵ�����Ʒ����ID��ȡ��Ʒ�б�
        /// </summary>
        /// <param name="productList"></param>
        /// <param name="classID"></param>
        /// <returns></returns>
        public static List<ProductInfo> ReadProductListByClassID(List<ProductInfo> productList, int classID)
        {
            List<ProductInfo> resultList = new List<ProductInfo>();
            foreach (ProductInfo info in productList)
            {
                if (StringHelper.CompareSingleString(info.ClassID, classID.ToString(), '|'))
                {
                    resultList.Add(info);
                }
            }
            return resultList;
        }

        /// <summary>
        /// ���ݲ�Ʒ�������Ʒ�б����
        /// </summary>
        /// <param name="productList">��Ʒ�б�</param>
        /// <param name="listCount">����</param>
        /// <returns></returns>
        public static Dictionary<string, List<ProductInfo>> SplitProductListByProductRootClass(List<ProductInfo> productList, int listCount)
        {
            return SplitProductListByProductClass(productList, 1, listCount);
        }

        /// <summary>
        /// ���ݲ�Ʒ�������Ʒ�б����
        /// </summary>
        /// <param name="productList">��Ʒ�б�</param>
        /// <param name="level">���鼶��</param>
        /// <param name="listCount">����</param>
        /// <returns></returns>
        public static Dictionary<string, List<ProductInfo>> SplitProductListByProductClass(List<ProductInfo> productList, int level, int listCount)
        {
            Dictionary<string, List<ProductInfo>> resultList = new Dictionary<string, List<ProductInfo>>();

            foreach (ProductInfo info in productList)
            {
                if (info.ClassID.Length > 0)
                {
                    string[] classIDArray = info.ClassID.Replace("||", "|$|").Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in classIDArray)
                    {
                        string[] idArray = item.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (idArray.Length >= level)
                        {
                            string classID = idArray[level - 1];
                            if (!resultList.ContainsKey(classID))
                            {
                                List<ProductInfo> product = new List<ProductInfo>();
                                product.Add(info);
                                resultList.Add(classID, product);
                                break;
                            }
                            else
                            {
                                if (listCount > 0 && resultList[classID].Count >= listCount)
                                    continue;
                                if (!resultList[classID].Contains(info))//classIDArray.Length > 1 && 
                                    resultList[classID].Add(info);
                            }
                        }
                    }
                }
            }
            return resultList;
        }

        public static int SearchProductNum(ProductSearchInfo productSearch)
        {
            return dal.SearchProductNum(productSearch);
        }

        public static List<ProductInfo> SearchProductList(ProductSearchInfo productSearch)
        {
            return dal.SearchProductList(productSearch);
        }

        public static List<ProductInfo> SearchProductList(int currentPage, int pageSize, ProductSearchInfo product, ref int count)
        {
            return dal.SearchProductList(currentPage, pageSize, product, ref count);
        }

        public static List<ProductInfo> SearchProductList(int currentPage, int pageSize, ProductSearchInfo productSearch, ref int count, int gradeID, decimal disCount)
        {
            return dal.SearchProductList(currentPage, pageSize, productSearch, ref count, gradeID, disCount);
        }

        public static DataTable StatisticsProductSale(int currentPage, int pageSize, ProductSearchInfo productSearch, ref int count, DateTime startDate, DateTime endDate)
        {
            return dal.StatisticsProductSale(currentPage, pageSize, productSearch, ref count, startDate, endDate);
        }

        public static void TaobaoProduct(ProductInfo product)
        {
            dal.TaobaoProduct(product);
        }

        public static void UnionUpdateProduct(string productIDList, ProductInfo product)
        {
            dal.UnionUpdateProduct(productIDList, product);
        }

        /// <summary>
        /// ��������ID�����²�Ʒ�������Ӹ�����ȥ��ָ�������ID��
        /// </summary>
        /// <param name="courseId"></param>
        public static void UpdateProductAccessoryByCourseId(int courseID)
        {
            ProductSearchInfo productsearch = new ProductSearchInfo();
            productsearch.Accessory = courseID.ToString();
            foreach (ProductInfo info in ProductBLL.SearchProductList(productsearch))
            {
                info.Accessory = StringHelper.SubString(info.Accessory, courseID.ToString());
                UpdateProduct(info);
            }
        }

        /// <summary>
        /// ȥ����Ʒ�Ĺ�����Ʒ��ָ���Ĳ�ƷID
        /// </summary>
        /// <param name="productID"></param>
        public static void UpdateRelationProductByProductId(string productID)
        {
            ProductSearchInfo productsearch = new ProductSearchInfo();
            productsearch.RelationProduct = productID;
            foreach (ProductInfo info in ProductBLL.SearchProductList(productsearch))
            {
                info.RelationProduct = StringHelper.SubString(info.RelationProduct, productID);
                UpdateProduct(info);
            }
        }

        public static void UpdateProduct(ProductInfo product)
        {
            dal.UpdateProduct(product);
            //���������ײ��ڵĿγ��޸ģ���ո�λ�γ̻���
            if (product.ClassID.IndexOf("|2|") != -1)
                CacheHelper.Remove(postCourseCacheKey);
            UploadBLL.UpdateUpload(TableID, 0, product.ID, Cookies.Admin.GetRandomNumber(false));
        }

        public static void UpdateProductCoverPhoto(int id, string photo)
        {
            dal.UpdateProductCoverPhoto(id, photo);
        }

        public static void UpdateProductStandardType(string strID, int standardType, int id)
        {
            dal.UpdateProductStandardType(strID, standardType, id);
        }
    }
}
