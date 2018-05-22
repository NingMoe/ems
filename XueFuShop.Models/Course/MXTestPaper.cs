using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace XueFuShop.Models.Course
{
    /// <summary>
    /// MXTestPaper 的摘要说明
    /// </summary>
    public class MXTestPaper
    {
        #region MXTestPaperModel

        /// <summary>
        /// 
        /// </summary>
        private int _TestPaperId;
        public int TestPaperId
        {
            set { _TestPaperId = value; }
            get { return _TestPaperId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _PaperName;
        public string PaperName
        {
            set { _PaperName = value; }
            get { return _PaperName; }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _CateId;
        public int CateId
        {
            set { _CateId = value; }
            get { return _CateId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _QuestionStyle;
        public string QuestionStyle
        {
            set { _QuestionStyle = value; }
            get { return _QuestionStyle; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _QuestionId;
        public string QuestionId
        {
            set { _QuestionId = value; }
            get { return _QuestionId; }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _Locked;
        public bool Locked
        {
            set { _Locked = value; }
            get { return _Locked; }
        }

        #endregion MXTestPaperModel
    }
}
