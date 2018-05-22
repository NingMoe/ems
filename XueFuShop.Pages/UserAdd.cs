using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using XueFu.EntLib;
using XueFuShop.BLL;
using XueFuShop.Common;
using XueFuShop.Models;

namespace XueFuShop.Pages
{
    public class UserAdd : UserManageBasePage
    {
        protected SingleUnlimitClass singleUnlimitClass = new SingleUnlimitClass();
        protected UserInfo user = new UserInfo();
        protected List<AdminGroupInfo> powerGroupList = new List<AdminGroupInfo>();
        protected int userID = RequestHelper.GetQueryString<int>("ID");
        protected List<PostInfo> departmentList = new List<PostInfo>();
        protected List<PostInfo> workingPostList = new List<PostInfo>();
        protected List<PostInfo> studyPostList = new List<PostInfo>();

        protected override void PageLoad()
        {
            base.PageLoad();
            base.CheckUserPower("AddUser,UpdateUser,ReadUser", PowerCheckType.OR);

            ////this.singleUnlimitClass.DataSource = RegionBLL.ReadRegionUnlimitClass();
            ////this.singleUnlimitClass.ClassID = this.user.RegionID;

            string companyPostSetting = CookiesHelper.ReadCookieValue("UserCompanyPostSetting");
            departmentList = PostBLL.ReadParentPostListByPostId(companyPostSetting);
            studyPostList = workingPostList = PostBLL.ReadPostListByPostId(companyPostSetting);
            //去除新加的培训项目
            if (!StringHelper.CompareString(ParentCompanyID, "779,296,733,770"))
            {
                departmentList = departmentList.FindAll(delegate(PostInfo tempPost) { return tempPost.CompanyID == CompanyBLL.SystemCompanyId; });
                workingPostList = workingPostList.FindAll(delegate(PostInfo tempPost) { return tempPost.CompanyID == CompanyBLL.SystemCompanyId; });
            }
            if (userID > 0)
            {
                this.user = UserBLL.ReadUser(userID);
                base.Title = "修改员工信息";

                //当前学习岗位课程通过后，销售与非销售之间可以调整，否则不能互调
                if (!PostBLL.IsPassPost(user.CompanyID, user.ID, user.StudyPostID))
                {
                    //公司培训列表信息
                    List<PostInfo> companyTrainingList = studyPostList.FindAll(delegate(PostInfo tempPost) { return tempPost.CompanyID != CompanyBLL.SystemCompanyId; });

                    PostInfo studyPost = PostBLL.ReadPost(user.StudyPostID);
                    //销售部岗位单独处理
                    if (studyPost.ParentId == 3)
                    {
                        studyPostList = PostBLL.FilterPostListByParentID(studyPostList, 3);
                    }
                    else
                    {
                        studyPostList = studyPostList.FindAll(delegate(PostInfo tempPost) { return tempPost.ParentId != 3; });
                    }

                    //如果公司培训项目不为空，则罗列出来
                    if (companyTrainingList.Count > 0 && studyPost.CompanyID != user.CompanyID)
                        studyPostList.AddRange(companyTrainingList);

                }
            }
            else
                base.Title = "增加员工";
            //powerGroupList = AdminGroupBLL.ReadAdminGroupList(base.CompanyID);

        }

