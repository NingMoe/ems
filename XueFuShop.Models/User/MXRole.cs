using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MX_Store.Models.User
{
    /// <summary>
    /// 实体类MXRole。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public class MXRole
    {
        #region MXRoleModel

        /// <summary>
        /// 
        /// </summary>
        private int _RoleId;
        public int RoleId
        {
            set { _RoleId = value; }
            get { return _RoleId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _RoleName;
        public string RoleName
        {
            set { _RoleName = value; }
            get { return _RoleName; }
        }
        #endregion MXRoleModel
    }
}
