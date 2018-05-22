using System;
using System.Collections.Generic;
using System.Text;
using XueFu.EntLib;

namespace XueFuShop.Models
{
    public class TableAttribute : Attribute
    {
        private string tableName;
        public string TableName
        {
            get { return this.tableName; }
            private set { this.tableName = value; }
        }

        private ConditionType conditionType;
        public ConditionType ConditionType
        {
            get { return this.conditionType; }
            set { this.conditionType = value; }
        }

        private string fieldName;
        public string FieldName
        {
            get { return this.fieldName; }
            set { this.fieldName = value; }
        }

        private string relationTableName;
        public string RelationTableName
        {
            get { return this.relationTableName; }
            set { this.relationTableName = value; }
        }

        public TableAttribute()
        {
        }

        public TableAttribute(string name)
        {
            this.tableName = name;
        }

    }
}
