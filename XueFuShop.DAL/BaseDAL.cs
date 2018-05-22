using System;
using System.Collections.Generic;
using System.Text;
using XueFuShop.IDAL;
using XueFuShop.Models;
using System.Data.SqlClient;
using System.Reflection;
using XueFu.EntLib;
using System.Data;
using System.Collections;
using System.ComponentModel;

namespace XueFuShop.DAL
{
    public abstract class BaseDAL<T> : IBaseDAL<T>
        where T : BaseModel
    {
        private Type type = null;
        private string tableName = string.Empty;

        public BaseDAL()
        {
            type = typeof(T);
            if (type.GetCustomAttributes(typeof(TableAttribute), false).Length > 0)
            {
                tableName = ((TableAttribute)(type.GetCustomAttributes(typeof(TableAttribute), false)[0])).TableName;
            }
        }

        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        //public bool Add<T>(T t) where T : BaseModel
        public bool Add(T t)
        {
            //Type type = typeof(T);

            List<string> sField = new List<string>();
            List<string> sValue = new List<string>();
            List<SqlParameter> paramList = new List<SqlParameter>();

            foreach (PropertyInfo item in type.GetProperties())
            {
                if (item.Name != "ID")
                {
                    sField.Add(string.Concat("[", item.Name, "]"));
                    sValue.Add(string.Concat("@", item.Name));
                    paramList.Add(new SqlParameter("@" + item.Name, item.GetValue(t, null) == null ? DBNull.Value : item.GetValue(t, null)));
                }
            }

            string sqlText = "insert into [{0}{1}]({2}) values({3})";
            sqlText = string.Format(sqlText, ShopMssqlHelper.TablePrefix, tableName, string.Join(",", sField.ToArray()), string.Join(",", sValue.ToArray()));

            return Convert.ToInt32(DbSQLHelper.ExecuteSql(sqlText, paramList.ToArray())) > 0;
        }

        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        //public bool Update<T>(T t) where T : BaseModel
        public bool Update(T t)
        {
            //Type type = typeof(T);
            string sqlText = string.Format("update [{0}{1}] set ", ShopMssqlHelper.TablePrefix, tableName);

            List<SqlParameter> paramList = new List<SqlParameter>();
            List<string> sSqlParameter = new List<string>();
            foreach (PropertyInfo item in type.GetProperties())
            {
                if (item.Name != "ID")
                {
                    sSqlParameter.Add(string.Concat("[", item.Name, "]=@", item.Name));
                }
                paramList.Add(new SqlParameter("@" + item.Name, item.GetValue(t, null) == null ? DBNull.Value : item.GetValue(t, null)));
            }
            sqlText = string.Concat(sqlText, string.Join(",", sSqlParameter.ToArray()), " where [ID]=@ID");

            return DbSQLHelper.ExecuteSql(sqlText.ToString(), paramList.ToArray()) > 0;
        }

        /// <summary>
        /// ɾ��ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        //public bool Delete<T>(int id) where T : BaseModel
        public bool Delete(int id)
        {
            Type type = typeof(T);

            string sqlText = string.Concat("delete from  [{0}{1}] where [ID]=@id", ShopMssqlHelper.TablePrefix, tableName);
            SqlParameter[] pt = {
                    new SqlParameter("@id", SqlDbType.Int,4)
            };
            pt[0].Value = id;

            return DbSQLHelper.ExecuteSql(sqlText.ToString(), pt) > 0;
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        //public T Read<T>(int id) where T : BaseModel
        public T Read(int id)
        {
            //Type type = typeof(T);

            string sqlText = string.Format("select {0} from [{1}{2}] where [ID]=@id", this.GetFieldString(type), ShopMssqlHelper.TablePrefix, tableName);

            SqlParameter[] pt = {
                    new SqlParameter("@id", SqlDbType.Int,4)
            };
            pt[0].Value = id;

            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sqlText, pt))
            {
                return this.PrepareModel(reader, type);
            }
        }

