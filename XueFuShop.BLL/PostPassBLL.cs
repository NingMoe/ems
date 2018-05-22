using System;
using System.Collections.Generic;
using XueFu.EntLib;
using XueFuShop.Common;
using XueFuShop.IDAL;
using XueFuShop.Models;

namespace XueFuShop.BLL
{
    public class PostPassBLL
    {
        private static readonly IPostPass dal = FactoryHelper.Instance<IPostPass>(Global.DataProvider, "PostPassDAL");

        public static PostPassInfo ReadPostPassInfo()
        {
            return dal.ReadPostPassInfo();
        }

        /// <summary>
        /// 返回通过的岗位信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static PostPassInfo ReadPostPassInfo(int Id)
        {
            return dal.ReadPostPassInfo(Id);
        }

        public static PostPassInfo ReadPostPassInfo(int userID, int postID)
        {
            return dal.ReadPostPassInfo(userID, postID);
        }

        public static int AddPostPassInfo(PostPassInfo Model)
        {
            return dal.AddPostPassInfo(Model);
        }

        public static void UpdatePostPassInfo(PostPassInfo Model)
        {
            dal.UpdatePostPassInfo(Model);
        }

        /// <summary>
        /// 更新认证时间
        /// </summary>
        /// <param name="Model"></param>
        public static void UpdateCreateDate(PostPassInfo Model)
        {
            dal.UpdateCreateDate(Model);
        }

        public static void UpdateIsRZ(PostPassInfo Model)
        {
            dal.UpdateIsRZ(Model);
        }

        /// <summary>
        /// 返回通过的岗位信息列表
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static List<PostPassInfo> ReadPostPassList(PostPassInfo Model)
        {
            return dal.ReadPostPassList(Model);
        }

        public static List<PostPassInfo> ReadPostPassList(PostPassInfo postpassSearch, int currentPage, int pageSize, ref int count)
        {
            return dal.ReadPostPassList(postpassSearch, currentPage, pageSize, ref count);
        }

        public static int ReadPostPassNum(int userID)
        {
            return dal.ReadPostPassNum(userID);
        }

        /// <summary>
        /// 读取通过的岗位名称
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="endDate">截至日期</param>
        /// <param name="count">通过的岗位数量</param>
        /// <returns></returns>
        public static string ReadPassPostName(int userID, DateTime endDate, ref int count)
        {
            return ReadPassPostName(userID, DateTime.MinValue, endDate, ref count);
        }

        /// <summary>
        /// 读取通过的岗位名称
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">截至日期</param>
        /// <param name="count">通过的岗位数量</param>
        /// <returns></returns>
        public static string ReadPassPostName(int userID, DateTime startDate, DateTime endDate, ref int count)
        {
            string ReturnString = string.Empty;
            PostPassInfo postPass = new PostPassInfo();
            postPass.UserId = userID;
            postPass.SearchStartDate = startDate;
            postPass.CreateDate = endDate;
            postPass.IsRZ = 1;
            List<PostPassInfo> PostPassList = ReadPostPassList(postPass);
            if (PostPassList != null)
            {
                foreach (PostPassInfo Info in PostPassList)
                {
                    count++;
                    if (string.IsNullOrEmpty(ReturnString))
                        ReturnString = PostBLL.ReadPost(Info.PostId).PostName;
                    else
                        ReturnString = ReturnString + "," + PostBLL.ReadPost(Info.PostId).PostName;
                }
            }
            return ReturnString;
        }

        public static string PassPostString(PostPassInfo Model)
        {
            string ReturnString = "0";
            List<PostPassInfo> PostPassList = ReadPostPassList(Model);
            if (PostPassList != null)
            {
                foreach (PostPassInfo Info in PostPassList)
                {
                    ReturnString = ReturnString + "," + Info.PostId;
                }
            }
            return ReturnString;
        }

        //public static Dictionary<int, string> PostPassReportList(PostPassInfo Model, string CompanyId)
        //{
        //    Dictionary<int, string> PostPassList = new Dictionary<int, string>();
        //    List<PostPassInfo> PostPassModel = dal.PostPassRepostList(Model, CompanyId);
        //    foreach (PostPassInfo Info in PostPassModel)
        //    {
        //        //if (PostPassList.Exists(delegate(PostPassInfo TempModel) { if (TempModel.UserId == Info.UserId) return true; else return false; }))
        //        if (PostPassList.ContainsKey(Info.UserId))
        //        {
        //            foreach (int Item in PostPassList.Keys)
        //            {
        //                if (Item == Info.UserId)
        //                {
        //                    PostPassList[Item] = PostPassList[Item] + "," + Info.PostId;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            PostPassList.Add(Info.UserId, Info.PostId.ToString());
        //        }
        //    }
        //    return PostPassList;
        //}

