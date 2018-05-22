using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;

namespace XueFuShop.DAL
{
    public class DALUserManage
    {
        /// <summary>
        /// �Զ���-��ý�ɫ��Ϣ 
        /// </summary>
        public SqlDataReader GetAllUserInfoDataTable(string userRole)
        {
            string sql = "";
            if (userRole == "all")
            {
                sql = "select userid,username,password,case gender when 0 then '��' else 'Ů' end as gender,userrole,passquestion,passanswer,email,telno,address,IdCardNo from userinfo order by userid";
            }
            else
            {
                sql = "select userid,username,password,case gender when 0 then '��' else 'Ů' end as gender,userrole,passquestion,passanswer,email,telno,address,idcardno  from userinfo where userrole='" + userRole + "' order by userid";
            }
            try
            {
                return Common.DbHelperSQL.ExecuteReader(sql.ToString());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// ��ô�����û���Ϣ
        /// </summary>
        /// <param name="userState">�û�ͨ�����״̬</param>
        /// <returns></returns>
        public SqlDataReader GetRegisterUserInfoDataTable(string userState)
        {
            string sql = "select userid,username,password,case gender when 0 then '��' else 'Ů' end as gender,passquestion,passanswer,email,telno,address,idcardno  from userinfo where userstate='" + userState + "' order by userid";
            try
            {
                return Common.DbHelperSQL.ExecuteReader(sql.ToString());
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// �Զ���-��ý�ɫ��Ϣ 
        /// </summary>
        public SqlDataReader GetAllRoleInfoDataTable()
        {
            string sql = "select * from roleinfo order by roleid";
            try
            {
                return Common.DbHelperSQL.ExecuteReader(sql.ToString());
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// �Զ���-��ý�ɫȨ����Ϣ 
        /// </summary>
        public SqlDataReader GetRoleRightInfoDataTable()
        {
            string sql = "select nodeid,displayname from sysfun where parentnodeid<>'0' order by nodeid asc";
            try
            {
                return Common.DbHelperSQL.ExecuteReader(sql.ToString());
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// �Զ���-��ý�ɫȨ����Ϣ 
        /// </summary>
        public SqlDataReader GetRoleFunctionDataTable(string roleId)
        {
            string sql = "select b.nodeid,b.displayname from roleright a,sysfun b where a.nodeid=b.nodeid and   a.roleid = '" + roleId + "'";
            try
            {
                return Common.DbHelperSQL.ExecuteReader(sql.ToString());
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// �Զ���-����½�ɫ
        /// </summary>
        /// <param name="roleName">��ɫ����</param>
        /// <param name="roleDesc">��ɫ����</param>
        public bool AddRole(string roleName, string roleDesc)
        {
            string
                sql = "insert into roleinfo values('" + roleName + "','" + roleDesc + "',null)";
            int result = Common.DbHelperSQL.ExecuteSql(sql.ToString());
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }            
        }


        /// <summary>
        /// �Զ���-���ݽ�ɫid��ý�ɫ��Ϣ��
        /// </summary>
        public SqlDataReader GetRoleInfoByIdDataTable(string roleId)
        {
            string sql = "select * from roleinfo where roleId='" + roleId + "'";
            try
            {
                return Common.DbHelperSQL.ExecuteReader(sql.ToString());
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// �Զ���-�޸Ľ�ɫ
        /// </summary>
        /// <param name="roleId">��ɫid</param>
        /// <param name="roleName">��ɫ����</param>
        /// <param name="roleDesc">��ɫ����</param>
        /// <param name="Propotion">��Ա�Żݱ���</param>
        public bool AlterSelectRole(string roleId, string roleName, string roleDesc, string Discount)
        {
            string sql = "";
            if (Discount == "")
            {
                sql = "update roleinfo set roleName='" + roleName + "',roleDesc='" + roleDesc + "' where roleId='" + roleId + "'";
            }
            else
            {
                sql = "update roleinfo set discount='" + Discount + "' where roleId='" + roleId + "'";
            }
            int result = Common.DbHelperSQL.ExecuteSql(sql.ToString());
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }   
        }


        /// <summary>
        /// �Զ���-ɾ����ɫ
        /// </summary>
        ///  <param name="roleId">��ɫid</param>    
        public bool DeleteSelectRole(string roleId)
        {
            string sql = "delete from roleinfo where roleid='" + roleId + "'";
            int result = Common.DbHelperSQL.ExecuteSql(sql.ToString());
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }   
        }


        /// <summary>
        /// �Զ���-��ӽ�ɫȨ��
        /// </summary>
        /// <param name="roleId">��ɫid</param>
        /// <param name="nodeid">Ȩ��id</param>
        public bool CreateRoleRight(string roleId, string nodeId)
        {
            string parentSql = "select parentnodeid from sysfun where nodeid='" + nodeId + "'";
            object parentNodeId = Common.DbHelperSQL.GetSingle(parentSql); //����nodeId�ڵ�ĸ��ڵ�

            object parentNodeCount = Common.DbHelperSQL.GetSingle("select count(*) from roleright where roleid='" + roleId + "' and nodeid='" + parentNodeId + "'");//��ѯ���ڵ��Ƿ��Ѿ����ڣ�����Ѿ�������ִ�в��븸�ڵ������������븸�ڵ�

            if (parentNodeCount.ToString() == "0")
            {
                string insertParentNodeId = "insert into roleright values('" + roleId + "','" + parentNodeId + "')";
                Common.DbHelperSQL.ExecuteSql(insertParentNodeId);//���븸�ڵ�
            }

            string sql = "insert into roleright values('" + roleId + "','" + nodeId + "')";
            int result = Common.DbHelperSQL.ExecuteSql(sql.ToString());
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }   
        }

        /// <summary>
        /// �Զ���-ɾ����ɫȨ��
        /// </summary>
        /// <param name="roleId">��ɫid</param>
        /// <param name="nodeid">Ȩ��id</param>   
        public bool DelRoleRight(string roleId, string nodeId)
        {
            string parentSql = "select parentnodeid from sysfun where nodeid='" + nodeId + "'";
            object parentNodeId = Common.DbHelperSQL.GetSingle(parentSql); //����nodeId�ڵ�ĸ��ڵ�

            object ChildNodeIdCount = Common.DbHelperSQL.GetSingle("select count(*) from roleright a,sysfun b where a.nodeid=b.nodeid and a.roleid='" + roleId + "' and b.parentnodeid='" + parentNodeId.ToString() + "'");//��ѯ������nodeId��ͬ���ڵ�ĸ��������ֻ��nodeIdһ���ڵ��ˣ�����ͬ���׽ڵ�һ��ɾ��

            if (ChildNodeIdCount.ToString() == "1")
            {
                string deleteParentNodeId = "delete from roleright where roleid='" + roleId + "' and nodeid='" + parentNodeId + "'";
                Common.DbHelperSQL.ExecuteSql(deleteParentNodeId);//ɾ�����ڵ�
            }
            string sql = "delete from roleright where roleid='" + roleId + "' and nodeid='" + nodeId + "'";
            int result = Common.DbHelperSQL.ExecuteSql(sql.ToString());
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }   
        }


        /// <summary>
        /// �Զ���-������û�
        /// </summary>
        /// <param name="userId">�û���½id</param>
        /// <param name="userName">��ʵ����</param>
        /// <param name="passWord">����</param>
        /// <param name="userRole">�û���ɫ</param>
        /// <param name="gender">�Ա�</param>
        /// <param name="passQuestion">����</param>
        /// <param name="passAnswer">��</param>
        /// <param name="email">Email</param>
        /// <param name="telNo">��ϵ�绰</param>
        /// <param name="address">��ϸ��ַ</param>
        /// <param name="IdCardNo">���֤��</param>
        public bool AddUser(string userId, string userName, string passWord, string userRole, string gender, string passQuestion, string passAnswer, string email, string telNo, string address, string IdCardNo)
        {
            string sql = "insert into userinfo values('" + userId + "','" + userName + "','" + passWord + "','" + userRole + "','" + gender + "','" + passQuestion + "','" + passAnswer + "','" + email + "','" + telNo + "','" + address + "','" + IdCardNo + "','0','1')";
            int result = Common.DbHelperSQL.ExecuteSql(sql.ToString());
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }   
        }


        /// <summary>
        /// �Զ���-�����û�id����û���Ϣ 
        /// </summary>
        public SqlDataReader GetDataTableUserInfoById(string userId)
        {
            string sql = "select a.*,b.rolename,case a.gender when 0 then '��' else 'Ů' end as sex from userinfo a,roleinfo b where a.userrole=b.roleid and userid='" + userId + "'";
            try
            {
                return Common.DbHelperSQL.ExecuteReader(sql.ToString());
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// �Զ���-�޸��û�
        /// </summary>
        /// <param name="userId">�û���½id</param>
        /// <param name="userName">��ʵ����</param>
        /// <param name="passWord">����</param>
        /// <param name="userRole">�û���ɫ</param>
        /// <param name="gender">�Ա�</param>
        /// <param name="passQuestion">����</param>
        /// <param name="passAnswer">��</param>
        /// <param name="email">Email</param>
        /// <param name="telNo">��ϵ�绰</param>
        /// <param name="address">��ϸ��ַ</param>
        /// <param name="IdCardNo">���֤��</param>
        public bool AlterSelectUser(string userId, string userName, string passWord, string userRole, string gender, string passQuestion, string passAnswer, string email, string telNo, string address, string IdCardNo)
        {
            string sql = "update userinfo set userName='" + userName + "',password='" + passWord + "',userrole='" + userRole + "',gender='" + gender + "',passquestion='" + passQuestion + "',passanswer='" + passAnswer + "',email='" + email + "',telno='" + telNo + "',address='" + address + "',idcardno='" + IdCardNo + "'  where userid='" + userId + "'";

            int result = Common.DbHelperSQL.ExecuteSql(sql.ToString());
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }   
        }


        /// <summary>
        /// �Զ���-ɾ���û�
        /// </summary>
        /// <param name="userId">�û���½��</param>   
        public bool DeleteSelectUser(string userId)
        {
            string sql = "";
            if (userId == "admin")
            {
                return false;
            }
            else
            {
                sql = "delete from userinfo where userid='" + userId + "'";
                int result = Common.DbHelperSQL.ExecuteSql(sql.ToString());
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }   
            }

        }


        /// <summary>
        /// �Զ���-�޸��û�����
        /// </summary>
        /// <param name="userNum">�û���½��</param>
        /// <param name="passWord">�û�����</param>
        public bool UpdateUserPassWord(string userId, string passWord)
        {
            string sql = "update userinfo set password='" + passWord + "' where userid='" + userId + "'";
            int result = Common.DbHelperSQL.ExecuteSql(sql.ToString());
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }   
        }


        /// <summary>
        /// �Զ���-�޸��û����
        /// </summary>
        /// <param name="userNum">�û���½��</param>
        /// <param name="userRole">�û���ɫ</param>
        public bool UpdateUserRoleType(string userId, string userRole)
        {
            if (userId == "admin")
            {
                return false;
            }
            else
            {
                string sql = "update userinfo set userrole='" + userRole + "' where userid='" + userId + "'";
                int result = Common.DbHelperSQL.ExecuteSql(sql.ToString());
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }   
            }
        }


        /// <summary>
        /// �Զ���-��û����Ϣ�� 
        /// </summary>
        public SqlDataReader GetPostInfoDataTable(string type)
        {
            string sql = "";
            if (type == "all")
            {
                sql = "select a.posthistoryid,a.userid,a.bank,a.money,a.posttime,a.postdesc,case a.approvestate when 1 then 'δ׷��' when 2 then '�ѳ���' else '��' end as approvestate,b.username from posthistory a,userinfo b where a.userid=b.userid  order by a.posttime desc";
            }
            else
            {
                sql = "select a.posthistoryid,a.userid,a.bank,a.money,a.posttime,a.postdesc,case a.approvestate when 1 then 'δ׷��' when 2 then '�ѳ���' else '��' end as approvestate,b.username from posthistory a,userinfo b where a.userid=b.userid  and approvestate='" + type + "' order by a.posttime desc";
            }
            try
            {
                return Common.DbHelperSQL.ExecuteReader(sql.ToString());
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// �Զ���-�޸��û������Ϣ
        /// </summary>
        /// <param name="userId">�û���½��</param>
        /// <param name="money">�����</param>
        /// <param name="postHistoryId">���Ψһ��ʶ</param>
        public bool RelativeOperateWhenSurePostMoney(string userId, string money, string postHistoryId)
        {
            //string[] sqls = new string[2];
            List<String> sqls = new List<String>();           
            double postMoney = double.Parse(money);
            sqls.Add("update userinfo set money=money+ " + postMoney + "  where userid='" + userId + "'");
            sqls.Add("update posthistory set approvestate='3' where posthistoryid='" + postHistoryId + "'");
            int result = Common.DbHelperSQL.ExecuteSqlTran(sqls);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// �Զ���-ɾ���û�����¼
        /// </summary>
        ///  <param name="postHistoryId">���Ψһ��ʶid</param>
        public bool DeleteSelectedPostInfo(string postHistoryId)
        {
            string sql = "delete from posthistory where posthistoryid='" + postHistoryId + "'";
            int result = Common.DbHelperSQL.ExecuteSql(sql.ToString());
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// �Զ���-�˻��û�����¼
        /// </summary>
        ///  <param name="postHistoryId">���Ψһ��ʶid</param>
        public bool SelectedPostInfoIfFailed(string postHistoryId, string userId)
        {
            //string[] sqls = new string[2];
            List<String> sqls = new List<String>(); 
            sqls.Add("insert into sysuntreadpost values('" + userId + "','" + postHistoryId + "','0')");
            sqls.Add("update posthistory set approvestate='2' where posthistoryid='" + postHistoryId + "'");
            int result = Common.DbHelperSQL.ExecuteSqlTran(sqls);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