        /// <summary>
        /// ��ȡ����ʵ����б�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        //public List<T> ReadList<T>() where T : BaseModel
        public List<T> ReadList()
        {
            //Type type = typeof(T);

            string sqlText = string.Format("select {0} from [{1}{2}]", this.GetFieldString(type), ShopMssqlHelper.TablePrefix, tableName);

            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sqlText))
            {
                return this.PrepareList(reader);
            }
        }

        /// <summary>
        /// ��ȡ����ʵ����б�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        //public List<T> ReadList<T>() where T : BaseModel
        public List<T> ReadList<S>(S s)
        {
            //Type type = typeof(T);

            string sqlText = string.Format("select {0} from [{1}{2}]", this.GetFieldString(type), ShopMssqlHelper.TablePrefix, tableName);

            MssqlCondition mssqlCondition = new MssqlCondition();
            this.PrepareCondition(mssqlCondition, s);

            if (!string.IsNullOrEmpty(mssqlCondition.ToString()))
            {
                sqlText = string.Format("{0} where {1}", sqlText, mssqlCondition.ToString());
            }

            using (SqlDataReader reader = DbSQLHelper.ExecuteReader(sqlText))
            {
                return this.PrepareList(reader);
            }
        }

        /// <summary>
        /// ��ȡʵ����б�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">��ѯ������</param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        //public List<T> ReadList<T>(T t, int currentPage, int pageSize, ref int count) where T : BaseModel
        public List<T> ReadList<S>(S s, int currentPage, int pageSize, ref int count)
        {
            //Type type = typeof(T);

            ShopMssqlPagerClass class2 = new ShopMssqlPagerClass();
            class2.TableName = string.Concat("[", ShopMssqlHelper.TablePrefix, tableName, "]");
            class2.Fields = GetFieldString(type);
            class2.CurrentPage = currentPage;
            class2.PageSize = pageSize;
            class2.OrderField = "[ID]";
            class2.OrderType = OrderType.Desc;
            this.PrepareCondition(class2.MssqlCondition, s);
            class2.Count = count;
            count = class2.Count;
            using (SqlDataReader reader = class2.ExecuteReader())
            {
                return this.PrepareList(reader);
            }
        }

        /// <summary>
        /// ��ȡʵ���ֶ��ַ���
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetFieldString(Type type)
        {
            string sField = string.Empty;
            foreach (PropertyInfo item in type.GetProperties())
            {
                if (string.IsNullOrEmpty(sField))
                    sField = string.Format("[{0}]", item.Name);
                else
                    sField = string.Concat(sField, ",", string.Format("[{0}]", item.Name));
            }
            return sField;
        }

        /// <summary>
        /// װ��ʵ���б�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="list"></param>
        //public List<T> PrepareList<T>(SqlDataReader reader) where T : BaseModel
        public List<T> PrepareList(SqlDataReader reader)
        {
            //Type type = typeof(T);
            List<T> list = new List<T>();

            while (reader.Read())
            {
                list.Add(this.PrepareModel(reader, type));
            }

            return list;
        }

        /// <summary>
        /// װ��ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        //public T PrepareModel<T>(SqlDataReader reader, Type type) where T : BaseModel
        public T PrepareModel(SqlDataReader reader, Type type)
        {
            T model = Activator.CreateInstance<T>();
            //if (reader.Read())
            {
                foreach (PropertyInfo item in type.GetProperties())
                {
                    item.SetValue(model, reader[item.Name] == DBNull.Value ? null : reader[item.Name], null);
                }
            }
            return model;
        }

        /// <summary>
        /// װ�ز�ѯ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mssqlCondition"></param>
        /// <param name="t"></param>
        //public abstract void PrepareCondition<T>(MssqlCondition mssqlCondition, T t) where T : BaseModel;
        //public abstract void PrepareCondition<S>(MssqlCondition mssqlCondition, S s);
        private void PrepareCondition<S>(MssqlCondition mssqlCondition, S s)
        {
            Type tType = typeof(S);
            foreach (PropertyInfo item in tType.GetProperties())
            {
                object[] customAttributes = item.GetCustomAttributes(typeof(TableAttribute), false);
                if (customAttributes.Length > 0)
                {
                    TableAttribute attribute = (TableAttribute)customAttributes[0];
                    if (!string.IsNullOrEmpty(attribute.RelationTableName))
                    {
                        MssqlCondition relationCondition = new MssqlCondition();
                        ConditionFactory(item, attribute, relationCondition, s);
                        mssqlCondition.Add("[ID] in (Select [" + this.tableName + "ID] from [" + ShopMssqlHelper.TablePrefix + attribute.RelationTableName + "] Where " + relationCondition.ToString() + ")");
                    }
                    else
                    {
                        ConditionFactory(item, attribute, mssqlCondition, s);
                    }
                }
            }
        }


        private void ConditionFactory<S>(PropertyInfo item, TableAttribute attribute, MssqlCondition mssqlCondition, S s)
        {
            if (item.PropertyType == typeof(string))
            {
                mssqlCondition.Add("[" + attribute.FieldName + "]", (string)item.GetValue(s, null), attribute.ConditionType);
            }
            else if (item.PropertyType == typeof(int))
            {
                mssqlCondition.Add("[" + attribute.FieldName + "]", (int)item.GetValue(s, null), attribute.ConditionType);
            }
            else if (item.PropertyType == typeof(DateTime))
            {
                mssqlCondition.Add("[" + attribute.FieldName + "]", (DateTime)item.GetValue(s, null), attribute.ConditionType);
            }
            else if (item.PropertyType == typeof(decimal))
            {
                mssqlCondition.Add("[" + attribute.FieldName + "]", (decimal)item.GetValue(s, null), attribute.ConditionType);
            }
        }
    }
}