        public static List<ReportPostPassInfo> PostPassReportList(PostPassInfo Model, string CompanyId)
        {
            List<ReportPostPassInfo> PostPassList = new List<ReportPostPassInfo>();
            List<ReportPostPassInfo> PostPassModel = dal.PostPassRepostList(Model, CompanyId);
            foreach (ReportPostPassInfo Info in PostPassModel)
            {
                if (PostPassList.Exists(delegate(ReportPostPassInfo TempModel) { if (TempModel.UserId == Info.UserId) return true; else return false; }))
                {
                    foreach (ReportPostPassInfo Item in PostPassList)
                    {
                        if (Item.UserId == Info.UserId)
                        {
                            Item.PassPostId = Item.PassPostId + "," + Info.PassPostId;
                            Item.PassPostName = Item.PassPostName + "、" + Info.PassPostName;
                        }
                    }
                }
                else
                {
                    PostPassList.Add(Info);
                }
            }
            return PostPassList;
        }

        /// <summary>
        /// 取得最新的num张岗位证书路径
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static List<string> GetTheLatestPostCert(int num)
        {
            List<string> certPathList = new List<string>();
            PostPassInfo postPass = new PostPassInfo();
            postPass.IsRZ = 1;
            List<PostPassInfo> PostPassList = dal.ReadPostPassList(postPass, num);
            foreach (PostPassInfo info in PostPassList)
            {
                UserInfo user = UserBLL.ReadUser(info.UserId);
                CertBLL certBLL = new CertBLL(user.CompanyID);
                if (string.IsNullOrEmpty(certBLL.GetCertPath(info.UserId, info.PostId)))
                    certBLL.Create(info.UserId, user.RealName, info.PostId, info.PostName);
                certPathList.Add(certBLL.GetCertPath(info.UserId, info.PostId));
            }
            return certPathList;
        }

        /// <summary>
        /// 核对岗位是否通过
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static void CheckPostPass(int userID, int productID)
        {
            string companyPostID = CookiesHelper.ReadCookieValue("UserCompanyPostSetting");
            ProductInfo product = ProductBLL.ReadProduct(productID);
            //如果本次通过的是综合考试，就更新通过岗位的记录状态
            if (StringHelper.CompareSingleString(product.ClassID, "4387", '|'))
            {
                PostPassInfo PostPassModel = new PostPassInfo();
                PostPassModel.UserId = userID;
                PostPassModel.PostId = RenZhengCateBLL.ReadTestCateByID(productID, companyPostID).PostId;
                PostPassModel.IsRZ = 1;
                PostPassBLL.UpdateIsRZ(PostPassModel);
            }
            else
            {
                //读取和本课程关联的岗位
                List<PostInfo> RelatedPostList = PostBLL.FilterPostListByCourseID(productID);
                if (RelatedPostList != null)
                {
                    //统计通过的课程ID
                    string PassCateId = TestPaperBLL.ReadCourseIDStr(TestPaperBLL.ReadList(userID, DateTime.Now.AddDays(1), 1));
                    string companyBrandID = CookiesHelper.ReadCookieValue("UserCompanyBrandID");
                    foreach (PostInfo Item in RelatedPostList)
                    {
                        //去除非公司设定的岗位
                        if (StringHelper.CompareSingleString(companyPostID, Item.PostId.ToString()))
                        {
                            string postCourseID = PostBLL.ReadPostCourseID(Item.PostId, companyBrandID);
                            if (string.IsNullOrEmpty(StringHelper.SubString(postCourseID, PassCateId)))
                            {
                                PostPassInfo PostPassModel = PostPassBLL.ReadPostPassInfo(userID, Item.PostId);
                                if (PostPassModel.Id <= 0)
                                {
                                    PostPassModel.UserId = userID;
                                    PostPassModel.PostId = Item.PostId;
                                    PostPassModel.PostName = Item.PostName;
                                    if (Item.PostId == 220) //钣金喷漆岗位直接发证书
                                        PostPassModel.IsRZ = 1;
                                    else
                                        PostPassModel.IsRZ = 0;
                                    PostPassBLL.AddPostPassInfo(PostPassModel);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
