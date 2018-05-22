using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace MobileEMS.Models
{
    public sealed class PostCateInfo
    {

        #region PostCateInfo

        /// <summary>
        /// 
        /// </summary>
        private string _EncryptFcateID;
        public string EncryptFcateID
        {
            set { _EncryptFcateID = value; }
            get { return _EncryptFcateID; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Title;
        public string Title
        {
            set { _Title = value; }
            get { return _Title; }
        }
        /// <summary>
        /// 
        /// </summary>
        private string _Icon = "icon-en";
        public string Icon
        {
            set { _Icon = value; }
            get { return _Icon; }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<Dictionary<string, string>> _ChildCourseFCateView = null;
        public List<Dictionary<string, string>> ChildCourseFCateView
        {
            set { _ChildCourseFCateView = value; }
            get { return _ChildCourseFCateView; }
        }

        #endregion PostCateInfo
    }
}
