using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;


using XueFuShop.Models;
using XueFu.EntLib;

namespace XueFuShop.DAL
{
    public class DALMXUser
    {
        public List<MXUser> SearchUserList(int currentPage, int pageSize, UserSearchInfo userSearch, ref int count)
        {
            List<MXUser> userList = new List<MXUser>();
            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = ShopMssqlHelper.TablePrefix + "User";
            class2.Fields = "[ID],[UserName],[UserPassword],[Email],[Sex],[Introduce],[Photo],[MSN],[QQ],[Tel],[Mobile],[RegionID],[Address],[Birthday],[RegisterIP],[RegisterDate],[LastLoginIP],[LastLoginDate],[LoginTimes],[SafeCode],[FindDate],[Status],[OpenID]";
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[ID]";
            class2.OrderType = OrderType.Desc;
            this.PrepareCondition(class2.MssqlCondition, userSearch);
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader reader = class2.ExecuteReader())
            {
                this.PrepareUserModel(reader, userList);
            }
            return userList;
        }


        public void PrepareCondition(MssqlCondition mssqlCondition, UserSearchInfo userSearch)
        {
            mssqlCondition.Add("[UserName]", userSearch.UserName, ConditionType.Like);
            mssqlCondition.Add("[Email]", userSearch.Email, ConditionType.Like);
            mssqlCondition.Add("[Sex]", userSearch.Sex, ConditionType.Equal);
            mssqlCondition.Add("[RegisterDate]", userSearch.StartRegisterDate, ConditionType.MoreOrEqual);
            mssqlCondition.Add("[RegisterDate]", userSearch.EndRegisterDate, ConditionType.Less);
            mssqlCondition.Add("[Status]", userSearch.Status, ConditionType.Equal);
            mssqlCondition.Add("[ID]", userSearch.InUserID, ConditionType.In);
        }


        public void PrepareUserModel(SqlDataReader dr, List<MXUser> userList)
        {
            while (dr.Read())
            {
                MXUser item = new MXUser();
                item.UserId = dr.GetInt32(0);
                item.UserName = dr["UserName"].ToString();
                item.UserPwd = dr["UserPwd"].ToString();
                item.UserEmail = dr["UserEmail"].ToString();
                item.UserSex = dr.GetInt32(4);
                //item.Introduce = dr[5].ToString();
                //item.Photo = dr[6].ToString();
                item.UserMsn = dr["UserMsn"].ToString();
                item.UserQQ = dr["UserQQ"].ToString();
                item.UserTel = dr["UserTel"].ToString();
                item.UserMobile = dr["UserMobile"].ToString();
                //item.RegionID = dr[11].ToString();
                item.UserAddress = dr["UserAddress"].ToString();
                item.UserBirthday = dr["UserBirthday"].ToString();
                //item.RegisterIP = dr[14].ToString();
                item.RegDate = dr["RegDate"].ToString();
                //item.LastLoginIP = dr[0x10].ToString();
                item.LastLoginDate =DateTime.Parse(dr["LastLoginDate"].ToString());
                //item.LoginTimes = dr.GetInt32(0x12);
                //item.SafeCode = dr[0x13].ToString();
                //item.FindDate = dr.GetDateTime(20);
                item.State = int.Parse(dr["State"].ToString());
                //item.OpenID = dr[0x16].ToString();
                userList.Add(item);
            }
        }


