using System;
using System.Collections.Generic;
using XueFuShop.Models;
using XueFuShop.IDAL;
using XueFuShop.Common;

using XueFu.EntLib;

namespace XueFuShop.BLL
{
    public sealed class ArticleBLL
    {

        private static readonly string cacheKey = CacheKey.ReadCacheKey("BottomList");
        private static readonly IArticle dal = FactoryHelper.Instance<IArticle>(Global.DataProvider, "ArticleDAL");
        public static readonly int TableID = UploadTable.ReadTableID("Article");
        private static Dictionary<int, ArticleInfo> FirstArticleDc = null;

        public static int AddArticle(ArticleInfo article)
        {
            article.ID = dal.AddArticle(article);
            UploadBLL.UpdateUpload(TableID, 0, article.ID, Cookies.Admin.GetRandomNumber(false));
            if (article.ClassID.IndexOf("|" + 2 + "|") > -1)
            {
                CacheHelper.Remove(cacheKey);
            }
            FirstArticleDc = null;
            return article.ID;
        }

        public static void DeleteArticle(string strID)
        {
            UploadBLL.DeleteUploadByRecordID(TableID, strID);
            dal.DeleteArticle(strID);
            CacheHelper.Remove(cacheKey);
            FirstArticleDc = null;
        }

        public static ArticleInfo ReadArticle(int id)
        {
            return dal.ReadArticle(id);
        }

        public static ArticleInfo ReadFirstAtricle(int classID)
        {
            if (FirstArticleDc==null || !FirstArticleDc.ContainsKey(classID))
            {
                int page = 1;
                int pageSize = 1;
                int count = 0;
                if (FirstArticleDc == null)
                {
                    FirstArticleDc = new Dictionary<int, ArticleInfo>();
                }

                ArticleSearchInfo articleSearch = new ArticleSearchInfo();
                articleSearch.ClassID = "|" + classID + "|";
                List<ArticleInfo> articleList = ArticleBLL.SearchArticleList(page, pageSize, articleSearch, ref count);
                if (articleList.Count > 0)
                {
                    FirstArticleDc.Add(classID, articleList[0]);
                }
            }
            return FirstArticleDc[classID];
        }

        public static List<ArticleInfo> ReadBottomList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                ArticleSearchInfo articleSearch = new ArticleSearchInfo();
                articleSearch.ClassID = "|" + 2 + "|";
                CacheHelper.Write(cacheKey, dal.SearchArticleList(articleSearch));
            }
            return (List<ArticleInfo>)CacheHelper.Read(cacheKey);
        }

        public static List<ArticleInfo> SearchArticleList(ArticleSearchInfo articleSearch)
        {
            return dal.SearchArticleList(articleSearch);
        }

        public static List<ArticleInfo> SearchArticleList(int currentPage, int pageSize, ArticleSearchInfo article, ref int count)
        {
            return dal.SearchArticleList(currentPage, pageSize, article, ref count);
        }

        public static void UpdateArticle(ArticleInfo article)
        {
            dal.UpdateArticle(article);
            CacheHelper.Remove(cacheKey);
            FirstArticleDc = null;
            UploadBLL.UpdateUpload(TableID, 0, article.ID, Cookies.Admin.GetRandomNumber(false));
        }
    }
}
