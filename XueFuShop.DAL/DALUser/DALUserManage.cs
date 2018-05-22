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
        /// 自定义-获得角色信息 
        /// </summary>
        public SqlDataReader GetAllUserInfoDataTable(string userRole)
        {
            string sql = "";
            if (userRole == "all")
            {
                sql = "select userid,username,password,case gender when 0 then '男' else '女' end as gender,userrole,passquestion,passanswer,email,telno,address,IdCardNo from userinfo order by userid";
            }
            else
            {
                sql = "select userid,username,password,case gender when 0 then '男' else '女' end as gender,userrole,passquestion,passanswer,email,telno,address,idcardno  from userinfo where userrole='" + userRole + "' order by userid";
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
        /// 获得待审核用户信息
        /// </summary>
        /// <param name="userState">用户通过审核状态</param>
        /// <returns></returns>
        public SqlDataReader GetRegisterUserInfoDataTable(string userState)
        {
            string sql = "select userid,username,password,case gender when 0 then '男' else '女' end as gender,passquestion,passanswer,email,telno,address,idcardno  from userinfo where userstate='" + userState + "' order by userid";
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
        /// 自定义-获得角色信息 
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
        /// 自定义-获得角色权限信息 
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
        /// 自定义-获得角色权限信息 
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
        /// 自定义-添加新角色
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <param name="roleDesc">角色描述</param>
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
        /// 自定义-根据角色id获得角色信息表
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
        /// 自定义-修改角色
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="roleName">角色名称</param>
        /// <param name="roleDesc">角色描述</param>
        /// <param name="Propotion">会员优惠比例</param>
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
        /// 自定义-删除角色
        /// </summary>
        ///  <param name="roleId">角色id</param>    
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
        /// 自定义-添加角色权限
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="nodeid">权限id</param>
        public bool CreateRoleRight(string roleId, string nodeId)
        {
            string parentSql = "select parentnodeid from sysfun where nodeid='" + nodeId + "'";
            object parentNodeId = Common.DbHelperSQL.GetSingle(parentSql); //查找nodeId节点的父节点

            object parentNodeCount = Common.DbHelperSQL.GetSingle("select count(*) from roleright where roleid='" + roleId + "' and nodeid='" + parentNodeId + "'");//查询父节点是否已经存在，如果已经存在则不执行插入父节点操作，否则插入父节点

            if (parentNodeCount.ToString() == "0")
            {
                string insertParentNodeId = "insert into roleright values('" + roleId + "','" + parentNodeId + "')";
                Common.DbHelperSQL.ExecuteSql(insertParentNodeId);//插入父节点
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
        /// 自定义-删除角色权限
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="nodeid">权限id</param>   
        public bool DelRoleRight(string roleId, string nodeId)
        {
            string parentSql = "select parentnodeid from sysfun where nodeid='" + nodeId + "'";
            object parentNodeId = Common.DbHelperSQL.GetSingle(parentSql); //查找nodeId节点的父节点

            object ChildNodeIdCount = Common.DbHelperSQL.GetSingle("select count(*) from roleright a,sysfun b where a.nodeid=b.nodeid and a.roleid='" + roleId + "' and b.parentnodeid='" + parentNodeId.ToString() + "'");//查询所有与nodeId相同父节点的个数，如果只有nodeId一个节点了，则连同父亲节点一起删除

            if (ChildNodeIdCount.ToString() == "1")
            {
                string deleteParentNodeId = "delete from roleright where roleid='" + roleId + "' and nodeid='" + parentNodeId + "'";
                Common.DbHelperSQL.ExecuteSql(deleteParentNodeId);//删除父节点
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
        /// 自定义-添加新用户
        /// </summary>
        /// <param name="userId">用户登陆id</param>
        /// <param name="userName">真实姓名</param>
        /// <param name="passWord">密码</param>
        /// <param name="userRole">用户角色</param>
        /// <param name="gender">性别</param>
        /// <param name="passQuestion">问题</param>
        /// <param name="passAnswer">答案</param>
        /// <param name="email">Email</param>
        /// <param name="telNo">联系电话</param>
        /// <param name="address">详细地址</param>
        /// <param name="IdCardNo">身份证号</param>
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
        /// 自定义-根据用户id获得用户信息 
        /// </summary>
        public SqlDataReader GetDataTableUserInfoById(string userId)
        {
            string sql = "select a.*,b.rolename,case a.gender when 0 then '男' else '女' end as sex from userinfo a,roleinfo b where a.userrole=b.roleid and userid='" + userId + "'";
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
        /// 自定义-修改用户
        /// </summary>
        /// <param name="userId">用户登陆id</param>
        /// <param name="userName">真实姓名</param>
        /// <param name="passWord">密码</param>
        /// <param name="userRole">用户角色</param>
        /// <param name="gender">性别</param>
        /// <param name="passQuestion">问题</param>
        /// <param name="passAnswer">答案</param>
        /// <param name="email">Email</param>
        /// <param name="telNo">联系电话</param>
        /// <param name="address">详细地址</param>
        /// <param name="IdCardNo">身份证号</param>
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
        /// 自定义-删除用户
        /// </summary>
        /// <param name="userId">用户登陆号</param>   
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
        /// 自定义-修改用户密码
        /// </summary>
        /// <param name="userNum">用户登陆号</param>
        /// <param name="passWord">用户密码</param>
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
        /// 自定义-修改用户身份
        /// </summary>
        /// <param name="userNum">用户登陆号</param>
        /// <param name="userRole">用户角色</param>
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
        /// 自定义-获得汇款信息表 
        /// </summary>
        public SqlDataReader GetPostInfoDataTable(string type)
        {
            string sql = "";
            if (type == "all")
            {
                sql = "select a.posthistoryid,a.userid,a.bank,a.money,a.posttime,a.postdesc,case a.approvestate when 1 then '未追加' when 2 then '已撤回' else '是' end as approvestate,b.username from posthistory a,userinfo b where a.userid=b.userid  order by a.posttime desc";
            }
            else
            {
                sql = "select a.posthistoryid,a.userid,a.bank,a.money,a.posttime,a.postdesc,case a.approvestate when 1 then '未追加' when 2 then '已撤回' else '是' end as approvestate,b.username from posthistory a,userinfo b where a.userid=b.userid  and approvestate='" + type + "' order by a.posttime desc";
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
        /// 自定义-修改用户汇款信息
        /// </summary>
        /// <param name="userId">用户登陆号</param>
        /// <param name="money">汇款金额</param>
        /// <param name="postHistoryId">汇款唯一标识</param>
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
        /// 自定义-删除用户汇款记录
        /// </summary>
        ///  <param name="postHistoryId">汇款唯一标识id</param>
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
        /// 自定义-退回用户汇款记录
        /// </summary>
        ///  <param name="postHistoryId">汇款唯一标识id</param>
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
