using System;

namespace XueFu.EntLib
{
    //条件类型Equal:=,More:>,Less:<,Like:like,In:,MoreOrEqual:,LessOrEqual:,NotIn:,NoEqual
    public enum ConditionType
    {
        Equal,
        More,
        Less,
        Like,
        In,
        MoreOrEqual,
        LessOrEqual,
        NotIn,
        NoEqual
    }

    //增加sql查询条件
    public class MssqlCondition
    {
        private string conditionString = string.Empty;

        public void Add(string condition)
        {
            if (condition != string.Empty)
            {
                if (condition.StartsWith("Group by"))
                {
                    if (this.conditionString == string.Empty)
                    {
                        this.conditionString = " 1=1 " + condition;
                    }
                    else
                    {
                        this.conditionString = this.conditionString + " " + condition;
                    }
                }
                else if (condition.StartsWith("Order by"))
                {
                    if (this.conditionString != string.Empty)
                    {
                        this.conditionString = this.conditionString + " " + condition;
                    }
                }
                else
                {
                    if (this.conditionString == string.Empty)
                    {
                        this.conditionString = condition;
                    }
                    else
                    {
                        this.conditionString = this.conditionString + " AND " + condition;
                    }
                }
            }
        }

        public void Add(string fieldName, DateTime value, ConditionType conditionType)
        {
            string str = value.ToString();
            if (value != DateTime.MinValue)
            {
                string str2 = string.Empty;
                switch (conditionType)
                {
                    case ConditionType.More:
                        str2 = ">'" + str + "'";
                        break;

                    case ConditionType.Less:
                        str2 = "<'" + str + "'";
                        break;

                    case ConditionType.MoreOrEqual:
                        str2 = ">='" + str + "'";
                        break;

                    case ConditionType.LessOrEqual:
                        str2 = "<='" + str + "'";
                        break;
                }
                if (this.conditionString == string.Empty)
                {
                    this.conditionString = fieldName + str2;
                }
                else
                {
                    this.conditionString = this.conditionString + " AND " + fieldName + str2;
                }
            }
        }

        public void Add(string fieldName, decimal value, ConditionType conditionType)
        {
            if (value != decimal.MinValue)
            {
                string str = string.Empty;
                switch (conditionType)
                {
                    case ConditionType.Equal:
                        str = "=" + value;
                        break;

                    case ConditionType.More:
                        str = ">" + value;
                        break;

                    case ConditionType.Less:
                        str = "<" + value;
                        break;

                    case ConditionType.MoreOrEqual:
                        str = ">=" + value;
                        break;

                    case ConditionType.LessOrEqual:
                        str = "<=" + value;
                        break;

                    case ConditionType.NoEqual:
                        str = "!=" + value;
                        break;
                }
                if (this.conditionString == string.Empty)
                {
                    this.conditionString = fieldName + str;
                }
                else
                {
                    this.conditionString = this.conditionString + " AND " + fieldName + str;
                }
            }
        }

        public void Add(string fieldName, int value, ConditionType conditionType)
        {
            if (value != -2147483648)
            {
                string str = string.Empty;
                switch (conditionType)
                {
                    case ConditionType.Equal:
                        str = "=" + value;
                        break;

                    case ConditionType.More:
                        str = ">" + value;
                        break;

                    case ConditionType.Less:
                        str = "<" + value;
                        break;

                    case ConditionType.MoreOrEqual:
                        str = ">=" + value;
                        break;

                    case ConditionType.LessOrEqual:
                        str = "<=" + value;
                        break;

                    case ConditionType.NoEqual:
                        str = "!=" + value;
                        break;
                }
                if (this.conditionString == string.Empty)
                {
                    this.conditionString = fieldName + str;
                }
                else
                {
                    this.conditionString = this.conditionString + " AND " + fieldName + str;
                }
            }
        }

        public void Add(string fieldName, string value, ConditionType conditionType)
        {
            if (value != string.Empty)
            {
                string str = string.Empty;
                switch (conditionType)
                {
                    case ConditionType.Equal:
                        str = "='" + StringHelper.SearchSafe(value) + "'";
                        break;

                    case ConditionType.Like:
                        str = " LIKE '%" + StringHelper.SearchSafe(value) + "%'";
                        break;

                    case ConditionType.In:
                        str = " IN (" + value + ")";
                        break;

                    case ConditionType.NotIn:
                        str = " NOT IN (" + StringHelper.SearchSafe(value) + ")";
                        break;
                }
                if (this.conditionString == string.Empty)
                {
                    this.conditionString = fieldName + str;
                }
                else
                {
                    this.conditionString = this.conditionString + " AND " + fieldName + str;
                }
            }
        }

        public string ToString()
        {
            return this.conditionString;
        }
    }
}
