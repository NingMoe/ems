using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MobileEMS.Models
{
    public sealed class QuestionJsonInfo
    {
        #region ÌâÄ¿Json´«Êä×Ö¶Î

        /// <summary>
        /// 
        /// </summary>
        private int _QuestionId = int.MinValue;
        public int QuestionId
        {
            set { _QuestionId = value; }
            get { return _QuestionId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _CateId = int.MinValue;
        public int CateId
        {
            set { _CateId = value; }
            get { return _CateId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Style = string.Empty;
        public string Style
        {
            set { _Style = value; }
            get { return _Style; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Question = string.Empty;
        public string Question
        {
            set { _Question = value; }
            get { return _Question; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _A;
        public string A
        {
            set { _A = value; }
            get { return _A; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _B;
        public string B
        {
            set { _B = value; }
            get { return _B; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _C;
        public string C
        {
            set { _C = value; }
            get { return _C; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _D;
        public string D
        {
            set { _D = value; }
            get { return _D; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _Answer = string.Empty;
        public string Answer
        {
            set { _Answer = value; }
            get { return _Answer; }
        }


        #endregion QuestionModel
    }
}
