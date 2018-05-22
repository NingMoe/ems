using System;
using System.Data.SqlClient;
using XueFuShop.DAL;
using XueFuShop.Models;

namespace XueFuShop.BLL
{
    public class BLLUser
    {
        private DALMXUser dal = new DALMXUser();
        private string _ErrMessage;//定义要返回的错误信息

        public string ErrMessage
        {
            get { return (this._ErrMessage); }
            set { this._ErrMessage = value; }
        }

        public bool UserLogin(string UserEmail,string UserPwd)
        {            
            MXUser Model = new MXUser();
            Model.UserEmail = UserEmail;
            Model.UserPwd = UserPwd;
            Model.LastLoginDate=DateTime.Now;
            SqlDataReader dr = dal.UserLogin(Model);
            if (dr.HasRows)
            {
                dr.Read();
                if ((bool)dr["IsChecked"])
                {
                    if (!(bool)dr["IsLocked"])
                    {
                        System.Web.HttpContext.Current.Session["UserName"] = dr["UserName"];
                        System.Web.HttpContext.Current.Session["UserId"] = dr["MXUserId"];
                        return true;
                    }
                    else
                    {
                        _ErrMessage = "您好，帐号【" + dr["UserName"] + "】已被锁定，请联系管理员！";
                        return false;
                    }
                }
                else
                {
                    _ErrMessage = "您还没有通过审核，请等待管理员审核后登陆！";
                    return false;
                }
                Model.UserId = (int)dr["MXUserId"];
                dal.UpdateLastLoginDate(Model);
            }
            else
            {
                _ErrMessage = "用户名或密码错误，请重新登陆！";
                return false;
            }
            
        }
        public int InsertUserInfor(string UserEmail,string UserPwd)
        {
            MXUser Model = new MXUser();
            Model.UserEmail = UserEmail;
            Model.UserPwd = UserPwd;
            return dal.InsertUserInfor(Model);
        }

        public bool InsertUserInfor(MXUser Model)
        {
            if (dal.InsertUserInfor(Model) > 0)
            {
                return true;
            }
            else
            {
                _ErrMessage = "数据录入异常，请再次录入！";
                return false;            
            }
        }
        public bool CheckUserEmail(string UserEmail)
        {
            return dal.IsVoidUserEmail(UserEmail);            
        }

        /// <summary>
        /// 自定义-验证用户是否存在
        /// </summary>
        /// <param name="userNum">用户登陆号</param>
        public bool CheckUserIdExist(string Field , string Value)
        {
            if (Field != "" && Value != "")
            {
                int? count = dal.IsExistUserId(Field, Value);
                if (count == null)
                {
                    this._ErrMessage = "查询数据库发生异常错误！";
                    return false;
                }
                else if (count == 0)
                {
                    this._ErrMessage = "该用户不存在，请查证后登陆！";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                _ErrMessage = "参数为空，非法操作！";
                return false;
            }
        }

        public SqlDataReader GetUserInfor(MXUser aa)
        {
            return dal.GetUserInfor(aa);
        }

        //读取会员信息
        public SqlDataReader GetUserInforByOrder(string Order)
        {            
            return dal.GetUserInforByOrder(Order);
        }

        public SqlDataReader GetUserInforByOrder(string Field, string Value, string Order)
        {
            return dal.GetUserInforByOrder(Field, Value, Order);
        }

        public SqlDataReader GetUserIdByUserEmail(MXUser aa)
        {
            return dal.GetUserIdByUserEmail(aa);
        }
        public SqlDataReader GetUserInforByUserEmail(MXUser aa)
        {
            return dal.GetUserInforByUserEmail(aa);
        }
        public int UpdateUserInfor(MXUser aa)
        {
            return dal.UpdateUserInfor(aa);
        }
        public int UpdateUserContact(MXUser aa)
        {
            return dal.UpdateUserContact(aa);
        }
        public SqlDataReader CheckUserPwd(MXUser aa)
        {
            return dal.CheckUserPwd(aa);
        }
        public int UpdateLastLoginDate(MXUser aa)
        {
            return dal.UpdateLastLoginDate(aa);
        }
        public int UpdateUserPwd(MXUser aa)
        {
            return dal.UpdateUserPwd(aa);
        }
        public MXUser GetModel(System.Data.DataRow aa)
        {
            return dal.GetModel(aa);
        }
    }
}
