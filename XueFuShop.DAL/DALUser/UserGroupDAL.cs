using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XueFu.EntLib;
using XueFuShop.Models;

namespace XueFuShop.DAL
{
    public sealed class UserGroupDAL
    {
        public int AddUserGroup(UserGroupInfo userGroup)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@name", SqlDbType.NVarChar), new SqlParameter("@power", SqlDbType.NText), new SqlParameter("@userCount", SqlDbType.Int), new SqlParameter("@addDate", SqlDbType.DateTime), new SqlParameter("@iP", SqlDbType.NVarChar), new SqlParameter("@note", SqlDbType.NText) };
            pt[0].Value = userGroup.Name;
            pt[1].Value = userGroup.Power;
            pt[2].Value = userGroup.UserCount;
            pt[3].Value = userGroup.AddDate;
            pt[4].Value = userGroup.IP;
            pt[5].Value = userGroup.Note;
            return Convert.ToInt32(ShopMssqlHelper.ExecuteScalar(ShopMssqlHelper.TablePrefix + "AddUserGroup", pt));
        }

        public void ChangeUserGroupCount(int id, ChangeAction action)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int), new SqlParameter("@action", SqlDbType.NVarChar) };
            pt[0].Value = id;
            pt[1].Value = action.ToString();
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "ChangeUserGroupCount", pt);
        }

        public void ChangeUserGroupCountByGeneral(string strID, ChangeAction action)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@strID", SqlDbType.NVarChar), new SqlParameter("@action", SqlDbType.NVarChar) };
            pt[0].Value = strID;
            pt[1].Value = action.ToString();
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "ChangeUserGroupCountByGeneral", pt);
        }

        public void DeleteUserGroup(string strID)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@strID", SqlDbType.NVarChar) };
            pt[0].Value = strID;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "DeleteUserGroup", pt);
        }

        public void PrepareUserGroupModel(SqlDataReader dr, List<UserGroupInfo> userGroupList)
        {
            while (dr.Read())
            {
                UserGroupInfo item = new UserGroupInfo();
                {
                    item.ID = dr.GetInt32(0);
                    item.Name = dr[1].ToString();
                    item.Power = dr[2].ToString();
                    item.UserCount = dr.GetInt32(3);
                    item.AddDate = dr.GetDateTime(4);
                    item.IP = dr[5].ToString();
                    item.Note = dr[6].ToString();
                }
                userGroupList.Add(item);
            }
        }

        public List<UserGroupInfo> ReadUserGroupAllList()
        {
            List<UserGroupInfo> userGroupList = new List<UserGroupInfo>();
            using (SqlDataReader reader = ShopMssqlHelper.ExecuteReader(ShopMssqlHelper.TablePrefix + "ReadUserGroupAllList"))
            {
                this.PrepareUserGroupModel(reader, userGroupList);
            }
            return userGroupList;
        }

        public void UpdateUserGroup(UserGroupInfo userGroup)
        {
            SqlParameter[] pt = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int), new SqlParameter("@name", SqlDbType.NVarChar), new SqlParameter("@power", SqlDbType.NText), new SqlParameter("@note", SqlDbType.NText) };
            pt[0].Value = userGroup.ID;
            pt[1].Value = userGroup.Name;
            pt[2].Value = userGroup.Power;
            pt[3].Value = userGroup.Note;
            ShopMssqlHelper.ExecuteNonQuery(ShopMssqlHelper.TablePrefix + "UpdateUserGroup", pt);
        }
    }
}