        //ע���Ա
        public int InsertUserInfor(MXUser Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into MXUser ( UserName,UserPwd,UserRealName,UserSex,UserEmail,UserTel,UserMobile,UserQQ,UserMsn,UserBirthday,UserCompany,UserPosition,UserPost,UserAddress,UserCompanyAddress,IsChecked,IsLocked,State ) values(@UserName,@UserPwd,@UserRealName,@UserSex,@UserEmail,@UserTel,@Usermobile,@UserQQ,@UserMsn,@UserBirthday,@UserCompany,@UserPosition,@UserPost,@UserAddress,@UserCompanyAddress,@IsChecked,@IsLocked,@State)");
            SqlParameter[] par = (SqlParameter[])this.ValueParas(Model);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        //��Ա����
        public SqlDataReader UserLogin(MXUser Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from MXUser where UserEmail=@UserEmail and UserPwd=@UserPwd");
            SqlParameter[] par ={
                                    new SqlParameter("@UserEmail",SqlDbType.VarChar,50),
                                    new SqlParameter("@UserPwd",SqlDbType.VarChar,50)
                                };
            par[0].Value = Model.UserEmail;
            par[1].Value = Common.DESEncrypt.Encrypt(Model.UserPwd);
            //
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        //��ȡ��Ա��Ϣ
        public SqlDataReader GetUserInfor(MXUser Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from MXUser where  UserId=@UserId");
            SqlParameter[] par ={
                                    new SqlParameter("@UserId",SqlDbType .Int,4)
                                };
            par[0].Value = Model.UserId;
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        //��ȡ��Ա��Ϣ
        public SqlDataReader GetUserInforByOrder(string Order)
        {
            if (Order == null)
            {
                Order = "Order By UserId Desc";
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from MXUser "+Order);
            return Common.DbHelperSQL.ExecuteReader(sql.ToString());
        }
        //��ȡ��Ա��Ϣ
        public SqlDataReader GetUserInforByOrder(string Field,string Value, string Order)
        {
            if (Order == null)
            {
                Order = "Order By UserId Desc";
            }
            string sql = "select * from [MXUser] where " + Field + "=@ValueString";
            SqlParameter[] par ={
                                    new SqlParameter("@ValueString",SqlDbType.VarChar,50)
                                };
            par[0].Value = Value;
            return Common.DbHelperSQL.ExecuteReader(sql, par);
        }

        //�ж��û������Ƿ����
        public bool IsVoidUserEmail(string UserEmail)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select MXUserId from MXUser where UserEmail=@UserEmail");
            SqlParameter[] par ={
                                    new SqlParameter("@UserEmail",SqlDbType.VarChar,50)
                                };
            par[0].Value = UserEmail;
            if ((int)Common.DbHelperSQL.GetSingle(sql.ToString(), par) <= 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// ����Field��֤�û��Ƿ����
        /// </summary>
        /// <param name="Field">�ֶ���</param>
        /// <param name="Value">�ֶ�ֵ</param>
        public int IsExistUserId(string Field , string Value)
        {
            string sql = "select count(*) from [MXUser] where " + Field + "=@ValueString";
            SqlParameter[] par ={
                                    new SqlParameter("@ValueString",SqlDbType.VarChar,50)
                                };
            par[0].Value = Value;
            return (int)Common.DbHelperSQL.GetSingle(sql, par);
        }        

        public SqlDataReader GetUserIdByUserEmail(MXUser Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select MXUserId from MXUser where UserEmail=@UserEmail");
            SqlParameter[] par ={
                                    new SqlParameter("@UserEmail",SqlDbType.VarChar,50)
                                };
            par[0].Value = Model.UserEmail;
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }



        public SqlDataReader GetUserInforByUserEmail(MXUser Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from MXUser where UserEmail=@UserEmail");
            SqlParameter[] par = {
                                     new SqlParameter("@UserEmail",SqlDbType .VarChar,50)
                                 };
            par[0].Value = Model.UserEmail;
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        //���»�Ա��Ϣ
        public int UpdateUserInfor(MXUser Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update MXUser set UserName=@UserName,UserPwd=@UserPwd,UserRealName=@UserRealName,UserSex=@UserSex,UserEmail=@UserEmail,UserTel=@UserTel,UserMobile=@UserMobile,UserQQ=@UserQQ,UserMsn=@UserMsn,UserBirthday=@UserBirthday,UserCompany=@UserCompany,UserPosition=@UserPosition,UserPost=@UserPost,UserAddress=@UserAddress,UserCompanyAddress=@UserCompanyAddress,IsChecked=@IsChecked,IsLocked=@IsLocked,,State=@State  where MXUserId=@UserId");
            SqlParameter[] par ={
                                    new SqlParameter ("@UserName",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserPwd",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserRealName",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserSex",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserEmail",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserTel",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserMobile",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserQQ",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserMsn",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserBirthday",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserCompany",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserPosition",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserPost",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserAddress",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserCompanyAddress",SqlDbType .VarChar,50),
                                    new SqlParameter ("@IsChecked",SqlDbType .VarChar,50),
                                    new SqlParameter ("@IsLocked",SqlDbType .VarChar,50),
                                    new SqlParameter ("@State",SqlDbType .VarChar,50)
                                };
            par[0].Value = Model.UserName;
            par[1].Value = Common.DESEncrypt.Encrypt(Model.UserPwd);
            par[2].Value = Model.UserRealName;
            par[3].Value = Model.UserSex;
            par[4].Value = Model.UserEmail;
            par[5].Value = Model.UserTel;
            par[6].Value = Model.UserMobile;
            par[7].Value = Model.UserQQ;
            par[8].Value = Model.UserMsn;
            par[9].Value = Model.UserBirthday;
            par[10].Value = Model.UserCompany;
            par[11].Value = Model.UserPosition;
            par[12].Value = Model.UserPost;
            par[13].Value = Model.UserAddress;
            par[14].Value = Model.UserCompanyAddress;
            par[15].Value = Model.IsChecked;
            par[16].Value = Model.IsLocked;
            par[17].Value = Model.State;         

            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        //�޸ĸ�����ϵ����
        public int UpdateUserContact(MXUser Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update MXUser set UserMsn=@UserMsn,UserQQ=@UserQQ,UserTel=@UserTel,UserMobile=@UserMobile where MXUserId=@UserId");
            SqlParameter[] par ={
                                    new SqlParameter("@UserMsn",SqlDbType.VarChar,50),
                                    new SqlParameter("@UserQQ",SqlDbType.VarChar,50),
                                    new SqlParameter("@UserTel",SqlDbType.VarChar,50),
                                    new SqlParameter("@UserMobile",SqlDbType.VarChar,50),
                                    new SqlParameter("@UserId",SqlDbType.VarChar,50)   
                                };
            par[0].Value = Model.UserMsn;
            par[1].Value = Model.UserQQ;
            par[2].Value = Model.UserTel;
            par[3].Value = Model.UserMobile;
            par[4].Value = Model.UserId;
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);

        }
        //�жϸ������Ƿ���ȷ
        public SqlDataReader CheckUserPwd(MXUser Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from MXUser  where UserPwd=@UserPwd and UserId=@UserId");
            SqlParameter[] par ={
                                    new SqlParameter("@UserPwd",SqlDbType.VarChar,50),
                                    new SqlParameter("@UserId",SqlDbType.Int,4)
                                };
            par[0].Value = Common.DESEncrypt.Encrypt(Model.UserPwd);
            par[1].Value = Model.UserId;
            return Common.DbHelperSQL.ExecuteReader(sql.ToString(), par);
        }

        /// <summary>
        /// ��֤�û���½�ź�����
        /// </summary>
        /// <param name="userId">�û���½��</param>
        /// <param name="passWord">�û�����</param>
        public bool CheckPass(string UserId, string PassWord)
        {
            string sql = "select password from [MXUser] where UserId='" + UserId + "'";
            string password = Common.DbHelperSQL.GetSingle(sql).ToString();
            if (password == PassWord)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //��Ա�ղؼ�
        /*
        public int seleid(Models.User.MXUser aa)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from collect where _userid=@id");
            SqlParameter[] par ={
                                    new SqlParameter("@id",SqlDbType.Int ,4)
                                };
            par[0].Value = aa.userid;
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }
         */
        //��ס�ϴε���ʱ��ʱ��
        public int UpdateLastLoginDate(MXUser Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update  MXUser set LastLoginDate=@LastLoginDate  where UserId=@UserId");
            SqlParameter[] par = {
                                     new SqlParameter("@UserId",SqlDbType.Int,4),
                                     new SqlParameter("@LastLoginDate",SqlDbType.DateTime,16)
                                 };
            par[0].Value = Model.UserId;
            par[1].Value = Model.LastLoginDate;
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        //��������
        public int UpdateUserPwd(MXUser Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update MXUser set UserPwd=@UserPwd  where UserId=@UserId ");
            SqlParameter[] par ={
                                    new SqlParameter("@UserId",SqlDbType.Int,4),
                                    new SqlParameter("@UserPwd",SqlDbType.VarChar,50)
                                };
            par[0].Value = Model.UserId;
            par[1].Value = Common.DESEncrypt.Encrypt(Model.UserPwd);
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }

        /// <summary>
        /// ����û�Ȩ����Ϣ 
        /// </summary>
        public SqlDataReader  GetUserRightInfoDataTable(string UserId, string ParentId)
        {
            string sql = "select * from viewUserRight where Userid='" + UserId + "' and parentnodeid='" + ParentId + "'";
            try
            {
                return Common.DbHelperSQL.ExecuteReader(sql);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// �����ݷ��ʶ��������ֵװ�ص����ݿ���²�������
        /// </summary>
        /// <remarks></remarks>
        protected IDbDataParameter[] ValueParas(MXUser Model)
        {
            SqlParameter[] par ={
                                    new SqlParameter ("@UserName",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserPwd",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserRealName",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserSex",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserEmail",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserTel",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserMobile",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserQQ",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserMsn",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserBirthday",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserCompany",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserPosition",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserPost",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserAddress",SqlDbType .VarChar,50),
                                    new SqlParameter ("@UserCompanyAddress",SqlDbType .VarChar,50),
                                    new SqlParameter ("@IsChecked",SqlDbType .VarChar,50),
                                    new SqlParameter ("@IsLocked",SqlDbType .VarChar,50),
                                    new SqlParameter ("@State",SqlDbType .VarChar,50)
                                };
            par[0].Value = Model.UserName;
            par[1].Value = Common.DESEncrypt.Encrypt(Model.UserPwd);
            par[2].Value = Model.UserRealName;
            par[3].Value = Model.UserSex;
            par[4].Value = Model.UserEmail;
            par[5].Value = Model.UserTel;
            par[6].Value = Model.UserMobile;
            par[7].Value = Model.UserQQ;
            par[8].Value = Model.UserMsn;
            par[9].Value = Model.UserBirthday;
            par[10].Value = Model.UserCompany;
            par[11].Value = Model.UserPosition;
            par[12].Value = Model.UserPost;
            par[13].Value = Model.UserAddress;
            par[14].Value = Model.UserCompanyAddress;
            par[15].Value = Model.IsChecked;
            par[16].Value = Model.IsLocked;
            par[17].Value = Model.State;
            return par;
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public MXUser GetModel(System.Data.DataRow row)
        {
            MXUser model = new MXUser();
            if (row != null)
            {
                model.UserId = int.Parse(row["UserId"].ToString());
                model.UserName = row["UserName"].ToString();
                model.UserPwd = row["UserPwd"].ToString();
                model.UserRealName = row["UserRealName"].ToString();
                model.UserSex = int.Parse(row["UserSex"].ToString());
                model.UserBirthday = row["UserBirthday"].ToString();
                model.UserEmail = row["UserEmail"].ToString();
                model.UserTel = row["UserTel"].ToString();
                model.UserMobile = row["UserMobile"].ToString();
                model.UserQQ = row["UserQQ"].ToString();
                model.UserMsn = row["UserMsn"].ToString();
                model.UserCompany = row["UserCompany"].ToString();
                model.UserPosition = row["UserPosition"].ToString();
                model.UserPost = row["UserPost"].ToString();
                model.UserAddress = row["UserAddress"].ToString();
                model.UserCompanyAddress = row["UserCompanyAddress"].ToString();
                model.IsChecked = int.Parse(row["IsChecked"].ToString());
                model.IsLocked = int.Parse(row["IsLocked"].ToString());
                model.RegDate = row["RegDate"].ToString();
                model.LastLoginDate = (DateTime)row["LastLoginDate"];

                return model;
            }
            else
            {
                return null;
            }
        }

    }
    //��ַ
    public class Address
    {
        public int InsertAddress(MXAddress Model)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into MXAddress (UserId,Address) values(@UserId,@Address)");
            SqlParameter[] par = {
                                     new SqlParameter("@UserId",SqlDbType.Int,4),
                                     new SqlParameter("@Address",SqlDbType.VarChar,150)
                                 };
            par[0].Value = Model.UserId;
            par[0].Value = Model.Address;
            return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
        }
    }
    //����
    //public class Order
    //{
    //    //�鿴����
    //    public DataSet GetUserIdOrderInfor(int a, int b, Models.Order.MXOrder Model)
    //    {
    //        StringBuilder sql = new StringBuilder();
    //        sql.Append("select * from MXOrder where UserId=@UserId  order by OrderDate ");
    //        SqlParameter[] par ={
    //                                new SqlParameter("@UserId",SqlDbType.Int,4)
    //                            };
    //        par[0].Value = Model.UserId;
    //        return Common.DbHelperSQL.PageQuery(sql.ToString(), a, b, par);
    //    }
    //    // ��������
    //    public int CountUserIdOrderInfor(Models.Order.MXOrder Model)
    //    {
    //        String sql = "select count(*) from MXOrder where UserId='" + Model.UserId + "'";
    //        int i = Convert.ToInt32(Common.DB.ExecuteScalar(sql));
    //        return i;
    //    }
    //    //�鿴ĳ������
    //    public DataSet GetOrderInfor(Models.Order.MXOrder Model)
    //    {
    //        StringBuilder sql = new StringBuilder();
    //        sql.Append("select * from MXOrder where OrderNumber=@OrderNumber");
    //        SqlParameter[] par = {
    //                                 new SqlParameter("@OrderNumber",SqlDbType.VarChar,50)
    //                             };
    //        par[0].Value = Model.OrderNumber;
    //        return Common.DbHelperSQL.Query(sql.ToString(), par);
    //    }
    //    public DataSet GetUserPayOrder(Models.Order.MXOrder Model)
    //    {
    //        StringBuilder sql = new StringBuilder();
    //        sql.Append("select * from MXOrder where UserId=@UserId and State=1 order by OrderDate ");
    //        SqlParameter[] par = {
    //                                 new SqlParameter("@UserId",SqlDbType.Int,4)
    //                             };
    //        par[0].Value = Model.UserId;
    //        return Common.DbHelperSQL.Query(sql.ToString(), par);
    //    }
    //    public int DeleteUserIdOrder(Models.Order.MXOrder Model)
    //    {
    //        StringBuilder sql = new StringBuilder();
    //        sql.Append("delete from MXOrder where UserId=@UserId and OrderId=@OrderId");
    //        SqlParameter[] par = {
    //                                 new  SqlParameter("@UserId",SqlDbType.Int,4),
    //                                 new  SqlParameter("@OrderId",SqlDbType.Int,4)
    //                             };
    //        par[0].Value = Model.UserId;
    //        par[1].Value = Model.OrderId;
    //        return Common.DbHelperSQL.ExecuteSql(sql.ToString(), par);
    //    }
        
        
    //}
}