        protected override void PostBack()
        {
            user = UserBLL.ReadUser(userID);

            int oldCompanyID = user.CompanyID;
            int oldStudyPostID = user.StudyPostID;

            user.CompanyID = RequestHelper.GetForm<int>("CompanyID");
            if (user.CompanyID < 0) user.CompanyID = oldCompanyID == int.MinValue ? base.UserCompanyID : oldCompanyID;

            user.Department = RequestHelper.GetForm<int>("Department");
            if (user.Department < 0) ScriptHelper.Alert("请选择部门");

            user.WorkingPostID = RequestHelper.GetForm<int>("WorkingPostID");
            if (user.WorkingPostID < 0) ScriptHelper.Alert("请选择岗位");

            user.PostName = StringHelper.AddSafe(RequestHelper.GetForm<string>("WorkingPostName"));
            if (user.WorkingPostID < 0 && string.IsNullOrEmpty(user.PostName)) ScriptHelper.Alert("请填写店内岗位名称");

            user.StudyPostID = RequestHelper.GetForm<int>("StudyPostID");
            if (user.StudyPostID < 0) ScriptHelper.Alert("请选择学习岗位");

            user.Email = StringHelper.AddSafe(RequestHelper.GetForm<string>("Email"));
            user.Sex = RequestHelper.GetForm<int>("Sex");
            user.QQ = StringHelper.AddSafe(RequestHelper.GetForm<string>("QQ"));
            user.Status = RequestHelper.GetForm<int>("Status");
            if (user.Status < 0)
                ScriptHelper.Alert("请选择状态");

            //新增会员或有更改姓名的权限
            if (userID <= 0 || base.CompareUserPower("UpdateRealName", PowerCheckType.Single))
            {
                user.RealName = StringHelper.AddSafe(RequestHelper.GetForm<string>("RealName"));
            }

            //新增会员或有更改手机号码的权限
            if (userID <= 0 || base.CompareUserPower("UpdateMobile", PowerCheckType.Single))
            {
                user.Mobile = StringHelper.AddSafe(RequestHelper.GetForm<string>("Mobile"));
                if (string.IsNullOrEmpty(user.Mobile)) ScriptHelper.Alert("请正确填写手机号码!");
            }

            //验证手机号码是否存在
            //if (UserBLL.IsExistMobile(user.Mobile, user.ID))
            //    ScriptHelper.Alert("手机号码已存在");

            if (base.CompareUserPower("ShowIDCard", PowerCheckType.Single))
                user.IDCard = StringHelper.AddSafe(RequestHelper.GetForm<string>("IDCard"));

            if (base.CompareUserPower("ShowIDCard", PowerCheckType.Single) && !string.IsNullOrEmpty(RequestHelper.GetForm<string>("EntryDate")))
                user.EntryDate = RequestHelper.GetForm<DateTime>("EntryDate");

            string alertMessage = ShopLanguage.ReadLanguage("AddOK");
            if (userID < 0)
            {
                base.CheckUserPower("AddUser", PowerCheckType.Single);
                user.Status = (int)UserState.Normal;

                if (UserBLL.IsUserNumOverflow(user.CompanyID))
                {
                    ScriptHelper.Alert("超过用户数量，暂不能添加！");
                }

                user.UserName = StringHelper.AddSafe(RequestHelper.GetForm<string>("UserName"));
                Regex regex = new Regex("^([a-zA-Z0-9_一-龥])+$");
                if (!regex.IsMatch(user.UserName))
                {
                    ScriptHelper.Alert("用户名只能包含字母、数字、下划线、中文");
                }

                if (UserBLL.CheckUserName(user.UserName) > 0)
                {
                    ScriptHelper.Alert("用户名已存在，请更换用户名!");
                }

                string userPassword = RequestHelper.GetForm<string>("UserPassword");
                string userPassword2 = RequestHelper.GetForm<string>("UserPassword2");
                if (string.IsNullOrEmpty(userPassword) || string.IsNullOrEmpty(userPassword2) || userPassword != userPassword2) ScriptHelper.Alert("两次密码不一致");
                regex = new Regex("(?=^[0-9a-zA-Z._@#]{6,16}$)((?=.*[0-9])(?=.*[^0-9])|(?=.*[a-zA-Z])(?=.*[^a-zA-Z]))");
                if (!regex.IsMatch(userPassword)) ScriptHelper.Alert("密码为6-16位数字与字母的组合！");
                user.UserPassword = StringHelper.Password(userPassword, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);

                user.GroupID = RequestHelper.GetForm<int>("GroupID");
                if (user.GroupID < 0) user.GroupID = 36;

                int id = UserBLL.AddUser(user);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("User"), id);
            }
            else
            {
                base.CheckUserPower("UpdateUser", PowerCheckType.Single);

                //变换学习岗位，如果岗位已通过则更新注册时间，以便岗位计划按新时间重新计数，否则不更改原有岗位计划统计时间
                PostInfo studyPost = PostBLL.ReadPost(user.StudyPostID);
                PostInfo oldStudyPost = PostBLL.ReadPost(oldStudyPostID);
                if (oldCompanyID != user.CompanyID || (user.StudyPostID != oldStudyPostID && (studyPost.ParentId == 3 || oldStudyPost.ParentId == 3) && studyPost.ParentId != oldStudyPost.ParentId))
                {
                    user.PostStartDate = DateTime.Today;
                }

                UserBLL.UpdateUser(user);

                //如果公司ID更改，相应修改成绩列表
                if (oldCompanyID != user.CompanyID)
                {
                    TestPaperBLL.UpdatePaperCompanyId(user.ID, user.CompanyID);
                }

                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("UpdateRecord"), ShopLanguage.ReadLanguage("User"), user.ID);
                alertMessage = ShopLanguage.ReadLanguage("UpdateOK");
            }
            string returnURL = ServerHelper.UrlDecode(RequestHelper.GetQueryString<string>("ReturnURL"));
            if (string.IsNullOrEmpty(returnURL))
                ScriptHelper.Alert(alertMessage, RequestHelper.RawUrl);
            else
                ScriptHelper.Alert(alertMessage, returnURL);
        }

        //protected string UploadUserPhoto()
        //{
        //    string filePath = string.Empty;
        //    if (HttpContext.Current.Request.Files[0].FileName != string.Empty)
        //    {
        //        try
        //        {
        //            UploadHelper helper = new UploadHelper();
        //            helper.Path = "/Upload/UserPhoto/Original/";
        //            helper.FileType = ShopConfig.ReadConfigInfo().UploadFile;
        //            FileInfo info = helper.SaveAs();
        //            filePath = helper.Path + info.Name;
        //            string str2 = string.Empty;
        //            string str3 = string.Empty;
        //            Dictionary<int, int> dictionary = new Dictionary<int, int>();
        //            dictionary.Add(70, 70);
        //            dictionary.Add(190, 190);
        //            foreach (KeyValuePair<int, int> pair in dictionary)
        //            {
        //                str3 = filePath.Replace("Original", pair.Key.ToString() + "-" + pair.Value.ToString());
        //                str2 = str2 + str3 + "|";
        //                ImageHelper.MakeThumbnailImage(ServerHelper.MapPath(filePath), ServerHelper.MapPath(str3), pair.Key, pair.Value, ThumbnailType.InBox);
        //            }
        //            str2 = str2.Substring(0, str2.Length - 1);
        //            UploadInfo upload = new UploadInfo();
        //            upload.TableID = UserBLL.TableID;
        //            upload.ClassID = 0;
        //            upload.RecordID = 0;
        //            upload.UploadName = filePath;
        //            upload.OtherFile = str2;
        //            upload.Size = Convert.ToInt32(info.Length);
        //            upload.FileType = info.Extension;
        //            upload.Date = RequestHelper.DateNow;
        //            upload.IP = ClientHelper.IP;
        //            UploadBLL.AddUpload(upload);
        //        }
        //        catch (Exception exception)
        //        {
        //            ExceptionHelper.ProcessException(exception, false);
        //        }
        //    }
        //    return filePath;
        //}
    }



}
