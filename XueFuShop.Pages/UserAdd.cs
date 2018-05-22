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
            //ȥ���¼ӵ���ѵ��Ŀ
            if (!StringHelper.CompareString(ParentCompanyID, "779,296,733,770"))
            {
                departmentList = departmentList.FindAll(delegate(PostInfo tempPost) { return tempPost.CompanyID == CompanyBLL.SystemCompanyId; });
                workingPostList = workingPostList.FindAll(delegate(PostInfo tempPost) { return tempPost.CompanyID == CompanyBLL.SystemCompanyId; });
            }
            if (userID > 0)
            {
                this.user = UserBLL.ReadUser(userID);
                base.Title = "�޸�Ա����Ϣ";

                //��ǰѧϰ��λ�γ�ͨ���������������֮����Ե����������ܻ���
                if (!PostBLL.IsPassPost(user.CompanyID, user.ID, user.StudyPostID))
                {
                    //��˾��ѵ�б���Ϣ
                    List<PostInfo> companyTrainingList = studyPostList.FindAll(delegate(PostInfo tempPost) { return tempPost.CompanyID != CompanyBLL.SystemCompanyId; });

                    PostInfo studyPost = PostBLL.ReadPost(user.StudyPostID);
                    //���۲���λ��������
                    if (studyPost.ParentId == 3)
                    {
                        studyPostList = PostBLL.FilterPostListByParentID(studyPostList, 3);
                    }
                    else
                    {
                        studyPostList = studyPostList.FindAll(delegate(PostInfo tempPost) { return tempPost.ParentId != 3; });
                    }

                    //�����˾��ѵ��Ŀ��Ϊ�գ������г���
                    if (companyTrainingList.Count > 0 && studyPost.CompanyID != user.CompanyID)
                        studyPostList.AddRange(companyTrainingList);

                }
            }
            else
                base.Title = "����Ա��";
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
            if (user.Department < 0) ScriptHelper.Alert("��ѡ����");

            user.WorkingPostID = RequestHelper.GetForm<int>("WorkingPostID");
            if (user.WorkingPostID < 0) ScriptHelper.Alert("��ѡ���λ");

            user.PostName = StringHelper.AddSafe(RequestHelper.GetForm<string>("WorkingPostName"));
            if (user.WorkingPostID < 0 && string.IsNullOrEmpty(user.PostName)) ScriptHelper.Alert("����д���ڸ�λ����");

            user.StudyPostID = RequestHelper.GetForm<int>("StudyPostID");
            if (user.StudyPostID < 0) ScriptHelper.Alert("��ѡ��ѧϰ��λ");

            user.Email = StringHelper.AddSafe(RequestHelper.GetForm<string>("Email"));
            user.Sex = RequestHelper.GetForm<int>("Sex");
            user.QQ = StringHelper.AddSafe(RequestHelper.GetForm<string>("QQ"));
            user.Status = RequestHelper.GetForm<int>("Status");
            if (user.Status < 0)
                ScriptHelper.Alert("��ѡ��״̬");

            //������Ա���и���������Ȩ��
            if (userID <= 0 || base.CompareUserPower("UpdateRealName", PowerCheckType.Single))
            {
                user.RealName = StringHelper.AddSafe(RequestHelper.GetForm<string>("RealName"));
            }

            //������Ա���и����ֻ������Ȩ��
            if (userID <= 0 || base.CompareUserPower("UpdateMobile", PowerCheckType.Single))
            {
                user.Mobile = StringHelper.AddSafe(RequestHelper.GetForm<string>("Mobile"));
                if (string.IsNullOrEmpty(user.Mobile)) ScriptHelper.Alert("����ȷ��д�ֻ�����!");
            }

            //��֤�ֻ������Ƿ����
            //if (UserBLL.IsExistMobile(user.Mobile, user.ID))
            //    ScriptHelper.Alert("�ֻ������Ѵ���");

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
                    ScriptHelper.Alert("�����û��������ݲ�����ӣ�");
                }

                user.UserName = StringHelper.AddSafe(RequestHelper.GetForm<string>("UserName"));
                Regex regex = new Regex("^([a-zA-Z0-9_һ-��])+$");
                if (!regex.IsMatch(user.UserName))
                {
                    ScriptHelper.Alert("�û���ֻ�ܰ�����ĸ�����֡��»��ߡ�����");
                }

                if (UserBLL.CheckUserName(user.UserName) > 0)
                {
                    ScriptHelper.Alert("�û����Ѵ��ڣ�������û���!");
                }

                string userPassword = RequestHelper.GetForm<string>("UserPassword");
                string userPassword2 = RequestHelper.GetForm<string>("UserPassword2");
                if (string.IsNullOrEmpty(userPassword) || string.IsNullOrEmpty(userPassword2) || userPassword != userPassword2) ScriptHelper.Alert("�������벻һ��");
                regex = new Regex("(?=^[0-9a-zA-Z._@#]{6,16}$)((?=.*[0-9])(?=.*[^0-9])|(?=.*[a-zA-Z])(?=.*[^a-zA-Z]))");
                if (!regex.IsMatch(userPassword)) ScriptHelper.Alert("����Ϊ6-16λ��������ĸ����ϣ�");
                user.UserPassword = StringHelper.Password(userPassword, (PasswordType)ShopConfig.ReadConfigInfo().PasswordType);

                user.GroupID = RequestHelper.GetForm<int>("GroupID");
                if (user.GroupID < 0) user.GroupID = 36;

                int id = UserBLL.AddUser(user);
                AdminLogBLL.AddAdminLog(ShopLanguage.ReadLanguage("AddRecord"), ShopLanguage.ReadLanguage("User"), id);
            }
            else
            {
                base.CheckUserPower("UpdateUser", PowerCheckType.Single);

                //�任ѧϰ��λ�������λ��ͨ�������ע��ʱ�䣬�Ա��λ�ƻ�����ʱ�����¼��������򲻸���ԭ�и�λ�ƻ�ͳ��ʱ��
                PostInfo studyPost = PostBLL.ReadPost(user.StudyPostID);
                PostInfo oldStudyPost = PostBLL.ReadPost(oldStudyPostID);
                if (oldCompanyID != user.CompanyID || (user.StudyPostID != oldStudyPostID && (studyPost.ParentId == 3 || oldStudyPost.ParentId == 3) && studyPost.ParentId != oldStudyPost.ParentId))
                {
                    user.PostStartDate = DateTime.Today;
                }

                UserBLL.UpdateUser(user);

                //�����˾ID���ģ���Ӧ�޸ĳɼ��б�
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
