using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using XueFu.EntLib;

namespace XueFuShop.Models
{
    public class TeacherSearchInfo : BaseModel
    {
        private string inID = string.Empty;
        private string inProductID = string.Empty;
        private string name = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [Table(ConditionType = ConditionType.In, FieldName = "ID")]
        public string InID
        {
            get { return this.inID; }
            set { this.inID = value; }
        }

        [Table(ConditionType = ConditionType.In, FieldName = "ProductID", RelationTableName = "ProductTeacher")]
        public string InProductID
        {
            get { return this.inProductID; }
            set { this.inProductID = value; }
        }

        /// <summary>
        /// ÐÕÃû
        /// </summary>
        [Table(ConditionType = ConditionType.In, FieldName = "Name")]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
    }
}
