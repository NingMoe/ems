using System;
using System.Data.SqlClient;
using XueFuShop.DAL;
using XueFuShop.Models;

namespace XueFuShop.BLL
{
    public class BLLUser
    {
        private DALMXUser dal = new DALMXUser();
        private string _ErrMessage;//����Ҫ���صĴ�����Ϣ

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
                        _ErrMessage = "���ã��ʺš�" + dr["UserName"] + "���ѱ�����������ϵ����Ա��";
                        return false;
                    }
                }
                else
                {
                    _ErrMessage = "����û��ͨ����ˣ���ȴ�����Ա��˺��½��";
                    return false;
                }
                Model.UserId = (int)dr["MXUserId"];
                dal.UpdateLastLoginDate(Model);
            }
            else
            {
                _ErrMessage = "�û�����������������µ�½��";
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
                _ErrMessage = "����¼���쳣�����ٴ�¼�룡";
                return false;            
            }
        }
        public bool CheckUserEmail(string UserEmail)
        {
            return dal.IsVoidUserEmail(UserEmail);            
        }

        /// <summary>
        /// �Զ���-��֤�û��Ƿ����
        /// </summary>
        /// <param name="userNum">�û���½��</param>
        public bool CheckUserIdExist(string Field , string Value)
        {
            if (Field != "" && Value != "")
            {
                int? count = dal.IsExistUserId(Field, Value);
                if (count == null)
                {
                    this._ErrMessage = "��ѯ���ݿⷢ���쳣����";
                    return false;
                }
                else if (count == 0)
                {
                    this._ErrMessage = "���û������ڣ����֤���½��";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                _ErrMessage = "����Ϊ�գ��Ƿ�������";
                return false;
            }
        }

        public SqlDataReader GetUserInfor(MXUser aa)
        {
            return dal.GetUserInfor(aa);
        }

        //��ȡ��Ա��Ϣ
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
